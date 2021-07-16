using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Emotify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Emotify.Authorization;

namespace Emotify.Pages.Emotes
{
    public class DeleteModel : EmotifyBasePageModel
    {

        public DeleteModel(
            EmotifyDbContext context,
            IAuthorizationService authorizationService,
            UserManager<EmotifyUser> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Emote Emote { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Emote = await Context.GetEmoteById(id);

            if (Emote == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, Emote, EmoteOperations.Modify);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Emote = await Context.Emotes.FindAsync(id);

            if (Emote != null)
            {
                var isAuthorized = await AuthorizationService.AuthorizeAsync(User, Emote, EmoteOperations.Modify);
                if (!isAuthorized.Succeeded)
                {
                    return Forbid();
                }
                Context.Emotes.Remove(Emote);
                await Context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
