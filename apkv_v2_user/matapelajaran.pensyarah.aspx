<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="matapelajaran.pensyarah.aspx.vb" Inherits="apkv_v2_user.matapelajaran_pensyarah" %>
<%@ Register src="commoncontrol/matapelajaran.pensyarah.create.ascx" tagname="matapelajaran" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:matapelajaran ID="matapelajaran1" runat="server" />
</asp:Content>
