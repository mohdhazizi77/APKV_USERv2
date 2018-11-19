<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="matapelajaran_create.ascx.vb" Inherits="apkv_v2_user.matapelajaran_create" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Pendaftaran MataPelajaran</td>
    </tr>
    <tr>
        <td>Kohort:</td>
         <td>
            <asp:DropDownList ID="ddlTahun" runat="server" AutoPostBack="false" Width="350px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
       <td>Semester:</td>
         <td>
            <asp:DropDownList ID="ddlSemester" runat="server" AutoPostBack="false" Width="350px">
            </asp:DropDownList>
            *</td>
    </tr>
    <tr>
       <td>Sesi Pengambilan:</td>
         <td>
            <asp:DropDownList ID="ddlSesi" runat="server" AutoPostBack="false" Width="350px">
            </asp:DropDownList>
            *</td>
    </tr>
    <tr>
        <td>Kod MataPelajaran:</td>
         <td>
            <asp:TextBox ID="txtKodMataPelajaran" runat="server" Width="350px" MaxLength="50"></asp:TextBox>*
        </td>
    </tr>
    <tr>
        <td>Nama MataPelajaran:</td>
         <td>
            <asp:TextBox ID="txtNamaMataPelajaran" runat="server" Width="350px" MaxLength="250"></asp:TextBox>*
        </td>
    </tr>
    <tr>
        <td>Jam Kredit:</td>
         <td>
            <asp:TextBox ID="txtJamKredit" runat="server" Width="350px" MaxLength="10"></asp:TextBox>
            *</td>
    </tr>
    <tr>
        <td colspan="2">&nbsp;<asp:Button ID="btnCreate" runat="server" Text="Daftar MataPelajaran Baru" CssClass="fbbutton" /><asp:LinkButton
                ID="lnkList" runat="server">|Carian MataPelajaran</asp:LinkButton>
        </td>
    </tr>
    </table>
<div class="info" id="divMsg" runat="server">
    <asp:Label ID="lblMsg" runat="server" Text="Mesej..."></asp:Label></div>
