using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeaponShop.Models;

public class ProductDetails : Controller
{
    private readonly DatabaseContext _databaseContext;

    public ProductDetails(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public IActionResult Details(int id)
    {
        
        var product = _databaseContext.Products.FirstOrDefault(p => p.Id == id);
        var currentUserName = HttpContext.Session.GetString("Username");
        var user = _databaseContext.Users.FirstOrDefault(u => u.Username == currentUserName);

        if (product == null)
        {
            return NotFound();
        }
        
        var viewModel = new DetailsViewModel { Products = product, Users = user };

        return View(viewModel);
    }
}
/*var product = _databaseContext.Products.Find(id);*/

/*var viewModel = new DBViewModel { Users = user, Products = product };
        return View(viewModel);*/