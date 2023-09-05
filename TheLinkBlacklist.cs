using JM_DiscordBlacklist.LinkBlacklist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JM_DiscordBlacklist
{
    internal class TheLinkBlacklist
    {
        public static Blacklist TheBlacklist;
        public TheLinkBlacklist() 
        { 
            TheBlacklist = new Blacklist();

            new Updater();
            
        }

        public async Task TestTimeSearch()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Testing time to search for a link...");
            Console.ForegroundColor = ConsoleColor.White;
            int time = await TheBlacklist.TestTimeSearch(1000);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Average time to search for a link: {time}ms");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
