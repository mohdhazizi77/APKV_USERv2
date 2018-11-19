<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="kelas.pensyarah.akademik.aspx.vb" Inherits="apkv_v2_user.kelas_pensyarah_akademik" %>
<%@ Register src="commoncontrol/kelas_pensyarah_akademik.ascx" tagname="kelas_pensyarah_akademik" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:kelas_pensyarah_akademik ID="kelas_pensyarah_akademik1" runat="server" />
</asp:Content>
