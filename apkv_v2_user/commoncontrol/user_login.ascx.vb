Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization

Partial Public Class user_login
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Session("LoginID") = ""
            Session("UserGroupCode") = ""

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogin.Click
        Try

            ''--login name
            strSQL = "Select LoginID,pwd from kpmkv_users where LoginID='" & oCommon.FixSingleQuotes(txtLoginID.Text) & "' AND StatusID='2' AND Pwd='" & oCommon.FixSingleQuotes(txtPwd.Text) & "'"
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT tbl_ctrl_usergroup.UserGroupCode FROM kpmkv_users  "
                strSQL += " LEFT OUTER JOIN tbl_ctrl_usergroup ON kpmkv_users.Usertype=tbl_ctrl_usergroup.UserGroup "
                strSQL += "WHERE LoginID='" & oCommon.FixSingleQuotes(txtLoginID.Text) & "' AND Pwd='" & oCommon.FixSingleQuotes(txtPwd.Text) & "'"
                strRet = oCommon.getFieldValueEx(strSQL)
                'Response.Write(strSQL)
                ''--get user info
                Dim ar_user_login As Array
                ar_user_login = strRet.Split("|")

                Session("UserGroupCode") = ar_user_login(0)
                Response.Cookies("kpmkv_loginid").Value = txtLoginID.Text
                If txtPwd.Text = "KV@1234lp" Then
                    Response.Redirect("user.change.password.aspx")
                Else
                    Response.Redirect("admin.login.success.aspx")
                End If

            Else
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Login ID atau Kata Laluan tidak sah!"
                'lblMsg.Text = "System Sedang Ditambahbaikan. Harap Maklum!"
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try

         
    End Sub
End Class