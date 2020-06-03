using ADKZProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADKZProject.Controllers
{
    [StaffAuthority]
    public class StaffController : Controller
    {
        Context db = new Context();
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (Session["Email"] != null)
            {
                string Email = Session["Email"].ToString();
                Guid id = db.Staffs.Where(s => s.Email == Email).FirstOrDefault().Id;
                ViewBag.name = Email;
                ViewBag.id = id;
                var x = db.Tasks.Where(t => t.StaffId == id).ToList();

                return View(x);
            }
            else
            {
                return Redirect("StaffLogin");
            }
        }
        [AllowAnonymous]
        public ActionResult StaffLogin()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult StaffLogin(StaffModel staff)
        {
            var x = db.Staffs.Where(s => s.Email == staff.Email && s.Password == staff.Password).FirstOrDefault();
            if (x != null)
            {
                Session["Email"] = staff.Email;
                return Redirect(Url.Action("Index"));
            }
            else
            {
                ViewBag.message = "Didn't found";
                return View(new StaffModel());
            }
        }

        public ActionResult Message(Guid id, string Title, string Content)
        {
            var x = new Notification();
            x.StaffId = id;
            x.Title = Title;
            x.Content = Content;
            x.IsChecked = false;
            x.IsBeyondDeadline = false;

            db.Notifications.Add(x);
            db.SaveChanges();
            return Redirect(Url.Action("Index"));
        }

        public ActionResult ChangeStatus()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangeStatus(Guid id, int Status)
        {
            bool isSuccess = StaffHelper.ChangeStatus(id, Status, db);
            if (isSuccess)
            {
                return Redirect(Url.Action("Index", "Staff"));
            }
            else
            {
                return View();
            }
        }
    }
}