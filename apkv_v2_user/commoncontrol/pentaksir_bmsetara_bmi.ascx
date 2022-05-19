<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="pentaksir_bmsetara_bmi.ascx.vb" Inherits="apkv_v2_user.pentaksir_bmsetara_bmi1" %>

<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
<script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
<link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="Stylesheet" type="text/css" />

<script type="text/javascript">
    $(function () {
        $("[id$=txtTarikh]").datepicker({
            dateFormat: 'dd MM yy',
            showOn: 'button',
            buttonImageOnly: true,
            buttonImage: '/icons/calendar.gif'
        });
    });
</script>

<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Borang Markah Induk</td>
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
        <td>
            <asp:Label ID="Label3" runat="server" Text="Jenis Borang :" Width="150px"></asp:Label>
            <asp:DropDownList ID="ddlJenisBorang" runat="server" Width="500px" AutoPostBack="true"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label7" runat="server" Text="Pusat Peperiksaan :" Width="150px"></asp:Label>
            <asp:DropDownList ID="ddlPP" runat="server" Width="100px" AutoPostBack="true"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label9" runat="server" Text="Sidang :" Width="150px"></asp:Label>
            <asp:DropDownList ID="ddlSidang" runat="server" Width="100px"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label1" runat="server" Text="Bilik Ujian :" Width="150px"></asp:Label>
            <asp:DropDownList ID="ddlBilik" runat="server" Width="150px"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label10" runat="server" Text="Tarikh :" Width="150px"></asp:Label>
            <asp:TextBox ID="txtTarikh" runat="server" Width="90px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label11" runat="server" Text="Masa :" Width="150px"></asp:Label>
            <asp:DropDownList ID="ddlMasa" runat="server" Width="100px"></asp:DropDownList>
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

<br />

<table class="fbform">

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
        <td style="text-align: center">
            <asp:Button ID="btnBack2" runat="server" Text="Kembali" CssClass="fbbutton" Width="70px" Visible="true" />&nbsp;
            <asp:Button ID="btnReset2" runat="server" Text="Reset" CssClass="fbbutton" Width="70px" Visible="true" />&nbsp;
            <asp:Button ID="btnRefresh2" runat="server" Text="Refresh" CssClass="fbbutton" Width="70px" Visible="true" />&nbsp;
            <asp:Button ID="btnPrint2" runat="server" Text="Cetak" CssClass="fbbutton" Width="70px" Visible="true" />&nbsp;
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

    <tr class="fbform_header">
        <td>Senarai Calon
        </td>
    </tr>
    <tr>
        <td>
            <asp:GridView ID="datRespondent" runat="server" AutoGenerateColumns="False" AllowPaging="False"
                CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="id"
                Width="100%" PageSize="50" CssClass="gridview_footer">
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <Columns>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkPrint" runat="server" />
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
        <td style="text-align: center">
            <asp:Button ID="btnBack" runat="server" Text="Kembali" CssClass="fbbutton" Width="70px" Visible="true" />&nbsp;
            <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="fbbutton" Width="70px" Visible="true" />&nbsp;
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="fbbutton" Width="70px" Visible="true" />&nbsp;
            <asp:Button ID="btnPrint" runat="server" Text="Cetak" CssClass="fbbutton" Width="70px" Visible="true" />&nbsp;
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



