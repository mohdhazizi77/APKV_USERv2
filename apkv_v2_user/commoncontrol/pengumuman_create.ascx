<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="pengumuman_create.ascx.vb" Inherits="apkv_v2_user.pengumuman_create" %>

<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Daftar Pengumuman Baru
        </td>
    </tr>
    <tr>
        <td class="fbtd_left">Tahun:
        </td>
        <td>
            <asp:DropDownList ID="ddlTahun" runat="server" AutoPostBack="false" Width="100px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>Paparkan:
        </td>
        <td>
            <select name="selIsDisplay" id="selIsDisplay" style="width: 100px;" runat="server">
                <option value="Y" selected="selected">YA</option>
                <option value="N">TIDAK</option>
            </select>
        </td>
    </tr>
    <tr>
        <td>Tajuk:
        </td>
        <td>
            <asp:TextBox ID="txtTitle" runat="server" Width="600px" MaxLength="250"></asp:TextBox>&nbsp;
        </td>
    </tr>
    <tr>
        <td style="width: 20%; vertical-align: top;">Pengumuman:
        </td>
        <td>
            <asp:TextBox ID="txtBody" runat="server" Width="600px" TextMode="MultiLine" Rows="10"></asp:TextBox>&nbsp;
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td class="fbform_sap">
            <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="column_width">&nbsp;
        </td>
        <td>
            <asp:Button ID="btnadd" runat="server" Text=" Tambah " CssClass="fbbutton" />
            &nbsp;|&nbsp;<asp:LinkButton ID="lnkList" runat="server">Senarai Pengumuman</asp:LinkButton>
        </td>
    </tr>
    
</table>
