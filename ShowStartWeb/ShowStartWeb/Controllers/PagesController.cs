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
    [LoginFilter]
    public class PagesController : Controller
    {
        ShowStartService ShowStartBll = new ShowStartService();

        // GET: Pages
        public ActionResult AUs()
        {
            userinfo user = Session["UserInfo"] as userinfo;
            ViewData["UserName"] = user.accountName;
            ViewData["UserEmail"] = user.email;
            ViewData["UserPhoneNumber"] = user.phoneNumber;
            return View();
        }
        public ActionResult Collect()
        {
            userinfo user = Session["UserInfo"] as userinfo;
            ViewData["UserName"] = user.accountName;
            ViewData["UserEmail"] = user.email;
            ViewData["UserPhoneNumber"] = user.phoneNumber;
            return View();
        }
        public ActionResult Show()
        {
            userinfo user = Session["UserInfo"] as userinfo;
            ViewData["UserName"] = user.accountName;
            ViewData["UserEmail"] = user.email;
            ViewData["UserPhoneNumber"] = user.phoneNumber;
            return View();
        }
    }
}