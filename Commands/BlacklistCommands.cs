using DisCatSharp;
using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.Entities;
using DisCatSharp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

using static JM_DiscordBlacklist.Bot;

namespace JM_DiscordBlacklist.Commands
{

    internal class BlacklistCommands : ApplicationCommandsModule
    {
        [SlashCommand("checklinks", "Enable or Disable the Link checking")]
        async Task CheckLinksToggle(InteractionContext context,
            [Choice("Enable", 1)][Choice("Disable", 0)][Option("option", "Enable or Disable")] int param)
        {
            if (!Config.ConfigObj.Botconfig.AdminIDs.Contains(context.User.Id))
            {
                await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("You're not allowed to use this Command").AsEphemeral());
                return;
            }
            if (param == 0 && TlBl.CheckCheckLinks()) 
            {
                TlBl.DisableCheckLinks();
                await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Links Checking is now disabled!").AsEphemeral());
                foreach(ConfigGuild guild in Config.ConfigObj.ConfigGuilds) 
                {
                    var g = await context.Client.GetGuildAsync(guild.GuildId);
                    var c = g.GetChannel(guild.ChannelID);
                    await c.SendMessageAsync($"The Link checking was disabled by {Formatter.Mention(context.User)}! No Links will be checked anymore!");
                }
            }
            if (param ==1 && !TlBl.CheckCheckLinks())
            {
                TlBl.EnableCheckLinks();
                await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Links Checking is now enabled!").AsEphemeral());
                foreach (ConfigGuild guild in Config.ConfigObj.ConfigGuilds)
                {
                    var g = await context.Client.GetGuildAsync(guild.GuildId);
                    var c = g.GetChannel(guild.ChannelID);
                    await c.SendMessageAsync($"The Link checking was enabled by {Formatter.Mention(context.User)}!");
                }
            }
        }

    }
}
