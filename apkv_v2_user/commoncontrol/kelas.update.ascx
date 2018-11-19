<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="kelas.update.ascx.vb" Inherits="apkv_v2_user.kelas_update" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Carian Dan Penyelenggaraan >> Kelas >> Kemaskini Maklumat Kelas</td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Kemaskini Maklumat Kelas
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Kohort:</td>
        <td><asp:TextBox ID="txtTahun" runat="server" Width="350px" Enabled ="false"></asp:TextBox>*</td>
    </tr>
    <tr>
        <td style="width: 20%;">Nama Kelas:</td>
         <td><asp:TextBox ID="txtNamaKelas" runat="server" Width="350px"></asp:TextBox>*</td>
    </tr>
     
    <tr>
         <td style="width: 20%;">&nbsp;</td>
         <td>
            <asp:Label ID="lblNamaKelas" runat="server" Text="" Visible ="false"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="2"><asp:Button ID="btnUpdate" runat="server" Text="Simpan" CssClass="fbbutton" />
        </td>
    </tr>
   
</table>
<br />
<div class="info" id="divMsg" runat="server">
<asp:Label ID="lblKolejID" runat="server" Text="" Visible="false"></asp:Label>
<asp:Label ID="lblMsg" runat="server" Text="Mesej..."></asp:Label>
</div>