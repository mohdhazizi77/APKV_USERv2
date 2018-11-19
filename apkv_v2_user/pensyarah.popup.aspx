<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/popup.Master" CodeBehind="pensyarah.popup.aspx.vb" Inherits="apkv_v2_user.pensyarah_popup1" %>
<%@ Register src="commoncontrol/pensyarah.popup.ascx" tagname="pensyarah" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:pensyarah ID="pensyarah1" runat="server" />
</asp:Content>
