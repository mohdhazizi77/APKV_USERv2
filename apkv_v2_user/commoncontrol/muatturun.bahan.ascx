<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="muatturun.bahan.ascx.vb" Inherits="apkv_v2_user.muatturun_bahan" %>
<table class="fbform" style ="width :100%">
    <tr class="fbform_header">
        <td colspan="2">Muat Turun Bahan</td>
    </tr>
</table>
<br />
<table class="fbform" style="width :100%">
    <tr>
        <td>Kohort:</td>
        <td><asp:DropDownList ID="ddlKohort" runat="server" Width="100px" ></asp:DropDownList></td>
    </tr>
    <tr>
        <td>Semester:</td>
        <td><asp:DropDownList ID="ddlSemester" runat="server" Width="100px" ></asp:DropDownList></td>
    </tr>
    <tr>
        <td style="width: 20%;">Kategori:</td>
        <td><asp:DropDownList ID="ddlKategory" runat="server" Width="400px" AutoPostBack ="true" ></asp:DropDownList></td>
        
    </tr>
    <tr>
        <td colspan ="2"></td>
    </tr>
    <tr>
         <td colspan="2">
            <asp:Button ID="btnSearch" runat="server" Text="Cari " CssClass="fbbutton" Width ="100" /></td>
    </tr>
    </table>
<br />

<%--umum--%>
<table class="fbform" style ="width :100%">
    <tr class="fbform_header">
        <td colspan="2">Senarai Kategori Muat Turun </td>
    </tr>
<tr>
    <td>
<asp:Panel Width ="100%" Height ="150%" runat ="server"  >
<asp:GridView ID="datRespondent2" runat="server" AutoGenerateColumns="False"
cellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="UmumID"
Width="100%"  CssClass="gridview_footer" EnableModelValidation="True" OnRowCommand ="datRespondent2_RowCommand">
<RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
<Columns>
        <asp:TemplateField HeaderText="#">
            <ItemTemplate>
                <%# Container.DataItemIndex + 1 %>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
            <ItemStyle VerticalAlign="Middle" />
        </asp:TemplateField>
     <asp:TemplateField HeaderText="Kategori">
            <ItemTemplate>
                <asp:label ID="Kategori" runat="server" Text='<%# Bind("Kategori")%>'></asp:label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" /><ItemStyle VerticalAlign="Middle" />
        </asp:TemplateField>
     <asp:TemplateField HeaderText="Tarikh Mula">
            <ItemTemplate>
                <asp:Label ID="STarikh" runat="server" Text='<%# Bind("STarikh")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" /><ItemStyle VerticalAlign="Middle" />
        </asp:TemplateField>
    <asp:TemplateField HeaderText="Tarikh Tamat">
            <ItemTemplate>
                <asp:Label ID="ETarikh" runat="server" Text='<%# Bind("ETarikh")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" /><ItemStyle VerticalAlign="Middle" />
        </asp:TemplateField>
    <asp:TemplateField HeaderText="Kohort">
            <ItemTemplate>
                <asp:Label ID="Kohort" runat="server" Text='<%# Bind("Kohort")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign ="Center"  VerticalAlign="Top" /><ItemStyle VerticalAlign="Middle" HorizontalAlign ="Center"  />
        </asp:TemplateField>
            <asp:TemplateField HeaderText="Semester">
            <ItemTemplate>
                <asp:Label ID="Semester" runat="server" Text='<%# Bind("Semester")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign ="Center"  VerticalAlign="Top" /><ItemStyle VerticalAlign="Middle" HorizontalAlign ="Center"  />
        </asp:TemplateField>
            <asp:TemplateField HeaderText="Sesi">
            <ItemTemplate>
                <asp:Label ID="Sesi" runat="server" Text='<%# Bind("Sesi")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center"  VerticalAlign="Top"  /><ItemStyle VerticalAlign="Middle" HorizontalAlign ="Center"  />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Tajuk">
            <ItemTemplate>
                <asp:TextBox ID="Tajuk" runat="server" Text='<%# Bind("Tajuk")%>' TextMode="MultiLine" Width="200px" Enabled ="false" ></asp:TextBox>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" /><ItemStyle VerticalAlign="Middle" />
        </asp:TemplateField>
            <asp:TemplateField HeaderText="Catatan">
            <ItemTemplate>
                 <asp:TextBox ID="Catatan" runat="server" Text='<%# Bind("Catatan")%>' TextMode="MultiLine" Width="200px" Enabled ="false" ></asp:TextBox>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" /><ItemStyle VerticalAlign="Middle" />
        </asp:TemplateField>
     
            <asp:TemplateField HeaderText="Muat Turun">
            <ItemTemplate>
                <asp:Label ID="lblID" runat="server" Text='<%# Bind("UmumID")%>' Visible ="false" ></asp:Label>
                <asp:ImageButton Width ="20" Height ="20" ID="btnDownload2" CommandName="DownloadBahan2" CommandArgument="<%# Container.DataItemIndex %>"  runat="server" ImageUrl="~/icons/download.gif" ToolTip="Muatturun"/>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center"  VerticalAlign="Top" /><ItemStyle VerticalAlign="Middle" HorizontalAlign ="Center"  />
        </asp:TemplateField>
    </Columns>
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" Font-Underline="true" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" VerticalAlign="Middle"
        HorizontalAlign="Left" />
    <EditRowStyle BackColor="#999999" />
    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
</asp:GridView>
    </asp:Panel>
    </td>
</tr>
        </table>
<br />

<%--list--%>
<table class="fbform" style ="width :100%">
    <tr class="fbform_header">
        <td colspan="2">Senarai Kategori VOKASIONAL</td>
    </tr>
<tr>
    <td>
<asp:Panel Width ="100%" Height ="450%" ScrollBars="Vertical" runat ="server"  >
<asp:GridView ID="datRespondent" runat="server" AutoGenerateColumns="False"
cellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="ID"
Width="100%" CssClass="gridview_footer" EnableModelValidation="True" OnRowCommand ="datRespondent_RowCommand">
<RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
<Columns>
        <asp:TemplateField HeaderText="#">
            <ItemTemplate>
                <%# Container.DataItemIndex + 1 %>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
            <ItemStyle VerticalAlign="Middle" />
        </asp:TemplateField>
    <asp:TemplateField HeaderText="Tarikh Mula">
            <ItemTemplate>
                <asp:Label ID="STarikh" runat="server" Text='<%# Bind("STarikh")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" /><ItemStyle VerticalAlign="Middle" />
        </asp:TemplateField>
    <asp:TemplateField HeaderText="Tarikh Tamat">
            <ItemTemplate>
                <asp:Label ID="ETarikh" runat="server" Text='<%# Bind("ETarikh")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" /><ItemStyle VerticalAlign="Middle" />
        </asp:TemplateField>
            <asp:TemplateField HeaderText="Kohort">
            <ItemTemplate>
                <asp:Label ID="Kohort" runat="server" Text='<%# Bind("Kohort")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign ="Center"  VerticalAlign="Top" /><ItemStyle VerticalAlign="Middle" HorizontalAlign ="Center" />
        </asp:TemplateField>
            <asp:TemplateField HeaderText="Sem">
            <ItemTemplate>
                <asp:Label ID="Semester" runat="server" Text='<%# Bind("Semester")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign ="Center"  VerticalAlign="Top" /><ItemStyle VerticalAlign="Middle" HorizontalAlign ="Center"  />
        </asp:TemplateField>
            <asp:TemplateField HeaderText="Sesi">
            <ItemTemplate>
                <asp:Label ID="Sesi" runat="server" Text='<%# Bind("Sesi")%>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign ="Center"  VerticalAlign="Top" /><ItemStyle VerticalAlign="Middle" HorizontalAlign ="Center"  />
        </asp:TemplateField>
     
        <asp:TemplateField HeaderText="Tajuk">
            <ItemTemplate>
                <asp:TextBox ID="Tajuk" runat="server" Text='<%# Bind("Tajuk")%>' TextMode="MultiLine" Width="250px" Enabled ="false" ></asp:TextBox>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" /><ItemStyle VerticalAlign="Middle" />
        </asp:TemplateField>
            <asp:TemplateField HeaderText="Catatan">
            <ItemTemplate>
                 <asp:TextBox ID="Catatan" runat="server" Text='<%# Bind("Catatan")%>' TextMode="MultiLine" Width="250px" Enabled ="false" ></asp:TextBox>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" /><ItemStyle VerticalAlign="Middle" />
        </asp:TemplateField>
     
            <asp:TemplateField HeaderText="Muat Turun">
            <ItemTemplate>
                <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID")%>' Visible ="false" ></asp:Label>
                <asp:ImageButton Width ="20" Height ="20" ID="btnDownload" CommandName="DownloadBahan" CommandArgument="<%# Container.DataItemIndex %>"  runat="server" ImageUrl="~/icons/download.gif" ToolTip="Muatturun"/>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" /><ItemStyle VerticalAlign="Middle" HorizontalAlign ="Center"  />
        </asp:TemplateField>
    </Columns>
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" Font-Underline="true" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" VerticalAlign="Middle"
        HorizontalAlign="Left" />
    <EditRowStyle BackColor="#999999" />
    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
</asp:GridView>
    </asp:Panel>
          </td>
</tr>
        </table>
<br />
<div class="info" id="divMsg" runat="server">
<asp:Label ID="lblKolejID" runat="server" Text="" Visible="false"></asp:Label>
<asp:Label ID="lblJenisKursus" runat="server" Text="" Visible="false"></asp:Label>
<asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
</div>