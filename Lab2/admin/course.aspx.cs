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
    public partial class course : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetDepartments();
                if (!String.IsNullOrEmpty(Request.QueryString["CourseID"]))
                {
                    //we have a url parameter if the count is > 0 so populate the form
                    GetCourse();
                }
            }
        }

        protected void GetDepartments()
        {
            try
            {
                using (ContosoEntitiesEF conn = new ContosoEntitiesEF())
                {
                    //use link to query the Departments model
                    var deps = from d in conn.Departments
                               select new { d.DepartmentID, d.Name };

                    //bind the query result to the ddl
                    ddlDepartments.DataSource = deps.ToList();
                    ddlDepartments.DataBind();
                }
            }
            catch (Exception e)
            {
                Response.Redirect("/error.aspx");
            }
            //connect using our connection string from web.config and EF context class
            
        }

        protected void GetCourse()
        {
            try
            {
                //connect
                using (ContosoEntitiesEF conn = new ContosoEntitiesEF())
                {
                    //get id from url parameter and store in a variable
                    Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);

                    var c = (from cour in conn.Courses
                             where cour.CourseID == CourseID
                             select cour).FirstOrDefault();

                    //populate the form from our object
                    txtTitle.Text = c.Title;
                    txtCredits.Text = c.Credits.ToString();
                    ddlDepartments.SelectedValue = Convert.ToString(c.DepartmentID);

                    var objS = (from s in conn.Students
                                join enr in conn.Enrollments on s.StudentID equals enr.StudentID
                                join co in conn.Courses on enr.CourseID equals co.CourseID
                                where enr.CourseID == CourseID
                                select new { enr.EnrollmentID, s.FirstMidName, s.LastName });

                    ddlStudent.ClearSelection();

                    var stus = from s in conn.Students
                               orderby s.LastName
                               select new { s.StudentID, Name = string.Concat(s.LastName, ", ", s.FirstMidName) };

                    ddlStudent.DataSource = stus.ToList();
                    ddlStudent.DataBind();

                    ListItem newItem = new ListItem("-Select-", "0");
                    ddlStudent.Items.Insert(0, newItem);

                    pnlStudents.Visible = true;

                }
            }
            catch (Exception e)
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
                    //instantiate a new deparment object in memory
                    Cours c = new Cours();

                    //decide if updating or adding, then save
                    if (!String.IsNullOrEmpty(Request.QueryString["CourseID"]))
                    {
                        Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);

                        c = (from cour in conn.Courses
                             where cour.CourseID == CourseID
                             select cour).FirstOrDefault();
                    }

                    //fill the properties of our object from the form inputs
                    c.Title = txtTitle.Text;
                    c.Credits = Convert.ToInt32(txtCredits.Text);
                    c.DepartmentID = Convert.ToInt32(ddlDepartments.SelectedValue);

                    if (String.IsNullOrEmpty(Request.QueryString["CourseID"]))
                    {
                        conn.Courses.Add(c);
                    }
                    conn.SaveChanges();

                    //redirect to updated departments page
                    Response.Redirect("courses.aspx");
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
                    Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                    Int32 StudentID = Convert.ToInt32(ddlStudent.SelectedValue);

                    Enrollment objE = new Enrollment();

                    objE.StudentID = StudentID;
                    objE.CourseID = CourseID;

                    conn.Enrollments.Add(objE);
                    conn.SaveChanges();

                    GetCourse();
                }
            }
            catch (Exception g)
            {
                Response.Redirect("/error.aspx");
            }
            
        }

        protected void grdStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}