<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="cetak.labelMeja.aspx.vb" Inherits="apkv_v2_user.cetak_labelMeja1" %>
<%@ Register src="commoncontrol/cetak_labelMeja.ascx" tagname="cetak_labelMeja" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:cetak_labelMeja ID="cetak_labelMeja" runat="server" />
</asp:Content>
