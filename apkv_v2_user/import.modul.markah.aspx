<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="import.modul.markah.aspx.vb" Inherits="apkv_v2_user.import_modul_markah" %>
<%@ Register src="commoncontrol/import_markah_modul.ascx" tagname="import_markah_modul" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:import_markah_modul ID="import_markah_modul1" runat="server" />
</asp:Content>
