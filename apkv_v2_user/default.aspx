<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.vb" Inherits="apkv_v2_user._default1" %>
<%@ Register src="commoncontrol/user_login.ascx" tagname="user_login" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:user_login ID="user_login1" runat="server" />
</asp:Content>

