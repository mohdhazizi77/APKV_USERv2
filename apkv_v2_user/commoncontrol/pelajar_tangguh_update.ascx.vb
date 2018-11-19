Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization

Public Class pelajar_tangguh_update
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strKursusID As String = ""

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblMsg.Text = ""
        Try
            If Not IsPostBack Then

                strSQL = "SELECT kpmkv_kolej.Nama,kpmkv_kolej.RecordID FROM kpmkv_kolej LEFT OUTER JOIN kpmkv_pelajar ON kpmkv_kolej.RecordID=kpmkv_pelajar.KolejRecordID  WHERE kpmkv_pelajar.PelajarID='" & Request.QueryString("PelajarID") & "'"
                lblKolej.Text = oCommon.getFieldValue(strSQL)
                strSQL = "SELECT kpmkv_kolej.RecordID FROM kpmkv_kolej LEFT OUTER JOIN kpmkv_pelajar ON kpmkv_kolej.RecordID=kpmkv_pelajar.KolejRecordID  WHERE kpmkv_pelajar.PelajarID='" & Request.QueryString("PelajarID") & "'"
                lblRecordID.Text = oCommon.getFieldValue(strSQL)
                lblKolejID.Text = lblRecordID.Text

                LoadPage()

                kpmkv_tahun_list()
                ddlTahun.Text = Now.Year

                kpmkv_semester_list()

                'kpmkv_kodkursus_list()

                'kpmkv_kelas_list()

               
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try
    End Sub

    Private Sub kpmkv_tahun_list()
        strSQL = "SELECT Tahun FROM kpmkv_tahun WITH (NOLOCK) ORDER BY Tahun DESC"
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
            ddlTahun.Items.Add(New ListItem("-Pilih-", "0"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_semester_list()
        strSQL = "SELECT Semester FROM kpmkv_semester"
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
            ddlSemester.Items.Add(New ListItem("-Pilih-", "0"))

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
        strSQL += " WHERE kpmkv_kelas.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kursus.Tahun='" & ddlTahun.Text & "' "
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

            ddlKelas.DataSource = ds
            ddlKelas.DataTextField = "NamaKelas"
            ddlKelas.DataValueField = "KelasID"
            ddlKelas.DataBind()

            '--ALL
            'ddlNamaKelas.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub LoadPage()
        
        strSQL = "SELECT kpmkv_pelajar.Pengajian,kpmkv_pelajar.AngkaGiliran, kpmkv_pelajar.Tahun, kpmkv_pelajar.Semester, kpmkv_pelajar.Sesi, kpmkv_pelajar.Nama, kpmkv_pelajar.MYKAD, kpmkv_pelajar.Tel, "
        strSQL += " kpmkv_pelajar.KursusID,kpmkv_kluster.NamaKluster, kpmkv_kursus.KodKursus, kpmkv_kursus.NamaKursus, kpmkv_pelajar.Kaum, kpmkv_pelajar.Jantina, "
        strSQL += " kpmkv_pelajar.Agama, kpmkv_status.StatusID, kpmkv_pelajar.Email, kpmkv_pelajar.Catatan, kpmkv_kelas.NamaKelas"
        strSQL += " FROM  kpmkv_pelajar LEFT OUTER JOIN kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
        strSQL += " kpmkv_status ON kpmkv_pelajar.StatusID = kpmkv_status.StatusID LEFT OUTER JOIN kpmkv_kelas ON kpmkv_pelajar.KelasID = kpmkv_kelas.KelasID"
        strSQL += " LEFT OUTER JOIN kpmkv_jeniscalon ON kpmkv_pelajar.JenisCalonID = kpmkv_jeniscalon.JenisCalonID "
        strSQL += "LEFT OUTER JOIN kpmkv_kluster ON kpmkv_pelajar.KursusID=kpmkv_kluster.KlusterID"
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
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("NamaKluster")) Then
                    lblNamaKluster.Text = ds.Tables(0).Rows(0).Item("NamaKluster")
                Else
                    lblNamaKursus.Text = ""
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


                If Not IsDBNull(ds.Tables(0).Rows(0).Item("NamaKelas")) Then
                    lblNamaKelas.Text = ds.Tables(0).Rows(0).Item("NamaKelas")
                Else
                    lblNamaKelas.Text = ""
                End If
                'personal
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Nama")) Then
                    txtNama.Text = ds.Tables(0).Rows(0).Item("Nama")
                Else
                    txtNama.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Mykad")) Then
                    lblMykad.Text = ds.Tables(0).Rows(0).Item("Mykad")


                    If Not IsDBNull(ds.Tables(0).Rows(0).Item("AngkaGiliran")) Then
                        lblAngkaGiliran.Text = ds.Tables(0).Rows(0).Item("AngkaGiliran")
                    Else
                        lblAngkaGiliran.Text = ""
                    End If
                    If Not IsDBNull(ds.Tables(0).Rows(0).Item("Jantina")) Then
                        lblJantina.Text = ds.Tables(0).Rows(0).Item("Jantina")
                    Else
                        lblJantina.Text = ""
                    End If
                    If Not IsDBNull(ds.Tables(0).Rows(0).Item("Kaum")) Then
                        lblkaum.Text = ds.Tables(0).Rows(0).Item("Kaum")
                    Else
                        lblkaum.Text = ""
                    End If
                    If Not IsDBNull(ds.Tables(0).Rows(0).Item("Agama")) Then
                        lblAgama.Text = ds.Tables(0).Rows(0).Item("Agama")
                    Else
                        lblAgama.Text = ""
                    End If

                    If Not IsDBNull(ds.Tables(0).Rows(0).Item("Email")) Then
                        txtEmail.Text = ds.Tables(0).Rows(0).Item("Email")
                    Else
                        txtEmail.Text = ""
                    End If
                    If Not IsDBNull(ds.Tables(0).Rows(0).Item("Catatan")) Then
                        txtCatatan.Text = ds.Tables(0).Rows(0).Item("Catatan")
                    Else
                        txtCatatan.Text = ""
                    End If
                    If Not IsDBNull(ds.Tables(0).Rows(0).Item("Tel")) Then
                        lbltelefon.Text = ds.Tables(0).Rows(0).Item("Tel")
                    Else
                        lbltelefon.Text = ""
                    End If
                End If
            End If
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
            If kpmkv_pelajar_create() = True Then
                divMsg.Attributes("class") = "info"
                lblMsg.Text = "Calon tangguh berjaya didaftarkan"
            Else
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Calon tangguh tidak berjaya didaftarkan"
            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try

    End Sub
    Private Function ValidatePage() As Boolean
        lblMsg.Text = ""

        If ddlTahun.Text = "" Then
            lblMsg.Text = "Sila pilih Tahun"
            ddlTahun.Focus()
            Return False
        End If

        If ddlTahun.Text < lblTahun.Text Then
            lblMsg.Text = "Sila pilih Tahun kehadapan"
            ddlTahun.Focus()
            Return False
        End If

        '--txtSemester
        If ddlSemester.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan Semester!"
            ddlSemester.Focus()
            Return False
        End If

        '--ddlsesi
        If chkSesi.Text = "" Then
            lblMsg.Text = "Sila Tick() pada kotak Sesi"
            chkSesi.Focus()
            Return False
        End If
        If Not ddlKodKursus.SelectedItem.Text = lblKodKursus.Text Then
            lblMsg.Text = "Sila pilih Kursus yang berkaitan"
            ddlKodKursus.Focus()
            Return False
        End If

        strSQL = "SELECT Mykad FROM kpmkv_pelajar WHERE Mykad='" & lblMykad.Text & "' AND Tahun='" & ddlTahun.SelectedValue & "'AND Semester='" & ddlSemester.SelectedValue & "' AND Sesi='" & chkSesi.Text & "'"
        strSQL += " AND KursusID='" & ddlKodKursus.SelectedValue & "' AND KelasID='" & ddlKelas.SelectedValue & "'"
        strRet = oCommon.isExist(strSQL)
        If strRet = True Then
            lblMsg.Text = "Pelajar ini telah didaftarkan."
            Return False
        End If

        Return True
    End Function
    Private Function kpmkv_pelajar_create() As Boolean
       
        'insert table pelajar
        strSQL = "INSERT INTO kpmkv_pelajar (KolejRecordID,Pengajian,KursusID,KelasID,Tahun,Semester,Sesi,Nama,MYKAD,Jantina,Kaum,Agama,Email,Catatan,StatusID,JenisCalonID,AngkaGiliran,Tel,IsDeleted)"
        strSQL += "VALUES ('" & lblRecordID.Text & "','" & lblPengajian.Text & "','" & ddlKodKursus.SelectedValue & "','" & ddlKelas.SelectedValue & "','" & ddlTahun.SelectedValue & "','" & ddlSemester.SelectedValue & "',"
        strSQL += "'" & chkSesi.SelectedValue & "','" & oCommon.FixSingleQuotes(txtNama.Text.ToUpper) & "','" & oCommon.FixSingleQuotes(lblMykad.Text.ToUpper) & "','" & lblJantina.Text & "',"
        strSQL += " '" & lblkaum.Text & "','" & lblAgama.Text & "','" & oCommon.FixSingleQuotes(txtEmail.Text) & "','" & oCommon.FixSingleQuotes(txtCatatan.Text) & "','2','2',"
        strSQL += " '" & oCommon.FixSingleQuotes(lblAngkaGiliran.Text) & "','" & oCommon.FixSingleQuotes(lbltelefon.Text) & "','N')"
        strRet = oCommon.ExecuteSQL(strSQL)

        If strRet = "0" Then

            strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE Mykad='" & lblMykad.Text & "' AND Tahun='" & ddlTahun.SelectedValue & "'AND Semester='" & ddlSemester.SelectedValue & "' AND Sesi='" & chkSesi.Text & "'"
            strSQL += " AND KursusID='" & ddlKodKursus.SelectedValue & "' AND KelasID='" & ddlKelas.SelectedValue & "'"
            Dim strPelajarID As Integer = oCommon.getFieldValue(strSQL)

            'insert table pelajar_markah
            strSQL = "INSERT INTO kpmkv_pelajar_markah (PelajarID,KolejRecordID,KursusID,Tahun,Semester,Sesi)"
            strSQL += " VALUES ('" & strPelajarID & "','" & lblRecordID.Text & "','" & ddlKodKursus.SelectedValue & "',"
            strSQL += "'" & ddlTahun.SelectedValue & "','" & ddlSemester.SelectedValue & "','" & chkSesi.Text & "')"
            strRet = oCommon.ExecuteSQL(strSQL)

            strSQL = "UPDATE kpmkv_pelajar SET StatusID='1',JenisCalonID='1' WHERE PelajarID='" & Request.QueryString("PelajarID") & "'"
            strRet = oCommon.ExecuteSQL(strSQL)

            Return True
        Else
            lblMsg.Text = "System Error:" & strRet
            Return False
        End If
        'Response.Write(strSQL)
    End Function

    Private Sub chkSesi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkSesi.SelectedIndexChanged
        kpmkv_kodkursus_list()
        kpmkv_kelas_list()
    End Sub

    Private Sub ddlKodKursus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlKodKursus.SelectedIndexChanged
        kpmkv_kelas_list()
    End Sub
End Class