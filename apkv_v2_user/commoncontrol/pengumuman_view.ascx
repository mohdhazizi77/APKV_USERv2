<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="pengumuman_view.ascx.vb" Inherits="apkv_v2_user.pengumuman_view" %>

<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Paparan Pengumuman
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
        <td>Paparkan:
        </td>
        <td>
            <asp:Label ID="lblIsDisplay" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td>Tajuk:
        </td>
        <td>
            <asp:Label ID="lblTitle" runat="server" Text="" Font-Bold="true"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="width: 20%; vertical-align: top;">Pengumuman:
        </td>
        <td>
            <asp:Literal ID="ltBody" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr>
        <td style="width: 20%; vertical-align: top;">Tarikh:
        </td>
        <td>
            <asp:Label ID="lblDateCreated" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td></td>
        <td class="fbform_sap">&nbsp;</td>
    </tr>
    <tr>
        <td class="column_width">&nbsp;
        </td>
        <td>
            <asp:Button ID="btnUpdate" runat="server" Text="Kemaskini " CssClass="fbbutton" />
            &nbsp;
            <asp:Button ID="btnDelete" runat="server" Text="Hapuskan " CssClass="fbbutton" />
            &nbsp;|&nbsp;<asp:LinkButton ID="lnkList" runat="server">Senarai Pengumuman</asp:LinkButton>
        </td>
    </tr>
    <tr>
        <td class="column_width"></td>
        <td>
            <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="red"></asp:Label>
        </td>
    </tr>
</table>
