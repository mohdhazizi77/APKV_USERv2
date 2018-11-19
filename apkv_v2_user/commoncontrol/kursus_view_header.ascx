<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="kursus_view_header.ascx.vb" Inherits="apkv_v2_user.kursus_view_header" %>

<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Paparan Maklumat Program
        </td>
    </tr>
     <tr>
        <td>Kohort:</td>
         <td>
            <asp:Label ID="lblTahun" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>Kod Nama Bidang:</td>
         <td>
            <asp:Label ID="lblKodKluster" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>Nama Nama Bidang:</td>
         <td>
            <asp:Label ID="lblNamaKluster" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>Kod Program:
        </td>
         <td>
            <asp:Label ID="lblKodKursus" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>Nama Program:
        </td>
         <td>
            <asp:Label ID="lblNamaKursus" runat="server"></asp:Label>
        </td>
    </tr>
   
   
    <tr>
        <td class="fbform_sap" colspan="2">&nbsp;
        </td>
    </tr>
    </table>