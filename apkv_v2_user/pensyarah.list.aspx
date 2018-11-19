<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="pensyarah.list.aspx.vb" Inherits="apkv_v2_user.pensyarah_list1" %>
<%@ Register src="commoncontrol/pensyarah_list.ascx" tagname="pensyarah_list" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:pensyarah_list ID="pensyarah_list" runat="server" />
</asp:Content>
