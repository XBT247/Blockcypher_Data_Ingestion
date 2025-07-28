using BlockCypher.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlockCypher.Persistence
{
    public class BlockCypherDbContext : DbContext
    {
        public DbSet<BlockchainRecord> BlockchainRecords => Set<BlockchainRecord>();

        public BlockCypherDbContext(DbContextOptions<BlockCypherDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlockchainRecord>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Coin).IsRequired();
                entity.Property(x => x.RawJson).IsRequired();
                entity.Property(x => x.CreatedAt).IsRequired();
            });
        }
    }
}
