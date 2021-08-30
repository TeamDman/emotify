using System.Threading.Tasks;
using Emotify.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Emotify.Pages
{
    [Authorize]
    public class Login : PageModel
    {
        public IActionResult OnGet(string returnUrl = "/Index")
        {
            return Redirect(returnUrl);
        }
    }
}