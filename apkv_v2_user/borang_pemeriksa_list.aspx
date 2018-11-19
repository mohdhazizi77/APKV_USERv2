<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="borang_pemeriksa_list.aspx.vb" Inherits="apkv_v2_user.borang_pemeriksa_list" %>
<%@ Register src="commoncontrol/pemeriksa_list.ascx" tagname="pemeriksa_list" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:pemeriksa_list ID="pemeriksa_list1" runat="server" />
</asp:Content>
