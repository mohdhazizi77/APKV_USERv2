<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="admin_reportAnalisis.aspx.vb" Inherits="apkv_v2_user.admin_reportAnalisis" %>
<%@ Register src="commoncontrol/pentaksiran_analisis.ascx" tagname="pentaksiran_analisis" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:pentaksiran_analisis ID="pentaksiran_analisis1" runat="server" />
</asp:Content>
