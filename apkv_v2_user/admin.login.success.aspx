<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="admin.login.success.aspx.vb" Inherits="apkv_v2_user.admin_login_success" %>
<%@ Register src="commoncontrol/pengumuman_list_pub.ascx" tagname="pengumuman_list_pub" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <uc1:pengumuman_list_pub ID="pengumuman_list_pub1" runat="server" />
    
</asp:Content>
