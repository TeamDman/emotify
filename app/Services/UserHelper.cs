using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Discord;
using Emotify.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Emotify.Services
{
    public class UserHelper
    {
        private readonly EmotifyDbContext _context;

        public UserHelper(EmotifyDbContext dbContext)
        {
            _context = dbContext;
        }

        public static ulong GetUserId(
            ClaimsPrincipal claimsPrincipal
        )
        {
            // Get Discord ID from claims
            ulong id = Convert.ToUInt64(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
            return id;
        }
        
        public static bool IsSignedIn(
            ClaimsPrincipal claimsPrincipal
        )
        {
            return GetUserId(claimsPrincipal) != 0;
        }
        public static string GetDisplayName(
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

        public static async Task<T> GetOAuthData<T>(User user, string scope)
        {
            var request = WebRequest.CreateHttp($"https://discord.com/api/{scope}");
            request.Accept = "application/json";
            request.Headers["Authorization"] = $"Bearer {user.AccessToken}";

            var response = (HttpWebResponse) await request.GetResponseAsync();
            using var reader = new StreamReader(response.GetResponseStream());
            var content = await reader.ReadToEndAsync();
            // var data = await JsonDocument.ParseAsync<T>(response.GetResponseStream());
            var data = JsonSerializer.Deserialize<T>(content);
            return data;
            // using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            // request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            //
            // using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            // if (!response.IsSuccessStatusCode)
            // {
            //     Logger<>.LogError("An error occurred while retrieving the user profile: the remote server " +
            //                       "returned a {Status} response with the following payload: {Headers} {Body}.",
            //         /* Status: */ response.StatusCode,
            //         /* Headers: */ response.Headers.ToString(),
            //         /* Body: */ await response.Content.ReadAsStringAsync(Context.RequestAborted));
            //
            //     throw new HttpRequestException("An error occurred while retrieving the user profile.");
            // }
            //
            // using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync(Context.RequestAborted));
            //
            // var principal = new ClaimsPrincipal(identity);
            // var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
            // context.RunClaimActions();
            //
            // await Options.Events.CreatingTicket(context);
            // return new AuthenticationTicket(context.Principal!, context.Properties, Scheme.Name);

        }
    }
}