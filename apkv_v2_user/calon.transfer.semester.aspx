<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="calon.transfer.semester.aspx.vb" Inherits="apkv_v2_user.calon_transfer_semester" %>
<%@ Register src="commoncontrol/pelajar.kpmkv.transfrer.ascx" tagname="pelajar" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:pelajar ID="pelajar1" runat="server" />
</asp:Content>
