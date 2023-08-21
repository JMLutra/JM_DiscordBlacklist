using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JM_DiscordBlacklist
{
    internal class Config
    {
        public ConfigObj ConfigObj { get; set; }
        public Config(string path) 
        {
            ConfigObj = JsonConvert.DeserializeObject<ConfigObj>(File.ReadAllText(path));
        }
    }

    internal class ConfigObj 
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("lists")]
        public ConfigList[] ConfigLists { get; set; }

        [JsonProperty("guilds")]
        public ConfigGuild[] ConfigGuilds { get; set; }
    }

    internal class ConfigList
    {
        [JsonProperty("url")]
        private string Url { get; set; }

        [JsonProperty("topic")]
        private string Topic { get; set; }

        [JsonProperty("comment")]
        private string Comment { get; set; }

        [JsonProperty("prefix")]
        private string Prefix { get; set; }

        [JsonProperty("suffix")]
        private string Suffix { get; set; }
    }

    internal class ConfigGuild
    {
        [JsonProperty("guildId")]
        private ulong GuildId { get; set; }

        [JsonProperty("channelId")]
        private ulong ChannelID { get; set; }

        [JsonProperty("timeout")]
        private int Timeout { get; set; }
    }
}
