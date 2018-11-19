<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="modul.search.aspx.vb" Inherits="apkv_v2_user.modul_search1" %>
<%@ Register src="commoncontrol/modul_search.ascx" tagname="modul_search" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:modul_search ID="modul_search" runat="server" />
</asp:Content>
