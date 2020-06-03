using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADKZProject.Models
{
    public static class StaffHelper
    {
        public static bool AddStaff(StaffModel staff, Guid managerId, Context db)
        {
            var x = new Staff();
            x.Name = staff.Name;
            x.Email = staff.Email;
            x.Password = staff.Password;
            x.ManagerId = managerId;
            x.Salary = staff.Salary;
            x.Phone = staff.Phone != null ? staff.Phone : null;
            db.Staffs.Add(x);
            int r = db.SaveChanges();
            if (r > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ChangeStatus(Guid id, int Status, Context db)
        {
            bool allFinished = false;
            var x = db.Tasks.Find(id);
            var projectId = x.ProjectId;
            var project = db.Projects.Where(p => p.Id == projectId).FirstOrDefault();

            x.Status = Status;
            if (Status == 3)
            {
                allFinished = true;
                x.IsFinished = true;
                x.FinishTime = DateTime.Now;

                foreach (var item in project.Tasks.ToList())
                {
                    if (item.Status != 3)
                    {
                        allFinished = false;
                    }
                }
            }
            db.Tasks.AddOrUpdate(x);
            db.SaveChanges();

            if (allFinished)
            {
                project.FinishedTime = DateTime.Now;
                project.IsFinished = true;
                decimal TotalCost = 0;
                foreach (var item in project.Tasks.ToList())
                {
                    var salary = db.Staffs.Find(item.StaffId).Salary;
                    TimeSpan t = (TimeSpan)(item.CreateTime - item.FinishTime);
                    TotalCost += ((int)(t.Days) + 1) * salary;
                }
                project.RealBudget = TotalCost;
                project.IsFinished = true;
                project.FinishedTime = DateTime.Now;
                db.Projects.AddOrUpdate(project);
                db.SaveChanges();

                var n = new Notification();
                n.Title = $"Finished...";
                n.Content = $"{project.ProjectTitle} finished...";
                n.StaffId = x.StaffId;
                n.IsChecked = false;
                n.IsBeyondDeadline = true;

                db.Notifications.Add(n);
                db.SaveChanges();
            }
            return true;
        }
    }
}