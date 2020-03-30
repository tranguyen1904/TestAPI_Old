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
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly TestAPIContext _context;
        private readonly IMapper _mapper;

        public PurchaseOrdersController(TestAPIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/PurchaseOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseOrderViewModel>>> GetPurchaseOrder()
        {
            var purchaseorder = await _context.PurchaseOrder.ToListAsync();
            var po_vm = _mapper.Map<IEnumerable<PurchaseOrder>,IEnumerable<PurchaseOrderViewModel>>(purchaseorder);
            return Ok(po_vm);

        }

        // GET: api/PurchaseOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseOrderViewModel>> GetPurchaseOrder(int id)
        {
            var purchaseOrder = await _context.PurchaseOrder.FindAsync(id);
           
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return _mapper.Map<PurchaseOrder, PurchaseOrderViewModel>(purchaseOrder);
        }

        // PUT: api/PurchaseOrders/5
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchaseOrder(int id, PurchaseOrderViewModel purchaseOrderVM)
        {
            PurchaseOrder purchaseOrder = _mapper.Map<PurchaseOrderViewModel, PurchaseOrder>(purchaseOrderVM);
            if (id != purchaseOrder.Id)
            {
                return BadRequest();
            }

            _context.Entry(purchaseOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!PurchaseOrderExists(id))
                {
                    return NotFound();
                }
                else if (!_context.Customer.Any(c => c.Id == purchaseOrder.CustomerId))
                {
                    return StatusCode(400, $"Customer ID={purchaseOrder.CustomerId} does not exists. Can not update to database.");
                }
                else if (!_context.Employee.Any(e => e.Id == purchaseOrder.EmployeeId))
                {
                    return StatusCode(400, $"Employee ID={purchaseOrder.EmployeeId} does not exists. Can not update to database.");
                }
                else
                {
                    return StatusCode(500, "Internal server error");
                }
            }

            return NoContent();
        }

        // POST: api/PurchaseOrders
        [HttpPost]
        public async Task<ActionResult<PurchaseOrder>> PostPurchaseOrder(PurchaseOrderViewModel purchaseOrderVM)
        {
            PurchaseOrder purchaseOrder = _mapper.Map<PurchaseOrderViewModel, PurchaseOrder>(purchaseOrderVM);
            _context.PurchaseOrder.Add(purchaseOrder);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (PurchaseOrderExists(purchaseOrder.Id))
                {
                    return StatusCode(409, $"PurchaseOrder ID={purchaseOrder.Id} already exists. Can not insert to database.");
                } 
                else if (!_context.Customer.Any(c => c.Id==purchaseOrder.CustomerId))
                {
                    return StatusCode(400, $"Customer ID={purchaseOrder.CustomerId} does not exists. Can not insert to database.");
                }
                else if (!_context.Employee.Any(e => e.Id == purchaseOrder.EmployeeId))
                {
                    return StatusCode(400, $"Employee ID={purchaseOrder.EmployeeId} does not exists. Can not insert to database.");
                }
                else
                {
                    return StatusCode(500, "Internal server error");
                }
            }

            return CreatedAtAction(nameof(GetPurchaseOrder), new { id = purchaseOrder.Id }, purchaseOrder);
        }

        // DELETE: api/PurchaseOrders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PurchaseOrderViewModel>> DeletePurchaseOrder(int id)
        {
            var purchaseOrder = await _context.PurchaseOrder.FindAsync(id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            _context.PurchaseOrder.Remove(purchaseOrder);
            await _context.SaveChangesAsync();

            return _mapper.Map<PurchaseOrder, PurchaseOrderViewModel>(purchaseOrder);
        }

        private bool PurchaseOrderExists(int id)
        {
            return _context.PurchaseOrder.Any(e => e.Id == id);
        }
    }
}
