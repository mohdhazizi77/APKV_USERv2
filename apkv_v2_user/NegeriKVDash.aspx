<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="NegeriKVDash.aspx.vb" Inherits="apkv_v2_user.NegeriKVDash" %>
<%@ Register src="commoncontrol/admin_reportNegeriKV.ascx" tagname="admin_reportNegeriKV" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:admin_reportNegeriKV ID="admin_reportNegeriKV1" runat="server" />
</asp:Content>
