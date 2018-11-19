<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="admin.takwim.list.view.aspx.vb" Inherits="apkv_v2_user.admin_takwim_list_view" %>

<%@ Register Src="commoncontrol/takwim_list_uniform.ascx" TagName="takwim_list_uniform" TagPrefix="uc1" %>
<%@ Register Src="commoncontrol/takwim_list_sukan.ascx" TagName="takwim_list_sukan" TagPrefix="uc2" %>
<%@ Register src="commoncontrol/takwim_list_renang.ascx" tagname="takwim_list_renang" tagprefix="uc4" %>
<%@ Register Src="commoncontrol/koko_list_sukan_tempat.ascx" TagName="koko_list_sukan_tempat" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlContents.ClientID %>");
            var printWindow = window.open('', '', 'height=400,width=800');
            printWindow.document.write('<html><head><title>KOKOSystem-Takwim Kokurikulum</title>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table class="fbform">
        <tr class="fbform_bread">
            <td>Laporan>Senarai Takwim
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlContents" runat="server">
        &nbsp;<uc1:takwim_list_uniform ID="takwim_list_uniform1" runat="server" />
        &nbsp;<uc2:takwim_list_sukan ID="takwim_list_sukan1" runat="server" />
        &nbsp;<uc3:koko_list_sukan_tempat ID="koko_list_sukan_tempat1" runat="server" />
        &nbsp;<uc4:takwim_list_renang ID="takwim_list_renang1" runat="server" />
        
    </asp:Panel>
    &nbsp;
    <table class="fbform">
        <tr>
            <td>
                <asp:Button ID="btnPrint" runat="server" Text="Cetak  " CssClass="fbbutton" OnClientClick="return PrintPanel();" /></td>
        </tr>
    </table>


</asp:Content>
