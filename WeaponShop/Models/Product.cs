using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WeaponShop.Models;

public partial class Product
{
    public int Id { get; set; }

    public string? ProductName { get; set; }

    public string? ProductDescription { get; set; }

    public string? ProductType { get; set; }
    public string? ProductSubtype { get; set; }

    public double? ProductPrice { get; set; }

    public string? Caliber { get; set; }
    public string? Image { get; set; }

    public bool under_licence { get; set; }
}
