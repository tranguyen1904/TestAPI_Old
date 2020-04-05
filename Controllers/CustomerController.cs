using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAPI.Contracts;
using TestAPI.Filters;
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
        private ILoggerManager _logger;
        public CustomersController(IMapper mapper, IRepositoryWrapper repoWrapper, ILoggerManager logger)
        {            
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {       
            var customers = await _repoWrapper.Customer.GetCustomersAsync();
            _logger.LogInfo(LogMessage.GetAll(nameof(Customer)));
            return Ok(_mapper.Map<IEnumerable<CustomerViewModel>>(customers));
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Customer>))]
        public async Task<ActionResult<CustomerViewModel>> GetCustomer(int id)
        {
            Customer customer = HttpContext.Items["entity"] as Customer;
            _logger.LogInfo(LogMessage.GetById(nameof(Customer), id));
            return Ok(_mapper.Map<Customer, CustomerViewModel>(customer));
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]        
        public async Task<ActionResult> PostCustomer([FromBody] CustomerViewModel customerVM)
        {            
            Customer customer = await _repoWrapper.Customer.GetCustomerById(customerVM.Id);
            if (customer != null)
            {
                _logger.LogError(LogMessage.ExistsId(nameof(Customer), customerVM.Id));
                return BadRequest(LogMessage.ExistsId(nameof(Customer), customerVM.Id));
            }
            customer = _mapper.Map<Customer>(customerVM);
            if (customer.Id == 0)
            {
                _logger.LogError(LogMessage.InvalidId(nameof(Customer)));
                return BadRequest(LogMessage.InvalidId(nameof(Customer)));
            }
            _repoWrapper.Customer.CreateCustomer(customer);
            await _repoWrapper.SaveAsync();

            _logger.LogInfo(LogMessage.Created(nameof(Customer), customerVM.Id));
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customerVM);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Customer>))]
        public async Task<ActionResult> PutCustomer(int id, CustomerViewModel customerVM)
        {
            
            if (id != customerVM.Id)
            {
                _logger.LogError(LogMessage.IdNotMatch());
                return BadRequest(LogMessage.IdNotMatch());
            }

            Customer customer = HttpContext.Items["entity"] as Customer;           
            _mapper.Map(customerVM, customer);

            _repoWrapper.Customer.UpdateCustomer(customer);
            await _repoWrapper.SaveAsync();

            _logger.LogInfo(LogMessage.Updated(nameof(Customer), id));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Customer>))]
        public async Task<ActionResult<CustomerViewModel>> DeleteCustomer(int id)
        {
            Customer customer = HttpContext.Items["entity"] as Customer;
            var purchaseOrders = await _repoWrapper.PurchaseOrder.PurchaseOrdersByCustomer(id);
            purchaseOrders.Any();
            if ((await _repoWrapper.PurchaseOrder.PurchaseOrdersByCustomer(id)).Any())
            {
                _logger.LogError(LogMessage.DeleteError(nameof(Customer), id, nameof(PurchaseOrder)));
                return BadRequest(LogMessage.DeleteError(nameof(Customer), id, nameof(PurchaseOrder)));
            }
            _repoWrapper.Customer.DeleteCustomer(customer);
            await _repoWrapper.SaveAsync();

            _logger.LogInfo(LogMessage.Deleted(nameof(Customer), id));
            return NoContent();
        }

    }
}