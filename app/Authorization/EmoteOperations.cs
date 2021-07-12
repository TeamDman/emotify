using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Emotify.Authorization
{
    public static class EmoteOperations
    {
        public static OperationAuthorizationRequirement Modify = new OperationAuthorizationRequirement { Name = nameof(Modify) };
    }

}