using AutoMapper;
using BlockCypher.Application.Blockchain.Dtos;
using BlockCypher.Domain.Repositories;
using MediatR;

namespace BlockCypher.Application.Blockchain.Queries
{
    public class GetAllLatestBlockchainsQueryHandler : IRequestHandler<GetAllLatestBlockchainsQuery, IEnumerable<BlockchainRecordDto>>
    {
        private readonly IBlockchainRecordRepository _repo;
        private readonly IMapper _mapper;

        public GetAllLatestBlockchainsQueryHandler(IBlockchainRecordRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BlockchainRecordDto>> Handle(GetAllLatestBlockchainsQuery request, CancellationToken cancellationToken)
        {
            var records = await _repo.GetLatestByAllCoinsAsync(cancellationToken);
            return _mapper.Map<IEnumerable<BlockchainRecordDto>>(records);
        }
    }
}
