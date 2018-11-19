<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="admin.takwim.create.aspx.vb" Inherits="apkv_v2_user.admin_takwim_create" %>

<%@ Register Src="commoncontrol/takwim_create.ascx" TagName="takwim_create" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="fbform">
        <tr class="fbform_bread">
            <td>Lain-Lain>Takwim>Tambah
            </td>
        </tr>
    </table>
    <uc1:takwim_create ID="takwim_create1" runat="server" />
</asp:Content>
