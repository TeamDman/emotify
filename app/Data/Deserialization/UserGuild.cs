using System;

namespace Emotify.Data.Deserialization
{
    public class UserGuild
    {
        public string id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }

        public string iconUrl
        {
            get
            {
                if (icon == null)
                {
                    return "";
                }
                var extension = icon.StartsWith("a_") ? "gif" : "webp";
                return $"https://cdn.discordapp.com/icons/{id}/{icon}.{extension}";
            }
        }

        public bool owner { get; set; }
        public int permissions { get; set; }
        public string[] features { get; set; }

        public ulong GetId()
        {
            return UInt64.Parse(id);
        }
    }
}