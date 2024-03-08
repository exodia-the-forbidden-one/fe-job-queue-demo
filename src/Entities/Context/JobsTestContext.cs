using Microsoft.EntityFrameworkCore;

namespace job_queue_demo.Entities.Context;

public class JobsTestContext : DbContext
{
    public JobsTestContext(DbContextOptions<JobsTestContext> options) : base(options)
    {
    }

    public DbSet<JobRecord> JobRecords { get; set; }
    public DbSet<User> Users { get; set; }

}
