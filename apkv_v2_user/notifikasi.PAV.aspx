<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="notifikasi.PAV.aspx.vb" Inherits="apkv_v2_user.notifikasi_PAV1" %>
<%@ Register src="commoncontrol/notifikasi_PAV.ascx" tagname="notifikasi_PAV" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:notifikasi_PAV ID="notifikasi_PAV" runat="server" />
</asp:Content>
