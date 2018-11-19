<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="slip_view.ascx.vb" Inherits="apkv_v2_user.slip_view" %>
<table class="fbform">
    <tr class="fbform_header">
        <td style="width:15%;"><img src="../img/logogov.png" alt="GOV" /></td>
         <td>KEMENTERIAN PENDIDIKAN MALAYSIA</td>
    </tr>
    <tr>
       <td style="width:15%;"></td>
         <td>MINISTRY OF EDUCATION MALAYSIA</td>
    </tr>
    <tr>
        <td style="width:15%;"></td>
         <td></td>
    </tr>
     <tr>
        <td style="width:15%;"></td>
         <td>Transkrip Sijil Vokasional Malaysia</td>
    </tr>
     <tr>
        <td style="width:15%;"></td>
         <td></td>
    </tr>
</table>
<table class="fbform">
    <tr class="fbform_header">
        <td style="width:15%;">NAMA:</td>
         <td><asp:Label ID="lblNama" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
       <td style="width:15%;">ANGKA GILIRAN:</td>
         <td><asp:Label ID="lblAngkaGiliran" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td style="width:15%;">INSTITUSI:</td>
         <td><asp:Label ID="lblKod" runat="server" Text=""></asp:Label></td>
    </tr>
     <tr>
        <td style="width:15%;">KLUSTER:</td>
         <td><asp:Label ID="lblKluster" runat="server" Text=""></asp:Label></td>
    </tr>
     <tr>
        <td style="width:15%;">KURSUS:</td>
         <td><asp:Label ID="lblKursus" runat="server" Text=""></asp:Label></td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td>
         <table class="fbform">
    <tr class="fbform_header">
        <td>SEMESTER<asp:Label ID="lblSemester" runat="server" Text=""></asp:Label></td>
        <td>TAHUN <asp:Label ID="lblTahun" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr class="fbform_header">
         <td>KOD</td>
         <td>MATA PELAJARAN</td>
         <td style="width:15%;">JAM KREDIT</td>
         <td style="width:5%;">GRED</td>
    </tr>
    <tr>
        <td>A01100</td>
        <td>BAHASA MELAYU</td>
    </tr>
    <tr>
        <td>A02100</td>
        <td>BAHASA INGGERIS</td>
    </tr>
    <tr>
        <td>A03100</td>
        <td>MATHEMATICS</td>
    </tr>
    <tr>
        <td>A04100</td>
        <td>SCIENCE</td>
    </tr>
    <tr>
        <td>A05100</td>
        <td>SEJARAH</td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodI_M" runat="server" Text=""></asp:Label></td>
        <td><asp:Label ID="lblI_M" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul1" runat="server" Text=""></asp:Label></td>
        <td><asp:Label ID="lblModul1" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul2" runat="server" Text=""></asp:Label></td>
        <td><asp:Label ID="lblModul2" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul3" runat="server" Text=""></asp:Label></td>
        <td><asp:Label ID="lblModul3" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul4" runat="server" Text=""></asp:Label></td>
        <td><asp:Label ID="lblModul4" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
       <td><asp:Label ID="lblKodModul5" runat="server" Text=""></asp:Label></td>
       <td><asp:Label ID="lblModul5" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr> 
       <td><asp:Label ID="lblKodModul6" runat="server" Text=""></asp:Label></td>
       <td><asp:Label ID="lblModul6" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul7" runat="server" Text=""></asp:Label></td>
        <td><asp:Label ID="lblModul7" runat="server" Text=""></asp:Label></td>
    </tr>
     <tr>
        <td><asp:Label ID="lblKodModul8" runat="server" Text=""></asp:Label></td>
        <td><asp:Label ID="lblModul8" runat="server" Text=""></asp:Label></td>
    </tr>
</table>
        </td>
        <td style="width:15%;"></td>
        <td>
         <table class="fbform">
    <tr class="fbform_header">
        <td>SEMESTER<asp:Label ID="lblSemester2" runat="server" Text=""></asp:Label></td>
        <td>TAHUN <asp:Label ID="lblTahun2" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr class="fbform_header">
         <td>KOD</td>
         <td>MATA PELAJARAN</td>
         <td style="width:15%;">JAM KREDIT</td>
         <td style="width:5%;">GRED</td>
    </tr>
    <tr>
        <td>A01100</td>
        <td>BAHASA MELAYU</td>
    </tr>
    <tr>
        <td>A02100</td>
        <td>BAHASA INGGERIS</td>
    </tr>
    <tr>
        <td>A03100</td>
        <td>MATHEMATICS</td>
    </tr>
    <tr>
        <td>A04100</td>
        <td>SCIENCE</td>
    </tr>
    <tr>
        <td>A05100</td>
        <td>SEJARAH</td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodI_M2" runat="server" Text=""></asp:Label></td>
        <td><asp:Label ID="lblI_M2" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul9" runat="server" Text=""></asp:Label></td>
        <td><asp:Label ID="lblModul9" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul10" runat="server" Text=""></asp:Label></td>
        <td><asp:Label ID="lblModul10" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul11" runat="server" Text=""></asp:Label></td>
        <td><asp:Label ID="lblmodul11" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul12" runat="server" Text=""></asp:Label></td>
        <td><asp:Label ID="lblModul12" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
       <td><asp:Label ID="lblKodModul13" runat="server" Text=""></asp:Label></td>
       <td><asp:Label ID="lblModul13" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr> 
       <td><asp:Label ID="lblKodModul14" runat="server" Text=""></asp:Label></td>
       <td><asp:Label ID="lblModul14" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul15" runat="server" Text=""></asp:Label></td>
        <td><asp:Label ID="lblModul15" runat="server" Text=""></asp:Label></td>
    </tr>
     <tr>
        <td><asp:Label ID="lblKodModul16" runat="server" Text=""></asp:Label></td>
        <td><asp:Label ID="lblModul16" runat="server" Text=""></asp:Label></td>
    </tr>
</table>
        </td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td>
         <table class="fbform">
    <tr class="fbform_header">
        <td>SEMESTER<asp:Label ID="lblSemester3" runat="server"></asp:Label></td>
        <td>TAHUN <asp:Label ID="lblTahun3" runat="server"></asp:Label></td>
    </tr>
    <tr class="fbform_header">
         <td>KOD</td>
         <td>MATA PELAJARAN</td>
         <td style="width:15%;">JAM KREDIT</td>
         <td style="width:5%;">GRED</td>
    </tr>
    <tr>
        <td>A01100</td>
        <td>BAHASA MELAYU</td>
    </tr>
    <tr>
        <td>A02100</td>
        <td>BAHASA INGGERIS</td>
    </tr>
    <tr>
        <td>A03100</td>
        <td>MATHEMATICS</td>
    </tr>
    <tr>
        <td>A04100</td>
        <td>SCIENCE</td>
    </tr>
    <tr>
        <td>A05100</td>
        <td>SEJARAH</td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul17" runat="server"></asp:Label></td>
        <td><asp:Label ID="lblModul17" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul18" runat="server"></asp:Label></td>
        <td><asp:Label ID="lblModul18" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul19" runat="server"></asp:Label></td>
        <td><asp:Label ID="lblModul19" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul20" runat="server"></asp:Label></td>
        <td><asp:Label ID="lblModul20" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul21" runat="server"></asp:Label></td>
        <td><asp:Label ID="lblModul21" runat="server"></asp:Label></td>
    </tr>
    <tr>
       <td><asp:Label ID="lblKodModul22" runat="server"></asp:Label></td>
       <td><asp:Label ID="lblModul22" runat="server"></asp:Label></td>
    </tr>
    <tr> 
       <td><asp:Label ID="lblKodModul23" runat="server"></asp:Label></td>
       <td><asp:Label ID="lblModul23" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul24" runat="server"></asp:Label></td>
        <td><asp:Label ID="lblModul24" runat="server"></asp:Label></td>
    </tr>
     <tr>
        <td><asp:Label ID="lblKodModul25" runat="server"></asp:Label></td>
        <td><asp:Label ID="lblModul25" runat="server"></asp:Label></td>
    </tr>
</table>
        </td>
        <td style="width:15%;"></td>
        <td>
         <table class="fbform">
    <tr class="fbform_header">
        <td>SEMESTER<asp:Label ID="lblSemester4" runat="server"></asp:Label></td>
        <td>TAHUN <asp:Label ID="lblTahun4" runat="server"></asp:Label></td>
    </tr>
    <tr class="fbform_header">
         <td>KOD</td>
         <td>MATA PELAJARAN</td>
         <td style="width:15%;">JAM KREDIT</td>
         <td style="width:5%;">GRED</td>
    </tr>
    <tr>
        <td>A01100</td>
        <td>BAHASA MELAYU</td>
    </tr>
    <tr>
        <td>A02100</td>
        <td>BAHASA INGGERIS</td>
    </tr>
    <tr>
        <td>A03100</td>
        <td>MATHEMATICS</td>
    </tr>
    <tr>
        <td>A04100</td>
        <td>SCIENCE</td>
    </tr>
    <tr>
        <td>A05100</td>
        <td>SEJARAH</td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul26" runat="server"></asp:Label></td>
        <td><asp:Label ID="lblModul26" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul27" runat="server"></asp:Label></td>
        <td><asp:Label ID="lblModul27" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul28" runat="server"></asp:Label></td>
        <td><asp:Label ID="lblModul28" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul29" runat="server"></asp:Label></td>
        <td><asp:Label ID="lblModul29" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul30" runat="server"></asp:Label></td>
        <td><asp:Label ID="lblModul30" runat="server"></asp:Label></td>
    </tr>
    <tr>
       <td><asp:Label ID="lblKodModul31" runat="server"></asp:Label></td>
       <td><asp:Label ID="lblModul31" runat="server"></asp:Label></td>
    </tr>
    <tr> 
       <td><asp:Label ID="lblKodModul32" runat="server"></asp:Label></td>
       <td><asp:Label ID="lblModul32" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblKodModul33" runat="server"></asp:Label></td>
        <td><asp:Label ID="lblModul33" runat="server"></asp:Label></td>
    </tr>
     <tr>
        <td><asp:Label ID="lblKodModul34" runat="server"></asp:Label></td>
        <td><asp:Label ID="lblModul34" runat="server"></asp:Label></td>
    </tr>
</table>
        </td>
    </tr>
</table>
<br />
<table class="fbform">
               <tr class="fbform_header">
                 <td>PNGK AKADEMIK:</td>
                 <td><asp:Label ID="lblPA" runat="server" Text=""></asp:Label></td>
                  <td style="width:15%;"></td>
                  <td>JUMLAH JAM KREDIT AKADEMIK:</td>
                 <td><asp:Label ID="lblJJKA" runat="server" Text=""></asp:Label></td>
                 
               </tr>
               <tr>
                 <td>PNGK VOKASIONAL:</td>
                 <td><asp:Label ID="lblPV" runat="server"></asp:Label></td>
                  <td style="width:15%;"></td>
                  <td>JUMLAH JAM KREDIT VOKASIONAL:</td>
                <td><asp:Label ID="lblJJKV" runat="server"></asp:Label></td>
                </tr>
               <tr>
                 <td>PNGK KESELURUHAN:</td>
                 <td><asp:Label ID="lbPK" runat="server" Text=""></asp:Label></td>
                  <td style="width:15%;"></td>
                 <td></td>
                 <td></td>
               </tr>
 </table>
       
