<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="pensyarah_update.ascx.vb" Inherits="apkv_v2_user.pensyarah_update" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Carian Dan Penyelengaraan >> Pensyarah >> Kemaskini Pensyarah</td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">
            Kemaskini Pensyarah
        </td>
    </tr>
    <tr>
         <td style="width: 20%;">Nama Pensyarah:</td>
        <td>
            <asp:TextBox ID="txtNama" runat="server" Width="350px" MaxLength="50"></asp:TextBox>*
        </td>
    </tr>
    <tr>
         <td style="width: 20%;">Mykad:</td>
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
         <td style="width: 20%;">Telefon:</td>
        <td>
            <asp:TextBox ID="txtTel" runat="server" Width="350px" MaxLength="150"></asp:TextBox>*
        </td>
    </tr>
    <tr>
         <td style="width: 20%;">Emel:</td>
        <td> <asp:TextBox ID="txtEmail" runat="server" Width="350px" MaxLength="150"></asp:TextBox>*</td>
    </tr>
    <tr>
         <td style="width: 20%;">Jantina:</td>
        <td><asp:DropDownList ID="ddlJantina" runat="server" AutoPostBack="false" Width="350px"></asp:DropDownList>*</td>
    </tr>
    <tr>
         <td style="width: 20%;">Kaum:</td>
        <td><asp:DropDownList ID="ddlKaum" runat="server" AutoPostBack="false" Width="350px"></asp:DropDownList>*</td>
    </tr>
    <tr>
         <td style="width: 20%;">Agama:</td>
        <td>
            <asp:DropDownList ID="ddlAgama" runat="server" AutoPostBack="false" Width="350px">
            </asp:DropDownList>
            *</td>
    </tr>
    <tr>
         <td style="width: 20%;">Status:</td>
        <td>
            &nbsp;<asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="false" Width="350px"></asp:DropDownList>*
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
        <td class="fbform_sap">
            &nbsp;
        </td>
    </tr>
    <tr>
       <td colspan="2">
            <asp:Button ID="btnUpdate" runat="server" Text="Simpan" CssClass="fbbutton" />&nbsp;
            <asp:Button ID="btnDelete" runat="server" Text="Hapuskan" CssClass="fbbutton" />
        </td>
    </tr>
</table>
<br />
<div class="info" id="divMsg" runat="server">
<asp:Label ID="lblMsg" runat="server" Text="Mesej..."></asp:Label></div>
<asp:Label ID="lblPensyarahID" runat="server" Text="" Visible="false"></asp:Label>
<asp:Label ID="lblKolejID" runat="server" Text="" Visible="false"></asp:Label>
