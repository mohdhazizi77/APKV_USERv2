<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="matapelajaran.pensyarah.create.ascx.vb" Inherits="apkv_v2_user.matapelajaran_pensyarah_create" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Paparan Maklumat MataPelajaran</td>
    </tr>
    <tr>
         <td>Kohort:</td>
         <td>
            <asp:Label ID="lblTahun" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
         <td>Semester:</td>
         <td>
            <asp:Label ID="lblSemester" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
         <td>Sesi Pengambilan:</td>
         <td>
            <asp:Label ID="lblSesi" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
         <td>Nama Bidang: <td>
            <asp:Label ID="lblNamaKluster" runat="server"></asp:Label>
         </td>
    </tr>
    <tr>
         <td>Kod Program:</td>
         <td>
            <asp:Label ID="lblKodKursus" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
         <td>Nama Program:</td>
         <td>
            <asp:Label ID="lblNamaKursus" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
         <td>&nbsp;</td>
         <td>
            &nbsp;</td>
    </tr>
    <tr>
         <td>Kod MataPelajaran:</td>
         <td>
            <asp:Label ID="lblKod" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td>Nama MataPelajaran:</td>
         <td>
            <asp:Label ID="lblNama" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td>Jam Kredit:</td>
         <td>
            <asp:Label ID="lblJamKredit" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="fbform_sap" colspan="2">&nbsp;
        </td>
    </tr>
     <tr>
        <td colspan="2">Carian Pensyarah.
        </td>
    </tr>
<tr>
         <td>Kod Kolej</td>
         <td>
            <asp:DropDownList ID="ddlKodKolej" runat="server" AutoPostBack="false" Width="350px">
            </asp:DropDownList>
        </td>
    </tr>
<tr>
         <td>Nama Kolej:</td>
         <td>
            <asp:TextBox ID="txtKod" runat="server" Width="350px" MaxLength="50"></asp:TextBox>
        </td>
    </tr>
    <tr>
         <td>
            <asp:Button ID="btnSearch" runat="server" Text="Cari " CssClass="fbbutton" />
    </tr>
    <tr>
         <td>
           </td>
    </tr>
    <tr>
    <td colspan="2">Senarai Pensyarah. <td> 
    </tr>
    <tr>
        <td colspan="2">
            <asp:GridView ID="datRespondent" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="Nama"
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
                     <asp:TemplateField HeaderText="Nama">
                        <ItemTemplate>
                            <asp:Label ID="Nama" runat="server" Text='<%# Bind("Nama")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="MyKad">
                        <ItemTemplate>
                            <asp:Label ID="MyKad" runat="server" Text='<%# Bind("MyKad")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                   
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" />
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

    </tr>
    <tr>
        <td colspan="2">
            &nbsp;</tr>
   <tr>
         <td>
        </td>
        <td>            <asp:Button ID="btnCreate" runat="server" Text="Pilih Pensyarah" CssClass="fbbutton" /><asp:LinkButton
                    ID="lnkList" runat="server">|Carian MataPelajaran</asp:LinkButton></td>
    </tr>
</table>
<div class="info" id="divMsg" runat="server">
    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
</div>