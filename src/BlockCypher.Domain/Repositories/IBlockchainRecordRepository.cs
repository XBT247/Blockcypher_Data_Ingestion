using BlockCypher.Domain.Entities;

namespace BlockCypher.Domain.Repositories
{
    public interface IBlockchainRecordRepository
    {
        Task AddAsync(BlockchainRecord record, CancellationToken cancellationToken = default);
        Task<IEnumerable<BlockchainRecord>> GetByCoinAsync(string coin, CancellationToken cancellationToken = default);
        Task<IEnumerable<BlockchainRecord>> GetLatestByAllCoinsAsync(CancellationToken cancellationToken = default);
    }
}
