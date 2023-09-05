using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JM_DiscordBlacklist.LinkBlacklist
{
    internal class Blacklist
    {
        private static ConcurrentDictionary<string, Link> links = new ConcurrentDictionary<string, Link>();

        public bool ready = false;
        public Blacklist()
        { }

        public void AddLink(string topic, string url)
        {
            Link link = new Link(topic, url);
            links[url] = link;
        }

        public Link? SearchForLink(string potLink)
        {
            if (links.TryGetValue(potLink, out Link link))
            {
                return link;
            }
            return null;
        }

        public void Clear()
        {
            links.Clear();
        }

        public int Count()
        {
            return links.Count;
        }

        public async Task<int> TestTimeSearch(int times)
        {
            List<string> keyList = new List<string>(links.Keys);
            Random rand = new Random();

            int total = 0;

            for (int i = 0; i < times; i++)
            {
                string key = keyList[rand.Next(keyList.Count)];
                long start = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                Link link = SearchForLink(key);
                long end = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                total += (int)(end - start);
            }

            return total / times;
        }

    }
}
