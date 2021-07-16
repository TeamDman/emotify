using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Emotify.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Emotify.Pages.Emotes
{

    public class IndexModel : EmotifyBasePageModel
    {

        public IndexModel(
            EmotifyDbContext context,
            IAuthorizationService authorizationService,
            UserManager<EmotifyUser> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        public IList<Emote> Emotes { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync()
        {
            // Get emotes and their images
            var emoteQuery =
                from e in Context.Emotes.Include(e => e.EmoteImage)
                select e;

            // If search query provided, filter results
            if (!string.IsNullOrEmpty(SearchString))
            {
                emoteQuery = emoteQuery.Where(e => e.Name.Contains(SearchString));
            }

            // Return result list
            Emotes = await emoteQuery.ToListAsync();
        }
    }
}
