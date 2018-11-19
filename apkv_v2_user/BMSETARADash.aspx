<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="BMSETARADash.aspx.vb" Inherits="apkv_v2_user.BMSETARADash" %>
<%@ Register src="commoncontrol/admin_reportBMSetara.ascx" tagname="admin_reportBMSetara" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:admin_reportBMSetara ID="admin_reportBMSetara1" runat="server" />
</asp:Content>
