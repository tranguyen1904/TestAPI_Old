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
    public class OrderDetailsController : ControllerBase
    {
        private readonly TestAPIContext _context;
        private readonly IMapper _mapper;
        public OrderDetailsController(TestAPIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/OrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetailViewModel>>> GetOrderDetail()
        {
            var orderDetail = await _context.OrderDetail.ToListAsync();
            var orderDetailVM = _mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailViewModel>>(orderDetail);
            return Ok(orderDetailVM);
        }

        // GET: api/OrderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailViewModel>> GetOrderDetail(int id)
        {
            var orderDetail = await _context.OrderDetail.FindAsync(id);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return _mapper.Map<OrderDetail, OrderDetailViewModel>(orderDetail);
        }

        // PUT: api/OrderDetails/5

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderDetail(int id, OrderDetailViewModel orderDetailVM)
        {
            OrderDetail orderDetail = _mapper.Map<OrderDetailViewModel, OrderDetail>(orderDetailVM);
            if (id != orderDetail.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(orderDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!OrderDetailExists(id))
                {
                    return NotFound();
                }
                else if (!_context.PurchaseOrder.Any(c => c.Id == orderDetail.OrderId))
                {
                    return StatusCode(400, $"PurChaseOrder ID={orderDetail.OrderId} does not exists. Can not update to database.");
                }
                else if (!_context.Product.Any(e => e.Id == orderDetail.ProductId))
                {
                    return StatusCode(400, $"Product ID={orderDetail.ProductId} does not exists. Can not update to database.");
                }
                else
                {
                    return StatusCode(500, "Internal server error");
                }
            }

            return NoContent();
        }

        // POST: api/OrderDetails

        [HttpPost]
        public async Task<ActionResult<OrderDetail>> PostOrderDetail([FromBody]OrderDetailViewModel orderDetailVM)
        {
            OrderDetail orderDetail = _mapper.Map<OrderDetailViewModel, OrderDetail>(orderDetailVM);
            _context.OrderDetail.Add(orderDetail);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (OrderDetailExists(orderDetail.OrderId))
                {
                    return StatusCode(409, $"OrderDetail ID={orderDetail.OrderDetailId} already exists. Can not insert to database."); ;
                }
                else if (!_context.PurchaseOrder.Any(c => c.Id == orderDetail.OrderId))
                {
                    return StatusCode(400, $"PurChaseOrder ID={orderDetail.OrderId} does not exists. Can not insert to database.");
                }
                else if (!_context.Product.Any(e => e.Id == orderDetail.ProductId))
                {
                    return StatusCode(400, $"Product ID={orderDetail.ProductId} does not exists. Can not insert to database.");
                }
                else
                {
                    return StatusCode(500, "Internal server error");
                }
            }

            return CreatedAtAction("GetOrderDetail", new { id = orderDetail.OrderId }, orderDetail);
        }

        // DELETE: api/OrderDetails/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderDetailViewModel>> DeleteOrderDetail(int id)
        {
            var orderDetail = await _context.OrderDetail.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            _context.OrderDetail.Remove(orderDetail);
            await _context.SaveChangesAsync();

            return _mapper.Map<OrderDetail, OrderDetailViewModel>(orderDetail);
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetail.Any(e => e.OrderDetailId == id);
        }
    }
}
