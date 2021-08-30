using System.Collections.Generic;
using System.Threading.Tasks;
using Emotify.Data.Deserialization;
using Emotify.Models;
using Emotify.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Emotify.Pages
{
    [Authorize]
    public class SecureModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SecureModel> _logger;
        private readonly UserHelper _userHelper;
        private readonly UserGuildStore _userGuildStore;

        public SecureModel(
            ILogger<SecureModel> logger,
            IConfiguration configuration,
            UserHelper userHelper,
            UserGuildStore userGuildStore
        )
        {
            _logger = logger;
            _configuration = configuration;
            _userHelper = userHelper;
            _userGuildStore = userGuildStore;
        }
        
        public UserGuild[] Guilds { get; set; }

        public async Task OnGetAsync()
        {
            // read external identity from the temporary cookie
            var context = await HttpContext.AuthenticateAsync("scheme.discord");
            if (context.Succeeded != true)
            {
                Page();
                return;
            }

            var user = await _userHelper.GetOrCreateUser(User);
            Guilds = await _userGuildStore.GetGuilds(user);
        }
    }
}