using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MVC4Demo.Models
{
    //the : Person was added afterwards, this is how to reference the person abstract class
    public class Instructor : Person
    {
       // public int PersonID { get; set; } //you reference the personID now, instead of this

        /*
        [Required]
        [Display(Name = "LastName")]
        [StringLength(50, MinimumLength = 1)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "FirstName")]
        [StringLength(50, MinimumLength = 1)]
        public string FirstMidName { get; set; }
        */

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }
        
        /*
        public string FullName
        {
            get { return LastName + ", " + FirstMidName; }
        }
        */

        public virtual ICollection<Course> Courses { get; set; }
        public virtual OfficeAssignment OfficeAssignment { get; set; }
    }
}