using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATicketServerFW.Job
{
    class CheckCollectionJob : IJob
    {
        readonly ILog _log = LogManager.GetLogger(typeof(CheckCollectionJob));

        public void Execute(IJobExecutionContext context)
        {
            _log.Debug("进入Check");
            try
            {
                CheckCollectionWork checkCollectionWork = new CheckCollectionWork();
                checkCollectionWork.work();
            }
            catch (Exception e)
            {
                _log.Debug("CheckCollectionJob 发生错误 ：" + e.Message);
            }
        }
    }
}
