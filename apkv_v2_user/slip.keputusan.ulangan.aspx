<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="slip.keputusan.ulangan.aspx.vb" Inherits="apkv_v2_user.slip_keputusan_ulangan" %>

<%@ Register Src="~/commoncontrol/slip_keputusan_ulangan.ascx" TagPrefix="uc1" TagName="slip_keputusan_ulangan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:slip_keputusan_ulangan runat="server" id="slip_keputusan_ulangan" />
</asp:Content>
