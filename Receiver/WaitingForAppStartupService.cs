namespace Receiver;

public class WaitingForAppStartupService
{
    private readonly IHostApplicationLifetime _lifetime;

    public WaitingForAppStartupService(IHostApplicationLifetime lifetime)
    {
        _lifetime = lifetime;
    }

    public async Task<bool> WaitForAppStartup(CancellationToken stoppingToken)
    {
        var startedSource = new TaskCompletionSource();
        await using var reg1 = _lifetime.ApplicationStarted.Register(() => startedSource.SetResult());

        var cancelledSource = new TaskCompletionSource();
        await using var reg2 = stoppingToken.Register(() => cancelledSource.SetResult());

        var completedTask = await Task.WhenAny(startedSource.Task, cancelledSource.Task);

        return completedTask == startedSource.Task;
    }
}