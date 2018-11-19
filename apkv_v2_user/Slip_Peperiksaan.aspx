<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="Slip_Peperiksaan.aspx.vb" Inherits="apkv_v2_user.Slip_Peperiksaan" %>
<%@ Register src="commoncontrol/pentaksiran_vokasional.ascx" tagname="pentaksiran_vokasional" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc2:pentaksiran_vokasional ID="pentaksiran_vokasional1" runat="server" />
</asp:Content>
