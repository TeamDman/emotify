using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Emotify.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public class EmotifyDbContext : IdentityDbContext<EmotifyUser>
    {
        public EmotifyDbContext (DbContextOptions<EmotifyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Emotify.Models.Emote> Emotes {get; set;}

        public async Task<Emote> GetEmoteById(int? id) {
            return await Emotes.Where(e=>e.Id == id).Include(e => e.Names).FirstOrDefaultAsync();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            EmoteName.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }


    }
