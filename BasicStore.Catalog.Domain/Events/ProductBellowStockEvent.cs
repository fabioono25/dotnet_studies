using BasicStore.Core.DomainObjects;

namespace BasicStore.Catalog.Domain.Events
{
    public class ProductBellowStockEvent : DomainEvent
    {
        public int AvailableQuantity { get; private set; }

        public ProductBellowStockEvent(Guid aggregateId, int availableQuantity) : base(aggregateId)
        {
            AvailableQuantity = availableQuantity;
        }
    }

}
