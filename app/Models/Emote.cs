
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emotify.Models
{
    public class Emote : UserOwnable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int EmoteImageId { get; set; }

        [ForeignKey(nameof(EmoteImageId))]

        public virtual EmoteImage EmoteImage { get; set; }
    }
}