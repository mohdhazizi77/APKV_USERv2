Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class user_change_password
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub btnPwdUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPwdUpdate.Click

        ''--blank, new and verify
        If ValidatePage() = False Then
            Exit Sub
        End If

        ''-must be valid userid and password
        If isValidLogin() = False Then
            Exit Sub
        End If

        ' ''--cant reuse password
        'If isLoginPwdReuse() = True Then
        '    Exit Sub
        'End If

        Try
            ''--update new password
            strSQL = "UPDATE kpmkv_users SET Pwd='" & oCommon.FixSingleQuotes(txtNewPwd.Text) & "' WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
            strRet = oCommon.ExecuteSQL(strSQL)
            If Not strRet = "0" Then
                lblMsg.Text = strRet
                Exit Sub
            End If

            ' ''--add new password into historical table. user getnow format. used to check 30days password expiry.
            'strSQL = "INSERT user_login_pwd (LoginIDNo,LoginPwd,CreateDateTime) VALUES('" & Session("LoginIDNo") & "','" & oCommon.FixSingleQuotes(txtNewPwd.Text) & "','" & oCommon.getNow & "')"
            'strRet = oCommon.ExecuteSQL(strSQL)
            'If Not strRet = "0" Then
            '    lblMsg.Text = strRet
            '    Exit Sub
            'End If

            ''--update user currently login
            'strSQL = "UPDATE user_login SET IsLogin='N',LoginDate='N',SessionID='N' WHERE LoginIDNo='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
            'strRet = oCommon.ExecuteSQL(strSQL)
            'If Not strRet = "0" Then
            '    lblMsg.Text = "Logout Pengguna tidak berjaya " & strRet
            '    Exit Sub
            'End If


            lblMsg.Text = "KataLaluan berjaya dikemaskini. <a href='default.aspx'>Sila Login Kembali</a>"
        Catch ex As Exception

        End Try


    End Sub

    Private Function isValidLogin() As Boolean
        Dim strLoginPwd As String = oCommon.FixSingleQuotes(txtOldPwd.Text)

        ''--single login only. have to logout
        strSQL = "SELECT LoginID,Pwd FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "' AND Pwd='" & strLoginPwd & "'"

        If oCommon.isExist(strSQL) = True Then
            Return True
        Else
            lblMsg.Text = "KataLaluan Semasa Tidak Sah!"
            Return False
        End If

    End Function

    Private Function ValidatePage() As Boolean
        If txtOldPwd.Text.Length = 0 Then
            lblMsg.Text = "Sila isi KataLaluan Semasa!"
            txtOldPwd.Focus()
            Return False
        End If

        If txtNewPwd.Text.Length = 0 Then
            lblMsg.Text = "Sila isi KataLaluan Baru!"
            txtNewPwd.Focus()
            Return False
        End If

        If txtVerPwd.Text.Length = 0 Then
            lblMsg.Text = "Sila isi KataLaluan Pengesahan !"
            txtVerPwd.Focus()
            Return False
        End If

        If Not txtNewPwd.Text = txtVerPwd.Text Then
            lblMsg.Text = "KataLaluan Baru dan KataLaluan Pengesahan tidak sama.Sila isi kembali!"
            txtVerPwd.Focus()
            Return False
        End If

        If txtNewPwd.Text = Session("LoginID") Then
            lblMsg.Text = "KataLaluan harus berbeza dari LoginID. Sila isi kembali"
            txtNewPwd.Focus()
            Return False
        End If

        '* Length is at least 8.
        '* Has letters uppercased and lowercased.
        '* Has numbers.
        '* Has symbols.
        Dim nStrength As Integer = getPolicyMeter(txtNewPwd.Text)
        If nStrength < 3 Then
            lblMsg.Text = "Password policy. [1]Length is at least 8. [2]Has letters uppercased and lowercased [3]Has numbers - Strength:" & nStrength.ToString

            txtNewPwd.Focus()
            Return False
        End If

        Return True
    End Function


    'Private Function isLoginPwdReuse() As Boolean
    '    ''after 7try you can re-use the previous password

    '    strSQL = "SELECT LoginID FROM user_login_pwd WHERE LoginIDNo='" & Session("LoginID") & "' AND LoginPwd='" & oCommon.FixSingleQuotes(txtNewPwd.Text) & "'"
    '    If oCommon.isExist(strSQL) = True Then
    '        lblMsg.Text = "Password have been used before."
    '        Return True
    '    End If

    '    Return False
    'End Function

    Private Function getPolicyMeter(ByVal password As String) As Integer
        Dim score As Integer = 0

        If (password.Length > 5) Then score += 1 'Length more than 6

        If System.Text.RegularExpressions.Regex.IsMatch(password, "[a-z]") And System.Text.RegularExpressions.Regex.IsMatch(password, "[A-Z]") Then
            score += 1 'upper and lower case
        End If

        If System.Text.RegularExpressions.Regex.IsMatch(password, "\d+") Then
            score += 1 'number
        End If

        'If System.Text.RegularExpressions.Regex.IsMatch(password, "[!,@,#,$,%,^,&,*,?,_,~,-,/""]") Then
        '    score += 1 'special character
        'End If

        Return score

    End Function
End Class