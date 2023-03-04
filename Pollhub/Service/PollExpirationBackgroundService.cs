using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pollhub.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pollhub.Service
{
    public class PollExpirationBackgroundService : BackgroundService
    {
        private readonly ILogger<PollExpirationBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public PollExpirationBackgroundService(ILogger<PollExpirationBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<AppDbContext>();

                    // Check for expired polls and update their status
                    var expiredPolls = await context.Polls
                        .Where(p => p.Status == "Open" && p.ExpirationTime <= DateTime.UtcNow)
                        .ToListAsync();

                    foreach (var poll in expiredPolls)
                    {
                        poll.Status = "Closed";
                    }

                    await context.SaveChangesAsync();

                    _logger.LogInformation($"Checked {expiredPolls.Count} polls for expiration.");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Wait for one minute before checking again
            }
        }
    }
}
