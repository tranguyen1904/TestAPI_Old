using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TestAPI.Contracts;
using TestAPI.Models;

namespace TestAPI.Repositories
{
    public class PurchaseOrderRepository: RepositoryBase<PurchaseOrder>, IPurchaseOrderRepository
    {
        public PurchaseOrderRepository(TestAPIContext context):base(context)
        {
        }

        public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<PurchaseOrder> GetPurchaseOrderById(int id)
        {
            return await FindByCondition(c => c.Id == id).FirstOrDefaultAsync();
        }

        public void CreatePurchaseOrder(PurchaseOrder PurchaseOrder)
        {
            Create(PurchaseOrder);
        }

        public void UpdatePurchaseOrder(PurchaseOrder PurchaseOrder)
        {
            Update(PurchaseOrder);
        }

        public void DeletePurchaseOrder(PurchaseOrder PurchaseOrder)
        {
            Delete(PurchaseOrder);
        }

        public async Task<IEnumerable<PurchaseOrder>> PurchaseOrdersByCustomer(int Id)
        {
            return await FindByCondition(p => p.CustomerId == Id).ToListAsync();
        }

        public async Task<IEnumerable<PurchaseOrder>> PurchaseOrdersByEmployee(int Id)
        {
            return await FindByCondition(p => p.EmployeeId == Id).ToListAsync();
        }
    }
}
