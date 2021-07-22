using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Emotify.Extensions;
using Emotify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Emotify.Pages.Servers
{
    public record GuildPair(
        EnrolledGuild Enrollment,
        IGuild Guild
    );

    public class IndexModel : PageModel
    {
        public IndexModel(
            EmotifyDbContext context,
            UserManager<EmotifyUser> userManager,
            DiscordSocketClient discordSocketClient
        )
        {
            Context = context;
            UserManager = userManager;
            DiscordClient = discordSocketClient;
        }


        public IList<GuildPair> Guilds { get; set; }

        // Filters
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        [DisplayName("Show mine")]
        public bool ShowMine { get; set; } = true;


        [BindProperty]
        public IList<int> SelectedGuildIds { get; set; }

        public EmotifyDbContext Context { get; }
        public UserManager<EmotifyUser> UserManager { get; }
        public DiscordSocketClient DiscordClient { get; }

        public async Task OnGetAsync()
        {
            // Get emotes and their images
            var guildQuery =
                from e in Context.EnrolledGuilds.AsAsyncEnumerable()
                join g in DiscordClient.Guilds.AsAsyncEnumerable() on e.Id equals g.Id
                select new GuildPair(e, g);
            // If search query provided, filter results
            if (!string.IsNullOrEmpty(SearchString))
                guildQuery = guildQuery.Where(x => x.Guild.Name.Contains(SearchString));

            // If not showing mine, filter out mine
            if (!ShowMine) guildQuery = guildQuery.Where(x => x.Enrollment.OwnerUserId != UserManager.GetUserId(User));

            // Return result list
            Guilds = await guildQuery.ToListAsync();
        }

        public async Task<IActionResult> OnPostCopyAsync()
        {
            if (!ModelState.IsValid) return Page();

            return RedirectToPage("./Index");
        }
    }
}