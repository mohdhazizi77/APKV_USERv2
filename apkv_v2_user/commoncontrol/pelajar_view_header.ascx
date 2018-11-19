<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="pelajar_view_header.ascx.vb"
    Inherits="apkv_v2_user.pelajar_view_header" %>
<table class="fbform">
    <tr class="fbform_header">
        <td style="width:15%;">Maklumat Calon
        </td>
        <td style="text-align: right;">
            <asp:LinkButton ID="lnkUpdate" runat="server">Kemaskini</asp:LinkButton>
        </td>
    </tr>
    <tr>
         <td>Nama Calon:
        </td>
         <td>
            <asp:Label ID="lblNama" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td>MYKAD:</td>
         <td>
            <asp:Label ID="lblMYKAD" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td>Angka Giliran
            :</td>
         <td>
            <asp:Label ID="lblAngkaGiliran" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td>Kohort:</td>
         <td>
            <asp:Label ID="lblTahun" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
         <td>Semester:</td>
         <td>
            <asp:Label ID="lblSemester" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
         <td>Sesi Pengambilan:</td>
         <td>
            <asp:Label ID="lblSesi" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
         <td>Kod Program
            :</td>
         <td>
            <asp:Label ID="lblKodKursus" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td>Jantina
            :</td>
         <td>
            <asp:Label ID="lblJantina" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td>Kaum
            :</td>
         <td>
            <asp:Label ID="lblKaum" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td>Agama
            :</td>
         <td>
            <asp:Label ID="lblAgama" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td>Status
            :</td>
         <td>
            <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td>Emel:</td>
         <td>
            <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label>
        </td>
    </tr>

    <tr>
         <td>Catatan
            :</td>
         <td>
            <asp:Label ID="lblCatatan" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
        </td>
    </tr>
</table>
