using AutoMapper;
using SpatialPOC.Models;
using SpatialPOC.Dto;

namespace SpatialPOC.Mappings
{
    public class EntityToModel : Profile
    {
        public EntityToModel()
        {
            SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            DestinationMemberNamingConvention = new PascalCaseNamingConvention();

            CreateMap<ZogLine, LineDto>();
        }
    }
}
