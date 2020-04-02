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
using TestAPI.Repositories;
using TestAPI.ViewModels;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public EmployeesController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repoWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeViewModel>>> GetEmployee()
        {
            try
            {

                var employees = await _repoWrapper.Employee.FindAll().ToListAsync();
                return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees));
            } catch (Exception e)
            {
                return StatusCode(500, ErrorMessage.ServerError);
            }            
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeViewModel>> GetEmployee(int id)
        {
            try
            {
                Employee employee = await _repoWrapper.Employee.GetEmployeeById(id);
                if (employee == null)
                {
                    return NotFound();
                } else
                {
                    return Ok(_mapper.Map<EmployeeViewModel>(employee));
                }                
            } catch (Exception e)
            {
                return StatusCode(500, ErrorMessage.ServerError);
            }            
        }
                
        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee([FromBody] EmployeeViewModel employeeVM)
        {
            try
            {
                if (employeeVM == null)
                {
                    return BadRequest(ErrorMessage.ObjectNull(nameof(Employee)));
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ErrorMessage.ModelInvalid);
                }

                Employee employee = await _repoWrapper.Employee.GetEmployeeById(employeeVM.Id);
                if (employee != null)
                {
                    return BadRequest("Exists");
                }
                employee = _mapper.Map<Employee>(employeeVM);
                
                _repoWrapper.Employee.UpdateEmployee(employee);
                await _repoWrapper.SaveAsync();
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employeeVM);
            } catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, [FromBody]EmployeeViewModel employeeVM)
        {
            try
            {
                if (employeeVM == null)
                {
                    return BadRequest(ErrorMessage.ObjectNull(nameof(Employee)));
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ErrorMessage.ModelInvalid);
                }
                Employee employee = await _repoWrapper.Employee.GetEmployeeById(id);
                if (employee == null)
                {
                    return NotFound();
                }
                _mapper.Map(employeeVM, employee);
                _repoWrapper.Employee.UpdateEmployee(employee);
                await _repoWrapper.SaveAsync();
                return NoContent();
            } catch (Exception e)
            {
                return StatusCode(500, ErrorMessage.ServerError);
            }
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeViewModel>> DeleteEmployee(int id)
        {
            try
            {
                Employee employee = await _repoWrapper.Employee.GetEmployeeById(id);
                if (employee == null)
                {
                    return NotFound();
                }
                _repoWrapper.Employee.DeleteEmployee(employee);
                await _repoWrapper.SaveAsync();
                return NoContent();
            } catch(Exception e)
            {
                return StatusCode(500, ErrorMessage.ServerError);
            }
        }

        
    }
}
