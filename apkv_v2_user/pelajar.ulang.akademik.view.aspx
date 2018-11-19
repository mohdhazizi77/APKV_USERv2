<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="pelajar.ulang.akademik.view.aspx.vb" Inherits="apkv_v2_user.pelajar_ulang_akademik_view" %>
<%@ Register src="commoncontrol/pelajar.ulang.akademik.view.ascx" tagname="pelajar" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:pelajar ID="pelajar1" runat="server" />
</asp:Content>
