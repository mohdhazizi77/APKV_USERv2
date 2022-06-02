﻿Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization

Public Class pelajar_list_kemaskini
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""
    Dim IntTakwim As Integer = 0

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblMsg.Text = ""
        Try
            If Not IsPostBack Then

                Dim PelajarID As String = Request.QueryString("id")

                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                strSQL = "SELECT Tahun FROM kpmkv_pelajar WHERE PelajarID = '" & PelajarID & "'"
                Dim Tahun As String = oCommon.getFieldValue(strSQL)

                kpmkv_tahun_list()
                ddlTahun.Text = Tahun

                strSQL = "SELECT Semester FROM kpmkv_pelajar WHERE PelajarID = '" & PelajarID & "'"
                Dim Semester As String = oCommon.getFieldValue(strSQL)

                kpmkv_semester_list()
                ddlSemester.Text = Semester

                chkSesi.SelectedIndex = 0

                strSQL = "SELECT Kaum FROM kpmkv_pelajar WHERE PelajarID = '" & PelajarID & "'"
                Dim Kaum As String = oCommon.getFieldValue(strSQL)

                kpmkv_kaum_list()
                ddlKaum.Text = Kaum

                strSQL = "SELECT KursusID FROM kpmkv_pelajar WHERE PelajarID = '" & PelajarID & "'"
                Dim KursusID As String = oCommon.getFieldValue(strSQL)

                strSQL = "SELECT KlusterID FROM kpmkv_kursus WHERE KursusID = '" & KursusID & "'"
                Dim KlusterID As String = oCommon.getFieldValue(strSQL)

                kpmkv_kluster_list()
                ddlKluster.SelectedValue = KlusterID

                kpmkv_kodkursus_list()
                ddlKodKursus.SelectedValue = KursusID

                strSQL = "SELECT KelasID FROM kpmkv_pelajar WHERE PelajarID = '" & PelajarID & "'"
                Dim KelasID As String = oCommon.getFieldValue(strSQL)

                kpmkv_kelas_list()
                ddlNamaKelas.SelectedValue = KelasID

                strSQL = "SELECT Nama FROM kpmkv_pelajar WHERE PelajarID = '" & PelajarID & "'"
                Dim NamaCalon As String = oCommon.getFieldValue(strSQL)

                txtNama.Text = NamaCalon

                strSQL = "SELECT MYKAD FROM kpmkv_pelajar WHERE PelajarID = '" & PelajarID & "'"
                Dim MYKAD As String = oCommon.getFieldValue(strSQL)

                txtMYKAD.Text = MYKAD

                strSQL = "SELECT Jantina FROM kpmkv_pelajar WHERE PelajarID = '" & PelajarID & "'"
                Dim Jantina As String = oCommon.getFieldValue(strSQL)

                If Jantina = "LELAKI" Then

                    chkJantina.SelectedIndex = 0

                Else

                    chkJantina.SelectedIndex = 1

                End If

                strSQL = "SELECT Agama FROM kpmkv_pelajar WHERE PelajarID = '" & PelajarID & "'"
                Dim Agama As String = oCommon.getFieldValue(strSQL)

                Agama = Agama.Trim

                If Agama = "ISLAM" Then

                    chkAgama.SelectedIndex = 0

                Else

                    chkAgama.SelectedIndex = 1

                End If

                strSQL = "SELECT Email FROM kpmkv_pelajar WHERE PelajarID = '" & PelajarID & "'"
                Dim Email As String = oCommon.getFieldValue(strSQL)

                txtEmail.Text = Email

                strSQL = "SELECT Catatan FROM kpmkv_pelajar WHERE PelajarID = '" & PelajarID & "'"
                Dim Catatan As String = oCommon.getFieldValue(strSQL)

                txtCatatan.Text = Catatan




            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try
    End Sub
    Private Sub kpmkv_tahun_list()
        strSQL = "SELECT Tahun FROM kpmkv_tahun  ORDER BY Tahun"
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

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Sub kpmkv_semester_list()
        strSQL = "SELECT Semester FROM kpmkv_semester  WHERE Semester='1'"
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

        Catch ex As Exception

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Sub kpmkv_kaum_list()
        strSQL = "SELECT Kaum FROM kpmkv_kaum  ORDER BY Kaum"
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
    Private Sub kpmkv_kluster_list()
        strSQL = "SELECT kpmkv_kluster.NamaKluster, kpmkv_kluster.KlusterID FROM  kpmkv_kursus_kolej"
        strSQL += " LEFT OUTER JOIN kpmkv_kursus ON kpmkv_kursus_kolej.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
        strSQL += " kpmkv_kluster ON kpmkv_kursus.KlusterID = kpmkv_kluster.KlusterID"
        strSQL += " WHERE kpmkv_kursus.Tahun ='" & ddlTahun.SelectedValue & "' AND kpmkv_kursus.Sesi='" & chkSesi.Text & "' GROUP BY kpmkv_kluster.NamaKluster,  kpmkv_kluster.KlusterID"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlKluster.DataSource = ds
            ddlKluster.DataTextField = "NamaKluster"
            ddlKluster.DataValueField = "KlusterID"
            ddlKluster.DataBind()

            '--ALL
            ddlKluster.Items.Add(New ListItem("-Pilih-", "0"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_kodkursus_list()

        strSQL = "SELECT kpmkv_kursus.KodKursus, kpmkv_kursus.KursusID FROM kpmkv_kursus_kolej LEFT OUTER JOIN"
        strSQL += " kpmkv_kursus ON kpmkv_kursus_kolej.KursusID = kpmkv_kursus.KursusID"
        strSQL += " WHERE kpmkv_kursus_kolej.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kursus.Tahun='" & ddlTahun.SelectedValue & "' AND kpmkv_kursus.Sesi='" & chkSesi.SelectedValue & "' AND KlusterID='" & ddlKluster.SelectedValue & "'"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlKodKursus.DataSource = ds
            ddlKodKursus.DataTextField = "KodKursus"
            ddlKodKursus.DataValueField = "KursusID"
            ddlKodKursus.DataBind()

            '--ALL
            ddlKodKursus.Items.Add(New ListItem("-Pilih-", "0"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_kelas_list()
        strSQL = " SELECT kpmkv_kelas.NamaKelas, kpmkv_kelas.KelasID FROM kpmkv_kelas_kursus LEFT OUTER JOIN kpmkv_kelas ON kpmkv_kelas_kursus.KelasID = kpmkv_kelas.KelasID LEFT OUTER JOIN"
        strSQL += " kpmkv_kursus ON kpmkv_kelas_kursus.KursusID = kpmkv_kursus.KursusID"
        strSQL += " WHERE kpmkv_kelas.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kelas_kursus.KursusID= '" & ddlKodKursus.SelectedValue & "' AND kpmkv_kursus.Tahun= '" & ddlTahun.SelectedValue & "' ORDER BY  kpmkv_kelas.NamaKelas"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)
        ' Response.Write(strSQL)
        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlNamaKelas.DataSource = ds
            ddlNamaKelas.DataTextField = "NamaKelas"
            ddlNamaKelas.DataValueField = "KelasID"
            ddlNamaKelas.DataBind()

            '--ALL
            ddlNamaKelas.Items.Add(New ListItem("-Pilih-", "0"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Function GetData(ByVal cmd As SqlCommand) As DataTable
        Dim dt As New DataTable()
        Dim strConnString As [String] = ConfigurationManager.AppSettings("ConnectionString")
        Dim con As New SqlConnection(strConnString)
        Dim sda As New SqlDataAdapter()
        cmd.CommandType = CommandType.Text
        cmd.Connection = con
        Try
            con.Open()
            sda.SelectCommand = cmd
            sda.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
        End Try
        con.Close()
        sda.Dispose()
        con.Dispose()

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
            If kpmkv_pelajar_create() = True Then
                divMsg.Attributes("class") = "info"
                lblMsg.Text = "Berjaya!. Mengemaskini Calon."
            Else
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Tidak Berjaya!. Mengemaskini Calon."
            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try

    End Sub
    Private Function ValidatePage() As Boolean


        '--ddlsesi
        If chkSesi.Text = "" Then
            lblMsg.Text = "Sila pilih Sesi!"
            chkSesi.Focus()
            Return False
        End If

        '--txtKluster
        If ddlKluster.Text.Length = 0 Then
            lblMsg.Text = "Sila Pilih Kluster!"
            ddlKluster.Focus()
            Return False
        End If

        '--txtNama
        If txtNama.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan Nama Calon!"
            txtNama.Focus()
            Return False
        End If

        '--txtMYKAD
        If txtMYKAD.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan MYKAD Calon!"
            txtMYKAD.Focus()
            Return False
        ElseIf oCommon.isMyKad2(txtMYKAD.Text) = False Then
            lblMsg.Text = "Huruf tidak dibenarkan .Sila masukkan no MYKAD [######06####]"
            txtMYKAD.Focus()
            Return False
        End If

        'strSQL = "SELECT MYKAD FROM kpmkv_pelajar WHERE MYKAD='" & txtMYKAD.Text & "'"
        'If oCommon.isExist(strSQL) = True Then
        '    lblMsg.Text = "MYKAD telah digunakan. Pendaftaran Pelajar tidak berjaya."
        '    Return False
        'End If

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

        '--txtEmail
        If txtEmail.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan Email Calon!"
        ElseIf oCommon.isEmail(txtEmail.Text) = False Then
            lblMsg.Text = "Emel Calon tidak sah. (Contoh: Emel@contoh.com)"
            Return False
        End If

        Return True
    End Function
    Private Function kpmkv_pelajar_create() As Boolean
        Dim strRecordID As String = oCommon.getGUID
        'create with isApproved='Y'

        strSQL = "UPDATE kpmkv_pelajar SET "
        strSQL += "KolejRecordID = '" & lblKolejID.Text & "',"
        strSQL += "KursusID = '" & ddlKodKursus.SelectedValue & "',"
        strSQL += "KelasID = '" & ddlNamaKelas.SelectedValue & "',"
        strSQL += "Tahun = '" & ddlTahun.Text & "',"
        strSQL += "Semester = '" & ddlSemester.Text & "',"
        strSQL += "Sesi = '" & chkSesi.SelectedValue & "',"
        strSQL += "Nama = '" & txtNama.Text.ToUpper & "',"
        strSQL += "MYKAD = '" & txtMYKAD.Text & "',"
        strSQL += "Jantina = '" & chkJantina.SelectedValue & "',"
        strSQL += "Kaum = '" & ddlKaum.Text & "',"
        strSQL += "Agama = '" & chkAgama.SelectedValue & "',"
        strSQL += "Email = '" & txtEmail.Text & "',"
        strSQL += "Catatan = '" & txtCatatan.Text & "'"
        strSQL += "WHERE PelajarID = '" & Request.QueryString("id") & "'"

        strRet = oCommon.ExecuteSQL(strSQL)

        'strSQL = "INSERT INTO kpmkv_pelajar (KolejRecordID,Pengajian,KursusID,KelasID,Tahun,Semester,Sesi,Nama,MYKAD,Jantina,Kaum,Agama,Email,Catatan,StatusID,JenisCalonID,IsDeleted)"
        'strSQL += "VALUES ('" & lblKolejID.Text & "','Pra Diploma','" & ddlKodKursus.SelectedValue & "','" & ddlNamaKelas.SelectedValue & "','" & ddlTahun.Text & "','1','" & chkSesi.SelectedValue & "','" & oCommon.FixSingleQuotes(txtNama.Text.ToUpper) & "','" & oCommon.FixSingleQuotes(txtMYKAD.Text.ToUpper) & "','" & chkJantina.SelectedValue & "','" & ddlKaum.Text & "','" & chkAgama.SelectedValue & "','" & oCommon.FixSingleQuotes(txtEmail.Text) & "','" & oCommon.FixSingleQuotes(txtCatatan.Text) & "','2','2','N')"
        'strRet = oCommon.ExecuteSQL(strSQL)
        If strRet = "0" Then

            'strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE Mykad='" & oCommon.FixSingleQuotes(txtMYKAD.Text.ToUpper) & "'"
            'Dim strPelajarID As Integer = oCommon.getFieldValue(strSQL)

            'strSQL = "INSERT INTO kpmkv_pelajar_markah (PelajarID,KolejRecordID,KursusID,Tahun,Semester,Sesi)"
            'strSQL += " VALUES ('" & strPelajarID & "','" & lblKolejID.Text & "','" & ddlKodKursus.SelectedValue & "','" & ddlTahun.Text & "','1','" & chkSesi.SelectedValue & "')"
            'strRet = oCommon.ExecuteSQL(strSQL)

            Return True
        Else
            lblMsg.Text = "System Error:" & strRet
            Return False
        End If

    End Function
    Private Sub chkSesi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkSesi.SelectedIndexChanged
        kpmkv_kluster_list()
        kpmkv_kodkursus_list()
        kpmkv_kelas_list()
    End Sub
    Protected Sub ddlKluster_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlKluster.SelectedIndexChanged
        kpmkv_kodkursus_list()
        kpmkv_kelas_list()
    End Sub
    Private Sub ddlKodKursus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlKodKursus.SelectedIndexChanged
        kpmkv_kelas_list()
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Response.Redirect("pelajar.list.aspx")
    End Sub
End Class