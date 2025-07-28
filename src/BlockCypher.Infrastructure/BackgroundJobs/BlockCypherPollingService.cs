using BlockCypher.Domain.Entities;
using BlockCypher.Domain.Repositories;
using BlockCypher.Infrastructure.BlockCypherApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;


namespace BlockCypher.Infrastructure.BackgroundJobs
{
    public class BlockCypherPollingService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ILogger<BlockCypherPollingService> _logger;

        private readonly string[] _coins = new[] { "btc", "eth", "dash", "ltc" };

        public BlockCypherPollingService(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            ILogger<BlockCypherPollingService> logger)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var pollIntervalMin = int.TryParse(
                _configuration["BLOCKCYPHER_POLL_INTERVAL_MINUTES"],
                out var result) ? result : 5;
            var token = _configuration["BLOCKCYPHER_TOKEN"];

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("BLOCKCYPHER_TOKEN is not set. Polling will not run.");
                return;
            }

            _logger.LogInformation("BlockCypher Polling Service started (Interval: {Interval} minutes)", pollIntervalMin);

            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var apiClient = scope.ServiceProvider.GetRequiredService<IBlockCypherApiClient>();
                var repo = scope.ServiceProvider.GetRequiredService<IBlockchainRecordRepository>();

                foreach (var coin in _coins)
                {
                    try
                    {
                        var json = await apiClient.FetchMainAsync(coin, token, stoppingToken);
                        var record = new BlockchainRecord
                        {
                            Coin = coin,
                            RawJson = json,
                            CreatedAt = DateTime.UtcNow
                        };
                        await repo.AddAsync(record, stoppingToken);
                        _logger.LogInformation("Stored {coin} data at {ts}", coin, record.CreatedAt);
                    }
                    catch (Exception ex) 
                    {
                        _logger.LogError(ex, "Failed to fetch/store {coin}", coin);
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(pollIntervalMin), stoppingToken);
            }
        }
    }
}
