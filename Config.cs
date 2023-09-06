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
        public static ConfigObj? ConfigObj { get; set; }
        public Config(string path)
        { 
                ConfigObj = JsonConvert.DeserializeObject<ConfigObj>(File.ReadAllText(path));
                foreach (ConfigGuild confGuild in ConfigObj.ConfigGuilds)
                {
                    ConfigObj.Guilds[confGuild.GuildId] = confGuild;
                }
        }
    }

    internal class ConfigObj 
    {
        [JsonProperty("botconfig")]
        public ConfigBot Botconfig { get; set; }

        [JsonProperty("lists")]
        public ConfigList[] ConfigLists { get; set; }

        [JsonProperty("guilds")]
        public ConfigGuild[] ConfigGuilds { get; set; }

        public Dictionary<ulong?, ConfigGuild> Guilds { get; set; } = new Dictionary<ulong?, ConfigGuild>();
    }

    internal class ConfigBot
    {
        [JsonProperty("token"/*, Required = Required.Always*/)]
        public string Token { get; set; }

        [JsonProperty("activity")]
        public string Activity { get; set; }
    }

    internal class ConfigList
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        [JsonProperty("suffix")]
        public string Suffix { get; set; }
    }

    internal class ConfigGuild
    {
        [JsonProperty("guildId")]
        public ulong GuildId { get; set; }

        [JsonProperty("channelId")]
        public ulong ChannelID { get; set; }

        [JsonProperty("timeout")]
        public int Timeout { get; set; }
    }
}
