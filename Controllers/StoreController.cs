using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RobotJester.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
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
            //On landing, checks if the user is logged in
            int? session_id = HttpContext.Session.GetInt32("id");
            if(session_id==null) 
            {
                foreach (var cookie in Request.Cookies.Keys) //Removes all cookies
                {
                    Response.Cookies.Delete(cookie);
                }
                
            }
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

        //Add to cart 
        /*
        TODO: If the user already added that specific item to their cart 
        Update the quantity instead of adding a seperate item
        */
        [HttpPost]
        [Route("AddToBag/{id}")]
        public IActionResult AddToCart(int product_id, int quantity)
        {
            
            int? session_id = HttpContext.Session.GetInt32("id");
            Products added_prod = _context.products.SingleOrDefault(p => p.product_id == product_id);
            Cart_Items cart_check = _context.cart_items.FirstOrDefault(c => c.cart_id == (int)session_id); //Check if the item is already in cart
            
            //Null check in case the user is doing something weird like adding 0 items or manipulating the HTML form
            if(added_prod == null || quantity < 1) 
            {
                TempData["Cart"] = "Please add at least one item to the cart";
                RedirectToAction("Show");
            }
            else if(session_id == null)
            {
                TempData["Error"] = "You must register or log in in to purchase our treasures!";
                return RedirectToAction("Show");
            }
            //Check if item is in the cart. If true (not null) then update existing cart
            else if(cart_check != null)
            {
                cart_check.quantity += quantity;
                _context.SaveChanges();
                TempData["Success"] = "Product added to your cart successfully!";
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
            return View("Show");   
            
            
        }

        //Update quantity from manage cart section need overhaul    
        [HttpPost]
        [Route("Update/Cart")]
        public IActionResult UpdateCart(int quantity) 
        {
            //Note: if a user has a duplicate item in their cart it will throw a serious error
            int? session_id = HttpContext.Session.GetInt32("id");
            Cart_Items updated_item = _context.cart_items.Include(p => p.all_items).FirstOrDefault(c => c.cart_id == (int)session_id);

            if(session_id == null)
            {
                return RedirectToAction("Index", "Store");
            }

            //Query the item that needs to be updated
            
            /* 
            Will also need to query the cart to update their total as well
            reduce or increase their total with if statement

            if(updated_item.quantity > quantity)
            {
                cart_query.total -= updated_item.quantity * updated_item.all_items.price;
            }
            else if(updated_item.quantity < quantity)
            {
                cart_query.total += updated_item.quantity * updated_item.all_items.price;
            }
            */
            else
            {            
                updated_item.quantity = quantity;
                _context.SaveChanges();
                TempData["Update"] = "Item updated successfully";
                return RedirectToAction("CartView", "Account");
            }
            
        }

        [HttpGet]
        [Route("Remove/Item/{id}")]
        public IActionResult RemoveFromCart(int id)
        {
            int? session_id = HttpContext.Session.GetInt32("id");
            Cart_Items item_to_be_removed = _context.cart_items.SingleOrDefault(i => i.item_id == id);
            if(item_to_be_removed == null || item_to_be_removed.cart_id != (int)session_id)
            {
                return RedirectToAction("Index");
            }
            _context.Remove(item_to_be_removed);
            _context.SaveChanges();
            return RedirectToAction("CartView", "Account");
        }

        [HttpGet]
        [Route("Checkout")]
        public IActionResult Checkout()
        {
            return View();
        }


    }
    
    
}
