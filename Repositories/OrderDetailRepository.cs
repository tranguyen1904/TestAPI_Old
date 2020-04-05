using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TestAPI.Contracts;
using TestAPI.Models;

namespace TestAPI.Repositories
{
    public class OrderDetailRepository: RepositoryBase<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(TestAPIContext context):base(context)
        {
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<OrderDetail> GetOrderDetailById(int id)
        {
            return await FindByCondition(c => c.Id == id).FirstOrDefaultAsync();
        }

        public void CreateOrderDetail(OrderDetail OrderDetail)
        {
            Create(OrderDetail);
        }

        public void UpdateOrderDetail(OrderDetail OrderDetail)
        {
            Update(OrderDetail);
        }

        public void DeleteOrderDetail(OrderDetail OrderDetail)
        {
            Delete(OrderDetail);
        }

        public async Task<IEnumerable<OrderDetail>> OrderDetailsByPurchaseOrder(int id)
        {
            return await FindByCondition(o => o.OrderId == id).ToListAsync();
        }

        public async Task<IEnumerable<OrderDetail>> OrderDetailsByProduct(int id)
        {
            return await FindByCondition(o => o.ProductId == id).ToListAsync();
        }
    }
}
