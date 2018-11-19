<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="pensyarah.view.aspx.vb" Inherits="apkv_v2_user.pensyarah_view1" %>
<%@ Register src="commoncontrol/pensyarah_view.ascx" tagname="pensyarah_view" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:pensyarah_view ID="pensyarah_view" runat="server" />
</asp:Content>
