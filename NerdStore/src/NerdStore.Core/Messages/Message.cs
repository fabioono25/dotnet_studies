using System;

namespace NerdStore.Core.Messages
{
    // This class is a base class for all messages in the system
    public abstract class Message
    {
        public string MessageType { get; protected set; }
        public Guid AggregateId { get; protected set; }

        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}