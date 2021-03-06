using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Emotify.Models;
using Emotify.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Emotify.Extensions
{
    public static class BuiltinExtensions
    {
        // https://stackoverflow.com/a/14895226/11141271
        public static async Task<IList<T>> WhereParallelAsync<T>(
            this IEnumerable<T> source,
            Func<T, Task<bool>> predicate
        )
        {
            var results = new ConcurrentQueue<T>();
            var tasks = source.Select(
                async item =>
                {
                    if (await predicate(item))
                        results.Enqueue(item);
                }
            );
            await Task.WhenAll(tasks);
            return results.ToList();
        }

        public static async Task<IList<T>> WhereAuthorizedAsync<T>(
            this IEnumerable<T> source,
            ClaimsPrincipal user,
            IAuthorizationService authorizationService,
            IAuthorizationRequirement requirement
        )
        {
            return await source.WhereParallelAsync(
                async item =>
                {
                    var result = await authorizationService.AuthorizeAsync(user, item, requirement);
                    return result.Succeeded;
                }
            );
        }
        
        public static async Task<IList<T>> ExceptAuthorizedAsync<T>(
            this IEnumerable<T> source,
            ClaimsPrincipal user,
            IAuthorizationService authorizationService,
            IAuthorizationRequirement requirement
        )
        {
            return await source.WhereParallelAsync(
                async item =>
                {
                    var result = await authorizationService.AuthorizeAsync(user, item, requirement);
                    return !result.Succeeded;
                }
            );
        }

        // public static async Task<IdentityResult> MigrateDataAndDelete(
        //     this UserHelper userHelper,
        //     User user,
        //     EmotifyDbContext context
        // )
        // {
        //     var placeholderUser = new User { UserName = "DeletedUser" };
        //     await userHelper.CreateAsync(placeholderUser);
        //     IQueryable<UserOwnable>[] toMove = { context.Emotes.AsQueryable(), context.EnrolledGuilds };
        //     foreach (var items in toMove)
        //     {
        //         await items.ForEachAsync(item =>
        //         {
        //             item.Owner = placeholderUser;
        //             item.OwnerUserId = placeholderUser.Id;
        //             context.Entry(item).State = EntityState.Modified;
        //         });
        //     }
        //
        //     await context.SaveChangesAsync();
        //     
        //     return await userHelper.DeleteAsync(user);
        // }
    }
}