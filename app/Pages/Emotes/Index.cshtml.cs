using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Emotify.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Emotify.Pages.Emotes
{

    public class IndexModel : PageModel
    {
        private readonly EmotifyDbContext _context;

        public IndexModel(EmotifyDbContext context)
        {
            _context = context;
        }

        public IList<Emote> Emotes { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync()
        {
            var emoteQuery =
                from e in _context.Emotes
                select e;
            emoteQuery = emoteQuery.Include(emote => emote.Names);
            if (!string.IsNullOrEmpty(SearchString))
            {
                emoteQuery = emoteQuery.Where(e => e.Names.Any(name => name.Name.Contains(SearchString)));
            }
            Emotes = await emoteQuery.ToListAsync();
        }
    }
}
