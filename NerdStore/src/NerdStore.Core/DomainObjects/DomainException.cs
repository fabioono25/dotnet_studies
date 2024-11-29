using System;

namespace NerdStore.Core.DomainObjects
{
    // according to DDD, it's a good practice to create a DomainException
    public class DomainException : Exception
    {
        public DomainException()
        { }

        public DomainException(string message) : base(message)
        { }

        public DomainException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}