using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RobotJester.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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

        //LOGIN USERS CHANGE TO ASYNC METHOD LATER WITH POSTGRES + IDENTITY CLAIMS
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LogUser user)
        {
            PasswordHasher<LogUser> hasher = new PasswordHasher<LogUser>();
            User user_logging_in = _context.users.Where(u => u.email == user.Email).SingleOrDefault();
            if(user_logging_in == null)
            {
                //Check if email exists
                ModelState.AddModelError("Email", "Invalid Email/Password");
            }                 
            else if(hasher.VerifyHashedPassword(user, user_logging_in.password, user.Password) == 0)
            {
                ModelState.AddModelError("Password", "Invalid Email/Password");
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
                    created_at = DateTime.Today,
                    updated_at = DateTime.Today,


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
            List<Cart_Items> all_items = _context.cart_items.Include(a => a.all_items).Where(a => a.cart_id == (int)session_id).ToList();
            return View(all_items);
        }

        [HttpGet]
        [Route("Account/Addresses")]
        public IActionResult Addresses()
        {
            int? session_id = HttpContext.Session.GetInt32("id");
            List<Addresses> all_addresses = _context.addresses.Include(u => u.corresponding_user).Where(a => a.user_id == (int)session_id).ToList();
            return View(all_addresses);
            

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