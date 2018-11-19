<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="markah_import.ascx.vb"
    Inherits="apkv_v2_user.markah_import" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Akademik >> Pentaksiran Berterusan Akademik Import</td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Pentaksiran Berterusan Akademik Import.</td>
    </tr>
     <tr>
          <td style="width: 20%;">Kohort:</td>
        <td>
            <asp:DropDownList ID="ddlTahun" runat="server" AutoPostBack="false" Width="350px"></asp:DropDownList>
        </td>
    </tr>
     <tr>
          <td style="width: 20%;">Semester:</td>
        <td>
            <asp:DropDownList ID="ddlSemester" runat="server" AutoPostBack="false" Width="350px"></asp:DropDownList>
        </td>
    </tr>
     <tr>
          <td style="width: 20%;">Sesi Pengambilan:</td>
        <td><asp:CheckBoxList ID="chkSesi" runat="server"  AutoPostBack="true" Width="349px" RepeatDirection="Horizontal">
             <asp:ListItem Enabled="False">1</asp:ListItem>
             <asp:ListItem Enabled="False">2</asp:ListItem>
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
         <td colspan ="2">MuatNaik Format Fail Excel:
         <asp:Button ID="btnFile" runat="server" Text="Excel" CssClass="fbbutton" Height="25px" Width="116px" /></td>
    </tr>
    <tr>
         <td>Pilih Fail Excel:
        </td>
         <td>
            <asp:FileUpload ID="FlUploadcsv" runat="server" />&nbsp;
            <asp:RegularExpressionValidator ID="regexValidator" runat="server" ErrorMessage="Only XLSX file are allowed"
                ValidationExpression="(.*\.([Xx][Ll][Ss][Xx])$)" ControlToValidate="FlUploadcsv"></asp:RegularExpressionValidator>
        </td>
    </tr>
   </table> 
<div class="info" id="divMsgResult" runat="server">
  <asp:Label ID="lblMsgResult" runat="server" Text="Mesej..."></asp:Label>
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
                    <asp:TemplateField HeaderText="BM">
                        <ItemTemplate>
                            <asp:TextBox ID="B_BahasaMelayu" runat="server" Width="30px" MaxLength="3" Text='<%# Bind("B_BahasaMelayu")%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="BM 1">
                        <ItemTemplate>
                            <asp:TextBox ID="B_BahasaMelayu1" runat="server" Width="30px" MaxLength="3" Text='<%# Bind("B_BahasaMelayu1")%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="BM 2">
                        <ItemTemplate>
                            <asp:TextBox ID="B_BahasaMelayu2" runat="server" Width="30px" MaxLength="3" Text='<%# Bind("B_BahasaMelayu2")%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="BM 3">
                        <ItemTemplate>
                            <asp:TextBox ID="B_BahasaMelayu3" runat="server" Width="30px" MaxLength="3" Text='<%# Bind("B_BahasaMelayu3")%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="BI">
                        <ItemTemplate>
                            <asp:TextBox ID="B_BahasaInggeris" runat="server" Width="30px" MaxLength="3" Text='<%# Bind("B_BahasaInggeris")%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Science">
                        <ItemTemplate>
                            <asp:TextBox ID="B_Science1" runat="server" Width="30px" MaxLength="3" Text='<%# Bind("B_Science1")%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                   <%--  <asp:TemplateField HeaderText="Science 2">
                        <ItemTemplate>
                            <asp:TextBox ID="B_Scienc2e" runat="server" Width="30px" MaxLength="3" Text='<%# Bind("B_Science2")%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Sejarah">
                        <ItemTemplate>
                            <asp:TextBox ID="B_Sejarah" runat="server" Width="30px" MaxLength="3" Text='<%# Bind("B_Sejarah")%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="P.Islam">
                        <ItemTemplate>
                            <asp:TextBox ID="B_PendidikanIslam1" runat="server" Width="30px" MaxLength="3" Text='<%# Bind("B_PendidikanIslam1")%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                   <%--  <asp:TemplateField HeaderText="P.Islam1">
                        <ItemTemplate>
                            <asp:TextBox ID="B_PendidikanIslam2" runat="server" Width="30px" MaxLength="3" Text='<%# Bind("B_PendidikanIslam2")%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                     <asp:TemplateField HeaderText="P.Moral">
                        <ItemTemplate>
                            <asp:TextBox ID="B_PendidikanMoral" runat="server"  Width="30px" MaxLength="3" Text='<%# Bind("B_PendidikanMoral")%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Mathematics">
                        <ItemTemplate>
                            <asp:TextBox ID="B_Mathematics" runat="server" Width="30px" MaxLength="3" Text='<%# Bind("B_Mathematics")%>'></asp:TextBox>
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
        <td>
            <asp:Button ID="btnUpdate" runat="server" Text="Kemaskini" CssClass="fbbutton" Visible="true" />&nbsp;
            <asp:Button ID="btnExport" runat="server" Text="Eksport" CssClass="fbbutton" Visible="true" />
        </td>
    </tr>
</table>
<br />
<div class="info" id="divMsg" runat="server">
 <asp:Label ID="lblKolejID" runat="server" Text="" Visible="false"></asp:Label>
 <asp:Label ID="lblMsg" runat="server" Text="System message..."></asp:Label>
</div>