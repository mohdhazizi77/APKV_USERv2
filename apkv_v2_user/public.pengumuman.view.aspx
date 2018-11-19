<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="public.pengumuman.view.aspx.vb" Inherits="apkv_v2_user.public_pengumuman_view" %>
<%@ Register src="commoncontrol/pengumuman_view_pub.ascx" tagname="pengumuman_view_pub" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:pengumuman_view_pub ID="pengumuman_view_pub1" runat="server" />
</asp:Content>
