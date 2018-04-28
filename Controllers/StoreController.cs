/* NOTES: THIS CONTROLLER IS THE MAIN HUB WHERE CUSTOMERS
WILL BE LOOKING AT  */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RobotJester.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using RobotJester.Utilities;
using Microsoft.AspNetCore.Http;

namespace RobotJester.Controllers
{
    public class StoreController : Controller
    {
        private StoreContext _context;
 
        public StoreController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpGet]
        [Route("About")]
        public IActionResult About()
        {
            return View();
        }

        //MAIN PRODUCT PAGE WHERE ITEMS ARE ON DISPLAY

        [HttpGet]
        [Route("Products")]
        public IActionResult Products()
        {
            List<Products> allItems = _context.products.ToList();
            ViewBag.All = allItems;
            return View();
        }
        
        //VIEW SPECIFIC ITEM AND GET DETAILS

        [HttpGet]
        [Route("Product/{id}")]
        public IActionResult Show(int id)
        {
            Products show = _context.products.SingleOrDefault(item => item.product_id == id);
            return View(show);
        }

        //ADD TO CART AND STORE THE VALUE IN A USER SPECIFIC SESSION
        [HttpPost]
        [Route("AddToBag/{id}")]
        public IActionResult AddToCart(int product_id, int quantity, float price)
        {
            
            int? session_id = HttpContext.Session.GetInt32("id");
            Products added_prod = _context.products.SingleOrDefault(p => p.product_id == product_id);
            User user = _context.users.SingleOrDefault(u => u.id == session_id);
            Cart current_cart = _context.cart.SingleOrDefault(c => c.id == session_id);
            // if(product_id == null || quantity < 1)
                // RedirectToAction("Products");
            
            // current_cart.addedProduct = ;
            current_cart.total += quantity * price;
            _context.SaveChanges();
            return RedirectToAction("Products");
        }

        [HttpGet]
        [Route("Remove/{id}")]
        public IActionResult RemoveFromCart(int id, int quantity, float price)
        {
            return null;
        }

        public void Checkout()
        {
        }

    }
}
