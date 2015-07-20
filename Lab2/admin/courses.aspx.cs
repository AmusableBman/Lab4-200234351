using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Lab2.Models;
using System.Web.ModelBinding;
using System.Linq.Dynamic;

namespace Lab2
{
    public partial class courses : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["SortColumn"] = "CourseID";
                Session["SortDirection"] = "ASC";
                GetCourses();
            }
        }

        protected void GetCourses()
        {
            try
            {
                using (ContosoEntitiesEF conn = new ContosoEntitiesEF())
                {
                    //use link to query the Departments model
                    var cours = from c in conn.Courses
                                select new { c.CourseID, c.Title, c.Credits, c.Department.Name };

                    //bind the query result to the gridview
                    String sort = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();
                    grdCourses.DataSource = cours.AsQueryable().OrderBy(sort).ToList();
                    grdCourses.DataBind();
                }
            }
            catch (Exception e)
            {
                Response.Redirect("/error.aspx");
            }
            
        }

        protected void grdCourses_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                //connect
                using (ContosoEntitiesEF conn = new ContosoEntitiesEF())
                {
                    //get the selected DepartmentID
                    Int32 CourseID = Convert.ToInt32(grdCourses.DataKeys[e.RowIndex].Values["CourseID"]);

                    var c = (from cour in conn.Courses
                             where cour.CourseID == CourseID
                             select cour).FirstOrDefault();

                    //process the delete
                    conn.Courses.Remove(c);
                    conn.SaveChanges();

                    //update the grid
                    GetCourses();
                }
            }
            catch (Exception f)
            {
                Response.Redirect("/error.aspx");
            }
            
        }

        protected void grdCourses_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //set page index, refresh grid
            grdCourses.PageIndex = e.NewPageIndex;
            GetCourses();
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //set page size and refresh grid
            grdCourses.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            GetCourses();
        }

        protected void grdCourses_Sorting(object sender, GridViewSortEventArgs e)
        {
            //set sort column to column by user
            Session["SortColumn"] = e.SortExpression;
            GetCourses();

            //toggle direction
            if (Session["SortDirection"] == "ASC")
            {
                Session["SortDirection"] = "DESC";
            }
            else
            {
                Session["SortDirection"] = "ASC";
            }
        }

        protected void grdCourses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (IsPostBack) {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    Image SortImage = new Image();
               
                    for (int i = 0; i <= grdCourses.Columns.Count -1; i++) {
                        if (grdCourses.Columns[i].SortExpression == Session["SortColumn"].ToString())
                        {
                            if (Session["SortDirection"].ToString() == "DESC")
                            {
                                SortImage.ImageUrl = "/images/desc.jpg";
                                SortImage.AlternateText = "Sort Descending";
                            }
                            else
                            {
                                SortImage.ImageUrl = "/images/asc.jpg";
                                SortImage.AlternateText = "Sort Ascending";
                            }
                       
                            e.Row.Cells[i].Controls.Add(SortImage);
                           
                        }
                    }
                }
              
            }
        }
    }
}