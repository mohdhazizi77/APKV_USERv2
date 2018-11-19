<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="pelajar.import.aspx.vb" Inherits="apkv_v2_user.pelajar_import1" %>
<%@ Register src="commoncontrol/pelajar_import.ascx" tagname="pelajar_import" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:pelajar_import ID="pelajar_import" runat="server" />
</asp:Content>
