using BlockCypher.Application.Blockchain.Dtos;
using MediatR;

namespace BlockCypher.Application.Blockchain.Queries
{
    public class GetAllLatestBlockchainsQuery : IRequest<IEnumerable<BlockchainRecordDto>> { }
}
