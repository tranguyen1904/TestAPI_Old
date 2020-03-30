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
        public async Task<ActionResult<IEnumerable<CustomerViewModel>>> GetCustomers()
        {
            var customers = await _context.Customer.ToListAsync();
            var customervm = _mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerViewModel>>(customers);
            return Ok(customervm);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerViewModel>> GetCustomer(int id)
        {
            Customer customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return _mapper.Map<Customer, CustomerViewModel>(customer);
        }

        [HttpGet("{id}/detail")]
        public async Task<ActionResult<Customer>> GetCustomerWithDetail(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            var productorder = _context.PurchaseOrder.Where(po=>po.CustomerId==id).ToList();
            foreach(var po in productorder)
            {
                customer.PurchaseOrder.Add(po);
            }
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }

        [HttpPut("{id}")]

        public async Task<ActionResult> PutCustomer(int id, [FromBody] CustomerViewModel vmcustomer)
        {
            Customer customer = _mapper.Map<CustomerViewModel, Customer>(vmcustomer);
            if (id != customer.Id)
            {
                return BadRequest();
            }
            _context.Entry(customer).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500, "Internal server error");
                }
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> PostCustomer([FromBody] CustomerViewModel vmcustomer)
        {
            Customer customer = _mapper.Map<CustomerViewModel, Customer>(vmcustomer);
            if (CustomerExists(customer.Id))
            {
                return StatusCode(409,$"Customer ID={customer.Id} already exists. Can not insert!");
            }
            _context.Customer.Add(customer);
            try
            {
                await _context.SaveChangesAsync();
            } catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
            
            return CreatedAtAction(nameof(GetCustomer), new { id=customer.Id}, vmcustomer);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CustomerViewModel>> Delete(int id)
        {
            var customer = await _context.Customer.FindAsync(id);

            if (!CustomerExists(id))
            {
                return NotFound();
            }
            var purchaseOrder = _context.PurchaseOrder.Where(po => po.CustomerId == customer.Id);
            foreach(var po in purchaseOrder)
            {
                customer.PurchaseOrder.Remove(po);
            }
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return _mapper.Map<Customer, CustomerViewModel>(customer);
        }

        private bool CustomerExists(int id)
        {
            
            return _context.Customer.Any(c => c.Id == id);
        }
    }
}