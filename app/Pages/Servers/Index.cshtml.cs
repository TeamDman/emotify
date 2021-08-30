using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Emotify.Extensions;
using Emotify.Models;
using Emotify.Services;
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
        
        private readonly EmotifyDbContext _context;
        private readonly DiscordSocketClient _discordClient;
        public IndexModel(
            EmotifyDbContext context,
            DiscordSocketClient discordSocketClient
        )
        {
            _context = context;
            _discordClient = discordSocketClient;
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


        public async Task OnGetAsync()
        {
            // Get emotes and their images
            var guildQuery =
                from e in _context.EnrolledGuilds.AsAsyncEnumerable()
                join g in _discordClient.Guilds.AsAsyncEnumerable() on e.Id equals g.Id
                select new GuildPair(e, g);
            // If search query provided, filter results
            if (!string.IsNullOrEmpty(SearchString))
                guildQuery = guildQuery.Where(x => x.Guild.Name.Contains(SearchString));

            // If not showing mine, filter out mine
            if (!ShowMine)
            {
                var userId = UserHelper.GetUserId(User);
                guildQuery = guildQuery.Where(x => x.Enrollment.OwnerUserId != userId);
            }

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