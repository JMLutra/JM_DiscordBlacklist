using DisCatSharp;
using DisCatSharp.ApplicationCommands;
using DisCatSharp.Entities;
using DisCatSharp.Enums;
using JM_DiscordBlacklist.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JM_DiscordBlacklist
{
    internal class Bot : IDisposable
    {
        public static CancellationTokenSource ShutdownRequest;
        public static DiscordClient Client { get; set; }
        public static ApplicationCommandsExtension Appl;

        public ulong guildID { get; set; }
        public Bot(string Token)
        {
            ShutdownRequest = new CancellationTokenSource();

            var cfg = new DiscordConfiguration
            {
                Token = Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug, //TODO Change to Error
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContent,
                MessageCacheSize = 2048
            };

            Client = new DiscordClient(cfg);
            Client.UseApplicationCommands();

            Appl = Client.GetApplicationCommands();
            var applConf = new ApplicationCommandsConfiguration()
            {
                EnableDefaultHelp = false,
                DebugStartup = true
            };

            RegisterEventListener(Client, Appl);
            RegisterCommands(Appl, applConf);
        }

        public void Dispose()
        {
            Client.Dispose();
            Client = null;
            Appl = null;

            Environment.Exit(0);
        }

        public async Task RunAsync()
        {
            await Client.ConnectAsync();
            while (!ShutdownRequest.IsCancellationRequested)
            {
                await Task.Delay(2000);
            }
            await Client.UpdateStatusAsync(activity: null, userStatus: UserStatus.Offline, idleSince: null);
            await Client.DisconnectAsync();
            await Task.Delay(2500);
            Dispose();
        }

        private void RegisterEventListener(DiscordClient client, ApplicationCommandsExtension appl)
        {
            client.SocketOpened += ClientBasicEvents.Client_SocketOpened;
            client.SocketClosed += ClientBasicEvents.Client_SocketClosed;
            client.SocketErrored += ClientBasicEvents.Client_SocketErrored;
            client.Heartbeated += ClientBasicEvents.Client_Heartbeated;
            client.Ready += ClientBasicEvents.Client_Ready;
            client.Resumed += ClientBasicEvents.Client_Resumed;
        }

        private void RegisterCommands(ApplicationCommandsExtension appl, ApplicationCommandsConfiguration applConf)
        {

        }
    }
}
