using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Threading.Tasks;
using Emotify.Models;
using Microsoft.Extensions.Configuration;
using OAuth;

namespace Emotify.Pages
{
    [Authorize]
    public class SecureModel : PageModel
    {
        private readonly ILogger<SecureModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly UserManager<EmotifyUser> _userManager;

        public SecureModel(ILogger<SecureModel> logger, IConfiguration configuration, UserManager<EmotifyUser> userManager)
        {
            _logger = logger;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async void OnGetAsync()
        {
            // read external identity from the temporary cookie
            var result = await HttpContext.AuthenticateAsync();
            // var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (result?.Succeeded != true)
            {
                Page();
                return;
                // throw new Exception("External authentication error");
            }

            // retrieve claims of the external user
            var externalUser = result.Principal;
            if (externalUser == null)
            {
                Page();
                return;
                // throw new Exception("External authentication error");
            }

            // retrieve claims of the external user
            var claims = externalUser.Claims.ToList();

            // try to determine the unique id of the external user - the most common claim type for that are the sub claim and the NameIdentifier
            // depending on the external provider, some other claim type might be used
            // var userIdClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Ident.Subject);
            // if (userIdClaim == null)
            // {
                var userIdClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            // }
            if (userIdClaim == null)
            {
                Page();
                return;
            }

            return;
            {
                EmotifyUser user = await _userManager.GetUserAsync(User);
                string ConsumerKey = _configuration["Discord:ClientId"];
                string ConsumerToken = _configuration["Discord:ClientSecret"];
                string AccessToken = "";
                string AccessTokenSecret = await _userManager.GetAuthenticationTokenAsync(user, "discord", "AuthenticatorKey");
                string RequestURL = "https://discord.com/api/users/@me/guilds";
                OAuthRequest client = OAuthRequest.ForProtectedResource("GET", ConsumerKey, ConsumerToken, AccessToken, AccessTokenSecret);
                client.RequestUrl = RequestURL;
                string auth = client.GetAuthorizationHeader();
                HttpWebRequest request = WebRequest.CreateHttp(client.RequestUrl);
                request.Headers.Add("Authorization", auth);
                Console.WriteLine("Calling " + RequestURL);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string strResponse = reader.ReadToEnd();
                Console.WriteLine("Got response");
                Console.WriteLine(strResponse);
            }
        }
    }
}
