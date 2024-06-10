using Microsoft.AspNetCore.Mvc;
using WeaponShop.Models;
using Microsoft.EntityFrameworkCore;
namespace WeaponShop.Controllers
{
    public class UserController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        public UserController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public IActionResult ShowProfile ()
        {
            var currentUserName = HttpContext.Session.GetString("Username");
            var users = _databaseContext.Users.FromSqlRaw("SELECT * FROM \"Users\" WHERE \"Username\" = {0}", currentUserName).ToList();
            var products = _databaseContext.Products.FromSqlRaw("Select * from \"Products\" ").ToList();
            var viewModel = new DBViewModel { Users = users };
            return View(viewModel);
        }
    }
}
