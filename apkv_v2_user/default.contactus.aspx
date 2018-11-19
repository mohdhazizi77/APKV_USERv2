<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/default.Master"
    CodeBehind="default.contactus.aspx.vb" Inherits="apkv_v2_user.default_contactus" %>

<%@ Register Src="commoncontrol/contact_us.ascx" TagName="contact_us" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:contact_us ID="contact_us1" runat="server" />
    &nbsp;
</asp:Content>
