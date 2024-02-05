using AutoMapper;
using BasicStore.Catalog.Application.DTOs;
using BasicStore.Catalog.Domain;

namespace BasicStore.Catalog.Application.AutoMapper
{
    // Domain to DTO
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.Width, o => o.MapFrom(s => s.Dimensions.Width))
                .ForMember(d => d.Height, o => o.MapFrom(s => s.Dimensions.Height))
                .ForMember(d => d.Deep, o => o.MapFrom(s => s.Dimensions.Deep));

            CreateMap<Category, CategoryDto>();
        }
    }
}
