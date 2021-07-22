using System.Threading.Tasks;
using Emotify.Authorization;
using Emotify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Emotify.Pages.Emotes
{
    public class DeleteModel : PageModel
    {
        public DeleteModel(
            EmotifyDbContext context,
            IAuthorizationService authorizationService)
        {
            Context = context;
            AuthorizationService = authorizationService;
        }

        [BindProperty]
        public Emote Emote { get; set; }

        public EmotifyDbContext Context { get; }
        public IAuthorizationService AuthorizationService { get; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Emote = await Context.GetEmoteById(id);

            if (Emote == null) return NotFound();

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, Emote, EmoteOperations.Modify);
            if (!isAuthorized.Succeeded) return Forbid();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            Emote = await Context.Emotes.FindAsync(id);

            if (Emote != null)
            {
                var isAuthorized = await AuthorizationService.AuthorizeAsync(User, Emote, EmoteOperations.Modify);
                if (!isAuthorized.Succeeded) return Forbid();
                Context.Emotes.Remove(Emote);
                await Context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}