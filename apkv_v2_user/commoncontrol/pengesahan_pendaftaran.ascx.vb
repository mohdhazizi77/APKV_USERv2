Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.pdf.draw
Imports System.Globalization


Public Class pengesahan_pendaftaran1
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String
    Dim strRet As String
    Dim IntTakwim As Integer = 0
    Dim strVkohort As String
    Dim strVSemester As String
    Dim strVSesi As String
    Dim strVKod As String

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then

                kpmkv_tahun_2_list()
                ddlTahun_Semasa.SelectedValue = Now.Year
                lblMsg.Text = ""
                'lblMsgResult.Text = ""

                strVkohort = Request.QueryString("VKohort")
                strVSemester = Request.QueryString("VSemester")
                strVSesi = Request.QueryString("Sesi")
                strVKod = Request.QueryString("Vkod")

                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                '------exist takwim
                strSQL = "SELECT * FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Pengesahan Pendaftaran' AND Aktif='1'"
                If oCommon.isExist(strSQL) = True Then

                    'count data takwim
                    'Get the data from database into datatable
                    Dim cmd As New SqlCommand("SELECT TakwimID FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Pengesahan Pendaftaran' AND Aktif='1'")
                    Dim dt As DataTable = GetData(cmd)
                    Dim exist As String = "0"

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

                        btnSearch.Enabled = False


                        If strMula IsNot Nothing Then
                            If strAkhir IsNot Nothing And dayDiff >= 0 Then

                                kpmkv_tahun_list()
                                kpmkv_semester_list()
                                exist = "1"

                                If Not strVkohort = "" Then
                                    ddlTahun.SelectedValue = strVkohort
                                    ddlSemester.SelectedValue = strVSemester
                                    'If strVSesi = "1" Then
                                    '    chkSesi.Items.FindByValue("1").Selected = True
                                    'ElseIf strVSesi = "2" Then
                                    '    chkSesi.Items.FindByValue("2").Selected = True
                                    'End If

                                    'kpmkv_kodkursus_list()
                                    'ddlKodKursus.SelectedValue = strVKod
                                End If

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
                                'btnExport.Enabled = True
                                'btnUpdate.Enabled = True

                                lblMsg.Text = ""

                            ElseIf strAkhir IsNot Nothing And dayDiff < 0 And exist = "0" Then
                                'btnExport.Enabled = False
                                'btnUpdate.Enabled = False
                                lblMsg.Text = "Pengesahan Pendaftaran telah ditutup!"
                            End If
                        End If

                        If exist = "1" Then
                            btnSearch.Enabled = True
                        End If
                    Next
                Else
                    'btnExport.Enabled = False
                    'btnUpdate.Enabled = False
                    lblMsg.Text = "Pengesahan Pendaftaran telah ditutup!"
                End If
                RepoveDuplicate(ddlTahun)
                RepoveDuplicate(ddlSemester)

                'kpmkv_tahun_list()
                'ddlTahun.Text = Now.Year

                kpmkv_kelas_list()
                ddlNamaKelas.Text = "0"

                kpmkv_kodkursus_list()
                ddlKodKursus.Text = "0"

                ddlStatus.SelectedValue = "2"

                'strRet = BindData(datRespondent)
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
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

    Private Sub kpmkv_tahun_2_list()
        strSQL = "SELECT Tahun FROM kpmkv_tahun ORDER BY TahunID"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlTahun_Semasa.DataSource = ds
            ddlTahun_Semasa.DataTextField = "Tahun"
            ddlTahun_Semasa.DataValueField = "Tahun"
            ddlTahun_Semasa.DataBind()

        Catch ex As Exception

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Sub kpmkv_kodkursus_list()

        strSQL = "SELECT kpmkv_kursus.KodKursus, kpmkv_kursus.KursusID FROM kpmkv_kursus_kolej LEFT OUTER JOIN"
        strSQL += " kpmkv_kursus ON kpmkv_kursus_kolej.KursusID = kpmkv_kursus.KursusID"
        strSQL += " WHERE kpmkv_kursus_kolej.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kursus.Tahun='" & ddlTahun.SelectedValue & "' "
        strSQL += " AND kpmkv_kursus.Sesi='" & chkSesi.SelectedValue & "' GROUP BY kpmkv_kursus.KodKursus, kpmkv_kursus.KursusID ORDER BY kpmkv_kursus.KodKursus"
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
        strSQL += " WHERE kpmkv_kelas.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kelas_kursus.KursusID= '" & ddlKodKursus.SelectedValue & "' ORDER BY  kpmkv_kelas.NamaKelas"
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


        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        lblMsg.Text = ""
        strRet = BindData(datRespondent)
        tbl_menu_check()
    End Sub

    Private Sub tbl_menu_check()

        Dim str As String
        For i As Integer = 0 To datRespondent.Rows.Count - 1
            Dim row As GridViewRow = datRespondent.Rows(0)
            Dim cb As CheckBox = datRespondent.Rows(i).FindControl("chkSelect")

            str = datRespondent.DataKeys(i).Value.ToString
            Dim strMykad As String = CType(datRespondent.Rows(i).FindControl("Mykad"), Label).Text

            strSQL = "SELECT KelasID FROM kpmkv_pelajar Where Mykad='" & strMykad & "' AND IsDeleted='N' AND KelasID IS NOT NULL"
            If oCommon.isExist(strSQL) = False Then
                cb.Checked = True
            End If
        Next

    End Sub

    Protected Sub CheckUncheckAll(sender As Object, e As System.EventArgs)
        Dim chk1 As CheckBox
        chk1 = DirectCast(datRespondent.HeaderRow.Cells(0).FindControl("chkAll"), CheckBox)
        For Each row As GridViewRow In datRespondent.Rows
            Dim chk As CheckBox
            chk = DirectCast(row.Cells(0).FindControl("chkSelect"), CheckBox)
            chk.Checked = chk1.Checked
        Next
    End Sub

    Private Function BindData(ByVal gvTable As GridView) As Boolean
        Dim myDataSet As New DataSet
        Dim myDataAdapter As New SqlDataAdapter(getSQL, strConn)
        myDataAdapter.SelectCommand.CommandTimeout = 120

        Try
            myDataAdapter.Fill(myDataSet, "myaccount")

            If myDataSet.Tables(0).Rows.Count = 0 Then
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Rekod tidak dijumpai!"
            Else
                divMsg.Attributes("class") = "info"
                lblMsg.Text = "Jumlah Rekod#:" & myDataSet.Tables(0).Rows.Count
            End If

            gvTable.DataSource = myDataSet
            gvTable.DataBind()
            objConn.Close()
        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
            Return False
        End Try

        Return True

    End Function

    Private Function getSQL() As String
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY kpmkv_pelajar.Tahun, kpmkv_pelajar.Semester, kpmkv_pelajar.Sesi, kpmkv_pelajar.Nama ASC"

        '--not deleted
        tmpSQL = "SELECT kpmkv_pelajar.PelajarID, kpmkv_pelajar.Tahun, kpmkv_pelajar.Semester, kpmkv_pelajar.Sesi, kpmkv_pelajar.Nama,"
        tmpSQL += " kpmkv_pelajar.MYKAD, kpmkv_pelajar.AngkaGiliran,kpmkv_pelajar.IsSahkan, "
        tmpSQL += " kpmkv_kursus.KodKursus, kpmkv_pelajar.Kaum, kpmkv_pelajar.Jantina, kpmkv_pelajar.Agama, kpmkv_status.Status, kpmkv_kelas.NamaKelas"
        tmpSQL += " FROM  kpmkv_pelajar LEFT OUTER JOIN kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
        tmpSQL += " kpmkv_status ON kpmkv_pelajar.StatusID = kpmkv_status.StatusID LEFT OUTER JOIN kpmkv_kelas ON kpmkv_pelajar.KelasID = kpmkv_kelas.KelasID"
        strWhere = " WHERE kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.StatusID='2' AND kpmkv_pelajar.KolejRecordID='" & lblKolejID.Text & "'"


        '--tahun
        If Not ddlTahun.Text = "" Then
            strWhere += " AND kpmkv_pelajar.Tahun ='" & ddlTahun.Text & "'"
        End If
        '--semester
        If Not ddlSemester.Text = "" Then
            strWhere += " AND kpmkv_pelajar.Semester ='" & ddlSemester.Text & "'"
        End If
        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere += " AND kpmkv_pelajar.Sesi ='" & chkSesi.Text & "'"
        End If
        '--kodkursus
        If Not ddlKodKursus.Text = "" Then
            strWhere += " AND kpmkv_pelajar.KursusID ='" & ddlKodKursus.SelectedValue & "'"
        End If

        '--txtNama
        If Not txtNama.Text.Length = 0 Then
            strWhere += " AND kpmkv_pelajar.Nama LIKE '%" & oCommon.FixSingleQuotes(txtNama.Text) & "%'"
        End If

        '--txtMYKAD
        If Not txtMYKAD.Text.Length = 0 Then
            strWhere += " AND kpmkv_pelajar.MYKAD='" & oCommon.FixSingleQuotes(txtMYKAD.Text) & "'"
        End If

        If ddlStatus.SelectedValue = "Y" Then
            strWhere += " AND kpmkv_pelajar.IsSahkan='Y'"
        Else
            strWhere += " AND (kpmkv_pelajar.IsSahkan IS NULL or kpmkv_pelajar.IsSahkan='')"
        End If

        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        'Response.Write(getSQL)

        Return getSQL

    End Function

    Private Sub datRespondent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondent.PageIndexChanging
        datRespondent.PageIndex = e.NewPageIndex
        strRet = BindData(datRespondent)

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
            con.Close()
            sda.Dispose()
            con.Dispose()
        End Try
    End Function

    Protected Sub chkSesi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkSesi.SelectedIndexChanged
        kpmkv_kodkursus_list()
        kpmkv_kelas_list()
    End Sub

    Private Sub ddlKodKursus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlKodKursus.SelectedIndexChanged
        kpmkv_kelas_list()
    End Sub

    Private Sub ddlStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlStatus.SelectedIndexChanged
        strRet = BindData(datRespondent)
    End Sub



    'sahkan pengesahan pendaftran
    Protected Sub btnSah_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSah.Click
        Try
            For i As Integer = 0 To datRespondent.Rows.Count - 1

                Dim cb As CheckBox = datRespondent.Rows(i).FindControl("chkSelect")

                If cb.Checked = True Then

                    Dim strkey As String = datRespondent.DataKeys(i).Value.ToString

                    strSQL = " UPDATE kpmkv_pelajar SET IsSahkan='Y' WHERE PelajarID='" & strkey & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)

                    If Not strRet = 0 Then
                        divMsgTop.Attributes("class") = "error"
                        lblMsgTop.Text = "Pengesahan maklumat pelajar tidak berjaya dikemaskini"

                        divMsg.Attributes("class") = "error"
                        lblMsg.Text = "Pengesahan maklumat pelajar tidak berjaya dikemaskini"
                    End If
                End If
            Next
            divMsgTop.Attributes("class") = "info"
            lblMsgTop.Text = "Pengesahan maklumat pelajar berjaya dikemaskini"

            divMsg.Attributes("class") = "info"
            lblMsg.Text = "Pengesahan maklumat pelajar berjaya dikemaskini"
            strRet = BindData(datRespondent)
        Catch ex As Exception
            divMsg.Attributes("class") = "error"
            lblMsg.Text = "System Error. " & ex.Message
        End Try


    End Sub

    'batalkan pengesahan
    Protected Sub btnBatal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBatal.Click
        Try
            For i As Integer = 0 To datRespondent.Rows.Count - 1

                Dim cb As CheckBox = datRespondent.Rows(i).FindControl("chkSelect")

                If cb.Checked = True Then

                    Dim strkey As String = datRespondent.DataKeys(i).Value.ToString

                    strSQL = " UPDATE kpmkv_pelajar SET IsSahkan='' WHERE PelajarID='" & strkey & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)

                    If Not strRet = 0 Then
                        divMsg.Attributes("class") = "error"
                        lblMsgTop.Text = "Pengesahan maklumat pelajar tidak berjaya dikemaskini"
                        lblMsg.Text = "Pengesahan maklumat pelajar tidak berjaya dikemaskini"
                    End If
                End If
            Next
            divMsg.Attributes("class") = "info"
            lblMsgTop.Text = "Pengesahan maklumat pelajar berjaya dikemaskini"
            lblMsg.Text = "Pengesahan maklumat pelajar berjaya dikemaskini"
            strRet = BindData(datRespondent)

        Catch ex As Exception
            divMsg.Attributes("class") = "error"
            lblMsg.Text = "System Error." & ex.Message
        End Try


    End Sub

    'download pdf kenyataan
    Protected Sub btnprint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click
        Try

            Dim tableColumn As DataColumnCollection
            Dim tableRows As DataRowCollection

            Dim myDataSet As New DataSet
            Dim myDataAdapter As New SqlDataAdapter(getSQL, strConn)
            myDataAdapter.Fill(myDataSet, "Kenyataan Kemasukan Semester")
            myDataAdapter.SelectCommand.CommandTimeout = 80000
            objConn.Close()

            '--transfer to an object
            tableColumn = myDataSet.Tables(0).Columns
            tableRows = myDataSet.Tables(0).Rows

            CreatePDF(tableColumn, tableRows)

        Catch ex As Exception
            '--display on screen
            lblMsg.Text = "System Error." & ex.Message
        End Try
    End Sub

    Private Sub CreatePDF(ByVal tableColumns As DataColumnCollection, ByVal tableRows As DataRowCollection)
        Dim myDocument As New Document(PageSize.A4)

        Try
            HttpContext.Current.Response.ContentType = "application/pdf"
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=KenyataanKemasukan.pdf")
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)

            PdfWriter.GetInstance(myDocument, HttpContext.Current.Response.OutputStream)

            myDocument.Open()

            '1'--start here
            strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
            Dim strKolejnama As String = oCommon.getFieldValue(strSQL)

            'kolejnegeri
            strSQL = "SELECT Negeri FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
            Dim strKolejnegeri As String = oCommon.getFieldValue(strSQL)


            For i As Integer = 0 To datRespondent.Rows.Count - 1

                Dim cb As CheckBox = datRespondent.Rows(i).FindControl("chkSelect")

                If cb.Checked = True Then

                    Dim strkey As String = datRespondent.DataKeys(i).Value.ToString

                    strSQL = " SELECT kpmkv_pelajar.Nama, kpmkv_pelajar.MYKAD, kpmkv_pelajar.AngkaGiliran,"
                    strSQL += " kpmkv_kursus.KodKursus, kpmkv_kursus.NamaKursus, kpmkv_pelajar.Kaum, kpmkv_pelajar.Jantina,"
                    strSQL += "  kpmkv_pelajar.Agama, kpmkv_kluster .NamaKluster "
                    strSQL += " FROM  kpmkv_pelajar "
                    strSQL += " Left JOIN kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID "
                    strSQL += " LEFT JOIN kpmkv_kluster ON kpmkv_kursus.KlusterID =kpmkv_kluster .KlusterID "
                    strSQL += " WHERE kpmkv_pelajar.PelajarID='" & oCommon.FixSingleQuotes(strkey) & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)

                    Dim ar_info As Array
                    ar_info = strRet.Split("|")

                    Dim strname As String = ar_info(0)
                    Dim strmykad As String = ar_info(1)
                    Dim strag As String = ar_info(2)
                    Dim strkodKursus As String = ar_info(3)
                    Dim strprogram As String = ar_info(4)
                    Dim strkaum As String = ar_info(5)
                    Dim strjantina As String = ar_info(6)
                    Dim stragama As String = ar_info(7).trim()
                    Dim strbidang As String = ar_info(8)
                    ''getting data end

                    Dim table As New PdfPTable(3)
                    table.WidthPercentage = 100
                    table.SetWidths({42, 16, 42})
                    table.DefaultCell.Border = 0

                    Dim cell As New PdfPCell()
                    Dim cetak As String
                    Dim myPara001 As New Paragraph()
                    cell.AddElement(myPara001)
                    cell.Border = 0
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    Dim imgdrawSpacing As String = Server.MapPath("~/img/GLogo.gif")
                    Dim imgSpacing As Image = Image.GetInstance(imgdrawSpacing)
                    imgSpacing.Alignment = Image.MIDDLE_ALIGN
                    imgSpacing.Border = 0
                    cell.AddElement(imgSpacing)
                    cell.Border = 0
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    Dim siri As New Paragraph("", FontFactory.GetFont("Arial", 10))
                    siri.Alignment = Element.ALIGN_RIGHT
                    cell.AddElement(siri)
                    cell.Border = 0
                    table.AddCell(cell)

                    myDocument.Add(table)

                    cetak = "LEMBAGA PEPERIKSAAN"
                    cetak += Environment.NewLine & "KEMENTERIAN PENDIDIKAN MALAYSIA"
                    cetak += Environment.NewLine & "SIJIL VOKASIONAL MALAYSIA (SVM)"
                    cetak += Environment.NewLine & "TAHUN PEPERIKSAAN : SEMESTER " & ddlSemester.SelectedValue & ", SESI " & ddlSesi_Semasa.SelectedValue & ", TAHUN " & ddlTahun_Semasa.SelectedValue & Environment.NewLine
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 10))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    myDocument.Add(myPara001)
                    Debug.WriteLine(cetak)


                    cetak = "KENYATAAN KEMASUKAN SEMESTER"
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 10, Font.BOLD))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    myDocument.Add(myPara001)
                    Debug.WriteLine(cetak)

                    ''PROFILE STARTS HERE

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    table = New PdfPTable(2)

                    table.WidthPercentage = 100
                    table.SetWidths({30, 70})

                    cell = New PdfPCell()
                    cetak = "Nama Sekolah / Pusat"
                    cetak += Environment.NewLine & "Negeri"
                    cetak += Environment.NewLine & "Angka Giliran"
                    cetak += Environment.NewLine & "Nama Calon"
                    cetak += Environment.NewLine & "No. Pengenalan Diri"
                    cetak += Environment.NewLine & "Nama Bidang"
                    cetak += Environment.NewLine & "Program"
                    cetak += Environment.NewLine & "Jantina"
                    cetak += Environment.NewLine & "Agama"
                    cetak += Environment.NewLine & ""

                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
                    cell.Border = 0
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ": " & strKolejnama
                    cetak += Environment.NewLine & ": " & strKolejnegeri
                    cetak += Environment.NewLine & ": " & strag
                    cetak += Environment.NewLine & ": " & strname
                    cetak += Environment.NewLine & ": " & strmykad
                    cetak += Environment.NewLine & ": " & strbidang
                    cetak += Environment.NewLine & ": " & strprogram & " (" & strkodKursus & ")"
                    cetak += Environment.NewLine & ": " & strjantina
                    cetak += Environment.NewLine & ": " & stragama
                    cetak += Environment.NewLine & " "

                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
                    cell.Border = 0
                    table.AddCell(cell)
                    Debug.WriteLine(cetak)

                    myDocument.Add(table)

                    ''profile ends here
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    table = New PdfPTable(1)
                    table.WidthPercentage = 100
                    table.SetWidths({100})

                    cell = New PdfPCell()
                    cetak = "Mata Pelajaran Yang Diambil"
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10, Font.UNDERLINE)))
                    cell.Border = 0
                    table.AddCell(cell)

                    myDocument.Add(table)


                    table = New PdfPTable(2)
                    table.WidthPercentage = 100
                    table.SetWidths({12, 88})
                    table.DefaultCell.Border = 0

                    ''matapelajaran

                    strSQL = "select KursusID from kpmkv_pelajar where PelajarID = '" & strkey & "'"
                    Dim kursusid As String = oCommon.getFieldValue(strSQL)
                    strSQL = "select Semester from kpmkv_pelajar where PelajarID = '" & strkey & "'"
                    Dim semester As String = oCommon.getFieldValue(strSQL)
                    strSQL = "select Tahun from kpmkv_pelajar where PelajarID = '" & strkey & "'"
                    Dim tahun As String = oCommon.getFieldValue(strSQL)
                    Debug.WriteLine(tahun)

                    cell = New PdfPCell()
                    cetak = ""
                    Dim countsubj As Integer = 0
                    Dim strJenisKursusMT As String = ""

                    If (ddlSemester.SelectedValue = "3" Or ddlSemester.SelectedValue = "4") Then
                        strSQL = "SELECT  JenisKursus From kpmkv_kursus Where KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strJenisKursusMT = oCommon.getFieldValue(strSQL)
                    End If

                    strSQL = "SELECT KodMataPelajaran, NamaMataPelajaran from kpmkv_matapelajaran where Tahun ='" & tahun & "' and semester = '" & semester & "'"
                    If stragama = "ISLAM" Then
                        strSQL += " AND NamaMataPelajaran NOT like ('PENDIDIKAN MORAL%')"
                    Else
                        strSQL += " AND NamaMataPelajaran NOT like ('PENDIDIKAN ISLAM%')"
                    End If
                    If Not strJenisKursusMT = "" Then
                        If strJenisKursusMT = "TECHNOLOGY" Then
                            strSQL += " AND (Jenis = 'TEKNOLOGI' OR Jenis IS NULL OR Jenis = '')"
                        ElseIf strJenisKursusMT = "SOCIAL" Then
                            strSQL += " AND (Jenis ='SOCIAL' OR Jenis IS NULL OR Jenis = '')"
                        End If
                    End If
                    strSQL += " ORDER BY KodMataPelajaran"
                    strRet = oCommon.ExecuteSQL(strSQL)

                    Dim sqlDA As New SqlDataAdapter(strSQL, objConn)
                    Dim ds As DataSet = New DataSet
                    sqlDA.Fill(ds, "AnyTable")


                    Dim cetakNum As String = ""
                    Dim subj As Integer = ds.Tables(0).Rows.Count - 1
                    For iloop As Integer = 0 To subj
                        Dim subjcode As String = (ds.Tables(0).Rows(iloop).Item(0).ToString())
                        subjcode = subjcode.Replace(vbCr, "").Replace(vbLf, "")

                        countsubj += 1

                        cetakNum += countsubj & ". " & ds.Tables(0).Rows(iloop).Item(0).ToString & " -" & Environment.NewLine

                        cetak += ds.Tables(0).Rows(iloop).Item(1).ToString & Environment.NewLine

                    Next

                    strSQL = "SELECT KodMPVOK,NamaMPVOK FROM kpmkv_matapelajaran_v WHERE Tahun = '" & tahun & "'"
                    strSQL += " AND KursusID='" & kursusid & "' AND Semester='" & semester & "'"

                    sqlDA = New SqlDataAdapter(strSQL, objConn)
                    ds = New DataSet
                    sqlDA.Fill(ds, "AnyTable")

                    Dim subj2 As Integer = ds.Tables(0).Rows.Count - 1

                    For iloop2 As Integer = 0 To subj2

                        countsubj += 1

                        cetakNum += countsubj & ". " & ds.Tables(0).Rows(iloop2).Item(0).ToString & " - " & Environment.NewLine

                        cetak += ds.Tables(0).Rows(iloop2).Item(1).ToString & Environment.NewLine

                    Next

                    cell = New PdfPCell()
                    Debug.WriteLine(cetakNum)
                    myPara001 = New Paragraph(cetakNum, FontFactory.GetFont("Arial", 9))
                    myPara001.Alignment = Element.ALIGN_LEFT
                    cell.AddElement(myPara001)
                    cell.Border = 0
                    cell.VerticalAlignment = Element.ALIGN_LEFT
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    Debug.WriteLine(cetak)
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 9))
                    myPara001.Alignment = Element.ALIGN_LEFT
                    cell.AddElement(myPara001)
                    cell.Border = 0
                    cell.VerticalAlignment = Element.ALIGN_LEFT
                    table.AddCell(cell)

                    myDocument.Add(table)



                    table = New PdfPTable(1)
                    table.WidthPercentage = 100
                    table.SetWidths({100})

                    cell = New PdfPCell()
                    cetak = "Kursus yang diambil"
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10, Font.UNDERLINE)))
                    cell.Border = 0
                    table.AddCell(cell)

                    myDocument.Add(table)

                    table = New PdfPTable(2)
                    table.WidthPercentage = 100
                    table.SetWidths({12, 88})
                    table.DefaultCell.Border = 0

                    Dim countsubj2 As Integer = 0

                    ''modul
                    strSQL = "select KodModul,NamaModul from kpmkv_modul where Tahun = '" & tahun & "' and KursusID='" & kursusid & "' and Semester='" & semester & "'"

                    sqlDA = New SqlDataAdapter(strSQL, objConn)
                    ds = New DataSet
                    sqlDA.Fill(ds, "Subject2")

                    Dim subj3 As Integer = ds.Tables(0).Rows.Count - 1
                    cetakNum = ""
                    Dim newLine As String = ""
                    cetak = ""

                    For iloop2 As Integer = 0 To subj3

                        countsubj2 += 1

                        cetakNum += countsubj2 & ". " & ds.Tables(0).Rows(iloop2).Item(0).ToString & " -" & Environment.NewLine

                        cetak += ds.Tables(0).Rows(iloop2).Item(1).ToString & Environment.NewLine

                    Next

                    cell = New PdfPCell()
                    Debug.WriteLine(cetakNum)
                    myPara001 = New Paragraph(cetakNum, FontFactory.GetFont("Arial", 9))
                    myPara001.Alignment = Element.ALIGN_LEFT
                    cell.AddElement(myPara001)
                    cell.Border = 0
                    cell.VerticalAlignment = Element.ALIGN_LEFT
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    Debug.WriteLine(cetak)
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 9))
                    myPara001.Alignment = Element.ALIGN_LEFT
                    cell.AddElement(myPara001)
                    cell.Border = 0
                    cell.VerticalAlignment = Element.ALIGN_LEFT
                    table.AddCell(cell)

                    myDocument.Add(table)

                    cetak = Environment.NewLine
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 10))
                    myDocument.Add(myPara001)

                    cetak = "Jumlah Mata Pelajaran : " & countsubj & Environment.NewLine
                    cetak += Environment.NewLine & "Saya akui telah menyemak maklumat pendaftaran peperiksaan di atas dan didapati betul dan tidak akan menukar matapelajaran/kertas selepas pendaftaran ini."
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 10))
                    myDocument.Add(myPara001)


                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    table = New PdfPTable(3)
                    table.WidthPercentage = 100
                    table.SetWidths({35, 30, 35})
                    table.DefaultCell.Border = 0

                    cell = New PdfPCell()
                    cetak = "______________________"
                    cetak += Environment.NewLine & "Ibubapa/Penjaga"
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 10))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = 0
                    cell.VerticalAlignment = Element.ALIGN_BOTTOM
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = Environment.NewLine & Environment.NewLine
                    cetak += Environment.NewLine & Environment.NewLine
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
                    cell.Border = 0
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = "______________________"
                    cetak += Environment.NewLine & "Tandatangan Calon"
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 10))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = 0
                    cell.VerticalAlignment = Element.ALIGN_BOTTOM
                    table.AddCell(cell)

                    myDocument.Add(table)

                    cetak = "Nama:"
                    cetak += Environment.NewLine & "No Kad Pengenalan:"
                    cetak += Environment.NewLine & "Tarikh: ________________________"
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 10))
                    myDocument.Add(myPara001)

                    cetak = Environment.NewLine & "Lembaga Peperiksaan"
                    cetak += Environment.NewLine & "Kementerian Pendidikan Malaysia"
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 10))
                    myDocument.Add(myPara001)

                    myDocument.NewPage()
                End If

            Next

            myDocument.Close()

            HttpContext.Current.Response.Write(myDocument)
            HttpContext.Current.Response.End()

        Catch ex As Exception

        End Try
    End Sub
End Class