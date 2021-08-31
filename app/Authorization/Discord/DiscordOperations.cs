using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Emotify.Authorization.Discord
{
    public static class DiscordOperations
    {
        public static OperationAuthorizationRequirement ManageGuildEmotes = new OperationAuthorizationRequirement { Name = nameof(ManageGuildEmotes) };
    }

}