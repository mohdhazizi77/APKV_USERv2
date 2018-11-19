<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="kolej_view.ascx.vb"
    Inherits="apkv_v2_user.kolej_view" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Carian Dan Penyelenggaraan >> Kolej >> Paparan Maklumat Kolej</td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Paparan Maklumat Kolej
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Jenis Kolej:</td>
         <td>
            <asp:Label ID="lblJenis" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Kod Kolej:</td>
         <td>
            <asp:Label ID="lblKod" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Nama Kolej:</td>
         <td>    
        <asp:Label ID="lblNama" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Telefon:</td>
         <td>
            <asp:Label ID="lblTel" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Fax:</td>
         <td>
            <asp:Label ID="lblFax" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Emel:</td>
         <td>
            <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Alamat
            :</td>
         <td>
            <asp:Label ID="lblAlamat1" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">
        </td>
         <td>
            <asp:Label ID="lblAlamat2" runat="server" Text=""></asp:Label>
        </td>
    </tr>
     <tr>
           <td style="width: 20%;">Poskod
            :</td>
         <td>
            <asp:Label ID="lblPoskod" runat="server" Text=""></asp:Label>
        </td>
    </tr>
     <tr>
           <td style="width: 20%;">Bandar
            :</td>
         <td>
            <asp:Label ID="lblBandar" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Negeri
            :</td>
         <td>
            <asp:Label ID="lblNegeri" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan ="2"></td>
    </tr>
    <tr class="fbform_header">
        <td colspan="2">Maklumat Lain
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Nama Pengarah:</td>
         <td>
            <asp:Label ID="lblNamaPengarah" runat="server" Text=""></asp:Label>
        </td>
    </tr>
     <tr>
          <td style="width: 20%;">Jawatan:</td>
         <td>
            <asp:Label ID="lblJawatanPengarah" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Gred:</td>
         <td>
            <asp:Label ID="lblGredPengarah" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Emel:</td>
         <td>
            <asp:Label ID="lblEmailPengarah" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Telefon Bimbit:</td>
         <td>
            <asp:Label ID="lblBimbitPengarah" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Telefon:</td>
         <td>
            <asp:Label ID="lblTelPengarah" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td>&nbsp;</td>
         <td>
            &nbsp;</td>
    </tr>
    <tr>
           <td style="width: 20%;">Nama KJPP:</td>
         <td>
            <asp:Label ID="lblNamaKJPP" runat="server"></asp:Label>
        </td>
    </tr>
     <tr>
          <td style="width: 20%;">Jawatan:</td>
         <td>
            <asp:Label ID="lblJawatanKJPP" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Gred:</td>
         <td>
            <asp:Label ID="lblGredKJPP" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Emel:</td>
         <td>
            <asp:Label ID="lblEmailKJPP" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Telefon Bimbit:</td>
         <td>
            <asp:Label ID="lblBimbitKJPP" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Telefon:</td>
         <td>
            <asp:Label ID="lblTelKJPP" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
         <td>&nbsp;</td>
         <td>
            &nbsp;</td>
    </tr>
    <tr>
           <td style="width: 20%;">Nama SUP:</td>
         <td>
            <asp:Label ID="lblNamaSUP" runat="server"></asp:Label>
        </td>
    </tr>
     <tr>
          <td style="width: 20%;">Jawatan:</td>
         <td>
            <asp:Label ID="lblJawatanSUP" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Gred:</td>
         <td>
            <asp:Label ID="lblGredSUP" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Emel:</td>
         <td>
            <asp:Label ID="lblEmailSUP" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Telefon Bimbit:</td>
         <td>
            <asp:Label ID="lblBimbitSUP" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Telefon:</td>
         <td>
            <asp:Label ID="lblTelSUP" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="fbform_sap" colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td colspan="2"><asp:Button ID="btnExecute" runat="server" Text="Kemaskini" CssClass="fbbutton" />
        </td>
    </tr>
   
</table>
<br />
<div class="info" id="divMsg" runat="server">
 <asp:Label ID="lblKolejID" runat="server" Text="" Visible="false"></asp:Label>
 <asp:Label ID="lblMsg" runat="server" Text="Mesej..."></asp:Label>
</div>
