<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="KolejDash.aspx.vb" Inherits="apkv_v2_user.KolejDash" %>
<%@ Register src="commoncontrol/admin_reportKV.ascx" tagname="admin_reportKV" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:admin_reportKV ID="admin_reportKV1" runat="server" />
</asp:Content>
