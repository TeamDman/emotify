using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Discord;

namespace Emotify.Models
{
    public class EnrolledGuild : UserOwnable
    {
        public ulong Id { get; set; }
    }
}