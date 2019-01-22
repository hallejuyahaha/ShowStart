using ShowStart.IBll;
using ShowStart.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ShowStart.Bll
{
    public class MonitorService : BaseService<monitor>, IMonitorService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.MonitorDal;
        }
    }
}