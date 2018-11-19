<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="user_change_password.ascx.vb" Inherits="apkv_v2_user.user_change_password" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Tukar KataLaluan</td>
    </tr>
    <tr>
        <td style ="width:20%">KataLaluan Semasa :</td>
        <td><asp:TextBox ID="txtOldPwd" runat="server" TextMode="Password" Width="200px" MaxLength="50"></asp:TextBox></td>
    </tr>
    <tr>
        <td>KataLaluan Baru :</td>
        <td><asp:TextBox ID="txtNewPwd" runat="server" TextMode="Password" Width="200px" MaxLength="50"></asp:TextBox></td>
         
    </tr>
    <tr>
        <td>Pengesahan KataLaluan:</td>
        <td><asp:TextBox ID="txtVerPwd" runat="server" TextMode="Password" Width="200px" MaxLength="50"></asp:TextBox></td>
    </tr>
    <tr>
        <td colspan ="2"></td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="btnPwdUpdate" runat="server" Text="Simpan" CssClass="fbbutton" />&nbsp;
        </td>
    </tr>
</table>
<br />
<div class="info" id="div" runat="server">
  <asp:Label ID="lblMsg" runat="server" Text="Mesej..."></asp:Label>
</div>
