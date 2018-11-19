<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="pelajar.ulang.vokasional.view.ascx.vb" Inherits="apkv_v2_user.pelajar_ulang_vokasional_view" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Carian Dan Penyelengaraan >> Calon >> Paparan Calon Ulang Vokasional </td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Paparan Calon Ulang Vokasional.</td>
    </tr>
    <tr><td  colspan="2"></td></tr>
     <tr>
         <td style="width: 20%;">Peringkat Pengajian:</td>
        <td><asp:Label ID="lblPengajian" runat="server"></asp:Label></td>
    </tr>
     <tr>
         <td>&nbsp;</td>
        <td>&nbsp;</td>
    </tr>
    <tr>
         <td style="width: 20%;">Kohort:</td>
         <td><asp:Label ID="lblTahun" runat="server"></asp:Label>
        </td>
    </tr>
     <tr>
          <td style="width: 20%;">Semester:</td>
         <td>
            <asp:Label ID="lblSemester" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Sesi Pengambilan:</td>
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
            <asp:Label ID="lblKursusID" runat="server" Text="" Visible ="false" ></asp:Label>
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
         <td style="width: 20%;">Nama Calon:
        </td>
         <td>
            <asp:Label ID="lblNama" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td style="width: 20%;">Mykad:</td>
         <td>
            <asp:Label ID="lblMYKAD" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Angka Giliran
            :</td>
         <td>
            <asp:Label ID="lblAngkaGiliran" runat="server" Text=""></asp:Label>
        </td>
    </tr>
   
   </table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Daftar Calon Ulang Vokasional.</td>
    </tr>
     <tr>
         <td>&nbsp;</td>
        <td>&nbsp;</td>
    </tr>
      <tr>
          <td style="width: 20%;">Kohort:</td>
        <td>
            <asp:DropDownList ID="ddlTahun" runat="server" AutoPostBack="false" Width="350px"></asp:DropDownList>
        </td>
    </tr>
     <tr>
          <td style="width: 20%;">Semester:</td>
        <td>
            <asp:DropDownList ID="ddlSemester" runat="server" AutoPostBack="false" Width="350px"></asp:DropDownList>
        </td>
    </tr>
     <tr>
          <td style="width: 20%;">Sesi Pengambilan:</td>
        <td><asp:CheckBoxList ID="chkSesi" runat="server"  AutoPostBack="true" Width="349px" RepeatDirection="Horizontal">
             <asp:ListItem>1</asp:ListItem>
             <asp:ListItem>2</asp:ListItem>
             </asp:CheckBoxList></td>
    </tr>
   <tr>
          <td style="width: 20%;">Kod Program:</td>
        <td>
            <asp:DropDownList ID="ddlKodKursus" runat="server" AutoPostBack="true" Width="350px"></asp:DropDownList>
        </td>
    </tr>
     <tr>        
        <td style="width: 20%;">Kelas:</td>
          <td><asp:DropDownList ID="ddlNamaKelas" runat="server" AutoPostBack="false" Width="350px"></asp:DropDownList></td>
    </tr>
    </table>
<br />
<table class="fbform">
    <tr class="fbform_header">
         <td colspan ="6">Senarai Vokasional.
        </td>
   </tr>
         <tr>
         <td style="width: 50%; text-align: left;">Nama Kursus:</td>
         <td style="width: 10%; text-align: center;">Gred</td>
         <td style="width: 10%; text-align: center;">PB Teori</td>
         <td style="width: 10%; text-align: center;">PB Amali</td>
         <td style="width: 10%; text-align: center;">PA Teori</td>
         <td style="width: 10%; text-align: center;">PA Amali</td>
    </tr>
     <tr>
         <td style="width: 50%; text-align: left;"><asp:Label ID="lblModul1" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:Label ID="lblGred1" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB1" runat="server" /> </td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB9" runat="server" /> </td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA1" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA9" runat="server" /></td>
    </tr>
     <tr>
         <td style="width: 50%; text-align: left;"><asp:Label ID="lblModul2" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:Label ID="lblGred2" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB2" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB10" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA2" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA10" runat="server" /></td>
        
    </tr>
     <tr>
         <td style="width: 50%; text-align: left;"><asp:Label ID="lblModul3" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:Label ID="lblGred3" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB3" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB11" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA3" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA11" runat="server" /></td>

    </tr>
     <tr>
         <td style="width: 50%; text-align: left;"><asp:Label ID="lblModul4" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:Label ID="lblGred4" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB4" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB12" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA4" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA12" runat="server" /></td>
    </tr>
     <tr>
         <td style="width: 50%; text-align: left;"><asp:Label ID="lblModul5" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:Label ID="lblGred5" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB5" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB13" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA5" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA13" runat="server" /></td>
     </tr>
     <tr>
         <td style="width: 50%; text-align: left;"><asp:Label ID="lblModul6" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:Label ID="lblGred6" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB6" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB14" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA6" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA14" runat="server" /></td>
    </tr>
     <tr>
         <td style="width: 50%; text-align: left;"><asp:Label ID="lblmodul7" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:Label ID="lblGred7" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB7" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB15" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA7" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA15" runat="server" /></td>
    </tr>
     <tr>
         <td style="width: 50%; text-align: left;"><asp:Label ID="lblModul8" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:Label ID="lblGred8" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB8" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB16" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA8" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA16" runat="server" /></td>
    </tr>
    <tr>
        <td class="fbform_sap">&nbsp;
        </td>
    </tr>
    <tr>
         <td>
            <asp:Button ID="btnApprove" runat="server" Text="Simpan" CssClass="fbbutton" />&nbsp;<asp:Button
                ID="btnCancel" runat="server" Text="Batal" CssClass="fbbutton" />
        </td>
    </tr>
   </table>
<div class="info" id="divMsg" runat="server">           
    <asp:Label ID="lblKolejID" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
</div>