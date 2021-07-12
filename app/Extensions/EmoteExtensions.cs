
using System.Threading.Tasks;
using Emotify.Models;
using Microsoft.AspNetCore.Identity;

namespace Emotify.Extensions
{
    public static class EmoteExtensions
    {
        public async static Task<bool> CanModify(this Emote emote, EmotifyUser user, RoleManager<EmotifyUser> manager)
        {
            if (user.Id == emote.OwnerUserId) {
                return true;
            }
            var claims = await manager.GetClaimsAsync(user);
            // if (claims.Contains(Claim)) {
            //     return true;
            // }
            return false;
        }
    }
}