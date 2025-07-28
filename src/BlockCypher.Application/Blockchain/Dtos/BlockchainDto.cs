using System.Text.Json.Serialization;

namespace BlockCypher.Application.Blockchain.Dtos
{
    public class BlockchainDto
    {
        public int Id { get; set; }
        public string Coin { get; set; } = default!;
        public BlockchainRecordDto blockchainRecord { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}