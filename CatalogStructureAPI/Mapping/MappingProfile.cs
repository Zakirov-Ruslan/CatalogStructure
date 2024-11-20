using AutoMapper;
using CatalogStructureAPI.ORM;
using CatalogStructureDto;

namespace CatalogStructureAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CatalogItem, CatalogItemDto>().ReverseMap();
            CreateMap<CatalogItem, CatalogItemUpdateDto>().ReverseMap();
            CreateMap<CatalogItem, CatalogItemCreateDto>().ReverseMap();
        }
    }
}
