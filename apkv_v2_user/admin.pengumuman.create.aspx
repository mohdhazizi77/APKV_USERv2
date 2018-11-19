<%@ Page Title="" Language="vb" AutoEventWireup="false" ValidateRequest="false"  MasterPageFile="~/admin.Master" CodeBehind="admin.pengumuman.create.aspx.vb" Inherits="apkv_v2_user.admin_pengumuman_create" %>

<%@ Register Src="commoncontrol/pengumuman_create.ascx" TagName="pengumuman_create" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="fbform">
        <tr class="fbform_bread">
            <td>Lain-Lain>Pengumunan>Tambah
            </td>
        </tr>
    </table>
    <uc1:pengumuman_create ID="pengumuman_create1" runat="server" />
    &nbsp;
</asp:Content>
