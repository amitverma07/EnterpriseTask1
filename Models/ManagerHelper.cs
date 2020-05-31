using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADKZProject.Models
{
    public static class ManagerHelper
    {
        public static bool DeleteStaff(Guid Staffid, Context db)
        {
            var x = db.Staffs.Find(Staffid);
            var tasks = db.Tasks.Where(t => t.StaffId == Staffid).ToList();
            var notifications = db.Notifications.Where(n => n.StaffId == Staffid).ToList();

            foreach (var item in tasks)
            {
                db.Tasks.Remove(item);
                db.SaveChanges();
            }
            foreach (var item in notifications)
            {
                db.Notifications.Remove(item);
                db.SaveChanges();
            }
            db.Staffs.Remove(x);
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
    }
}