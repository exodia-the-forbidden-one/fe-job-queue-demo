using FastEndpoints;
using FastEndpoints.Swagger;
using job_queue_demo;
using job_queue_demo.Entities;
using job_queue_demo.Entities.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.
AddDbContext<JobsTestContext>(o=> o.UseNpgsql($"Host=localhost;Port=5432;Database=jobs_test;Username=postgres;Password=123456;"),ServiceLifetime.Transient).AddAuthorization()
                .AddFastEndpoints()
                .AddJobQueues<JobRecord, JobStorageProvider>()
                .SwaggerDocument();

var app = builder.Build();
app.UseAuthorization();
app.UseFastEndpoints();
app.UseJobQueues(o =>
{
    //general per queue limits
    o.MaxConcurrency = 1;
    o.ExecutionTimeLimit = TimeSpan.FromSeconds(10);

    //applicable only to MyCommand
    // o.LimitsFor<HelloWorldCommand>(
    //     maxConcurrency: 1,
    //     timeLimit: TimeSpan.FromSeconds(10));
});
app.UseSwaggerGen();

app.Run();
