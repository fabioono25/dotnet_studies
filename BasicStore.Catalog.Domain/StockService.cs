using BasicStore.Catalog.Domain.Events;
using BasicStore.Core.Communication.Mediator;

namespace BasicStore.Catalog.Domain
{
    // representa acoes ligadas a linguaguem ubiqua
    public class StockService : IStockService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediatorHandler _bus;

        public StockService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> RemoveFromStock(Guid productId, int quantity)
        {
            var product = await _productRepository.GetById(productId);

            if (product == null) return false; // pode lancar exception aqui

            if (!product.HasStock(quantity)) return false;

            product.DecreaseStock(quantity);

            // TODO: Parametrizar a quantidade de estoque baixo
            if (product.QuantityStock < 10)
            {
                await _bus.PublishEvent(new ProductBellowStockEvent(product.Id, product.QuantityStock));
            }

            _productRepository.Update(product);
            return await _productRepository.UnitOfWork.Commit();
        }

        public async Task<bool> AddToStock(Guid productId, int quantity)
        {
            var produto = await _productRepository.GetById(productId);

            if (produto == null) return false;
            produto.RestoreStock(quantity);

            _productRepository.Update(produto);
            return await _productRepository.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            _productRepository.Dispose();
        }
    }
}
