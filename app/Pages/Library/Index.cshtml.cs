using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Emotify.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Emote = Emotify.Models.Emote;

namespace Emotify.Pages.Library
{
    public class IndexModel : PageModel
    {
        
        private readonly EmotifyDbContext _context;
        private readonly DiscordSocketClient _discordClient;
        public IndexModel(
            EmotifyDbContext context,
            DiscordSocketClient discordClient
        )
        {
            _context = context;
            _discordClient = discordClient;
        }


        public IList<Emote> Emotes { get; set; }
        public IList<IGuild> Guilds { get; set; }

        // Filters
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool ShowMine { get; set; } = true;


        [BindProperty]
        public IList<int> SelectedEmoteIds { get; set; }


        public async Task OnGetAsync()
        {
            Guilds = _discordClient.Guilds
                .OrderBy(g => g.Name)
                .Cast<IGuild>()
                .ToList();
            
            // Get emotes and their images
            var emoteQuery =
                from e in _context.Emotes.Include(e => e.EmoteImage)
                select e;
            
            // If search query provided, filter results
            if (!string.IsNullOrEmpty(SearchString)) emoteQuery = emoteQuery.Where(e => e.Name.Contains(SearchString));
            if (!ShowMine)
            {
                var userId = UserHelper.GetUserId(User);
                emoteQuery = emoteQuery.Where(e => e.OwnerUserId != userId);
            }
            
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