using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace ATicketServerFW
{
    class MainService
    {
        readonly ILog _log = LogManager.GetLogger(typeof(MainService));
        public MainService(string[] args)
        {
            _log.Debug("参数是 "+args);
        }
        public void Start()
        {
            _log.Debug("start Wrap Service");
        }
        public void Stop()
        {
            _log.Debug("Stop Wrap Service");
        }
    }
}
