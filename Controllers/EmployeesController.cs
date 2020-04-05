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
    public class EmployeesController : ControllerBase
    {

        private IMapper _mapper;
        private IRepositoryWrapper _repoWrapper;
        private ILoggerManager _logger;
        public EmployeesController(IMapper mapper, IRepositoryWrapper repoWrapper, ILoggerManager logger)
        {
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _repoWrapper.Employee.GetEmployeesAsync();
            _logger.LogInfo(LogMessage.GetAll(nameof(Employee)));
            return Ok(_mapper.Map<IEnumerable<EmployeeViewModel>>(employees));
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Employee>))]
        public async Task<ActionResult<EmployeeViewModel>> GetEmployee(int id)
        {
            Employee employee = HttpContext.Items["entity"] as Employee;
            _logger.LogInfo(LogMessage.GetById(nameof(Employee), id));
            return Ok(_mapper.Map<Employee, EmployeeViewModel>(employee));
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> PostEmployee([FromBody] EmployeeViewModel employeeVM)
        {
            Employee employee = await _repoWrapper.Employee.GetEmployeeById(employeeVM.Id);
            if (employee != null)
            {
                _logger.LogError(LogMessage.ExistsId(nameof(Employee), employeeVM.Id));
                return BadRequest(LogMessage.ExistsId(nameof(Employee), employeeVM.Id));
            }
            employee = _mapper.Map<Employee>(employeeVM);
            if (employee.Id == 0)
            {
                _logger.LogError(LogMessage.InvalidId(nameof(Employee)));
                return BadRequest(LogMessage.InvalidId(nameof(Employee)));
            }
            _repoWrapper.Employee.CreateEmployee(employee);
            await _repoWrapper.SaveAsync();

            _logger.LogInfo(LogMessage.Created(nameof(Employee), employeeVM.Id));
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employeeVM);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Employee>))]
        public async Task<ActionResult> PutEmployee(int id, EmployeeViewModel employeeVM)
        {

            if (id != employeeVM.Id)
            {
                _logger.LogError(LogMessage.IdNotMatch());
                return BadRequest(LogMessage.IdNotMatch());
            }

            Employee employee = HttpContext.Items["entity"] as Employee;
            _mapper.Map(employeeVM, employee);

            _repoWrapper.Employee.UpdateEmployee(employee);
            await _repoWrapper.SaveAsync();

            _logger.LogInfo(LogMessage.Updated(nameof(Employee), id));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Employee>))]
        public async Task<ActionResult<EmployeeViewModel>> DeleteEmployee(int id)
        {
            Employee employee = HttpContext.Items["entity"] as Employee;
            if ((await _repoWrapper.PurchaseOrder.PurchaseOrdersByEmployee(id)).Any())
            {
                _logger.LogError(LogMessage.DeleteError(nameof(Employee), id, nameof(PurchaseOrder)));
                return BadRequest(LogMessage.DeleteError(nameof(Employee), id, nameof(PurchaseOrder)));
            }
            _repoWrapper.Employee.DeleteEmployee(employee);
            await _repoWrapper.SaveAsync();

            _logger.LogInfo(LogMessage.Deleted(nameof(Employee), id));
            return NoContent();
        }

    }
}