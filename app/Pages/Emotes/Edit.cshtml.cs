using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Emotify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Emotify.Authorization;

namespace Emotify.Pages.Emotes
{
    public class EmoteEditVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class EditModel : EmotifyBasePageModel
    {

        public EditModel(
            EmotifyDbContext context,
            IAuthorizationService authorizationService,
            UserManager<EmotifyUser> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public EmoteEditVM Edit { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Emote = await Context.GetEmoteById(id);

            if (Emote == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, Emote, EmoteOperations.Modify);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Edit = new EmoteEditVM
            {
                Id = Emote.Id,
                Name = Emote.Name
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var Emote = await Context.GetEmoteById(Edit.Id);
            var Entry = Context.Attach(Emote);
            Entry.CurrentValues.SetValues(Edit);
            Entry.State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmoteExists(Emote.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool EmoteExists(int id)
        {
            return Context.Emotes.Any(e => e.Id == id);
        }
    }
}
