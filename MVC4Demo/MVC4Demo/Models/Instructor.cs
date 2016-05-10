﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MVC4Demo.Models
{
        public class Instructor : Person
        {
            [DataType(DataType.Date)]
            [Display(Name = "Hire Date")]
            public DateTime HireDate { get; set; }

            public virtual ICollection<Course> Courses { get; set; }
            public virtual OfficeAssignment OfficeAssignment { get; set; }
        }
    }
}