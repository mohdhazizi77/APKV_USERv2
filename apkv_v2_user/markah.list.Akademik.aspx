<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="markah.list.Akademik.aspx.vb" Inherits="apkv_v2_user.markah_list_Akademik" %>
<%@ Register src="commoncontrol/markah_list.ascx" tagname="markah_list" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:markah_list ID="markah_list1" runat="server" />
</asp:Content>
