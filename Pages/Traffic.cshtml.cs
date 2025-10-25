using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _.Pages
{
    [Authorize]
    public class TrafficModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}