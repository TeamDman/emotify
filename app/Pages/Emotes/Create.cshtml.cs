using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Emotify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Emotify.Pages.Emotes
{
    public class EmoteVM
    {
        public string Name { get; set; }
    }

    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly EmotifyDbContext _context;
        private readonly UserManager<EmotifyUser> _userManager;

        public CreateModel(UserManager<EmotifyUser> userManager, EmotifyDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public EmoteVM EmoteResponse { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // get current user
            var user = await _userManager.GetUserAsync(User);

            // create new emote
            var emote = new Emote()
            {
                OwnerUserId = user.Id,
                Names = new List<EmoteName>(){
                    new EmoteName()
                    {
                        Name = EmoteResponse.Name
                    }
                }
            };

            _context.Emotes.Add(emote);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
