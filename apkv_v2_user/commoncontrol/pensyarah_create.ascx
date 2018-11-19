<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="pensyarah_create.ascx.vb" Inherits="apkv_v2_user.pensyarah_create" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Pendaftaran >> Pensyarah >> Pendaftaran Pensyarah</td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">
            Pendaftaran Pensyarah</td>
    </tr>
     <tr>
         <td style="width: 20%;">
            Nama Pensyarah:</td>
         <td>
            <asp:TextBox ID="txtNama" runat="server" Width="350px" MaxLength="50"></asp:TextBox>*
        </td>
    </tr>
    <tr>
        <td style="width: 20%;">
            Mykad:</td>
         <td>
            <asp:TextBox ID="txtMYKAD" runat="server" Width="350px" MaxLength="250"></asp:TextBox>*
        </td>
    </tr>
    <tr>
        <td style="width: 20%;">
            Jawatan:</td>
         <td>
            <asp:TextBox ID="txtJawatan" runat="server" Width="350px" MaxLength="250"></asp:TextBox>*
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">
            Gred:</td>
         <td>
            <asp:TextBox ID="txtGred" runat="server" Width="350px" MaxLength="150"></asp:TextBox>*
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">
            Telefon:</td>
         <td>
            <asp:TextBox ID="txtTel" runat="server" Width="350px" MaxLength="150"></asp:TextBox>*
        </td>
    </tr>
    <tr>
         <td style="width: 20%;">Emel:</td>
         <td><asp:TextBox ID="txtEmail" runat="server" Width="350px" MaxLength="150"></asp:TextBox>*</td>
    </tr>
     <tr>
     <td style="width: 20%;">Jantina:</td>
      <td><asp:CheckBoxList ID="chkJantina" runat="server" Width="349px" RepeatDirection="Horizontal">
             <asp:ListItem>LELAKI</asp:ListItem>
             <asp:ListItem>PEREMPUAN </asp:ListItem>
             </asp:CheckBoxList>  
     </td> 
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
             <asp:ListItem>BUKAN ISLAM</asp:ListItem>
             </asp:CheckBoxList>
    </tr>
    <tr>
         <td colspan="2"></td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
    <td colspan="2"><asp:Button ID="btnCreate" runat="server" Text="Simpan" CssClass="fbbutton" />&nbsp;</td>
    </tr>
</table>
<br />
<div class="info" id="divMsg" runat="server">
<asp:Label ID="lblMsg" runat="server" Text="Mesej..."></asp:Label></div>
<asp:Label ID="lblKolejID" runat="server" Text="" Visible="false"></asp:Label>


