using BlockCypher.Domain.Entities;
using BlockCypher.Infrastructure.Repositories;
using BlockCypher.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BlockCypher.UnitTests.Blockchain
{
    public class BlockchainRecordRepositoryTests
    {
        private BlockCypherDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<BlockCypherDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new BlockCypherDbContext(options);
        }

        [Fact]
        public async Task AddAndRetrieve_Record_Works()
        {
            // Arrange
            var db = GetDbContext();
            var repo = new BlockchainRecordRepository(db);

            var record = new BlockchainRecord
            {
                Coin = "btc",
                RawJson = "{ \"height\": 123, \"test\": true }",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            await repo.AddAsync(record);
            var results = await repo.GetByCoinAsync("btc");

            // Assert
            Assert.Single(results);
            Assert.Equal("btc", results.First().Coin);
            Assert.Contains("height", results.First().RawJson);
        }

        [Fact]
        public async Task GetLatestByAllCoinsAsync_ReturnsLatest()
        {
            // Arrange
            var db = GetDbContext();
            var repo = new BlockchainRecordRepository(db);

            await repo.AddAsync(new BlockchainRecord
            {
                Coin = "btc",
                RawJson = "{ \"height\": 1 }",
                CreatedAt = DateTime.UtcNow.AddMinutes(-5)
            });
            await repo.AddAsync(new BlockchainRecord
            {
                Coin = "btc",
                RawJson = "{ \"height\": 2 }",
                CreatedAt = DateTime.UtcNow
            });
            await repo.AddAsync(new BlockchainRecord
            {
                Coin = "eth",
                RawJson = "{ \"height\": 10 }",
                CreatedAt = DateTime.UtcNow
            });

            // Act
            var latest = (await repo.GetLatestByAllCoinsAsync()).ToList();

            // Assert
            Assert.Equal(2, latest.Count);
            Assert.Contains(latest, r => r.Coin == "btc" && r.RawJson.Contains("2"));
            Assert.Contains(latest, r => r.Coin == "eth");
        }
    }
}
