

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

            Config conf = new Config(args[0]);

            Bot bot = new Bot(conf.ConfigObj.Botconfig.Token);
            bot.RunAsync().Wait();

        }
    }
}