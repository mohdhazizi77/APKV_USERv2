<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="borang_penetapan_pemeriksa.aspx.vb" Inherits="apkv_v2_user.borang_penetapan_pemeriksa" %>
<%@ Register src="commoncontrol/penetapan_pemeriksa.ascx" tagname="penetapan_pemeriksa" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:penetapan_pemeriksa ID="penetapan_pemeriksa" runat="server" />
</asp:Content>
