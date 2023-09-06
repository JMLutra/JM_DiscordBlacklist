using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JM_DiscordBlacklist.LinkBlacklist
{
    public class Link
    {
        public string Topic { get; }
        public string URL { get; }
        public Link(string topic, string url)
        {
            Topic = topic;
            URL = url;
        }
    }
}
