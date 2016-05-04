﻿using System;
using System.Collections.Generic;

namespace MVC4Demo.Models
{

    public enum Grade
    {
        A,B,C,D,F
    }
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        public Grade? Grade { get; set; } //the question mark beside it indicates the fiekd is nullable

        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
    }
}