using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DataAcces;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees
                                 .Include(e => e.District)
                                 .ThenInclude(d => d.State)
                                 .ToListAsync();
        }

        // GET: api/EmployeeApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees
                                         .Include(e => e.District)
                                         .ThenInclude(d => d.State)
                                         .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // POST: api/EmployeeApi
        //[HttpPost]
        //public async Task<ActionResult<Employee>> PostEmployee([FromBody]Employee employee)
        //{
        //    if (!_context.Districts.Any(d => d.Id == employee.DistrictId))
        //    {
        //        return BadRequest("Invalid DistrictId.");
        //    }

        //    _context.Employees.Add(employee);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        //}
        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee([FromBody] EmployeeCreateDto dto)
        {
            var employee = new Employee
            {
                Name = dto.Name,
                Dob = dto.Dob,
                DistrictId = dto.DistrictId
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }

        // PUT: api/EmployeeApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest("ID mismatch.");
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Employees.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/EmployeeApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
public class EmployeeCreateDto
{
    public string Name { get; set; }
    public DateTime Dob { get; set; }
    public int DistrictId { get; set; }
}
public class DistrictDto
{
    public string Name { get; set; }
    public string StateName { get; set; }
}
