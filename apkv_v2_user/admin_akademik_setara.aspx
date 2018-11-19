<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="admin_akademik_setara.aspx.vb" Inherits="apkv_v2_user.admin_akademik_setara" %>
<%@ Register src="commoncontrol/akademik_setara.ascx" tagname="akademik_setara" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:akademik_setara ID="akademik_setara1" runat="server" />
</asp:Content>
