<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="kolej_markah_list.ascx.vb"
    Inherits="apkv_v2_user.kolej_markah_list" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="4">Carian.
        </td>
    </tr>
    <tr>
        <td style="width:15%;">Kohort:
        </td>
         <td>
            <asp:DropDownList ID="ddlTahun" runat="server" AutoPostBack="false" Width="200px">
            </asp:DropDownList></td>
         <td>Semester</td>
         <td>
            <asp:DropDownList ID="ddlSemester" runat="server" AutoPostBack="false" Width="100px">
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td class="fbform_sap" colspan="4">&nbsp;
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <asp:Button ID="btnSearch" runat="server" Text="Cari " CssClass="fbbutton" Style="height: 26px" />
            &nbsp;<asp:Button ID="btnExport" runat="server" Text="Eksport " CssClass="fbbutton" Style="height: 26px" />
        </td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
         <td>Senarai Pelajar dan Markah.
        </td>
    </tr>
    <tr>
         <td>
            <asp:GridView ID="datRespondent" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="MarkahID"
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
                            <asp:Label ID="Nama" runat="server" Text='<%# Bind("Nama") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kohort">
                        <ItemTemplate>
                            <asp:Label ID="Tahun" runat="server" Text='<%# Bind("Tahun") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Semester">
                        <ItemTemplate>
                            <asp:Label ID="Semester" runat="server" Text='<%# Bind("Semester") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="M1_T">
                        <ItemTemplate>
                            <asp:Label ID="M1_TEORI" runat="server" Text='<%# Eval("M1_TEORI", "{0:n2}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="M1_A">
                        <ItemTemplate>
                            <asp:Label ID="M1_AMALI" runat="server" Text='<%# Eval("M1_AMALI", "{0:n2}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="M2_T">
                        <ItemTemplate>
                            <asp:Label ID="M2_TEORI" runat="server" Text='<%# Eval("M2_TEORI", "{0:n2}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="M2_A">
                        <ItemTemplate>
                            <asp:Label ID="M2_AMALI" runat="server" Text='<%# Eval("M2_AMALI", "{0:n2}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="M3_T">
                        <ItemTemplate>
                            <asp:Label ID="M3_TEORI" runat="server" Text='<%# Eval("M3_TEORI", "{0:n2}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="M3_A">
                        <ItemTemplate>
                            <asp:Label ID="M3_AMALI" runat="server" Text='<%# Eval("M3_AMALI", "{0:n2}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="M4_T">
                        <ItemTemplate>
                            <asp:Label ID="M4_TEORI" runat="server" Text='<%# Eval("M4_TEORI", "{0:n2}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="M4_A">
                        <ItemTemplate>
                            <asp:Label ID="M4_AMALI" runat="server" Text='<%# Eval("M4_AMALI", "{0:n2}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="M5_T">
                        <ItemTemplate>
                            <asp:Label ID="M5_TEORI" runat="server" Text='<%# Eval("M5_TEORI", "{0:n2}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="M5_A">
                        <ItemTemplate>
                            <asp:Label ID="M5_AMALI" runat="server" Text='<%# Eval("M5_AMALI", "{0:n2}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="M6_T">
                        <ItemTemplate>
                            <asp:Label ID="M6_TEORI" runat="server" Text='<%# Eval("M6_TEORI", "{0:n2}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="M6_A">
                        <ItemTemplate>
                            <asp:Label ID="M6_AMALI" runat="server" Text='<%# Eval("M6_AMALI", "{0:n2}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="M7_T">
                        <ItemTemplate>
                            <asp:Label ID="M7_TEORI" runat="server" Text='<%# Eval("M7_TEORI", "{0:n2}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="M7_A">
                        <ItemTemplate>
                            <asp:Label ID="M7_AMALI" runat="server" Text='<%# Eval("M7_AMALI", "{0:n2}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="M8_T">
                        <ItemTemplate>
                            <asp:Label ID="M8_TEORI" runat="server" Text='<%# Eval("M8_TEORI", "{0:n2}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="M8_A">
                        <ItemTemplate>
                            <asp:Label ID="M8_AMALI" runat="server" Text='<%# Eval("M8_AMALI", "{0:n2}") %>'></asp:Label>
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
<div class="info" id="divMsg" runat="server">
    <asp:Label ID="lblMsg" runat="server" Text="Mesej..."></asp:Label>
</div>
