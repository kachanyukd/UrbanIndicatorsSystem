using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UrbanIndicatorsSystem.Models
{
    // Додаємо кастомні поля до IdentityUser
    public class ApplicationUser : IdentityUser
    {
        [StringLength(500)]
        public string? FullName { get; set; }
    }
}