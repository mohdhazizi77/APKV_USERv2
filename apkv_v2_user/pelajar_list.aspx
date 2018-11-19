<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="pelajar_list.aspx.vb" Inherits="apkv_v2_user.pelajar_list" %>
<%@ Register src="commoncontrol/pelajar_list.ascx" tagname="pelajar_list" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:pelajar_list ID="pelajar_list1" runat="server" />
</asp:Content>
