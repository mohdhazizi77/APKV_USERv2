<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="user.change.password.aspx.vb" Inherits="apkv_v2_user.user_change_password1" %>
<%@ Register src="commoncontrol/user_change_password.ascx" tagname="user_change_password" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:user_change_password ID="user_change_password" runat="server" />
</asp:Content>
