﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="user_login.ascx.vb" Inherits="apkv_v2_user.user_login" %>
<%--<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2" style="font-family: Arial, Helvetica, sans-serif; font-size: large; font-style: normal;">Sistem sedang ditambahbaikan. Harap Maklum.
            <br />
            Sistem Aplikasi Pengurusan Pentaksiran Kolej Vokasional telah ditukar kepada pautan berikut: 
            <asp:HyperLink ID="HyperLink1" runat="server" ForeColor="Blue">http://apkv.moe.gov.my/</asp:HyperLink>
        </td>
    </tr>
</table>
<br --%>
<table class="fbform">
    <tr class="fbform_header">
        <td colspan="2">
            Login Screen
        </td>
    </tr>
    <tr>
        <td style="width: 80px;">
            <asp:Label ID="Label1" runat="server" Text="Login ID"></asp:Label>
        </td>
         <td>
            :<asp:TextBox ID="txtLoginID" runat="server" Text="" Width="200px" MaxLength="50"></asp:TextBox>&nbsp;*<br />
            <asp:Label ID="lbl15_instruction" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td>
            <asp:Label ID="Label2" runat="server" Text="Kata Laluan"></asp:Label>
        </td>
         <td>
            :<asp:TextBox ID="txtPwd" runat="server" TextMode="Password" Width="200px" MaxLength="50"></asp:TextBox>&nbsp;*<br />
            <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
         <td>
            &nbsp;
        </td>
         <td>
            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="fbbutton" />&nbsp;
        </td>
    </tr>
</table>
<div class="info" id="divMsg" runat="server">
    <asp:Label ID="lblMsg" runat="server" Text="Medan bertanda * adalah wajib diisi."></asp:Label></div>
