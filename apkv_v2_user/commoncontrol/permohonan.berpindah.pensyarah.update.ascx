<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="permohonan.berpindah.pensyarah.update.ascx.vb" Inherits="apkv_v2_user.permohonan_berpindah_pensyarah_update" %>
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
        <td colspan="2">Permohonan Berpindah >> Pensyarah >> Kemaskini Permohonan Berpindah </td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Paparan Maklumat Pensyarah</td>
    </tr>
    <tr>
         <td style="width: 20%;">Nama Pensyarah:</td>
        <td><asp:Label ID="lblNama" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td style="width: 20%;">Jawatan:</td>
        <td><asp:Label ID="lblJawatan" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td style="width: 20%;">Gred:</td>
        <td><asp:Label ID="lblGred" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td style="width: 20%;">Mykad:</td>
        <td><asp:Label ID="lblMYKAD" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td style="width: 20%;">Telefon:</td>
        <td><asp:Label ID="lblTel" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
         <td style="width: 20%;">Emel:</td>
        <td><asp:Label ID="lblEmail" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td style="width: 20%;">Jantina:</td>
        <td><asp:Label ID="lblJantina" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Kaum:</td>
        <td>
            <asp:Label ID="lblKaum" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td style="width: 20%;">Agama:</td>
        <td>
            <asp:Label ID="lblAgama" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="width: 20%;">Status:</td>
        <td><asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="2"></td>
    </tr>
  
    </table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">
          Permohonan Berpindah Pensyarah - Pilihan Kolej
        </td>
    </tr>
     <tr>
          <td style="width: 20%;">Jenis Kolej:</td>
         <td style="width: 80%;"><asp:DropDownList ID="ddlJenisKolej" runat="server" AutoPostBack="true" Width="350px"></asp:DropDownList>*
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Nama Kolej:</td>
         <td style="width: 80%;"><asp:DropDownList ID="ddlNamaKolej" runat="server" AutoPostBack="false" Width="350px"></asp:DropDownList>*
        </td>
    </tr>
 </table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">
          Permohonan Berpindah Pensyarah - Pengesahan 
        </td>
    </tr> 
    <tr>
        <td colspan="2">
        <asp:CheckBox ID="chkconfirm" runat="server" text=""/>
           <asp:Label ID="lblConfirm" runat="server" Text="Pensyarah ini telah mendapat kelulusan berpindah daripada bahagian BPTV pada tarikh " Font-Bold="False" ForeColor="Red"></asp:Label>: <asp:TextBox ID="txtEventDate" runat="server" Width="150px" MaxLength="250"></asp:TextBox>&nbsp;      
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
  <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label></div>
