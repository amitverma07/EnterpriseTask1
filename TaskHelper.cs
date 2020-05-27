using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADKZProject.Models
{
    public static class TaskHelper
    {
        public static bool AddTask(TaskModel task, Guid projectId, Guid StaffsId,Context db)
        {
            var x = new Task();
            x.Title = task.Title;
            x.ProjectId = projectId;
            x.Content = task.Content;
            x.StaffId = StaffsId;
            x.CreateTime = DateTime.Now;
            x.Deadline = task.Deadline;
            x.IsFinished = false;
            x.Priority = 1;
            x.Status = 1;
            db.Tasks.Add(x);
            var r = db.SaveChanges();
            if (r >=0)
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