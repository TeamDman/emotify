using Emotify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Emotify.Services;

namespace Emotify.Authorization
{
    public class EmoteAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Emote>
    {
        private readonly UserHelper _userHelper;

        public EmoteAuthorizationHandler(
            UserHelper userHelper
        )
        {
            _userHelper = userHelper;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Emote resource)
        {
            if (context.User == null || resource == null) 
            {
                return Task.CompletedTask;
            }

            if (requirement != EmoteOperations.Modify)
            {
                return Task.CompletedTask;
            }

            if (resource.OwnerUserId == _userHelper.GetUserId(context.User))
            {
                context.Succeed(requirement);
            }

            // if (_userManager.IsInRoleAsync()) // admin?
            // {
            //     // https://github.com/dotnet/AspNetCore.Docs/issues/8502
            // }

            return Task.CompletedTask;
        }
    }
}