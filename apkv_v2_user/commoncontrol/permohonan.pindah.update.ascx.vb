Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class permohonan_pindah_update
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


                '--set default date
                txtEventDate.Text = Format(CDate(Date.Now), "dd-MM-yyyy")

                kpmkv_jeniskolej_list()
                kpmkv_namakolej_list()

                LoadPage()

                strSQL = "SELECT KlusterID FROM kpmkv_kursus WHERE KodKursus='" & lblKodKursus.Text & "'"
                Dim strKlusterID As String = oCommon.getFieldValue(strSQL)

                strSQL = "SELECT NamaKluster FROM kpmkv_kluster WHERE KlusterID='" & strKlusterID & "'"
                lblKluster.Text = oCommon.getFieldValue(strSQL)

            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try
    End Sub
    Private Sub kpmkv_jeniskolej_list()
        strSQL = "SELECT Jenis FROM kpmkv_kolej GROUP BY Jenis"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlJenisKolej.DataSource = ds
            ddlJenisKolej.DataTextField = "Jenis"
            ddlJenisKolej.DataValueField = "Jenis"
            ddlJenisKolej.DataBind()

            '--ALL
            ' ddlJenisKolej.Items.Add(New ListItem("PILIH", "PILIH"))
        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_namakolej_list()
        strSQL = "SELECT Nama,RecordID FROM kpmkv_kolej WHERE Jenis='" & ddlJenisKolej.Text & "' ORDER BY Nama"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlNamaKolej.DataSource = ds
            ddlNamaKolej.DataTextField = "Nama"
            ddlNamaKolej.DataValueField = "RecordID"
            ddlNamaKolej.DataBind()

            '--ALL
            ddlNamaKolej.Items.Add(New ListItem("PILIH", "PILIH"))
        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub LoadPage()
        strSQL = "SELECT kpmkv_kursus.KursusID,kpmkv_pelajar.Pengajian, kpmkv_pelajar.Tahun, kpmkv_pelajar.Semester, kpmkv_pelajar.Sesi, kpmkv_pelajar.Nama, kpmkv_pelajar.MYKAD, kpmkv_pelajar.AngkaGiliran, "
        strSQL += " kpmkv_kursus.KursusID, kpmkv_jeniscalon.JenisCalon, kpmkv_kursus.KursusID, kpmkv_kursus.KodKursus, kpmkv_kursus.NamaKursus, kpmkv_pelajar.Kaum, kpmkv_pelajar.Jantina, "
        strSQL += " kpmkv_pelajar.Agama, kpmkv_status.Status, kpmkv_pelajar.Email, kpmkv_pelajar.Catatan, kpmkv_kelas.NamaKelas"
        strSQL += " FROM  kpmkv_pelajar LEFT OUTER JOIN kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
        strSQL += " kpmkv_status ON kpmkv_pelajar.StatusID = kpmkv_status.StatusID LEFT OUTER JOIN kpmkv_kelas ON kpmkv_pelajar.KelasID = kpmkv_kelas.KelasID"
        strSQL += " LEFT OUTER JOIN kpmkv_jeniscalon ON kpmkv_pelajar.JenisCalonID = kpmkv_jeniscalon.JenisCalonID "
        strSQL += " WHERE kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.StatusID='2' AND kpmkv_pelajar.PelajarID='" & Request.QueryString("PelajarID") & "'"

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
                '--Account Details 
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Tahun")) Then
                    lblTahun.Text = ds.Tables(0).Rows(0).Item("Tahun")
                Else
                    lblTahun.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Semester")) Then
                    lblSemester.Text = ds.Tables(0).Rows(0).Item("Semester")
                Else
                    lblSemester.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Sesi")) Then
                    lblSesi.Text = ds.Tables(0).Rows(0).Item("Sesi")
                Else
                    lblSesi.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("KodKursus")) Then
                    lblKodKursus.Text = ds.Tables(0).Rows(0).Item("KodKursus")
                Else
                    lblKodKursus.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("NamaKursus")) Then
                    lblNamaKursus.Text = ds.Tables(0).Rows(0).Item("NamaKursus")
                Else
                    lblNamaKursus.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("NamaKelas")) Then
                    lblNamaKelas.Text = ds.Tables(0).Rows(0).Item("NamaKelas")
                Else
                    lblNamaKelas.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Nama")) Then
                    lblNama.Text = ds.Tables(0).Rows(0).Item("Nama")
                Else
                    lblNama.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("MYKAD")) Then
                    lblMykad.Text = ds.Tables(0).Rows(0).Item("MYKAD")
                Else
                    lblMykad.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("ANGKAGILIRAN")) Then
                    lblAngkaGiliran.Text = ds.Tables(0).Rows(0).Item("ANGKAGILIRAN")
                Else
                    lblAngkaGiliran.Text = ""
                End If

                '--
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Jantina")) Then
                    lblJantina.Text = ds.Tables(0).Rows(0).Item("Jantina")
                Else
                    lblJantina.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Kaum")) Then
                    lblKaum.Text = ds.Tables(0).Rows(0).Item("Kaum")
                Else
                    lblKaum.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Agama")) Then
                    lblAgama.Text = ds.Tables(0).Rows(0).Item("Agama")
                Else
                    lblAgama.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Status")) Then
                    lblStatus.Text = ds.Tables(0).Rows(0).Item("Status")
                Else
                    lblStatus.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("JenisCalon")) Then
                    lblJenisCalon.Text = ds.Tables(0).Rows(0).Item("JenisCalon")
                Else
                    lblJenisCalon.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Email")) Then
                    lblEmail.Text = ds.Tables(0).Rows(0).Item("Email")
                Else
                    lblEmail.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Catatan")) Then
                    txtCatatan.Text = ds.Tables(0).Rows(0).Item("Catatan")
                Else
                    txtCatatan.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("KursusID")) Then
                    lblKursusID.Text = ds.Tables(0).Rows(0).Item("KursusID")
                Else
                    lblKursusID.Text = ""
                End If
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub ddlJenisKolej_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlJenisKolej.SelectedIndexChanged
        kpmkv_namakolej_list()

    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click

        'check form validation. if failed exit
        If ValidateForm() = False Then
            Exit Sub
        End If

        '--execute
        Try
            lblMsg.Text = ""
            If kpmkv_pelajar_update() = 0 Then
                If kpmkv_pelajar_markah_update() = 0 Then
                    If kpmkv_pelajar_history() = 0 Then
                        If kpmkv_pelajar_Markah_history() = 0 Then
                        Else
                            strSQL = "Delete From kpmkv_pelajar_history WHERE PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                            strSQL = "Delete From kpmkv_pelajar_markah_history WHERE PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)


                            'change05082016
                            Dim strPelajarID2 As String = ""
                            strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE IsDeleted='N' AND StatusID='2' AND Mykad='" & lblMykad.Text & "'"
                            Dim cmd As New SqlCommand(strSQL)
                            Dim dt As DataTable = GetData(cmd)

                            For i As Integer = 0 To dt.Rows.Count - 1
                                strPelajarID2 = dt.Rows(i)("PelajarID").ToString()

                                strSQL = "UPDATE kpmkv_pelajar SET KolejRecordID='" & lblKolejID.Text & "', DateTransfer='', RefNoTransfer='' WHERE PelajarID='" & strPelajarID2 & "'"
                                strRet = oCommon.ExecuteSQL(strSQL)

                                strSQL = "UPDATE kpmkv_pelajar_markah SET KolejRecordID='" & lblKolejID.Text & "'  WHERE PelajarID='" & strPelajarID2 & "'"
                                strRet = oCommon.ExecuteSQL(strSQL)

                                divMsg.Attributes("class") = "error"
                                lblMsg.Text = "Permohonan Calon berpindah tidak berjaya!."
                            Next
                           
                        End If
                    End If
                End If
            End If

            If lblMsg.Text.Length = 0 Then
                lblMsg.Text = "Permohonan Calon berpindah berjaya!."
            End If

        Catch ex As Exception
            lblMsg.Text = "System error:" & ex.Message
        End Try
        btnUpdate.Enabled = False
    End Sub

    '--CHECK form validation.
    Private Function ValidateForm() As Boolean
        If txtEventDate.Text.Length = 0 Then
            lblMsg.Text = "Sila pilih Tarikh."
            txtEventDate.Focus()
            Return False
        End If

        If txtCatatan.Text.Length = 0 Then
            lblMsg.Text = "Catatan ini mesti diisi."
            txtCatatan.Focus()
            Return False
        End If

        If chkconfirm.Checked = False Then
            lblMsg.Text = "Sila Tick() pada kotak yang disediakan."
            chkconfirm.Focus()
            Return False
        End If

        If txtRef.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan No. Rujukan."
            txtRef.Focus()
            Return False
        End If

        strSQL = "SELECT kpmkv_kursus.KursusID FROM kpmkv_kursus_kolej LEFT OUTER JOIN kpmkv_kursus ON kpmkv_kursus_kolej.KursusID=kpmkv_kursus.KursusID "
        strSQL += "WHERE kpmkv_kursus_kolej.KursusID='" & lblKursusID.Text & "' AND kpmkv_kursus_kolej.KolejRecordID='" & ddlNamaKolej.SelectedValue & "'"
        If oCommon.isExist(strSQL) = False Then
            lblMsg.Text = "Kursus tiada ditawarkan di kolej yang ingin berpindah"
            Return False
        End If

        Return True
    End Function
    Private Function kpmkv_pelajar_history() As String
         'insert into course list
        strSQL = "Insert Into kpmkv_pelajar_history(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, AngkaGiliran, Nama, MYKAD, Tel, Email, Jantina, Bangsa, Kaum, "
        strSQL += "Agama, StatusID, JenisCalonID, Catatan, DateCreated, CreateBy, IsDeleted)"
        strSQL += "Select PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, AngkaGiliran, Nama, MYKAD, Tel, Email, Jantina, Bangsa, Kaum, "
        strSQL += "Agama, StatusID, JenisCalonID, Catatan, DateCreated, CreateBy, IsDeleted From kpmkv_pelajar WHERE PelajarID='" & Request.QueryString("PelajarID") & "'"

        strRet = oCommon.ExecuteSQL(strSQL)
        ''--debug
        'Response.Write(SQL)

        Return strRet

    End Function
    Private Function kpmkv_pelajar_Markah_history() As String
        'insert into course list
        strSQL = "INSERT INTO kpmkv_pelajar_markah_history(PelajarID, Tahun, Sesi, Semester, KursusID, KolejRecordID, DateCreated, CreateBy, UploadCode, B_Amali1, B_Amali2, B_Amali3, B_Amali4, B_Amali5, B_Amali6, "
        strSQL += " B_Amali7, B_Amali8, B_Teori1, B_Teori2, B_Teori3, B_Teori4, B_Teori5, B_Teori6, B_Teori7, B_Teori8, A_Amali1, A_Amali2, A_Amali3, A_Amali4, A_Amali5, "
        strSQL += " A_Amali6, A_Amali7, A_Amali8, A_Teori1, A_Teori2, A_Teori3, A_Teori4, A_Teori5, A_Teori6, A_Teori7, A_Teori8, B_BahasaMelayu, B_BahasaMelayu1, "
        strSQL += " B_BahasaMelayu2, B_BahasaMelayu3, B_BahasaInggeris, B_Science1, B_Science2, B_Sejarah, B_PendidikanIslam1, B_PendidikanIslam2, B_PendidikanMoral,"
        strSQL += " B_Mathematics, A_BahasaMelayu, A_BahasaMelayu1, A_BahasaMelayu2, A_BahasaMelayu3, A_BahasaInggeris, A_Science1, A_Science2, A_Sejarah,"
        strSQL += " A_PendidikanIslam1, A_PendidikanIslam2, A_PendidikanMoral, A_Mathematics, GredV1, PBPAM1, GredV2, PBPAM2, GredV3, PBPAM3, GredV4, PBPAM4, GredV5,"
        strSQL += " PBPAM5, GredV6, PBPAM6, GredV7, PBPAM7, GredV8, PBPAM8, BahasaMelayu, GredBM, BahasaInggeris, GredBI, Science, GredSC, Sejarah, GredSJ,"
        strSQL += " PendidikanIslam, GredPI, PendidikanMoral, GredPM, Mathematics, GredMT, PNG_Akademik, PNG_Vokasional, JamKredit_Akademik, JamKredit_Vokasional)"
        strSQL += " SELECT PelajarID, Tahun, Sesi, Semester, KursusID, KolejRecordID, DateCreated, CreateBy, UploadCode, B_Amali1, B_Amali2, B_Amali3, B_Amali4, B_Amali5, B_Amali6, B_Amali7, B_Amali8, B_Teori1, "
        strSQL += " B_Teori2, B_Teori3, B_Teori4, B_Teori5, B_Teori6, B_Teori7, B_Teori8, A_Amali1, A_Amali2, A_Amali3, A_Amali4, A_Amali5, A_Amali6, A_Amali7, A_Amali8, "
        strSQL += " A_Teori1, A_Teori2, A_Teori3, A_Teori4, A_Teori5, A_Teori6, A_Teori7, A_Teori8, B_BahasaMelayu, B_BahasaMelayu1, "
        strSQL += " B_BahasaMelayu2, B_BahasaMelayu3, B_BahasaInggeris, B_Science1, B_Science2, B_Sejarah, B_PendidikanIslam1, B_PendidikanIslam2, B_PendidikanMoral,"
        strSQL += " B_Mathematics, A_BahasaMelayu, A_BahasaMelayu1, A_BahasaMelayu2, A_BahasaMelayu3, A_BahasaInggeris, A_Science1, A_Science2, A_Sejarah,"
        strSQL += " A_PendidikanIslam1, A_PendidikanIslam2, A_PendidikanMoral, A_Mathematics, GredV1, PBPAM1, GredV2, PBPAM2, GredV3, PBPAM3, GredV4, PBPAM4, GredV5,"
        strSQL += " PBPAM5, GredV6, PBPAM6, GredV7, PBPAM7, GredV8, PBPAM8, BahasaMelayu, GredBM, BahasaInggeris, GredBI, Science, GredSC, Sejarah, GredSJ,"
        strSQL += " PendidikanIslam, GredPI, PendidikanMoral, GredPM, Mathematics, GredMT, PNG_Akademik, PNG_Vokasional, JamKredit_Akademik, JamKredit_Vokasional"
        strSQL += " FROM kpmkv_pelajar_markah WHERE PelajarID='" & Request.QueryString("PelajarID") & "'"
        strRet = oCommon.ExecuteSQL(strSQL)
        ''--debug
        'Response.Write(strSQL)

        Return strRet

    End Function

    Private Function kpmkv_pelajar_update() As String
        'insert into course list
        strSQL = "UPDATE kpmkv_pelajar SET KolejRecordID='" & ddlNamaKolej.SelectedValue & "', Catatan='" & txtCatatan.Text & "', DateTransfer='" & Request.Form(txtEventDate.UniqueID) & "', RefNoTransfer='" & txtRef.Text & "' WHERE Mykad='" & lblMykad.Text & "'"
        strRet = oCommon.ExecuteSQL(strSQL)
        ''--debug
        'Response.Write(SQL)

        Return strRet

    End Function
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
            con.Close()
            sda.Dispose()
            con.Dispose()
        End Try
    End Function
    Private Function kpmkv_pelajar_markah_update() As String
        'update into kpmkv_pelajar_markah
        'change05082016
        Dim strPelajarID2 As String = ""

        strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE IsDeleted='N' AND StatusID='2' AND Mykad='" & lblMykad.Text & "'"
        Dim cmd As New SqlCommand(strSQL)
        Dim dt As DataTable = GetData(cmd)

        For i As Integer = 0 To dt.Rows.Count - 1
            strPelajarID2 = dt.Rows(i)("PelajarID").ToString()
            strSQL = "UPDATE kpmkv_pelajar_markah SET KolejRecordID='" & ddlNamaKolej.SelectedValue & "'  WHERE PelajarID='" & strPelajarID2 & "'"
            strRet = oCommon.ExecuteSQL(strSQL)
        Next
        ''--debug
        'Response.Write(SQL)


        Return strRet

    End Function
End Class