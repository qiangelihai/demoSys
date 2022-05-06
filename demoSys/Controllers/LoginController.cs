using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using demoSys.Models;//引入进来数据库

namespace demoSys.Controllers
{
    public class LoginController : Controller
    {
        //数据库上下文连接对象
        private Consulting_systemEntities db = new Consulting_systemEntities();


        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        //实现登录功能
        [HttpPost]
        public ActionResult Index(string username,string password)
        {
            
            //去数据库查账号密码
            admin admin = db.admin.FirstOrDefault(p=>p.username == username);
            if(admin == null)
            {
                MessageBox.Show("用户不存在！");
                return Redirect("/Login/Index");
            }
            else if(admin.password != password)
            {
                MessageBox.Show("密码错误！");
                return Redirect("/Login/Index");
            }
            else
            {
                //在这里需要记住登陆成功的用户信息  cookie或者session =》会话管理。（HTTP请求不会带状态）
                //cookie存在是本地 session是存在服务器。最好放在session里，安全性高。
                Session["username"] = admin.username;
                Session["nickname"] = admin.nickname;
                Session["power"] = admin.power;


                ViewBag.notice = "登陆成功！";
                //登陆成功 跳转
                if (admin.power == 2)
                {
                    return Redirect("/Admin/index");
                }
                if (admin.power == 0)
                {
                    return Redirect("/Student/index");
                }
                if (admin.power == 1)
                {
                    return Redirect("/Teacher/index");
                }

            }

                return View();
        }
        public ActionResult Logout()
        {
            Session["username"] = null;
            Session["nickname"] = null;
            MessageBox.Show("退出成功！");
            return Redirect("/Login/Index");
        }
        

    }
}