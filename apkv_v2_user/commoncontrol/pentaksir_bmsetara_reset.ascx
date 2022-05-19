<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="pentaksir_bmsetara_reset.ascx.vb" Inherits="apkv_v2_user.pentaksir_bmsetara_reset" %>

<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Reset Markah Penilaian Calon</td>
    </tr>
</table>

<br />

<table class="fbform">
    <tr>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td>Sila masukkan No. KP atau Angka Giliran calon dan klik butang <b>Cari</b></td>
        <td></td>
        <td></td>
    </tr>

    <tr>
        <td></td>
        <td></td>
        <td></td>
    </tr>

    <tr>
        <td>
            <asp:Label ID="Label3" runat="server" Text="No. KP atau Angka Giliran :" Width="200px"></asp:Label>

            <asp:TextBox ID="txtCarian" runat="server"></asp:TextBox>
            <asp:Button ID="btnCari" runat="server" Text="Cari" /></td>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td></td>
        <td></td>
    </tr>

</table>

<br />

<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Reset Markah Penilaian Calon</td>
    </tr>
</table>

<br />

<table class="fbform">
    <tr>
        <td></td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <td>Sila lengkapkan medan yang diperlukan dan klik butang <b>Reset</b></td>
    </tr>

    <tr>
        <td></td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <td></td>
    </tr>

    <tr>
        <td>
            <asp:Label ID="Label1" runat="server" Text="Angka Giliran Calon :" Width="200px"></asp:Label>
            <asp:Label ID="lblAG" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label2" runat="server" Text="No. K/P Calon :" Width="200px"></asp:Label>
            <asp:Label ID="lblMYKAD" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label4" runat="server" Text="Nama Calon :" Width="200px"></asp:Label>
            <asp:Label ID="lblNama" runat="server"></asp:Label></td>
    </tr>

    <tr>
        <td></td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <td></td>
    </tr>

    <tr>
        <td>
            <asp:Label ID="Label6" runat="server" Text="Mata Pelajaran / Kertas :" Width="200px"></asp:Label>
            <asp:DropDownList ID="ddlMP" runat="server" Width="250px"></asp:DropDownList></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label8" runat="server" Text="Pentaksir :" Width="200px"></asp:Label>
            <asp:DropDownList ID="ddlPentaksir" runat="server" Width="250px"></asp:DropDownList></td>
    </tr>

    <tr>
        <td></td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <td></td>
    </tr>

    <tr>
        <td>
            <asp:Label ID="Label5" runat="server" Width="200px"></asp:Label>
            <asp:Button ID="btnBack" runat="server" Text="Kembali" />
            <asp:Button ID="btnReset" runat="server" Text="Reset" />
        </td>
    </tr>

</table>

<br />

<div class="info" id="divMsg" runat="server">
    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label><br />
</div>

