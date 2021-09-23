using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Emotify.Models;
using Emotify.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Emote = Emotify.Models.Emote;

namespace Emotify.Pages
{
    public class IndexModel : PageModel
    {
        private readonly EmotifyDbContext _context;
        private readonly DiscordSocketClient _discordClient;
        private readonly UserHelper _userHelper;
        private readonly UserGuildStore _userGuildStore;
        private readonly IConfiguration _config;
        private readonly IAuthorizationService _auth;

        public IndexModel(
            IConfiguration configuration,
            EmotifyDbContext context,
            DiscordSocketClient discordSocketClient,
            IAuthorizationService authorizationService,
            UserHelper userHelper,
            UserGuildStore userGuildStore
        )
        {
            _config = configuration;
            _context = context;
            _discordClient = discordSocketClient;
            _auth = authorizationService;
            _userHelper = userHelper;
            _userGuildStore = userGuildStore;
        }

        public string EnrollUrl
        {
            get
            {
                var clientId = _config["Discord:ClientId"];
                var permissions = new GuildPermissions(
                    manageEmojis: true,
                    manageGuild: true,
                    createInstantInvite: true,
                    administrator: true
                );
                return
                    $"https://discord.com/api/oauth2/authorize?client_id={clientId}&scope=bot&permissions={permissions.RawValue}";
            }
        }

        public IList<IGuild> Guilds { get; set; }

        public IList<Emote> Emotes { get; set; }
        
        public User user { get; set; }

        public async Task<IActionResult> OnGet()
        {
            user = await _userHelper.GetOrCreateUser(User);
            if (user == null)
            {
                return Page();
            }
            var userGuilds = await _userGuildStore.GetGuilds(user)
                .Select(g => UInt64.Parse(g.id))
                .ToListAsync();

            Guilds = _discordClient.Guilds.AsEnumerable()
                .Where(g => userGuilds.Contains(g.Id))
                .Cast<IGuild>()
                .OrderBy(g => g.Name)
                .ToList();

            Emotes = await _context.Emotes.AsQueryable()
                .Where(e => e.Owner == user)
                .Include(e => e.EmoteImage)
                .ToListAsync();
            return Page();
        }
    }
}