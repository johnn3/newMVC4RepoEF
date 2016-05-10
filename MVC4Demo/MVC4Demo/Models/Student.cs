using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC4Demo.Models
{
    public class Student : Person //now references the person abstract class
    {
        //this all gets referenced in the person abstract class
        /*
        public int PersonID { get; set; }
        [StringLength(50, MinimumLength = 1)]
        public string LastName { get; set; }
        [StringLength(50, MinimumLength =1, ErrorMessage = "First name cannot be longer than 50 characters.")]

        public string FullName
        {
            get { return LastName + ", " + FirstMidName; }
        }


        //The addition of the Column attribute changes the model backing the SchoolContext, 
        //so it won't match the database. Enter the following commands in the PMC to create another migration:

        //add-migration ColumnFirstName
        //update-database

        [Column("FirstName")]
        public string FirstMidName { get; set; }
        */ 
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime EnrollmentDate { get; set; }
            public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}