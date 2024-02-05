using BasicStore.Core.Messages;

namespace BasicStore.Sales.Application.Events
{
    public class PedidoRascunhoIniciadoEvent : Event
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }

        public PedidoRascunhoIniciadoEvent(Guid clienteId, Guid pedidoId)
        {
            AggregateId = pedidoId; // persistir para qual tipo de entidade (raiz de agregacao)
            ClienteId = clienteId;
            PedidoId = pedidoId;
        }
    }
}
