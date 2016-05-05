using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC4Demo.Models
{
    public class OfficeAssignment
    {
        //There's a one-to-zero-or-one relationship  between the Instructor and the OfficeAssignment entities

        [Key][ForeignKey("Instructor")]
        public int InstructorID { get; set; }
        [StringLength(50)][Display(Name = "Office Location")] 
        public virtual Instructor Instructor { get; set; }
    }
}