<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="pelajar_view.ascx.vb"
    Inherits="apkv_v2_user.pelajar_view" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Carian Dan Penyelengaraan >> Calon >> Paparan Maklumat Calon</td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Paparan Maklumat Calon.</td>
    </tr>
    <tr><td  colspan="2"></td></tr>
     <tr>
         <td style="width: 20%;">Peringkat Pengajian:</td>
        <td><asp:Label ID="lblPengajian" runat="server"></asp:Label></td>
    </tr>
     <tr>
         <td>&nbsp;</td>
        <td>&nbsp;</td>
    </tr>
    <tr>
         <td style="width: 20%;">Kohort:</td>
         <td><asp:Label ID="lblTahun" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Sesi Pengambilan:</td>
         <td>
            <asp:Label ID="lblSesi" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
         <td style="width: 20%;">Nama Bidang:</td>
         <td>
            <asp:Label ID="lblKluster" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
         <td style="width: 20%;">Kod Program:</td>
         <td>
            <asp:Label ID="lblKodKursus" runat="server" Text=""></asp:Label>
            <asp:Label ID="lblKursusID" runat="server" Text="" Visible="false"></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Nama Program:</td>
         <td>
            <asp:Label ID="lblNamaKursus" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Nama Kelas:</td>
         <td>
            <asp:Label ID="lblNamaKelas" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
         <td></td>
    </tr>
    <tr>
         <td style="width: 20%;">Nama Calon:
        </td>
         <td>
            <asp:Label ID="lblNama" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td style="width: 20%;">Mykad:</td>
         <td>
            <asp:Label ID="lblMYKAD" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Angka Giliran
            :</td>
         <td>
            <asp:Label ID="lblAngkaGiliran" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td style="width: 20%;">Jantina
            :</td>
         <td>
            <asp:Label ID="lblJantina" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Kaum:</td>
         <td>
            <asp:Label ID="lblKaum" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td style="width: 20%;">Agama:</td>
         <td>
            <asp:Label ID="lblAgama" runat="server" Text=""></asp:Label>
        </td>
    </tr>
     <tr>
          <td style="width: 20%;">Emel:</td>
         <td>
            <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label>
        </td>
    </tr>
     <tr>
         <td>&nbsp;</td>
         <td>
             &nbsp;</td>
    </tr>
    <tr>
          <td style="width: 20%;">Status Calon:</td>
         <td>
            <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
          <td style="width: 20%;">Jenis Calon:</td>
         <td>
            <asp:Label ID="lblJenisCalon" runat="server" Text=""></asp:Label>
        </td>
    </tr>

    <tr>
          <td style="width: 20%;">&nbsp;</td>
         <td>
             &nbsp;</td>
    </tr>

    <tr>
          <td style="width: 20%;">Catatan:</td>
         <td>
            <asp:Label ID="lblCatatan" runat="server" Width="480px" Height="117px"></asp:Label>
        </td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td>Senarai Program.</td>
    </tr>
    <tr>
        <td>
            <asp:GridView ID="datRespondent" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="ModulID"
                Width="100%" PageSize="25" CssClass="gridview_footer">
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <Columns>
                    <asp:TemplateField HeaderText="#">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle VerticalAlign="Middle" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Semester">
                        <ItemTemplate>
                            <asp:Label ID="Semester" runat="server" Text='<%# Bind("Semester")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kod Kursus">
                        <ItemTemplate>
                            <asp:Label ID="KodModul" runat="server" Text='<%# Bind("KodModul")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nama Kursus">
                        <ItemTemplate>
                            <asp:Label ID="NamaModul" runat="server" Text='<%# Bind("NamaModul")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Jam Kredit">
                        <ItemTemplate>
                            <asp:Label ID="JamKredit" runat="server" Text='<%# Bind("JamKredit")%>'></asp:Label>
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
     <tr>
        <td colspan="2"></td>
     </tr>
    <tr>
        <td colspan="2">
            <asp:Button ID="btnExecute" runat="server" Text="Kemaskini" CssClass="fbbutton" />&nbsp;
        </td>
    </tr>
</table>
<br />
<div class="info" id="divMsg" runat="server">           
    <asp:Label ID="lblKolejID" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblMsg" runat="server" Text="Mesej..."></asp:Label>
</div>
