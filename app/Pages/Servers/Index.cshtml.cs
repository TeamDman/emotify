using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Emotify.Authorization.Discord;
using Emotify.Extensions;
using Emotify.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Emotify.Pages.Servers
{
    public class IndexModel : PageModel
    {
        private readonly IAuthorizationService _auth;

        private readonly EmotifyDbContext _context;
        private readonly DiscordSocketClient _discordClient;
        private readonly UserHelper _userHelper;
        private readonly UserGuildStore _userGuildStore;

        public IndexModel(
            EmotifyDbContext context,
            DiscordSocketClient discordSocketClient,
            IAuthorizationService authorizationService,
            UserHelper userHelper,
            UserGuildStore userGuildStore
        )
        {
            _context = context;
            _discordClient = discordSocketClient;
            _auth = authorizationService;
            _userHelper = userHelper;
            _userGuildStore = userGuildStore;
        }


        public IList<IGuild> MyGuilds { get; set; }
        public IList<IGuild> OtherGuilds { get; set; }

        // Filters
        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }

        public async Task OnGetAsync()
        {
            // Get emotes and their images
            var guildQuery = _discordClient.Guilds.AsQueryable();
            // If search query provided, filter results
            if (!string.IsNullOrEmpty(Search))
                guildQuery = guildQuery.Where(x => x.Name.Contains(Search));

            // Get user
            var user = await _userHelper.GetOrCreateUser(User);

            // Get user guilds from OAuth
            var userGuildIds = await _userGuildStore.GetGuilds(user).Select(g => UInt64.Parse(g.id)).ToListAsync();

            // Get bot guilds matching user guilds
            MyGuilds = guildQuery.AsEnumerable().Where(g => userGuildIds.Contains(g.Id)).Cast<IGuild>().ToList();

            // Get rest of guilds
            OtherGuilds = guildQuery.AsEnumerable().Except(MyGuilds).ToList();

            // // If not showing mine, filter out mine
            // if (!ShowMine)
            // {
            //     Guilds = await Guilds.ExceptAuthorizedAsync(User, _auth, DiscordOperations.ManageGuildEmotes);
            // }
        }

        public IActionResult OnPostCopyAsync()
        {
            if (!ModelState.IsValid) return Page();

            return RedirectToPage("./Index");
        }
    }
}