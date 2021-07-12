using Emotify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Emotify.Pages
{
    public class EmotifyBasePageModel : PageModel
    {
        protected EmotifyDbContext Context { get; }
        protected IAuthorizationService AuthorizationService { get; }
        protected UserManager<EmotifyUser> UserManager { get; }

        public EmotifyBasePageModel(
            EmotifyDbContext context,
            IAuthorizationService authorizationService,
            UserManager<EmotifyUser> userManager
        ) : base()
        {
            Context = context;
            AuthorizationService = authorizationService;
            UserManager = userManager;
        }
    }
}