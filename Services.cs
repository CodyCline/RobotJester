using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RobotJester.Models;

//These are queries registered in the Startup.cs file that can be accessed with dependency injection.
namespace RobotJester
{
    public class LoggedInUserService
    {
        private StoreContext _dbContext;
        private IHttpContextAccessor _httpContext;
        public LoggedInUserService(StoreContext context, IHttpContextAccessor httpContext)
        {
            _dbContext = context;
            _httpContext = httpContext;
        }
        public User LoggedInUser
        {
            get { 
                int? userSessionId = (int)_httpContext.HttpContext.Session.GetInt32("id");
                return _dbContext.users.SingleOrDefault(u => u.user_id == userSessionId);
            }
        }
        public List<Cart_Items> user_cart
        {
            get {
                int? userSessionId = (int)_httpContext.HttpContext.Session.GetInt32("id");
                return _dbContext.cart_items.Include(a => a.all_items).Where(a => a.cart_id == userSessionId).ToList();
            }

        }
    }

}