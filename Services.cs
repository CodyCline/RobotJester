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
                return _context.cart_items.Include(a => a.all_items).Where(a => a.cart_id == userSessionId && a.is_active==1).ToList();
                
            }

        }
        //Item count in user cart injectable into any view
        public List<Cart_Items> get_sum
        {
            get {
                int? userSessionId = (int)_httpContext.HttpContext.Session.GetInt32("id");
                // var sum = (from t in _context.cart_items where t.cart_id==userSessionId select t.quantity).Sum(); Later on, query the cart to add up the quantities.
                return _context.cart_items.Where(u => u.cart_id == userSessionId && u.is_active == 1).ToList();                
            }

        }

        //User total displayed on any desired view.
        public Cart user_total
        {
            get {
                int? userSessionId = (int)_httpContext.HttpContext.Session.GetInt32("id");
                return _context.carts.FirstOrDefault(a => a.user_id == userSessionId && a.is_active == 1);
            }

        }
    }

}