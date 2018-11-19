<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="Roaster_Peperiksaan.aspx.vb" Inherits="apkv_v2_user.Roaster_Peperiksaan" %>
<%@ Register src="commoncontrol/pentaksiran_roster.ascx" tagname="pentaksiran_roster" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:pentaksiran_roster ID="pentaksiran_roster1" runat="server" />
</asp:Content>
