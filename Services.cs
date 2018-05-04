using System.Linq;
using Microsoft.AspNetCore.Http;
using RobotJester.Models;

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
        public User LoggedInDood
        {
            get { 
                int userSessionId = (int)_httpContext.HttpContext.Session.GetInt32("id");
                return _dbContext.users.SingleOrDefault(u => u.user_id == userSessionId);}
        }
    }
}