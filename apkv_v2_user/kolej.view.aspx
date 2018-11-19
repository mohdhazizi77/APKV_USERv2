<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="kolej.view.aspx.vb" Inherits="apkv_v2_user.kolej_view1" %>
<%@ Register src="commoncontrol/kolej_view.ascx" tagname="kolej_view" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:kolej_view ID="kolej_view" runat="server" />
</asp:Content>
