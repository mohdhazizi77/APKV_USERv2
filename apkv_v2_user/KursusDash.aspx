<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="KursusDash.aspx.vb" Inherits="apkv_v2_user.KursusDash" %>
<%@ Register src="commoncontrol/admin_reportKursus.ascx" tagname="admin_reportKursus" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:admin_reportKursus ID="admin_reportKursus1" runat="server" />
</asp:Content>
