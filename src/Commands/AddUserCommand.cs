using FastEndpoints;
using job_queue_demo.Entities;
using job_queue_demo.Entities.Context;

namespace job_queue_demo.Commands;

public class AddUserCommand : ICommand
{
    public User User { get; set; }
}

sealed class AddUserCommandHandler(IServiceProvider _serviceProvider) : ICommandHandler<AddUserCommand>
{
    public async Task ExecuteAsync(AddUserCommand cmd, CancellationToken ct)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<JobsTestContext>();

            context.Users.Add(cmd.User);
            await context.SaveChangesAsync(ct);
        }
    }
}
