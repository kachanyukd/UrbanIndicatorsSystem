using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UrbanIndicatorsSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(500)]
        public string? FullName { get; set; }
    }
}