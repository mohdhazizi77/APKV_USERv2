<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="permohonan.pindah.update.ascx.vb" Inherits="apkv_v2_user.permohonan_pindah_update" %>
<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
<script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
<link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="Stylesheet" type="text/css" />

<script type="text/javascript">
    $(function () {
        $("[id$=txtEventDate]").datepicker({
            dateFormat: 'dd-mm-yy',
            showOn: 'button',
            buttonImageOnly: true,
            buttonImage: '/icons/calendar.gif'
        });
    });
</script>


<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Permohonan Berpindah >> Calon >> Kemaskini Permohonan Berpindah </td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">
            Kemaskini Permohonan Berpindah Calon
        </td>
    </tr>
     <tr>
          <td style="width: 20%;">Tahun:</td>
         <td>
            <asp:Label ID="lblTahun" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Semester:</td>
         <td>
            <asp:Label ID="lblSemester" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Sesi:</td>
         <td>
            <asp:Label ID="lblSesi" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Nama Bidang:</td>
         <td>
            <asp:Label ID="lblKluster" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Kod Program:</td>
         <td>
            <asp:Label ID="lblKodKursus" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Nama Program:</td>
         <td>
            <asp:Label ID="lblNamaKursus" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Nama Kelas:</td>
         <td>
            <asp:Label ID="lblNamaKelas" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
         <td></td>
    </tr>
     <tr>
           <td style="width: 20%;">Nama Calon: </td>
         <td>
            <asp:Label ID="lblNama" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Mykad :</td>
         <td>
            <asp:Label ID="lblMykad" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">
            Angka Giliran:</td>
         <td>
          
            <asp:Label ID="lblAngkaGiliran" runat="server"></asp:Label>
         </td>
    </tr>
    
    <tr>
           <td style="width: 20%;">Jantina:</td>
         <td>
            <asp:Label ID="lblJantina" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Kaum:</td>
         <td>
            <asp:Label ID="lblKaum" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;"> Agama:</td>
         <td>
            <asp:Label ID="lblAgama" runat="server"></asp:Label>
        </td>
    </tr>
     <tr>
           <td style="width: 20%;">Emel:</td>
         <td>
            <asp:Label ID="lblEmail" runat="server"></asp:Label>
        &nbsp;</td>
    </tr>
     <tr>
         <td colspan="2"> </td>
     </tr>
    <tr>
           <td style="width: 20%;">Status:</td>
         <td> <asp:Label ID="lblStatus" runat="server"></asp:Label></td>
    </tr>
    <tr>
           <td style="width: 20%;">JenisCalon:</td>
         <td> <asp:Label ID="lblJenisCalon" runat="server"></asp:Label></td>
    </tr>
      <tr>
          <td style="width: 20%;">Catatan:</td>
         <td>
            <asp:TextBox ID="txtCatatan"  runat="server" Width="350px" MaxLength="250" Height="117px" TextMode="MultiLine"></asp:TextBox>*
        </td>
    </tr>
    </table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">
          Permohonan Berpindah Calon - Pilihan Kolej
        </td>
    </tr>
     <tr>
          <td style="width: 20%;">Jenis Kolej:</td>
         <td>
             <asp:DropDownList ID="ddlJenisKolej" runat="server" AutoPostBack="true" Width="350px"></asp:DropDownList>*
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Nama Kolej:</td>
         <td>
             <asp:DropDownList ID="ddlNamaKolej" runat="server" AutoPostBack="false" Width="350px"></asp:DropDownList>*
        </td>
    </tr>
 </table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">
          Permohonan Berpindah Calon - Pengesahan 
        </td>
    </tr> 
    <tr>
        <td colspan="2">
        <asp:CheckBox ID="chkconfirm" runat="server" text=""/>
           <asp:Label ID="lblConfirm" runat="server" Text="Calon ini telah mendapat kelulusan berpindah daripada bahagian BPTV pada tarikh " Font-Bold="False" ForeColor="Red"></asp:Label>: <asp:TextBox ID="txtEventDate" runat="server" Width="150px" MaxLength="250"></asp:TextBox>&nbsp;
           <asp:Label ID="lblConfirm1" runat="server" Text="dan surat rujukan bernombor " ForeColor="Red"></asp:Label>:<asp:TextBox ID="txtRef" runat="server" Width="200px" MaxLength="25"></asp:TextBox>*
        </td>
    </tr>
    <tr>
        <td colspan="2"></td>
    </tr>
    <tr>
        <td colspan="2"><asp:Button ID="btnUpdate" runat="server" Text="Simpan" CssClass="fbbutton" /></td>
        <td>&nbsp;</td>
    </tr>
</table>
<br />
<div class="info" id="divMsg" runat="server">
 <asp:Label ID="lblKolejID" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblKursusID" runat="server" Text="" Visible="false"></asp:Label>
  <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label></div>
