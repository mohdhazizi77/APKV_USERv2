<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="user_view_master.ascx.vb"
    Inherits="apkv_v2_user.user_view_master" %>
<table class="fbform">
    <tr class="fbform_header">
         <td>
            Maklumat Anda
        </td>
    </tr>
    <tr>
         <td>
            Login ID:
        </td>
    </tr>
    <tr>
         <td>
            <asp:Label ID="lblLoginID" runat="server" Text="" CssClass="fblabel_view"></asp:Label>
        </td>
    </tr>
    <tr>
         <td>
            Nama Pengguna:
        </td>
    </tr>
    <tr>
         <td>
            <asp:Label ID="lblNama" runat="server" Text="" CssClass="fblabel_view"></asp:Label>
        </td>
    </tr>
</table>
