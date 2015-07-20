<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="departments.aspx.cs" Inherits="Lab2.departments" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Departments</h1>

    <a href="department.aspx">Add Department</a>

    <div>
        <label for="ddlPageSize">Records Per Page:</label>
        <asp:DropDownList ID="ddlPageSize" runat="server" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Value="3" Text="3"></asp:ListItem>
            <asp:ListItem Value="5" Text="5"></asp:ListItem>
            <asp:ListItem Value="10" Text="10"></asp:ListItem>
        </asp:DropDownList>
    </div>

    <asp:GridView ID="grdDepartments" runat="server" CssClass="table table-striped"
        AutoGenerateColumns="false" OnRowDeleting="grdDepartments_RowDeleting"
        DataKeyNames="DepartmentID" AllowPaging="true" OnPageIndexChanging="grdDepartments_PageIndexChanging" 
        PageSize="3" AllowSorting="true"
        OnSorting="grdDepartments_Sorting" OnRowDataBound="grdDepartments_RowDataBound">
        <Columns>        
            <asp:BoundField DataField="Name" HeaderText="Department Name" SortExpression="Name"/>
            <asp:BoundField DataField="Budget" HeaderText="Budget" DataFormatString="{0:c}" SortExpression="Budget" />
            <asp:HyperLinkField HeaderText="Edit" NavigateUrl="department.aspx" 
                 Text="Edit" DataNavigateUrlFields="DepartmentID"
                 DataNavigateUrlFormatString="department.aspx?DepartmentID={0}" />
            <asp:CommandField DeleteText="Delete" ShowDeleteButton="true" HeaderText="Delete" />
        </Columns>
    </asp:GridView>
</asp:Content>
