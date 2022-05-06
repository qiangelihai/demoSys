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
    public class momentsController : Controller
    {
        private Consulting_systemEntities db = new Consulting_systemEntities();

        // GET: moments
        public ActionResult Index(int? id, int? page)
        {
            if (Session["nickname"] == null)
            {
                Session.Abandon();
                return Content("<script>alert('无权访问！！');window.location.href='/Login/Index';</script>");

            }


            var moment = from pl in db.moment
                          
                          select pl;

       


            int pageNumber = page ?? 1;

            //每页显示多少条  
            int pageSize = 6;


            moment = moment.OrderByDescending(x => x.time);

            //通过ToPagedList扩展方法进行分页  
            IPagedList<moment> userPagedList = moment.ToPagedList(pageNumber, pageSize);

            //将分页处理后的列表传给View 
            return View(userPagedList);
            //return View(db.moment.ToList());
        }

        // GET: moments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            moment moment = db.moment.Find(id);
            if (moment == null)
            {
                return HttpNotFound();
            }
            return View(moment);
        }

        // GET: moments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: moments/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,report_nickname,detail,picture_1,picture_2,picture_3,time,title")] moment moment)
        {
            if (ModelState.IsValid)
            {
                db.moment.Add(moment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(moment);
        }

        // GET: moments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            moment moment = db.moment.Find(id);
            if (moment == null)
            {
                return HttpNotFound();
            }
            return View(moment);
        }

        // POST: moments/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,report_nickname,detail,picture_1,picture_2,picture_3,time,title")] moment moment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(moment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(moment);
        }

        // GET: moments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            moment moment = db.moment.Find(id);
            if (moment == null)
            {
                return HttpNotFound();
            }
            return View(moment);
        }

        // POST: moments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            moment moment = db.moment.Find(id);
            db.moment.Remove(moment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
