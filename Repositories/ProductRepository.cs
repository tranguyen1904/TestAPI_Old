using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TestAPI.Contracts;
using TestAPI.Models;

namespace TestAPI.Repositories
{
    public class ProductRepository: RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(TestAPIContext context):base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await FindByCondition(c => c.Id == id).FirstOrDefaultAsync();
        }

        public void CreateProduct(Product Product)
        {
            Create(Product);
        }

        public void UpdateProduct(Product Product)
        {
            Update(Product);
        }

        public void DeleteProduct(Product Product)
        {
            Delete(Product);
        }
    }
}
