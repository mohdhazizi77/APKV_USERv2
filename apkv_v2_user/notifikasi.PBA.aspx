<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="notifikasi.PBA.aspx.vb" Inherits="apkv_v2_user.notifikasi_PBA1" %>
<%@ Register src="commoncontrol/notifikasi_PBA.ascx" tagname="notifikasi_PBA" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:notifikasi_PBA ID="notifikasi_PBA" runat="server" />
</asp:Content>
