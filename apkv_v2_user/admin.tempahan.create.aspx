<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="admin.tempahan.create.aspx.vb" Inherits="apkv_v2_user.admin_tempahan_create" %>

<%@ Register Src="commoncontrol/tempahan_create.ascx" TagName="tempahan_create" TagPrefix="uc1" %>
<%@ Register Src="commoncontrol/tempahandetail_list.ascx" TagName="tempahandetail_list" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="fbform">
        <tr class="fbform_bread">
            <td>Lain-Lain>Tempahan>Tambah
            </td>
        </tr>
    </table>
    <uc1:tempahan_create ID="tempahan_create1" runat="server" />
    &nbsp;
    <uc2:tempahandetail_list ID="tempahandetail_list1" runat="server" />
</asp:Content>
