using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADKZProject.Models
{
    public class Manager
    {
        public Manager()
        {
            this.Staffs = new HashSet<Staff>();
            this.Projects =new HashSet<Project>();
        }
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [MaxLength(20)]
        public string Password { get; set; }
        public Nullable<int> Phone { get; set; }
       public virtual ICollection<Staff> Staffs { get; set; }
       public virtual ICollection<Project> Projects { get; set; }
    }
}