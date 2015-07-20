<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Lab2.login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1>Login</h1>

    <div>
        <asp:Label ID="lblStatus" runat="server" CssClass="label label-danger"></asp:Label>
    </div>

    <div class="form-group">
        <label for="txtUsername" class="col-sm-2">Username:</label>
        <asp:TextBox ID="txtUsername" runat="server" ></asp:TextBox>
    </div>
    <div class="form-group">
        <label for="txtPassword" class="col-sm-2">Password:</label>
        <asp:TextBox ID="txtPassword" runat="server" textmode="Password"></asp:TextBox>
    </div>

    <div class="form-group">
        <asp:CompareValidator runat="server" ControlToCompare ="txtConfirm" ControlToValidate ="txtPassword" Operator="Equal" ErrorMessage="Passwords must match" CssClass="label label-danger" 
    </div>

    <div class="col-sm-offset-2">
        <asp:Button ID="btnLogin" runat="server" text="Login" CssClass="btn btn-primary" OnClick="btnLogin_Click"/>
    </div>
</asp:Content>
