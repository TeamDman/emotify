using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Emotify.Models
{
    public class User
    {
        public virtual ICollection<Emote> Emotes { get; set; }
        public ulong Id { get; set; }
        public string Name { get; set; }
        public ushort Discriminator { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public string AvatarHash { get; set; }
    }
}