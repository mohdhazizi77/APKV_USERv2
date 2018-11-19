<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="admin.slip.view.aspx.vb" Inherits="apkv_v2_user.admin_slip_view" %>

<%@ Register Src="commoncontrol/pelajar_view_header.ascx" TagName="pelajar_view_header" TagPrefix="uc1" %>
<%@ Register Src="commoncontrol/slip_view.ascx" TagName="slip_view" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="fbform">
        <tr class="fbform_header">
            <td colspan="4">SLIP PEPERIKSAAN KOLEJ VOKASIONAL
            </td>
        </tr>
    </table>
    <uc1:pelajar_view_header ID="pelajar_view_header1" runat="server" />
    &nbsp;
    <uc2:slip_view ID="slip_view1" runat="server" />
</asp:Content>
