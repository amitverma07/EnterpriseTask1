using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADKZProject.Models
{
    public class ProjectModel
    {
        [Required]
        public decimal Budget { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Your title too lone")]
        public string ProjectTitle { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string ProjectContent { get; set; }
    }
}