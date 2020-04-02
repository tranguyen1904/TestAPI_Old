using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestAPI.Contracts;
using TestAPI.Models;

namespace TestAPI.Repositories
{
    public class RepositoryWrapper: IRepositoryWrapper
    {
        private TestAPIContext _context;
        private ICustomerRepository _customer;
        private IProductRepository _product;
        private IEmployeeRepository _employee;
        private IOrderDetailRepository _orderDetail;
        private IPurchaseOrderRepository _purchaseOrder;

        public RepositoryWrapper(TestAPIContext context)
        {
            _context = context;
        }

        public ICustomerRepository Customer
        {
            get
            {
                if (_customer == null)
                {
                    _customer = new CustomerRepository(_context);
                }
                return _customer;
            }
        }

        public IProductRepository Product
        {
            get
            {
                if (_product == null)
                {
                    _product = new ProductRepository(_context);
                }
                return _product;
            }
        }
        
        public IEmployeeRepository Employee
        {
            get
            {
                if (_employee == null)
                {
                    _employee = new EmployeeRepository(_context);
                }
                return _employee;
            }
        }

        public IOrderDetailRepository OrderDetail
        {
            get
            {
                if (_orderDetail == null)
                {
                    _orderDetail = new OrderDetailRepository(_context);
                }
                return _orderDetail;
            }
        }
        

        public IPurchaseOrderRepository PurchaseOrder
        {
            get
            {
                if (_purchaseOrder == null)
                {
                    _purchaseOrder = new PurchaseOrderRepository(_context);
                }
                return _purchaseOrder;
            }
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
