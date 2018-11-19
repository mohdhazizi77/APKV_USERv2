<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="kelas.list.aspx.vb" Inherits="apkv_v2_user.kelas_list1" %>
<%@ Register src="commoncontrol/kelas.list.ascx" tagname="kelas" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:kelas ID="kelas1" runat="server" />
</asp:Content>
