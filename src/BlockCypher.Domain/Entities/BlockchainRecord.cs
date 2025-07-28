namespace BlockCypher.Domain.Entities
{
    public class BlockchainRecord
    {
        public int Id { get; set; }
        public string Coin { get; set; } = default!;
        public string RawJson { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
