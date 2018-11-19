<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="pelajar.create.aspx.vb" Inherits="apkv_v2_user.pelajar_create1" %>
<%@ Register src="commoncontrol/pelajar_create.ascx" tagname="pelajar_create" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:pelajar_create ID="pelajar_create" runat="server" />
</asp:Content>
