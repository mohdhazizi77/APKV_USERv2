<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="markah.ulang.bm.aspx.vb" Inherits="apkv_v2_user.markah_ulang_bm" %>

<%@ Register Src="~/commoncontrol/markah_ulang_bm.ascx" TagPrefix="uc1" TagName="markah_ulang_bm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:markah_ulang_bm runat="server" id="markah_ulang_bm" />
</asp:Content>
