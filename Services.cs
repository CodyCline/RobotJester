using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RobotJester.Models;


namespace RobotJester
{
    //This class is a set of queries that are accessed in specific views with dependency injection.
    public class LoggedInUserService
    {
        private StoreContext _context;
        private IHttpContextAccessor _httpContext;
        

        public LoggedInUserService(StoreContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        //Access current user at any view, controller
        public User LoggedInUser
        {
            // int? userSessionId = (int)_httpContext.HttpContext.Session.GetInt32("id");
            get {
                int? userSessionId = (int)_httpContext.HttpContext.Session.GetInt32("id");
                return _context.users.SingleOrDefault(u => u.user_id == (int)userSessionId);
                
            }  
        }

        //Use: To display user cart in dropdown menu at any location
        public List<Cart_Items> user_cart
        {
            get {
                int? userSessionId = (int)_httpContext.HttpContext.Session.GetInt32("id");
                return _context.cart_items.Include(a => a.all_items).Where(a => a.cart_id == userSessionId).ToList();
            }

        }
    }

}