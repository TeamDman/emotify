using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Emotify.Models;

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