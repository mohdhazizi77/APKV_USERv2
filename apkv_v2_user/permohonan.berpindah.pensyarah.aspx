<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="permohonan.berpindah.pensyarah.aspx.vb" Inherits="apkv_v2_user.permohonan_berpindah_pensyarah" %>
<%@ Register src="commoncontrol/permohonan.berpindah.pensyarah.create.ascx" tagname="permohonan" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:permohonan ID="permohonan1" runat="server" />
</asp:Content>
