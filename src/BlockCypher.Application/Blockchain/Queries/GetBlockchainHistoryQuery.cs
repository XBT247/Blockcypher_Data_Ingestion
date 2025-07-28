using BlockCypher.Application.Blockchain.Dtos;
using MediatR;

namespace BlockCypher.Application.Blockchain.Queries
{
    public class GetBlockchainHistoryQuery : IRequest<IEnumerable<BlockchainDto>>
    {
        public string Coin { get; set; } = default!;
    }
}
