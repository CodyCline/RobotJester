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
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        public IActionResult AddToCart(int product_id, int quantity)
        {
            
            int? session_id = HttpContext.Session.GetInt32("id");
            Products added_prod = _context.products.SingleOrDefault(p => p.product_id == product_id);
            Cart current_cart = _context.carts.SingleOrDefault(c => c.cart_id == session_id);
            if(added_prod == null || quantity < 1)
            {
                RedirectToAction("Products");
            }
            else if(session_id == null)
            {
                TempData["Error"] = "You must register or log in in to purchase our treasures!";
                return RedirectToAction("Show");
            }
            else
            {
                Cart_Items new_item = new Cart_Items
                {
                    product_id = added_prod.product_id,
                    cart_id = (int)session_id,
                    quantity = quantity

                };
                _context.Add(new_item);
                _context.SaveChanges();
                TempData["Success"] = "Product added to your cart successfully!";
                return RedirectToAction("Show");
            }
            return View(product_id);   
            
            
        }

        [HttpGet]
        [Route("Remove/{id}")]
        public IActionResult RemoveFromCart(int id)
        {
            return null;
        }

        public IActionResult Checkout()
        {
            return null;
        }


    }
    
    
}
