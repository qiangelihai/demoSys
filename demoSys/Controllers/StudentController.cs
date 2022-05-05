using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using demoSys.Models;
using PagedList;
using System.Threading;
using System.Windows;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace demoSys.Controllers
{
    public class StudentController : Controller
    {
        private Consulting_systemEntities db = new Consulting_systemEntities();

        // GET: Student
        public ActionResult Index(int? page)
        {
            if (Session["nickname"] == null)
            {
                Session.Abandon();
                return Content("<script>alert('无权访问！！');window.location.href='/Login/Index';</script>");

            }
            Session["nowPage"] = page;
            //Class1 class1 = new Class1();
            //return View(class1) ;
            var moment = from s in db.moment select s;

            //第几页  
            int pageNumber = page ?? 1;

            //每页显示多少条  
            int pageSize = 3;

            //根据ID升序排序  
            moment = moment.OrderByDescending(x => x.time);


            //通过ToPagedList扩展方法进行分页  
            IPagedList<moment> userPagedList = moment.ToPagedList(pageNumber, pageSize);

            //将分页处理后的列表传给View 
            return View(userPagedList);
        }

        public ActionResult AddMoment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMoment(string detail, moment moment)
        {
            if (Session["nickname"] == null)
            {
                Session.Abandon();
                return Content("<script>alert('无权访问！！');window.location.href='/Login/Index';</script>");

            }
            moment.report_nickname = Session["nickname"].ToString();
            moment.time = DateTime.Now;
            moment.detail = detail;
            db.moment.Add(moment);
            int result = db.SaveChanges();
            if (result > 0)
            {
                //成功提示，并跳转到首页！
                MessageBox.Show("发布成功！");
                return Content("<script>window.location.href='/Student/Index';</script>");
            }
            else
            {
                ViewBag.notice = "保存失败！";
            }
            return View();
        }

        public ActionResult DeleteMoment(int id)
        {
            if (Session["nickname"] == null)
            {
                Session.Abandon();
                return Content("<script>alert('无权访问！！');window.location.href='/Login/Index';</script>");

            }
            moment moment = db.moment.Find(id);
            db.moment.Remove(moment);
            db.SaveChanges();
            MessageBox.Show("删除成功！");
            return RedirectToAction("Index");
        }



        public ActionResult Comment(int? id, int? page)
        {
            if (Session["nickname"] == null)
            {
                Session.Abandon();
                return Content("<script>alert('无权访问！！');window.location.href='/Login/Index';</script>");

            }
            Session["nowid"] = id;
            if (Session["nickname"] == null)
            {
                Session.Abandon();
                return Content("<script>alert('无权访问！！');window.location.href='/Login/Index';</script>");

            }


            var comment = from pl in db.comment
                          where pl.to_moment == id
                          select pl;

            int pageNumber = page ?? 1;

            //每页显示多少条  
            int pageSize = 3;

            //根据ID升序排序  
            comment = comment.OrderByDescending(x => x.time);


            //通过ToPagedList扩展方法进行分页  
            IPagedList<comment> userPagedList = comment.ToPagedList(pageNumber, pageSize);

            //将分页处理后的列表传给View 
            return View(userPagedList);
        }

        public ActionResult AddComment()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddComment(string detail, comment comment, int id)
        {
            if (Session["nickname"] == null)
            {
                Session.Abandon();
                return Content("<script>alert('无权访问！！');window.location.href='/Login/Index';</script>");

            }
            comment.from_who = Session["nickname"].ToString();
            comment.time = DateTime.Now;
            comment.detail = detail;
            comment.to_moment = id;
            int nowid = id;
            db.comment.Add(comment);
            int result = db.SaveChanges();
            if (result > 0)
            {
                //成功提示，并跳转到首页！
                MessageBox.Show("评论成功！");
                return RedirectToAction("Comment", "Student", new { id = nowid });
            }
            else
            {
                ViewBag.notice = "保存失败！";
            }
            return View();

        }
        public ActionResult DeleteComment(int id)
        {
            if (Session["nickname"] == null)
            {
                Session.Abandon();
                return Content("<script>alert('无权访问！！');window.location.href='/Login/Index';</script>");

            }
            comment comment = db.comment.Find(id);
            db.comment.Remove(comment);
            db.SaveChanges();
            MessageBox.Show("删除成功！");
            return RedirectToAction("Comment", "Student", new { id = Session["nowid"] });
        } 

        //问卷

        public ActionResult TextSelf()
        {
            if (Session["nickname"] == null)
            {
                Session.Abandon();
                return Content("<script>alert('无权访问！！');window.location.href='/Login/Index';</script>");

            }
            return View(db.Text_1.ToList());
        }
        [HttpPost]
        public ActionResult TextSubmit()
        {
            if (Session["nickname"] == null)
            {
                Session.Abandon();
                return Content("<script>alert('无权访问！！');window.location.href='/Login/Index';</script>");
            }
            //
            MessageBox.Show("提交成功");
            return RedirectToAction("Index", "Student", new { });


        }
        //咨询
        public ActionResult Appointment()
        {
            if (Session["nickname"] == null)
            {
                Session.Abandon();
                return Content("<script>alert('无权访问！！');window.location.href='/Login/Index';</script>");
            }

            return View(db.Appointment.ToList());
        }
        //预约
        public ActionResult Appoint(string nickname, int id, int time_span)//time_span是当前预约的是哪个时间段的数字
        {
            if (Session["nickname"] == null)
            {
                Session.Abandon();
                return Content("<script>alert('无权访问！！');window.location.href='/Login/Index';</script>");
            }

            Appointment appointment = db.Appointment.Find(id);
            if (appointment.opt1_name == nickname || appointment.opt2_name == nickname ||
                appointment.opt3_name == nickname || appointment.opt4_name == nickname)
            {
                return Content("<script>alert('您这天已经预约过了，请预约其他日子吧！！');window.location.href='/Student/Appointment';</script>");
               
            }
           
                switch (time_span)
                {
                    case 1:
                        appointment.opt1_name = nickname;
                        break;
                    case 2:
                        appointment.opt2_name = nickname;
                        break;
                    case 3:
                        appointment.opt3_name = nickname;
                        break;
                    case 4:
                        appointment.opt4_name = nickname;
                        break;
                }
            int result = db.SaveChanges();
            if (result > 0)
            {
                return Content("<script>alert('预约成功！');window.location.href='/Student/Appointment';</script>");
                //成功提示，并跳转到预约！
            
            }
            else
            {
                ViewBag.notice = "预约失败！";
            }
            return View();
        }
        //取消预约
        public ActionResult DisAppoint(int id, int time_span)//time_span是当前预约的是哪个时间段的数字
        {
            if (Session["nickname"] == null)
            {
                Session.Abandon();
                return Content("<script>alert('无权访问！！');window.location.href='/Login/Index';</script>");
            }

            Appointment appointment = db.Appointment.Find(id);
            
            //根据不同时间段 将其置为空
            switch (time_span)
            {
                case 1:
                    appointment.opt1_name = null;              
                    break;
                case 2:
                    appointment.opt2_name = null;
                    break;
                case 3:
                    appointment.opt3_name = null;
                    break;
                case 4:
                    appointment.opt4_name = null;
                    break;
            }
            int result = db.SaveChanges();
            if (result > 0)
            {
                return Content("<script>alert('取消成功！');window.location.href='/Student/Appointment';</script>");
                //成功提示，并跳转到预约页！
                //MessageBox.Show("取消成功！");
                //return RedirectToAction("Appointment", "Student");
            }
            else
            {
                ViewBag.notice = "取消失败！";
            }
            return View();
        }

        //点赞
        public ActionResult LikeDetail(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                string is_like;

                string name = Session["nickname"].ToString();
                //var query = db.like.Find(id);
                string count = db.like.Where(x => x.like_moments == id).Count().ToString();
                
                int i = db.like.Where(like => like.like_name == name && like.like_moments == id).Select(like => like.id).FirstOrDefault();
                if (i == 0)
                {
                    is_like = "0";
                }
                else
                {
                     is_like = "1";
                }
                //三个位置 第一个是赞数 第二个是是否点赞 
                string[] query = { count, is_like };
                
                return Json(query, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult ClickLike(int id,like like1)
        {
            if (Session["nickname"] == null)
            {
                Session.Abandon();
                return Content("<script>alert('无权访问！！');window.location.href='/Login/Index';</script>");

            }
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                string name = Session["nickname"].ToString();
                int i = db.like.Where(like => like.like_name == name && like.like_moments == id).Select(like => like.id).FirstOrDefault();
                if (i == 0)//加id
                {
                    like1.like_name = name;
                    like1.like_moments = id;
                    db.like.Add(like1);
                    db.SaveChanges();
                    MessageBox.Show("点赞成功！");
                    return RedirectToAction("Index", "Student", new { page = Session["nowPage"] });
                }
                else
                {
                    
                    like likes = db.like.Find(i);
                    db.like.Remove(likes);
                    db.SaveChanges();
                    MessageBox.Show("取消成功！");
                    return RedirectToAction("Index", "Student", new { page = Session["nowPage"] });
                }
            }
        }

    }
}
