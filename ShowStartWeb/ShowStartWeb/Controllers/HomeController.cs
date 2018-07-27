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
            userinfo user = Session["UserInfo"] as userinfo;
            return View();
        }
    }
}