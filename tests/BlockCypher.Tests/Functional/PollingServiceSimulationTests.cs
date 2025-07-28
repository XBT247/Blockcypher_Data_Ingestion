using BlockCypher.Domain.Entities;
using BlockCypher.Infrastructure.BlockCypherApi;
using BlockCypher.Infrastructure.BackgroundJobs;
using BlockCypher.Infrastructure.Repositories;
using BlockCypher.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.Extensions.Configuration;
using BlockCypher.Domain.Repositories;

namespace BlockCypher.Tests.Functional
{
    public class PollingServiceSimulationTests
    {
        [Fact]
        public async Task PollingService_FetchesAndStoresData()
        {
            // Arrange
            var dbOptions = new DbContextOptionsBuilder<BlockCypherDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new BlockCypherDbContext(dbOptions);

            var fakeApiClient = new Mock<IBlockCypherApiClient>();
            fakeApiClient.Setup(m => m.FetchMainAsync(It.IsAny<string>(), It.IsAny<string>(), default))
                .ReturnsAsync("{ \"height\": 456 }");

            var repo = new BlockchainRecordRepository(db);

            var services = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IBlockCypherApiClient>(fakeApiClient.Object)
                .AddSingleton<IBlockchainRecordRepository>(repo)
                .AddSingleton<IServiceProvider>(sp => sp)
                .BuildServiceProvider();

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["BLOCKCYPHER_POLL_INTERVAL_MINUTES"] = "0",
                    ["BLOCKCYPHER_TOKEN"] = "fake-token"
                }).Build();

            var logger = services.GetService<ILogger<BlockCypherPollingService>>() ?? Mock.Of<ILogger<BlockCypherPollingService>>();

            var pollingService = new BlockCypherPollingService(services, config, logger);

            // Act: Manually call ExecuteAsync once (don't actually loop)
            var cts = new CancellationTokenSource();
            var execTask = pollingService
                .GetType()
                .GetMethod("ExecuteAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .Invoke(pollingService, new object[] { cts.Token }) as Task;

            // Let it run a moment, then cancel to break the loop
            await Task.Delay(200);
            cts.Cancel();

            // Assert: Data was stored in DB
            Assert.True(db.BlockchainRecords.Any());
        }
    }
}
