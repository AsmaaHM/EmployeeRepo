using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Data;
using EmployeeManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        #region Properties
        private readonly IRepository<Employee> _employeeRepo;
        #endregion

        #region CTOR
        public EmployeesController(IRepository<Employee> employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }
        #endregion

        #region Public methods 
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
        {
            var employees = await _employeeRepo.GetAll();
            return employees.ToList();
        }
        
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _employeeRepo.Get(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            try
            {
                await _employeeRepo.Update(employee);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _employeeRepo.Exists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent(); 
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            try
            {
                await _employeeRepo.Insert(employee);
            }
            catch (DbUpdateException)
            {
                if (await _employeeRepo.Exists(employee.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _employeeRepo.Get(id);
            if (employee == null)
            {
                return NotFound();
            }
            await _employeeRepo.Delete(employee);
            return NoContent();
        }
        #endregion
    }
}