using System.Linq;
using Emotify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace Emotify.Authorization
{
    public class DiscordGuildEnrollmentAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, SocketGuild>
    {
        UserManager<EmotifyUser> _userManager;

        public DiscordGuildEnrollmentAuthorizationHandler(UserManager<EmotifyUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, SocketGuild resource)
        {
            if (resource == null || requirement != DiscordOperations.EnrollGuild) 
            {
                return;
            }

            var user = await _userManager.GetUserAsync(context.User);
            bool hasEmojiPerms = resource.Users
                .Where(u => u.Id == user.VerifiedDiscordId)
                .Any(u => u.GuildPermissions.ManageEmojis);
            if (hasEmojiPerms)
            {
                context.Succeed(requirement);
            }
        }
    }
}