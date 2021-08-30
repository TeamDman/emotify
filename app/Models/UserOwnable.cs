using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Emotify.Models
{
    public class UserOwnable
    {
        
        [MaxLength(128)]
        public virtual ulong OwnerUserId { get; set; }
        
        [ForeignKey(nameof(OwnerUserId))]
        public virtual User Owner { get; set; }
    }
}