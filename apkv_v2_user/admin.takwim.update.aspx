<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="admin.takwim.update.aspx.vb" Inherits="apkv_v2_user.admin_takwim_update" %>

<%@ Register Src="commoncontrol/takwim_update.ascx" TagName="takwim_update" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="fbform">
        <tr class="fbform_bread">
            <td>Lain-Lain>Takwim>Kemaskini
            </td>
        </tr>
    </table>
    <uc1:takwim_update ID="takwim_update1" runat="server" />
</asp:Content>
