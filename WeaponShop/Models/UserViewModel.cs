using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace WeaponShop.Models
{
    public class UserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public IFormFile id_card { get; set; }
        public IFormFile ccw { get; set; }
    }
}
