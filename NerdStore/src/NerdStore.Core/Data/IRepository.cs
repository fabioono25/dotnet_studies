using System;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Core.Data
{
    // generic repository here. This is a generic interface that will be implemented by the repositories of the domain.
    // 1 repository per aggregate root. why: the repository is responsible for the aggregate root, and the aggregate root is the main entity of the domain.
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}