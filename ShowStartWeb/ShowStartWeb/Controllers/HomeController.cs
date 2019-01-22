using ShowStart.Bll;
using ShowStart.Model;
using ShowStartWeb.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShowStartWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [LoginFilter]
        public ActionResult Index()
        {
            ShowStartService ShowStartBll = new ShowStartService();
            userinfo user = Session["UserInfo"] as userinfo;

            ViewData["UserName"] = user.accountName;
            ViewData["UserEmail"] = user.email;
            ViewData["UserPhoneNumber"] = user.phoneNumber;

            //总数
            IQueryable<showstarts> showstartList = ShowStartBll.LoadEntities(u => u.showname != "");
            int allShow = showstartList.Count();
            ViewData["allShow"] = allShow;

            //今天的演出
            DateTime today = DateTime.Today.Date;
            DateTime todayBegin = today.AddSeconds(1);
            DateTime todayEnd = today.AddDays(1).AddSeconds(-1);

            IQueryable<showstarts> showstartListtoday = ShowStartBll.LoadEntities(u => u.startime > todayBegin && u.startime < todayEnd);
            int allCounttoday = showstartListtoday.Count();
            ViewData["TodayShow"] = allCounttoday;

            //未开演
            IQueryable<showstarts> showstartNotShow = ShowStartBll.LoadEntities(u => u.startime > today);
            int NotShowCount = showstartNotShow.Count();
            ViewData["NotShow"] = NotShowCount;

            //监控的歌手
            MonitorService monitorService = new MonitorService();
            IQueryable<monitor> monitors = monitorService.LoadEntities(u => u.username == user.username);
            int collectionCount = monitors.Count();
            ViewData["Monitor"] = collectionCount;

            return View();
        }
    }
}