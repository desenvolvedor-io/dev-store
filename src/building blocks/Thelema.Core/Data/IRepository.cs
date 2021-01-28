using System;
using Thelema.Core.DomainObjects;

namespace Thelema.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}