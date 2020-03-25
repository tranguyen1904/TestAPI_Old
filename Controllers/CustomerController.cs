using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAPI.Models;
using TestAPI.ViewModels;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private TestAPIContext _context;
        private IMapper _mapper;
        public CustomersController(TestAPIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _context.Customer.ToListAsync();
        }

        [HttpGet ("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            Customer customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }

        [HttpPost]
        public async Task<ActionResult> PostCustomer([FromBody] CustomerViewModel vmcustomer)
        {
            Customer customer = _mapper.Map<CustomerViewModel, Customer>(vmcustomer);
            if (CustomerExists(customer.Id))
            {
                return BadRequest($"Customer ID={customer.Id} already exists. Can not insert!");
            }
            _context.Customer.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCustomer), new { id=customer.Id}, vmcustomer);
        }

        private bool CustomerExists(int id)
        {
            
            return _context.Customer.Any(c => c.Id == id);
        }
    }
}