using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Emotify.Models;

namespace Emotify.Pages.Emotes
{
    public class DeleteModel : PageModel
    {
        private readonly EmotifyDbContext _context;

        public DeleteModel(EmotifyDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Emote Emote { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Emote = await _context.Emotes.Where(e => e.Id == id).Include(e => e.Names).FirstOrDefaultAsync();

            if (Emote == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Emote = await _context.Emotes.FindAsync(id);

            if (Emote != null)
            {
                _context.Emotes.Remove(Emote);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}