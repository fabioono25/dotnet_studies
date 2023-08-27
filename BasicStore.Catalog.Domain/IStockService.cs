using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicStore.Catalog.Domain
{
    public interface IStockService : IDisposable
    {
        Task<bool> RemoveFromStock(Guid productId, int quantity);
        Task<bool> AddToStock(Guid productId, int quantity);
    }
}
