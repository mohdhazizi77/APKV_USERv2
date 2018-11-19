<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="bmsetara_baru.aspx.vb" Inherits="apkv_v2_user.bmsetara_baru" %>
<%@ Register src="commoncontrol/bmsetara_pelajar_baru_create.ascx" tagname="bmsetara_pelajar_baru_create" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:bmsetara_pelajar_baru_create ID="bmsetara_pelajar_baru_create1" runat="server" />
</asp:Content>
