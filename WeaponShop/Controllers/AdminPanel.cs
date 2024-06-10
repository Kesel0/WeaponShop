using Microsoft.AspNetCore.Mvc;
using WeaponShop.Models;
using Microsoft.EntityFrameworkCore;
using WeaponShop.Filters;
namespace WeaponShop.Controllers
{
    [ServiceFilter(typeof(AdminFilter))]
    public class AdminPanel : Controller
    {
        private readonly DatabaseContext _databaseContext;
        public AdminPanel(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public IActionResult Panel ()
        {
            var currentUserName = HttpContext.Session.GetString("Username");
            var users = _databaseContext.Users.FromSqlRaw("SELECT * FROM \"Users\" WHERE \"Username\" = {0}", currentUserName).ToList();
            var products = _databaseContext.Products.FromSqlRaw("Select * from \"Products\" ").ToList();
            var viewModel = new DBViewModel { Users = users, Products = products };
            return View(viewModel);
        }
    }
}
