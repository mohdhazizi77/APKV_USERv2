<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="markah_list.ascx.vb" Inherits="apkv_v2_user.markah_list" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Markah >> Paparan Keputusan Akademik </td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Paparan Keputusan Akademik</td>
    </tr>
     <tr>
          <td style="width: 20%;">Kohort:</td>
        <td>
            <asp:DropDownList ID="ddlTahun" runat="server" AutoPostBack="True" Width="350px"></asp:DropDownList>
        </td>
    </tr>
     <tr>
          <td style="width: 20%;">Semester:</td>
        <td>
            <asp:DropDownList ID="ddlSemester" runat="server" AutoPostBack="True" Width="350px"></asp:DropDownList>
        </td>
    </tr>
     <tr>
          <td style="width: 20%;">Sesi Pengambilan:</td>
        <td><asp:CheckBoxList ID="chkSesi" runat="server"  AutoPostBack="true" Width="349px" RepeatDirection="Horizontal">
             <asp:ListItem Selected="True">1</asp:ListItem>
             <asp:ListItem>2</asp:ListItem>
             </asp:CheckBoxList></td>
    </tr>
   <tr>
          <td style="width: 20%;">Kod Program:</td>
        <td>
            <asp:DropDownList ID="ddlKodKursus" runat="server" AutoPostBack="true" Width="350px"></asp:DropDownList>
        </td>
    </tr>
     <tr>        
        <td style="width: 20%;">Kelas:</td>
          <td><asp:DropDownList ID="ddlKelas" runat="server" AutoPostBack="false" Width="350px"></asp:DropDownList></td>
    </tr>
    <tr>
        <td colspan="2"><asp:Button ID="btnCari" runat="server" Text="Cari" CssClass="fbbutton" />&nbsp;</td>
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
                CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="PelajarID"
                Width="100%" PageSize="100" CssClass="gridview_footer">
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
                        <ItemTemplate>
                            <asp:Label ID="AngkaGiliran" runat="server" Text='<%# Bind("AngkaGiliran")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kod Program">
                        <ItemTemplate>
                            <asp:Label ID="KodKursus" runat="server" Text='<%# Bind("KodKursus")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Gred BM">
                        <ItemTemplate>
                            <asp:Label ID="GredBM" runat="server" Width="30px" Text='<%# Bind("GredBM")%>'></asp:Label>
                        </ItemTemplate>
                          <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Gred BI">
                        <ItemTemplate>
                            <asp:Label ID="GredBI" runat="server" Width="30px"  Text='<%# Bind("GredBI")%>'></asp:Label>
                        </ItemTemplate>
                          <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Gred Math">
                        <ItemTemplate>
                            <asp:Label ID="GredMT" runat="server"  Width="30px" Text='<%# Bind("GredMT")%>'></asp:Label>
                        </ItemTemplate>
                           <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Gred Science">
                        <ItemTemplate>
                            <asp:Label ID="GredSC" runat="server" Width="30px" Text='<%# Bind("GredSC")%>'></asp:Label>
                        </ItemTemplate>
                           <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Gred Sejarah">
                        <ItemTemplate>
                            <asp:Label ID="GredSJ" runat="server" Width="30px" Text='<%# Bind("GredSJ")%>'></asp:Label>
                        </ItemTemplate>
                          <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Gred P.Islam">
                        <ItemTemplate>
                            <asp:Label ID="GredPI" runat="server" Width="30px" Text='<%# Bind("GredPI")%>'></asp:Label>
                        </ItemTemplate>
                          <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Gred P.Moral">
                        <ItemTemplate>
                            <asp:Label ID="GredPM" runat="server"  Width="30px" Text='<%# Bind("GredPM")%>'></asp:Label>
                        </ItemTemplate>
                           <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
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
        <td>
            &nbsp;</td>
    </tr>
</table>
<br />
<div class="info" id="divMsg" runat="server">
  <asp:Label ID="lblKolejID" runat="server" Text="" Visible="false"></asp:Label>
  <asp:Label ID="lblMsg" runat="server" Text="System message..."></asp:Label>
</div>

