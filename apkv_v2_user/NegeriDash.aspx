<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="NegeriDash.aspx.vb" Inherits="apkv_v2_user.NegeriDash" %>
<%@ Register src="commoncontrol/admin_reportNegeri.ascx" tagname="admin_reportNegeri" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:admin_reportNegeri ID="admin_reportNegeri1" runat="server" />
</asp:Content>
