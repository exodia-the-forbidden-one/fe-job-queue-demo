using FastEndpoints;
using job_queue_demo.Entities;
using job_queue_demo.Entities.Context;
using Microsoft.AspNetCore.Authorization;

namespace job_queue_demo.Features.TestEndpoint;

[HttpGet("/test"), AllowAnonymous]
public class TestEndpoint(JobsTestContext context) : EndpointWithoutRequest
{
    public override async Task HandleAsync(CancellationToken ct)
    {

        await context.Users.AddAsync(new User
        {
            Name = "Test"
        });

        await context.SaveChangesAsync();
        await SendAsync("WORKS!");
    }
}
