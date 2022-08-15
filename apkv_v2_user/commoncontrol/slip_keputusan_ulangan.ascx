﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="slip_keputusan_ulangan.ascx.vb" Inherits="apkv_v2_user.slip_keputusan_ulangan1" %>

<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Slip Keputusan Ulangan</td>
    </tr>
    <tr>
        <td style="width: 20%;">Tahun Peperiksaan : </td>
        <td>
            <asp:DropDownList ID="ddlTahun" runat="server" AutoPostBack="false" Width="200px">
            </asp:DropDownList>
        </td>
    </tr>

    <tr>
        <td style="width: 20%;">Matapelajaran : </td>
        <td>
            <asp:DropDownList ID="ddlMatapelajaran" runat="server"  Width="200px">
                <asp:ListItem Selected="True" Value="0">- Pilih -</asp:ListItem>
                <asp:ListItem Value="1">BAHASA MELAYU</asp:ListItem>
                <asp:ListItem Value="2">SEJARAH</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>

    <tr>
        <td colspan="2">
            <asp:Button ID="btnSearch" runat="server" Text="Cari" CssClass="fbbutton" /></td>
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
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <ItemStyle VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kohort" HeaderStyle-Width="10px">
                        <ItemTemplate>
                            <asp:Label ID="Tahun" runat="server" Width="10px" Text='<%# Bind("Tahun")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <ItemStyle VerticalAlign="Middle" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Semester" HeaderStyle-Width="10px">
                        <ItemTemplate>
                            <asp:Label ID="Semester" runat="server" Width="10px" Text='<%# Bind("Semester")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <ItemStyle VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sesi" HeaderStyle-Width="10px">
                        <ItemTemplate>
                            <asp:Label ID="Sesi" runat="server" Width="10px" Text='<%# Bind("Sesi") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <ItemStyle VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nama">
                        <ItemTemplate>
                            <asp:Label ID="Nama" runat="server" Text='<%# Bind("Nama") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <ItemStyle VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mykad">
                        <ItemTemplate>
                            <asp:Label ID="Mykad" runat="server" Text='<%# Bind("Mykad")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <ItemStyle VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Angka Giliran">
                        <ItemTemplate>
                            <asp:Label ID="AngkaGiliran" runat="server" Text='<%# Bind("AngkaGiliran")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <ItemStyle VerticalAlign="Middle" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" OnCheckedChanged="CheckUncheckAll" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
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
        <td colspan="3">Tetapan Tarikh</td>
    </tr>
    <tr style="text-align: center;">
        <td style="width: 10%;">Hari:
             <asp:DropDownList ID="ddlHari" runat="server" AutoPostBack="false">
                 <asp:ListItem>01</asp:ListItem>
                 <asp:ListItem>02</asp:ListItem>
                 <asp:ListItem>03</asp:ListItem>
                 <asp:ListItem>04</asp:ListItem>
                 <asp:ListItem>05</asp:ListItem>
                 <asp:ListItem>06</asp:ListItem>
                 <asp:ListItem>07</asp:ListItem>
                 <asp:ListItem>08</asp:ListItem>
                 <asp:ListItem>09</asp:ListItem>
                 <asp:ListItem>10</asp:ListItem>
                 <asp:ListItem>11</asp:ListItem>
                 <asp:ListItem>12</asp:ListItem>
                 <asp:ListItem>13</asp:ListItem>
                 <asp:ListItem>14</asp:ListItem>
                 <asp:ListItem>15</asp:ListItem>
                 <asp:ListItem>16</asp:ListItem>
                 <asp:ListItem>17</asp:ListItem>
                 <asp:ListItem>18</asp:ListItem>
                 <asp:ListItem>19</asp:ListItem>
                 <asp:ListItem>20</asp:ListItem>
                 <asp:ListItem>21</asp:ListItem>
                 <asp:ListItem>22</asp:ListItem>
                 <asp:ListItem>23</asp:ListItem>
                 <asp:ListItem>24</asp:ListItem>
                 <asp:ListItem>25</asp:ListItem>
                 <asp:ListItem>26</asp:ListItem>
                 <asp:ListItem>27</asp:ListItem>
                 <asp:ListItem>28</asp:ListItem>
                 <asp:ListItem>29</asp:ListItem>
                 <asp:ListItem>30</asp:ListItem>
                 <asp:ListItem>31</asp:ListItem>
             </asp:DropDownList>
        </td>
        <td style="width: 10%;">Bulan:
            <asp:DropDownList ID="ddlBulan" runat="server" AutoPostBack="false">
                <asp:ListItem>01</asp:ListItem>
                <asp:ListItem>02</asp:ListItem>
                <asp:ListItem>03</asp:ListItem>
                <asp:ListItem>04</asp:ListItem>
                <asp:ListItem>05</asp:ListItem>
                <asp:ListItem>06</asp:ListItem>
                <asp:ListItem>07</asp:ListItem>
                <asp:ListItem>08</asp:ListItem>
                <asp:ListItem>09</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
                <asp:ListItem>12</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td style="width: 10%;">Tahun:
         <asp:DropDownList ID="ddlTahun_1" runat="server" AutoPostBack="false" Width="53px" Height="17px">
         </asp:DropDownList>
        </td>
    </tr>

    <tr>
        <td style="text-align: center;" colspan="3">
                        <asp:Button ID="btnPrint" runat="server" Text="Cetak Slip Keputusan" CssClass="fbbutton" />&nbsp;<asp:HyperLink ID="hyPDF" runat="server" Target="_blank"
                Visible="false">Klik disini untuk muat turun.</asp:HyperLink>
        </td>
    </tr>
</table>
<br />
<div class="info" id="divMsg" runat="server">
    <asp:Label ID="lblKolejID" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
</div>

