using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static JM_DiscordBlacklist.TheLinkBlacklist;

namespace JM_DiscordBlacklist.LinkBlacklist
{
    internal class Updater
    {
        public Updater()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Updating the Blacklist");
            Console.ForegroundColor = ConsoleColor.White;
            TheBlacklist.ready = false;
            long tS = DateTimeOffset.Now.ToUnixTimeSeconds();
            Update().Wait();
            long tE = DateTimeOffset.Now.ToUnixTimeSeconds();
            TheBlacklist.ready = true;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Updated the Blacklist in {tE - tS}ms. It now contains {TheBlacklist.Count()} links.");
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        private async Task Update()
        {
            TheBlacklist.Clear();

            foreach(ConfigList confList in Config.ConfigObj.ConfigLists)
            {
                foreach(string line in await downloadList(confList))
                {
                    TheBlacklist.AddLink(confList.Topic, line);
                    //Console.WriteLine(line);
                }
            }
        }

        private async Task<List<string>> downloadList(ConfigList confList)
        {
            List<string> lines;
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    // Download the file content as a string
                    string fileContent = await httpClient.GetStringAsync(confList.Url);

                    // Split the content into lines and store in a List<string>
                    lines = new List<string>(fileContent.Split('\n'));
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Error downloading the file: {e.Message}");
                    lines = new List<string>();
                }

                if(confList.Comment != null)
                {
                    lines.RemoveAll(x => x.StartsWith(confList.Comment) || x == "" || x == "\n");
                }

                if(confList.Prefix != "" || confList.Suffix != "")
                {
                    for(int i = 0; i < lines.Count; i++)
                    {
                        if(confList.Prefix != "" && lines[i].StartsWith(confList.Prefix)) lines[i] = lines[i].Substring(confList.Prefix.Length);
                        if(confList.Suffix != "" && lines[i].StartsWith(confList.Suffix)) lines[i] = lines[i].Substring(0, lines[i].Length - confList.Suffix.Length);
                    }
                }

                return lines;
            }
        }
    }
}
