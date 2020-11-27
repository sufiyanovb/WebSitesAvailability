using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebSitesAvailability
{
    public class SitesStateUpdateService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly SitesStateUpdateProvider _provider;
        public SitesStateUpdateService(IServiceScopeFactory scopeFactory, IConfiguration configuration, SitesStateUpdateProvider provider)
        {
            _provider = provider;
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _provider.UpdateStates(_scopeFactory, stoppingToken);
                await Task.Delay(Convert.ToInt32(_configuration["RefreshTime"]), stoppingToken);
            }
        }
    }
}
