<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="kelas.view.ascx.vb" Inherits="apkv_v2_user.kelas_view" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Carian Dan Penyelenggaraan >> Kelas >> Paparan Maklumat Kelas.</td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Paparan Maklumat Kelas.</td>
    </tr>
    <tr>
          <td style="width: 20%;">Kohort:</td>
        <td><asp:Label ID="lblTahun" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Sesi Pengambilan:</td>
        <td><asp:Label ID="lblSesi" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
         <td style="width: 20%;">Nama Nama Bidang:</td>
        <td><asp:Label ID="lblNamaKluster" runat="server"></asp:Label>
         </td>
    </tr>
    <tr>
          <td style="width: 20%;">Kod Program:</td>
        <td><asp:Label ID="lblKodKursus" runat="server"></asp:Label>
            <asp:Label ID="lblKursusID" runat="server" Visible ="false" ></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Nama Program:</td>
        <td><asp:Label ID="lblNamaKursus" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
         <td style="width: 20%;">Nama Kelas</td>
        <td><asp:Label ID="lblNamaKelas" runat="server"></asp:Label><asp:Label ID="lblKelasID" runat="server" Visible="false"></asp:Label>
        </td>
    </tr>
     <tr>
        <td colspan="2"></td>
         </tr> 
     <tr>
        <td colspan="2"><asp:Button ID="btnUpdate" runat="server" Text="Kemaskini" CssClass="fbbutton" />
        </td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Senarai Modul.</td>
    </tr>
   <tr>
        <td colspan="2">
            <asp:GridView ID="datRespondent" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="ModulID"
                Width="100%" PageSize="25" CssClass="gridview_footer">
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <Columns>
                    <asp:TemplateField HeaderText="#">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <ItemStyle VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Semester">
                        <ItemTemplate>
                            <asp:Label ID="Semester" runat="server" Text='<%# Bind("Semester")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kod Kursus">
                        <ItemTemplate>
                            <asp:Label ID="KodModul" runat="server" Text='<%# Bind("KodModul")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nama Kursus">
                        <ItemTemplate>
                            <asp:Label ID="NamaModul" runat="server" Text='<%# Bind("NamaModul")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Jam Kredit">
                        <ItemTemplate>
                            <asp:Label ID="JamKredit" runat="server" Text='<%# Bind("JamKredit")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nama Pensyarah">
                        <ItemTemplate>
                            <asp:Label ID="Nama" runat="server" Text='<%# Bind("Nama")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" Font-Underline="true" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" CssClass="cssPager" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" VerticalAlign="Middle"
                    HorizontalAlign="Left" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
        </td>
    </tr>
      
</table>
<br />
<div class="info" id="divMsg" runat="server">
 <asp:Label ID="lblKolejID" runat="server" Text="" Visible="false"></asp:Label>
  <asp:Label ID="lblMsg" runat="server" Text="Mesej..."></asp:Label>
</div>