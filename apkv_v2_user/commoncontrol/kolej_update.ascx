<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="kolej_update.ascx.vb"
    Inherits="apkv_v2_user.kolej_update" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Carian Dan Penyelenggaraan >> Kolej >> Kemaskini Maklumat Kolej.</td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">
            Kemaskini Maklumat Kolej
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Jenis Kolej:</td>
         <td> <asp:DropDownList ID="ddlJenis" runat="server" AutoPostBack="false" Width="350px" Enabled="false">
            </asp:DropDownList>
            </td>
    </tr>
    <tr>
          <td style="width: 20%;">Kod Kolej:</td>
         <td>
            <asp:TextBox ID="txtKod" runat="server" Width="350px" MaxLength="50" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
          <td style="width: 20%;">Nama Kolej:</td>
         <td>
            <asp:TextBox ID="txtNama" runat="server" Width="350px" MaxLength="250" Enabled="false"></asp:TextBox></td>
    </tr>
    <tr>
          <td style="width: 20%;">Telefon:</td>
         <td>
            <asp:TextBox ID="txtTel" runat="server" Width="350px" MaxLength="150" Enabled="false"></asp:TextBox>*
        </td>
    </tr>
    <tr>
          <td style="width: 20%;"> Fax:</td>
         <td>
            <asp:TextBox ID="txtFax" runat="server" Width="350px" MaxLength="150" Enabled="false"></asp:TextBox>
            *</td>
    </tr>
    <tr>
          <td style="width: 20%;"> Emel:</td>
         <td>
            <asp:TextBox ID="txtEmail" runat="server" Width="350px" MaxLength="150" Enabled="false"></asp:TextBox>*
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Alamat:</td>
         <td>
            <asp:TextBox ID="txtAlamat1" runat="server" Width="350px" MaxLength="250" Enabled="false"></asp:TextBox>*
        </td>
    </tr>
    <tr>
         <td>
            
        </td>
         <td>
            <asp:TextBox ID="txtAlamat2" runat="server" Width="350px" MaxLength="250" Enabled="false"></asp:TextBox>*
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Poskod:</td>
         <td>
            <asp:TextBox ID="txtPoskod" runat="server" Width="350px" MaxLength="250" Enabled="false"></asp:TextBox>*
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Bandar:</td>
         <td>
            <asp:TextBox ID="txtBandar" runat="server" Width="350px" MaxLength="250" Enabled="false"></asp:TextBox>*
        </td>
    </tr>
    <tr>
          <td style="width: 20%;"> Negeri:</td>
         <td>
            <asp:DropDownList ID="ddlNegeri" runat="server" AutoPostBack="false" Width="350px" Enabled="false">
            </asp:DropDownList>
            *
        </td>
    </tr>
     <tr>
        <td colspan="2">
        </td>
    </tr>
    <tr class="fbform_header">
        <td colspan="2">Maklumat Lain
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Nama Pengarah:</td>
         <td>
            <asp:TextBox ID="txtNamaPengarah" runat="server" Width="350px" MaxLength="150"></asp:TextBox>
        </td>
    </tr>
     <tr>
          <td style="width: 20%;">Jawatan:</td>
         <td>
            <asp:TextBox ID="txtJawatanPengarah" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Gred:</td>
         <td>
            <asp:TextBox ID="txtGredPengarah" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Emel:</td>
         <td>
            <asp:TextBox ID="txtEmailPengarah" runat="server" Width="350px" MaxLength="150"></asp:TextBox>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Telefon Bimbit:</td>
         <td>
            <asp:TextBox ID="txtMobilePengarah" runat="server" Width="350px" MaxLength="150"></asp:TextBox>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Telefon:</td>
         <td>
            <asp:TextBox ID="txtTelPengarah" runat="server" Width="350px" MaxLength="150"></asp:TextBox>
        </td>
    </tr>
    <tr>
         <td>
            &nbsp;</td>
         <td>
            &nbsp;</td>
    </tr>
    <tr>
          <td style="width: 20%;">Nama KJPP:
        </td>
         <td>
            <asp:TextBox ID="txtNamaKJPP" runat="server" Width="350px" MaxLength="150"></asp:TextBox>
        </td>
    </tr>
     <tr>
          <td style="width: 20%;">Jawatan:</td>
         <td>
            <asp:TextBox ID="txtJawatanKJPP" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Gred:</td>
         <td>
            <asp:TextBox ID="txtGredKJPP" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Emel:</td>
         <td>
            <asp:TextBox ID="txtEmailKJPP" runat="server" Width="350px" MaxLength="150"></asp:TextBox>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;"> Telefon Bimbit:</td>
         <td>
            <asp:TextBox ID="txtMobileKJPP" runat="server" Width="350px" MaxLength="150"></asp:TextBox>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;"> Telefon:</td>
        <td>
            <asp:TextBox ID="txtTelKJPP" runat="server" Width="350px" MaxLength="150"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan ="2"></td>
    </tr>
     <tr>
           <td style="width: 20%;">Nama SUP:</td>
         <td>
            <asp:TextBox ID="txtNamaSUP" runat="server" Width="350px" MaxLength="150"></asp:TextBox>
        </td>
    </tr>
     <tr>
          <td style="width: 20%;">Jawatan:</td>
         <td>
            <asp:TextBox ID="txtJawatanSUP" runat="server" Width="350px" MaxLength="150"></asp:TextBox>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Gred:</td>
         <td>
            <asp:TextBox ID="txtGredSUP" runat="server" Width="350px" MaxLength="150"></asp:TextBox>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Emel:</td>
         <td>
            <asp:TextBox ID="txtEmailSUP" runat="server" Width="350px" MaxLength="150"></asp:TextBox>
        </td>
    </tr>
    <tr>
           <td style="width: 20%;">Telefon Bimbit:</td>
         <td>
            <asp:TextBox ID="txtMobileSUP" runat="server" Width="350px" MaxLength="150"></asp:TextBox>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Telefon:</td>
         <td>
            <asp:TextBox ID="txtTelSUP" runat="server" Width="350px" MaxLength="150"></asp:TextBox>
        </td>
    </tr>
    <tr><td colspan="2"></td></tr>
    <tr>
        <td colspan="2">
            <asp:Button ID="btnUpdate" runat="server" Text="Simpan" CssClass="fbbutton" />
        </td>
    </tr>
</table>
<div class="info" id="divMsg" runat="server">
    <asp:Label ID="lblMsg" runat="server" Text="Mesej..."></asp:Label></div>
<asp:Label ID="lblKod" runat="server" Text="" Visible="false"></asp:Label>