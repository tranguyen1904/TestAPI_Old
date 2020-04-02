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
    public class PurchaseOrdersController : ControllerBase
    {
        private IMapper _mapper;
        private IRepositoryWrapper _repoWrapper;
        public PurchaseOrdersController(IMapper mapper, IRepositoryWrapper repoWrapper)
        {

            _mapper = mapper;
            _repoWrapper = repoWrapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchaseOrders()
        {
            try
            {
                var purchaseOrders = await _repoWrapper.PurchaseOrder.GetPurchaseOrdersAsync();
                return Ok(_mapper.Map<IEnumerable<PurchaseOrderViewModel>>(purchaseOrders));
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseOrderViewModel>> GetPurchaseOrder(int id)
        {
            try
            {
                var purchaseOrder = await _repoWrapper.PurchaseOrder.GetPurchaseOrderById(id);
                if (purchaseOrder == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(_mapper.Map<PurchaseOrder, PurchaseOrderViewModel>(purchaseOrder));
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, ErrorMessage.ServerError);
            }

        }

        [HttpPost]
        public async Task<ActionResult> PostPurchaseOrder([FromBody] PurchaseOrderViewModel purchaseOrderVM)
        {

            try
            {
                if (purchaseOrderVM == null)
                {
                    return BadRequest(ErrorMessage.ObjectNull(nameof(PurchaseOrder)));
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                PurchaseOrder purchaseOrder = await _repoWrapper.PurchaseOrder.GetPurchaseOrderById(purchaseOrderVM.Id);
                if (purchaseOrder != null)
                {
                    return BadRequest("Exists");
                }
                purchaseOrder = _mapper.Map<PurchaseOrder>(purchaseOrderVM);
                _repoWrapper.PurchaseOrder.CreatePurchaseOrder(purchaseOrder);
                await _repoWrapper.SaveAsync();
                return CreatedAtAction(nameof(GetPurchaseOrder), new { id = purchaseOrder.Id }, purchaseOrderVM);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutPurchaseOrder(int id, [FromBody] PurchaseOrderViewModel purchaseOrderVM)
        {
            try
            {
                if (purchaseOrderVM == null)
                {
                    return BadRequest(ErrorMessage.ObjectNull(nameof(PurchaseOrder)));
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ErrorMessage.ModelInvalid);
                }
                if (id != purchaseOrderVM.Id)
                {
                    return BadRequest();
                }
                var purchaseOrder = await _repoWrapper.PurchaseOrder.GetPurchaseOrderById(purchaseOrderVM.Id);
                if (purchaseOrder == null)
                {
                    return NotFound();
                }
                _mapper.Map(purchaseOrderVM, purchaseOrder);
                _repoWrapper.PurchaseOrder.UpdatePurchaseOrder(purchaseOrder);
                await _repoWrapper.SaveAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, ErrorMessage.ServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PurchaseOrderViewModel>> Delete(int id)
        {
            try
            {
                PurchaseOrder purchaseOrder = await _repoWrapper.PurchaseOrder.GetPurchaseOrderById(id);
                if (purchaseOrder == null)
                {
                    return NotFound();
                }
                _repoWrapper.PurchaseOrder.DeletePurchaseOrder(purchaseOrder);
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

