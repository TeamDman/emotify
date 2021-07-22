using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Emotify.Authorization
{
    public static class DiscordOperations
    {
        public static OperationAuthorizationRequirement EnrollGuild = new OperationAuthorizationRequirement { Name = nameof(EnrollGuild) };
    }

}