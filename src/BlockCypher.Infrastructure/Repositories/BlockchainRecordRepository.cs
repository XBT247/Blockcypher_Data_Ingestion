using BlockCypher.Domain.Entities;
using BlockCypher.Domain.Repositories;
using BlockCypher.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BlockCypher.Infrastructure.Repositories
{
    public class BlockchainRecordRepository : IBlockchainRecordRepository
    {
        private readonly BlockCypherDbContext _db;

        public BlockchainRecordRepository(BlockCypherDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(BlockchainRecord record, CancellationToken cancellationToken = default)
        {
            await _db.BlockchainRecords.AddAsync(record, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<BlockchainRecord>> GetByCoinAsync(string coin, CancellationToken cancellationToken = default)
        {
            return await _db.BlockchainRecords
                .Where(x => x.Coin == coin.ToLower())
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BlockchainRecord>> GetLatestByAllCoinsAsync(CancellationToken cancellationToken = default)
        {
            // Get the latest record for each coin
            return await _db.BlockchainRecords
                .GroupBy(x => x.Coin)
                .Select(g => g.OrderByDescending(x => x.CreatedAt).First())
                .ToListAsync(cancellationToken);
        }
    }
}
