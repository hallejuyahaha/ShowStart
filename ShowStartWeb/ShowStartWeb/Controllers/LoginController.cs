using Newtonsoft.Json.Linq;
using ShowStart.Bll;
using ShowStart.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShowStartWeb.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public JObject UserLogin()
        {
            JObject jo = new JObject();
            string loginName = Request["login"];
            string pwd = Request["pwd"];
            UserInfoService user = new ShowStart.Bll.UserInfoService();
            IQueryable<userinfo> userlist = user.LoadEntities(u => u.username == loginName && u.password == pwd);
            if (userlist.Count() > 0)
            {
                userinfo userNow = userlist.FirstOrDefault();
                //HttpContext.User 
                Session["UserInfo"] = userNow;
                jo.Add("Status", "ok");
                jo.Add("Text", "登录成功<br /><br />欢迎回来");
                return jo;
            }
            else
            {
                jo.Add("Status", "Erro");
                jo.Add("Erro", "账号名或密码或验证码有误");
                return jo;
            }            
        }
    }
}