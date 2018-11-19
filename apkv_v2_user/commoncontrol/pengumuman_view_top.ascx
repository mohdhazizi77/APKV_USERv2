<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="pengumuman_view_top.ascx.vb" Inherits="permatapintar.pengumuman_view_top" %>

<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Paparan Pengumuman Terkini
        </td>
    </tr>
    <tr>
        <td class="fbtd_left">Tarikh Pos
        </td>
        <td>:<asp:Label ID="lblDateCreated" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td>Tajuk
        </td>
        <td>:<asp:Label ID="lblTitle" runat="server" Text="" Font-Bold="true"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="fbtd_top">Pengumuman
        </td>
        <td>:<asp:Literal ID="ltBody" runat="server"></asp:Literal>
        </td>
    </tr>

    <tr>
        <td class="column_width"></td>
        <td>
            <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="red"></asp:Label>
        </td>
    </tr>
</table>
