using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UrbanIndicatorsSystem.Pages
{
    [Authorize] 
    public class Subroutine1Model : PageModel
    {
        public void OnGet() { }
    }
}