<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="admin.takwim.view.aspx.vb" Inherits="apkv_v2_user.admin_takwim_view" %>

<%@ Register Src="commoncontrol/takwim_view.ascx" TagName="takwim_view" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="fbform">
        <tr class="fbform_bread">
            <td>Lain-Lain>Takwim>Tambah
            </td>
        </tr>
    </table>
    <uc1:takwim_view ID="takwim_view1" runat="server" />
    <table class="fbform">
        <tr>
            <td>
                <asp:Button ID="btnUpdate" runat="server" Text="Kemaskini " CssClass="fbbutton" />
                &nbsp;<asp:Button ID="btnDelete" runat="server" Text="Hapuskan " CssClass="fbbutton" />
                &nbsp;|&nbsp;<asp:LinkButton ID="lnkList" runat="server">Senarai Takwim</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="red"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
