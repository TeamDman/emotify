using System.Collections.Generic;
using System.Threading.Tasks;

namespace Emotify.Extensions
{
    public static class DiscordExtensions
    {
        public static async IAsyncEnumerable<T> AsAsyncEnumerable<T>(
            this IReadOnlyCollection<T> collection
        )
        {
            foreach (var item in collection)
            {
                yield return await Task.FromResult(item);
            }
        }
    }
}