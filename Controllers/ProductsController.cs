using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAPI.Extensions;
using TestAPI.Models;
using TestAPI.ViewModels;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly TestAPIContext _context;
        private readonly IMapper _mapper;

        public ProductsController(TestAPIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetProducts()
        {
            var products = await _context.Product.ToListAsync();
            var vmproducts = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(products);
            return Ok(vmproducts);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductViewModel>> GetProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            
            if (product == null)
            {
                return NotFound();
            }
            var vmproduct = _mapper.Map<Product, ProductViewModel>(product);
            return vmproduct;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductViewModel vmproduct)
        {
            var product = _mapper.Map<ProductViewModel, Product>(vmproduct);
            if (id != product.Id)
            {
                return BadRequest();
            }
            
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!ProductExists(id))
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
        public async Task<ActionResult<Product>> PostProduct([FromBody] ProductViewModel VMproduct)
        {
            Product product = _mapper.Map<ProductViewModel, Product>(VMproduct);

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Product.Add(product);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetProduct", new { id = product.Id }, product);
                }
            } catch (Exception e)
            {
                if (ProductExists(product.Id))
                {
                    return StatusCode(409,$"Product ID={product.Id} already exists. Can not insert to database.");
                }
                ModelState.AddModelError("",e.Message);
                return BadRequest(ModelState.GetErrorMessages());
            }
            
            return BadRequest();
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductViewModel>> DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }


            var orderdetails = _context.OrderDetail.Where(a => a.ProductId == product.Id);
            foreach (var orderdetail in orderdetails)
            {
                product.OrderDetail.Remove(orderdetail);
            }
            
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<Product, ProductViewModel>(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
