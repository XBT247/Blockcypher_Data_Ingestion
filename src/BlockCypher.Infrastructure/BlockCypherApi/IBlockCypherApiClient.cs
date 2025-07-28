namespace BlockCypher.Infrastructure.BlockCypherApi
{
    public interface IBlockCypherApiClient
    {
        Task<string> FetchMainAsync(string coin, string token, CancellationToken cancellationToken = default);
    }
}
