<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="cetak.pelajar.ulang.aspx.vb" Inherits="apkv_v2_user.cetak_pelajar_ulang" %>

<%@ Register Src="~/commoncontrol/cetak_pelajar_ulang.ascx" TagPrefix="uc1" TagName="cetak_pelajar_ulang" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:cetak_pelajar_ulang runat="server" id="cetak_pelajar_ulang" />
</asp:Content>
