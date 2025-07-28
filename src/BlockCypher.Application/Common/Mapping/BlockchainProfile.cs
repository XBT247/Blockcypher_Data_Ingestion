using AutoMapper;
using BlockCypher.Domain.Entities;
using BlockCypher.Application.Blockchain.Dtos;
using System.Text.Json;

namespace BlockCypher.Application.Common.Mapping
{
    public class BlockchainProfile : Profile
    {
        public BlockchainProfile()
        {
            //CreateMap<BlockchainRecord, BlockchainDto>();
            CreateMap<BlockchainRecord, BlockchainDto>()
                .ForMember(dest => dest.blockchainRecord,
                    opt => opt.MapFrom<RawJsonToDtoResolver>()
                );
        }
    }

    public class RawJsonToDtoResolver : IValueResolver<BlockchainRecord, BlockchainDto, BlockchainRecordDto>
    {
        public BlockchainRecordDto Resolve(BlockchainRecord source, BlockchainDto destination, BlockchainRecordDto destMember, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(source.RawJson))
                return default!;  // Use null-forgiving operator because the mapping expects a non-null return

            return JsonSerializer.Deserialize<BlockchainRecordDto>(source.RawJson) ?? new BlockchainRecordDto();
        }
    }
}
