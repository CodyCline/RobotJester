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
        [Route("product/{id}")]
        public IActionResult Show(int id)
        {
            Products show = _context.products.SingleOrDefault(item => item.product_id == id);
            return View(show);
        }

        //ADD TO CART AND STORE THE VALUE IN A USER SPECIFIC SESSION
        [HttpPost]
        [Route("addtobag")]
        
        public IActionResult AddToCart(int id, int quantity)
        {
            Products prod = _context.products.SingleOrDefault(p => p.product_id == id);
            HttpContext.Session.SetObjectAsJson("1", prod);
            // if (prod == null)
                // return RedirectToAction("Products");
            // List<Products> cart = HttpContext.Session.GetObjectFromJson<List<Products>>("cart");
            // cart.Add(prod);
            // HttpContext.Session.SetObjectAsJson("cart", cart);
            return View("Index");
        }

        public void Checkout()
        {
            
            //DESERIALIZE THE OBJECTS IN SESSION AND START PENDING THEM AS AN ORDER 
            List<Products> cart = HttpContext.Session.GetObjectFromJson<List<Products>>("cart");
        }

    }
}
