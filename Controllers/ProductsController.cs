using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAPI.Contracts;
using TestAPI.Extensions;
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
        public ProductsController(IMapper mapper, IRepositoryWrapper repoWrapper)
        {

            _mapper = mapper;
            _repoWrapper = repoWrapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _repoWrapper.Product.GetProductsAsync();
                return Ok(_mapper.Map<IEnumerable<ProductViewModel>>(products));
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductViewModel>> GetProduct(int id)
        {
            try
            {
                var product = await _repoWrapper.Product.GetProductById(id);
                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(_mapper.Map<Product, ProductViewModel>(product));
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, ErrorMessage.ServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostProduct([FromBody] ProductViewModel productVM)
        {
            try
            {
                if (productVM == null)
                {
                    return BadRequest(ErrorMessage.ObjectNull(nameof(Product)));
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                Product product = await _repoWrapper.Product.GetProductById(productVM.Id);
                if (product != null)
                {
                    return BadRequest("Exists");
                }
                product = _mapper.Map<Product>(productVM);
                _repoWrapper.Product.CreateProduct(product);
                await _repoWrapper.SaveAsync();
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productVM);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(int id, [FromBody] ProductViewModel productVM)
        {
            try
            {
                if (productVM == null)
                {
                    return BadRequest(ErrorMessage.ObjectNull(nameof(Product)));
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ErrorMessage.ModelInvalid);
                }
                if (id != productVM.Id)
                {
                    return BadRequest();
                }
                var product = await _repoWrapper.Product.GetProductById(productVM.Id);
                if (product == null)
                {
                    return NotFound();
                }
                _mapper.Map(productVM, product);
                _repoWrapper.Product.UpdateProduct(product);
                await _repoWrapper.SaveAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, ErrorMessage.ServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductViewModel>> Delete(int id)
        {
            try
            {
                Product product = await _repoWrapper.Product.GetProductById(id);
                if (product == null)
                {
                    return NotFound();
                }
                _repoWrapper.Product.DeleteProduct(product);
                await _repoWrapper.SaveAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, ErrorMessage.ServerError);
            }
        }

    }
}

