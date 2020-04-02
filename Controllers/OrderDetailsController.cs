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
    public class OrderDetailsController : ControllerBase
    {
        private IMapper _mapper;
        private IRepositoryWrapper _repoWrapper;
        public OrderDetailsController(IMapper mapper, IRepositoryWrapper repoWrapper)
        {

            _mapper = mapper;
            _repoWrapper = repoWrapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderDetails()
        {
            try
            {
                var orderDetails = await _repoWrapper.OrderDetail.GetOrderDetailsAsync();
                return Ok(_mapper.Map<IEnumerable<OrderDetailViewModel>>(orderDetails));
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailViewModel>> GetOrderDetail(int id)
        {
            try
            {
                var orderDetail = await _repoWrapper.OrderDetail.GetOrderDetailById(id);
                if (orderDetail == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(_mapper.Map<OrderDetail, OrderDetailViewModel>(orderDetail));
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, ErrorMessage.ServerError);
            }

        }

        [HttpPost]
        public async Task<ActionResult> PostOrderDetail([FromBody] OrderDetailViewModel orderDetailVM)
        {

            try
            {
                if (orderDetailVM == null)
                {
                    return BadRequest(ErrorMessage.ObjectNull(nameof(OrderDetail)));
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                OrderDetail orderDetail = await _repoWrapper.OrderDetail.GetOrderDetailById(orderDetailVM.OrderDetailId);
                if (orderDetail != null)
                {
                    return BadRequest("Exists");
                }
                orderDetail = _mapper.Map<OrderDetail>(orderDetailVM);
                _repoWrapper.OrderDetail.CreateOrderDetail(orderDetail);
                await _repoWrapper.SaveAsync();
                return CreatedAtAction(nameof(GetOrderDetail), new { id = orderDetail.OrderDetailId }, orderDetailVM);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutOrderDetail(int id, [FromBody] OrderDetailViewModel orderDetailVM)
        {
            try
            {
                if (orderDetailVM == null)
                {
                    return BadRequest(ErrorMessage.ObjectNull(nameof(OrderDetail)));
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ErrorMessage.ModelInvalid);
                }
                if (id != orderDetailVM.OrderDetailId)
                {
                    return BadRequest();
                }
                var orderDetail = await _repoWrapper.OrderDetail.GetOrderDetailById(orderDetailVM.OrderDetailId);
                if (orderDetail == null)
                {
                    return NotFound();
                }
                _mapper.Map(orderDetailVM, orderDetail);
                _repoWrapper.OrderDetail.UpdateOrderDetail(orderDetail);
                await _repoWrapper.SaveAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, ErrorMessage.ServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderDetailViewModel>> Delete(int id)
        {
            try
            {
                OrderDetail orderDetail = await _repoWrapper.OrderDetail.GetOrderDetailById(id);
                if (orderDetail == null)
                {
                    return NotFound();
                }
                _repoWrapper.OrderDetail.DeleteOrderDetail(orderDetail);
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

