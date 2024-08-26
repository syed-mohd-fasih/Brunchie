using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Brunchie.Areas.Identity.Data;

// Add profile data for application users by adding properties to the BrunchieUser class
public class BrunchieUser : IdentityUser
{
    // Common properties for all users
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }

    // Student-specific properties
    public bool IsStudent { get; set; }
    public string StudentUniId { get; set; } = string.Empty;
    public string University { get; set; } = string.Empty;

    // Vendor-specific properties
    public bool IsVendor { get; set; }
    public string VendorName { get; set; } = string.Empty;
    public string VendorAddress { get; set; } = string.Empty;

    // Admin-specific properties
    public bool IsAdmin { get; set; }
    public DateTime HireDate { get; set; }

    // Additional properties for all users
    public string ProfilePictureUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; }
}

