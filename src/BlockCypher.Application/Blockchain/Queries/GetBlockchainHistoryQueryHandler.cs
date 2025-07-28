using AutoMapper;
using BlockCypher.Application.Blockchain.Dtos;
using BlockCypher.Domain.Repositories;
using MediatR;

namespace BlockCypher.Application.Blockchain.Queries
{
    public class GetBlockchainHistoryQueryHandler : IRequestHandler<GetBlockchainHistoryQuery, IEnumerable<BlockchainDto>>
    {
        private readonly IBlockchainRecordRepository _repo;
        private readonly IMapper _mapper;

        public GetBlockchainHistoryQueryHandler(IBlockchainRecordRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BlockchainDto>> Handle(GetBlockchainHistoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var records = await _repo.GetByCoinAsync(request.Coin, cancellationToken);
                var result = _mapper.Map<IEnumerable<BlockchainDto>>(records);
                return result;
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving blockchain history for coin '{request.Coin}'.", ex);
            }
        }
    }
}
