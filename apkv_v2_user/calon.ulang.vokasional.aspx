<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="calon.ulang.vokasional.aspx.vb" Inherits="apkv_v2_user.calon_ulang_vokasional" %>

<%@ Register src="commoncontrol/pelajar_ulang_vokasional.ascx" tagname="pelajar_ulang_vokasional" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:pelajar_ulang_vokasional ID="pelajar_ulang_vokasional1" runat="server" />
    </asp:Content>
