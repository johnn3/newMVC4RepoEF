using MVC4Demo.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MVC4Demo.DAL
{
    public class SchoolContext : DbContext
    {
        public DbSet<Person> People { get; set; } //added later after the already established inst and stud.
        public DbSet<Student> Students{get; set;}
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating (DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //For the many - to - many relationship between the Instructor and Course entities,
            //the code specifies the table and column names for the join table.Code First can 
            //configure the many - to - many relationship for you without this code, but if you
            //don't call it, you will get default names such as InstructorInstructorID for the 
            //InstructorID column.
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Instructors).WithMany(i => i.Courses)
                .Map(t => t.MapLeftKey("CourseID")
                .MapRightKey("PersonID").ToTable("CourseInstructor"));
        }

    }
}