using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlockCypher.Persistence
{
    public static class MiddlewareDI
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                   ?? configuration["ConnectionStrings__DefaultConnection"]
                                   ?? "Data Source=blockcypher.db";
            services.AddDbContext<BlockCypherDbContext>(options =>
                options.UseSqlite(connectionString));
            return services;
        }
    }
}
