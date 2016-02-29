﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class ProjectBaseModel
    {
        [Required(ErrorMessage = "ProjectName is required!")]
        [MaxLength(50, ErrorMessage = "ProjectName must be less than 50 characters!")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "ProjectNo is required!")]
        [MaxLength(50, ErrorMessage = "ProjectNo must be less than 50 characters!")]
        public string ProjectNo { get; set; }
    }

    public class ProjectAddModel: ProjectBaseModel
    {
        [Required(ErrorMessage = "CustomerId is required!")]
        public int CustomerId { get; set; }

    }

    public class ProjectUpdateModel: ProjectBaseModel
    {
        [Required(ErrorMessage = "Completed is required!")]
        public bool Completed { get; set; }
    }

    public class ProjectSearchModel : ProjectBaseModel
    {
        public int CustomerId { get; set; }
        public int ProjectId { get; set; }
        public string CreateDate { get; set; }
        public bool Completed { get; set; }
    }
}
