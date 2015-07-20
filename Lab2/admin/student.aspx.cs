using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Lab2.Models;
using System.Web.ModelBinding;
namespace Lab2
{
    public partial class student : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                if (Request.QueryString.Keys.Count > 0)
                {
                    GetStudent();
                }
            }
        }

        protected void GetStudent()
        {
            try
            {
                //connect
                using (ContosoEntitiesEF db = new ContosoEntitiesEF())
                {
                    Int32 StudentID = Convert.ToInt32(Request.QueryString["StudentID"]);

                    //populate a student instance with the StudentID from the URL parameter
                    Student s = (from objS in db.Students
                                 where objS.StudentID == StudentID
                                 select objS).FirstOrDefault();

                    //map the student properties to the form controls if we found a match
                    if (s != null)
                    {
                        txtLastName.Text = s.LastName;
                        txtFirstName.Text = s.FirstMidName;
                        txtEnrollmentDate.Text = s.EnrollmentDate.ToString("yyyy-MM-dd");
                    }

                    //enrollments - this code goes in the same method that populates the student form but below the existing code that's already in GetStudent()              
                    var objE = (from en in db.Enrollments
                                join c in db.Courses on en.CourseID equals c.CourseID
                                join d in db.Departments on c.DepartmentID equals d.DepartmentID
                                where en.StudentID == StudentID
                                select new { en.EnrollmentID, en.Grade, c.Title, d.Name });

                    grdCourses.DataSource = objE.ToList();
                    grdCourses.DataBind();


                    //clear dropdowns
                    ddlDepartment.ClearSelection();
                    ddlCourse.ClearSelection();

                    //fill departments to dropdown
                    var deps = from d in db.Departments
                               orderby d.Name
                               select d;

                    ddlDepartment.DataSource = deps.ToList();
                    ddlDepartment.DataBind();

                    //add default options to the 2 dropdowns
                    ListItem newItem = new ListItem("-Select-", "0");
                    ddlDepartment.Items.Insert(0, newItem);
                    ddlCourse.Items.Insert(0, newItem);

                    //show the course panel
                    pnlCourses.Visible = true;
                }
            }
            catch (Exception a)
            {
                Response.Redirect("/error.aspx");
            }
            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //connect
                using (ContosoEntitiesEF conn = new ContosoEntitiesEF())
                {

                    Student s = new Student();


                    if (Request.QueryString["StudentID"] != null)
                    {
                        Int32 StudentID = Convert.ToInt32(Request.QueryString["StudentID"]);

                        s = (from stu in conn.Students where stu.StudentID == StudentID select stu).FirstOrDefault();
                    }

                    s.FirstMidName = txtFirstName.Text;
                    s.LastName = txtLastName.Text;
                    s.EnrollmentDate = Convert.ToDateTime(txtEnrollmentDate.Text);

                    if (Request.QueryString.Count == 0)
                    {
                        conn.Students.Add(s);
                    }
                    conn.SaveChanges();

                    Response.Redirect("students.aspx");
                }
            }
            catch (Exception v)
            {
                Response.Redirect("/error.aspx");
            }
            
        }

        protected void grdCourses_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                //get the selected EnrollmentID
                Int32 EnrollmentID = Convert.ToInt32(grdCourses.DataKeys[e.RowIndex].Values["EnrollmentID"]);

                using (ContosoEntitiesEF db = new ContosoEntitiesEF())
                {
                    Enrollment objE = (from en in db.Enrollments
                                       where en.EnrollmentID == EnrollmentID
                                       select en).FirstOrDefault();

                    //process the deletion
                    db.Enrollments.Remove(objE);
                    db.SaveChanges();

                    //repopulate the page
                    GetStudent();
                }
            }
            catch (Exception f)
            {
                Response.Redirect("/error.aspx");
            }
            
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (ContosoEntitiesEF conn = new ContosoEntitiesEF())
                {
                    Int32 StudentID = Convert.ToInt32(Request.QueryString["StudentID"]);
                    Int32 CourseID = Convert.ToInt32(ddlCourse.SelectedValue);

                    Enrollment objE = new Enrollment();

                    objE.StudentID = StudentID;
                    objE.CourseID = CourseID;

                    conn.Enrollments.Add(objE);
                    conn.SaveChanges();

                    GetStudent();
                }
            }
            catch (Exception g)
            {
                Response.Redirect("/error.aspx");
            }
            
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (ContosoEntitiesEF conn = new ContosoEntitiesEF())
                {
                    Int32 DepartmentID = Convert.ToInt32(ddlDepartment.SelectedValue);

                    var objC = from c in conn.Courses where c.DepartmentID == DepartmentID orderby c.Title select c;

                    ddlCourse.DataSource = objC.ToList();
                    ddlCourse.DataBind();

                    ListItem newItem = new ListItem("-Select-", "0");
                    ddlCourse.Items.Insert(0, newItem);
                }
            }
            catch (Exception h)
            {
                Response.Redirect("/error.aspx");
            }
            
        }
    }
}
