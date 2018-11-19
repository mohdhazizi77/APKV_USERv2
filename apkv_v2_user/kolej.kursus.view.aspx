<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/kolej.Master" CodeBehind="kolej.kursus.view.aspx.vb" Inherits="apkv_v2_user.kolej_kursus_view" %>
<%@ Register src="commoncontrol/kursus_view.ascx" tagname="kursus_view" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:kursus_view ID="kursus_view1" runat="server" />
</asp:Content>
