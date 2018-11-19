<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="matapelajaran.list.aspx.vb" Inherits="apkv_v2_user.matapelajaran_list1" %>
<%@ Register src="commoncontrol/matapelajaran_list.ascx" tagname="matapelajaran_list" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:matapelajaran_list ID="matapelajaran_list" runat="server" />
</asp:Content>
