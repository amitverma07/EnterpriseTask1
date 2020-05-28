using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADKZProject.Controllers
{
    //[ADKZAuthority]
    public class HomeController : Controller
    {
        Context db = new Context();
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (Session["LoginName"] != null)
            {
                string name = Session["LoginName"].ToString();

                ViewBag.name = name;
                var id = db.Managers.Where(m => m.Email == name).FirstOrDefault().Id;
                ViewBag.Projects = db.Projects.Where(p => p.Manager.Email == name).ToList();
                var staff = db.Staffs.Where(s => s.ManagerId == id);

                var projects = db.Projects.Where(p => p.Manager.Email == name);
                foreach (var p in projects.ToList())
                {
                    foreach (var task in p.Tasks.ToList())
                    {
                        TimeSpan t = (DateTime.Now - task.Deadline);
                        if (t.TotalDays > 0)
                        {
                            var notificationList = db.Notifications.Where(nt => nt.Staff.ManagerId == id);
                            if (!notificationList.Any(n => n.Task == task.Id))
                            {
                                var staffName = db.Staffs.Find(task.StaffId).Name;
                                var n = new Notification();
                                n.Title = $"Warning...";
                                n.Content = $"{staffName} didn't finish task on time...,Fire him.";
                                n.StaffId = task.StaffId;
                                n.IsChecked = false;
                                n.IsBeyondDeadline = true;
                                n.Task = task.Id;
                                db.Notifications.Add(n);
                                db.SaveChanges();
                            }
                        }
                    }
                }

                ViewBag.notification = db.Notifications.Where(n => n.Staff.ManagerId == id);
                var x = db.Staffs.Where(s => s.ManagerId == id);

                return View(x);
            }
            else
            {
                return Redirect("/Home/ManagerLogin");
            }
            /* ViewBag.name = Session["LoginName"].ToString() != null ? Session["LoginName"].ToString() : "bad";
            return View();*/
        }
        [AllowAnonymous]
        public ActionResult ManagerRegister()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ManagerRegister(ManagerModel manager)
        {
            if (!ModelState.IsValid)
            {
                return View(new ManagerModel());
            }

            Session["LoginName"] = manager.Email;
            Manager x = new Manager();
            x.Email = manager.Email;
            x.Name = manager.Name;
            x.Password = manager.Password;
            db.Managers.Add(x);
            int result = db.SaveChanges();
            if (result > 0)
            {
                return Redirect(Url.Action("Index"));
            }
            else
            {
                return View(new ManagerModel());
            }
        }
        [AllowAnonymous]
        public ActionResult ManagerLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ManagerLogin(ManagerModel model)
        {
            var x = db.Managers.Where(m => m.Email == model.Email && m.Password == model.Password && m.Name == model.Name).FirstOrDefault();
            if (x != null)
            {
                Session["LoginName"] = x.Email;
                return Redirect(Url.Action("Index", "Home"));
            }
            else
            {
                ViewBag.message = "Didn't found";
                return View();
            }
        }
    }
}