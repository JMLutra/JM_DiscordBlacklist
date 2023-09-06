

using static JM_DiscordBlacklist.TheLinkBlacklist;

namespace JM_DiscordBlacklist
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please provide the path to the config file as an Argument.");
                Console.ForegroundColor = ConsoleColor.White;
                Environment.Exit(1);
            }

            new Config(args[0]);

            /* var tlbl = new TheLinkBlacklist(new CancellationTokenSource());
            Console.Read(); */
            
            Bot bot = new Bot(Config.ConfigObj.Botconfig.Token);
            bot.RunAsync().Wait();

        }
    }
}