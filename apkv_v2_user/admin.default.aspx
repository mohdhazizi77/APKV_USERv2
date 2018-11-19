<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master"
    CodeBehind="admin.default.aspx.vb" Inherits="apkv_v2_user.admin_default" %>

<%@ Register Src="commoncontrol/kpmkv_intro.ascx" TagName="kpmkv_intro" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:kpmkv_intro ID="kpmkv_intro1" runat="server" />
    &nbsp;
   
</asp:Content>
