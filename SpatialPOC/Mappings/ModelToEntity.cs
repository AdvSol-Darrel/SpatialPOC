using AutoMapper;
using SpatialPOC.Dto;
using SpatialPOC.Models;

namespace SpatialPOC.Mappings
{
    public class ModelToEntity : Profile
    {
        public ModelToEntity()
        {
            SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            DestinationMemberNamingConvention = new PascalCaseNamingConvention();

            CreateMap<LineDto, ZogGeometry>();
        }
    }
}
