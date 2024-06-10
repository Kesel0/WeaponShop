using Microsoft.AspNetCore.Mvc;
using WeaponShop.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace WeaponShop.Controllers
{
    /*public class AccountController : Controller
    {
        private readonly DatabaseContext _databaseContext;

        public AccountController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
                return View(model);
            }

            if (_databaseContext.Users.Any(u => u.Username == model.Username || u.Email == model.Email))
            {
                ModelState.AddModelError(string.Empty, "Пользователь с таким именем или email уже существует");
                return View(model);
            }

            var user = new User
            {
                Email = model.Email,
                Username = model.Username,
                Password = HashPassword(model.Password),
            };

            if (model.id_card != null && model.ccw != null)
            {
                var imagePath1 = SaveImage(model.id_card);
                user.id_card = imagePath1;

                var imagePath2 = SaveImage(model.ccw);
                user.ccw = imagePath2;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Необходимо загрузить оба изображения.");
                return View(model);
            }

            _databaseContext.Users.Add(user);
            _databaseContext.SaveChanges();

            return RedirectToAction("Login");
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                var user = _databaseContext.Users.FirstOrDefault(u => u.Username == model.Username || u.Email == model.Email);

                if (user != null && VerifyPassword(model.Password, user.Password))
                {
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("Username", user.Username);
                    return RedirectToAction("Index", "DBshow");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Неверные учетные данные");
                }
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }

        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            var parts = storedPassword.Split(':');
            var salt = Convert.FromBase64String(parts[0]);
            var storedHash = Convert.FromBase64String(parts[1]);

            var enteredHash = KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            return enteredHash.SequenceEqual(storedHash);
        }

        private string SaveImage(IFormFile image)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userdocs");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }

            return "/images/" + uniqueFileName;
        }
    }*/

    public class AccountController : Controller
    {
        public interface IUserFactory
        {
            User CreateUser(UserViewModel model);
        }
        public class UserFactory : IUserFactory
        {
            private readonly IHttpContextAccessor _httpContextAccessor;

            public UserFactory(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }

            public User CreateUser(UserViewModel model)
            {
                var user = new User
                {
                    Email = model.Email,
                    Username = model.Username,
                    Password = HashPassword(model.Password),
                };

                if (model.id_card != null && model.ccw != null)
                {
                    var imagePath1 = SaveImage(model.id_card);
                    user.id_card = imagePath1;

                    var imagePath2 = SaveImage(model.ccw);
                    user.ccw = imagePath2;
                }

                return user;
            }

            private string HashPassword(string password)
            {
                byte[] salt = new byte[16];
                using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                string hashed = Convert.ToBase64String(Microsoft.AspNetCore.Cryptography.KeyDerivation.KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: Microsoft.AspNetCore.Cryptography.KeyDerivation.KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

                return $"{Convert.ToBase64String(salt)}:{hashed}";
            }

            private string SaveImage(IFormFile image)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userdocs");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }

                return "/images/" + uniqueFileName;
            }
        }
        private readonly DatabaseContext _databaseContext;
        private readonly IUserFactory _userFactory;

        public AccountController(DatabaseContext databaseContext, IUserFactory userFactory)
        {
            _databaseContext = databaseContext;
            _userFactory = userFactory;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
                return View(model);
            }

            if (_databaseContext.Users.Any(u => u.Username == model.Username || u.Email == model.Email))
            {
                ModelState.AddModelError(string.Empty, "Пользователь с таким именем или email уже существует");
                return View(model);
            }

            var user = _userFactory.CreateUser(model);

            if (user.id_card == null || user.ccw == null)
            {
                ModelState.AddModelError(string.Empty, "Необходимо загрузить оба изображения.");
                return View(model);
            }

            _databaseContext.Users.Add(user);
            _databaseContext.SaveChanges();

            return RedirectToAction("Login");
        }
        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            var parts = storedPassword.Split(':');
            var salt = Convert.FromBase64String(parts[0]);
            var storedHash = Convert.FromBase64String(parts[1]);

            var enteredHash = KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            return enteredHash.SequenceEqual(storedHash);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                var user = _databaseContext.Users.FirstOrDefault(u => u.Username == model.Username || u.Email == model.Email);

                if (user != null && VerifyPassword(model.Password, user.Password))
                {
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("Username", user.Username);
                    return RedirectToAction("Index", "DBshow");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Неверные учетные данные");
                }
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
