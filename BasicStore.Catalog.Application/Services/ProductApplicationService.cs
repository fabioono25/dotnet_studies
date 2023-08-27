using AutoMapper;
using BasicStore.Catalog.Application.DTOs;
using BasicStore.Catalog.Domain;
using BasicStore.Core.DomainObjects;

namespace BasicStore.Catalog.Application.Services
{
    public class ProductApplicationService: IProductApplicationService
    {
        private readonly IProductRepository _productRepository;
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;

        public ProductApplicationService(IProductRepository productRepository, IMapper mapper, IStockService stockService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _stockService = stockService;
        }


        public void Dispose()
        {
            _productRepository?.Dispose();
            _stockService?.Dispose();
        }

        public async Task<IEnumerable<ProductDto>> GetByCategory(int code)
        {
            return _mapper.Map<IEnumerable<ProductDto>>(await _productRepository.GetByCategory(code));
        }

        public async Task<ProductDto> GetById(Guid id)
        {
            return _mapper.Map<ProductDto>(await _productRepository.GetById(id));
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            return _mapper.Map<IEnumerable<ProductDto>>(await _productRepository.GetAll());
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            return _mapper.Map<IEnumerable<CategoryDto>>(await _productRepository.GetCategories());
        }

        public async Task AddProduct(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            _productRepository.Add(product);

            await _productRepository.UnitOfWork.Commit();
        }

        public async Task UpdateProduct(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            _productRepository.Update(product);

            await _productRepository.UnitOfWork.Commit();
        }

        public async Task<ProductDto> RemoveStock(Guid id, int quantity)
        {
            if (!_stockService.RemoveFromStock(id, quantity).Result)
            {
                throw new DomainException("Falha ao debitar estoque");
            }

            return _mapper.Map<ProductDto>(await _productRepository.GetById(id));
        }

        public async Task<ProductDto> AddStock(Guid id, int quantity)
        {
            if (!_stockService.AddToStock(id, quantity).Result)
            {
                throw new DomainException("Falha ao repor estoque");
            }

            return _mapper.Map<ProductDto>(await _productRepository.GetById(id));
        }
    }
}
