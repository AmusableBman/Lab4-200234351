﻿<%@ Page Title="Contoso University" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="Lab2.main" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="well">
            <h3>Departments</h3>
            <ul class="list-group">
                <li class="list-group-item"><a href="departments.aspx">List Departments</a></li>
                <li class="list-group-item"><a href="department.aspx">Add Departments</a></li>
            </ul>
        </div>

        <div class="well">
            <h3>Students</h3>
            <ul class="list-group">
                <li class="list-group-item"><a href="students.aspx">List Students</a></li>
                <li class="list-group-item"><a href="student.aspx">Add Students</a></li>
            </ul>
        </div>

        <div class="well">
            <h3>Courses</h3>
            <ul class="list-group">
                <li class="list-group-item"><a href="courses.aspx">List Courses</a></li>
                <li class="list-group-item"><a href="course.aspx">Add Courses</a></li>
            </ul>
        </div>
    </div>
</asp:Content>
