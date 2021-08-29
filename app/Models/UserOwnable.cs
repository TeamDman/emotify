using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Emotify.Models
{
    public class UserOwnable
    {
        
        [MaxLength(128)]
        public virtual string OwnerUserId { get; set; }
        
        [ForeignKey(nameof(OwnerUserId))]
        public virtual EmotifyUser Owner { get; set; }
    }
}