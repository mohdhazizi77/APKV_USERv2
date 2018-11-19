<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="notifikasi.PAA.aspx.vb" Inherits="apkv_v2_user.notifikasi_PAA1" %>
<%@ Register src="commoncontrol/notifikasi_PAA.ascx" tagname="notifikasi_PAA" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:notifikasi_PAA ID="notifikasi_PAA" runat="server" />
</asp:Content>
