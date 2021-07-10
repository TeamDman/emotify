using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Emotify.Models;

    public class EmotifyContext : DbContext
    {
        public EmotifyContext (DbContextOptions<EmotifyContext> options)
            : base(options)
        {
        }

        public DbSet<Emotify.Models.Movie> Movie { get; set; }
    }
