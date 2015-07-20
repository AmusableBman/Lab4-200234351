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
    public partial class department : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if the page isn't posted back, check the url for an id to see know add or edit
            if (!IsPostBack)
            {
                if (Request.QueryString.Keys.Count > 0)
                {
                    //we have a url parameter if the count is > 0 so populate the form
                    GetDepartment();
                }
            }
        }

        protected void GetDepartment()
        {
            try
            {
                //connect
                using (ContosoEntitiesEF conn = new ContosoEntitiesEF())
                {
                    //get id from url parameter and store in a variable
                    Int32 DepartmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);

                    var d = (from dep in conn.Departments
                             where dep.DepartmentID == DepartmentID
                             select dep).FirstOrDefault();

                    //populate the form from our department object
                    txtName.Text = d.Name;
                    txtBudget.Text = d.Budget.ToString();

                    var objE = (from dep in conn.Departments
                                join c in conn.Courses on dep.DepartmentID equals c.DepartmentID
                                where dep.DepartmentID == DepartmentID
                                select new { c.CourseID, c.Title, dep.Name });

                    grdCourses.DataSource = objE.ToList();
                    grdCourses.DataBind();

                    pnlCourses.Visible = true;
                }
            }
            catch (Exception t)
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
                    Department d = new Department();

                    //decide if updating or adding, then save
                    if (Request.QueryString.Count > 0)
                    {
                        Int32 DepartmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);

                        d = (from dep in conn.Departments
                             where dep.DepartmentID == DepartmentID
                             select dep).FirstOrDefault();
                    }

                    //fill the properties of our object from the form inputs
                    d.Name = txtName.Text;
                    d.Budget = Convert.ToDecimal(txtBudget.Text);

                    if (Request.QueryString.Count == 0)
                    {
                        conn.Departments.Add(d);
                    }
                    conn.SaveChanges();

                    //redirect to updated departments page
                    Response.Redirect("departments.aspx");
                }
            }
            catch (Exception f)
            {
                Response.Redirect("/error.aspx");
            }
            
        }

        protected void grdCourses_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Int32 CourseID = Convert.ToInt32(grdCourses.DataKeys[e.RowIndex].Values["CourseID"]);

                using (ContosoEntitiesEF db = new ContosoEntitiesEF())
                {
                    Cours objE = (from c in db.Courses
                                  where c.CourseID == CourseID
                                  select c).FirstOrDefault();

                    //process the deletion
                    db.Courses.Remove(objE);
                    db.SaveChanges();

                    //repopulate the page
                    GetDepartment();
                }
            }
            catch (Exception g)
            {
                Response.Redirect("/error.aspx");
            }
            
        }
    }
}