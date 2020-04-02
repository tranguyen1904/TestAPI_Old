using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAPI.Contracts;
using TestAPI.Models;
using TestAPI.ViewModels;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        
        private IMapper _mapper;
        private IRepositoryWrapper _repoWrapper;
        public CustomersController(IMapper mapper, IRepositoryWrapper repoWrapper)
        {
            
            _mapper = mapper;
            _repoWrapper = repoWrapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                var customers = await _repoWrapper.Customer.GetCustomersAsync();
                return Ok(_mapper.Map<IEnumerable<CustomerViewModel>>(customers));
            } catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerViewModel>> GetCustomer(int id)
        {
            try
            {
                var customer = await _repoWrapper.Customer.GetCustomerById(id);
                if (customer == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(_mapper.Map<Customer, CustomerViewModel>(customer));
                }
            } catch(Exception e)
            {
                return StatusCode(500, ErrorMessage.ServerError);
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> PostCustomer([FromBody] CustomerViewModel customerVM)
        {
            
            try
            {
                if (customerVM == null)
                {
                    return BadRequest(ErrorMessage.ObjectNull(nameof(Customer)));
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                Customer customer = await _repoWrapper.Customer.GetCustomerById(customerVM.Id);
                if (customer != null)
                {
                    return BadRequest("Exists");
                }
                customer = _mapper.Map<Customer>(customerVM);
                _repoWrapper.Customer.CreateCustomer(customer);
                await _repoWrapper.SaveAsync();
                return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customerVM);
            } catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutCustomer(int id, [FromBody] CustomerViewModel customerVM)
        {
            try
            {
                if (customerVM == null)
                {
                    return BadRequest(ErrorMessage.ObjectNull(nameof(Customer)));
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ErrorMessage.ModelInvalid);
                }
                if (id != customerVM.Id)
                {
                    return BadRequest();
                }
                var customer = await _repoWrapper.Customer.GetCustomerById(customerVM.Id);
                if (customer == null)
                {
                    return NotFound();
                }
                _mapper.Map(customerVM, customer);
                _repoWrapper.Customer.UpdateCustomer(customer);
                await _repoWrapper.SaveAsync();
                return NoContent();
            } catch(Exception e)
            {
                return StatusCode(500, ErrorMessage.ServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CustomerViewModel>> Delete(int id)
        {
            try
            {
                Customer customer = await _repoWrapper.Customer.GetCustomerById(id);
                if (customer == null)
                {
                    return NotFound();
                }
                _repoWrapper.Customer.DeleteCustomer(customer);
                await _repoWrapper.SaveAsync();
                return NoContent();
            } catch(Exception e)
            {
                return StatusCode(500, ErrorMessage.ServerError);
            }
        }

    }
}