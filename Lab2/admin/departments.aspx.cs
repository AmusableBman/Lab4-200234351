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
    public partial class departments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //fill the grid
            if (!IsPostBack)
            {
                Session["SortColumn"] = "DepartmentID";
                Session["SortDirection"] = "ASC";
                GetDepartments();
            }
        }

        protected void GetDepartments()
        {
            try
            {
                //connect using our connection string from web.config and EF context class
                using (ContosoEntitiesEF conn = new ContosoEntitiesEF())
                {
                    //use link to query the Departments model
                    var deps = from d in conn.Departments
                               select new { d.DepartmentID, d.Name, d.Budget };

                    //bind the query result to the gridview
                    String sort = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();
                    grdDepartments.DataSource = deps.AsQueryable().OrderBy(sort).ToList();
                    grdDepartments.DataBind();
                }
            }
            catch (Exception e)
            {
                Response.Redirect("/error.aspx");
            }
            
        }

        protected void grdDepartments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                //connect
                using (ContosoEntitiesEF conn = new ContosoEntitiesEF())
                {
                    //get the selected DepartmentID
                    Int32 DepartmentID = Convert.ToInt32(grdDepartments.DataKeys[e.RowIndex].Values["DepartmentID"]);

                    var d = (from dep in conn.Departments
                             where dep.DepartmentID == DepartmentID
                             select dep).FirstOrDefault();

                    //process the delete
                    conn.Departments.Remove(d);
                    conn.SaveChanges();

                    //update the grid
                    GetDepartments();
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
            grdDepartments.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            GetDepartments();
        }

        protected void grdDepartments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //set page index, refresh grid
            grdDepartments.PageIndex = e.NewPageIndex;
            GetDepartments();
        }

        protected void grdDepartments_Sorting(object sender, GridViewSortEventArgs e)
        {
            Session["SortColumn"] = e.SortExpression;
            GetDepartments();

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

        protected void grdDepartments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (IsPostBack)
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    Image SortImage = new Image();

                    for (int i = 0; i <= grdDepartments.Columns.Count - 1; i++)
                    {
                        if (grdDepartments.Columns[i].SortExpression == Session["SortColumn"].ToString())
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