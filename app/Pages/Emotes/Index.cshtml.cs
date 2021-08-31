using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Emotify.Models;
using Emotify.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Emotify.Pages.Emotes
{
    public class IndexModel : PageModel
    {
        private readonly EmotifyDbContext _context;

        public IndexModel(
            EmotifyDbContext context
        )
        {
            _context = context;
        }


        public IList<Emote> Emotes { get; set; }

        // Filters
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        [DisplayName("Show mine")]
        public bool ShowMine { get; set; } = true;


        [BindProperty]
        public IList<int> SelectedEmoteIds { get; set; }

        public async Task OnGetAsync()
        {

            // Get emotes and their images
            var emoteQuery =
                from e in _context.Emotes.Include(e => e.EmoteImage)
                select e;
            // If search query provided, filter results
            if (!string.IsNullOrEmpty(SearchString))
            {
                emoteQuery = emoteQuery.Where(e => e.Name.Contains(SearchString));
            }

            if (!ShowMine)
            {
                emoteQuery = emoteQuery.Where(e => e.OwnerUserId != UserHelper.GetUserId(User));
            }

            // Return result list
            Emotes = await emoteQuery.ToListAsync();
        }

        public IActionResult OnPostCopyAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}