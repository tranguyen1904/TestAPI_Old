using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestAPI.Models;

namespace TestAPI.Contracts
{
    public interface IPurchaseOrderRepository: IRepositoryBase<PurchaseOrder>
    {
        Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersAsync();
        Task<PurchaseOrder> GetPurchaseOrderById(int id);
        void CreatePurchaseOrder(PurchaseOrder PurchaseOrder);
        void UpdatePurchaseOrder(PurchaseOrder PurchaseOrder);
        void DeletePurchaseOrder(PurchaseOrder PurchaseOrder);

        Task<IEnumerable<PurchaseOrder>> PurchaseOrdersByCustomer(int Id);
        Task<IEnumerable<PurchaseOrder>> PurchaseOrdersByEmployee(int Id);

    }
}
