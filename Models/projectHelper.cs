using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADKZProject.Models
{
    public static class projectHelper
    {
        public static bool AddProject(Guid id, ProjectModel p, Context c)
        {
            var project = new Project();
            project.ManagerId = id;
            project.Budget = p.Budget;
            project.CreateTime = DateTime.Now;
            project.Deadline = p.Deadline;
            project.ProjectTitle = p.ProjectTitle;
            project.IsFinished = false;
            project.Priority = 1;
            project.ProjectContent = p.ProjectContent;
            c.Projects.Add(project);
            int r = c.SaveChanges();
            if (r > 0)
            {
                return true;
            }
            return false;
        }

        public static bool DeleteProject(Guid ProjectId, Context c)
        {
            var x = c.Projects.Find(ProjectId);
            if (x != null)
            {
                foreach (var item in x.Tasks.ToList())
                {
                    c.Tasks.Remove(item);
                    c.SaveChanges();
                }
                c.Projects.Remove(x);
                c.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}