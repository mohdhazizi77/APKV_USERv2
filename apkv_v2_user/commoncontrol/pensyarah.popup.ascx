<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="pensyarah.popup.ascx.vb" Inherits="apkv_v2_user.pensyarah_popup" %>
<script type="text/javascript">

    function SetName(val) {
        console.log(val);
        var t
        val.Innertext = t;

        //window.opener.getdata(t);
        // window.close();
        alert(t);

    }

</script>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Carian
        </td>
    </tr>
        <tr>
        <td>Nama Pensyarah:
        </td>
        <td>
            <asp:TextBox ID="txtNama" runat="server" Width="350px" MaxLength="50"></asp:TextBox>
        </td>
    </tr>
   
   <%-- <tr>
        <td>
            <asp:Button ID="btnPilih" runat="server" Text="Pilih " CssClass="fbbutton"></td>
        <td>
            &nbsp;</td>
    </tr>--%>
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>
     <tr class="fbform_header">
        <td colspan="2">Senarai Pensyarah.
        </td>
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
                           <asp:linkbutton ID="linkbutton" runat="server" Text='<%#Eval("Nama")%>' OnClientClick ='SetName(this)'></asp:linkbutton>                         
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mykad">
                        <ItemTemplate>
                            <asp:Label ID="MYKAD" runat="server" Text='<%# Bind("MYKAD") %>'></asp:Label>
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
        <td colspan="2">
            <asp:Label ID="lblKolejID" runat="server" Text="" Visible="false"></asp:Label>
        </td>
    </tr>
 </table>
<div class="info" id="divMsg" runat="server">
    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label><br />
</div>
