<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="admin.users.search.aspx.vb" Inherits="apkv_v2_user.admin_users_search" %>

<%@ Register Src="commoncontrol/users_search.ascx" TagName="users_search" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:users_search ID="users_search1" runat="server" />
    &nbsp;
</asp:Content>
