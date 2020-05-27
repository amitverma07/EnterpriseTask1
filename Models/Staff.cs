using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ADKZProject.Models
{
    public class Staff
    {
        public Staff()
        {
           this. Tasks = new HashSet<Task>();
           this. Notifications = new HashSet<Notification>();
        }
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(maximumLength: 20)]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(6), MaxLength(20)]
        public string Password { get; set; }
        public Nullable<int> Phone { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        [Required]
        public decimal Salary { get; set; }

       public Guid ManagerId { get; set; }
        public virtual Manager Manager { get; set; }
    }
}