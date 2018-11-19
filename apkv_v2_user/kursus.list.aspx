<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="kursus.list.aspx.vb" Inherits="apkv_v2_user.kursus_list1" %>
<%@ Register src="commoncontrol/kursus_list.ascx" tagname="kursus_list" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:kursus_list ID="kursus_list" runat="server" />
</asp:Content>
