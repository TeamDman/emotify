using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Discord;
using Emotify.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Emotify.Services
{
    public class UserHelper
    {
        private readonly EmotifyDbContext _context;

        public UserHelper(EmotifyDbContext dbContext)
        {
            _context = dbContext;
        }

        public ulong GetUserId(
            ClaimsPrincipal claimsPrincipal
        )
        {
            // Get Discord ID from claims
            ulong id = Convert.ToUInt64(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
            return id;
        }

        public bool IsSignedIn(
            ClaimsPrincipal claimsPrincipal
        )
        {
            return GetUserId(claimsPrincipal) != 0;
        }
        public string GetDisplayName(
            ClaimsPrincipal claimsPrincipal
        )
        {
            var name = claimsPrincipal.FindFirstValue(ClaimTypes.Name);
            return name;
        }
        
        public async Task<User> GetOrCreateUser(
            ClaimsPrincipal claimsPrincipal
        )
        {
            // Get Discord ID from claims
            ulong Id = GetUserId(claimsPrincipal);
            if (Id == 0)
            {
                // No id, return null
                return null;
            }

            // Discover existing user
            var user = await _context.Users.FindAsync(Id);
            if (user == null)
            {
                // No user exists, create new one
                user = new User()
                {
                    Id = Id,
                    Email = claimsPrincipal.FindFirstValue(ClaimTypes.Email),
                    Name = claimsPrincipal.FindFirstValue(ClaimTypes.Name),
                    Discriminator = Convert.ToUInt16(
                        claimsPrincipal.FindFirstValue(DiscordAuthenticationConstants.Claims.Discriminator)
                    ),
                    AvatarHash = claimsPrincipal.FindFirstValue(DiscordAuthenticationConstants.Claims.AvatarHash),
                    AvatarUrl = claimsPrincipal.FindFirstValue(DiscordAuthenticationConstants.Claims.AvatarUrl)
                };
                
                // Persist user in DB
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }

            return user;
        }
    }
}