using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC4Demo.Models
{

    //a couple of things to mention here--- this person class was created after the fact 
    //since the student and instructor classes were made prior with regards to simplifying those models
    public abstract class Person
    {
        [Key]
        public int PersonID { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [StringLength(50, MinimumLength = 1)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Column("FirstName")]
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string FirstMidName { get; set; }

        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }
    }

}