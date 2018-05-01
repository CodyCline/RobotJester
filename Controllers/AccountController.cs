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

namespace RobotJester.Controllers
{
    public class AccountController : Controller
    {
        

        private StoreContext _context;
        public AccountController(StoreContext context)
        {
            _context = context;
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
                ModelState.AddModelError("Email", "Invalid Email/Password");
            }                 
            else if(hasher.VerifyHashedPassword(user, user_logging_in.password, user.Password) == 0)
            {
                ModelState.AddModelError("Password", "Invalid Email/Password");
                return View("Login", "Account");
            }
            if(!ModelState.IsValid)
            {
                return View("Login", user_logging_in);
            }                
            HttpContext.Session.SetInt32("id", user_logging_in.user_id);
            HttpContext.Session.SetString("active_user", user_logging_in.first_name);
            return RedirectToAction("Dashboard");
        }

        //REGISTER USERS
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
                IN ORDER TO CREATE A USER CART WITH MATCHING ID'S IT 
                FIRST NEEDS TO BE FILED INTO THE DATABASE THEN WE QUERY 
                THE USER ID AND THEN CREATE THE CART
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
                return RedirectToAction("Dashboard");
            }
            return View("Register", newUser);
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }
        

        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout()
        {

            foreach (var cookie in Request.Cookies.Keys) //REMOVES ALL COOKIES
            {
                Response.Cookies.Delete(cookie);
            }
            
            return RedirectToAction("Index", "Store");
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

       
        [HttpGet]
        [Route("Account")]
        public IActionResult Dashboard()
        {
            //WILL CHANGE THIS TO AUTHORIZE LATER!
            if(HttpContext.Session.GetInt32("id")==null) 
            {
                return RedirectToAction("Index", "Store");
            }
            User active_user = _context.users.SingleOrDefault(u => u.user_id==(int)HttpContext.Session.GetInt32("id"));
            ViewBag.active_user = active_user;
            return View();
        }

        [HttpGet]
        [Route("Account/Manage")]
        public IActionResult Manage()
        {
            return View();
        }

        

        


        
    }
}