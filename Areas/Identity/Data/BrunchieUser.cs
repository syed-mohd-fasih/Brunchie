using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using Brunchie.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Brunchie.Areas.Identity.Data;

public class BrunchieUser : IdentityUser
{
    public DateTime DateCreated { get; set; }
    public Location? UserLocation { get; set; }
}

public class Vendor : BrunchieUser
{
    public Menu Menu { get; set; }
    public int rating { get; set; }
}

public class Student : BrunchieUser
{
    public string Institute {  get; set; } = string.Empty;
    public List<Vendor> FavoriteVendors { get; set; }
    public List<Order> OrderHistory { get; set; }
}

