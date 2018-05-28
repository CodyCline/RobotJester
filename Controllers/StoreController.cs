using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RobotJester.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Stripe;
using System.Diagnostics;

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
        
        //Search the store for products
        [HttpPost]
        [Route("Search/q={query}")]
        public IActionResult Search(string query)
        {
            if(!String.IsNullOrEmpty(query))
            {
                List<Products> search = _context.products.Where(s => s.name.Contains(query)).ToList();
                return View(search);
            }
            else
            {
                return RedirectToAction("Index", "Store");
            }
            
        }


        [HttpPost]
        [Route("AddToBag/{id}")]
        public IActionResult AddToCart(int product_id, int quantity)
        {   
            int? session_id = HttpContext.Session.GetInt32("id");
            Products added_prod = _context.products.SingleOrDefault(p => p.product_id == product_id);
            List<Cart_Items> item_check = _context.cart_items.Include(u => u.user_cart).Where(i => i.cart_id == (int)session_id).ToList();
            Cart cart_query = _context.carts.SingleOrDefault(c => c.user_id == (int)session_id && c.is_active == 1); 
             
            //Loop through item_check to see if it's already in the cart
            foreach(var i in item_check)
            {
                if(i.product_id == product_id && i.is_active == 1)
                {
                    i.quantity += quantity;                    
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
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                    is_active = 1
                };
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
            //Query the cart_item, product, and current cart that needs to be updated
            int? session_id = HttpContext.Session.GetInt32("id");
            Cart_Items updated_item = _context.cart_items.Include(p => p.all_items).SingleOrDefault(c => c.product_id == update_id && c.is_active==1);
            Products product_updated = _context.products.SingleOrDefault(w => w.product_id == updated_item.product_id);
            Cart cart_query = _context.carts.SingleOrDefault(c => c.user_id == (int)session_id && c.is_active==1);
            if(session_id == null)
            {
                return RedirectToAction("Index", "Store");
            }
            
            else if(updated_item.quantity > quantity)
            {
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
            //Remove from cart
            int? session_id = HttpContext.Session.GetInt32("id");
            Cart_Items item_to_be_removed = _context.cart_items.SingleOrDefault(i => i.item_id == id);
            Cart cart_query = _context.carts.SingleOrDefault(c => c.user_id == (int)session_id && c.is_active == 1);
            Products product_being_removed = _context.products.SingleOrDefault(w => w.product_id == item_to_be_removed.product_id);
            if(item_to_be_removed == null || item_to_be_removed.cart_id != (int)session_id || cart_query == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
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
            if(session_id == null)
            {
                return RedirectToAction("Index");
            }
            //For displaying on the front end
            List<Addresses> addr_list = _context.addresses.Where(a => a.user_id == (int)session_id).ToList();
            ViewBag.AddressList = addr_list;
            //May deprecate this later this is for displaying the user's cart items
            List<Cart_Items> cart_list = _context.cart_items.Include(a => a.all_items).Where(a => a.cart_id == (int)session_id).ToList();
            ViewBag.Items = cart_list;
            return View();
        }
        
        [HttpPost]
        [Route("Charge")]
        public IActionResult Charge(string stripeEmail, string stripeToken, int total, int address_chosen)
        {
            if(total <=0 )
            {
                return RedirectToAction("Index", "Store");
            }
            //Stripe server code for initiating the charge for their card
            var customerService = new StripeCustomerService();
            var chargeService = new StripeChargeService();
            var customer = customerService.Create(new StripeCustomerCreateOptions {
                Email = stripeEmail,
                SourceToken = stripeToken,
            });
            var charge = chargeService.Create(new StripeChargeCreateOptions {
                Amount = total,
                Description = "RobotJester Checkout",
                Currency = "usd",
                CustomerId = customer.Id
            });
            //Add address here for taxing purposes


            //Update values within the database as well as create a new order 
            //Queries to grab
            int? session_id = HttpContext.Session.GetInt32("id");
            List<Cart_Items> items_bought = _context.cart_items.Include(i => i.all_items).Where(i => i.cart_id==(int)session_id && i.is_active==1).ToList();
            Cart current_cart = _context.carts.SingleOrDefault(c => c.user_id==(int)session_id && c.is_active==1);
            

            //New order gets created first
            Orders new_order = new Orders
            {
                user_id = (int)session_id,
                address_id = address_chosen,
                subtotal = 0, //This field and the one below will created in the future subtotal is cost of items 
                tax = 0, //This will change to shipping_cost and the tax will be estimated at front end and calculated by stripe
                total_billed = total / 100, //From the form
                //Stripe transaction token will also be stored for refunding purposes
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };
            _context.Add(new_order);
            _context.SaveChanges();
            
            //Change the cart_items state to inactive and assign the incremented order_id
            foreach(var i in items_bought)
            {
                //Update the quantity in stock of product bought
                Products quantity_change = _context.products.SingleOrDefault(p => p.product_id==i.product_id);
                quantity_change.instock_quantity -= i.quantity;
                i.order_id = new_order.order_id;
                i.is_active = 0;
                i.updated_at = DateTime.Now;           
            }
            current_cart.is_active = 0; //Deactivate the cart
            current_cart.updated_at = DateTime.Now;
            Cart new_cart = new Cart
            {
                user_id = (int)session_id,
                is_active = 1,
                created_at = DateTime.Now,
                updated_at = DateTime.Now,
                
            };
            _context.Add(new_cart);
            _context.SaveChanges();
            return View("ChargeSuccess");
        }


    }
    
    
}
