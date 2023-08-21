

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
            if (conf.ConfigObj == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please make shure to provide the path to the config file as an Argument.");
                Console.ForegroundColor = ConsoleColor.White;
                Environment.Exit(1);
            }

            Bot bot = new Bot(conf.ConfigObj.Token);
            bot.RunAsync().Wait();

        }
    }
}