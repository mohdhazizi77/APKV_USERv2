Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class matapelajaran_create
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then

                kpmkv_tahun_list()
                ddlTahun.Text = Now.Year


                kpmkv_semester_list()
                ddlSemester.Text = "1"

                kpmkv_sesi_list()
                ddlSesi.Text = "1"
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try
    End Sub
    Private Sub kpmkv_tahun_list()
        strSQL = "SELECT Tahun FROM kpmkv_tahun WITH (NOLOCK) ORDER BY TahunID"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlTahun.DataSource = ds
            ddlTahun.DataTextField = "Tahun"
            ddlTahun.DataValueField = "Tahun"
            ddlTahun.DataBind()

            '--ALL
            ddlTahun.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_semester_list()
        strSQL = "SELECT Semester FROM kpmkv_semester WITH (NOLOCK) ORDER BY Semester"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlSemester.DataSource = ds
            ddlSemester.DataTextField = "Semester"
            ddlSemester.DataValueField = "Semester"
            ddlSemester.DataBind()

            '--ALL
            ddlSemester.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_sesi_list()
        strSQL = "SELECT Sesi FROM kpmkv_sesi WITH (NOLOCK) ORDER BY Sesi"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlSesi.DataSource = ds
            ddlSesi.DataTextField = "Sesi"
            ddlSesi.DataValueField = "Sesi"
            ddlSesi.DataBind()


        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Protected Sub btnCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCreate.Click
        lblMsg.Text = ""

        Try
            '--validate
            If ValidatePage() = False Then
                divMsg.Attributes("class") = "error"
                Exit Sub
            End If

            '--execute
            If kpmkv_matapelajaran_create() = True Then
                divMsg.Attributes("class") = "info"
                lblMsg.Text = "Berjaya mendaftarkan MataPelajaran baru."
            Else
                divMsg.Attributes("class") = "error"
            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try

    End Sub
    Private Function ValidatePage() As Boolean

        '--txtNama
        If ddlSemester.Text = "PILIH" Then
            lblMsg.Text = "Sila pilih Semester!"
            ddlSemester.Text = ""
            Return False
        End If

        '--txtNama
        If txtNamaMataPelajaran.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan Nama Nama MataPelajaran!"
            txtNamaMataPelajaran.Focus()
            Return False
        End If

        '--txtKod
        If txtKodMataPelajaran.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan Kod MataPelajaran!"
            txtKodMataPelajaran.Focus()
            Return False
        End If

        strSQL = "SELECT * FROM kpmkv_matapelajaran WHERE KodMataPelajaran='" & txtKodMataPelajaran.Text & "' and NamaMataPelajaran='" & txtNamaMataPelajaran.Text & "' and IsDeleted='N'"
        If oCommon.isExist(strSQL) = True Then
            lblMsg.Text = "Kod MataPelajaran telah digunakan. Sila masukkan kod yang baru."
            Return False
        Else
            Return True
        End If

        Return True
    End Function
    Private Function kpmkv_matapelajaran_create() As Boolean

        strSQL = "INSERT INTO kpmkv_matapelajaran (Tahun,Semester,Sesi,NamaKluster,KodKursus,NamaKursus,KodMataPelajaran,NamaMataPelajaran,,JamKredit,IsDeleted) "
        strSQL += "VALUES ('" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & ddlSesi.Text & "','NULL','NULL','NULL','" & oCommon.FixSingleQuotes(txtKodMataPelajaran.Text.ToUpper) & "','" & oCommon.FixSingleQuotes(txtNamaMataPelajaran.Text.ToUpper) & "','" & txtJamKredit.Text & "','N')"
        strRet = oCommon.ExecuteSQL(strSQL)
        If strRet = "0" Then
            Return True
        End If
        Return True
    End Function
    Protected Sub lnkList_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkList.Click
        Response.Redirect("matapelajaran.search.aspx")

    End Sub
End Class