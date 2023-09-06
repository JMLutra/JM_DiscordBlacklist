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
    internal class BotCommands :ApplicationCommandsModule
    {
        [SlashCommand("shutdown", "Shuts down the Bot"), ApplicationCommandRequireOwner]
        public async Task Shutdown(InteractionContext context)
        {
            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Shutting down..."));
            ShutdownRequest.Cancel();
        }
    }
}
