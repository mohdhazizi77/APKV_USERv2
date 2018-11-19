<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="kursus.popup.aspx.vb" Inherits="apkv_v2_user.kursus_popup1" %>

<%@ Register src="commoncontrol/kursus.popup.ascx" tagname="kursus" tagprefix="uc1" %>

<!DOCTYPE html>


<head runat="server">
    <title></title>
       <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 43px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="style1">
            <tr>
                <td colspan="2" style="font-size: large">
                 </td>
            </tr>
            <tr>
                <td class="style2">
                   
                </td>
                <td>
       <div>

           <uc1:kursus ID="kursus1" runat="server" />

       </div>
                </td>
            </tr>
        </table>
        <br />
    </div>
    </form>
</body>


