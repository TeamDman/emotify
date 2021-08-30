using System.Threading.Tasks;
using Emotify.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Emotify.Pages
{
    public class Logout : PageModel
    {
        private readonly ILogger<Logout> _logger;

        public Logout(ILogger<Logout> logger)
        {
            _logger = logger;
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            var name = UserHelper.GetDisplayName(User);
            _logger.LogInformation("Logging out {Name}", name);
            await HttpContext.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}