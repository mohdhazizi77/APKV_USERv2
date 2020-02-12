<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/default.Master" CodeBehind="error.page.aspx.vb" Inherits="apkv_v2_user.error_page1" %>

<%@ Register Src="~/commoncontrol/error_page.ascx" TagPrefix="uc1" TagName="error_page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:error_page runat="server" id="error_page" />
</asp:Content>
