<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="courses.aspx.cs" Inherits="Lab2.courses" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Courses</h1>

    <a href="course.aspx">Add Courses</a>

    <div>
        <label for="ddlPageSize">Records Per Page:</label>
        <asp:DropDownList ID="ddlPageSize" runat="server" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Value="3" Text="3"></asp:ListItem>
            <asp:ListItem Value="5" Text="5"></asp:ListItem>
            <asp:ListItem Value="10" Text="10"></asp:ListItem>
        </asp:DropDownList>
    </div>

    <asp:GridView ID="grdCourses" runat="server" CssClass="table table-striped"
        AutoGenerateColumns="false" OnRowDeleting="grdCourses_RowDeleting"
        DataKeyNames="CourseID" AllowPaging="true" 
        OnPageIndexChanging="grdCourses_PageIndexChanging" PageSize="3" AllowSorting="true"
        OnSorting="grdCourses_Sorting" OnRowDataBound="grdCourses_RowDataBound">
        <Columns>        
            <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
            <asp:BoundField DataField="Credits" HeaderText="Credits" SortExpression="Credits" />
            <asp:BoundField DataField="Name" HeaderText="Department" SortExpression="Name" />
            <asp:HyperLinkField HeaderText="Edit" NavigateUrl="course.aspx" 
                 Text="Edit" DataNavigateUrlFields="CourseID"
                 DataNavigateUrlFormatString="course.aspx?CourseID={0}" />
            <asp:CommandField DeleteText="Delete" ShowDeleteButton="true" HeaderText="Delete" />
        </Columns>
    </asp:GridView>
</asp:Content>
