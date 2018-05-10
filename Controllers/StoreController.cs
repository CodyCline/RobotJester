using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RobotJester.Models;
using Microsoft.EntityFrameworkCore;
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
        [Route("Products/{id}")]
        public IActionResult Show(int id)
        {
            Products show = _context.products.SingleOrDefault(item => item.product_id == id);
            if(show == null)
            {
                return RedirectToAction("Index", "StatusCode");
            }
            else 
            {
                return View(show);
            }
            
        }
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
            List<Cart_Items> item_check = _context.cart_items.Include(u => u.user_cart).Where(i => i.cart_id == (int)session_id).ToList();
            Cart cart_query = _context.carts.SingleOrDefault(c => c.cart_id == (int)session_id);
             
            //Loop through item_check to see if it's already in the cart
            foreach(var i in item_check)
            {
                if(i.product_id == product_id)
                {
                    i.quantity += quantity;
                    i.user_cart.total += quantity * added_prod.price;
                    
                    _context.SaveChanges();
                    TempData["Success"] = "Product added to your cart successfully!";
                    return RedirectToAction("Show");
                }
            }

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
            
            //Create new item to add to cart
            else
            {
                Cart_Items new_item = new Cart_Items
                {
                    product_id = added_prod.product_id,
                    cart_id = (int)session_id,
                    quantity = quantity,
                };
                cart_query.total += quantity * added_prod.price;
                _context.Add(new_item);
                _context.SaveChanges();
                TempData["Success"] = "Product added to your cart successfully!";
                return RedirectToAction("Show");
            }
            return View("Show");   
            
            
        }

        //Update quantity from manage cart section   
        [HttpPost]
        [Route("Update/Cart")]
        public IActionResult UpdateCart(int quantity, int update_id) 
        {
            int? session_id = HttpContext.Session.GetInt32("id");
            Cart_Items updated_item = _context.cart_items.Include(p => p.all_items).SingleOrDefault(c => c.product_id == update_id);
            Products product_updated = _context.products.SingleOrDefault(w => w.product_id == updated_item.product_id);
            Cart cart_query = _context.carts.SingleOrDefault(c => c.user_id == (int)session_id);
            if(session_id == null)
            {
                return RedirectToAction("Index", "Store");
            }
            //Query the item that needs to be updated
            /* 
            Will also need to query the cart to update their total as well
            reduce or increase their total with if statement
            */
            
            else if(updated_item.quantity > quantity)
            {
                //Loop through the difference between the current quantity and entered quantity decreasing the price each time
                for(var i = updated_item.quantity; i > quantity; i--)
                {
                    cart_query.total -= product_updated.price;
                    _context.SaveChanges();
                }
                cart_query.updated_at = DateTime.Now;
                updated_item.quantity = quantity;
                _context.SaveChanges();
                TempData["Update"] = "Item updated successfully!";
                return RedirectToAction("CartView", "Account");

            }
            else if(updated_item.quantity == quantity)
            {
                TempData["Update"] = "Item updated successfully!";
                return RedirectToAction("CartView", "Account");
            }
            else
            {
                //Loop through the difference between the current quantity and entered quantity increasing the price each time
                for(var i = updated_item.quantity; i < quantity; i++)
                {
                    cart_query.total += product_updated.price;
                    _context.SaveChanges();
                }
                cart_query.updated_at = DateTime.Now;
                updated_item.quantity = quantity;
                _context.SaveChanges();
                TempData["Update"] = "Item updated successfully!";
                return RedirectToAction("CartView", "Account");
            }
            
            
        }       

        [HttpGet]
        [Route("Remove/Item/{id}")]
        public IActionResult RemoveFromCart(int id)
        {
            int? session_id = HttpContext.Session.GetInt32("id");
            
            Cart_Items item_to_be_removed = _context.cart_items.SingleOrDefault(i => i.item_id == id);
            Cart cart_query = _context.carts.SingleOrDefault(c => c.user_id == (int)session_id);
            Products product_being_removed = _context.products.SingleOrDefault(w => w.product_id == item_to_be_removed.product_id);
            if(item_to_be_removed == null || item_to_be_removed.cart_id != (int)session_id || cart_query == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                cart_query.total -= product_being_removed.price * item_to_be_removed.quantity;
                cart_query.updated_at = DateTime.Now; 
                _context.Remove(item_to_be_removed);
                _context.SaveChanges();
                return RedirectToAction("CartView", "Account");
            }
        }

        [HttpGet]
        [Route("Checkout")]
        public IActionResult Checkout()
        {
            int? session_id = HttpContext.Session.GetInt32("id");
            List<Addresses> addr_list = _context.addresses.Where(a => a.user_id == (int)session_id).ToList();
            ViewBag.AddressList = addr_list;
            return View();
        }

        [HttpPost]
        [Route("Checkout")]
        public IActionResult ValidateCheckout()
        {
            
            /*
            This is where orders will be processed.
            TODO: Stripe.js on front-end for credit cards and use data in backend. 
            File order into database and charge user.
            */
            return RedirectToAction("Manage", "Account");
        }


    }
    
    
}
