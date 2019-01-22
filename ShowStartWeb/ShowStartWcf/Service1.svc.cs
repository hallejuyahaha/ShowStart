using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ShowStart.Bll;
using ShowStart.Model;

namespace ShowStartWcf
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
    public class Service1 : IService1
    {

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

        public List<showstarts> GetShow()
        {
            ShowStartService ShowStartBll = new ShowStartService();
            IQueryable<showstarts> shows = ShowStartBll.LoadEntities(u => u.startime > DateTime.Today);
            List<showstarts> a = shows.ToList();
            return a;
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
}
