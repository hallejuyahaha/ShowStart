using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Config;
using Quartz;
using System.Configuration;
using System.IO;
using Topshelf;
using Topshelf.Quartz;
using ATicketServerFW.Job;

namespace ATicketServerFW
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = HostFactory.New(x =>
            {
                x.Service<MainService>(s =>
                {
                    s.ConstructUsing(name => new MainService(args));
                    s.WhenStarted(tc =>
                    {
                        XmlConfigurator.ConfigureAndWatch(
                            new FileInfo(".\\log4net.config"));
                        tc.Start();
                    });
                    s.WhenStopped(tc => tc.Stop());
                    
                    //检查job
                    s.ScheduleQuartzJob(q =>
                        q.WithJob(() =>
                            JobBuilder.Create<CheckCollectionJob>().Build())
                            .AddTrigger(() => TriggerBuilder.Create()
                                .WithCronSchedule(ConfigurationManager.AppSettings["CheckCollectionCron"])
                                .Build()));
                });
                x.RunAsLocalSystem();
                x.SetDescription("ATicketServiceService");
                x.SetDisplayName("ATicketServiceService");
                x.SetServiceName("ATicketServiceService");
            });
            host.Run();
        }
    }
}
