using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Emotify.Extensions;
using Emotify.Models;
using Emotify.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Emotify.Pages.Emotes
{
    public class EmoteVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }

    [Authorize]
    public class CreateModel : PageModel
    {        
        private readonly UserHelper _userHelper; 
        private readonly EmotifyDbContext _context;

        public CreateModel(
            EmotifyDbContext context,
            UserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }



        [BindProperty]
        public EmoteVM EmoteResponse { get; set; }


        public IActionResult OnGet()
        {
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var expression = new Regex(@"\.(png|jpg|jpeg|gif)");
            if (!expression.IsMatch(EmoteResponse.File.FileName))
            {
                ModelState.AddModelError("EmoteResponse.File", "Please select an image file.");
                return Page();
            }
            
            // create new emote
            var emote = new Emote
            {
                OwnerUserId = _userHelper.GetUserId(User),
                Name = EmoteResponse.Name
            };

            await using (var memoryStream = new MemoryStream())
            {
                await EmoteResponse.File.CopyToAsync(memoryStream);
                if (memoryStream.Length >= 1024 * 256)
                {
                    ModelState.AddModelError("EmoteResponse.File", "The file must be 256KiB or smaller.");
                    return Page();
                }

                // Create image from file
                var imageArray = memoryStream.ToArray();
                var image = new EmoteImage
                {
                    Content = imageArray,
                    Hash = Convert.ToBase64String(MD5.Create().ComputeHash(imageArray)),
                    FileType = Path.GetExtension(EmoteResponse.File.FileName)
                };

                // Use existing image entry if exists
                image = await image.FindExistingOrDefault(_context);

                // Assign image to emote
                emote.EmoteImage = image;

                // Commit image to db
                _context.EmoteImages.Add(image);
            }

            // Commit emote to db
            _context.Emotes.Add(emote);

            // Save new records
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}