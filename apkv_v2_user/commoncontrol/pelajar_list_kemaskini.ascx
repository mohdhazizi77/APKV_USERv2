<%@ control language="vb" autoeventwireup="false" codebehind="pelajar_list_kemaskini.ascx.vb" inherits="apkv_v2_user.pelajar_list_kemaskini" %>

<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Pendaftaran >> Kemaskini Calon  </td>
    </tr>
</table>
<br />
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">Kemaskini Calon</td>
    </tr>
    <tr>
        <td colspan="2">Peringkat Pengajian: Pra Diploma</td>
    </tr>
    <tr>
        <td colspan="2"></td>
    </tr>
    <tr>
        <td style="width: 20%;">Kohort:</td>
        <td>
            <asp:dropdownlist id="ddlTahun" runat="server" autopostback="false" width="350px">
            </asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td>Semester:</td>
        <td>
            <asp:dropdownlist id="ddlSemester" runat="server" autopostback="false" width="100px">
            </asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td style="width: 20%;">Sesi Pengambilan:</td>
        <td>
            <asp:checkboxlist id="chkSesi" runat="server" width="349px" repeatdirection="Horizontal" autopostback="true">
                <asp:listitem enabled="False">1</asp:listitem>
                <asp:listitem enabled="False">2</asp:listitem>
            </asp:checkboxlist>

    </tr>
    <tr>
        <td style="width: 20%;">Nama Bidang:</td>
        <td>
            <asp:dropdownlist id="ddlKluster" runat="server" autopostback="true" width="350px"></asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td style="width: 20%;">Kod Program:</td>
        <td>
            <asp:dropdownlist id="ddlKodKursus" runat="server" autopostback="true" width="350px">
            </asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td style="width: 20%;">Kelas:</td>
        <td>
            <asp:dropdownlist id="ddlNamaKelas" runat="server" autopostback="false" width="350px">
            </asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td style="width: 20%;">Nama Calon: </td>
        <td>
            <asp:textbox id="txtNama" runat="server" width="350px" maxlength="250"></asp:textbox>
        </td>
    </tr>
    <tr>
        <td style="width: 20%;">Mykad:</td>
        <td>
            <asp:textbox id="txtMYKAD" runat="server" width="350px" maxlength="50"></asp:textbox>
        </td>
    </tr>
    <tr>
        <td style="width: 20%;">Jantina:</td>
        <td>
            <asp:checkboxlist id="chkJantina" runat="server" width="349px" repeatdirection="Horizontal">
                <asp:listitem>LELAKI</asp:listitem>
                <asp:listitem>PEREMPUAN</asp:listitem>
            </asp:checkboxlist>
    </tr>
    <tr>
        <td style="width: 20%;">Kaum:</td>
        <td>
            <asp:dropdownlist id="ddlKaum" runat="server" autopostback="false" width="350px">
            </asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td style="width: 20%;">Agama:</td>
        <td>
            <asp:checkboxlist id="chkAgama" runat="server" width="349px" repeatdirection="Horizontal">
                <asp:listitem>ISLAM</asp:listitem>
                <asp:listitem>LAIN-LAIN</asp:listitem>
            </asp:checkboxlist>
    </tr>
    <tr>
        <td style="width: 20%;">Emel:</td>
        <td>
            <asp:textbox id="txtEmail" runat="server" width="350px" maxlength="250"></asp:textbox>
        </td>
    </tr>

    <tr>
        <td style="width: 20%;">Catatan
            :</td>
        <td>
            <asp:textbox id="txtCatatan" runat="server" width="350px" maxlength="250" height="117px"></asp:textbox>
        </td>
    </tr>
    <tr>
        <td colspan="2"></td>
    </tr>
    <tr>
        <td></td>
        <td colspan="2">
            <asp:button id="btnCreate" runat="server" text="Kemaskini" cssclass="fbbutton" />
            <asp:button id="btnBack" runat="server" text="Kembali" cssclass="fbbutton" />
            &nbsp;</td>
    </tr>
</table>
<br />
<div class="info" id="divMsg" runat="server">
    <asp:label id="lblKolejID" runat="server" text="" visible="false"></asp:label>
    <asp:label id="lblMsg" runat="server" text="Mesej..."></asp:label>
</div>
