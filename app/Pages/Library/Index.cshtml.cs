using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Emotify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Emotify.Pages.Library
{
    public class IndexModel : PageModel
    {
        public IndexModel(
            EmotifyDbContext context,
            UserManager<EmotifyUser> userManager,
            DiscordSocketClient discordClient
        )
        {
            Context = context;
            UserManager = userManager;
            DiscordClient = discordClient;
        }


        public IList<Emote> Emotes { get; set; }

        // Filters
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool ShowMine { get; set; } = true;


        [BindProperty]
        public IList<int> SelectedEmoteIds { get; set; }

        public EmotifyDbContext Context { get; }
        public UserManager<EmotifyUser> UserManager { get; }
        public DiscordSocketClient DiscordClient { get; }

        public async Task OnGetAsync()
        {
            // Get emotes and their images
            var emoteQuery =
                from e in Context.Emotes.Include(e => e.EmoteImage)
                select e;
            // If search query provided, filter results
            if (!string.IsNullOrEmpty(SearchString)) emoteQuery = emoteQuery.Where(e => e.Name.Contains(SearchString));
            if (!ShowMine) emoteQuery = emoteQuery.Where(e => e.OwnerUserId != UserManager.GetUserId(User));
            // Return result list
            Emotes = await emoteQuery.ToListAsync();
        }

        public async Task<IActionResult> OnPostCopyAsync()
        {
            if (!ModelState.IsValid) return Page();
            return RedirectToPage("./Index");
        }
    }
}