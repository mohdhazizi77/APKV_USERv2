<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="pelajar_tangguh_update.ascx.vb" Inherits="apkv_v2_user.pelajar_tangguh_update" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Paparan Maklumat Calon</td>
    </tr>
    <tr><td  colspan="2"></td></tr>
     <tr>
         <td style="width: 20%;">Kolej:</td>
        <td><asp:Label ID="lblKolej" runat="server"></asp:Label><asp:Label ID="lblRecordID" runat="server" Visible ="false" ></asp:Label></td>
    </tr>
     <tr>
         <td style="width: 20%;">Peringkat Pengajian:</td>
        <td><asp:Label ID="lblPengajian" runat="server"></asp:Label></td>
    </tr>
     <tr>
         <td colspan ="2"></td>
    </tr>
    <tr>
         <td style="width: 20%;">Kohort:</td>
         <td><asp:Label ID="lblTahun" runat="server"></asp:Label></td>
    </tr>
    <tr>
          <td style="width: 20%;">Sesi Pengambilan:</td>
         <td><asp:Label ID="lblSesi" runat="server"></asp:Label></td>
    </tr>
    <tr>
         <td style="width: 20%;">Nama Bidang:</td>
         <td><asp:Label ID="lblNamaKluster" runat="server"></asp:Label></td>
    </tr>
    <tr>
         <td style="width: 20%;">Kod Program:</td>
         <td> <asp:Label ID="lblKodKursus" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
          <td style="width: 20%;">Nama Program:</td>
         <td><asp:Label ID="lblNamaKursus" runat="server"></asp:Label></td>
    </tr>
    <tr>
          <td style="width: 20%;">Nama Kelas:</td>
         <td>
            <asp:Label ID="lblNamaKelas" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
         <td colspan="2"></td>
    </tr>
     <tr>
          <td style="width: 20%;">Nama Calon: </td>
         <td>
            <asp:Label ID="txtNama" runat="server" Width="350px" MaxLength="250"></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Mykad:</td>
         <td><asp:Label ID="lblMykad" runat="server" ></asp:Label></td>
    </tr>
     <tr>
         <td style="width: 20%;">AngkaGiliran:</td>
         <td><asp:Label ID="lblAngkaGiliran" runat="server"></asp:Label></td>
    </tr>
    <tr>
          <td style="width: 20%;">Jantina:</td>
         <td><asp:Label ID="lblJantina" runat="server"></asp:Label></td>
         
    </tr>
    <tr>
          <td style="width: 20%;">Kaum:</td>
         <td><asp:Label ID="lblkaum" runat="server"></asp:Label></td>
    </tr>
    <tr>
         <td style="width: 20%;">Agama:</td>
         <td><asp:Label ID="lblAgama" runat="server"></asp:Label></td>
    </tr>
    <tr>
         <td style="width: 20%;">Emel:</td>
         <td>
            <asp:Label ID="txtEmail" runat="server" Width="350px" MaxLength="250"></asp:Label>
        </td>
    </tr>
      <tr>
         <td style="width: 20%;">Telefon:</td>
         <td>
            <asp:Label ID="lbltelefon" runat="server" Width="350px" MaxLength="250"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="2"></td>
    </tr>

    <tr>
          <td style="width: 20%;">Catatan:</td>
         <td>
            <asp:Textbox ID="txtCatatan" runat="server" Width="460px" Height="117px" TextMode="MultiLine"></asp:Textbox>
        </td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Daftar Calon Tangguh</td>
    </tr>
     <tr>
         <td style="width: 20%;">Kohort:</td>
         <td> <asp:DropDownList ID="ddlTahun" runat="server" AutoPostBack="false" Width="350px"></asp:DropDownList></td>
    </tr>
     <tr>
         <td style="width: 20%;">Semester:</td>
         <td> <asp:DropDownList ID="ddlSemester" runat="server" AutoPostBack="false" Width="350px"></asp:DropDownList></td>
    </tr>
    <tr>
         <td style="width: 20%;">Sesi Pengambilan:</td>
         <td><asp:CheckBoxList ID="chkSesi" runat="server" AutoPostBack="true" Width="349px" RepeatDirection="Horizontal">
             <asp:ListItem>1</asp:ListItem>
             <asp:ListItem>2</asp:ListItem>
             </asp:CheckBoxList>
    </tr>
     <tr>
          <td style="width: 20%;">Kod Program:</td>
        <td>
            <asp:DropDownList ID="ddlKodKursus" runat="server" AutoPostBack="true" Width="350px"></asp:DropDownList>
        </td>
    </tr>
     <tr>        
        <td style="width: 20%;">Nama Kelas:</td>
          <td><asp:DropDownList ID="ddlKelas" runat="server" AutoPostBack="false" Width="350px"></asp:DropDownList></td>
    </tr>
   
      <tr>
        <td colspan="2"><asp:Button ID="btnCreate" runat="server" Text="Simpan" CssClass="fbbutton" /></td>
    </tr>
</table>
<div class="info" id="divMsg" runat="server">
        <asp:Label ID="lblKod" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="lblKolejID" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="lblMsg" runat="server" Text="Mesej..."></asp:Label>
</div>


