<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="admin.pengumuman.list.aspx.vb" Inherits="apkv_v2_user.admin_pengumuman_list" %>

<%@ Register Src="commoncontrol/pengumuman_list.ascx" TagName="pengumuman_list" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="fbform">
        <tr class="fbform_bread">
            <td>Lain-Lain>Pengumuman
            </td>
        </tr>
    </table>
    <uc1:pengumuman_list ID="pengumuman_list1" runat="server" />
    &nbsp;
</asp:Content>
