<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="pentaksir_bmsetara_daftar.ascx.vb" Inherits="apkv_v2_user.pentaksir_bmsetara_daftar1" %>

<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Pentaksir BM Setara > Pendaftaran </td>
    </tr>
</table>

<br />

<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Pendaftaran Pentaksir BM Setara</td>
    </tr>
    <tr>
        <td style="width: 20%;">Tahun Peperiksaan:</td>
        <td>
            <asp:DropDownList ID="ddlTahun" runat="server" AutoPostBack="false" Width="350px"></asp:DropDownList>
        </td>
    </tr>

    <tr>
        <td style="width: 20%;">Nama:</td>
        <td>
            <asp:TextBox ID="txtNama" runat="server" Width="349px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 20%;">MYKAD:</td>
        <td>
            <asp:TextBox ID="txtMYKAD" runat="server" Width="349px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 20%;">Email:</td>
        <td>
            <asp:TextBox ID="txtEmail" runat="server" Width="349px"></asp:TextBox>
        </td>
    </tr>

    <tr>
        <td></td>
    </tr>

    <tr>
        <td></td>
        <td colspan="2">
            <asp:Button ID="btnDaftar" runat="server" Text="Daftar" CssClass="fbbutton" />&nbsp;</td>
    </tr>
</table>

<br />

<div class="info" id="divMsg2" runat="server">

    <asp:Label ID="lblMsg2" runat="server" Text=""></asp:Label>

</div>

<br />

<table class="fbform">
    <tr class="fbform_header">
        <td>Senarai Pentaksir yang telah didaftarkan
        </td>
    </tr>
    <tr>
        <td>
            <asp:GridView ID="datRespondent" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="UserID"
                Width="100%" PageSize="500" CssClass="gridview_footer">
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
                            <asp:Label ID="Nama" runat="server" Text='<%# Bind("Nama")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mykad">
                        <ItemTemplate>
                            <asp:Label ID="Mykad" runat="server" Text='<%# Bind("Mykad")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label ID="Email" runat="server" Text='<%# Bind("LoginID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:CommandField SelectText="[PADAM]" ShowSelectButton="True" HeaderText="PADAM" />

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

    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>

</div>
