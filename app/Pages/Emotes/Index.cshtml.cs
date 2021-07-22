using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Emotify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Emotify.Pages.Emotes
{
    public class IndexModel : PageModel
    {
        
        public IndexModel(
            EmotifyDbContext context)
        {
            Context = context;
        }


        public IList<Emote> Emotes { get; set; }

        // Filters
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        
        [BindProperty(SupportsGet = true)]
        [DisplayName("Show mine")]
        public bool ShowMine {get; set;} = true;


        [BindProperty]
        public IList<int> SelectedEmoteIds {get; set;}

        public EmotifyDbContext Context { get; }
        public UserManager<EmotifyUser> UserManager { get; }

        public async Task OnGetAsync()
        {
            // Get emotes and their images
            var emoteQuery =
                from e in Context.Emotes.Include(e => e.EmoteImage)
                select e;
            // If search query provided, filter results
            if (!string.IsNullOrEmpty(SearchString))
            {
                emoteQuery = emoteQuery.Where(e => e.Name.Contains(SearchString));
            }
            if (!ShowMine)
            {
                emoteQuery = emoteQuery.Where(e => e.OwnerUserId != UserManager.GetUserId(User));
            }
            // Return result list
            Emotes = await emoteQuery.ToListAsync();
        }

        public async Task<IActionResult> OnPostCopyAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            return RedirectToPage("./Index");
        }
    }
}
