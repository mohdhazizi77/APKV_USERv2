<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="markah.import.aspx.vb" Inherits="apkv_v2_user.markah_import1" %>
<%@ Register src="commoncontrol/markah_import.ascx" tagname="markah_import" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:markah_import ID="markah_import" runat="server" />
</asp:Content>
