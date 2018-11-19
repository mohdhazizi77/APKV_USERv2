<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="matapelajaran.view.aspx.vb" Inherits="apkv_v2_user.matapelajaran_view1" %>
<%@ Register src="commoncontrol/matapelajaran_view.ascx" tagname="matapelajaran_view" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:matapelajaran_view ID="matapelajaran_view" runat="server" />
</asp:Content>
