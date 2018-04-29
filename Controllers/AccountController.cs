using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RobotJester.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

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
            User user_logging_in = _context.users.Where(u => u.email == user.logEmail).SingleOrDefault();
            if(user_logging_in == null)
                ModelState.AddModelError("Email", "Invalid Email/Password");
                
            else if(hasher.VerifyHashedPassword(user, user_logging_in.password, user.logPassword) == 0)
            {
                ModelState.AddModelError("Password", "Invalid Email/Password");
                return View("Index", "Store");
            }
            if(!ModelState.IsValid)
                return View("Index", user_logging_in);
            HttpContext.Session.SetInt32("id", user_logging_in.user_id);
            return RedirectToAction("Dashboard");
        }

        //REGISTER USERS
        [HttpPost]
        [Route("Register")]
        public IActionResult Register(NewUser newUser)
        {
            PasswordHasher<NewUser> hasher = new PasswordHasher<NewUser>();
            if(_context.users.Where(u => u.email == newUser.email).SingleOrDefault() != null)
                ModelState.AddModelError("Email", "Email in use");
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

                //ADD NEW CART 
                User theUser = _context.Add(User).Entity;
                _context.SaveChanges();

                HttpContext.Session.SetInt32("id", theUser.user_id);
                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }
        

        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("id");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }


        [HttpGet]
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }

        
    }
}