<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="pelajar_create.ascx.vb"
    Inherits="apkv_v2_user.pelajar_create" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Pendaftaran >> Calon >> Pendaftaran Calon Baru </td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Pendaftaran Calon Baru</td>
    </tr>
    <tr>
         <td colspan="2">Peringkat Pengajian: Pra Diploma</td>
    </tr>
    <tr><td colspan="2"></td></tr>
    <tr>
        <td style="width: 20%;">Kohort:</td>
         <td>
            <asp:DropDownList ID="ddlTahun" runat="server" AutoPostBack="false" Width="350px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Semester:</td>
         <td>
             1</td>
    </tr>
    <tr>
         <td style="width: 20%;">Sesi Pengambilan:</td>
         <td><asp:CheckBoxList ID="chkSesi" runat="server" Width="349px" RepeatDirection="Horizontal" AutoPostBack="true">
             <asp:ListItem Enabled="False">1</asp:ListItem>
             <asp:ListItem Enabled="False">2</asp:ListItem>
             </asp:CheckBoxList>

    </tr>
     <tr>
         <td style="width: 20%;">Nama Bidang:</td>
        <td><asp:DropDownList ID="ddlKluster" runat="server" AutoPostBack="true" Width="350px"></asp:DropDownList></td> 
    </tr>
   <tr>
         <td style="width: 20%;">Kod Program:</td>
         <td>
            <asp:DropDownList ID="ddlKodKursus" runat="server" AutoPostBack="true" Width="350px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Kelas:</td>
         <td>
            <asp:DropDownList ID="ddlNamaKelas" runat="server" AutoPostBack="false" Width="350px">
            </asp:DropDownList>
         </td>
    </tr>
    <tr>
         <td>&nbsp;</td>
         <td>
             &nbsp;</td>
    </tr>
    <tr>
          <td style="width: 20%;">Nama Calon: </td>
         <td>
            <asp:TextBox ID="txtNama" runat="server" Width="350px" MaxLength="250"></asp:TextBox>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Mykad:</td>
         <td>
            <asp:TextBox ID="txtMYKAD" runat="server" Width="350px" MaxLength="50"></asp:TextBox>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Jantina:</td>
         <td><asp:CheckBoxList ID="chkJantina" runat="server" Width="349px" RepeatDirection="Horizontal">
             <asp:ListItem>LELAKI</asp:ListItem>
             <asp:ListItem>PEREMPUAN</asp:ListItem>
             </asp:CheckBoxList>   
    </tr>
    <tr>
          <td style="width: 20%;">Kaum:</td>
         <td>
            <asp:DropDownList ID="ddlKaum" runat="server" AutoPostBack="false" Width="350px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
         <td style="width: 20%;">Agama:</td>
         <td><asp:CheckBoxList ID="chkAgama" runat="server" Width="349px" RepeatDirection="Horizontal">
             <asp:ListItem>ISLAM</asp:ListItem>
             <asp:ListItem>LAIN-LAIN</asp:ListItem>
             </asp:CheckBoxList>
    </tr>
    <tr>
         <td style="width: 20%;">Emel:</td>
         <td>
            <asp:TextBox ID="txtEmail" runat="server" Width="350px" MaxLength="250"></asp:TextBox>
        </td>
    </tr>

    <tr>
         <td style="width: 20%;">Catatan
            :</td>
         <td>
            <asp:TextBox ID="txtCatatan" runat="server" Width="350px" MaxLength="250" Height="117px"></asp:TextBox>
        </td>
    </tr>
     <tr>
        <td colspan="2"></td>
    </tr>
    <tr>
        <td colspan="2"><asp:Button ID="btnCreate" runat="server" Text="Simpan" CssClass="fbbutton" />&nbsp;</td>
    </tr>
</table>
<br/>
<div class="info" id="divMsg" runat="server">           
    <asp:Label ID="lblKolejID" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="lblMsg" runat="server" Text="Mesej..."></asp:Label>
</div>
