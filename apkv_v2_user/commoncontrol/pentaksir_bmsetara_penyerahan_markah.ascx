<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="pentaksir_bmsetara_penyerahan_markah.ascx.vb" Inherits="apkv_v2_user.pentaksir_bmsetara_penyerahan_markah" %>

<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Penyerahan Markah Bahasa Melayu Setara</td>
    </tr>
</table>

<br />

<table class="fbform">
    <tr>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td style="width: 200px">Mata Pelajaran / Kertas:</td>
        <td>
            <asp:Label ID="lblMP" runat="server"></asp:Label>
        </td>
        <td style="text-align: right"></td>
    </tr>

    <tr>
        <td style="width: 200px">Pusat Peperiksaan:</td>
        <td>
            <asp:Label ID="lblPP" runat="server"></asp:Label>
        </td>
        <td></td>
    </tr>

    <tr>
        <td style="width: 200px">Peranan:</td>
        <td>
            <asp:Label ID="lblPeranan" runat="server" Text="PENTAKSIR"></asp:Label>
        </td>
        <td></td>
    </tr>
    <tr>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td></td>
        <td></td>
    </tr>

</table>

<br />

<table class="fbform">
    <tr class="fbform_header">
        <td>Senarai Calon
        </td>
    </tr>
    <tr>
        <td>
            <asp:GridView ID="datRespondent" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="id"
                Width="100%" PageSize="50" CssClass="gridview_footer">
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <Columns>
                    <asp:TemplateField HeaderText="#">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemStyle VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nama">
                        <ItemTemplate>
                            <asp:Label ID="lblNama" runat="server" Text='<%# Bind("Nama")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mykad">
                        <ItemTemplate>
                            <asp:Label ID="lblMykad" runat="server" Text='<%# Bind("Mykad")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="AngkaGiliran">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label ID="AngkaGiliran" runat="server" Text='<%# Bind("AngkaGiliran")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Markah Elemen">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            [<asp:Label ID="BM4" runat="server" Width="30px" MaxLength="3" Text='<%# Bind("BM4")%>'></asp:Label>]
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Jumlah Markah">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="Jumlah" Enabled="false" runat="server" Width="30px" MaxLength="3" Text='<%# Bind("BM4_Total")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Status Hantar">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="Status" runat="server" Text='<%# Bind("StatusHantarBM4_Pentaksir")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
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
        <td></td>
    </tr>

    <tr>
        <td></td>
    </tr>

    <tr>
        <td></td>
    </tr>


    <tr>
        <td><p style="text-align:center">Dengan menekan butang <b>Hantar</b>, anda mengaku bahawa segala maklumat dan data yang dimasukkan adalah tepat dan betul.</p>
            <p style="text-align:center">Dengan ini, anda juga bersetuju dengan segala syarat yang dikenakan ke atas anda terhadap tanggungjawab yang perlu dilakukan.</p>
            <p style="text-align:center">Tiada unsur-unsur lain yang boleh mempengaruhi kesemua data yang dimasukkan.</p>
            <p style="text-align:center">Sekiranya ketidakadilan berlaku anda mesti bertanggungjawab. Perlu diingat, ia adalah <b>Rahsia</b>.</p>
        </td>
    </tr>

    <tr>
        <td style="text-align: center">
            <asp:Button ID="btnBack" runat="server" Text="Kembali" CssClass="fbbutton" Width="70px" Visible="true" />&nbsp;
            <asp:Button ID="btnSubmit" runat="server" Text="Hantar" CssClass="fbbutton" Width="70px" Visible="true" />&nbsp;
        </td>
    </tr>
    <tr>
        <td></td>
    </tr>

    <tr>
        <td></td>
    </tr>

    <tr>
        <td></td>
    </tr>

</table>

<br />

<div class="info" id="divMsg" runat="server">
    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label><br />
</div>


