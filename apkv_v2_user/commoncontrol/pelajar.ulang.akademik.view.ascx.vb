Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class peajar_ulang_akademik_view
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

                kpmkv_tahun_list()
                ddlTahun.Text = Now.Year
                kpmkv_semester_list()

                kpmkv_kodkursus_list()

                kpmkv_kelas_list()

                lblMsg.Text = ""
                LoadPage()
                LoadPageAkademikGred()
                HiddenPBPACheckBox()
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try

    End Sub
    Private Sub kpmkv_tahun_list()
        strSQL = "SELECT Tahun FROM kpmkv_tahun WITH (NOLOCK) ORDER BY Tahun"
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

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_kodkursus_list()

        strSQL = "SELECT kpmkv_kursus.KodKursus, kpmkv_kursus.KursusID"
        strSQL += " FROM kpmkv_kelas_kursus INNER JOIN kpmkv_kursus ON kpmkv_kelas_kursus.KursusID = kpmkv_kursus.KursusID INNER JOIN"
        strSQL += " kpmkv_kelas ON kpmkv_kelas_kursus.KelasID = kpmkv_kelas.KelasID"
        strSQL += " WHERE kpmkv_kelas.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kursus.Tahun='" & ddlTahun.Text & "'"
        strSQL += " AND kpmkv_kursus.Sesi='" & chkSesi.SelectedValue & "' GROUP BY kpmkv_kursus.KodKursus,kpmkv_kursus.KursusID"
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
            ' ddlKodKursus.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_kelas_list()
        strSQL = " SELECT kpmkv_kelas.NamaKelas, kpmkv_kelas.KelasID"
        strSQL += " FROM  kpmkv_kelas_kursus LEFT OUTER JOIN kpmkv_kelas ON kpmkv_kelas_kursus.KelasID = kpmkv_kelas.KelasID LEFT OUTER JOIN"
        strSQL += " kpmkv_kursus ON kpmkv_kelas_kursus.KursusID = kpmkv_kursus.KursusID"
        strSQL += " WHERE kpmkv_kelas.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kelas_kursus.KursusID= '" & ddlKodKursus.SelectedValue & "' ORDER BY KelasID"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlNamaKelas.DataSource = ds
            ddlNamaKelas.DataTextField = "NamaKelas"
            ddlNamaKelas.DataValueField = "KelasID"
            ddlNamaKelas.DataBind()

            '--ALL
            'ddlNamaKelas.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Sub LoadPage()
        strSQL = "SELECT kpmkv_pelajar.Pengajian, kpmkv_pelajar.Tahun, kpmkv_pelajar.Semester, kpmkv_pelajar.Sesi, kpmkv_pelajar.Nama, kpmkv_pelajar.MYKAD, kpmkv_pelajar.AngkaGiliran, "
        strSQL += " kpmkv_kursus.KursusID, kpmkv_kursus.KodKursus, kpmkv_kursus.NamaKursus, kpmkv_kluster.NamaKluster, kpmkv_kelas.NamaKelas"
        strSQL += " FROM kpmkv_pelajar LEFT OUTER JOIN kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
        strSQL += " kpmkv_kluster ON kpmkv_kursus.KlusterID = kpmkv_kluster.KlusterID LEFT OUTER JOIN"
        strSQL += " kpmkv_kelas ON kpmkv_pelajar.KelasID = kpmkv_kelas.KelasID"
        strSQL += " WHERE kpmkv_pelajar.PelajarID='" & Request.QueryString("PelajarID") & "'"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            Dim nRows As Integer = 0
            Dim nCount As Integer = 1
            Dim MyTable As DataTable = New DataTable
            MyTable = ds.Tables(0)
            If MyTable.Rows.Count > 0 Then

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Pengajian")) Then
                    lblPengajian.Text = ds.Tables(0).Rows(0).Item("Pengajian")
                Else
                    lblPengajian.Text = ""
                End If

                '--Account Details 
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Tahun")) Then
                    lblTahun.Text = ds.Tables(0).Rows(0).Item("Tahun")
                Else
                    lblTahun.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Sesi")) Then
                    lblSesi.Text = ds.Tables(0).Rows(0).Item("Sesi")
                Else
                    lblSesi.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Semester")) Then
                    lblSemester.Text = ds.Tables(0).Rows(0).Item("Semester")
                Else
                    lblSemester.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("NamaKluster")) Then
                    lblKluster.Text = ds.Tables(0).Rows(0).Item("NamaKluster")
                Else
                    lblKluster.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("NamaKursus")) Then
                    lblNamaKursus.Text = ds.Tables(0).Rows(0).Item("NamaKursus")
                Else
                    lblNamaKursus.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("KodKursus")) Then
                    lblKodKursus.Text = ds.Tables(0).Rows(0).Item("KodKursus")
                Else
                    lblKodKursus.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("KursusID")) Then
                    lblKursusID.Text = ds.Tables(0).Rows(0).Item("KursusID")
                Else
                    lblKursusID.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("NamaKelas")) Then
                    lblNamaKelas.Text = ds.Tables(0).Rows(0).Item("NamaKelas")
                Else
                    lblNamaKelas.Text = ""
                End If
                'personal
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Nama")) Then
                    lblNama.Text = ds.Tables(0).Rows(0).Item("Nama")
                Else
                    lblNama.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Mykad")) Then
                    lblMYKAD.Text = ds.Tables(0).Rows(0).Item("Mykad")
                Else
                    lblMYKAD.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("AngkaGiliran")) Then
                    lblAngkaGiliran.Text = ds.Tables(0).Rows(0).Item("AngkaGiliran")
                Else
                    lblAngkaGiliran.Text = ""
                End If

                '--
            End If
        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub LoadPageAkademikGred()

        strSQL = "SELECT  kpmkv_pelajar_markah.GredBM, kpmkv_pelajar_markah.GredBI, kpmkv_pelajar_markah.GredSJ, "
        strSQL += " kpmkv_pelajar_markah.GredSC, kpmkv_pelajar_markah.GredPI, kpmkv_pelajar_markah.GredPM, kpmkv_pelajar_markah.GredMT"
        strSQL += " FROM kpmkv_pelajar_markah"
        strSQL += " WHERE kpmkv_pelajar_markah.PelajarID='" & Request.QueryString("PelajarID") & "'"
        strSQL += " AND kpmkv_pelajar_markah.Tahun='" & lblTahun.Text & "'"
        strSQL += " AND kpmkv_pelajar_markah.Semester='" & lblSemester.Text & "'"
        strSQL += " AND kpmkv_pelajar_markah.Sesi='" & lblSesi.Text & "'"

        ''--debug
        ''Response.Write(getSQL)
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            Dim nRows As Integer = 0
            Dim nCount As Integer = 1
            Dim MyTable As DataTable = New DataTable
            MyTable = ds.Tables(0)
            If MyTable.Rows.Count > 0 Then

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("GredBM")) Then
                    lblBM.Text = ds.Tables(0).Rows(0).Item("GredBM")
                Else
                    lblBM.Text = ""
                End If

                '--Account Details 
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("GredBI")) Then
                    lblBI.Text = ds.Tables(0).Rows(0).Item("GredBI")
                Else
                    lblBI.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("GredSC")) Then
                    lblSC.Text = ds.Tables(0).Rows(0).Item("GredSC")
                Else
                    lblSC.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("GredSJ")) Then
                    lblSJ.Text = ds.Tables(0).Rows(0).Item("GredSJ")
                Else
                    lblSJ.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("GredPI")) Then
                    lblPI.Text = ds.Tables(0).Rows(0).Item("GredPI")
                Else
                    lblPI.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("GredPM")) Then
                    lblPM.Text = ds.Tables(0).Rows(0).Item("GredPM")
                Else
                    lblPM.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("GredMT")) Then
                    lblMT.Text = ds.Tables(0).Rows(0).Item("GredMT")
                Else
                    lblMT.Text = ""
                End If


                '--
            End If
        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try


    End Sub
    Private Sub ddlTahun_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTahun.SelectedIndexChanged
        kpmkv_kelas_list()
    End Sub
    '--CHECK form validation.
    Private Function ValidateForm() As Boolean
        lblMsg.Text = ""

        If ddlTahun.Text = "PILIH" Then
            lblMsg.Text = "Sila pilih Tahun."
            ddlTahun.Focus()
            Return False
        End If

        If ddlTahun.Text >= lblTahun.Text Then
        Else
            lblMsg.Text = "Sila pilih Tahun kehadapan."
            ddlTahun.Focus()
            Return False
        End If

        If chkSesi.Text = " Then" Then
            lblMsg.Text = "Sila Tick() pada kotak yang Sesi."
            chkSesi.Focus()
            Return False
        End If

        If ddlNamaKelas.Text = "PILIH" Then
            lblMsg.Text = "Sila pilih Kelas."
            ddlNamaKelas.Focus()
            Return False
        End If

        Return True
    End Function

    Protected Sub btnApprove_Click(sender As Object, e As EventArgs) Handles btnApprove.Click
        lblMsg.Text = ""
        'check form validation. if failed exit
        If ValidateForm() = False Then
            Exit Sub
        End If

        strSQL = "SELECT KursusID from kpmkv_kursus WHERE Tahun='" & lblTahun.Text & "'"
        strSQL += " AND Sesi='" & lblSesi.Text & "' AND KodKursus='" & lblKodKursus.Text & "'"
        Dim StrKodKursusID As String = oCommon.getFieldValue(strSQL)

        If PB1.Checked = True Then
            'get markah
            strSQL = "SELECT A_BahasaMelayu FROM kpmkv_pelajar_markah WHERE PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            Dim strA_BahasaMelayu As String = oCommon.getFieldValue(strSQL)
            '----------------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaMataPelajaran='BAHASA MELAYU' AND PB IS NOT NULL"
            If oCommon.isExist(strSQL) = True Then
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Calon ini telah mengulang bagi Matapelajaran BAHASA MELAYU"
                Exit Sub
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaMataPelajaran, PB, MarkahPA)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','BAHASA MELAYU','1','" & strA_BahasaMelayu & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
        End If

        'PA
        If PA1.Checked = True Then
            'get markah
            strSQL = "SELECT B_BahasaMelayu FROM kpmkv_pelajar_markah WHERE PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            Dim strB_BahasaMelayu As String = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaMataPelajaran='BAHASA MELAYU' AND PA IS NOT NULL"
            If oCommon.isExist(strSQL) = True Then
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Calon ini telah mengulang bagi Matapelajaran BAHASA MELAYU"
                Exit Sub
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaMataPelajaran, PA,MarkahPB)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','BAHASA MELAYU','1', '" & strB_BahasaMelayu & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
        End If
        'BM----------------------------------------------------------------------------------------------------------------------------------

        If PB2.Checked = True Then
            'get markah
            strSQL = "SELECT A_BahasaInggeris FROM kpmkv_pelajar_markah WHERE PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            Dim strA_BahasaInggeris As String = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaMataPelajaran='BAHASA INGGERIS' AND PB IS NOT NULL"
            If oCommon.isExist(strSQL) = True Then
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Calon ini telah mengulang bagi Matapelajaran BAHASA INGGERIS"
                Exit Sub
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaMataPelajaran, PB, MarkahPA)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','BAHASA INGGERIS','1','" & strA_BahasaInggeris & "' )"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
        End If

        If PA2.Checked = True Then
            'get markah
            strSQL = "SELECT B_BahasaInggeris FROM kpmkv_pelajar_markah WHERE PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            Dim strB_BahasaInggeris As String = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaMataPelajaran='BAHASA INGGERIS' AND PA IS NOT NULL"
            If oCommon.isExist(strSQL) = True Then
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Calon ini telah mengulang bagi Matapelajaran BAHASA INGGERIS"
                Exit Sub
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaMataPelajaran, PA, MarkahPB)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','BAHASA INGGERIS','1','" & strB_BahasaInggeris & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
        End If
        'BI----------------------------------------------------------------------------------------------------------------------------

        If PB3.Checked = True Then
            'get markah
            strSQL = "SELECT A_Mathematics FROM kpmkv_pelajar_markah WHERE PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            Dim strA_Mathematics As String = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaMataPelajaran='MATEMATIK' AND PB IS NOT NULL"
            If oCommon.isExist(strSQL) = True Then
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Calon ini telah mengulang bagi Matapelajaran MATEMATIK"
                Exit Sub
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaMataPelajaran, PB, MarkahPA)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','MATEMATIK','1','" & strA_Mathematics & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
        End If

        If PA3.Checked = True Then
            'get markah
            strSQL = "SELECT B_Mathematics FROM kpmkv_pelajar_markah WHERE PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            Dim strB_Mathematics As String = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaMataPelajaran='MATEMATIK' AND PA IS NOT NULL "
            If oCommon.isExist(strSQL) = True Then
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Calon ini telah mengulang bagi Matapelajaran MATEMATIK"
                Exit Sub
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaMataPelajaran,PA, MarkahPB)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','MATEMATIK','1','" & strB_Mathematics & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
        End If
        'MAT------------------------------------------------------------------------------------------------------------------------------------

        If PB4.Checked = True Then
            'get markah
            strSQL = "SELECT A_Science1 FROM kpmkv_pelajar_markah WHERE PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            Dim strA_Science1 As String = oCommon.getFieldValue(strSQL)

            'get markah
            strSQL = "SELECT A_Science2 FROM kpmkv_pelajar_markah WHERE PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            Dim strA_Science2 As String = oCommon.getFieldValue(strSQL)

            Dim AM_Science2 As Integer
            Dim AM_Science1 As Integer

            If Not String.IsNullOrEmpty(strA_Science1) Then
                AM_Science1 = oCommon.DoConvertN(((strA_Science1 / 100) * 50), 0) '50%
            End If

            If Not String.IsNullOrEmpty(strA_Science2) Then
                AM_Science2 = oCommon.DoConvertN(((strA_Science2 / 100) * 20), 0) '20%
            End If

            Dim strA_MarkahSC2 As Integer = CInt(AM_Science1) + CInt(AM_Science2)

            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaMataPelajaran='SAINS' AND PB IS NOT NULL"
            If oCommon.isExist(strSQL) = True Then
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Calon ini telah mengulang bagi Matapelajaran SAINS"
                Exit Sub
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaMataPelajaran, PB, MarkahPA)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','SAINS','1','" & strA_MarkahSC2 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
        End If

        If PA4.Checked = True Then

            'get markah
            'change on 06092016
            strSQL = "SELECT B_Science1 FROM kpmkv_pelajar_markah WHERE PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            Dim strB_Science1 As String = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaMataPelajaran='SAINS' AND PA IS NOT NULL"
            If oCommon.isExist(strSQL) = True Then
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Calon ini telah mengulang bagi Matapelajaran SAINS"
                Exit Sub
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaMataPelajaran, PA,MarkahPB)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','SAINS','1', '" & strB_Science1 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
        End If
        'SCIENCE---------------------------------------------------------------------

        If PB5.Checked = True Then
            'get markah
            strSQL = "SELECT A_Sejarah FROM kpmkv_pelajar_markah WHERE PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            Dim strA_Sejarah As String = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaMataPelajaran='SEJARAH' AND PB IS NOT NULL"
            If oCommon.isExist(strSQL) = True Then
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Calon ini telah mengulang bagi Matapelajaran SEJARAH"
                Exit Sub
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaMataPelajaran, PB, MarkahPA)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','SEJARAH','1', '" & strA_Sejarah & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
        End If

        If PA5.Checked = True Then

            'get markah
            strSQL = "SELECT B_Sejarah FROM kpmkv_pelajar_markah WHERE PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            Dim strB_Sejarah As String = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaMataPelajaran='SEJARAH' AND PA IS NOT NULL"
            If oCommon.isExist(strSQL) = True Then
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Calon ini telah mengulang bagi Matapelajaran SEJARAH"
                Exit Sub
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaMataPelajaran, PA, MarkahPB)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','SEJARAH','1','" & strB_Sejarah & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
        End If
        'SEJARAH----------------------------------------------------------------------------------

        If PB6.Checked = True Then
            'get markah
            strSQL = "SELECT A_PendidikanIslam1 FROM kpmkv_pelajar_markah WHERE PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            Dim strA_PendidikanIslam1 As String = oCommon.getFieldValue(strSQL)

            'get markah
            strSQL = "SELECT A_PendidikanIslam2 FROM kpmkv_pelajar_markah WHERE PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            Dim strA_PendidikanIslam2 As String = oCommon.getFieldValue(strSQL)

            If Not String.IsNullOrEmpty(strA_PendidikanIslam1) Then
                strA_PendidikanIslam1 = oCommon.DoConvertN(((strA_PendidikanIslam1 / 100) * 50), 0) '50%
            End If

            If Not String.IsNullOrEmpty(strA_PendidikanIslam2) Then
                strA_PendidikanIslam2 = oCommon.DoConvertN(((strA_PendidikanIslam2 / 100) * 20), 0) '20%
            End If

            Dim strA_Markah_PendidikanIslam2 As Integer = CInt(strA_PendidikanIslam1) + CInt(strA_PendidikanIslam2)

            ''get markah
            'strSQL = "SELECT B_PendidikanIslam1 FROM kpmkv_pelajar_markah WHERE PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            'strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            'strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            'Dim strB_PendidikanIslam1 As String = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaMataPelajaran='PENDIDIKAN ISLAM' AND PB IS NOT NULL"
            If oCommon.isExist(strSQL) = True Then
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Calon ini telah mengulang bagi Matapelajaran PENDIDIKAN ISLAM"
                Exit Sub
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaMataPelajaran, PB, MarkahPA)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','PENDIDIKAN ISLAM','1', '" & strA_Markah_PendidikanIslam2 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
        End If

        If PA6.Checked = True Then
            ''get markah
            'strSQL = "SELECT A_PendidikanIslam1 FROM kpmkv_pelajar_markah WHERE PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            'strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            'strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            'Dim strA_PendidikanIslam1 As String = oCommon.getFieldValue(strSQL)

            ''get markah
            'strSQL = "SELECT A_PendidikanIslam2 FROM kpmkv_pelajar_markah WHERE PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            'strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            'strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            'Dim strA_PendidikanIslam2 As String = oCommon.getFieldValue(strSQL)

            'If Not String.IsNullOrEmpty(strA_PendidikanIslam1) Then
            '    strA_PendidikanIslam1 = oCommon.DoConvertN(((strA_PendidikanIslam1 / 100) * 50), 0) '50%
            'End If

            'If Not String.IsNullOrEmpty(strA_PendidikanIslam2) Then
            '    strA_PendidikanIslam2 = oCommon.DoConvertN(((strA_PendidikanIslam2 / 100) * 20), 0) '20%
            'End If

            'Dim strA_Markah_PendidikanIslam2 As Integer = CInt(strA_PendidikanIslam1) + CInt(strA_PendidikanIslam2)
            'get markah'
            'change on 06092016
            strSQL = "SELECT B_PendidikanIslam1 FROM kpmkv_pelajar_markah WHERE PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            Dim strB_PendidikanIslam1 As String = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaMataPelajaran='PENDIDIKAN ISLAM' AND PA IS NOT NULL"
            If oCommon.isExist(strSQL) = True Then
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Calon ini telah mengulang bagi Matapelajaran PENDIDIKAN ISLAM"
                Exit Sub
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaMataPelajaran, PA, MarkahPB)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','PENDIDIKAN ISLAM','1','" & strB_PendidikanIslam1 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
        End If
        'PENDIDIKAN ISLAM--------------------------------------

        If PB7.Checked = True Then
            'get markah
            strSQL = "SELECT A_PendidikanMoral FROM kpmkv_pelajar_markah WHERE PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            Dim strA_PendidikanMoral As String = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaMataPelajaran='PENDIDIKAN MORAL' AND PB IS NOT NULL"
            If oCommon.isExist(strSQL) = True Then
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Calon ini telah mengulang bagi Matapelajaran PENDIDIKAN MORAL"
                Exit Sub
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaMataPelajaran, PB, MarkahPA)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','PENDIDIKAN MORAL','1','" & strA_PendidikanMoral & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
        End If

        If PA7.Checked = True Then
            'get markah
            strSQL = "SELECT B_PendidikanMoral FROM kpmkv_pelajar_markah WHERE PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            Dim strB_PendidikanMoral As String = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaMataPelajaran='PENDIDIKAN MORAL' AND PA IS NOT NULL"
            If oCommon.isExist(strSQL) = True Then
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Calon ini telah mengulang bagi Matapelajaran PENDIDIKAN MORAL"
                Exit Sub
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaMataPelajaran, PA, MarkahPB)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','PENDIDIKAN MORAL','1','" & strB_PendidikanMoral & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
        End If
        'PENDIDIKAN MORAL---------------------------------------------------------------

        If lblMsg.Text = "" Then
            divMsg.Attributes("class") = "info"
            lblMsg.Text = "Berjaya!.Daftar Calon Ulang Akademik."
            'Else
            '    divMsg.Attributes("class") = "error"
            '    lblMsg.Text = "Tidak Berjaya!.Daftar Calon Ulang Akademik."
        End If
        clear()
        HiddenPBPACheckBox()
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        lblMsg.Text = ""
        clear()
    End Sub
    Private Sub clear()

        'lblMsg.Text = ""
        PB1.Checked = False
        PB2.Checked = False
        PB3.Checked = False
        PB4.Checked = False
        PB5.Checked = False
        PB6.Checked = False
        PB7.Checked = False
        PA1.Checked = False
        PA2.Checked = False
        PA3.Checked = False
        PA4.Checked = False
        PA5.Checked = False
        PA6.Checked = False
        PA7.Checked = False
    End Sub
    Private Sub HiddenPBPACheckBox()
        '('D','E','D-','D+','T')
        If lblBM.Text <> "D" And lblBM.Text <> "E" And lblBM.Text <> "D-" And lblBM.Text <> "D+" And lblBM.Text <> "T" Then
            PB1.Enabled = False
            PA1.Enabled = False
        Else
            PB1.Enabled = True
            PA1.Enabled = True
        End If

        If lblBI.Text <> "D" And lblBI.Text <> "E" And lblBI.Text <> "D-" And lblBI.Text <> "D+" And lblBI.Text <> "T" Then
            PB2.Enabled = False
            PA2.Enabled = False
        Else
            PB2.Enabled = True
            PA2.Enabled = True
        End If

        If lblMT.Text <> "D" And lblMT.Text <> "E" And lblMT.Text <> "D-" And lblMT.Text <> "D+" And lblMT.Text <> "T" Then
            PB3.Enabled = False
            PA3.Enabled = False
        Else
            PB3.Enabled = True
            PA3.Enabled = True
        End If

        If lblSC.Text <> "D" And lblSC.Text <> "E" And lblSC.Text <> "D-" And lblSC.Text <> "D+" And lblSC.Text <> "T" Then
            PB4.Enabled = False
            PA4.Enabled = False
        Else
            PB4.Enabled = True
            PA4.Enabled = True
        End If

        If lblSJ.Text <> "D" And lblSJ.Text <> "E" And lblSJ.Text <> "D-" And lblSJ.Text <> "D+" And lblSJ.Text <> "T" Then
            PB5.Enabled = False
            PA5.Enabled = False
        Else
            PB5.Enabled = True
            PA5.Enabled = True
        End If

        If lblPI.Text <> "D" And lblPI.Text <> "E" And lblPI.Text <> "D-" And lblPI.Text <> "D+" And lblPI.Text <> "T" Then
            PB6.Enabled = False
            PA6.Enabled = False
        Else
            PB6.Enabled = True
            PA6.Enabled = True
        End If

        If lblPM.Text <> "D" And lblPM.Text <> "E" And lblPM.Text <> "D-" And lblPM.Text <> "D+" And lblPM.Text <> "T" Then
            PB7.Enabled = False
            PA7.Enabled = False
        Else
            PB7.Enabled = True
            PA7.Enabled = True
        End If
    End Sub

    Private Sub chkSesi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkSesi.SelectedIndexChanged
        kpmkv_kodkursus_list()
        kpmkv_kelas_list()
    End Sub

    Private Sub ddlKodKursus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlKodKursus.SelectedIndexChanged
        kpmkv_kelas_list()
    End Sub
End Class
