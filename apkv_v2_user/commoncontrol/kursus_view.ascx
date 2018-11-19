<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="kursus_view.ascx.vb" Inherits="apkv_v2_user.kursus_view" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Carian Dan Penyelenggaraan >> Program >> Paparan Maklumat Program</td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Paparan Maklumat Program</td>
    </tr>
    <tr>
           <td style="width: 20%;">Kohort:</td>
        <td><asp:Label ID="lblTahun" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Sesi Pengambilan:</td>
        <td><asp:Label ID="lblSesi" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Nama Nama Bidang:</td>
        <td><asp:Label ID="lblNamaKluster" runat="server"></asp:Label>
         </td>
    </tr>
    <tr>
           <td style="width: 20%;">Kod Program:</td>
        <td><asp:Label ID="lblKodKursus" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Nama Program:</td>
        <td><asp:Label ID="lblNamaKursus" runat="server"></asp:Label>
        </td>
    </tr>
<tr>
    <td>&nbsp;</td>
</tr>
    <tr>
        <td colspan="2"><asp:Button ID="btnExecute" runat="server" Text="Hapuskan" CssClass="fbbutton" />
        </td>
</table>
<br />
<div class="info" id="divMsg" runat="server">
  <asp:Label ID="lblKolejID" runat="server" Text="" Visible="false"></asp:Label>
   <asp:Label ID="lblMsg" runat="server" Text="Mesej..."></asp:Label>
</div>

