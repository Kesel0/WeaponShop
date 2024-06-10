using System;
using System.Collections.Generic;

namespace WeaponShop.Models;

public partial class User
{
    public int Id{ get; set; }

    public string? Email { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }
    public bool isadmin { get; set; }

    public string? id_card { get; set; }
    public string? ccw {  get; set; }
    
}
