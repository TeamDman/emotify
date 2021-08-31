using System.Linq;
using Emotify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Emotify.Services;

namespace Emotify.Authorization.Discord
{
    public class ManageGuildEmotesAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, IGuild>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, IGuild resource)
        {
            if (resource == null || requirement != DiscordOperations.ManageGuildEmotes) 
            {
                return;
            }

            var userId = UserHelper.GetUserId(context.User);
            var guildUser = await resource.GetUserAsync(userId);
            bool hasEmojiPerms = guildUser.GuildPermissions.ManageEmojis;
            if (hasEmojiPerms)
            {
                context.Succeed(requirement);
            }
        }
    }
}