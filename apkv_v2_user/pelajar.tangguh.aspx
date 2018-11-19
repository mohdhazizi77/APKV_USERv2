<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="pelajar.tangguh.aspx.vb" Inherits="apkv_v2_user.pelajar_tangguh" %>
<%@ Register src="commoncontrol/pelajar_tangguh.ascx" tagname="pelajar_tangguh" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:pelajar_tangguh ID="pelajar_tangguh1" runat="server" />
</asp:Content>
