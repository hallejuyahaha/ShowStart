using ShowStart.Bll;
using ShowStart.Model;
using ShowStartWeb.Filters;
using System;
using System.Collections.Generic;
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

            //前一天总量
            DateTime yesterday = DateTime.Today.AddDays(-1);
            IQueryable<showstart> showstartListyesterday = ShowStartBll.LoadEntities(u=>u.ReadDate== yesterday);
            int allCountyesterday = showstartListyesterday.Count();
            ViewData["allCountyesterday"] = allCountyesterday;

            //今日总量
            DateTime today = DateTime.Today;
            IQueryable<showstart> showstartListtoday = ShowStartBll.LoadEntities(u => u.ReadDate == today);
            int allCounttoday = showstartListtoday.Count();
            ViewData["allCounttoday"] = allCounttoday;

            //截止今日未开演
            IQueryable<showstart> showstartNotShow = ShowStartBll.LoadEntities(u => u.StartOrEnd == 1 && u.ReadDate == today);
            int NotShowCount = showstartNotShow.Count();
            ViewData["NotShow"] = NotShowCount;

            //收藏
            CollectionService CollectionBll = new CollectionService();
            IQueryable<collection> collection = CollectionBll.LoadEntities(u => u.username == user.username);
            int collectionCount = collection.Count();
            ViewData["Collect"] = collectionCount;

            return View();
        }
    }
}