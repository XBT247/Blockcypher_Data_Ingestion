using BlockCypher.Domain.Repositories;
using BlockCypher.Infrastructure.BlockCypherApi;
using BlockCypher.Infrastructure.BackgroundJobs;
using BlockCypher.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlockCypher.Infrastructure
{
    public static class MiddlewareDI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IBlockCypherApiClient, BlockCypherApiClient>();
            services.AddScoped<IBlockchainRecordRepository, BlockchainRecordRepository>();
            services.AddHostedService<BlockCypherPollingService>();
            return services;
        }
    }
}
