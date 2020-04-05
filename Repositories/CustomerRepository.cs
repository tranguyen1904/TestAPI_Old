using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TestAPI.Contracts;
using TestAPI.Models;
using TestAPI.ViewModels;

namespace TestAPI.Repositories
{
    public class CustomerRepository: RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(TestAPIContext context):base(context)
        {            
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            return await FindByCondition(c=>c.Id==id).FirstOrDefaultAsync();
        }

        public void CreateCustomer(Customer customer)
        {
            Create(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            Update(customer);
        }

        public void DeleteCustomer(Customer customer)
        {
            Delete(customer);
        }

    }
}
