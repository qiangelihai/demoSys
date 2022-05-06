using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using demoSys.Models;

namespace demoSys.Controllers
{

    public class AdminController : Controller
    {
        private Consulting_systemEntities db = new Consulting_systemEntities();
        // GET: Admin
        public ActionResult Index()
        {
            if(Session["nickname"] == null)
            {
                return Content("<script>alert('无权访问！');window.location.href='/Login/Index';</script>");
               
            }
            return View();
        }
        ////登出
        //public ActionResult Logout()
        //{
        //    Session.Abandon();

        //    return Redirect("/Login/Index");
        //}
        //学生信息页面
        public ActionResult StudentManage()
        {
            //获取一条学生信息返回
            admin admin = db.admin.FirstOrDefault();
            return View(admin);

        }

        //添加学生信息
        public ActionResult AddStudent()
        {

            return View();
        }
        //添加学生信息
        [HttpPost]
        public ActionResult AddStudent(admin admin)
        {
            ViewBag.notice = "";
            db.admin.Add(admin);
            //执行完添加之后，还要补充一句话！！
            //执行修改数据库的操作
            int result = db.SaveChanges();
            if(result > 0)
            {
                //保存成功提示，并跳转到首页！
                return Content("<script>alert('发布成功！');window.location.href='/Admin/StudentManage';</script>");
            }
            else
            {
                ViewBag.notice = "保存失败！";
            }
            return View();
        }


            //编辑学生信息
            public ActionResult UpdateStudent()
        {
            //获取一条学生信息返回
            admin admin = db.admin.FirstOrDefault();
            if(admin == null)
            {
                return Content("<script>alert('未找到信息！');window.location.href='/Admin/AddStudent';</script>");
            }


            return View(admin);
        }

        [HttpPost]
        public ActionResult UpdateStudent(admin admin)
        {
            //修改、新增、删除操作时，都需要保存修改操作！！！
            db.Entry(admin).State = System.Data.Entity.EntityState.Modified;

            if (db.SaveChanges() > 0)
            {
                return Content("<script>alert('编辑成功！');window.location.href='/Admin/StudentManage';</script>");
            }
            else
            {
                ViewBag.notice = "编辑失败！";
            }
            return View();
        }

    

    }
}