<%@ control language="vb" autoeventwireup="false" codebehind="pelajar_list.ascx.vb" inherits="apkv_v2_user.pelajar_list2" %>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Carian </td>
    </tr>
    <tr>
        <td style="width: 20%;">Kohort:</td>
        <td>
            <asp:dropdownlist id="ddlTahun" runat="server" autopostback="true" width="200px">
            </asp:dropdownlist>
        </td>
    </tr>

    <tr>
        <td style="width: 20%;">Semester:</td>
        <td>
            <asp:dropdownlist id="ddlSemester" runat="server" autopostback="false" width="200px">
            </asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td style="width: 20%;">Sesi Pengambilan:</td>
        <td>
            <asp:checkboxlist id="chkSesi" runat="server" autopostback="true" width="349px" repeatdirection="Horizontal">
                <asp:listitem enabled="False">1</asp:listitem>
                <asp:listitem enabled="False">2</asp:listitem>
            </asp:checkboxlist>
        </td>
    </tr>
    <tr>
        <td style="width: 20%;">Kod Program:</td>
        <td>
            <asp:dropdownlist id="ddlKodKursus" runat="server" autopostback="true" width="350px"></asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td style="width: 20%;">Nama Kelas:</td>
        <td>
            <asp:dropdownlist id="ddlNamaKelas" runat="server" autopostback="false" width="350px"></asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td style="width: 20%;">Nama Calon:</td>
        <td>
            <asp:textbox id="txtNama" runat="server" width="350px" maxlength="200"></asp:textbox>
        </td>
    </tr>
    <tr>
        <td style="width: 20%;">Mykad:</td>
        <td>
            <asp:textbox id="txtMYKAD" runat="server" width="350px" maxlength="200"></asp:textbox>
        </td>
    </tr>

    <tr>
        <td colspan="2">
            <asp:button id="btnSearch" runat="server" text="Cari " cssclass="fbbutton" />
        </td>
    </tr>
</table>
<div class="info" id="divMsgTop" runat="server">
    <asp:label id="lblMsgTop" runat="server" text=""></asp:label>
</div>
<table class="fbform">
    <tr class="fbform_header">
        <td>Senarai Calon
        </td>
    </tr>
    <tr>
        <td>
            <asp:gridview id="datRespondent" runat="server" autogeneratecolumns="False" allowpaging="True"
                cellpadding="4" forecolor="#333333" gridlines="None" datakeynames="PelajarID"
                width="100%" pagesize="25" cssclass="gridview_footer" enablemodelvalidation="True">
                <rowstyle backcolor="#F7F6F3" forecolor="#333333" />
                <columns>
                    <asp:templatefield headertext="#">
                        <itemtemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </itemtemplate>
                        <headerstyle horizontalalign="Left" verticalalign="Top" />
                        <itemstyle verticalalign="Middle" />
                    </asp:templatefield>
                    <asp:templatefield headertext="Kohort">
                        <itemtemplate>
                            <asp:label id="Tahun" runat="server" text='<%# Bind("Tahun")%>'></asp:label>
                        </itemtemplate>
                    </asp:templatefield>
                    <asp:templatefield headertext="Semester">
                        <itemtemplate>
                            <asp:label id="Semester" runat="server" text='<%# Bind("Semester")%>'></asp:label>
                        </itemtemplate>
                    </asp:templatefield>
                    <asp:templatefield headertext="Sesi">
                        <itemtemplate>
                            <asp:label id="Sesi" runat="server" text='<%# Bind("Sesi") %>'></asp:label>
                        </itemtemplate>
                    </asp:templatefield>
                    <asp:templatefield headertext="Nama">
                        <itemtemplate>
                            <asp:label id="Nama" runat="server" text='<%# Bind("Nama") %>'></asp:label>
                        </itemtemplate>
                    </asp:templatefield>
                    <asp:templatefield headertext="Mykad">
                        <itemtemplate>
                            <asp:label id="Mykad" runat="server" text='<%# Bind("Mykad")%>'></asp:label>
                        </itemtemplate>
                    </asp:templatefield>
                    <asp:templatefield headertext="Angka Giliran">
                        <itemtemplate>
                            <asp:label id="AngkaGiliran" runat="server" text='<%# Bind("AngkaGiliran")%>'></asp:label>
                        </itemtemplate>
                    </asp:templatefield>
                    <asp:templatefield headertext="Kod Program">
                        <itemtemplate>
                            <asp:label id="KodKursus" runat="server" text='<%# Bind("KodKursus")%>'></asp:label>
                        </itemtemplate>
                    </asp:templatefield>
                    <asp:templatefield headertext="Nama Kelas">
                        <itemtemplate>
                            <asp:label id="NamaKelas" runat="server" text='<%# Bind("NamaKelas")%>'></asp:label>
                        </itemtemplate>
                    </asp:templatefield>
                    <asp:templatefield headertext="Jantina">
                        <itemtemplate>
                            <asp:label id="Jantina" runat="server" text='<%# Bind("Jantina")%>'></asp:label>
                        </itemtemplate>
                    </asp:templatefield>
                    <asp:templatefield headertext="Kaum">
                        <itemtemplate>
                            <asp:label id="Kaum" runat="server" text='<%# Bind("Kaum")%>'></asp:label>
                        </itemtemplate>
                    </asp:templatefield>
                    <asp:templatefield headertext="Agama">
                        <itemtemplate>
                            <asp:label id="Agama" runat="server" text='<%# Bind("Agama")%>'></asp:label>
                        </itemtemplate>
                    </asp:templatefield>

                    <asp:commandfield selecttext="[KEMASKINI]" showselectbutton="True" headertext="KEMASKINI" />

                    <asp:templatefield headertext="Padam">
                        <itemtemplate>
                            <asp:imagebutton width="12" height="12" id="btnDelete" commandname="Delete" onclientclick="javascript:return confirm('Anda pasti untuk padam rekod ini? Pemadaman yang dilakukan tidak boleh diubah')" runat="server" imageurl="~/icons/download.jpg" tooltip="Padam Rekod" />
                        </itemtemplate>
                    </asp:templatefield>
                </columns>
                <footerstyle backcolor="#5D7B9D" font-bold="True" forecolor="White" font-underline="true" />
                <pagerstyle backcolor="#284775" forecolor="White" horizontalalign="Center" cssclass="cssPager" />
                <selectedrowstyle backcolor="#E2DED6" font-bold="True" forecolor="#333333" />
                <headerstyle backcolor="#5D7B9D" font-bold="True" forecolor="White" verticalalign="Middle"
                    horizontalalign="Left" />
                <editrowstyle backcolor="#999999" />
                <alternatingrowstyle backcolor="White" forecolor="#284775" />
            </asp:gridview>
        </td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <td>
            <asp:button id="btnFile" runat="server" text="Export" cssclass="fbbutton" />
        </td>
    </tr>
</table>
<br />
<div class="info" id="divMsg" runat="server">
    <asp:label id="lblKolejID" runat="server" text="" visible="false"></asp:label>
    <asp:label id="lblMsg" runat="server" text=""></asp:label>
</div>
