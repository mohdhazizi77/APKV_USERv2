<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="takwim_create.ascx.vb" Inherits="apkv_v2_user.takwim_create" %>

<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Tambah Takwim Baru
        </td>
    </tr>
    <tr>
        <td class="fbtd_left">Tahun:
        </td>
        <td>
            <asp:DropDownList ID="ddlTahun" runat="server" AutoPostBack="false" Width="150px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>Kategori:
        </td>
        <td>
            <asp:DropDownList ID="ddlMenu" runat="server" AutoPostBack="false" Width="150px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="width: 20%; vertical-align: top;">Tarikh Mula:
        </td>
        <td>
            <asp:TextBox ID="txtTarikhMula" runat="server" Width="150px" MaxLength="250"></asp:TextBox><asp:Label ID="lblTarikhMula" runat="server" Text="" ForeColor="red" Visible="False"></asp:Label>*&nbsp;
            <asp:ImageButton ID="btnDateMula" runat="server" ImageUrl="~/img/department-store-emoticon.png" AlternateText=".." Width="15" Height="15" />
            <asp:Calendar ID="calTarikhMula" runat="server" Visible="false" BackColor="White"></asp:Calendar>
        </td>
    </tr>
    <tr>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td style="width: 20%; vertical-align: top;">Tarikh Akhir:
        </td>
        <td>
            <asp:TextBox ID="txtTarikhAkhir" runat="server" Width="150px" MaxLength="250"></asp:TextBox>

<asp:Label ID="lblTarikhAkhir" runat="server" Text="" ForeColor="red" Visible="False"></asp:Label>*&nbsp;
            <asp:ImageButton ID="btnDateAkhir" runat="server" ImageUrl="~/img/department-store-emoticon.png" AlternateText=".." Width="15" Height="15" />
            <asp:Calendar ID="calTarikhAkhir" runat="server" Visible="false" BackColor="White"></asp:Calendar>
        </td>
    </tr>
    <tr>
        <td >Tajuk:
        </td>
        <td>
            <asp:TextBox ID="txtTitle" runat="server" Width="450px" MaxLength="250"></asp:TextBox>&nbsp;
        </td>
    </tr>
    <tr>
        <td style="width: 20%; vertical-align: top;">Catatan:
        </td>
        <td>
            <asp:TextBox ID="txtCatatan" runat="server" Width="450px" TextMode="MultiLine" Rows="10"></asp:TextBox>&nbsp;
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
            &nbsp;|&nbsp;<asp:LinkButton ID="lnkList" runat="server">Senarai Takwim</asp:LinkButton>
        </td>
    </tr>

</table>

