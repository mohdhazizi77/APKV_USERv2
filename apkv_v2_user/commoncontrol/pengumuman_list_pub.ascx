<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="pengumuman_list_pub.ascx.vb" Inherits="apkv_v2_user.pengumuman_list_pub" %>

<table class="fbform">
    <tr>
        <td style="vertical-align: top;">
            <asp:GridView ID="datRespondent" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                CellPadding="3" ForeColor="Black" GridLines="Vertical" PageSize="40" DataKeyNames="PengumumanID"
                Width="100%" Style="text-align: left;" HeaderStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Top" RowStyle-VerticalAlign="Top">
                <Columns>
                    <asp:TemplateField HeaderText="#">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Pengumuman">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkRead" runat="server" OnClick="lnkRead_Click" Text='<%# Bind("Title")%>' Font-Bold="true"></asp:LinkButton><br />
                            <%--<asp:Label ID="lblBody" runat="server" Text='<%# EvalWithMaxLength("Body", 100)%>' ToolTip='<%# Eval("Body")%>'></asp:Label>--%>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                </Columns>
                <FooterStyle BackColor="#CCCCCC" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" CssClass="cssPager" />
                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" VerticalAlign="Middle"
                    HorizontalAlign="Left" />
                <AlternatingRowStyle BackColor="#CCCCCC" />
            </asp:GridView>
        </td>
    </tr>
     
     <tr>
        <td class="fbform_sap">
            <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>
