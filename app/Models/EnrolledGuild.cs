using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Discord;

namespace Emotify.Models
{
    public class EnrolledGuild
    {
        public ulong Id { get; set; }

        [MaxLength(128)]
        public virtual string OwnerUserId { get; set; }

        [ForeignKey(nameof(OwnerUserId))]
        public virtual EmotifyUser Owner { get; set; }
    }
}