﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC4Demo.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        //the Column attribute is being used to change SQL data
        //type mapping so that the column will be defined using the SQL Server money type in the database
        [Display(Name = "Administrator")]

        //•A department may or may not have an administrator, and an administrator 
        //is always an instructor. Therefore the InstructorID property is included 
        //as the foreign key to the Instructor entity, and a question mark is added 
        //after the int type designation to mark the property as nullable.The navigation 
        //property is named Administrator but holds an Instructor entity
        public int? IntructorID { get; set; }

        public virtual Instructor Administrator { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}