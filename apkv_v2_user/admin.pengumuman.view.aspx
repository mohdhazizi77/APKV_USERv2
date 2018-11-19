<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="admin.pengumuman.view.aspx.vb" Inherits="apkv_v2_user.admin_pengumuman_view" %>

<%@ Register Src="commoncontrol/pengumuman_view.ascx" TagName="pengumuman_view" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="fbform">
        <tr class="fbform_bread">
            <td>Lain-Lain>Pengumunan>Paparan
            </td>
        </tr>
    </table>
    <uc1:pengumuman_view ID="pengumuman_view1" runat="server" />
</asp:Content>
