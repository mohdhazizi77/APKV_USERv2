<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="pensyarah.search.aspx.vb" Inherits="apkv_v2_user.pensyarah_search1" %>
<%@ Register src="commoncontrol/pensyarah_search.ascx" tagname="pensyarah_search" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:pensyarah_search ID="pensyarah_search" runat="server" />
</asp:Content>
