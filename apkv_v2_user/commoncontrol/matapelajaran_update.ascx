<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="matapelajaran_update.ascx.vb" Inherits="apkv_v2_user.matapelajaran_update" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Paparan Maklumat MataPelajaran</td>
    </tr>
   <tr>
        <td>Tahun:</td>
         <td>
            <asp:Label ID="lblTahun" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>Nama Kluster:</td>
         <td>
            <asp:Label ID="lblNamaKluster" runat="server"></asp:Label>
            </td>
    </tr>
    <tr>
        <td>Kod Kursus:</td>
         <td>
            <asp:Label ID="lblKodKursus" runat="server"></asp:Label>
            </td>
    </tr>
    <tr>
        <td>Nama Kursus:</td>
         <td>
            <asp:Label ID="lblNamaKursus" runat="server"></asp:Label>
            </td>
    </tr>
    <tr>
        <td></td>
         <td></td>
    </tr>
    <tr>
        <td>Semester:</td>
         <td>
            <asp:DropDownList ID="ddlSemester" runat="server" AutoPostBack="false" Width="350px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>Sesi:</td>
         <td>
            <asp:DropDownList ID="ddlSesi" runat="server" AutoPostBack="false" Width="350px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>Kod MataPelajaran:</td>
         <td>
            <asp:TextBox ID="txtKodMataPelajaran" runat="server" Width="350px" MaxLength="250"></asp:TextBox>
            *
        </td>
    </tr>
    <tr>
        <td>Nama MataPelajaran:</td>
         <td>
            <asp:TextBox ID="txtNamaMataPelajaran" runat="server" Width="350px" MaxLength="250"></asp:TextBox>
            *
        </td>
    </tr>
    <tr>
        <td>Jam Kredit:</td>
         <td>
            <asp:TextBox ID="txtJamKredit" runat="server" Width="350px" MaxLength="250"></asp:TextBox>
            *</td>
    </tr>
    <tr>
        <td colspan="2">&nbsp;
        </td>
    </tr>
    <tr>
        <td class="fbform_sap" colspan="2">&nbsp;
        </td>
    </tr>
    <tr>
         <td>
            &nbsp;
        </td>
        <td style="text-align: left;">
            <asp:Button ID="btnUpdate" runat="server" Text="Kemaskini" CssClass="fbbutton" />&nbsp;<asp:Button
                ID="btnDelete" runat="server" Text="Hapuskan" CssClass="fbbutton" />&nbsp;<asp:LinkButton
                    ID="lnkList" runat="server">|Carian MataPelajaran</asp:LinkButton>
        </td>
    </tr>
</table>
<div class="info" id="divMsg" runat="server">
    <asp:Label ID="lblMsg" runat="server" Text="Mesej..."></asp:Label></div>
<asp:Label ID="lblKodMataPelajaran" runat="server" Text="" Visible="false"></asp:Label>
