using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ADKZProject.Models
{
    public class Context : DbContext
    {
        public Context() : base("conn") { }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            /*
                        modelBuilder.Entity<Staff>()
                        .HasRequired(c => c.Tasks)
                        .WithMany()
                        .WillCascadeOnDelete(false);

                        modelBuilder.Entity<Manager>()
                       .HasRequired(c => c.Staffs)
                       .WithMany()
                       .WillCascadeOnDelete(false);*/
        }


    }
}