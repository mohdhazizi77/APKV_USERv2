<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="pelajar.view.aspx.vb" Inherits="apkv_v2_user.pelajar_view1" %>
<%@ Register src="commoncontrol/pelajar_view.ascx" tagname="pelajar_view" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:pelajar_view ID="pelajar_view" runat="server" />
</asp:Content>
