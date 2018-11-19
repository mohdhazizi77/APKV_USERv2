<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="kolej.kursus.list.aspx.vb" Inherits="apkv_v2_user.kolej_kursus_list" %>
<%@ Register src="commoncontrol/kolej.kursus.list.ascx" tagname="kolej" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:kolej ID="kolej1" runat="server" />
</asp:Content>
