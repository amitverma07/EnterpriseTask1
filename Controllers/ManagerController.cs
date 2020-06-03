using ADKZProject.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADKZProject.Controllers
{
    [ADKZAuthority]
    public class ManagerController : Controller
    {
        Models.Context db = new Models.Context();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddStaff()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddStaff(StaffModel staff)
        {
            if (!ModelState.IsValid)
            {
                return View(new StaffModel());
            }

            var managerEmail = Session["LoginName"].ToString();
            var managerId = db.Managers.Where(m => m.Email == managerEmail).FirstOrDefault().Id;

            bool isSuccess = StaffHelper.AddStaff(staff, managerId, db);
            if (isSuccess)
            {
                return Redirect(Url.Action("Index", "Home"));
            }
            else
            {
                return View(new StaffModel());
            }
        }

        public ActionResult DeleteStaff(Guid id)
        {
            bool isSuccess = ManagerHelper.DeleteStaff(id, db);
            if (isSuccess)
            {
                return Redirect(Url.Action("Index", "Home"));
            }
            else
            {
                return View();
            }
        }

        public ActionResult EditStaff(Guid id)
        {
            var x = db.Staffs.Find(id);
            return View(x);
        }
        [HttpPost]
        public ActionResult EditStaff(Staff s)
        {
            db.Staffs.AddOrUpdate(s);
            int r = db.SaveChanges();
            if (r > 0)
            {
                return Redirect(Url.Action("Index", "Home"));
            }
            return Redirect(Url.Action("EditStaff", "Manager", new { id = s.Id }));
        }

        public ActionResult AddProject()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddProject(ProjectModel p)
        {
            if (!ModelState.IsValid)
            {
                return View(new ProjectModel());
            }
            var managerEmail = Session["LoginName"].ToString();
            var managerId = db.Managers.Where(m => m.Email == managerEmail).FirstOrDefault().Id;

            bool isSuccess = projectHelper.AddProject(managerId, p, db);
            if (isSuccess)
            {
                return Redirect(Url.Action("Index", "Home"));
            }
            else
            {
                return View(new ProjectModel());
            }
        }

        public ActionResult AddTask()
        {
            var managerEmail = Session["LoginName"].ToString();
            var managerId = db.Managers.Where(m => m.Email == managerEmail).FirstOrDefault().Id;
            ViewBag.Staffs = new SelectList(db.Staffs.Where(s => s.ManagerId == managerId), "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult AddTask(TaskModel task, Guid projectId, Guid Staffs)
        {
            var managerEmail = Session["LoginName"].ToString();
            var managerId = db.Managers.Where(m => m.Email == managerEmail).FirstOrDefault().Id;
            ViewBag.Staffs = new SelectList(db.Staffs.Where(s => s.ManagerId == managerId), "Id", "Name");

            bool isSuccess = TaskHelper.AddTask(task, projectId, Staffs, db);
            if (isSuccess)
            {
                return Redirect(Url.Action("Index", "Home"));
            }
            return View(new Task());
        }

        public ActionResult DeleteProject(Guid ProjectId)
        {
            bool isSuccess = projectHelper.DeleteProject(ProjectId, db);
            if (isSuccess)
            {
                return Redirect(Url.Action("Index", "Home"));
            }
            else
            {
                return View();
            }
        }

        public ActionResult DeleteTask(Guid TaskId)
        {
            var x = db.Tasks.Find(TaskId);
            db.Tasks.Remove(x);
            db.SaveChanges();
            return Redirect(Url.Action("Index", "Home"));
        }

        public ActionResult ReadMessage(Guid id)
        {
            var x = db.Notifications.Find(id);
            x.IsChecked = true;
            db.SaveChanges();
            return View(x);
        }

        public ActionResult ChangePriority()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePriority(Guid id, int Priority)
        {
            var x = db.Tasks.Find(id);
            x.Priority = Priority;
            db.SaveChanges();
            return Redirect(Url.Action("Index", "Home"));
        }

        public ActionResult CheckAllNotification()
        {
            var notifications = db.Notifications;
            return View(notifications);
        }


        public ActionResult UnfinishedWithDeadline()
        {
            var PassedDeadLineTask = db.Tasks
                .Where(t => t.IsFinished == false && t.Deadline < DateTime.Now)
                .ToList();
            return View(PassedDeadLineTask);
        }

        public ActionResult BeyondBudget()
        {
            var x = db.Projects.Where(p => p.IsFinished == true && p.RealBudget - p.Budget > 0);
            return View(x);
        }
    }
}