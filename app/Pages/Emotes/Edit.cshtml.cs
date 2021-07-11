using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Emotify.Models;

namespace Emotify.Pages.Emotes
{
    public class EmoteEditVM
    {
        public int Id { get; set; }
        public string Names { get; set; }
    }

    public class EditModel : PageModel
    {
        private readonly EmotifyDbContext _context;

        public EditModel(EmotifyDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EmoteEditVM Edit { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Emote = await _context.GetEmoteById(id);

            if (Emote == null)
            {
                return NotFound();
            }

            Edit = new EmoteEditVM
            {
                Id = Emote.Id,
                Names = string.Join(",", Emote.Names.Select(n => n.Name))
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var Emote = await _context.GetEmoteById(Edit.Id);
            Emote.Names = Edit.Names.Split(",").Select(name => new EmoteName() { EmoteId = Emote.Id, Name = name }).ToList();
            _context.Attach(Emote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
            return _context.Emotes.Any(e => e.Id == id);
        }
    }
}
