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
    public class OrderDetailsController : ControllerBase
    {

        private IMapper _mapper;
        private IRepositoryWrapper _repoWrapper;
        private ILoggerManager _logger;
        public OrderDetailsController(IMapper mapper, IRepositoryWrapper repoWrapper, ILoggerManager logger)
        {
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderDetails()
        {
            var orderDetails = await _repoWrapper.OrderDetail.GetOrderDetailsAsync();
            _logger.LogInfo(LogMessage.GetAll(nameof(OrderDetail)));
            return Ok(_mapper.Map<IEnumerable<OrderDetailViewModel>>(orderDetails));
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<OrderDetail>))]
        public async Task<ActionResult<OrderDetailViewModel>> GetOrderDetail(int id)
        {
            OrderDetail orderDetail = HttpContext.Items["entity"] as OrderDetail;
            _logger.LogInfo(LogMessage.GetById(nameof(OrderDetail), id));
            return Ok(_mapper.Map<OrderDetail, OrderDetailViewModel>(orderDetail));
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> PostOrderDetail([FromBody] OrderDetailViewModel orderDetailVM)
        {
            OrderDetail orderDetail = await _repoWrapper.OrderDetail.GetOrderDetailById(orderDetailVM.Id);
            if (orderDetail != null)
            {
                _logger.LogError(LogMessage.ExistsId(nameof(OrderDetail), orderDetailVM.Id));
                return BadRequest(LogMessage.ExistsId(nameof(OrderDetail), orderDetailVM.Id));
            }
            orderDetail = _mapper.Map<OrderDetail>(orderDetailVM);
            if (orderDetail.Id == 0)
            {
                _logger.LogError(LogMessage.InvalidId(nameof(OrderDetail)));
                return BadRequest(LogMessage.InvalidId(nameof(OrderDetail)));
            }
            _repoWrapper.OrderDetail.CreateOrderDetail(orderDetail);
            await _repoWrapper.SaveAsync();

            _logger.LogInfo(LogMessage.Created(nameof(OrderDetail), orderDetailVM.Id));
            return CreatedAtAction(nameof(GetOrderDetail), new { id = orderDetail.Id }, orderDetailVM);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<OrderDetail>))]
        public async Task<ActionResult> PutOrderDetail(int id, OrderDetailViewModel orderDetailVM)
        {

            if (id != orderDetailVM.Id)
            {
                _logger.LogError(LogMessage.IdNotMatch());
                return BadRequest(LogMessage.IdNotMatch());
            }

            OrderDetail orderDetail = HttpContext.Items["entity"] as OrderDetail;
            _mapper.Map(orderDetailVM, orderDetail);

            _repoWrapper.OrderDetail.UpdateOrderDetail(orderDetail);
            await _repoWrapper.SaveAsync();

            _logger.LogInfo(LogMessage.Updated(nameof(OrderDetail), id));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<OrderDetail>))]
        public async Task<ActionResult<OrderDetailViewModel>> DeleteOrderDetail(int id)
        {
            OrderDetail orderDetail = HttpContext.Items["entity"] as OrderDetail;

            _repoWrapper.OrderDetail.DeleteOrderDetail(orderDetail);
            await _repoWrapper.SaveAsync();

            _logger.LogInfo(LogMessage.Deleted(nameof(OrderDetail), id));
            return NoContent();
        }

    }
}