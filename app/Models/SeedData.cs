using Emotify.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
// using Emotify.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Emotify.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new EmotifyDbContext(serviceProvider.GetRequiredService<DbContextOptions<EmotifyDbContext>>()))
            {

                // var admin = await EnsureUser(serviceProvider, "f9qNO70pg$FkiFOR2hxiUGId", "admin@example.com");
                // await EnsureRole(serviceProvider, admin.Id, EmoteOperations.Modify.Name);
                // if (context.Emotes.Any())
                // {
                //     return;
                // }
                // context.Emotes.Add(
                //     new Emote()
                //     {
                //         OwnerUserId = admin.Id,
                //         Name = "Dan"
                //     }
                // );
                // await context.SaveChangesAsync();
            }
        }

        // private static async Task<User> EnsureUser(IServiceProvider serviceProvider, string testUserPw, string UserName)
        // {
        //     var userManager = serviceProvider.GetService<UserManager<User>>();
        //
        //     var user = await userManager.FindByNameAsync(UserName);
        //     if (user == null)
        //     {
        //         user = new User
        //         {
        //             UserName = UserName,
        //             EmailConfirmed = true
        //         };
        //         await userManager.CreateAsync(user, testUserPw);
        //     }
        //
        //     if (user == null)
        //     {
        //         throw new Exception("The password is probably not strong enough!");
        //     }
        //
        //     return user;
        // }
        //
        // private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider, string uid, string role)
        // {
        //     IdentityResult IR = null;
        //     var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
        //
        //     if (roleManager == null)
        //     {
        //         throw new Exception("roleManager null");
        //     }
        //
        //     if (!await roleManager.RoleExistsAsync(role))
        //     {
        //         IR = await roleManager.CreateAsync(new IdentityRole(role));
        //     }
        //
        //     var userManager = serviceProvider.GetService<UserManager<User>>();
        //
        //     var user = await userManager.FindByIdAsync(uid);
        //
        //     if (user == null)
        //     {
        //         throw new Exception("The testUserPw password was probably not strong enough!");
        //     }
        //
        //     IR = await userManager.AddToRoleAsync(user, role);
        //
        //     return IR;
        // }
    }
}