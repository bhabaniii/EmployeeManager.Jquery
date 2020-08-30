using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.Jquery.Controllers
{
   // [Route("[Controller]")]
    public class EmployeeManagerController : Controller
    {
      //  [Route("~/List")]
        public IActionResult List()
        {
            return View();
           



        }


        public IActionResult Insert()
        {
            return View();
        }


        public IActionResult Update(int id )
        {
            ViewBag.EmployeeID = id;
            return View();
        }

        public IActionResult Delete(int id )
        {
            ViewBag.EmployeeID = id;
            return View();
        }


        public IActionResult Register()
        {
            return View();
        }

        //[Route("~/SignIn")]
        public IActionResult SignIn()
        {
            return View();
        }


    }
}
