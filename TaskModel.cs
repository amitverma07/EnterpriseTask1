using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADKZProject.Models
{
    public class TaskModel
    {
        public Guid StaffId { get; set; }
        public Guid ProjectId { get; set; }
        [Required]
        [MaxLength(100,ErrorMessage ="Too long")]
        public string Title { get; set; }
        [Required]
        [MaxLength(500)]
        public string Content { get; set; }
        [Required]
        public DateTime Deadline { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
    }
}