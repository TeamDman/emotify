using System.Linq;
using Emotify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Discord.WebSocket;
using Emotify.Services;

namespace Emotify.Authorization
{
    public class DiscordGuildEnrollmentAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, SocketGuild>
    {
        private readonly UserHelper _userHelper;

        public DiscordGuildEnrollmentAuthorizationHandler(
            UserHelper userHelper
        )
        {
            _userHelper = userHelper;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, SocketGuild resource)
        {
            if (resource == null || requirement != DiscordOperations.EnrollGuild) 
            {
                return;
            }

            var userId = _userHelper.GetUserId(context.User);
            bool hasEmojiPerms = resource.Users
                .Where(u => u.Id == userId)
                .Any(u => u.GuildPermissions.ManageEmojis);
            if (hasEmojiPerms)
            {
                context.Succeed(requirement);
            }
        }
    }
}