<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="pelajar_list_kemaskini.aspx.vb" Inherits="apkv_v2_user.pelajar_list_kemaskini1" %>

<%@ register src="~/commoncontrol/pelajar_list_kemaskini.ascx" tagprefix="uc1" tagname="pelajar_list_kemaskini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:pelajar_list_kemaskini runat="server" id="pelajar_list_kemaskini" />
</asp:Content>
