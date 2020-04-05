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
    public class ProductsController : ControllerBase
    {

        private IMapper _mapper;
        private IRepositoryWrapper _repoWrapper;
        private ILoggerManager _logger;
        public ProductsController(IMapper mapper, IRepositoryWrapper repoWrapper, ILoggerManager logger)
        {
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _repoWrapper.Product.GetProductsAsync();
            _logger.LogInfo(LogMessage.GetAll(nameof(Product)));
            return Ok(_mapper.Map<IEnumerable<ProductViewModel>>(products));
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Product>))]
        public async Task<ActionResult<ProductViewModel>> GetProduct(int id)
        {
            Product product = HttpContext.Items["entity"] as Product;
            _logger.LogInfo(LogMessage.GetById(nameof(Product), id));
            return Ok(_mapper.Map<Product, ProductViewModel>(product));
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> PostProduct([FromBody] ProductViewModel productVM)
        {
            Product product = await _repoWrapper.Product.GetProductById(productVM.Id);
            if (product != null)
            {
                _logger.LogError(LogMessage.ExistsId(nameof(Product), productVM.Id));
                return BadRequest(LogMessage.ExistsId(nameof(Product), productVM.Id));
            }
            product = _mapper.Map<Product>(productVM);
            if (product.Id == 0)
            {
                _logger.LogError(LogMessage.InvalidId(nameof(Product)));
                return BadRequest(LogMessage.InvalidId(nameof(Product)));
            }
            _repoWrapper.Product.CreateProduct(product);
            await _repoWrapper.SaveAsync();

            _logger.LogInfo(LogMessage.Created(nameof(Product), productVM.Id));
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productVM);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Product>))]
        public async Task<ActionResult> PutProduct(int id, ProductViewModel productVM)
        {

            if (id != productVM.Id)
            {
                _logger.LogError(LogMessage.IdNotMatch());
                return BadRequest(LogMessage.IdNotMatch());
            }

            Product product = HttpContext.Items["entity"] as Product;
            _mapper.Map(productVM, product);

            _repoWrapper.Product.UpdateProduct(product);
            await _repoWrapper.SaveAsync();

            _logger.LogInfo(LogMessage.Updated(nameof(Product), id));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Product>))]
        public async Task<ActionResult<ProductViewModel>> DeleteProduct(int id)
        {
            Product product = HttpContext.Items["entity"] as Product;
            if ((await _repoWrapper.OrderDetail.OrderDetailsByProduct(id)).Any())
            {
                _logger.LogError(LogMessage.DeleteError(nameof(Product), id, nameof(OrderDetail)));
                return BadRequest(LogMessage.DeleteError(nameof(Product), id, nameof(OrderDetail)));
            }
            _repoWrapper.Product.DeleteProduct(product);
            await _repoWrapper.SaveAsync();

            _logger.LogInfo(LogMessage.Deleted(nameof(Product), id));
            return NoContent();
        }

    }
}