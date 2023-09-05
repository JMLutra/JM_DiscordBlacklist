using DisCatSharp.Entities;
using DisCatSharp.EventArgs;
using DisCatSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JM_DiscordBlacklist.Events
{
    internal class ClientBasicEvents
    {
        public static Task Client_SocketOpened(DiscordClient dcl, SocketEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Socket opened");
            return Task.CompletedTask;
        }

        public static Task Client_SocketClosed(DiscordClient dcl, SocketCloseEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Socket closed: " + e.CloseMessage);
            return Task.CompletedTask;
        }

        public static Task Client_SocketErrored(DiscordClient dcl, SocketErrorEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Socket has an error! " + e.Exception.Message.ToString());
            return Task.CompletedTask;
        }

        /*Print Heartbeat only every ten beats*/
        private static int heartBeats;
        private static int pings;
        public static Task Client_Heartbeated(DiscordClient dcl, HeartbeatEventArgs e)
        {
            if (heartBeats < 10)
            {
                heartBeats++;
                pings += e.Ping;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(DateTime.Now + $" Received {heartBeats} Heartbeats with an average Ping of:" + pings / heartBeats);
                Console.ForegroundColor = ConsoleColor.White;
                heartBeats = 0;
                pings = 0;
            }
            return Task.CompletedTask;
        }

        public static Task Client_Ready(DiscordClient dcl, ReadyEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Starting {dcl.CurrentUser.Username}");
            Console.WriteLine("Client ready!");
            //Console.WriteLine($"Shard {dcl.ShardId}");
            //TODO: name all commands
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Bot ready!");
            DiscordActivity activity = new()
            {
                Name = Config.ConfigObj.Botconfig.Activity,
                ActivityType = ActivityType.Playing
            };
            dcl.UpdateStatusAsync(activity: activity, userStatus: UserStatus.Online, idleSince: null);
            Console.ForegroundColor = ConsoleColor.White;
            return Task.CompletedTask;
        }

        public static Task Client_Resumed(DiscordClient dcl, ReadyEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Bot resumed!");
            return Task.CompletedTask;
        }
    }
}
