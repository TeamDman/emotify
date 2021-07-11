
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emotify.Models
{
    public class Emote
    {
        public int Id { get; set; }

        [MaxLength(128), ForeignKey(nameof(EmotifyUser))]
        public virtual string OwnerUserId { get; set; }
        public virtual EmotifyUser Owner { get; set; }

        public virtual ICollection<EmoteName> Names { get; set; }
    }

    public class EmoteName
    {
        public int EmoteId { get; set; }
        public string Name { get; set; }

        public virtual Emote Emote {get; set;}

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EmoteName>()
                .HasKey(e => new { e.EmoteId, e.Name });
        }
    }

}