using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JM_DiscordBlacklist.LinkBlacklist
{
    internal class Blacklist
    {
        private static List<Link> links = new();

        public Blacklist()
        { }

        public static void AddLink(string topic, string url)
        {
            links.Add(new Link(topic, url));
        }

        public static Link SearchForLink(string potLink)
        {
            return links.Find(x => x.URL.Equals(potLink));
        }

    }
}
