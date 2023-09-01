using BasicStore.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
