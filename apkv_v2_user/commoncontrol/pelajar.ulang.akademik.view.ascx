<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="pelajar.ulang.akademik.view.ascx.vb" Inherits="apkv_v2_user.peajar_ulang_akademik_view" %>
<div>
    <table class="fbform" style="width:100%">
        <tr>
            <td>
                <a href ="pelajar.ulang.akademik.aspx" ><img title="Back" style="vertical-align: middle;" src="icons/back.png" width="20" height="20" alt="::"/></a>
               </td>
        </tr>
        <tr>
            <td></td>
        </tr>
        <tr class="fbform_header">
            <td>Carian Dan Penyelengaraan >> Calon >> Paparan Calon Ulang Akademik</td>
        </tr>
    </table>
</div>
<br />

<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Paparan Calon Ulang Akademik.</td>
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
        <td colspan="2">Daftar Calon Ulang&nbsp; Akademik.</td>
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
         <td>Senarai Akademik.
        </td>
    </tr>
    <tr>
         <td colspan ="4" ></td>
    </tr>
       <tr>
         <td style="width: 50%; text-align: left;">Nama MataPelajaran:</td>
         <td style="width: 30%; text-align: center;">Gred</td>
         <td style="width: 10%; text-align: center;">PB</td>
         <td style="width: 10%; text-align: center;">PA</td>
    </tr>
     <tr>
         <td style="width: 50%; text-align: left;">BAHASA MELAYU</td>
         <td style="width: 30%; text-align: center;"><asp:Label ID="lblBM" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB1" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA1" runat="server" /></td>
    </tr>
    <tr>
         <td style="width: 50%; text-align: left;">BAHASA INGGERIS</td>
         <td style="width: 30%; text-align: center;"><asp:Label ID="lblBI" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB2" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA2" runat="server" /></td>
    </tr>
     <tr>
         <td style="width: 50%; text-align: left;">MATHEMATIC</td>
         <td style="width: 30%; text-align: center;"><asp:Label ID="lblMT" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB3" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA3" runat="server" /></td>
    </tr>
     <tr>
         <td style="width: 50%; text-align: left;">SCIENCE</td>
         <td style="width: 30%; text-align: center;"><asp:Label ID="lblSC" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB4" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA4" runat="server" /></td>
    </tr>
     <tr>
         <td style="width: 50%; text-align: left;">SEJARAH</td>
         <td style="width: 30%; text-align: center;"><asp:Label ID="lblSJ" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB5" runat="server" /></td>
         <td style="width: 10%; text-align: center;"> <asp:CheckBox ID="PA5" runat="server" /></td>
    </tr>
     <tr>
         <td style="width: 50%; text-align: left;">PENDIDIKAN ISLAM</td>
         <td style="width: 30%; text-align: center;"><asp:Label ID="lblPI" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB6" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA6" runat="server" /></td>
    </tr>
     <tr>
         <td style="width: 50%; text-align: left;">PENDIDIKAN MORAL</td>
         <td style="width: 30%; text-align: center;"><asp:Label ID="lblPM" runat="server" Text=""></asp:Label></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PB7" runat="server" /></td>
         <td style="width: 10%; text-align: center;"><asp:CheckBox ID="PA7" runat="server" /></td>
    </tr>
    <tr class="fbform_sap">
        <td colspan ="4">
        </td>
    </tr>
    <tr>
          <td colspan ="4">
            <asp:Button ID="btnApprove" runat="server" Text="Simpan" CssClass="fbbutton" />&nbsp;<asp:Button
                ID="btnCancel" runat="server" Text="Batal" CssClass="fbbutton" />
        </td>
    </tr>
   </table>
<div class="info" id="divMsg" runat="server">           
    <asp:Label ID="lblKolejID" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
</div>