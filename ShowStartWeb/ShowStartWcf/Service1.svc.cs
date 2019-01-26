using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using log4net;
using ShowStart.Bll;
using ShowStart.Common;
using ShowStart.Model;

namespace ShowStartWcf
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
    public class Service1 : IService1
    {
        readonly static ILog _log = LogManager.GetLogger(typeof(Service1));

        //要添加log4net.config到项目中
        ////其他成员
        //public event System.EventHandler AddComplete;
        //public IAsyncResult BeginAdd(double x, double y, AsyncCallback callback, object asyncState)
        //{
        //    //省略实现
        //}

        //public double EndAdd(System.IAsyncResult result)
        //{
        //    //省略实现
        //}

        //public void AddAsync(double x, double y)
        //{
        //    //省略实现
        //}

        //public void AddAsync(double x, double y, object userState)
        //{
        //    //省略实现
        //}





        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public bool DeleteMonitors(monitor monitor)
        {
            MonitorService monitorService = new MonitorService();
            bool t = monitorService.DeleteEntity(monitor);            
            return t;
        }

        public bool AddMonitors(monitor monitor)
        {
            MonitorService monitorService = new MonitorService();
            bool t = monitorService.AddEntity(monitor);
            return t;
        }

        public List<monitor> GetMonitors(string username)
        {
            MonitorService monitorService = new MonitorService();
            IQueryable<monitor> monitors = monitorService.LoadEntities(_x => _x.username == username);
            List<monitor> a = monitors.ToList();
            return a;
        }

        Dictionary<string, List<showstarts>> IService1.GetShow(string username)
        {

            string dir = System.Web.Hosting.HostingEnvironment.MapPath("~/bin/Log4net.config");
            FileInfo f = new FileInfo(dir);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(f);
      
            Dictionary<string, List<showstarts>> keys = new Dictionary<string, List<showstarts>>();
            ShowStartService ShowStartBll = new ShowStartService();
            //get all shows
            IQueryable<showstarts> shows = ShowStartBll.LoadEntities(u => u.showname != "" &&  u.startime > DateTime.Now);
            List<showstarts> DBShows = shows.ToList();
            //get redis shows
            List<showstarts> redisShows = RedisCacheHelper.Get<List<showstarts>>(username);
            //过去新show
            OrderCompare oc = new OrderCompare();
            List<showstarts> newShow;
            List<showstarts> oldShow;
            if (redisShows == null)
            {
                newShow = DBShows;
            }
            else {
                newShow = DBShows.Except(redisShows, oc).ToList();
            }
            oldShow = DBShows.Except(newShow, oc).ToList();

            //将新show和老show塞进dic
            keys.Add("newshow", newShow);
            keys.Add("oldshow", oldShow);
            //更新redis
            RedisCacheHelper.Add<List<showstarts>>(username, DBShows);
            return keys;
        }


        public int Login(string username, string password)
        {
            UserInfoService userInfoService = new UserInfoService();
            IQueryable<userinfo> userinfos = userInfoService.LoadEntities(_x => _x.username == username && _x.password == password);
            if (userinfos.Count() > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int Register(string username, string password, string email, string phonenumber, string sex, string accountName)
        {
            UserInfoService userInfoService = new UserInfoService();
            IQueryable<userinfo> userinfos = userInfoService.LoadEntities(_x => _x.username == username);
            if (userinfos.Count() > 0)
            {
                return 2;
            }
            else
            {
                userinfo newUser = new userinfo() { username = username, password = password, email = email, phoneNumber = phonenumber, sex = sex , accountName  = accountName };
                if (userInfoService.AddEntity(newUser))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
    public class AsyncCompleteEventArgs : EventArgs
    {
        public bool Cancelled { get; }
        public Exception Error { get; }
        public object UserState { get; }
    }
    //比较器
    public class OrderCompare : IEqualityComparer<showstarts>
    {
        public bool Equals(showstarts o1, showstarts o2)
        {

            if (Object.ReferenceEquals(o1, o2))
            {
                return true;
            }

            if (Object.ReferenceEquals(o1, null) || Object.ReferenceEquals(o2, null))
            {
                return false;
            }

            bool flag = o1.showname.Equals(o2.showname) 
                     && o1.actor.Equals(o2.actor) 
                     && o1.price.Equals(o2.price) 
                     && o1.startime.Equals(o2.startime) 
                     && o1.place.Equals(o2.place)
                     && o1.url.Equals(o2.url)
                     && o1.type.Equals(o2.type)
                     && o1.front_image_path.Equals(o2.front_image_path)
                     && o1.readtime.Equals(o2.readtime);
            return flag;
        }

        public int GetHashCode(showstarts oinfo)
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
}
