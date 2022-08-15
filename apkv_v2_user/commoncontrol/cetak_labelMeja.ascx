<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="cetak_labelMeja.ascx.vb" Inherits="apkv_v2_user.cetak_labelMeja" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Cetakan Label Meja Pelajar</td>
    </tr>
    <tr>
        <td style="width: 20%;">Kohort:</td>
        <td>
            <asp:DropDownList ID="ddlTahun" runat="server" AutoPostBack="true" Width="200px">
            </asp:DropDownList>
        </td>
    </tr>

    <tr>
        <td>Semester:</td>
        <td>
            <asp:DropDownList ID="ddlSemester" runat="server" AutoPostBack="false" Width="200px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>Sesi Pengambilan:</td>
        <td>
            <asp:CheckBoxList ID="chkSesi" runat="server" AutoPostBack="true" Width="349px" RepeatDirection="Horizontal">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
            </asp:CheckBoxList></td>
    </tr>
    <tr>
        <td>Kod Program:</td>
        <td>
            <asp:DropDownList ID="ddlKodKursus" runat="server" AutoPostBack="true" Width="350px"></asp:DropDownList></td>
    </tr>
    <tr>
        <td>Nama Kelas:</td>
        <td>
            <asp:DropDownList ID="ddlNamaKelas" runat="server" AutoPostBack="false" Width="350px"></asp:DropDownList></td>
    </tr>
    <tr>
        <td>Nama Calon:</td>
        <td>
            <asp:TextBox ID="txtNama" runat="server" Width="350px" MaxLength="200"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>Mykad:</td>
        <td>
            <asp:TextBox ID="txtMYKAD" runat="server" Width="350px" MaxLength="200"></asp:TextBox>
        </td>
    </tr>

    <tr>
        <td colspan="2">
            <asp:Button ID="btnSearch" runat="server" Text="Cari " CssClass="fbbutton" /></td>
    </tr>
</table>
<div class="info" id="divMsgTop" runat="server">
    <asp:Label ID="lblMsgTop" runat="server" Text=""></asp:Label>
</div>
<table class="fbform">
    <tr class="fbform_header">
        <td>Senarai Calon
        </td>
    </tr>
    <tr>
        <td>
            <asp:GridView ID="datRespondent" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="PelajarID"
                Width="100%" PageSize="100" CssClass="gridview_footer" EnableModelValidation="True">
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <Columns>
                    <asp:TemplateField HeaderText="#">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <ItemStyle VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kohort">
                        <ItemTemplate>
                            <asp:Label ID="Tahun" runat="server" Text='<%# Bind("Tahun")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Semester">
                        <ItemTemplate>
                            <asp:Label ID="Semester" runat="server" Text='<%# Bind("Semester")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sesi">
                        <ItemTemplate>
                            <asp:Label ID="Sesi" runat="server" Text='<%# Bind("Sesi") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nama">
                        <ItemTemplate>
                            <asp:Label ID="Nama" runat="server" Text='<%# Bind("Nama") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mykad">
                        <ItemTemplate>
                            <asp:Label ID="Mykad" runat="server" Text='<%# Bind("Mykad")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="AngkaGiliran">
                        <ItemTemplate>
                            <asp:Label ID="AngkaGiliran" runat="server" Text='<%# Bind("AngkaGiliran")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kod Program">
                        <ItemTemplate>
                            <asp:Label ID="KodKursus" runat="server" Text='<%# Bind("KodKursus")%>'></asp:Label>
                            <headerstyle horizontalalign="Left" />
                            <itemstyle verticalalign="Middle" />
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Jantina">
                        <ItemTemplate>
                            <asp:Label ID="Jantina" runat="server" Text='<%# Bind("Jantina")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kaum">
                        <ItemTemplate>
                            <asp:Label ID="Kaum" runat="server" Text='<%# Bind("Kaum")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Agama">
                        <ItemTemplate>
                            <asp:Label ID="Agama" runat="server" Text='<%# Bind("Agama")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" OnCheckedChanged="CheckUncheckAll" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle VerticalAlign="Middle" />
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
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="3">Tetapan Semasa Peperiksaan</td>
    </tr>
    <tr style="text-align: center;">
        <td style="width: 10%;">Tahun :<asp:DropDownList ID="ddlTahun_Semasa" runat="server" AutoPostBack="false" Width="53px" Height="17px"></asp:DropDownList>
            &nbsp;
               Sesi :
            <asp:DropDownList ID="ddlSesi_Semasa" runat="server" AutoPostBack="false">
                <asp:ListItem>01</asp:ListItem>
                <asp:ListItem>02</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="text-align: center;" colspan="3">
            <asp:Button ID="btnPrint" runat="server" Text="Cetak Slip Pengesahan" CssClass="fbbutton" />&nbsp;<asp:HyperLink ID="hyPDF" runat="server" Target="_blank"
                Visible="false">Klik disini untuk muat turun.</asp:HyperLink>
        </td>
    </tr>
</table>

<br />
<div class="info" id="divMsg" runat="server">
    <asp:Label ID="lblKolejID" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
</div>
