// THIS IS WHERE USERS REGISTER AND LOGIN AND WHERE ROLES ARE ASSIGNED (LATER)
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
    }
}