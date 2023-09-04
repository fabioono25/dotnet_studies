using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BasicStore.Core.Messages;
using BasicStore.Core.Messages;

namespace BasicStore.Core.Data.EventSourcing
{
    public interface IEventSourcingRepository
    {
        Task SalvarEvento<TEvent>(TEvent evento) where TEvent : Event;
        Task<IEnumerable<StoredEvent>> ObterEventos(Guid aggregateId);
    }
}