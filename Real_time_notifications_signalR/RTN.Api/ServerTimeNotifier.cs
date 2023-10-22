using Microsoft.AspNetCore.SignalR;

namespace RTN.Api;

public class ServerTimeNotifier : BackgroundService
{
    private static readonly TimeSpan Period = TimeSpan.FromSeconds(5);
    
    private readonly ILogger<ServerTimeNotifier> _logger;
    private readonly IHubContext<NotificationsHub, INotificationClient> _context;
        

    public ServerTimeNotifier(ILogger<ServerTimeNotifier> logger, IHubContext<NotificationsHub, INotificationClient> context)
    {
        _logger = logger;
        _context = context;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Period);

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            var dateTime = DateTime.Now;
            
            _logger.LogInformation("Sending server time {DateTime}, service {Service}", dateTime, nameof(ServerTimeNotifier));
            await _context.Clients.All.ReceiveNotification($"Server time = {dateTime}");
        }
    }
}