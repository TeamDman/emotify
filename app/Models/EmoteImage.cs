
using System;
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

        public string FileType {get; set;}

        public string GetHtmlSrcAttribute() 
        {
            return $"data:image/{FileType};base64,{Convert.ToBase64String(Content)}";
        }
    }
}