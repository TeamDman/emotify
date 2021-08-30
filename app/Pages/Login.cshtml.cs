using System.Threading.Tasks;
using Emotify.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SQLitePCL;

namespace Emotify.Pages
{
    [Authorize]
    public class Login : PageModel
    {
        private readonly UserHelper _userHelper;
        private readonly EmotifyDbContext _context;

        public Login(
            UserHelper userHelper,
            EmotifyDbContext dbContext
        )
        {
            _userHelper = userHelper;
            _context = dbContext;
        }

        public async Task<IActionResult> OnGet(string returnUrl = "/Index")
        {
            // Get or create user
            var user = await _userHelper.GetOrCreateUser(User);
            
            // Acquire discord token
            var auth = await HttpContext.AuthenticateAsync("scheme.discord");
            var token  = auth.Properties.Items[".Token.access_token"];
            
            // Update user token field
            if (user.AccessToken != token)
            {
                user.AccessToken = token;
                _context
                    .Entry(user)
                    .Property(nameof(user.AccessToken))
                    .IsModified = true;
                await _context.SaveChangesAsync();
            }
            
            // Redirect to return page
            return Redirect(returnUrl);
        }
    }
}