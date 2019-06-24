<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="pentaksiran_analisis.ascx.vb" Inherits="apkv_v2_user.pentaksiran_analisis" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Carian</td>
    </tr>
    <tr>
           <td style="width: 20%;">Kohort:</td>
             <td><asp:DropDownList ID="ddlTahun" runat="server" AutoPostBack="false" Width="200px">
            </asp:DropDownList>
        </td>
    </tr>
   
    <tr>
           <td style="width: 20%;">Semester:</td>
             <td> <asp:DropDownList ID="ddlSemester" runat="server" AutoPostBack="false" Width="200px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
         <td style="width: 20%;">Sesi Pengambilan:</td>
                 <td><asp:CheckBoxList ID="chkSesi" runat="server" AutoPostBack="true" Width="349px" RepeatDirection="Horizontal">
             <asp:ListItem>1</asp:ListItem>
             <asp:ListItem>2</asp:ListItem>
             </asp:CheckBoxList></td>
    </tr>
     <tr>
         <td style="width: 20%;">Kod Program:</td>
        <td><asp:DropDownList ID="ddlKodKursus" runat="server" AutoPostBack="true" Width="350px"></asp:DropDownList></td> 
    </tr>
     <tr>
         <td style="width: 20%;">Nama Kelas:</td>
        <td><asp:DropDownList ID="ddlKelas" runat="server" Width="350px"></asp:DropDownList></td> 
    </tr>
    <tr>
         <td colspan="2">
            <asp:Button ID="btnSearch" runat="server" Text="Cari " CssClass="fbbutton" /></td>
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
                     <asp:TemplateField HeaderText="PelajarID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="PelajarID" runat="server" Text='<%# Bind("PelajarID")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" /><ItemStyle VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nama">
                        <ItemTemplate>
                            <asp:Label ID="Nama" runat="server" Text='<%# Bind("Nama") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" /><ItemStyle VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Angka Giliran">
                        <ItemTemplate>
                            <asp:Label ID="AngkaGiliran" runat="server" Text='<%# Bind("AngkaGiliran")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" /><ItemStyle VerticalAlign="Middle" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Mykad">
                        <ItemTemplate>
                            <asp:Label ID="Mykad" runat="server" Text='<%# Bind("Mykad")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" /><ItemStyle VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="BM" >
                        <ItemTemplate>
                            <asp:Label ID="GredBM" runat="server"  Text='<%# Bind("GredBM")%>'></asp:Label>
                        </ItemTemplate>
                          <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PB" >
                        <ItemTemplate>
                            <asp:Label ID="B_BahasaMelayu" runat="server" DataFormatString="{0:F2}" HtmlEncode="false" Text='<%# Bind("B_BahasaMelayu")%>'></asp:Label>
                        </ItemTemplate>
                          <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="PA" >
                        <ItemTemplate>
                            <asp:Label ID="A_BahasaMelayu" runat="server" DataFormatString="{0:F2}" HtmlEncode="false" Text='<%# Bind("A_BahasaMelayu")%>'></asp:Label>
                        </ItemTemplate>
                          <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PB Sem 4" >
                        <ItemTemplate>
                            <asp:Label ID="B_BahasaMelayu3" runat="server" DataFormatString="{0:F2}" HtmlEncode="false" Text='<%# Bind("B_BahasaMelayu3")%>'></asp:Label>
                        </ItemTemplate>
                          <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="PA Sem 4" >
                        <ItemTemplate>
                            <asp:Label ID="A_BahasaMelayu3" runat="server" DataFormatString="{0:F2}" HtmlEncode="false" Text='<%# Bind("A_BahasaMelayu3")%>'></asp:Label>
                        </ItemTemplate>
                          <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="BI" >
                        <ItemTemplate>
                            <asp:Label ID="GredBI" runat="server"  Text='<%# Bind("GredBI")%>'></asp:Label>
                        </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PB" >
                        <ItemTemplate>
                            <asp:Label ID="B_BahasaInggeris" runat="server" Text='<%# String.Format("{0:f2}", DataBinder.Eval(Container.DataItem, "B_BahasaInggeris"))%>' ></asp:Label>
                        </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PA" >
                        <ItemTemplate>
                            <asp:Label ID="A_BahasaInggeris" runat="server" Text='<%# String.Format("{0:f2}", DataBinder.Eval(Container.DataItem, "A_BahasaInggeris"))%>' ></asp:Label>
                        </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="MT" >
                        <ItemTemplate>
                            <asp:Label ID="GredMT" runat="server"  Text='<%# Bind("GredMT")%>'></asp:Label>
                        </ItemTemplate>
                          <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PB" >
                        <ItemTemplate>
                            <asp:Label ID="B_Mathematics" runat="server" Text='<%# Bind("B_Mathematics")%>'></asp:Label>
                        </ItemTemplate>
                          <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField> 
                     <asp:TemplateField HeaderText="PA" >
                        <ItemTemplate>
                            <asp:Label ID="A_Mathematics" runat="server" Text='<%# Bind("A_Mathematics")%>'></asp:Label>
                        </ItemTemplate>
                          <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField> 
                     <asp:TemplateField HeaderText="SC" >
                        <ItemTemplate>
                            <asp:Label ID="GreC" runat="server"  Text='<%# Bind("GredSC")%>'></asp:Label>
                        </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PB1" >
                        <ItemTemplate>
                            <asp:Label ID="B_Science1" runat="server" Text='<%# Bind("B_Science1")%>'></asp:Label>
                        </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PA1" >
                        <ItemTemplate>
                            <asp:Label ID="A_Science1" runat="server" Text='<%# Bind("A_Science1")%>'></asp:Label>
                        </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="PB2" >
                        <ItemTemplate>
                            <asp:Label ID="B_Science2" runat="server" Text='<%# Bind("B_Science2")%>'></asp:Label>
                        </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PA2" >
                        <ItemTemplate>
                            <asp:Label ID="A_Science2" runat="server" Text='<%# Bind("A_Science2")%>'></asp:Label>
                        </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SJ" >
                        <ItemTemplate>
                            <asp:Label ID="GredSJ" runat="server"  Text='<%# Bind("GredSJ")%>'></asp:Label>
                        </ItemTemplate>
                           <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PB" >
                        <ItemTemplate>
                            <asp:Label ID="B_Sejarah" runat="server" Text='<%# Bind("B_Sejarah")%>'></asp:Label>
                        </ItemTemplate>
                           <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PA" >
                        <ItemTemplate>
                            <asp:Label ID="A_Sejarah" runat="server" Text='<%# Bind("A_Sejarah")%>' visible ='<%# If(Eval("Semester").ToString() = "4", "False", "True") %>'></asp:Label>
                        </ItemTemplate>
                           <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PI" >
                        <ItemTemplate>
                            <asp:Label ID="GredPI" runat="server"  Text='<%# Bind("GredPI")%>'></asp:Label>
                        </ItemTemplate>
                           <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PB1" >
                        <ItemTemplate>
                            <asp:Label ID="B_PendidikanIslam1" runat="server" Text='<%# Bind("B_PendidikanIslam1")%>'></asp:Label>
                        </ItemTemplate>
                           <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PA1" >
                        <ItemTemplate>
                            <asp:Label ID="A_PendidikanIslam1" runat="server" Text='<%# Bind("A_PendidikanIslam1")%>'></asp:Label>
                        </ItemTemplate>
                           <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PB2" >
                        <ItemTemplate>
                            <asp:Label ID="B_PendidikanIslam2" runat="server" Text='<%# Bind("B_PendidikanIslam2")%>'></asp:Label>
                        </ItemTemplate>
                           <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PA2" >
                        <ItemTemplate>
                            <asp:Label ID="A_PendidikanIslam2" runat="server" Text='<%# Bind("A_PendidikanIslam2")%>'></asp:Label>
                        </ItemTemplate>
                           <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PM" >
                        <ItemTemplate>
                            <asp:Label ID="GredPM" runat="server"   Text='<%# Bind("GredPM")%>'></asp:Label>
                        </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PB" >
                        <ItemTemplate>
                            <asp:Label ID="B_PendidikanMoral" runat="server"  Text='<%# Bind("B_PendidikanMoral")%>'></asp:Label>
                        </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="PA" >
                        <ItemTemplate>
                            <asp:Label ID="A_PendidikanMoral" runat="server"  Text='<%# Bind("A_PendidikanMoral")%>'></asp:Label>
                        </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"/>
                          <ItemStyle  HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="M1" >
                        <ItemTemplate>
                            <asp:Label ID="GredV1" runat="server"  Text='<%# Bind("GredV1")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PB Amali" >
                        <ItemTemplate>
                            <asp:Label ID="B_Amali1" runat="server" Text='<%# Bind("B_Amali1")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PB Teori" >
                        <ItemTemplate>
                            <asp:Label ID="B_Teori1" runat="server" Text='<%# Bind("B_Teori1")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="PA Amali" >
                        <ItemTemplate>
                            <asp:Label ID="A_Amali1" runat="server" Text='<%# Bind("A_Amali1")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <%-- <asp:TemplateField HeaderText="PA Teori" >
                        <ItemTemplate>
                            <asp:Label ID="A_Teori1" runat="server" Text='<%# Bind("A_Teori1")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>--%>
                     <asp:TemplateField HeaderText="M2" >
                        <ItemTemplate>
                            <asp:Label ID="GredV2" runat="server"  Text='<%# Bind("GredV2")%>'></asp:Label>
                        </ItemTemplate>
                         <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="PB Amali" >
                        <ItemTemplate>
                            <asp:Label ID="B_Amali2" runat="server" Text='<%# Bind("B_Amali2")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PB Teori" >
                        <ItemTemplate>
                            <asp:Label ID="B_Teori2" runat="server" Text='<%# Bind("B_Teori2")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="PA Amali" >
                        <ItemTemplate>
                            <asp:Label ID="A_Amali2" runat="server" Text='<%# Bind("A_Amali2")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <%-- <asp:TemplateField HeaderText="PA Teori" >
                        <ItemTemplate>
                            <asp:Label ID="A_Teori2" runat="server" Text='<%# Bind("A_Teori2")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>--%>
                   <asp:TemplateField HeaderText="M3" >
                        <ItemTemplate>
                            <asp:Label ID="GredV3" runat="server"  Text='<%# Bind("GredV3")%>'></asp:Label>
                        </ItemTemplate>
                       <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="PB Amali" >
                        <ItemTemplate>
                            <asp:Label ID="B_Amali3" runat="server" Text='<%# Bind("B_Amali3")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PB Teori" >
                        <ItemTemplate>
                            <asp:Label ID="B_Teori3" runat="server" Text='<%# Bind("B_Teori3")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="PA Amali" >
                        <ItemTemplate>
                            <asp:Label ID="A_Amali3" runat="server" Text='<%# Bind("A_Amali3")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                  <%--   <asp:TemplateField HeaderText="PA Teori" >
                        <ItemTemplate>
                            <asp:Label ID="A_Teori3" runat="server" Text='<%# Bind("A_Teori3")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>--%>
                     <asp:TemplateField HeaderText="M4" >
                        <ItemTemplate>
                            <asp:Label ID="GredV4" runat="server"  Text='<%# Bind("GredV4")%>'></asp:Label>
                        </ItemTemplate>
                         <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="PB Amali" >
                        <ItemTemplate>
                            <asp:Label ID="B_Amali4" runat="server" Text='<%# Bind("B_Amali4")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PB Teori" >
                        <ItemTemplate>
                            <asp:Label ID="B_Teori4" runat="server" Text='<%# Bind("B_Teori4")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="PA Amali" >
                        <ItemTemplate>
                            <asp:Label ID="A_Amali4" runat="server" Text='<%# Bind("A_Amali4")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                   <%--  <asp:TemplateField HeaderText="PA Teori" >
                        <ItemTemplate>
                            <asp:Label ID="A_Teori4" runat="server" Text='<%# Bind("A_Teori4")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>--%>
                     <asp:TemplateField HeaderText="M5" >
                        <ItemTemplate>
                            <asp:Label ID="GredV5" runat="server"  Text='<%# Bind("GredV5")%>'></asp:Label>
                        </ItemTemplate>
                         <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="PB Amali" >
                        <ItemTemplate>
                            <asp:Label ID="B_Amali5" runat="server" Text='<%# Bind("B_Amali5")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PB Teori" >
                        <ItemTemplate>
                            <asp:Label ID="B_Teori5" runat="server" Text='<%# Bind("B_Teori5")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="PA Amali" >
                        <ItemTemplate>
                            <asp:Label ID="A_Amali5" runat="server" Text='<%# Bind("A_Amali5")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                  <%--   <asp:TemplateField HeaderText="PA Teori" >
                        <ItemTemplate>
                            <asp:Label ID="A_Teori5" runat="server" Text='<%# Bind("A_Teori5")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>--%>
                      <asp:TemplateField HeaderText="M6" >
                        <ItemTemplate>
                            <asp:Label ID="GredV6" runat="server"  Text='<%# Bind("GredV6")%>'></asp:Label>
                        </ItemTemplate>
                          <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="PB Amali" >
                        <ItemTemplate>
                            <asp:Label ID="B_Amali6" runat="server" Text='<%# Bind("B_Amali6")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PB Teori" >
                        <ItemTemplate>
                            <asp:Label ID="B_Teori6" runat="server" Text='<%# Bind("B_Teori6")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="PA Amali" >
                        <ItemTemplate>
                            <asp:Label ID="A_Amali6" runat="server" Text='<%# Bind("A_Amali6")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <%-- <asp:TemplateField HeaderText="PA Teori" >
                        <ItemTemplate>
                            <asp:Label ID="A_Teori6" runat="server" Text='<%# Bind("A_Teori6")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="M7" >
                        <ItemTemplate>
                            <asp:Label ID="GredV7" runat="server"  Text='<%# Bind("GredV7")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="PB Amali" >
                        <ItemTemplate>
                            <asp:Label ID="B_Amali7" runat="server" Text='<%# Bind("B_Amali7")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PB Teori" >
                        <ItemTemplate>
                            <asp:Label ID="B_Teori7" runat="server" Text='<%# Bind("B_Teori7")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="PA Amali" >
                        <ItemTemplate>
                            <asp:Label ID="A_Amali7" runat="server" Text='<%# Bind("A_Amali7")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <%-- <asp:TemplateField HeaderText="PA Teori" >
                        <ItemTemplate>
                            <asp:Label ID="A_Teori7" runat="server" Text='<%# Bind("A_Teori7")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>--%>
                     <asp:TemplateField HeaderText="M8" >
                        <ItemTemplate>
                            <asp:Label ID="GredV8" runat="server"  Text='<%# Bind("GredV8")%>'></asp:Label>
                        </ItemTemplate>
                         <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="PB Amali" >
                        <ItemTemplate>
                            <asp:Label ID="B_Amali8" runat="server" Text='<%# Bind("B_Amali8")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PB Teori" >
                        <ItemTemplate>
                            <asp:Label ID="B_Teori8" runat="server" Text='<%# Bind("B_Teori8")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="PA Amali" >
                        <ItemTemplate>
                            <asp:Label ID="A_Amali8" runat="server" Text='<%# Bind("A_Amali8")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                   <%--  <asp:TemplateField HeaderText="PA Teori" >
                        <ItemTemplate>
                            <asp:Label ID="A_Teori8" runat="server" Text='<%# Bind("A_Teori8")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>--%>
                      <asp:TemplateField HeaderText="PNG BM">
                        <ItemTemplate>
                            <asp:Label ID="PNGBM" runat="server"  Text='<%# Bind("PNGBM")%>'></asp:Label>
                        </ItemTemplate>
                         <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="PNGK BM">
                        <ItemTemplate>
                            <asp:Label ID="PNGKBM" runat="server"  Text='<%# Bind("PNGKBM")%>'></asp:Label>
                        </ItemTemplate>
                         <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PNGA">
                        <ItemTemplate>
                            <asp:Label ID="PNGA" runat="server"  Text='<%# Bind("PNGA")%>'></asp:Label>
                        </ItemTemplate>
                         <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PNGK AKA">
                        <ItemTemplate>
                            <asp:Label ID="PNGKA" runat="server"  Text='<%# Bind("PNGKA")%>'></asp:Label>
                        </ItemTemplate>
                         <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PNGV">
                        <ItemTemplate>
                            <asp:Label ID="PNGV" runat="server"  Text='<%# Bind("PNGV")%>'></asp:Label>
                        </ItemTemplate>
                         <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PNGK VOK">
                        <ItemTemplate>
                            <asp:Label ID="PNGKV" runat="server"  Text='<%# Bind("PNGKV")%>'></asp:Label>
                        </ItemTemplate>
                         <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="PNGK">
                        <ItemTemplate>
                            <asp:Label ID="PNGK" runat="server"  Text='<%# Bind("PNGK")%>'></asp:Label>
                        </ItemTemplate>
                         <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="PNGKK">
                        <ItemTemplate>
                            <asp:Label ID="PNGKK" runat="server"  Text='<%# Bind("PNGKK")%>'></asp:Label>
                        </ItemTemplate>
                         <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="GredBMSetara">
                        <ItemTemplate>
                            <asp:Label ID="GredBMSetara" runat="server"  Text='<%# Bind("GredBMSetara")%>'></asp:Label>
                        </ItemTemplate>
                         <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="SMP Grade">
                        <ItemTemplate>
                            <asp:Label ID="SMP_Grade" runat="server"  Text='<%# Bind("SMP_Grade")%>'></asp:Label>
                        </ItemTemplate>
                         <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                       <asp:TemplateField >
                     <%--    <HeaderTemplate >
                                <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" OnCheckedChanged="CheckUncheckAll" />
                            </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect"  runat="server" />
                        </ItemTemplate>
                         <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <ItemStyle VerticalAlign="Middle" />--%>
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
<%--         <asp:Button ID="btnExport" runat="server" Text="Export " CssClass="fbbutton"/>&nbsp;--%>
            <asp:Button ID="btnPrint" runat="server" Text="Eksport ke Excel" CssClass="fbbutton"
               />&nbsp;<asp:HyperLink ID="hyPDF" runat="server" Target="_blank"
                    Visible="false">Klik disini untuk muat turun.</asp:HyperLink>
        </td>
    </tr>
    </table>
<br />
<div class="info" id="divMsg" runat="server">
<asp:Label ID="lblKolejID" runat="server" Text="" Visible="false"></asp:Label>
<asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
</div>