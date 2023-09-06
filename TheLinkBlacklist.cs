using DisCatSharp;
using DisCatSharp.Entities;
using JM_DiscordBlacklist.LinkBlacklist;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JM_DiscordBlacklist
{
    public class TheLinkBlacklist
    {
        public static Blacklist TheBlacklist;
        public static bool CheckLinks;
        public TheLinkBlacklist(CancellationTokenSource cToken) 
        { 
            TheBlacklist = new Blacklist();
            CheckLinks = true;
            ScheduleAsyncUpdater(cToken).Wait();
        }

        public bool CheckCheckLinks() { return CheckLinks; }
        public void EnableCheckLinks() { CheckLinks = true; }

        public void DisableCheckLinks() {  CheckLinks = false; }

        public async Task CheckLink(DiscordClient dcl, DiscordMessage msg, MatchCollection matches)
        {
            while(!TheBlacklist.ready)
            {
                await Task.Delay(10);
            }
            foreach (Match match in matches)
            {

                string potLink = match.Value;
                string pattern = @"(\.\w{2,6})(\/[^\/]*)?$";
                potLink = Regex.Replace(potLink, pattern, "$1");
                if(potLink.StartsWith("http://")) potLink = potLink.Remove(0, 7);
                if(potLink.StartsWith("https://")) potLink = potLink.Remove(0, 8);
                Link? link = TheBlacklist.SearchForLink(potLink);
                if (link != null)
                {
                    ConfigGuild confGuild= Config.ConfigObj.Guilds[msg.Guild.Id];
                    await msg.DeleteAsync();
                    DiscordEmbedField actionField = null;
                    if(confGuild.Timeout < 0){
                        await msg.Guild.TimeoutAsync(msg.Author.Id, TimeSpan.FromHours(confGuild.Timeout), $"Blacklisted Link; Topic: {link.Topic}");
                        actionField = new DiscordEmbedField("Action", $"The User was timeouted {Formatter.Timestamp(DateTime.Now, DisCatSharp.Enums.TimestampFormat.RelativeTime)} ago, until {Formatter.Timestamp(TimeSpan.FromHours(confGuild.Timeout), DisCatSharp.Enums.TimestampFormat.ShortDateTime)}", true);
                    }
                    var member = msg.Author.ConvertToMember(msg.Guild).Result;
                    var userField = new DiscordEmbedField("User", $"**Username:** {msg.Author.UsernameWithGlobalName} \n**Nickname:** {member.Nickname} \n**Joined:** {member.JoinedAt.DateTime} ({Formatter.Timestamp(member.JoinedAt, DisCatSharp.Enums.TimestampFormat.RelativeTime)})", false);
                    var linkField = new DiscordEmbedField("Link", $"**Link:** {potLink} \n**Topic:** {link.Topic}", true);
                    var messageField = new DiscordEmbedField("Message", $"**Content:** {msg.Content} \n**Channel:** https://discord.com/channels/{msg.GuildId}/{msg.ChannelId} \n**Sent:** {msg.CreationTimestamp.DateTime} ({Formatter.Timestamp(msg.CreationTimestamp, DisCatSharp.Enums.TimestampFormat.RelativeTime)})", false);
                    if(actionField == null) actionField = new DiscordEmbedField("Action", $"The User wasn't timed out.", true);

                    var embed = new DiscordEmbedBuilder()
                    .WithColor(DiscordColor.Red)
                    .WithTitle("Blacklisted Link detected")
                    .WithDescription($"The link sent by {Formatter.Mention(msg.Author)} was blacklisted. Topic: {link.Topic}")
                    .WithThumbnail(msg.Author.AvatarUrl)
                    .AddFields(userField, messageField, linkField, actionField)
                    .WithFooter($"Discord Blacklist: Currently {TheBlacklist.Count()} links are blacklisted. Made with ❤️ by J_M_Lutra");

                    await msg.Guild.GetChannel(confGuild.ChannelID).SendMessageAsync(embed.Build());
                }
            }
        }

        private async Task ScheduleAsyncUpdater(CancellationTokenSource cToken)
        {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = await schedulerFactory.GetScheduler();

            await scheduler.Start(cToken.Token);

            IJobDetail ubdateJob = JobBuilder.Create<UpdateJob>()
            .WithIdentity("updateJob")
            .Build();

            ITrigger updateTrigger = TriggerBuilder.Create()
                .WithIdentity("updateTrigger")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(30)
                    //.WithIntervalInSeconds(20)
                    .RepeatForever())
                .Build();
            
            await scheduler.ScheduleJob(ubdateJob, updateTrigger);
        }
    }

    internal class UpdateJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() => new Updater());

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Testing time to search for a link...");
            Console.ForegroundColor = ConsoleColor.White;
            int time = await TheLinkBlacklist.TheBlacklist.TestTimeSearch(100);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Average time to search for a link: {time}ms");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
