<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="admin.gred.list.aspx.vb" Inherits="apkv_v2_user.admin_gred_list" %>

<%@ Register Src="commoncontrol/gred_list.ascx" TagName="gred_list" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:gred_list ID="gred_list1" runat="server" />
    &nbsp;
</asp:Content>
