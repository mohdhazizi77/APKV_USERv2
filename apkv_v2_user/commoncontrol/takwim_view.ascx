<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="takwim_view.ascx.vb" Inherits="apkv_v2_user.takwim_view" %>

<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Paparan Takwim
        </td>
    </tr>
    <tr>
        <td class="fbtd_left">Tahun:
        </td>
        <td>
            <asp:Label ID="lblTahun" runat="server" Text=""></asp:Label>
        </td>
    </tr>
     <tr>
        <td >Kategori:
        </td>
        <td>
            <asp:Label ID="lblMenu" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td >Tarikh Mula:
        </td>
        <td>
            <asp:Label ID="lblTarikhMula" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td >Tarikh Akhir:
        </td>
        <td>
            <asp:Label ID="lblTarikhAkhir" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td >Tajuk:
        </td>
        <td>
            <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="width:20%; vertical-align:top;">Catatan:
        </td>
        <td>
            <asp:Literal ID="ltCatatan" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr>
        <td class="column_width"></td>
        <td>
            <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="red"></asp:Label>
        </td>
    </tr>
</table>

