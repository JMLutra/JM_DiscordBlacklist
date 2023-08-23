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
        public static Blacklist blacklist;
        public TheLinkBlacklist() 
        { 
            blacklist = new Blacklist();

            new Updater();
        }
    }
}
