
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emotify.Models
{
    public class Emote
    {
        public int Id { get; set; }

        [MaxLength(128)]
        public virtual string OwnerUserId { get; set; }
        
        [ForeignKey(nameof(OwnerUserId))]
        public virtual EmotifyUser Owner { get; set; }


        public string Name { get; set; }

        public int EmoteImageId { get; set; }

        [ForeignKey(nameof(EmoteImageId))]

        public virtual EmoteImage EmoteImage { get; set; }
    }
}