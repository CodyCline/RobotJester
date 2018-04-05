// THIS IS WHERE USERS REGISTER AND LOGIN AND WHERE ROLES ARE ASSIGNED
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RobotJester.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace RobotJester.Controllers
{
    public class LoginRegisterController : Controller
    {
        private StoreContext _context;
 
        public LoginRegisterController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return null;
        }

        public IActionResult Index()
        {
            var users = _context.users.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Register(NewUser newUser)
        {
            PasswordHasher<NewUser> hasher = new PasswordHasher<NewUser>();
            if(_context.users.Where(u => u.email == newUser.email).SingleOrDefault() != null)
                ModelState.AddModelError("Username", "Username in use");
            if(ModelState.IsValid)
            {
                User user = new User
                {
                    first_name = newUser.first_name,
                    last_name = newUser.last_name,
                    email = newUser.email,
                    password = hasher.HashPassword(newUser, newUser.password),
                };
                _context.Add(User);
                _context.SaveChanges();

                // HttpContext.Session.SetInt32("id", User.user_id);
                return RedirectToAction("Index","Wedding");
            }
            return View("Index");
        }
        [HttpPost]
        public IActionResult Login(LogUser logUser)
        {
            PasswordHasher<LogUser> hasher = new PasswordHasher<LogUser>();
            User userToLog = _context.users.Where(u => u.email == logUser.LogEmail).SingleOrDefault();
            if(userToLog == null)
                ModelState.AddModelError("LogEmail", "Invalid Email/Password");
            else if(hasher.VerifyHashedPassword(logUser, userToLog.password, logUser.LogPassword) == 0)
            {
                ModelState.AddModelError("LogEmail", "Invalid Email/Password");
            }
            if(!ModelState.IsValid)
                return View("Index");
            HttpContext.Session.SetInt32("id", userToLog.user_id);
            return RedirectToAction("Index","Wedding");
        }
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                return RedirectToAction("Login");
            }
            
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Role, "MustBeAdmin"),
            },  CookieAuthenticationDefaults.AuthenticationScheme);
            
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return RedirectToAction(nameof(Login));

            
        } 
    }
}