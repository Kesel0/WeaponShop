using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using WeaponShop.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WeaponShop.Controllers
{
    public class AllProducts : Controller
    {
        private readonly DatabaseContext _databaseContext;
        public AllProducts(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public IActionResult Products(string productType, string orderType, string searchTerm)
        {
            List<Product> products = new List<Product>();
            products = _databaseContext.Products.ToList();
            searchTerm = searchTerm?.ToLower() ?? string.Empty;
            if (productType == "Any")
            {
                if (orderType == "Asc")
                {
                    products = _databaseContext.Products
                    .Where(p => EF.Functions.Like(p.ProductName.ToLower(), $"%{searchTerm.ToLower()}%"))
                    .OrderBy(p => p.ProductPrice)
                    .ToList();
                }
                else if (orderType == "Desc")
                {
                    products = _databaseContext.Products
                    .Where(p => EF.Functions.Like(p.ProductName.ToLower(), $"%{searchTerm.ToLower()}%"))
                    .OrderByDescending(p => p.ProductPrice)
                    .ToList();
                }
            }
            else
            {
                if (orderType == "Asc")
                {
                    products = _databaseContext.Products
                    .Where(p => EF.Functions.Like(p.ProductType, $"%{productType}%") && EF.Functions.Like(p.ProductName.ToLower(), $"%{searchTerm.ToLower()}%"))
                    .OrderBy(p => p.ProductPrice)
                    .ToList();
                }
                else if (orderType == "Desc")
                {
                    products = _databaseContext.Products
                    .Where(p => EF.Functions.Like(p.ProductType, $"%{productType}%") && EF.Functions.Like(p.ProductName.ToLower(), $"%{searchTerm.ToLower()}%"))
                    .OrderByDescending(p => p.ProductPrice)
                    .ToList();
                }
            }
            return View(products);
        }
    }
}


