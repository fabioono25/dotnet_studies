using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicStore.Catalog.Application.DTOs;

namespace BasicStore.Catalog.Application.Services
{
    public interface IProductApplicationService: IDisposable
    {
        Task<IEnumerable<ProductDto>> GetByCategory(int code);
        Task<ProductDto> GetById(Guid id);
        Task<IEnumerable<ProductDto>> GetAll();
        Task<IEnumerable<CategoryDto>> GetAllCategories();

        Task AddProduct(ProductDto productDto);
        Task UpdateProduct(ProductDto productDto);

        Task<ProductDto> RemoveStock(Guid id, int quantity);
        Task<ProductDto> AddStock(Guid id, int quantity);
    }
}
