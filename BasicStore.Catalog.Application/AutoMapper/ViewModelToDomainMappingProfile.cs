using AutoMapper;
using BasicStore.Catalog.Domain;
using BasicStore.Catalog.Application.DTOs;

namespace BasicStore.Catalog.Application.AutoMapper
{
    // DTO to Domain
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<ProductDto, Product>()
                .ConstructUsing(p =>
                    new Product(p.Name, p.Description, p.Active,
                        p.Price, p.CategoryId, p.CreationDate,
                        p.Image, new Dimensions(p.Height, p.Width, p.Deep)));

            CreateMap<CategoryDto, Category>()
                .ConstructUsing(c => new Category(c.Name, c.Code));
        }
    }
}
