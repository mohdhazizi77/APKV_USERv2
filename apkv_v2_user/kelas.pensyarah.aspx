<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="kelas.pensyarah.aspx.vb" Inherits="apkv_v2_user.kelas_pensyarah" %>
<%@ Register src="commoncontrol/kelas.modul.pensyarah.ascx" tagname="kelas" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:kelas ID="kelas1" runat="server" />
</asp:Content>
