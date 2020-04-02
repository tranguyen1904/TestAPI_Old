using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestAPI.Models;

namespace TestAPI.Contracts
{
    public interface IProductRepository: IRepositoryBase<Product>
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductById(int id);
        void CreateProduct(Product Product);
        void UpdateProduct(Product Product);
        void DeleteProduct(Product Product);
    }
}
