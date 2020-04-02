using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TestAPI.Contracts;
using TestAPI.Models;

namespace TestAPI.Repositories
{
    public class EmployeeRepository: RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(TestAPIContext context):base(context)
        {
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            return await FindByCondition(c => c.Id == id).FirstOrDefaultAsync();
        }

        public void CreateEmployee(Employee Employee)
        {
            Create(Employee);
        }

        public void UpdateEmployee(Employee Employee)
        {
            Update(Employee);
        }

        public void DeleteEmployee(Employee Employee)
        {
            Delete(Employee);
        }
    }
}
