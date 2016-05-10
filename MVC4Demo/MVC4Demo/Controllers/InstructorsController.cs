using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC4Demo.DAL;
using MVC4Demo.Models;
using MVC4Demo.ViewModels;
using System.Data.Entity.Infrastructure;

namespace MVC4Demo.Controllers
{
    public class InstructorsController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Instructors
        //modified to add additional related data
        public ActionResult Index( int? id, int? courseID)
        {
            var viewModel = new InstructorIndexData();
            //The code begins by creating an instance of the view model and 
            //putting in it the list of instructors. The code specifies eager 
            //loading for the Instructor.OfficeAssignment and the Instructor.Courses 
            //navigation property.

            //The second Include method loads Courses, and for each Course that is
            //loaded it does eager loading for the Course.Department navigation property.
            viewModel.Instructors = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses.Select(c => c.Department))
                .OrderBy(i => i.LastName);

            if (id != null)
            {
                //The Where method returns a collection, but in this case the criteria 
                //passed to that method result in only a single Instructor entity being
                //returned. The Single method converts the collection into a single 
                //Instructor entity, which gives you access to that entity's Courses property
                ViewBag.PersonID = id.Value;
                viewModel.Courses = viewModel.Instructors.Where(
                    i => i.PersonID == id.Value).Single().Courses;
            }

            if (courseID != null)
            {
                ViewBag.CourseID = courseID.Value;
                /*
                //eager loading
                viewModel.Enrollments = viewModel.Courses.Where(
                    x => x.CourseID == courseID).Single().Enrollments;
                    */

                //explicit loading

                //first get entry
                var selectedCourse = viewModel.Courses.Where(x => x.CourseID == courseID).Single();
                //using the selected course, find out who is enrolled and load this as a db selection
                //and reload this
                db.Entry(selectedCourse).Collection(x => x.Enrollments).Load();
                //go through the 
                foreach (Enrollment enrollment in selectedCourse.Enrollments)
                {
                    //explicitly loads each Enrollment entity's related Student entity
                    db.Entry(enrollment).Reference(x => x.Student).Load();
                }

                viewModel.Enrollments = selectedCourse.Enrollments;

            }


            return View(viewModel);
        }

        // GET: Instructors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // GET: Instructors/Create
        public ActionResult Create()
        {
            ViewBag.PersonID = new SelectList(db.OfficeAssignments, "PersonID", "Location");
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonID,LastName,FirstMidName,HireDate")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Instructors.Add(instructor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PersonID = new SelectList(db.OfficeAssignments, "PersonID", "Location", instructor.PersonID);
            return View(instructor);
        }

        // GET: Instructors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Instructor instructor = db.Instructors.Find(id); //setup for a drop down but not a text box

            Instructor instructor = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .Where(i => i.PersonID == id)
                .Single();

            PopulateAssignedCourseData(instructor); // for eager loading of data

            if (instructor == null)
            {
                return HttpNotFound();
            }
            //ViewBag.PersonID = new SelectList(db.OfficeAssignments, "PersonID", "Location", instructor.PersonID); 
            //was replaced by the PopulateAssignedCourseData method
            return View(instructor);
        }

        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = db.Courses;
            var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.CourseID));
            var viewModel = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.CourseID)
                });
            }
            ViewBag.Courses = viewModel;
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection formCollection, string[] selectedCourses)
        {
            var instructorToUpdate = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .Where(i => i.PersonID == id)
                .Single();

            if (TryUpdateModel(instructorToUpdate, "",
               new string[] { "LastName", "FirstMidName", "HireDate", "OfficeAssignment" }))
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment.Location))
                    {
                        instructorToUpdate.OfficeAssignment = null;
                    }

                    UpdateInstructorCourses(selectedCourses, instructorToUpdate); //

                    db.Entry(instructorToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            // ViewBag.PersonID = new SelectList(db.OfficeAssignments, "PersonID", "Location", id);
            //was replaced by the PopulateAssignedCourseData method

            PopulateAssignedCourseData(instructorToUpdate);
            return View(instructorToUpdate);
        }

    private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            //If no check boxes were selected, the code in UpdateInstructorCourses 
            //initializes the Courses navigation property with an empty collection:
            if (selectedCourses == null)
            {
                instructorToUpdate.Courses = new List<Course>();
                return;
            }

            //The code then loops through all courses in the database and checks each
            //course against the ones currently assigned to the instructor versus the 
            //ones that were selected in the view. To facilitate efficient lookups, the
            //latter two collections are stored in HashSet objects
            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>
                (instructorToUpdate.Courses.Select(c => c.CourseID));
            foreach (var course in db.Courses)
            {
                //If the check box for a course was selected but the course isn't in
                //the Instructor.Courses navigation property, the course is added to
                //the collection in the navigation property.
                if (selectedCoursesHS.Contains(course.CourseID.ToString()))
                {
                    if (!instructorCourses.Contains(course.CourseID))
                    {
                        instructorToUpdate.Courses.Add(course);
                    }
                }
                //If the check box for a course wasn't selected, but the course is 
                //in the Instructor.Courses navigation property, the course is removed 
                //from the navigation property.
                else
                {
                    if (instructorCourses.Contains(course.CourseID))
                    {
                        instructorToUpdate.Courses.Remove(course);
                    }
                }
            }
        }
        // GET: Instructors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }


        // POST: Instructors/Delete/5
        //If the instructor is assigned as administrator of any department
        //, removes theinstructor assignment from that department. Without 
        //this code, you would get a referential integrity error if you 
        //tried to delete an instructor who was assigned as administrator 
        //for a department. 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instructor instructor = db.Instructors
        .Include(i => i.OfficeAssignment)
        .Where(i => i.PersonID == id)
        .Single();

            db.Instructors.Remove(instructor);

            var department = db.Departments
    .Where(d => d.PersonID == id)
    .SingleOrDefault();
            if (department != null)
            {
                department.PersonID = null;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
