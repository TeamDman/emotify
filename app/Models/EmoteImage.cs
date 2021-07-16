
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Emotify.Models
{
    public class EmoteImage
    {
        public int Id { get; set; }

        public string Hash { get; set; }
        public byte[] Content { get; set; }

    }
}