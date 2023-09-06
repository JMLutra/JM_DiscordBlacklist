using System.Text.RegularExpressions;
using DisCatSharp;
using DisCatSharp.Entities;
using DisCatSharp.EventArgs;
using static JM_DiscordBlacklist.Bot;

namespace JM_DiscordBlacklist.Events
{
    class MessageEvents
    {
        public static Task Client_MessageCreated(DiscordClient dcl, MessageCreateEventArgs e)
        {
            if (e.Guild == null) return Task.CompletedTask; 
            if(!Config.ConfigObj.Guilds.ContainsKey(e.GuildId)) return Task.CompletedTask; 
            
            string pattern = @"(?i)\bhttps?://(?:\S+\.)?\S+\b";
            var matches = Regex.Matches(e.Message.Content, pattern);
            if(matches.Count == 0) return Task.CompletedTask;
            TlBl.CheckLink(dcl, e.Message, matches).Wait();

            return Task.CompletedTask;
        }
    }
}