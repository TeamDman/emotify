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
        private readonly UserHelper _userHelper;

        public Logout(ILogger<Logout> logger, UserHelper userHelper)
        {
            _logger = logger;
            _userHelper = userHelper;
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            var name = _userHelper.GetDisplayName(User);
            _logger.LogInformation("Logging out {Name}", name);
            await HttpContext.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}