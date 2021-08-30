
using System.Linq;
using System.Threading.Tasks;
using Emotify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Emotify.Extensions
{
    public static class EmoteExtensions
    {
        public async static Task<bool> CanModify(this Emote emote, User user, RoleManager<User> manager)
        {
            if (user.Id == emote.OwnerUserId)
            {
                return true;
            }
            var claims = await manager.GetClaimsAsync(user);
            // if (claims.Contains(Claim)) {
            //     return true;
            // }
            return false;
        }

        public async static Task<EmoteImage> FindExistingOrDefault(this EmoteImage image, EmotifyDbContext context)
        {
            EmoteImage found = await context.EmoteImages.AsAsyncEnumerable().Where(i => i.Hash == image.Hash).FirstOrDefaultAsync();
            return found ?? image;
        }
    }
}