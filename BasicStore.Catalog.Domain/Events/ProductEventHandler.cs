using MediatR;

namespace BasicStore.Catalog.Domain.Events
{
    public class ProductEventHandler : INotificationHandler<ProductBellowStockEvent>
    {
        private readonly IProductRepository _ProductRepository;

        public ProductEventHandler(IProductRepository ProductRepository)
        {
            _ProductRepository = ProductRepository;
        }

        public async Task Handle(ProductBellowStockEvent message, CancellationToken cancellationToken)
        {
            var Product = await _ProductRepository.GetById(message.AggregateId);

            // Enviar um email para aquisicao de mais Products.
        }
    }
}
