using Microsoft.AspNetCore.Mvc;
using WeaponShop.Models;
using Microsoft.EntityFrameworkCore;
using WeaponShop.Filters;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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
        [HttpGet]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _databaseContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Json(new
            {
                id = product.Id,
                productName = product.ProductName,
                productType = product.ProductType,
                productSubtype = product.ProductSubtype,
                productDescription = product.ProductDescription,
                productPrice = product.ProductPrice,
                Caliber = product.Caliber,
                underLicence = product.under_licence
            });;
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductViewModel model, IFormFile editProductImage)
        {
            if (editProductImage != null && editProductImage.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/img", editProductImage.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await editProductImage.CopyToAsync(stream);
                }
                model.Image = editProductImage.FileName;
            }

            if (ModelState.IsValid)
            {
                var product = await _databaseContext.Products.FindAsync(model.Id);
                if (product == null)
                {
                    return NotFound();
                }

                product.ProductName = model.ProductName;
                product.ProductSubtype = model.ProductSubtype;
                product.ProductPrice = model.ProductPrice;
                product.ProductType = model.ProductType;
                product.ProductDescription = model.ProductDescription;
                product.Caliber = model.Caliber;
                product.under_licence = model.under_licence;
                if (!string.IsNullOrEmpty(model.Image))
                {
                    product.Image = model.Image; // Update the image only if a new one is uploaded
                }

                _databaseContext.Update(product);
                await _databaseContext.SaveChangesAsync();
                return Ok();
            }

            // Log model errors
            foreach (var state in ModelState)
            {
                Console.WriteLine($"Key: {state.Key}");
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }
            }

            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductViewModel model, IFormFile editProductImage)
        {
            if (editProductImage != null && editProductImage.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/img", editProductImage.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await editProductImage.CopyToAsync(stream);
                }
                model.Image = editProductImage.FileName;
            }

            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    ProductName = model.ProductName,
                    ProductSubtype = model.ProductSubtype,
                    ProductPrice = model.ProductPrice,
                    ProductType = model.ProductType,
                    ProductDescription = model.ProductDescription,
                    Caliber = model.Caliber,
                    under_licence = model.under_licence,
                    Image = model.Image // Ensure this is correctly set
                };

                _databaseContext.Add(product);
                await _databaseContext.SaveChangesAsync();
                return Ok();
            }

            // Log model errors
            foreach (var state in ModelState)
            {
                Console.WriteLine($"Key: {state.Key}");
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }
            }

            return BadRequest();
        }

    }

}
