using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestAPI.Contracts;
using TestAPI.Filters;
using TestAPI.Models;
using TestAPI.ViewModels;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrdersController : ControllerBase
    {

        private IMapper _mapper;
        private IRepositoryWrapper _repoWrapper;
        private ILoggerManager _logger;
        public PurchaseOrdersController(IMapper mapper, IRepositoryWrapper repoWrapper, ILoggerManager logger)
        {
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchaseOrders()
        {
            var purchaseOrders = await _repoWrapper.PurchaseOrder.GetPurchaseOrdersAsync();
            _logger.LogInfo(LogMessage.GetAll(nameof(PurchaseOrder)));
            return Ok(_mapper.Map<IEnumerable<PurchaseOrderViewModel>>(purchaseOrders));
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<PurchaseOrder>))]
        public async Task<ActionResult<PurchaseOrderViewModel>> GetPurchaseOrder(int id)
        {
            PurchaseOrder purchaseOrder = HttpContext.Items["entity"] as PurchaseOrder;
            _logger.LogInfo(LogMessage.GetById(nameof(PurchaseOrder), id));
            return Ok(_mapper.Map<PurchaseOrder, PurchaseOrderViewModel>(purchaseOrder));
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> PostPurchaseOrder([FromBody] PurchaseOrderViewModel purchaseOrderVM)
        {
            PurchaseOrder purchaseOrder = await _repoWrapper.PurchaseOrder.GetPurchaseOrderById(purchaseOrderVM.Id);
            if (purchaseOrder != null)
            {
                _logger.LogError(LogMessage.ExistsId(nameof(PurchaseOrder), purchaseOrderVM.Id));
                return BadRequest(LogMessage.ExistsId(nameof(PurchaseOrder), purchaseOrderVM.Id));
            }
            purchaseOrder = _mapper.Map<PurchaseOrder>(purchaseOrderVM);
            if (purchaseOrder.Id == 0)
            {
                _logger.LogError(LogMessage.InvalidId(nameof(PurchaseOrder)));
                return BadRequest(LogMessage.InvalidId(nameof(PurchaseOrder)));
            }
            _repoWrapper.PurchaseOrder.CreatePurchaseOrder(purchaseOrder);
            await _repoWrapper.SaveAsync();

            _logger.LogInfo(LogMessage.Created(nameof(PurchaseOrder), purchaseOrderVM.Id));
            return CreatedAtAction(nameof(GetPurchaseOrder), new { id = purchaseOrder.Id }, purchaseOrderVM);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<PurchaseOrder>))]
        public async Task<ActionResult> PutPurchaseOrder(int id, PurchaseOrderViewModel purchaseOrderVM)
        {
            if (id != purchaseOrderVM.Id)
            {
                _logger.LogError(LogMessage.IdNotMatch());
                return BadRequest(LogMessage.IdNotMatch());
            }

            PurchaseOrder purchaseOrder = HttpContext.Items["entity"] as PurchaseOrder;
            _mapper.Map(purchaseOrderVM, purchaseOrder);

            _repoWrapper.PurchaseOrder.UpdatePurchaseOrder(purchaseOrder);
            await _repoWrapper.SaveAsync();

            _logger.LogInfo(LogMessage.Updated(nameof(PurchaseOrder), id));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<PurchaseOrder>))]
        public async Task<ActionResult<PurchaseOrderViewModel>> DeletePurchaseOrder(int id)
        {
            PurchaseOrder purchaseOrder = HttpContext.Items["entity"] as PurchaseOrder;
            if ((await _repoWrapper.OrderDetail.OrderDetailsByPurchaseOrder(id)).Any())
            {
                _logger.LogError(LogMessage.DeleteError(nameof(PurchaseOrder), id, nameof(OrderDetail)));
                return BadRequest(LogMessage.DeleteError(nameof(PurchaseOrder), id, nameof(OrderDetail)));
            }
            _repoWrapper.PurchaseOrder.DeletePurchaseOrder(purchaseOrder);
            await _repoWrapper.SaveAsync();

            _logger.LogInfo(LogMessage.Deleted(nameof(PurchaseOrder), id));
            return NoContent();
        }
    }
}