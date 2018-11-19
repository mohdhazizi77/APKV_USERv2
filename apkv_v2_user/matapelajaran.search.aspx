<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="matapelajaran.search.aspx.vb" Inherits="apkv_v2_user.matapelajaran_search1" %>
<%@ Register src="commoncontrol/matapelajaran_search.ascx" tagname="matapelajaran_search" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:matapelajaran_search ID="matapelajaran_search" runat="server" />
</asp:Content>
