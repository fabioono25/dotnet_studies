using BasicStore.Catalog.Domain.Events;
using BasicStore.Core.Communication.Mediator;
using BasicStore.Core.DomainObjects.DTO;
using BasicStore.Core.Messages.Common.Notifications;

namespace BasicStore.Catalog.Domain
{
    // representa acoes ligadas a linguaguem ubiqua
    public class StockService : IStockService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediatorHandler _mediatorHandler;

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
                // await _mediatorHandler.PublishEvent(new ProductBellowStockEvent(product.Id, product.QuantityStock));
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

        public async Task<bool> DebitarEstoque(Guid produtoId, int quantidade)
        {
            if (!await DebitarItemEstoque(produtoId, quantidade)) return false;

            return await _productRepository.UnitOfWork.Commit();
        }

        public async Task<bool> DebitarListaProdutosPedido(ListaProdutosPedido lista)
        {
            foreach (var item in lista.Itens)
            {
                if (!await DebitarItemEstoque(item.Id, item.Quantidade)) return false;
            }

            return await _productRepository.UnitOfWork.Commit();
        }

        public async Task<bool> ReporEstoque(Guid produtoId, int quantidade)
        {
            var sucesso = await ReporItemEstoque(produtoId, quantidade);

            if (!sucesso) return false;

            return await _productRepository.UnitOfWork.Commit();
        }

        public Task<bool> ReporListaProdutosPedido(ListaProdutosPedido lista)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> ReporItemEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _productRepository.GetById(produtoId);

            if (produto == null) return false;
            produto.RestoreStock(quantidade);

            _productRepository.Update(produto);

            return true;
        }

        private async Task<bool> DebitarItemEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _productRepository.GetById(produtoId);

            if (produto == null) return false;

            if (!produto.HasStock(quantidade))
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("Estoque", $"Produto - {produto.Name} sem estoque"));
                return false;
            }

            produto.DecreaseStock(quantidade);

            // TODO: 10 pode ser parametrizavel em arquivo de configuração
            if (produto.QuantityStock < 10)
            {
                await _mediatorHandler.PublishEvent(new ProductBellowStockEvent(produto.Id, produto.QuantityStock));
            }

            _productRepository.Update(produto);
            return true;
        }

        public void Dispose()
        {
            _productRepository.Dispose();
        }
    }
}
