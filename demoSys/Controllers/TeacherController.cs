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
    public class TeacherController : Controller
    {
        // GET: Teacher
        private Consulting_systemEntities db = new Consulting_systemEntities();
        public ActionResult Index()
        {
            if ((Session["nickname"] == null) || ((int)Session["power"] != 1))
            {
                Session.Abandon();
                return Content("<script>alert('无权访问！！');window.location.href='/Login/Index';</script>");

            }

            return View(db.Appointment.ToList());
        }
        public ActionResult PublishAppointment()
        {
            if ((Session["nickname"] == null) || ((int)Session["power"] != 1))
            {
                Session.Abandon();
                return Content("<script>alert('无权访问！！');window.location.href='/Login/Index';</script>");

            }

            return View(db.Appointment.ToList());
        }
       
        [HttpPost]
        public ActionResult CreareAppoint(int opt1, int opt2, int opt3, int opt4, DateTime date, Appointment appointment)
        {



            appointment.opt_time1 = opt1;
            appointment.opt_time2 = opt2;
            appointment.opt_time3 = opt3;
            appointment.opt_time4 = opt4;


            appointment.day = date;
            db.Appointment.Add(appointment);
            db.SaveChanges();
            MessageBox.Show("发布成功！");
            return RedirectToAction("Index", "Teacher", new { });
        }
        public ActionResult DeleteAppoint(int id)
        {
            Appointment appointment = db.Appointment.Find(id);
            db.Appointment.Remove(appointment);
            db.SaveChanges();
            MessageBox.Show("删除成功！");
            return RedirectToAction("Index", "Teacher", new { });
        }
    }
}