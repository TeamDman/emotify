using System.Threading.Tasks;
using Emotify.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Emotify.Pages.Emotes
{
    public class DetailsModel : PageModel
    {
        public DetailsModel(
            EmotifyDbContext context)
        {
            Context = context;
        }

        public Emote Emote { get; set; }
        public EmotifyDbContext Context { get; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Emote = await Context.GetEmoteById(id);

            if (Emote == null) return NotFound();
            return Page();
        }
    }
}