Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Globalization

Public Class import_markah_matapelajaran
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String
    Dim strRet As String

    Dim strNama As String = ""
    Dim strMykad As String = ""
    Dim strAngkaGiliran As String = ""
    Dim strKodKursus As String = ""
    Dim strBM As String = ""
    'Dim strBM1 As String = ""
    'Dim strBM2 As String = ""
    Dim strBM3 As String = ""
    Dim strBI As String = ""
    Dim strScience As String = ""
    Dim strScience1 As String = ""
    Dim strScience2 As String = ""
    Dim strSejarah As String = ""
    Dim strPendidikanIslam As String = ""
    Dim strPendidikanIslam1 As String = ""
    Dim strPendidikanIslam2 As String = ""
    Dim strPendidikanMoral As String = ""
    Dim strMathematics As String = ""
    Dim IntTakwim As Integer = 0

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblMsg.Text = ""

        If Not IsPostBack Then
            divImport.Visible = False

            'kolejnama
            strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
            Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
            'kolejid
            strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
            lblKolejID.Text = oCommon.getFieldValue(strSQL)

            '------exist takwim
            strSQL = "SELECT * FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Import Markah Akademik' AND Aktif='1'"
            If oCommon.isExist(strSQL) = True Then

                'count data takwim
                'Get the data from database into datatable
                Dim cmd As New SqlCommand("SELECT TakwimID FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Import Markah Akademik' AND Aktif='1'")
                Dim dt As DataTable = GetData(cmd)

                For i As Integer = 0 To dt.Rows.Count - 1
                    IntTakwim = dt.Rows(i)("TakwimID")

                    strSQL = "SELECT TarikhMula,TarikhAkhir FROM kpmkv_takwim WHERE TakwimID='" & IntTakwim & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)

                    Dim ar_user_login As Array
                    ar_user_login = strRet.Split("|")
                    Dim strMula As String = ar_user_login(0)
                    Dim strAkhir As String = ar_user_login(1)

                    Dim strdateNow As Date = Date.Now
                    Dim startDate = DateTime.ParseExact(strMula, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                    Dim endDate = DateTime.ParseExact(strAkhir, "dd-MM-yyyy", CultureInfo.InvariantCulture)

                    Dim ts As New TimeSpan
                    ts = endDate.Subtract(strdateNow)
                    Dim dayDiff = ts.Days

                    If strMula IsNot Nothing Then
                        If strAkhir IsNot Nothing And dayDiff >= 0 Then
                            kpmkv_tahun_list()
                            kpmkv_semester_list()

                            'checkinbox
                            strSQL = "SELECT Sesi FROM kpmkv_takwim WHERE TakwimId='" & IntTakwim & "'ORDER BY Kohort ASC"
                            strRet = oCommon.getFieldValue(strSQL)

                            If strRet = 1 Then
                                chkSesi.Items(0).Enabled = True
                                'chkSesi.Items(1).Enabled = False
                            Else
                                'chkSesi.Items(0).Enabled = False
                                chkSesi.Items(1).Enabled = True
                            End If
                            btnFile.Enabled = True
                            btnUpload.Enabled = True
                        End If
                    Else
                        btnFile.Enabled = False
                        btnUpload.Enabled = False
                        lblMsg.Text = "Import Markah Akademik telah ditutup!"
                    End If
                Next
            Else
                btnFile.Enabled = False
                btnUpload.Enabled = False
                lblMsg.Text = "Import Markah Akademik telah ditutup!"
            End If
            RepoveDuplicate(ddlTahun)
            RepoveDuplicate(ddlSemester)
        End If

    End Sub
    Private Sub kpmkv_tahun_list()
         strSQL = "SELECT Kohort FROM kpmkv_takwim WHERE TakwimId='" & IntTakwim & "'ORDER BY Kohort ASC"
        strRet = oCommon.getFieldValue(strSQL)
        Try
            If Not ddlTahun.Text = strRet Then
                ddlTahun.Items.Add(strRet)
            End If
        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_semester_list()
        strSQL = "SELECT Semester FROM kpmkv_takwim WHERE TakwimId='" & IntTakwim & "'ORDER BY Semester ASC"
        strRet = oCommon.getFieldValue(strSQL)
        Try
            If Not ddlSemester.Text = strRet Then
                ddlSemester.Items.Add(strRet)
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Shared Function RepoveDuplicate(ByVal ddl As DropDownList) As DropDownList
        For Row As Int16 = 0 To ddl.Items.Count - 2
            For RowAgain As Int16 = ddl.Items.Count - 1 To Row + 1 Step -1
                If ddl.Items(Row).ToString = ddl.Items(RowAgain).ToString Then
                    ddl.Items.RemoveAt(RowAgain)
                End If
            Next
        Next
        Return ddl
    End Function
    Private Sub kpmkv_kodkursus_list()

        strSQL = "SELECT kpmkv_kursus.KodKursus, kpmkv_kursus.KursusID"
        strSQL += " FROM kpmkv_kelas_kursus INNER JOIN kpmkv_kursus ON kpmkv_kelas_kursus.KursusID = kpmkv_kursus.KursusID INNER JOIN"
        strSQL += " kpmkv_kelas ON kpmkv_kelas_kursus.KelasID = kpmkv_kelas.KelasID"
        strSQL += " WHERE kpmkv_kelas.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kursus.Tahun='" & ddlTahun.Text & "' AND kpmkv_kursus.Sesi='" & chkSesi.SelectedValue & "' ORDER BY kpmkv_kursus.KodKursus"
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
            ddlKelas.Items.Add(New ListItem("-Pilih-", "0"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Function BindData(ByVal gvTable As GridView) As Boolean
        Dim myDataSet As New DataSet
        Dim myDataAdapter As New SqlDataAdapter(getSQL, strConn)
        myDataAdapter.SelectCommand.CommandTimeout = 120
        Try
            myDataAdapter.Fill(myDataSet, "myaccount")

            If myDataSet.Tables(0).Rows.Count = 0 Then
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Tiada rekod pelajar."
            Else
                divMsg.Attributes("class") = "info"
                lblMsg.Text = "Jumlah rekod#:" & myDataSet.Tables(0).Rows.Count
            End If

            gvTable.DataSource = myDataSet
            gvTable.DataBind()
            objConn.Close()

        Catch ex As Exception
            lblMsg.Text = "Error:" & ex.Message
            Return False
        End Try

        Return True

    End Function

    Private Function getSQL() As String

        Dim tmpSQL As String = ""
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY kpmkv_pelajar.Nama ASC"

        If chkResult.Text = "PB" Then
            '--not deleted
            tmpSQL = "SELECT kpmkv_pelajar.Nama, kpmkv_pelajar.AngkaGiliran,kpmkv_pelajar.MYKAD, kpmkv_kursus.KodKursus AS [KodProgram],"
            tmpSQL += " kpmkv_pelajar_markah.B_BahasaMelayu AS BahasaMelayu, kpmkv_pelajar_markah.B_BahasaInggeris AS BahasaInggeris,kpmkv_pelajar_markah.B_Mathematics AS Matematik,"
            tmpSQL += " kpmkv_pelajar_markah.B_Science1 AS Sains, kpmkv_pelajar_markah.B_Sejarah AS Sejarah, "
            tmpSQL += " kpmkv_pelajar_markah.B_PendidikanIslam1 AS PendidikanIslam, kpmkv_pelajar_markah.B_PendidikanMoral AS PendidikanMoral"
            tmpSQL += " FROM kpmkv_pelajar_markah LEFT OUTER JOIN kpmkv_pelajar ON kpmkv_pelajar_markah.PelajarID = kpmkv_pelajar.PelajarID"
            tmpSQL += " LEFT OUTER Join kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID"
        Else
            tmpSQL = "SELECT kpmkv_pelajar.Nama, kpmkv_pelajar.AngkaGiliran,kpmkv_pelajar.MYKAD, kpmkv_kursus.KodKursus AS [KodProgram],"
            tmpSQL += " kpmkv_pelajar_markah.A_BahasaMelayu AS BahasaMelayu, kpmkv_pelajar_markah.A_BahasaMelayu3 AS BahasaMelayu3, "
            tmpSQL += " kpmkv_pelajar_markah.A_BahasaInggeris AS BahasaInggeris, kpmkv_pelajar_markah.A_Mathematics AS Matematik, kpmkv_pelajar_markah.A_Science1 AS Sains1, kpmkv_pelajar_markah.A_Science2 AS Sains2, "
            tmpSQL += " kpmkv_pelajar_markah.A_Sejarah AS Sejarah, kpmkv_pelajar_markah.A_PendidikanIslam1 AS PendidikanIslam1,"
            tmpSQL += " kpmkv_pelajar_markah.A_PendidikanIslam2 AS PendidikanIslam2, kpmkv_pelajar_markah.A_PendidikanMoral AS PendidikanMoral"
            tmpSQL += " FROM kpmkv_pelajar_markah LEFT OUTER JOIN kpmkv_pelajar ON kpmkv_pelajar_markah.PelajarID = kpmkv_pelajar.PelajarID"
            tmpSQL += " LEFT OUTER Join kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID"
        End If

        strWhere = " WHERE kpmkv_pelajar.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.StatusID='2' AND kpmkv_pelajar.JenisCalonID='2'"

        '--tahun
        If Not ddlTahun.Text = "PILIH" Then
            strWhere += " AND kpmkv_pelajar.Tahun ='" & ddlTahun.Text & "'"
        End If
        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere += " AND kpmkv_pelajar.Sesi ='" & chkSesi.Text & "'"
        End If
        '--semester
        If Not ddlSemester.Text = "" Then
            strWhere += " AND kpmkv_pelajar.Semester ='" & ddlSemester.Text & "'"
        End If
        '--Kod
        If Not ddlKodKursus.Text = "" Then
            strWhere += " AND kpmkv_pelajar.KursusID='" & ddlKodKursus.SelectedValue & "'"
        End If
        '--sesi
        If Not ddlKelas.Text = "" Then
            strWhere += " AND kpmkv_pelajar.KelasID ='" & ddlKelas.SelectedValue & "'"
        End If
        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        ''Response.Write(getSQL)

        Return getSQL

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
    Private Sub btnFile_Click(sender As Object, e As EventArgs) Handles btnFile.Click
        lblMsg.Text = ""
        If chkResult.Text = "PB" Then
            ExportToCSV(getSQL)
            'Response.ContentType = "Application/xlsx"
            'Response.AppendHeader("Content-Disposition", "attachment; filename=PELAJARIMPORT.xlsx")
            'Response.TransmitFile(Server.MapPath("~/sample_data/MATAPELAJARAN-IMPORT.xlsx"))
            'Response.End()
        Else
            ExportToCSV(getSQL)
            'Response.ContentType = "Application/xlsx"
            'Response.AppendHeader("Content-Disposition", "attachment; filename=PELAJARIMPORT.xlsx")
            'Response.TransmitFile(Server.MapPath("~/sample_data/MATAPELAJARAN-IMPORT2.xlsx"))
            'Response.End()

        End If

    End Sub
    Private Sub ExportToCSV(ByVal strQuery As String)
        'Get the data from database into datatable 
        Dim cmd As New SqlCommand(strQuery)
        Dim dt As DataTable = GetData(cmd)

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=markah.csv")
        Response.Charset = ""
        Response.ContentType = "application/text"


        Dim sb As New StringBuilder()
        For k As Integer = 0 To dt.Columns.Count - 1
            'add separator 
            sb.Append(dt.Columns(k).ColumnName + ","c)
        Next

        'append new line 
        sb.Append(vbCr & vbLf)
        For i As Integer = 0 To dt.Rows.Count - 1
            For k As Integer = 0 To dt.Columns.Count - 1
                '--add separator 
                'sb.Append(dt.Rows(i)(k).ToString().Replace(",", ";") + ","c)

                'cleanup here
                If k <> 0 Then
                    sb.Append(",")
                End If

                Dim columnValue As Object = dt.Rows(i)(k).ToString()
                If columnValue Is Nothing Then
                    sb.Append("")
                Else
                    Dim columnStringValue As String = columnValue.ToString()

                    Dim cleanedColumnValue As String = CleanCSVString(columnStringValue)

                    If columnValue.[GetType]() Is GetType(String) AndAlso Not columnStringValue.Contains(",") Then
                        ' Prevents a number stored in a string from being shown as 8888E+24 in Excel. Example use is the AccountNum field in CI that looks like a number but is really a string.
                        cleanedColumnValue = "=" & cleanedColumnValue
                    End If
                    sb.Append(cleanedColumnValue)
                End If

            Next
            'append new line 
            sb.Append(vbCr & vbLf)
        Next
        Response.Output.Write(sb.ToString())
        Response.Flush()
        Response.End()

    End Sub
    Protected Function CleanCSVString(ByVal input As String) As String
        Dim output As String = """" & input.Replace("""", """""").Replace(vbCr & vbLf, " ").Replace(vbCr, " ").Replace(vbLf, "") & """"
        Return output

    End Function
    Private Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        lblMsg.Text = ""
        'lblMsgTop.Text = ""
        'btnApprove.Enabled = True
        Try
            '--upload excel
            If ImportExcel() = True Then
                divMsg.Attributes("class") = "info"
                'tbl_menu_check()
            Else
                ' divMsg.Attributes("class") = "error"
                'lblMsg.Text = "Gagal untuk memuatnaik fail"
            End If
        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        End Try

    End Sub
    Private Function ImportExcel() As Boolean
        lblMsg.Text = ""
        Dim path As String = String.Concat(Server.MapPath("~/inbox/"))

        If FlUploadcsv.HasFile Then
            Dim rand As Random = New Random()
            Dim randNum = rand.Next(1000)
            Dim fullFileName As String = path + oCommon.getRandom + "-" + FlUploadcsv.FileName
            FlUploadcsv.PostedFile.SaveAs(fullFileName)

            '--required ms access engine
            Dim excelConnectionString As String = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & fullFileName & ";Extended Properties=Excel 12.0;")
            'Dim excelConnectionString As String = ("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & fullFileName & ";Extended Properties=Excel 8.0;HDR=YES;")
            'Response.Write("excelConnectionString:" & excelConnectionString)

            Dim connection As OleDbConnection = New OleDbConnection(excelConnectionString)
            Dim command As OleDbCommand = New OleDbCommand("SELECT * FROM [markah$]", connection)
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
                    lblMsg.Text = "Kesalahan Kemasukkan Maklumat Markah Calon:<br />" & validationMessage
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
            lblMsg.Text = "Please select file to upload!"
            Return False
        End If

        Return True

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
    Protected Function ValidateSiteData(ByVal SiteData As DataSet) As String
        Try
            'Loop through DataSet and validate data
            'If data is bad, bail out, otherwise continue on with the bulk copy
            Dim strMsg As String = ""
            Dim sb As StringBuilder = New StringBuilder()
            For i As Integer = 0 To SiteData.Tables(0).Rows.Count - 1

                refreshVar()
                strMsg = ""

                'nama
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Nama")) Then
                    strNama = SiteData.Tables(0).Rows(i).Item("Nama")
                Else
                    strMsg += "Sila isi Nama|"
                End If

                'mykad
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Mykad")) Then
                    strMykad = SiteData.Tables(0).Rows(i).Item("Mykad")
                Else
                    strMsg += "Sila isi Mykad|"
                End If
                'angka giliran
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("AngkaGiliran")) Then
                    strAngkaGiliran = SiteData.Tables(0).Rows(i).Item("AngkaGiliran")
                Else
                    strMsg += "Sila isi AngkaGiliran|"
                End If
                'kodkursus
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("KodProgram")) Then
                    strKodKursus = SiteData.Tables(0).Rows(i).Item("KodProgram")
                Else
                    strMsg += "Sila isi Kod Program|"
                End If

                'BM
                If IsNumeric(SiteData.Tables(0).Rows(i).Item("BahasaMelayu")) Then
                    strBM = SiteData.Tables(0).Rows(i).Item("BahasaMelayu")
                Else
                    strBM = 0
                End If

                If IsNumeric(SiteData.Tables(0).Rows(i).Item("BahasaInggeris")) Then
                    strBI = SiteData.Tables(0).Rows(i).Item("BahasaInggeris")
                Else
                    strBI = 0
                End If

                If IsNumeric(SiteData.Tables(0).Rows(i).Item("Matematik")) Then
                    strMathematics = SiteData.Tables(0).Rows(i).Item("Matematik")
                Else
                    strMathematics = 0
                End If

                If IsNumeric(SiteData.Tables(0).Rows(i).Item("Sejarah")) Then
                    strSejarah = SiteData.Tables(0).Rows(i).Item("Sejarah")
                Else
                    strSejarah = 0
                End If

                If IsNumeric(SiteData.Tables(0).Rows(i).Item("PendidikanMoral")) Then
                    strPendidikanMoral = SiteData.Tables(0).Rows(i).Item("PendidikanMoral")
                Else
                    strPendidikanMoral = 0
                End If

                '--checkPA
                If chkResult.Text = "PA" Then
                    'If IsNumeric(SiteData.Tables(0).Rows(i).Item("BahasaMelayu1")) Then
                    '    strBM1 = SiteData.Tables(0).Rows(i).Item("BahasaMelayu1")
                    'Else
                    '    strBM1 = 0
                    'End If

                    'If IsNumeric(SiteData.Tables(0).Rows(i).Item("BahasaMelayu2")) Then
                    '    strBM2 = SiteData.Tables(0).Rows(i).Item("BahasaMelayu2")
                    'Else
                    '    strBM2 = 0
                    'End If
                    If IsNumeric(SiteData.Tables(0).Rows(i).Item("BahasaMelayu3")) Then
                        strBM3 = SiteData.Tables(0).Rows(i).Item("BahasaMelayu3")
                    Else
                        strBM3 = 0
                    End If

                    If IsNumeric(SiteData.Tables(0).Rows(i).Item("Sains1")) Then
                        strScience1 = SiteData.Tables(0).Rows(i).Item("Sains1")
                    Else
                        strScience1 = 0
                    End If

                    If IsNumeric(SiteData.Tables(0).Rows(i).Item("Sains2")) Then
                        strScience2 = SiteData.Tables(0).Rows(i).Item("Sains2")
                    Else
                        strScience2 = 0
                    End If

                    If IsNumeric(SiteData.Tables(0).Rows(i).Item("PendidikanIslam1")) Then
                        strPendidikanIslam1 = SiteData.Tables(0).Rows(i).Item("PendidikanIslam1")
                    Else
                        strPendidikanIslam2 = 0
                    End If
                    If IsNumeric(SiteData.Tables(0).Rows(i).Item("PendidikanIslam2")) Then
                        strPendidikanIslam2 = SiteData.Tables(0).Rows(i).Item("PendidikanIslam2")
                    Else
                        strPendidikanIslam2 = 0
                    End If

                End If

                '---check PB
                If chkResult.Text = "PB" Then

                    If IsNumeric(SiteData.Tables(0).Rows(i).Item("Sains")) Then
                        strScience1 = SiteData.Tables(0).Rows(i).Item("Sains")
                    Else
                        strScience1 = 0
                    End If

                    If IsNumeric(SiteData.Tables(0).Rows(i).Item("PendidikanIslam")) Then
                        strPendidikanIslam = SiteData.Tables(0).Rows(i).Item("PendidikanIslam")
                    Else
                        strPendidikanIslam = 0
                    End If

                End If


                If strMsg.Length = 0 Then
                    'strMsg = "Record#:" & i.ToString & "OK"
                    'strMsg += "<br/>"
                Else
                    strMsg = " & strMsg"
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
    Private Function SaveSiteData(ByVal SiteData As DataSet) As String
        lblMsg.Text = ""
        strBM = 0
        'strBM1 = 0
        'strBM2 = 0
        strBM3 = 0
        strBI = 0
        strScience = 0
        strScience1 = 0
        strScience2 = 0
        strSejarah = 0
        strPendidikanIslam = 0
        strPendidikanIslam1 = 0
        strPendidikanIslam2 = 0
        strPendidikanMoral = 0
        strMathematics = 0

        Try
            Dim i As Integer = 0
            Dim sb As StringBuilder = New StringBuilder()
            For i = 0 To SiteData.Tables(0).Rows.Count - 1

                strNama = SiteData.Tables(0).Rows(i).Item("Nama")
                strMykad = SiteData.Tables(0).Rows(i).Item("Mykad")
                strAngkaGiliran = SiteData.Tables(0).Rows(i).Item("AngkaGiliran")
                strKodKursus = SiteData.Tables(0).Rows(i).Item("KodProgram")

                '***KursusID
                strSQL = "SELECT KursusID FROM kpmkv_kursus WHERE KodKursus='" & strKodKursus & "' AND Tahun='" & ddlTahun.Text & "' AND Sesi='" & chkSesi.SelectedValue & "'"
                Dim strKursusID As String = oCommon.getFieldValue(strSQL)

                '*****pelajarid
                strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE MYKAD='" & strMykad & "' AND KursusID='" & strKursusID & "' AND Tahun='" & ddlTahun.Text & "' AND Sesi='" & chkSesi.SelectedValue & "' AND Semester='" & ddlSemester.SelectedValue & "'"
                Dim strPelajarID As String = oCommon.getFieldValue(strSQL)
                If Not String.IsNullOrEmpty(strPelajarID) Then

                    If chkResult.Text = "PB" Then

                        strBM = SiteData.Tables(0).Rows(i).Item("BahasaMelayu")
                        strBI = SiteData.Tables(0).Rows(i).Item("BahasaInggeris")
                        strMathematics = SiteData.Tables(0).Rows(i).Item("Matematik")
                        strScience = SiteData.Tables(0).Rows(i).Item("Sains")
                        strSejarah = SiteData.Tables(0).Rows(i).Item("Sejarah")
                        strPendidikanIslam = SiteData.Tables(0).Rows(i).Item("PendidikanIslam")
                        strPendidikanMoral = SiteData.Tables(0).Rows(i).Item("PendidikanMoral")

                        strSQL = "UPDATE kpmkv_pelajar_markah SET B_BahasaMelayu='" & oCommon.FixSingleQuotes(strBM) & "',B_BahasaInggeris='" & oCommon.FixSingleQuotes(strBI) & "',"
                        strSQL += " B_Mathematics='" & oCommon.FixSingleQuotes(strMathematics) & "',B_Science1='" & oCommon.FixSingleQuotes(strScience) & "', B_Sejarah='" & oCommon.FixSingleQuotes(strSejarah) & "',"
                        strSQL += " B_PendidikanIslam1='" & oCommon.FixSingleQuotes(strPendidikanIslam) & "', B_PendidikanMoral='" & oCommon.FixSingleQuotes(strPendidikanMoral) & "'"
                        strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' AND PelajarID='" & oCommon.FixSingleQuotes(strPelajarID) & "'"
                        strSQL += " AND Tahun='" & oCommon.FixSingleQuotes(ddlTahun.Text) & "' AND Semester='" & oCommon.FixSingleQuotes(ddlSemester.Text) & "'"
                        strSQL += " AND Sesi='" & oCommon.FixSingleQuotes(chkSesi.Text) & "' AND KursusID='" & oCommon.FixSingleQuotes(strKursusID) & "'"

                        strRet = oCommon.ExecuteSQL(strSQL)

                    Else

                        strBM = SiteData.Tables(0).Rows(i).Item("BahasaMelayu")
                        'strBM1 = SiteData.Tables(0).Rows(i).Item("BahasaMelayu1")
                        'strBM2 = SiteData.Tables(0).Rows(i).Item("BahasaMelayu2")
                        strBM3 = SiteData.Tables(0).Rows(i).Item("BahasaMelayu3")
                        strBI = SiteData.Tables(0).Rows(i).Item("BahasaInggeris")
                        strMathematics = SiteData.Tables(0).Rows(i).Item("Matematik")
                        strScience1 = SiteData.Tables(0).Rows(i).Item("Sains1")
                        strScience2 = SiteData.Tables(0).Rows(i).Item("Sains2")
                        strSejarah = SiteData.Tables(0).Rows(i).Item("Sejarah")
                        strPendidikanIslam1 = SiteData.Tables(0).Rows(i).Item("PendidikanIslam1")
                        strPendidikanIslam2 = SiteData.Tables(0).Rows(i).Item("PendidikanIslam2")
                        strPendidikanMoral = SiteData.Tables(0).Rows(i).Item("PendidikanMoral")


                        strSQL = "UPDATE kpmkv_pelajar_markah SET A_BahasaMelayu='" & oCommon.FixSingleQuotes(strBM) & "', "
                        strSQL += " A_BahasaMelayu3='" & oCommon.FixSingleQuotes(strBM3) & "', A_BahasaInggeris='" & oCommon.FixSingleQuotes(strBI) & "',A_Mathematics='" & oCommon.FixSingleQuotes(strMathematics) & "',"
                        strSQL += " A_Science1='" & oCommon.FixSingleQuotes(strScience1) & "',  A_Science2='" & strScience2 & "',"
                        strSQL += " A_Sejarah='" & oCommon.FixSingleQuotes(strSejarah) & "', A_PendidikanIslam1='" & oCommon.FixSingleQuotes(strPendidikanIslam1) & "',A_PendidikanIslam2='" & oCommon.FixSingleQuotes(strPendidikanIslam2) & "', "
                        strSQL += " A_PendidikanMoral='" & oCommon.FixSingleQuotes(strPendidikanMoral) & "'"
                        strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' AND PelajarID='" & oCommon.FixSingleQuotes(strPelajarID) & "'"
                        strSQL += " AND Tahun='" & oCommon.FixSingleQuotes(ddlTahun.Text) & "' AND Semester='" & oCommon.FixSingleQuotes(ddlSemester.Text) & "'"
                        strSQL += " AND Sesi='" & oCommon.FixSingleQuotes(chkSesi.Text) & "' AND KursusID='" & oCommon.FixSingleQuotes(strKursusID) & "'"
                        strRet = oCommon.ExecuteSQL(strSQL)
                    End If

                    If strRet = "0" Then

                        divMsg.Attributes("class") = "info"
                        lblMsg.Text = "Markah berjaya di Import"
                    Else
                        divMsg.Attributes("class") = "error"
                        lblMsg.Text = "Markah tidak berjaya di Import"
                        Return False
                        Exit For
                    End If
                Else
                End If
            Next

            'Response.Write(strSQL)
        Catch ex As Exception
            divMsg.Attributes("class") = "error"
            'lblMsg.Text = "Calon tidak berjaya didaftarkan"
            'Return False
        End Try
        Return True
    End Function
    Private Sub refreshVar()

        strNama = ""
        strMykad = ""
        strAngkaGiliran = ""
        strKodKursus = ""
        strBM = ""
        'strBM1 = ""
        'strBM2 = ""
        strBM3 = ""
        strBI = ""
        strScience = ""
        strScience1 = ""
        strScience2 = ""
        strSejarah = ""
        strPendidikanIslam = ""
        strPendidikanIslam1 = ""
        strPendidikanIslam2 = ""
        strPendidikanMoral = ""
        strMathematics = ""

    End Sub

    Private Sub chkSesi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkSesi.SelectedIndexChanged
        kpmkv_kodkursus_list()
        kpmkv_kelas_list()
        ddlKelas.Text = "0"
    End Sub

    Private Sub ddlKodKursus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlKodKursus.SelectedIndexChanged
        kpmkv_kelas_list()
        ddlKelas.Text = "0"
    End Sub

    Private Sub chkResult_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkResult.SelectedIndexChanged
        If Not chkResult.Text = "" Then

            divImport.Visible = True

        Else
            divImport.Visible = False

        End If
    End Sub
End Class