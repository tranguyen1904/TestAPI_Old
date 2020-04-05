using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestAPI.Models;

namespace TestAPI.Contracts
{
    public interface IOrderDetailRepository: IRepositoryBase<OrderDetail>
    {
        Task<IEnumerable<OrderDetail>> GetOrderDetailsAsync();
        Task<OrderDetail> GetOrderDetailById(int id);
        void CreateOrderDetail(OrderDetail OrderDetail);
        void UpdateOrderDetail(OrderDetail OrderDetail);
        void DeleteOrderDetail(OrderDetail OrderDetail);

        Task<IEnumerable<OrderDetail>> OrderDetailsByPurchaseOrder(int id);
        Task<IEnumerable<OrderDetail>> OrderDetailsByProduct(int id);
    }
}
