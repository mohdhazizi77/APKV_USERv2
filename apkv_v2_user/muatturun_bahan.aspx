<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="muatturun_bahan.aspx.vb" Inherits="apkv_v2_user.muatturun_bahan1" %>
<%@ Register src="commoncontrol/muatturun.bahan.ascx" tagname="muatturun" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:muatturun ID="muatturun1" runat="server" />
</asp:Content>
