using System.Net.Http.Json;
using BlockCypher.Domain.Entities;
using BlockCypher.Persistence;
using Microsoft.Extensions.DependencyInjection;
using BlockCypher.Api; // for Program
using BlockCypher.Tests.TestHelpers;
using Microsoft.EntityFrameworkCore;

namespace BlockCypher.Tests.Api
{
    public class BlockchainApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public BlockchainApiTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Latest_Blockchains_ReturnsSuccess()
        {
            var response = await _client.GetAsync("/api/blockchains/latest");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetHistory_ReturnsEmptyInitially()
        {
            var response = await _client.GetAsync("/api/blockchain/btc");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<BlockchainRecord>>();
            Assert.NotNull(result);
            Assert.Empty(result!);
        }

        [Fact]
        public async Task AddData_And_GetHistory_ReturnsRecords()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BlockCypherDbContext>();

            var record = new BlockchainRecord
            {
                Coin = "btc",
                RawJson = "{ \"height\": 999 }",
                CreatedAt = DateTime.UtcNow
            };
            db.BlockchainRecords.Add(record);
            db.SaveChanges();
            var response = await _client.GetAsync("/api/blockchain/btc");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<BlockchainRecord>>();

            Assert.NotNull(result);
            Assert.Single(result!);
            Assert.Equal("btc", result!.First().Coin);
            Assert.Contains("height", result.First().RawJson);
        }
    }
}
