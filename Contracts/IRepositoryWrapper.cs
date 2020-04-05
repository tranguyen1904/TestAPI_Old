using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAPI.Contracts
{
    public interface IRepositoryWrapper
    {
        ICustomerRepository Customer { get; }
        IProductRepository Product { get; }
        IEmployeeRepository Employee { get; }
        IOrderDetailRepository OrderDetail { get; }
        IPurchaseOrderRepository PurchaseOrder { get; }
        void Save();
        Task SaveAsync();

        public IRepositoryBase<T> GetRepo<T>() where T : IEntity;
    }
}
