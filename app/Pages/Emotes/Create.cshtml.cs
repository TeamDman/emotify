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
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Security.Cryptography;
using Emotify.Extensions;
using System.Text.RegularExpressions;

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
    public class CreateModel : EmotifyBasePageModel
    {
        public CreateModel(
            EmotifyDbContext context,
            IAuthorizationService authorizationService,
            UserManager<EmotifyUser> userManager)
        : base(context, authorizationService, userManager)
        {
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
            Regex expression = new Regex(@"\.(png|jpg|jpeg)");
            if (!expression.IsMatch(EmoteResponse.File.FileName))
            {
                ModelState.AddModelError("File","Please select an image file.");
            }


            // get current user
            var user = await UserManager.GetUserAsync(User);

            // create new emote
            var emote = new Emote()
            {
                OwnerUserId = user.Id,
                Name = EmoteResponse.Name
            };

            using (var memoryStream = new MemoryStream())
            {
                await EmoteResponse.File.CopyToAsync(memoryStream);
                if (memoryStream.Length >= 1024 * 256)
                {
                    ModelState.AddModelError("File", "The file must be 256KiB or smaller.");
                    return Page();
                }
                // Create image from file
                byte[] imageArray = memoryStream.ToArray();
                var image = new EmoteImage()
                {
                    Content = imageArray,
                    Hash = Convert.ToBase64String(MD5.Create().ComputeHash(imageArray))
                };

                // Use existing image entry if exists
                image = await image.FindExistingOrDefault(Context);

                // Assign image to emote
                emote.EmoteImage = image;
            }


            Context.Emotes.Add(emote);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
