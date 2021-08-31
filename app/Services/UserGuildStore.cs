using System.Collections.Generic;
using System.Threading.Tasks;
using Emotify.Data.Deserialization;
using Emotify.Models;

namespace Emotify.Services
{
    public class UserGuildStore
    {
        private readonly Dictionary<ulong, UserGuild[]> _cache = new();
        
        public async IAsyncEnumerable<UserGuild> GetGuilds(
            User user,
            bool forceRefresh = false
        )
        {
            if (forceRefresh || !_cache.ContainsKey(user.Id))
            {
                var guilds = await UserHelper.GetOAuthData<UserGuild[]>(user, "users/@me/guilds");
                _cache[user.Id] = guilds;
            }

            foreach (var guild in _cache[user.Id])
            {
                yield return guild;
            }
        }
    }
}