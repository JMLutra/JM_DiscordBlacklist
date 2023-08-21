using Newtonsoft.Json.Linq;

namespace JM_DiscordBlacklist
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Bot bot = new Bot(args[1]);
            bot.RunAsync().Wait();
        }
    }
}