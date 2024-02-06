using BasicStore.Core.Communication.Mediator;
using BasicStore.Core.Messages.Common.IntegrationEvents;
using icStore.Core.Messages.Common.IntegrationEvents;
using MediatR;

namespace BasicStore.Catalog.Domain.Events
{
    public class ProductEventHandler : INotificationHandler<ProductBellowStockEvent>,
        INotificationHandler<PedidoIniciadoEvent>, INotificationHandler<PedidoProcessamentoCanceladoEvent>
    {
        private readonly IProductRepository _ProductRepository;
        private readonly IStockService _estoqueService;
        private readonly IMediatorHandler _mediatorHandler;

        public ProductEventHandler(IProductRepository ProductRepository, IMediatorHandler mediatorHandler)
        {
            _ProductRepository = ProductRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(ProductBellowStockEvent message, CancellationToken cancellationToken)
        {
            var Product = await _ProductRepository.GetById(message.AggregateId);

            // Enviar um email para aquisicao de mais Products.
        }

        public async Task Handle(PedidoIniciadoEvent message, CancellationToken cancellationToken)
        {
            var result = await _estoqueService.DebitarListaProdutosPedido(message.ProdutosPedido);

            //if (result)
            //{
            //    await _mediatorHandler.PublishEvent(new PedidoEstoqueConfirmadoEvent(message.PedidoId, message.ClienteId, message.Total, message.ProdutosPedido, message.NomeCartao, message.NumeroCartao, message.ExpiracaoCartao, message.CvvCartao));
            //}
            //else
            //{
            //    await _mediatorHandler.PublishEvent(new PedidoEstoqueRejeitadoEvent(message.PedidoId, message.ClienteId));
            //}
        }
        
        // devolucao para o estoque
        public async Task Handle(PedidoProcessamentoCanceladoEvent message, CancellationToken cancellationToken)
        {
            await _estoqueService.ReporListaProdutosPedido(message.ProdutosPedido);
        }
    }
}
