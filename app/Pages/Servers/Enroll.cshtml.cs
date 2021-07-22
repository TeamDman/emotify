using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Emotify.Authorization;
using Emotify.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Emotify.Pages.Servers
{
    public class Enroll : PageModel
    {
        public Enroll(
            EmotifyDbContext context,
            DiscordSocketClient discordSocketClient,
            IAuthorizationService authorizationService
        )
        {
            Context = context;
            DiscordClient = discordSocketClient;
            AuthorizationService = authorizationService;
        }

        public IAuthorizationService AuthorizationService { get; }

        public EmotifyDbContext Context { get; }

        public DiscordSocketClient DiscordClient { get; }

        public IList<SocketGuild> Guilds { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var enrolledIds = await Context.EnrolledGuilds.AsAsyncEnumerable().Select(e => e.Id).ToHashSetAsync();
            var guilds = DiscordClient.Guilds.AsEnumerable().Where(guild => !enrolledIds.Contains(guild.Id));
            var authorizedGuilds = await guilds.WhereAuthorizedAsync(
                User,
                AuthorizationService,
                DiscordOperations.EnrollGuild
            );
            Guilds = authorizedGuilds.OrderBy(guild => guild.Name).ToList();
            return Page();
        }
    }
}