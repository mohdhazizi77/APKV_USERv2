<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="notifikasi_PAV.ascx.vb" Inherits="apkv_v2_user.notifikasi_PAV" %>
<%-- PA Vokasional --%>
<table style ="width :100%;padding : 5px;font-weight :bold;background :#990000;color:white  ">
    <tr >
        <td colspan ="4">Notifikasi Kemasukan Markah
            : <a href ="notifikasi.PBA.aspx" style ="color:white;text-decoration :underline">PB Akademik</a>
            | <a href ="notifikasi.PAA.aspx" style ="color:white ;text-decoration :underline">PA Akademik</a> 
            | <a href ="notifikasi.kemasukanMarkah.aspx" style ="color:white ;text-decoration :underline">PB Vokasional</a>
            | <span style ="color :yellow">PA Vokasional</span></td>
    </tr>
</table>
<br />

<asp:Panel ID ="pnl" runat ="server"  ScrollBars ="Vertical" Height ="300">
<table style ='width :100%; border:solid 1px grey;border-Spacing:initial;Padding:2px'>
    <tr class="fbform_header" >
      
        <td style ="border-Spacing:initial;text-align :center ">Kohort</td>
        <td style ="border-Spacing:initial;text-align :center ">Semester</td>
        <td style ="border-Spacing:initial;text-align :center ">Sesi</td>
        <td style ="border-Spacing:initial">Kod Kursus</td>
    </tr>
    
       <span id="tblContent" runat="server"></span>
    
</table>
</asp:Panel>
<br />

<div class="info" id="divMsg" runat="server">
  <asp:Label ID="lblKolejID" runat="server" Text="" Visible="false"></asp:Label>
  <asp:Label ID="lblMsg" runat="server" Text="System message..."></asp:Label>
</div>