<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="bmsetara_markah_baru.aspx.vb" Inherits="apkv_v2_user.bmsetara_markah_baru" %>
<%@ Register src="commoncontrol/bmsetara_markah_calonbaru.ascx" tagname="bmsetara_markah_calonbaru" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:bmsetara_markah_calonbaru ID="bmsetara_markah_calonbaru1" runat="server" />
</asp:Content>
