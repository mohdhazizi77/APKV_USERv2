<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="pelajar.search.aspx.vb" Inherits="apkv_v2_user.pelajar_search1" %>
<%@ Register src="commoncontrol/pelajar_search.ascx" tagname="pelajar_search" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:pelajar_search ID="pelajar_search" runat="server" />
</asp:Content>
