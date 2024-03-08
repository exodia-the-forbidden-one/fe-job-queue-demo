using FastEndpoints;
using job_queue_demo.Commands;
using job_queue_demo.Entities.Context;
using Microsoft.AspNetCore.Authorization;


[HttpGet("/updateallbalances"), AllowAnonymous]
public class UpdateAllBalancesEndpoint(JobsTestContext context) : EndpointWithoutRequest
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        var updateTask = Parallel.ForEachAsync(
            context.Users.ToList(),
            ct,
            async (i, c) => await new AddBalanceCommand()
            {
                User = i
            }.QueueJobAsync(ct: c));

        await Task.WhenAll(updateTask);
        await SendAsync("queued!");
    }
}