<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="course.aspx.cs" Inherits="Lab2.course" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Course Details</h1>

    <h5>All fields are required</h5>

    <div class="form-group">
        <label for="txtTitle" class="col-sm-3">Title:</label>
        <asp:TextBox ID="txtTitle" runat="server" required="true" MaxLength="50" />
    </div>
    <div class="form-group">
        <label for="txtCredits" class="col-sm-3">Credits:</label>
        <asp:TextBox ID="txtCredits" runat="server" required="true" />
    </div>
    <div class="form-group">
        <label for="ddlDepartments" class="col-sm-3">Department:</label>
        <asp:DropDownList ID="ddlDepartments" runat="server" DataTextField="Name" DataValueField="DepartmentID" />
    </div>
    <div class="col-sm-offset-3">
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click"/>
    </div>

    <asp:Panel ID="pnlStudents" runat="server" Visible="false">
        <h2>Students Enrolled</h2>
        <asp:GridView ID="grdStudents" runat="server" DataKeyNames="EnrollmentID" 
            AutoGenerateColumns="false" CssClass="table table-striped table-hover"
            OnRowDeleting="grdStudents_RowDeleting">
            <Columns>
                <asp:BoundField DataField="FirstMidName" HeaderText="First Name" />
                <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                <asp:CommandField ShowDeleteButton="true" HeaderText="Delete" DeleteText="Delete" />
            </Columns>
        </asp:GridView>

        <table class="table table-striped table-hover">
            <thead>
                <th>Student</th>
                <th>Add</th>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlStudent" runat="server" AutoPostBack="true"
                             DataValueField="StudentID" DataTextField="Name"></asp:DropDownList>
                        <asp:RangeValidator runat="server" ControlToValidate="ddlStudent"
                             Type="Integer" MinimumValue="1" MaximumValue="99999999"
                             ErrorMessage="Required" CssClass="label label-danger" />
                    </td>
                    <td>
                        <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary"
                             OnClick="btnAdd_Click" />
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
</asp:Content>
