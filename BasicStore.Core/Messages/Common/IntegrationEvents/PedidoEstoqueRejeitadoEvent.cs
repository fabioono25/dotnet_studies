using icStore.Core.Messages.Common.IntegrationEvents;

namespace BasicStore.Core.Messages.Common.IntegrationEvents
{
    public class PedidoEstoqueRejeitadoEvent : IntegrationEvent
    {
        public Guid PedidoId { get; private set; }
        public Guid ClienteId { get; private set; }

        public PedidoEstoqueRejeitadoEvent(Guid pedidoId, Guid clienteId)
        {
            AggregateId = pedidoId;
            PedidoId = pedidoId;
            ClienteId = clienteId;
        }
    }
}
