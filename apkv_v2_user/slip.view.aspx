<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="slip.view.aspx.vb" Inherits="apkv_v2_user.slip_view1" %>
<%@ Register src="commoncontrol/slip_view.ascx" tagname="slip_view" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:slip_view ID="slip_view" runat="server" />
</asp:Content>
