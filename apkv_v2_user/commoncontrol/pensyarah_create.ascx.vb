Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class pensyarah_create
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                kpmkv_kaum_list()


            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try
    End Sub
    Private Sub kpmkv_kaum_list()
        strSQL = "SELECT Kaum FROM kpmkv_kaum WITH (NOLOCK) ORDER BY Kaum"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlKaum.DataSource = ds
            ddlKaum.DataTextField = "Kaum"
            ddlKaum.DataValueField = "Kaum"
            ddlKaum.DataBind()

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Function ValidatePage() As Boolean
        '--txtNama
        If txtNama.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan Nama!"
            txtNama.Focus()
            Return False
        End If

        '--txtMYKAD
        If txtMYKAD.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan MYKAD Pensyarah!"
            txtMYKAD.Focus()
            Return False
        ElseIf oCommon.isMyKad2(txtMYKAD.Text) = False Then
            lblMsg.Text = "Huruf tidak dibenarkan .Sila masukkan no MYKAD [############]"
            txtMYKAD.Focus()
            Return False
        End If

        strSQL = "SELECT MYKAD FROM kpmkv_pensyarah WHERE MYKAD='" & txtMYKAD.Text & "'"
        If oCommon.isExist(strSQL) = True Then
            lblMsg.Text = "MYKAD telah digunakan. Pendaftaran Pensyarah tidak berjaya."
            Return False
        End If

        '--txtJawatan
        If txtJawatan.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan jawatan!"
            txtJawatan.Focus()
            Return False
        End If

        '--txtGred
        If txtGred.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan Gred!"
            txtGred.Focus()
            Return False
        End If

        '--txtTelefon
        If txtTel.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan Telefon!"
            txtTel.Focus()
        End If

        '--txtEmail
        If txtEmail.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan Email Pensyarah!"
        ElseIf oCommon.isEmail(txtEmail.Text) = False Then
            lblMsg.Text = "Emel Pensyarah tidak sah. (Contoh: Pensyarah@contoh.com)"
            txtEmail.Focus()
            Return False
        End If

        '--ddlAgama
        If chkAgama.Text = "" Then
            lblMsg.Text = "Sila pilih jenis Agama!"
            chkAgama.Focus()
            Return False
        End If

        '--ddlJantina
        If chkJantina.Text = "" Then
            lblMsg.Text = "Sila pilih jenis Jantina!"
            chkJantina.Focus()
            Return False
        End If

        Return True
    End Function

    Protected Sub btnCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCreate.Click
        lblMsg.Text = ""

        Try
            '--validate
            If ValidatePage() = False Then
                divMsg.Attributes("class") = "error"
                Exit Sub
            End If

            '--execute
            If kpmkv_pensyarah_create() = True Then
                divMsg.Attributes("class") = "info"
                lblMsg.Text = "Berjaya!. Mendaftarkan pensyarah baru."
            Else
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Tidak Berjaya!. Mendaftarkan pensyarah baru."

            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try

    End Sub

    Private Function kpmkv_pensyarah_create() As Boolean
        strSQL = "INSERT INTO kpmkv_pensyarah (KolejRecordID,MYKAD,Nama,Jawatan,Gred,Tel,Email,Jantina,Kaum,Agama,StatusID,IsDeleted) "
        strSQL += "VALUES ('" & lblKolejID.Text & "','" & oCommon.FixSingleQuotes(txtMYKAD.Text) & "','" & oCommon.FixSingleQuotes(txtNama.Text.ToUpper) & "','" & oCommon.FixSingleQuotes(txtJawatan.Text.ToUpper) & "','" & oCommon.FixSingleQuotes(txtGred.Text.ToUpper) & "','" & oCommon.FixSingleQuotes(txtTel.Text) & "','" & oCommon.FixSingleQuotes(txtEmail.Text) & "','" & chkJantina.Text & "','" & ddlKaum.Text & "','" & chkAgama.Text & "','2','N')"
        strRet = oCommon.ExecuteSQL(strSQL)

        '-1.get nama Kolej in tbl kolej-'
        strSQL = "SELECT Nama FROM kpmkv_kolej WHERE RecordID='" & oCommon.FixSingleQuotes(lblKolejID.Text) & "'"
        Dim strNamaKolej As String = oCommon.getFieldValue(strSQL)
        '-2.get negeri Kolej in tbl kolej-'
        strSQL = "SELECT Negeri FROM kpmkv_kolej WHERE RecordID='" & oCommon.FixSingleQuotes(lblKolejID.Text) & "'"
        Dim strNegeriKolej As String = oCommon.getFieldValue(strSQL)

        ''-- create if user not exist in tbl user-'
        strSQL = " INSERT INTO kpmkv_users(RecordID,LoginID,Pwd,UserType,Nama,MyKad,Tel,Email,StatusID,Negeri)"
        strSQL += " VALUES('" & oCommon.FixSingleQuotes(lblKolejID.Text) & "','" & oCommon.FixSingleQuotes(txtMYKAD.Text) & "','pwd',"
        strSQL += " 'PENSYARAH','" & oCommon.FixSingleQuotes(txtNama.Text.ToUpper) & "','" & oCommon.FixSingleQuotes(txtMYKAD.Text) & "',"
        strSQL += " '" & oCommon.FixSingleQuotes(txtTel.Text) & "','" & oCommon.FixSingleQuotes(txtEmail.Text) & "',"
        strSQL += " '2','" & oCommon.FixSingleQuotes(strNegeriKolej) & "')"
        strRet = oCommon.ExecuteSQL(strSQL)
        Return True
    End Function
End Class