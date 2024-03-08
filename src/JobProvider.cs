using FastEndpoints;
using job_queue_demo.Entities;
using job_queue_demo.Entities.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace job_queue_demo;

sealed class JobStorageProvider : IJobStorageProvider<JobRecord>
{
    readonly PooledDbContextFactory<JobsTestContext> _dbPool;

    public JobStorageProvider()
    {
        var opts = new DbContextOptionsBuilder<JobsTestContext>()
                   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                   .UseNpgsql($"Host=localhost;Port=5432;Database=jobs_test;Username=postgres;Password=123456;").Options;
        _dbPool = new(opts);
        using var db = _dbPool.CreateDbContext();
        db.Database.EnsureCreated(); //use migrations instead of this.
    }

    public async Task StoreJobAsync(JobRecord job, CancellationToken ct)
    {
        using var db = _dbPool.CreateDbContext();
        await db.AddAsync(job, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<JobRecord>> GetNextBatchAsync(PendingJobSearchParams<JobRecord> p)
    {
        using var db = _dbPool.CreateDbContext();

        return await db.JobRecords
                       .Where(p.Match)
                       .Take(p.Limit)
                       .ToListAsync(p.CancellationToken);
    }

    public async Task MarkJobAsCompleteAsync(JobRecord job, CancellationToken c)
    {
        using var db = _dbPool.CreateDbContext();
        db.Update(job);
        await db.SaveChangesAsync(c);
    }

    public async Task OnHandlerExecutionFailureAsync(JobRecord job, Exception e, CancellationToken c)
    {
        using var db = _dbPool.CreateDbContext();
        job.ExecuteAfter = DateTime.UtcNow.AddMinutes(1);
        db.Update(job);
        await db.SaveChangesAsync(c);
    }

    public async Task PurgeStaleJobsAsync(StaleJobSearchParams<JobRecord> p)
    {
        using var db = _dbPool.CreateDbContext();
        var staleJobs = db.JobRecords.Where(p.Match);
        db.RemoveRange(staleJobs);
        await db.SaveChangesAsync(p.CancellationToken);
    }
}
