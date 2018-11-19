<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="Markah.list.Vokasional.aspx.vb" Inherits="apkv_v2_user.Markah_list_Vokasional" %>
<%@ Register src="commoncontrol/markah.vokasional.list.ascx" tagname="markah" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:markah ID="markah1" runat="server" />
</asp:Content>
