/*using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using WeaponShop.Models;

namespace WeaponShop.Controllers
{
    public class DBshow : Controller
    {
        private readonly DatabaseContext _databaseContext;
        public DBshow(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public IActionResult Index()
        {
            var products = _databaseContext.Products.FromSqlRaw("Select * from \"Products\" LIMIT 3").ToList();
            var users = _databaseContext.Users.FromSqlRaw("Select * from \"Users\" ").ToList();
            Console.WriteLine("Products:");
            foreach (var product in products)
            {
                Console.WriteLine($"{product.Id} - {product.ProductName}");
            }
            var viewModel = new DBViewModel { Products = products, Users = users };

            return View(viewModel);
        }
    }
}*/


//Сверху - без адаптера, снизу - с адаптером


using Microsoft.AspNetCore.Mvc;
using WeaponShop.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
namespace WeaponShop.Controllers
{
    public class DBshow : Controller
    {
        private readonly IDbAdapter _dbAdapter;

        public DBshow(IDbAdapter dbAdapter)
        {
            _dbAdapter = dbAdapter;
        }

        public IActionResult Index()
        {
            var products = _dbAdapter.GetProducts(5);
            var users = _dbAdapter.GetUsers();

            var viewModel = new DBViewModel { Products = products, Users = users };

            return View(viewModel);
        }
    }

    public interface IDbAdapter
    {
        List<Product> GetProducts(int limit);
        List<User> GetUsers();
    }

    public class PostgreSQLAdapter : IDbAdapter
    {
        private readonly DatabaseContext _databaseContext;

        public PostgreSQLAdapter(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public List<Product> GetProducts(int limit)
        {
            return _databaseContext.Products.FromSqlRaw($"Select * from \"Products\" LIMIT {limit}").ToList();
        }

        public List<User> GetUsers()
        {
            return _databaseContext.Users.FromSqlRaw("Select * from \"Users\" ").ToList();
        }
    }
}
