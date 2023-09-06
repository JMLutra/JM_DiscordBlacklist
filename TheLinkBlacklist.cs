using JM_DiscordBlacklist.LinkBlacklist;
using Quartz;
using Quartz.Impl;
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
        public TheLinkBlacklist(CancellationTokenSource cToken) 
        { 
            TheBlacklist = new Blacklist();

            ScheduleAsyncUpdater(cToken).Wait();
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
                    //.WithIntervalInMinutes(1)
                    .WithIntervalInSeconds(20)
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
