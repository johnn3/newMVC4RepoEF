using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System;
using System.Collections.Generic;

namespace MVC4Demo.Models
{
    public class Student
    {
        public class Student
        {
            public int StudentID { get; set; }
            public string LastName { get; set; }
            public string FirstMidName { get; set; }
            public DateTime EnrollmentDate { get; set; }
            public virtual ICollection<Enrollment> Enrollments { get; set; }
        }
    }
}