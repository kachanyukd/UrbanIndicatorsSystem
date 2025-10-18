using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _.Pages
{
    [Authorize] // <--- Закриваємо сторінку від неавторизованих
    public class TrafficModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}