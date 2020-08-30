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
    [Route("api/[controller]")]
   // [Authorize(Roles = "Manager")]
    public class CountriesController : ControllerBase
    {
        private readonly AppDbContext _appDbContext = null;

        public CountriesController(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;

        }


        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            List<Country> countries = await _appDbContext.Country.ToListAsync();
            return Ok(countries);
        }
    }
}
