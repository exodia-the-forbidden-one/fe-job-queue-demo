using FastEndpoints;
using job_queue_demo.Entities;
using job_queue_demo.Entities.Context;
using Microsoft.EntityFrameworkCore;

namespace job_queue_demo.Commands;

public class AddBalanceCommand : ICommand
{
    public User User { get; set; }
}

sealed class AddBalanceCommandHandler(IServiceProvider _serviceProvider) : ICommandHandler<AddBalanceCommand>
{
    public async Task ExecuteAsync(AddBalanceCommand cmd, CancellationToken ct)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<JobsTestContext>();

            cmd.User.Balance += 100;
            context.Users.Update(cmd.User);
            await context.SaveChangesAsync(ct);
        }
        Console.WriteLine($"Balance addded to user: {cmd.User.Name}");
    }
}