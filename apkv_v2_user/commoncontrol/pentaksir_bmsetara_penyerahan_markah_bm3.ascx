<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="pentaksir_bmsetara_penyerahan_markah_bm3.ascx.vb" Inherits="apkv_v2_user.pentaksir_bmsetara_penyerahan_markah_bm3" %>

<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Penyerahan Markah (SUP)</td>
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
        <td colspan="2">
            <asp:Label ID="lblBelumSerah" runat="server" Font-Bold="true" ForeColor="Red" Text="Masih terdapat rekod penilaian calon belum diserahkan bagi pusat yang dipilih. Sila klik butang Serah untuk buat penyerahan bagi rekod tersebut."></asp:Label>
        </td>
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
    </tr>

    <tr>
        <td style="width: 200px">Pusat Peperiksaan:</td>
        <td>
            <asp:Label ID="lblPP" runat="server"></asp:Label>
        </td>
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

    <tr>
        <td colspan="2">
            <asp:Label ID="Label3" runat="server" Text="No. KP atau Angka Giliran :" Width="200px"></asp:Label>

            <asp:TextBox ID="txtCarian" runat="server"></asp:TextBox>
            <asp:Button ID="btnCari" runat="server" Text="Carian" /></td>
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
        <td></td>
        <td></td>
    </tr>

    <tr>
        <td><b>Nota:</b></td>
    </tr>
    <tr>
        <td colspan="2">1. [*] - Elemen yang telah diserahkan oleh pentaksir dan <b>mempunyai markah</b></td>
    </tr>
    <tr>
        <td colspan="2">2. [T] - Elemen yang telah diserahkan oleh pentaksir dan statusnya adalah <b>Tidak Hadir</b></td>
    </tr>
    <tr>
        <td colspan="2">3. [-] - Elemen yang masih <b>belum diserahkan</b> oleh pentaksir</td>
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
        <td>Senarai Calon Bagi Penyerahan Markah Ujian Bertutur Bahasa Melayu
        </td>
    </tr>
    <tr>
        <td>
            <asp:GridView ID="datRespondent" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="id"
                Width="100%" PageSize="500" CssClass="gridview_footer">
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <Columns>
                    <asp:TemplateField HeaderText="#">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="No. Kad Pengenalan">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="lblMykad" runat="server" Text='<%# Bind("Mykad")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="AngkaGiliran">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="AngkaGiliran" runat="server" Text='<%# Bind("AngkaGiliran")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Markah Elemen">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="lblMarkahElemen" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Status Serah">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="Status" runat="server" Text='<%# Bind("StatusSerahBM3KJPP")%>'></asp:Label>
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
        <td style="text-align:center">
            <p>Dengan menekan butang <b>Serah</b>, anda mengaku bahawa segala maklumat dan data yang dimasukkan adalah tepat dan betul.</p>
            <p>Dengan ini, anda juga bersetuju dengan segala syarat yang dikenakan ke atas anda terhadap tanggungjawab yang perlu dilakukan</p>
            <p>Tiada unsur-unsur lain yang boleh mempengaruhi kesemua data yang dimasukkan. Sekiranya ketidakadilan berlaku anda mesti bertanggungjawab.</p>
            <p>Perlu diingat, ia adalah <b>Rahsia</b></p>
        </td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <td></td>
    </tr>


    <tr>
        <td style="text-align: center">
            <asp:Button ID="btnBack" runat="server" Text="Kembali" CssClass="fbbutton" Width="70px" Visible="true" />&nbsp;
            <asp:Button ID="btnSubmit" runat="server" Text="Serah" CssClass="fbbutton" Width="70px" Visible="true" />&nbsp;
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
        <td></td>
    </tr>
</table>


<div class="info" id="divMsg" runat="server">
    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label><br />
</div>
