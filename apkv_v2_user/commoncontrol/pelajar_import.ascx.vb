Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Imports System.Data.Common

Partial Public Class pelajar_import
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strBil As String = ""
    Dim strTahun As String = ""
    Dim strSemester As String = ""
    Dim strSesi As String = ""
    Dim strKodKursus As String = ""
    Dim strNama As String = ""
    Dim strAngkaGiliran As String = ""
    Dim strMykad As String = ""
    Dim strPengajian As String = ""
    Dim strEmail As String = ""
    Dim strJantina As String = ""
    Dim strKaum As String = ""
    Dim strAgama As String = ""
    Dim strBangsa As String = ""
    Dim strTel As String = ""
    Dim strKolej As String = ""
    Dim IntTakwim As Integer = 0

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Not IsPostBack Then
                ''kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejRecordID.Text = oCommon.getFieldValue(strSQL)
                '------exist takwim
                strSQL = "SELECT * FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Import Calon' AND Aktif='1'"
                If oCommon.isExist(strSQL) = True Then

                    'count data takwim
                    'Get the data from database into datatable
                    Dim cmd As New SqlCommand("SELECT TakwimID FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Import Calon' AND Aktif='1'")
                    Dim dt As DataTable = GetData(cmd)

                    For i As Integer = 0 To dt.Rows.Count - 1
                        IntTakwim = dt.Rows(i)("TakwimID")

                        strSQL = "SELECT Kohort,Semester,Sesi,TarikhMula,TarikhAkhir FROM kpmkv_takwim WHERE TakwimID='" & IntTakwim & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_user_login As Array
                        ar_user_login = strRet.Split("|")
                        lblKohort.Text = ar_user_login(0)
                        lblSemester.Text = ar_user_login(1)
                        lblSesi.Text = ar_user_login(2)
                        Dim strMula As String = ar_user_login(3)
                        Dim strAkhir As String = ar_user_login(4)

                        Dim strdateNow As Date = Date.Now
                        Dim startDate = DateTime.ParseExact(strMula, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                        Dim endDate = DateTime.ParseExact(strAkhir, "dd-MM-yyyy", CultureInfo.InvariantCulture)

                        Dim ts As New TimeSpan
                        ts = endDate.Subtract(strdateNow)
                        Dim dayDiff = ts.Days

                        If strMula IsNot Nothing Then
                            If strAkhir IsNot Nothing And dayDiff >= 0 Then
                                btnFile.Enabled = True
                                btnUpload.Enabled = True
                            End If
                        Else
                            btnFile.Enabled = False
                            btnUpload.Enabled = False
                            lblMsg.Text = "Import Calon telah ditutup!"
                        End If
                    Next
                Else
                    btnFile.Enabled = False
                    btnUpload.Enabled = False
                    lblMsg.Text = "Import Calon telah ditutup!"
                End If
            End If


        Catch ex As Exception
            lblMsg.Text = "Error Message:" & ex.Message
        End Try

    End Sub
    Private Sub btnFile_Click(sender As Object, e As EventArgs) Handles btnFile.Click
        Response.ContentType = "Application/xlsx"
        Response.AppendHeader("Content-Disposition", "attachment; filename=PELAJARIMPORT2.xlsx")
        Response.TransmitFile(Server.MapPath("~/sample_data/PELAJARIMPORT2.xlsx"))
        Response.End()
    End Sub
    Private Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        lblMsg.Text = ""
        Try
            '--upload excel
            lblUploadCode.Text = oCommon.getRandom
            If ImportExcel() = True Then
                divMsg.Attributes("class") = "info"
            Else
            End If
        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        End Try

    End Sub

    Private Function ImportExcel() As Boolean
        Dim path As String = String.Concat(Server.MapPath("~/inbox/"))

        If FlUploadcsv.HasFile Then
            Dim rand As Random = New Random()
            Dim randNum = rand.Next(1000)
            Dim fullFileName As String = path + oCommon.getRandom + "-" + FlUploadcsv.FileName
            FlUploadcsv.PostedFile.SaveAs(fullFileName)

            '--required ms access engine
            Dim excelConnectionString As String = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & fullFileName & ";Extended Properties=Excel 12.0;")

            Dim connection As OleDbConnection = New OleDbConnection(excelConnectionString)
            Dim command As OleDbCommand = New OleDbCommand("SELECT * FROM [pelajar$]", connection)
            Dim da As OleDbDataAdapter = New OleDbDataAdapter(command)
            Dim ds As DataSet = New DataSet

            Try
                connection.Open()
                da.Fill(ds)
                Dim validationMessage As String = ValidateSiteData(ds)
                If validationMessage = "" Then
                    SaveSiteData(ds)

                Else
                    'lblMsgTop.Text = "Muatnaik GAGAL!. Lihat mesej dibawah."
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Kesalahan Kemasukkan Maklumat Calon:<br />" & validationMessage
                    Return False
                End If

                da.Dispose()
                connection.Close()
                command.Dispose()

            Catch ex As Exception
                lblMsg.Text = "System Error:" & ex.Message
                Return False
            Finally
                If connection.State = ConnectionState.Open Then
                    connection.Close()
                End If
            End Try

        Else
            divMsg.Attributes("class") = "error"
            lblMsg.Text = "Sila pilih fail untuk diMuat Naik!"
            Return False
        End If

        Return True

    End Function
    Private Sub refreshVar()
        strBil = ""
        strNama = ""
        strAngkaGiliran = ""
        strMykad = ""
        strPengajian = ""
        strEmail = ""
        strJantina = ""
        strKaum = ""
        strAgama = ""
        strBangsa = ""
        strTel = ""
        strKolej = ""

    End Sub
    Protected Function ValidateSiteData(ByVal SiteData As DataSet) As String
        Try
            'Loop through DataSet and validate data
            'If data is bad, bail out, otherwise continue on with the bulk copy
            Dim strMsg As String = ""
            Dim sb As StringBuilder = New StringBuilder()
            For i As Integer = 0 To SiteData.Tables(0).Rows.Count - SiteData.Tables(0).Rows(i).Item("BIL")

                refreshVar()
                strMsg = ""

                'bil
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("BIL")) Then
                    strBil = SiteData.Tables(0).Rows(i).Item("BIL")
                End If

                'Tahun
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Tahun")) Then
                    strTahun = SiteData.Tables(0).Rows(i).Item("Tahun")
                Else
                    strMsg += "Sila isi Tahun|"
                End If

                If Not strTahun = lblKohort.Text Then
                    strMsg += "'" & lblKohort.Text & "'|"
                End If
                'Semester
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Semester")) Then
                    strSemester = SiteData.Tables(0).Rows(i).Item("Semester")
                Else
                    strMsg += "Sila isi Semester|"
                End If

                If Not strSemester = lblSemester.Text Then
                    strMsg += "'" & lblSemester.Text & "'|"
                End If
                'Sesi
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Sesi")) Then
                    strSesi = SiteData.Tables(0).Rows(i).Item("Sesi")
                Else
                    strMsg += "Sila isi Sesi|"
                End If

                If Not strSesi = lblSesi.Text Then
                    strMsg += "'" & lblSesi.Text & "'|"
                End If

                'Nama
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Nama")) Then
                    strNama = SiteData.Tables(0).Rows(i).Item("Nama")
                Else
                    strMsg += "Sila isi Nama|"
                End If
                'Angka
                'If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("AngkaGiliran")) Then
                '    strAngkaGiliran = SiteData.Tables(0).Rows(i).Item("AngkaGiliran")
                'End If
                'kodkursus
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("KodKursus")) Then
                    strKodKursus = SiteData.Tables(0).Rows(i).Item("KodKursus")
                Else
                    strMsg += "Sila isi KodProgram|"
                End If
                'Jantina
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Jantina")) Then
                    strJantina = SiteData.Tables(0).Rows(i).Item("Jantina")
                Else
                    strMsg += "Sila isi Jantina|"
                End If
                'Kaum
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Kaum")) Then
                    strKaum = SiteData.Tables(0).Rows(i).Item("Kaum")
                Else
                    strMsg += "Sila isi Kaum|"
                End If
                'Agama
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Agama")) Then
                    strAgama = SiteData.Tables(0).Rows(i).Item("Agama")
                Else
                    strMsg += "Sila isi Agama|"
                End If

                'Pengajian
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Pengajian")) Then
                    strPengajian = SiteData.Tables(0).Rows(i).Item("Pengajian")
                Else
                    strMsg += "Sila isi Pengajian|"
                End If

                'Tel
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Tel")) Then
                    strTel = SiteData.Tables(0).Rows(i).Item("Tel")
                    'Else
                    ' strMsg += "Sila isi Telefon|"
                End If

                'email
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Email")) Then
                    strEmail = SiteData.Tables(0).Rows(i).Item("Email")
                    'Else
                    '    strMsg += "Sila isi Email|"
                End If

                '--MYKAD is required!
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Mykad")) Then
                    strMykad = SiteData.Tables(0).Rows(i).Item("Mykad")
                Else
                    strMsg += "Sila isi Mykad|"
                End If

                'mykad length
                If oCommon.isMyKad2(strMykad) = False Then
                    strMsg += "Huruf tidak dibenarkan .Sila masukkan no MYKAD [######06####]"
                Else
                End If

                strSQL = "SELECT Mykad FROM kpmkv_pelajar Where Mykad='" & strMykad & "'"
                If oCommon.isExist(strSQL) = True Then
                    strMsg += "Mykad:" & strMykad & ":" & strNama & ". Mykad ini telah wujud."
                End If

                strSQL = "SELECT kpmkv_kursus.KursusID FROM kpmkv_kursus_kolej INNER JOIN kpmkv_kursus ON kpmkv_kursus_kolej.KursusID=kpmkv_kursus.KursusID "
                strSQL += "WHERE kpmkv_kursus.KodKursus='" & strKodKursus & "' AND kpmkv_kursus.Tahun='" & strTahun & "' AND kpmkv_kursus.Sesi='" & strSesi & "' AND KolejRecordID='" & lblKolejRecordID.Text & "'"

                If oCommon.isExist(strSQL) = False Then
                    strMsg += "Program:" & strKodKursus & " untuk Kohort :" & strTahun & " masih belum didaftarkan"
                End If

                If strMsg.Length = 0 Then
                    'strMsg = "Record#:" & i.ToString & "OK"
                    'strMsg += "<br/>"
                Else
                    strMsg = "Bil#:" & strBil & ":" & strMsg
                    strMsg += "<br/>"
                End If

                sb.Append(strMsg)
                'disp bil

            Next
            Return sb.ToString()
        Catch ex As Exception
            Return ex.Message
        End Try

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
        End Try
        con.Close()
        sda.Dispose()
        con.Dispose()

    End Function
    Public Function FileIsLocked(ByVal strFullFileName As String) As Boolean
        Dim blnReturn As Boolean = False
        Dim fs As System.IO.FileStream

        Try
            fs = System.IO.File.Open(strFullFileName, IO.FileMode.OpenOrCreate, IO.FileAccess.Read, IO.FileShare.None)
            fs.Close()
        Catch ex As System.IO.IOException
            divMsg.Attributes("class") = "error"
            lblMsg.Text = "Error Message FileIsLocked:" & ex.Message
            blnReturn = True
        End Try

        Return blnReturn
    End Function
    Private Function SaveSiteData(ByVal SiteData As DataSet) As String
        lblMsg.Text = ""

        'Dim str As String
        Try

            Dim sb As StringBuilder = New StringBuilder()
            For i As Integer = 0 To SiteData.Tables(0).Rows.Count - SiteData.Tables(0).Rows(i).Item("BIL")

                strTahun = SiteData.Tables(0).Rows(i).Item("Tahun")
                strSemester = SiteData.Tables(0).Rows(i).Item("Semester")
                strSesi = SiteData.Tables(0).Rows(i).Item("Sesi")
                strKodKursus = SiteData.Tables(0).Rows(i).Item("KodKursus")
                strNama = SiteData.Tables(0).Rows(i).Item("Nama")

                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("AngkaGiliran")) Then
                    strAngkaGiliran = SiteData.Tables(0).Rows(i).Item("AngkaGiliran")
                Else
                    strAngkaGiliran = ""
                End If
                strMykad = SiteData.Tables(0).Rows(i).Item("Mykad")
                strPengajian = SiteData.Tables(0).Rows(i).Item("Pengajian")
                strEmail = SiteData.Tables(0).Rows(i).Item("Email")
                strJantina = SiteData.Tables(0).Rows(i).Item("Jantina")
                strKaum = SiteData.Tables(0).Rows(i).Item("Kaum")
                strAgama = SiteData.Tables(0).Rows(i).Item("Agama")
                strTel = SiteData.Tables(0).Rows(i).Item("Tel")

                strSQL = "SELECT KursusID FROM kpmkv_kursus WHERE KodKursus='" & strKodKursus & "' AND Tahun='" & strTahun & "' AND Sesi='" & strSesi & "'"
                Dim strKursusID As String = oCommon.getFieldValue(strSQL)

                strSQL = "INSERT INTO kpmkv_pelajar (KolejRecordID,KursusID,Tahun,Semester,Sesi,Nama,AngkaGiliran,Mykad,Pengajian,Jantina,Kaum,Agama,Tel,Email,StatusID,JenisCalonID,IsDeleted)"
                strSQL += " VALUES ('" & lblKolejRecordID.Text & "','" & strKursusID & "','" & strTahun & "','" & strSemester & "','" & strSesi & "','" & oCommon.FixSingleQuotes(strNama) & "','" & oCommon.FixSingleQuotes(strAngkaGiliran) & "','" & oCommon.FixSingleQuotes(strMykad) & "','" & oCommon.FixSingleQuotes(strPengajian) & "','" & oCommon.FixSingleQuotes(strJantina) & "','" & oCommon.FixSingleQuotes(strKaum) & "','" & oCommon.FixSingleQuotes(strAgama) & "','" & oCommon.FixSingleQuotes(strTel) & "','" & oCommon.FixSingleQuotes(strEmail) & "','2','2','N')"
                strRet = oCommon.ExecuteSQL(strSQL)
                'Response.Write(strSQL)
                If strRet = "0" Then
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE Mykad='" & strMykad & "'"
                    Dim strPelajarID As Integer = oCommon.getFieldValue(strSQL)
                    strSQL = "INSERT INTO kpmkv_pelajar_markah (PelajarID,KolejRecordID,KursusID,Tahun,Semester,Sesi)"
                    strSQL += " VALUES ('" & strPelajarID & "','" & lblKolejRecordID.Text & "','" & strKursusID & "','" & strTahun & "','" & strSemester & "','" & strSesi & "')"
                    strRet = oCommon.ExecuteSQL(strSQL)

                    divMsg.Attributes("class") = "info"
                    lblMsg.Text = "Calon berjaya didaftarkan"
                Else
                    strSQL = "DELETE kpmkv_pelajar WHERE KolejRecordID='" & lblKolejRecordID.Text & "' AND KursusID='" & strKursusID & "' AND Tahun='" & strTahun & "' AND Semester='" & strSemester & "'"
                    strSQL = " AND Sesi='" & strSesi & "' AND Mykad='" & oCommon.FixSingleQuotes(strMykad) & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon tidak berjaya didaftarkan"
                    ' Exit For
                End If

            Next

        Catch ex As Exception
            divMsg.Attributes("class") = "error"
            lblMsg.Text = "Calon tidak berjaya didaftarkan"
            Return False
        End Try

        Return True

    End Function
End Class