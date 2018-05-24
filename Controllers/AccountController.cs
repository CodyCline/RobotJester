using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RobotJester.Models;

namespace RobotJester.Controllers
{
    public class AccountController : Controller
    {
        private StoreContext _context;
        public AccountController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }  

        //Login users
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LogUser user)
        {
            PasswordHasher<LogUser> hasher = new PasswordHasher<LogUser>();
            User user_logging_in = _context.users.Where(u => u.email == user.Email).SingleOrDefault();
            if(user_logging_in == null)
            {
                //Check if email exists
                ModelState.AddModelError("Email", "Sorry, we don't recognize that email");
            }         
            
            else if(hasher.VerifyHashedPassword(user, user_logging_in.password, user.Password) == 0) 
            {
                ModelState.AddModelError("Password", "Invalid/Bad Password");
                return View("Login", user);
            }
            if(!ModelState.IsValid)
            {
                return View("Login", user_logging_in);
            }                
            HttpContext.Session.SetInt32("id", user_logging_in.user_id);
            HttpContext.Session.SetString("active_user", user_logging_in.first_name);
            return RedirectToAction("Manage");
        }
        
        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        //Register users
        [HttpPost]
        [Route("Register")]
        public IActionResult Register(NewUser newUser)
        {
            PasswordHasher<NewUser> hasher = new PasswordHasher<NewUser>();
            if(_context.users.Where(u => u.email == newUser.email).SingleOrDefault() != null)
            {
                ModelState.AddModelError("Email", "Email in use");
            }    
            if(ModelState.IsValid)
            {
                User User = new User
                {
                    first_name = newUser.first_name,
                    last_name = newUser.last_name,
                    email = newUser.email,
                    password = hasher.HashPassword(newUser, newUser.password),
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now
                };
                User new_user = _context.Add(User).Entity;
                _context.SaveChanges();
                HttpContext.Session.SetInt32("id", new_user.user_id);
                HttpContext.Session.SetString("active_user",new_user.first_name);
                /*
                In order to create a user cart with matching id's it first
                must be filed into the database and saved. Then the cart object is created.
                */               
                int? session_id = HttpContext.Session.GetInt32("id");
                Cart user_cart = new Cart
                {
                    user_id = (int)session_id,
                    created_at = DateTime.Today,
                    updated_at = DateTime.Today,
                    is_active = 1,
                };
                _context.Add(user_cart);
                _context.SaveChanges();                
                return RedirectToAction("Manage");
            }
            return View("Register", newUser);
        }
        

        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout()
        {
            foreach (var cookie in Request.Cookies.Keys) //Removes all cookies
            {
                Response.Cookies.Delete(cookie);
            }
            return RedirectToAction("Index", "Store");
        }
              
        [HttpGet]
        [Route("Account")]
        public IActionResult Manage()
        {
            
            int? session_id = HttpContext.Session.GetInt32("id");
            User active_user = _context.users.SingleOrDefault(u => u.user_id==(int)session_id);
            ViewBag.active_user = active_user;
            return View();
        }

        [HttpGet]
        [Route("Account/Cart")]
        public IActionResult CartView()
        {
            int? session_id = HttpContext.Session.GetInt32("id");
            List<Cart_Items> all_items = _context.cart_items.Include(c => c.user_cart).Include(a => a.all_items).Where(a => a.cart_id == (int)session_id).Where(i => i.is_active==1).ToList(); //.Where(c => c.user_cart.cart_id==1)
            return View(all_items);
        }

        //CRUD for addresses
        
        [HttpGet]
        [Route("Account/Addresses")]
        public IActionResult AddressView()
        {
            int? session_id = HttpContext.Session.GetInt32("id");
            List<Addresses> all_addresses = _context.addresses.Include(u => u.corresponding_user).Where(a => a.user_id == (int)session_id).ToList();
            
            return View(all_addresses);
        }

        [HttpGet]
        [Route("Account/Addresses/New")]
        public IActionResult NewAddress()
        {
            return View();
        }

        

        [HttpPost]
        [Route("Account/Addresses/New")]
        public IActionResult ValidateAddress(NewAddress newAddress)
        {
            int? session_id = HttpContext.Session.GetInt32("id");
            if(ModelState.IsValid)
            {
                Addresses address = new Addresses
                {
                    address_line_one = newAddress.address_line_one,
                    address_line_two = newAddress.address_line_two,
                    city = newAddress.city,
                    state_or_province = newAddress.state_or_province,
                    zip_or_postal = newAddress.zip_or_postal,
                    country = newAddress.country,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                    user_id = (int)session_id,  
                };

                _context.Add(address);
                _context.SaveChanges();
                return RedirectToAction("AddressView");
            }
            return View("NewAddress", newAddress);
        }

        [HttpGet]
        [Route("Account/Addresses/Edit/{id}")]
        public IActionResult EditAddress(int id)
        {
            Addresses edit = _context.addresses.SingleOrDefault(address => address.address_id == id);
            return View(edit);
        }

        [HttpPost]
        [Route("Account/Addresses/Edit/{id}")]
        public IActionResult ValidateEdit(int id, NewAddress edit)
        {
            if(ModelState.IsValid)
            {
                Addresses current_address = _context.addresses.SingleOrDefault(a => a.address_id == id);
                {
                    current_address.address_line_one = edit.address_line_one;
                    current_address.address_line_two = edit.address_line_two; 
                    current_address.city = edit.city;
                    current_address.state_or_province = edit.state_or_province;
                    current_address.zip_or_postal = edit.zip_or_postal;
                    current_address.country = edit.country;
                    current_address.updated_at = DateTime.Now;
                    _context.SaveChanges();
                };
                return RedirectToAction("AddressView");
            }
            return View("EditAddress", edit);
        }

        [HttpGet]
        [Route("Account/Addresses/Delete/{id}")]
        public IActionResult DeleteAddress(int id)
        {
            int? session_id = HttpContext.Session.GetInt32("id");
            Addresses item_to_be_removed = _context.addresses.SingleOrDefault(i => i.address_id == id);
            if(item_to_be_removed == null)
            {
                return RedirectToAction("Index");
            }
            _context.Remove(item_to_be_removed);
            _context.SaveChanges();
            return RedirectToAction("AddressView", "Account");
        }

        

        


        // [HttpGet]
        // [Route("Account/Orders")]
        // public IActionResult Orders()
        // {
        //     int? session_id = HttpContext.Session.GetInt32("id");
        //     List<Orders> all_orders = _context.orders.Include(p => p.product_ordered).ToList();
        //     return View(all_orders);
        // }

        

        

        


        
    }
}