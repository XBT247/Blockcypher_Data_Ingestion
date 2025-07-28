using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace BlockCypher.Infrastructure.BlockCypherApi
{
    public class BlockCypherApiClient : IBlockCypherApiClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<BlockCypherApiClient> _logger;

        public BlockCypherApiClient(HttpClient client, ILogger<BlockCypherApiClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<string> FetchMainAsync(string coin, string token, CancellationToken cancellationToken = default)
        {
            string url = coin.ToLower() switch
            {
                "eth" => $"https://api.blockcypher.com/v1/eth/main?token={token}",
                "btc" => $"https://api.blockcypher.com/v1/btc/main?token={token}",
                "dash" => $"https://api.blockcypher.com/v1/dash/main?token={token}",
                "ltc" => $"https://api.blockcypher.com/v1/ltc/main?token={token}",
                _ => throw new ArgumentException("Unsupported coin.", nameof(coin))
            };

            try
            {
                var response = await _client.GetAsync(url, cancellationToken);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                return json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching data from {url}", url);
                throw;
            }
        }
    }
}
