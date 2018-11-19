<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="DashKursus_view.aspx.vb" Inherits="apkv_v2_user.DashKursus_view" %>
<%@ Register src="commoncontrol/admin_reportKursus_view.ascx" tagname="admin_reportKursus_view" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:admin_reportKursus_view ID="admin_reportKursus_view1" runat="server" />
</asp:Content>
