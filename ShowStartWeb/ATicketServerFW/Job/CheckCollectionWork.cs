using ATicketServerFW.Model;
using Jiguang.JPush;
using Jiguang.JPush.Model;
using log4net;
using ShowStart.Bll;
using ShowStart.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATicketServerFW.Job
{
    public class CheckCollectionWork
    {
        readonly ILog _log = LogManager.GetLogger(typeof(CheckCollectionJob));
        public void work()
        {
            //获取数据库中  monitor表
            showstartEntities db = new showstartEntities();
            IQueryable<monitor> Monitor = db.monitor.Where(u => u.username != null);
            List<monitor> DBMonitor = Monitor.ToList();
            //获取redis中 monitor表
            List<monitor> redisMonitors = RedisCacheHelper.Get<List<monitor>>("monitor");

            //取两个差集
            OrderCompare oc = new OrderCompare();
            List<monitor> newMonitor;
            if (redisMonitors == null)
            {
                newMonitor = DBMonitor;
            }
            else
            {
                newMonitor = DBMonitor.Except(redisMonitors, oc).ToList();
            }

            if (newMonitor.Count() <= 0)
            {
                _log.Debug("newMonitor is kong");
            }
            else
            {
                _log.Debug("开始发送jpush");
                //join
                List<showstarts> Showstarts = db.showstarts.Where(u => u.startime > DateTime.Now).ToList();
                var q = from a in newMonitor
                        join b in Showstarts
                        on a.actor equals b.actor //into c
                        //from cc in c.DefaultIfEmpty()
                        select new
                        {
                            actor = b.actor,
                            front_image_path = b.front_image_path,
                            place = b.place,
                            price = b.price,
                            readtime = b.readtime,
                            showname = b.showname,
                            startime = b.startime,
                            type = b.type,
                            url = b.url,
                            Monitorid = a.id,
                            Monitoractor = a.actor,
                            Monitoruname = a.username
                        };
                var alerts = q.ToList();
                if (alerts.Count > 0)
                {
                    //将差集 （新添加的） 发送到JPUSH
                    string alert = "";
                    foreach (var t in alerts)
                    {
                        alert += t.actor + " ";
                    }

                    PushPayload pushPayload = new PushPayload()
                    {
                        Platform = new List<string> { "android", "ios" },
                        Audience = "all",
                        Notification = new Notification()
                        {
                            Alert = "Hello ATicket",
                            Android = new Android()
                            {
                                Alert = "歌手： " + alert,
                                Title = "检测到你可以感兴趣演员的新Show哦！！！"
                            },
                            IOS = new IOS()
                            {
                                Alert = "ios alert",
                                Badge = "+1"
                            }
                        },
                        Message = new Message()
                        {
                            Title = "message title",
                            Content = "message content",
                            Extras = new Dictionary<string, string>()
                            {
                                ["key1"] = "value1"
                            }
                        }
                    };
                    JP jp = new JP();
                    jp.ExecutePushExample(pushPayload);

                    //将新的DB monitor表写进 redis
                    RedisCacheHelper.Add<List<monitor>>("monitor", DBMonitor);
                }
                else {
                    _log.Debug("没有要推送的");
                }
            }
        }
    }
    //比较器
    public class OrderCompare : IEqualityComparer<monitor>
    {
        public bool Equals(monitor o1, monitor o2)
        {
            if (Object.ReferenceEquals(o1, o2))
            {
                return true;
            }
            if (Object.ReferenceEquals(o1, null) || Object.ReferenceEquals(o2, null))
            {
                return false;
            }
            bool flag = o1.id.Equals(o2.id)
                     && o1.actor.Equals(o2.actor)
                     && o1.username.Equals(o2.username);

            return flag;
        }
        public int GetHashCode(monitor oinfo)
        {
            if (oinfo == null)
            {
                return 0;
            }
            else
            {
                int hashresult = oinfo.ToString().GetHashCode();
                return hashresult;
            }
        }
    }

    //JPUSH 
    public class JP
    {
        private static string AppKey = ConfigurationManager.AppSettings["AppKey"];
        private static string MasterSecret = ConfigurationManager.AppSettings["MasterSecret"];
        private static JPushClient client = new JPushClient(AppKey, MasterSecret);
        readonly ILog _log = LogManager.GetLogger(typeof(JP));
        public void ExecutePushExample(PushPayload pushPayload)
        {
            //PushPayload pushPayload = new PushPayload()
            //{
            //    Platform = new List<string> { "android", "ios" },
            //    Audience = "all",
            //    Notification = new Notification()
            //    {
            //        Alert = "hello jpush",
            //        Android = new Android()
            //        {
            //            Alert = "android alert",
            //            Title = "title"
            //        },
            //        IOS = new IOS()
            //        {
            //            Alert = "ios alert",
            //            Badge = "+1"
            //        }
            //    },
            //    Message = new Message()
            //    {
            //        Title = "message title",
            //        Content = "message content",
            //        Extras = new Dictionary<string, string>()
            //        {
            //            ["key1"] = "value1"
            //        }
            //    }
            //};
            var response = client.SendPush(pushPayload);
            Console.WriteLine(response.Content);
            _log.Debug("JPUSH内容  " + response.Content);
        }

        private static void ExecuteDeviceEample()
        {
            var registrationId = "12145125123151";
            var devicePayload = new DevicePayload()
            {
                Alias = "alias1",
                Mobile = "12300000000",
                Tags = new Dictionary<string, object>()
                {
                    { "add", new List<string>() { "tag1", "tag2" } },
                    { "remove", new List<string>() { "tag3", "tag4" } }
                }
            };
            var response = client.Device.UpdateDeviceInfo(registrationId, devicePayload);
            Console.WriteLine(response.Content);
        }

        private static void ExecuteReportExample()
        {
            var response = client.Report.GetMessageReport(new List<string> { "1251231231" });
            Console.WriteLine(response.Content);
        }

        private static void ExecuteScheduleExample()
        {
            var pushPayload = new PushPayload
            {
                Platform = "all",
                Notification = new Notification()
                {
                    Alert = "Hello JPush"
                }
            };
            var trigger = new Trigger()
            {
                StartDate = "2017-08-03 12:00:00",
                EndDate = "2017-12-30 12:00:00",
                TriggerTime = "12:00:00",
                TimeUnit = "week",
                Frequency = 2,
                TimeList = new List<string>()
                {
                    "wed", "fri"
                }
            };
            var response = client.Schedule.CreatePeriodicalScheduleTask("task1", pushPayload, trigger);
            Console.WriteLine(response.Content);
        }
    }
}
