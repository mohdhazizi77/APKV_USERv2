<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/pentaksir.Master" CodeBehind="pentaksir_bmsetara_main.aspx.vb" Inherits="apkv_v2_user.pentaksir_bmsetara_main" %>

<%@ Register Src="~/commoncontrol/pentaksir_bmsetara_main.ascx" TagPrefix="uc1" TagName="pentaksir_bmsetara_main" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:pentaksir_bmsetara_main runat="server" id="pentaksir_bmsetara_main" />
</asp:Content>
