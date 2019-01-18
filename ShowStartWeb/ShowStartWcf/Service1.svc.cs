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

        public List<showstart> GetShow()
        {
            ShowStartService ShowStartBll = new ShowStartService();
            IQueryable<showstart> shows = ShowStartBll.LoadEntities(u => u.ReadDate == DateTime.Today && u.StartOrEnd == 1);
            List<showstart> a = shows.ToList();
            return a;
        }
        
    }
    public class AsyncCompleteEventArgs : EventArgs
    {
        public bool Cancelled { get; }
        public Exception Error { get; }
        public object UserState { get; }
    }
}
