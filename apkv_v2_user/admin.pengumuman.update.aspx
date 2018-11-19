<%@ Page Title="" Language="vb" AutoEventWireup="false" ValidateRequest="false" MasterPageFile="~/admin.Master" CodeBehind="admin.pengumuman.update.aspx.vb" Inherits="apkv_v2_user.admin_pengumuman_update" %>

<%@ Register src="commoncontrol/pengumuman_update.ascx" tagname="pengumuman_update" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="fbform">
        <tr class="fbform_bread">
            <td>Lain-Lain>Pengumunan>Kemaskini
            </td>
        </tr>
    </table>
    <uc1:pengumuman_update ID="pengumuman_update1" runat="server" />
</asp:Content>
