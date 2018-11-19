<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="pelajar_import.ascx.vb"
    Inherits="apkv_v2_user.pelajar_import" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Kemasukan Calon
        </td>
    </tr>
    <tr>
         <td colspan ="2">MuatNaik Format Fail Excel:
         <asp:Button ID="btnFile" runat="server" Text="Excel" CssClass="fbbutton" Height="25px" Width="116px" /></td>
    </tr>
    <tr>
         <td>Pilih Fail Excel:
        </td>
         <td>
            <asp:FileUpload ID="FlUploadcsv" runat="server" />&nbsp;
            <asp:RegularExpressionValidator ID="regexValidator" runat="server" ErrorMessage="Only XLSX file are allowed"
                ValidationExpression="(.*\.([Xx][Ll][Ss][Xx])$)" ControlToValidate="FlUploadcsv"></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button ID="btnUpload" runat="server" Text="Muatnaik " CssClass="fbbutton" Style="height: 26px" />
        </td>
    </tr>
    </table>

<table class="fbform">
  <tr>
         <td><asp:Label ID="lblUploadCode" runat="server" Text="" Visible="false"></asp:Label><br />
            <asp:Label ID="lblKolejRecordID" runat="server" Text="" Visible ="false"></asp:Label>
        </td>
    </tr>
</table>
<div class="info" id="divMsg" runat="server">
    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblKohort" runat="server" Text="" Visible ="false" ></asp:Label>
        <asp:Label ID="lblSesi" runat="server" Text="" Visible ="false" ></asp:Label>
        <asp:Label ID="lblSemester" runat="server" Text="" Visible ="false" ></asp:Label>

</div>
