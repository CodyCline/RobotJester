/*TODO: RENAME TO OFFICE CONTROLLER THIS IS WHERE
ONLY ADMINS AND MANAGERS CAN VIEW ORDERS AND OTHER
SENSITIVE DATA */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RobotJester.Models;
using Microsoft.EntityFrameworkCore;

namespace RobotJester.Controllers
{
    public class OrderController : Controller
    {
        private StoreContext _context;
 
        public OrderController(StoreContext context)
        {
            _context = context;
        }

        

        [HttpGet]
        [Route("Orders")]
        public IActionResult Orders()
        {
            return View();
        }

        [HttpGet]
        [Route("Orders/{id}")]
        public IActionResult Show()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddToCart()
        {
            Orders cart = new Orders
            {
            };
            return RedirectToAction("orders/@product_id"); 
        }
    }
}