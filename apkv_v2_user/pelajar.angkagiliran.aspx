<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="pelajar.angkagiliran.aspx.vb" Inherits="apkv_v2_user.pelajar_angkagiliran" %>
<%@ Register src="commoncontrol/angkagiliran.create.ascx" tagname="angkagiliran" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:angkagiliran ID="angkagiliran1" runat="server" />
</asp:Content>
