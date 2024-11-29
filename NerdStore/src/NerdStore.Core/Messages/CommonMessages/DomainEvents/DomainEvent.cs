using System;
using MediatR;

namespace NerdStore.Core.Messages.CommonMessages.DomainEvents
{
    // This class is a base class for all domain events in the system
    public abstract class DomainEvent : Message, INotification
    {
        public DateTime Timestamp { get; private set; }

        protected DomainEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
            Timestamp = DateTime.Now;
        }
    }
}