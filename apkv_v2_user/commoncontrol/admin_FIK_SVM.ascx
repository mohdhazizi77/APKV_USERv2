<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="admin_FIK_SVM.ascx.vb" Inherits="apkv_v2_user.admin_FIK_SVM" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Carian </td>
    </tr>
    <tr>
       <td style="width: 10%;">BM Setara Pada Tahun: </td>
             <td><asp:DropDownList ID="ddlTahun" runat="server" Width="100px">
            </asp:DropDownList>
        </td>  
    </tr>
     <tr>
        <td style="width: 20%;">Sesi:</td>
        <td><asp:CheckBoxList ID="chkSesi" runat="server"  AutoPostBack="true" Width="349px" RepeatDirection="Horizontal">
             <asp:ListItem>1</asp:ListItem>
             <asp:ListItem>2</asp:ListItem>
             </asp:CheckBoxList></td>
    </tr>
    <tr>
         <td colspan="2">
             <asp:Button ID="btnSearch" runat="server" Text="Cari " CssClass="fbbutton" />
    </tr>
 </table> 
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan ="2">Senarai bagi Tahun:
            <asp:Label ID="lblTahun" runat="server" Text=""></asp:Label>
            </td>
        </tr>

    <tr>
        <td>
            <asp:GridView ID="datRespondent" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="PelajarID"
                Width="100%" PageSize="100" CssClass="gridview_footer" EnableModelValidation="True" Font-Names="Arial" Font-Size="Small">
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
                     <asp:TemplateField HeaderText="Sesi">
                        <ItemTemplate>
                            <asp:Label ID="SESI" runat="server" Text='<%# Bind("SESI")%>'></asp:Label>
                        </ItemTemplate>
                       
                    </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="Nama">
                        <ItemTemplate>
                            <asp:Label ID="Nama" runat="server" Text='<%# Bind("Nama")%>'></asp:Label>
                        </ItemTemplate>
                       
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Mykad">
                        <ItemTemplate>
                            <asp:Label ID="Mykad" runat="server" Text='<%# Bind("MYKAD")%>'></asp:Label>
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
                        </ItemTemplate>
                      
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Gred BM Setara">
                        <ItemTemplate>
                            <asp:Label ID="GredBMSetara" runat="server" Text='<%# Bind("GredBMSetara")%>'></asp:Label>
                        </ItemTemplate>
                      
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PNGKA">
                        <ItemTemplate>
                            <asp:Label ID="PNGKA" runat="server" Text='<%# Bind("PNGKA")%>'></asp:Label>
                        </ItemTemplate>
                      
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PNGKV">
                        <ItemTemplate>
                            <asp:Label ID="PNGKV" runat="server" Text='<%# Bind("PNGKV")%>'></asp:Label>
                        </ItemTemplate>
                      
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PNGKK">
                        <ItemTemplate>
                            <asp:Label ID="PNGKK" runat="server" Text='<%# Bind("PNGKK")%>'></asp:Label>
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
<br/>
<table class="fbform">
    <tr class="fbform_header">
      <td colspan="2">
           <asp:Label ID="lblMsgTop" runat="server" Text=""></asp:Label>
            <asp:Label ID="lblMsg" runat="server" Text="Pengesahan Data telah Berjaya "></asp:Label>
            <asp:Button ID="btnExport" runat="server" Text="Eksport " CssClass="fbbutton" Style="height: 26px" />
        </td>
    </tr>
    </table>
<br />
<div class="info" id="divMsg" runat="server">
<asp:Label ID="lblKolejID" runat="server" Text="" Visible="false"></asp:Label>
</div>