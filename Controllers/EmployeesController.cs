using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManager.Jquery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Jquery.Controllers
{
    [Route("api/[Controller]")]
    //[Authorize(Roles ="Manager")]
    public class EmployeesController : ControllerBase
    {
        private  AppDbContext _appDBcontext = null;


        public EmployeesController(AppDbContext appDbContext)
        {
            this._appDBcontext = appDbContext;

        }

        [HttpGet]
      
        public async Task<IActionResult> GetAsync()
        {
            List<Employee> employees = await _appDBcontext.Employees.ToListAsync();

            return Ok(employees);

        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id )
        {
            Employee emp = await _appDBcontext.Employees.FindAsync(id);

            return  Ok(emp);
                
        }



        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Employee emp)
            
        {
           if (ModelState.IsValid)
            {
                await _appDBcontext.Employees.AddAsync(emp);
                await _appDBcontext.SaveChangesAsync();
                 return CreatedAtAction("Get", new { id = emp.EmployeeID },emp);
                 //return Ok(emp);
                //return NoContent();
               
            }
           else
            {
                return BadRequest();
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id,[FromBody] Employee emp )
        {
          if (ModelState.IsValid)
            {
                _appDBcontext.Employees.Update(emp);

                await _appDBcontext.SaveChangesAsync();

                return NoContent();
            }

          else
            {

                return BadRequest();
            }




        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Employee employee = await _appDBcontext.Employees.FindAsync(id);
            _appDBcontext.Employees.Remove(employee);
            await _appDBcontext.SaveChangesAsync();
            return NoContent();
        }

    }
}
