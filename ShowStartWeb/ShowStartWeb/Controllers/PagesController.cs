using Newtonsoft.Json.Linq;
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

    public class PagesController : Controller
    {
        ShowStartService ShowStartBll = new ShowStartService();
        DateTime today = DateTime.Today;
        // GET: Pages
        [LoginFilter]
        public ActionResult AUs()
        {
            userinfo user = Session["UserInfo"] as userinfo;
            ViewData["UserName"] = user.accountName;
            ViewData["UserEmail"] = user.email;
            ViewData["UserPhoneNumber"] = user.phoneNumber;
            return View();
        }
        [LoginFilter]
        public ActionResult Collect()
        {
            userinfo user = Session["UserInfo"] as userinfo;
            ViewData["UserName"] = user.accountName;
            ViewData["UserEmail"] = user.email;
            ViewData["UserPhoneNumber"] = user.phoneNumber;

            IQueryable<showstart> ShowsList = ShowStartBll.LoadEntities(u => u.ReadDate == today).OrderBy(u => u.starttime);
            CollectionService CollectionBll = new CollectionService();
            IQueryable<collection> CollectionList = CollectionBll.LoadEntities(u => u.username == user.username);

            var Data = from a in CollectionList
                       join b in ShowsList
                       on a.url equals b.url into temp
                       from c in temp.DefaultIfEmpty()
                       select new
                       {
                           id = c.id,
                           showname = c.showname,
                           actor = c.actor,
                           price = c.price,
                           time = c.time,
                           place = c.place,
                           url = c.url,
                           type = c.type,
                           StartOrEnd = c.StartOrEnd,
                           ReadSessionTime = c.ReadSessionTime,
                           front_image_path = c.front_image_path,
                           ReadDate = c.ReadDate
                       };

            var collect = Data.ToList().Select(c => new showstart
            {
                id = c.id,
                showname = c.showname,
                actor = c.actor,
                price = c.price,
                time = c.time,
                place = c.place,
                url = c.url,
                type = c.type,
                StartOrEnd = c.StartOrEnd,
                ReadSessionTime = c.ReadSessionTime,
                front_image_path = c.front_image_path,
                ReadDate = c.ReadDate
            }).ToList();

            ViewData.Model = collect;
            return View("Collect", collect);
        }
        [LoginFilter]
        public ActionResult Show()
        {
            userinfo user = Session["UserInfo"] as userinfo;
            ViewData["UserName"] = user.accountName;
            ViewData["UserEmail"] = user.email;
            ViewData["UserPhoneNumber"] = user.phoneNumber;

            IQueryable<showstart> ShowsList = ShowStartBll.LoadEntities(u => u.ReadDate == today && u.StartOrEnd == 1).OrderBy(u => u.starttime);

            IQueryable<collection> CollectionList = new CollectionService().LoadEntities(u => u.username == user.username);

            var Data = from a in ShowsList
                       join b in CollectionList
                       on a.url equals b.url into tempa

                       from c in tempa.DefaultIfEmpty()
                       select new
                       {
                           id = a.id,
                           showname = a.showname,
                           actor = a.actor,
                           price = a.price,
                           time = a.time,
                           place = a.place,
                           url = a.url,
                           type = a.type,
                           StartOrEnd = a.StartOrEnd,
                           ReadSessionTime = a.ReadSessionTime,
                           front_image_path = a.front_image_path,
                           ReadDate = a.ReadDate,
                           IsCollect = c.username == null ? 0 : 1
                       };

            var _ShowsList = Data.ToList().Select(c => new showstartlist
            {
                id = c.id,
                showname = c.showname,
                actor = c.actor,
                price = c.price,
                time = c.time,
                place = c.place,
                url = c.url,
                type = c.type,
                StartOrEnd = c.StartOrEnd,
                ReadSessionTime = c.ReadSessionTime,
                front_image_path = c.front_image_path,
                ReadDate = c.ReadDate,
                IsCollect = c.IsCollect
            }).ToList();

            ViewData.Model = _ShowsList;

            return View("Show", _ShowsList);
        }


        public JsonResult MyCollect()
        {
            string url = Request["url"];
            bool HadCollectOrNot = Request["CollectOrNot"] == "1" ? true : false;

            userinfo user = Session["UserInfo"] as userinfo;
            CollectionService CollectionBll = new CollectionService();
            if (HadCollectOrNot)
            {
                //已收藏，先要删除收藏
                collection temp = new collection { url = url, username = user.username };
                CollectionBll.DeleteEntity(temp);

                return Json("{\"msg\":\"NotCollect\"}");
            }
            else
            {
                //没有收藏，先要加入收藏
                collection temp = new collection() { url = url, username = user.username };
                CollectionBll.AddEntity(temp);

                //JObject jo = new JObject();
                //jo.Add("status", 0);//table 的表格
                //jo.Add("msg", "IsCollect");//page的总页数

                return Json("{\"msg\":\"IsCollect\"}");
            }
        }

        /// <summary>
        /// 临时类
        /// </summary>
        public class showstartlist
        {
            public long id { get; set; }
            public string showname { get; set; }
            public string actor { get; set; }
            public string price { get; set; }
            public string time { get; set; }
            public string place { get; set; }
            public string url { get; set; }
            public string type { get; set; }
            public Nullable<sbyte> StartOrEnd { get; set; }
            public Nullable<System.DateTime> ReadSessionTime { get; set; }
            public string front_image_path { get; set; }
            public Nullable<System.DateTime> ReadDate { get; set; }
            public int IsCollect { get; set; }
        }
    }
}