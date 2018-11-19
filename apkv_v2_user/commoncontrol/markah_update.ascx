<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="markah_update.ascx.vb"
    Inherits="apkv_v2_user.markah_update" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">
            Kemaskini Markah Pelajar
        </td>
    </tr>
    <tr>
        <td style="width:15%;">
            Kolej:
        </td>
         <td>
            <asp:Label ID="lblNama" runat="server" Text="" Visible="true"></asp:Label>
        </td>
    </tr>
    <tr>
         <td style="width:15%;">
            Tahun:
        </td>
         <td>
            <asp:Label ID="lblTahun" runat="server" Text="" Visible="true"></asp:Label>
        </td>
    </tr>
    <tr>
         <td style="width:15%;">
            Semester:</td>
         <td>
            <asp:Label ID="lblSemester" runat="server" Text="" Visible="true"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="width:15%;">
            Sesi:</td>
         <td>
            <asp:Label ID="lblSesi" runat="server"></asp:Label>
        </td>
    </tr>
    </Table>

    <table class="fbform">
    <tr class="fbform_header">
        <td colspan="4">
        </td>
    </tr>
    <tr>
        <td class="fbform_sap" colspan="4">
            &nbsp;
        </td>
    </tr>
    <tr>
         <td style="width:15%;">
            M1_TEORI:
        </td>
         <td>
            <asp:TextBox ID="M1_TEORI" runat="server" Width="50px" MaxLength="5" Text="12.34"></asp:TextBox>
        </td>
        <td style="width:15%;">
            M1_AMALI:
        </td>
         <td>
            <asp:TextBox ID="M1_AMALI" runat="server" Width="50px" MaxLength="5" Text="12.34"></asp:TextBox>
        </td>
    </tr>
    <tr>
         <td style="width:15%;">
            M2_TEORI:
        </td>
         <td>
            <asp:TextBox ID="M2_TEORI" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
        </td>
         <td style="width:15%;">
            M2_AMALI:
        </td>
         <td>
            <asp:TextBox ID="M2_AMALI" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
        </td>
    </tr>
    <tr>
         <td style="width:15%;">
            M3_TEORI:
        </td>
         <td>
            <asp:TextBox ID="M3_TEORI" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
        </td>
         <td style="width:15%;">
            M3_AMALI:
        </td>
         <td>
            <asp:TextBox ID="M3_AMALI" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
        </td>
    </tr>
    <tr>
         <td style="width:15%;">
            M4_TEORI:
        </td>
         <td>
            <asp:TextBox ID="M4_TEORI" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
        </td>
        <td style="width:15%;">
            M4_AMALI:
        </td>
         <td>
            <asp:TextBox ID="M4_AMALI" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width:15%;">
            M5_TEORI:
        </td>
         <td>
            <asp:TextBox ID="M5_TEORI" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
        </td>
         <td style="width:15%;">
            M5_AMALI:
        </td>
         <td>
            <asp:TextBox ID="M5_AMALI" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width:15%;">
            M6_TEORI:
        </td>
         <td>
            <asp:TextBox ID="M6_TEORI" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
        </td>
        <td style="width:15%;">
            M6_AMALI:
        </td>
         <td>
            <asp:TextBox ID="M6_AMALI" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
        </td>
    </tr>
    <tr>
         <td style="width:15%;">
            M7_TEORI:
        </td>
         <td>
            <asp:TextBox ID="M7_TEORI" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
        </td>
        <td style="width:15%;">
            M7_AMALI:
        </td>
         <td>
            <asp:TextBox ID="M7_AMALI" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
        </td>
    </tr>
    <tr>
         <td style="width:15%;">
            M8_TEORI:
        </td>
         <td>
            <asp:TextBox ID="M8_TEORI" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
        </td>
        <td style="width:15%;">
            M8_AMALI:
        </td>
         <td>
            <asp:TextBox ID="M8_AMALI" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="fbform_sap" colspan="4">
            &nbsp;
        </td>
    </tr>
    <tr>
        <td style="text-align: left;" colspan="4">
            <asp:Button ID="btnUpdate" runat="server" Text="Kemaskini" CssClass="fbbutton" />&nbsp;
            <asp:Button ID="btnDelete" runat="server" Text="Hapuskan" CssClass="fbbutton" />
            <asp:LinkButton ID="lnkList" runat="server">|Markah Pelajar</asp:LinkButton>
        </td>
    </tr>
</table>
<div class="info" id="divMsg" runat="server">
    <asp:Label ID="lblMsg" runat="server" Text="Mesej..."></asp:Label></div>
KolejID:<asp:Label ID="lblKolejRecordID" runat="server" Text="" Visible="true"></asp:Label>
