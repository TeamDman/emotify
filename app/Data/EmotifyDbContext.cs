using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Emotify.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public class EmotifyDbContext : DbContext
{
    public EmotifyDbContext(DbContextOptions<EmotifyDbContext> options)
        : base(options)
    {
    }

    public DbSet<Emote> Emotes { get; set; }
    public DbSet<EmoteImage> EmoteImages { get; set; }

    public DbSet<EnrolledGuild> EnrolledGuilds { get; set; }
    
    public DbSet<User> Users { get; set; }

    public async Task<Emote> GetEmoteById(int? id)
    {
        return await Emotes.AsAsyncEnumerable().Where(e => e.Id == id).FirstOrDefaultAsync();
    }
}