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
    public partial class students : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["SortColumn"] = "StudentID";
                Session["SortDirection"] = "ASC";
                GetStudents();
            }
        }

        protected void GetStudents()
        {
            try
            {
                //connect using our connection string from web.config and EF context class
                using (ContosoEntitiesEF conn = new ContosoEntitiesEF())
                {

                    var studs = from s in conn.Students select new { s.StudentID, s.FirstMidName, s.LastName };

                    String sort = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();
                    grdStudents.DataSource = studs.AsQueryable().OrderBy(sort).ToList();
                    grdStudents.DataBind();
                }
            }
            catch (Exception e)
            {
                Response.Redirect("/error.aspx");
            }
           
        }

        protected void grdStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                //connect
                using (ContosoEntitiesEF conn = new ContosoEntitiesEF())
                {

                    Int32 StudentID = Convert.ToInt32(grdStudents.DataKeys[e.RowIndex].Values["StudentID"]);

                    var s = (from stu in conn.Students where stu.StudentID == StudentID select stu).FirstOrDefault();

                    conn.Students.Remove(s);
                    conn.SaveChanges();
                    GetStudents();
                }
            }
            catch (Exception f)
            {
                Response.Redirect("/error.aspx");
            }
            
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //set page size and refresh grid
            grdStudents.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            GetStudents();
        }

        protected void grdStudents_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdStudents.PageIndex = e.NewPageIndex;
            GetStudents();
        }

        protected void grdStudents_Sorting(object sender, GridViewSortEventArgs e)
        {
            Session["SortColumn"] = e.SortExpression;
            GetStudents();

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

        protected void grdStudents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (IsPostBack)
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    Image SortImage = new Image();

                    for (int i = 0; i <= grdStudents.Columns.Count - 1; i++)
                    {
                        if (grdStudents.Columns[i].SortExpression == Session["SortColumn"].ToString())
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
