using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Emotify.Models
{
    public class EmotifyUser : IdentityUser
    {
        public virtual ICollection<Emote> Emotes { get; set; }
    }
}