using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WeaponShop.Models;
using Microsoft.EntityFrameworkCore;
namespace WeaponShop.Filters
{
    public class AdminFilter : IAuthorizationFilter
    {
        private readonly DatabaseContext _databaseContext;
        public AdminFilter(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var currentUserName = context.HttpContext.Session.GetString("Username");
            var users = _databaseContext.Users.FromSqlRaw("SELECT * FROM \"Users\" WHERE \"Username\" = {0}", currentUserName).ToList();
            if (currentUserName == null || users == null || !users[0].isadmin)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}

