using FastEndpoints;
using job_queue_demo.Commands;
using job_queue_demo.Entities.Context;
using job_queue_demo.Entities;
using Microsoft.AspNetCore.Authorization;

namespace job_queue_demo.Features
{
    [HttpPost("adduser"), AllowAnonymous]
    public class AddUserEndpoint(JobsTestContext context) : EndpointWithoutRequest
    {
        public override async Task HandleAsync(CancellationToken ct)
        {
            var taskA = Parallel.ForEachAsync(
            Enumerable.Range(1, 1000),
            ct,
            async (i, c) => await new AddUserCommand()
            {
                User = new User
                {
                    Name = $"User {i}",
                }
            }.QueueJobAsync(ct: c));

            await Task.WhenAll(taskA);
            await SendAsync("queued!");
        }
    }
}