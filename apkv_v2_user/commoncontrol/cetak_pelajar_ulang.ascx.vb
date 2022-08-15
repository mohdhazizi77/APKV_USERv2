Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.pdf.draw
Imports System.Globalization

Public Class cetak_pelajar_ulang1

    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""
    Dim IntTakwim As Integer = 0

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Dim SubMenuText As String = "Jana Penyata Pendaftaran Ulang"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                lblMsg.Text = ""


                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                '------exist takwim
                strSQL = "SELECT * FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText = '" & SubMenuText & "' AND Aktif='1' AND GETDATE() BETWEEN CONVERT(date, TarikhMula, 103) AND DATEADD(day,1,CONVERT(date, TarikhAkhir, 103))"

                If oCommon.isExist(strSQL) = True Then

                    kpmkv_tahun_list()
                    kpmkv_semester_list()
                    kpmkv_tahun_2_list()
                    kpmkv_kodkursus_list()
                    kpmkv_kelas_list()

                Else
                    btnSearch.Enabled = False
                    btnPrint.Enabled = False
                    lblMsg.Text = "Jana Penyata Pendaftaran Ulang telah ditutup!"
                End If

            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try
    End Sub

    Private Sub kpmkv_tahun_list()

        strSQL = "  SELECT DISTINCT kpmkv_takwim.Kohort FROM kpmkv_takwim
                    LEFT JOIN kpmkv_takwim_kv ON kpmkv_takwim_kv.TakwimID = kpmkv_takwim.TakwimID
                    WHERE
                    kpmkv_takwim.SubMenuText = '" & SubMenuText & "'
                    AND kpmkv_takwim.Aktif = '1'
                    AND kpmkv_takwim.Tahun = '" & Now.Year & "'
                    AND kpmkv_takwim_kv.TakwimKVID IS NULL
                    AND GETDATE() BETWEEN CONVERT(date, TarikhMula, 103) AND DATEADD(day,1,CONVERT(date, TarikhAkhir, 103))

                    UNION

                    SELECT DISTINCT kpmkv_takwim.Kohort FROM kpmkv_takwim
                    LEFT JOIN kpmkv_takwim_kv ON kpmkv_takwim_kv.TakwimID = kpmkv_takwim.TakwimID
                    WHERE
                    kpmkv_takwim.SubMenuText = '" & SubMenuText & "'
                    AND kpmkv_takwim.Aktif = '1'
                    AND kpmkv_takwim.Tahun = '" & Now.Year & "'
                    AND kpmkv_takwim_kv.TakwimKVID IS NOT NULL
                    AND kpmkv_takwim_kv.KolejRecordID = '" & lblKolejID.Text & "'
                    AND GETDATE() BETWEEN CONVERT(date, TarikhMula, 103) AND DATEADD(day,1,CONVERT(date, TarikhAkhir, 103))"

        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlTahun.DataSource = ds
            ddlTahun.DataTextField = "Kohort"
            ddlTahun.DataValueField = "Kohort"
            ddlTahun.DataBind()

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Sub kpmkv_semester_list()

        strSQL = "  SELECT DISTINCT Semester FROM kpmkv_takwim 
                    WHERE 
                    SubMenuText = '" & SubMenuText & "' 
                    AND Aktif = '1'
                    AND Tahun = '" & Now.Year & "'
                    AND Kohort = '" & ddlTahun.Text & "'
                    AND GETDATE() BETWEEN CONVERT(date, TarikhMula, 103) AND DATEADD(day,1,CONVERT(date, TarikhAkhir, 103))"

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
            'ddlKodKursus.Items.Add(New ListItem("-Pilih-", "0"))

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
            'ddlKelas.Items.Add(New ListItem("-Pilih-", "0"))

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
        Dim strOrder As String = " ORDER BY kpmkv_pelajar.Nama ASC"

        '--not deleted
        tmpSQL = "  SELECT DISTINCT
                    kpmkv_pelajar_ulang.PelajarID, kpmkv_pelajar_ulang.Tahun, kpmkv_pelajar_ulang.Semester, kpmkv_pelajar_ulang.Sesi, kpmkv_pelajar.Nama, kpmkv_pelajar.MYKAD, kpmkv_pelajar.AngkaGiliran,
                    kpmkv_kelas.NamaKelas,
                    kpmkv_kursus.KodKursus
                    FROM
                    kpmkv_pelajar_ulang
                    LEFT JOIN kpmkv_pelajar ON kpmkv_pelajar.PelajarID = kpmkv_pelajar_ulang.PelajarID
                    LEFT JOIN kpmkv_kelas ON kpmkv_kelas.KelasID = kpmkv_pelajar_ulang.KelasID
                    LEFT JOIN kpmkv_kursus ON kpmkv_kursus.KursusID = kpmkv_pelajar_ulang.KursusID
                    WHERE 
                    kpmkv_pelajar_ulang.KolejRecordID = '" & lblKolejID.Text & "'"

        '--tahun
        If Not ddlTahun.Text = "" Then
            strWhere += " AND kpmkv_pelajar_ulang.Tahun ='" & ddlTahun.Text & "'"
        End If
        '--semester
        If Not ddlSemester.Text = "" Then
            strWhere += " AND kpmkv_pelajar_ulang.Semester ='" & ddlSemester.Text & "'"
        End If
        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere += " AND kpmkv_pelajar_ulang.Sesi ='" & chkSesi.Text & "'"
        End If
        '--kodkursus
        If Not ddlKodKursus.Text = "" Then
            strWhere += " AND kpmkv_pelajar_ulang.KursusID ='" & ddlKodKursus.SelectedValue & "'"
        End If

        If Not ddlNamaKelas.Text = "" Then
            strWhere += " AND kpmkv_pelajar_ulang.KelasID ='" & ddlNamaKelas.SelectedValue & "'"
        End If

        '--txtNama
        If Not txtNama.Text.Length = 0 Then
            strWhere += " AND kpmkv_pelajar.Nama LIKE '%" & oCommon.FixSingleQuotes(txtNama.Text) & "%'"
        End If

        '--txtMYKAD
        If Not txtMYKAD.Text.Length = 0 Then
            strWhere += " AND kpmkv_pelajar.MYKAD='" & oCommon.FixSingleQuotes(txtMYKAD.Text) & "'"
        End If

        getSQL = tmpSQL & strWhere & strOrder


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
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=slipPelajarUlang.pdf")
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

                    myDocument.NewPage()

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

                    Dim table As New PdfPTable(2)
                    table.WidthPercentage = 100
                    table.SetWidths({80, 20})
                    table.DefaultCell.Border = 0

                    Dim cell As New PdfPCell()
                    Dim cetak As String
                    Dim myPara001 As New Paragraph()
                    cell.AddElement(myPara001)
                    cell.Border = 0
                    table.AddCell(cell)

                    cetak = "SALINAN CALON"

                    cell = New PdfPCell()
                    Debug.Write(cetak)
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 7))
                    myPara001.Alignment = Element.ALIGN_RIGHT
                    cell.AddElement(myPara001)
                    cell.Border = 0
                    cell.VerticalAlignment = Element.ALIGN_RIGHT
                    table.AddCell(cell)

                    myDocument.Add(table)

                    cetak = "LEMBAGA PEPERIKSAAN"
                    cetak += Environment.NewLine & "KEMENTERIAN PENDIDIKAN MALAYSIA"
                    cetak += Environment.NewLine & "PERNYATAAN PENDAFTARAN PEPERIKSAAN (ULANGAN)"
                    cetak += Environment.NewLine & "TAHUN PEPERIKSAAN : SEMESTER " & ddlSemester.SelectedValue & ", SESI " & ddlSesi_Semasa.SelectedValue & ", TAHUN " & ddlTahun_Semasa.SelectedValue & Environment.NewLine
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 10))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    myDocument.Add(myPara001)
                    Debug.WriteLine(cetak)

                    ''PROFILE STARTS HERE

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    table = New PdfPTable(2)
                    table.WidthPercentage = 100
                    table.SetWidths({20, 80})

                    cell = New PdfPCell()
                    cetak = Environment.NewLine & "Nama Sekolah / Pusat"
                    cetak += Environment.NewLine & "Angka Giliran"
                    cetak += Environment.NewLine & "Nama Calon"
                    cetak += Environment.NewLine & "No. Pengenalan Diri"
                    cetak += Environment.NewLine & "Nama Bidang"
                    cetak += Environment.NewLine & "Program"
                    cetak += Environment.NewLine
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
                    cell.Border = 0
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = Environment.NewLine & ": " & strKolejnama
                    cetak += Environment.NewLine & ": " & strag
                    cetak += Environment.NewLine & ": " & strname
                    cetak += Environment.NewLine & ": " & strmykad
                    cetak += Environment.NewLine & ": " & strbidang
                    cetak += Environment.NewLine & ": " & strprogram
                    cetak += Environment.NewLine
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
                    cell.Border = 0
                    table.AddCell(cell)
                    Debug.WriteLine(cetak)

                    myDocument.Add(table)

                    ''profile ends here
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    ''mata pelajaran yang didaftarkan
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    table = New PdfPTable(5)
                    table.WidthPercentage = 100
                    table.SetWidths({96, 1, 1, 1, 1})
                    table.DefaultCell.Border = 0

                    cell = New PdfPCell()
                    cetak = "Mata Pelajaran Yang Didaftarkan"
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10, Font.UNDERLINE)))
                    cell.Border = 0
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 9)))
                    cell.Border = 0
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 9)))
                    cell.Border = 0
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 9)))
                    cell.Border = 0
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 9)))
                    cell.Border = 0
                    table.AddCell(cell)

                    myDocument.Add(table)

                    ''table matapelajaran

                    table = New PdfPTable(5)
                    table.WidthPercentage = 100
                    table.SetWidths({10, 35, 1, 10, 45})
                    table.DefaultCell.Border = 0

                    cell = New PdfPCell()
                    cetak = ""
                    Dim cetakNum As String = ""
                    Dim countsubj As Integer = 0
                    Dim strJenisKursusMT As String = ""

                    strSQL = "  SELECT kpmkv_pelajar_ulang.NamaMataPelajaran FROM kpmkv_pelajar_ulang WHERE kpmkv_pelajar_ulang.PelajarID = '" & strkey & "' AND kpmkv_pelajar_ulang.Tahun = '" & ddlTahun.SelectedValue & "' AND kpmkv_pelajar_ulang.Semester = '" & ddlSemester.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)

                    Dim sqlDA As New SqlDataAdapter(strSQL, objConn)
                    Dim ds As DataSet = New DataSet
                    sqlDA.Fill(ds, "AnyTable")

                    strSQL = "SELECT JenisKursus FROM kpmkv_kursus WHERE KursusID = '" & ddlKodKursus.SelectedValue & "'"
                    Dim strJenisKursus As String = oCommon.getFieldValue(strSQL)


                    If strJenisKursus = "TECHNOLOGY" Or strJenisKursus = "TEKNOLOGI" Then
                        strJenisKursus = "TEKNOLOGI"
                    End If

                    If ds.Tables(0).Rows.Count <= 4 Then

                        For iloop As Integer = 0 To ds.Tables(0).Rows.Count - 1

                            If ddlSemester.Text = 3 Or ddlSemester.Text = 4 Then

                                If ds.Tables(0).Rows(iloop).Item(0).ToString = "MATEMATIK" Then

                                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                    strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%MATEMATIK%' AND (Jenis = '" & strJenisKursus & "' OR Jenis IS NULL OR Jenis = '') AND Tahun='" & ddlTahun.SelectedValue & "'"
                                    strRet = oCommon.getFieldValueEx(strSQL)

                                    Dim ar_KODMT As Array
                                    ar_KODMT = strRet.Split("|")
                                    Dim strKodMT As String = ar_KODMT(0)
                                    Dim strNamaMT As String = ar_KODMT(1)

                                    countsubj += 1

                                    cetakNum += countsubj & ". " & strKodMT & " - " & Environment.NewLine

                                    cetak += strNamaMT & Environment.NewLine

                                ElseIf ds.Tables(0).Rows(iloop).Item(0).ToString = "SAINS" Then

                                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                    strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%SAINS%' AND (Jenis = '" & strJenisKursus & "' OR Jenis IS NULL OR Jenis = '') AND Tahun='" & ddlTahun.SelectedValue & "'"
                                    strRet = oCommon.getFieldValueEx(strSQL)

                                    Dim ar_KodNamaMP As Array
                                    ar_KodNamaMP = strRet.Split("|")
                                    Dim strKodMP As String = ar_KodNamaMP(0)
                                    Dim strNamaMP As String = ar_KodNamaMP(1)

                                    countsubj += 1

                                    cetakNum += countsubj & ". " & strKodMP & " - " & Environment.NewLine

                                    cetak += strNamaMP & Environment.NewLine

                                Else

                                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                    strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%" & ds.Tables(0).Rows(iloop).Item(0).ToString & "%' AND Tahun='" & ddlTahun.SelectedValue & "'"
                                    strRet = oCommon.getFieldValueEx(strSQL)

                                    Dim ar_KodNamaMP As Array
                                    ar_KodNamaMP = strRet.Split("|")
                                    Dim strKodMP As String = ar_KodNamaMP(0)
                                    Dim strNamaMP As String = ar_KodNamaMP(1)

                                    countsubj += 1

                                    cetakNum += countsubj & ". " & strKodMP & " - " & Environment.NewLine

                                    cetak += strNamaMP & Environment.NewLine

                                End If

                            Else

                                strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%" & ds.Tables(0).Rows(iloop).Item(0).ToString & "%' AND Tahun='" & ddlTahun.SelectedValue & "'"
                                strRet = oCommon.getFieldValueEx(strSQL)

                                Dim ar_KodNamaMP As Array
                                ar_KodNamaMP = strRet.Split("|")
                                Dim strKodMP As String = ar_KodNamaMP(0)
                                Dim strNamaMP As String = ar_KodNamaMP(1)

                                countsubj += 1

                                cetakNum += countsubj & ". " & strKodMP & " - " & Environment.NewLine

                                cetak += strNamaMP & Environment.NewLine

                            End If

                        Next

                        cell = New PdfPCell()
                        Debug.WriteLine(cetakNum)
                        myPara001 = New Paragraph(cetakNum, FontFactory.GetFont("Arial", 8))
                        myPara001.Alignment = Element.ALIGN_LEFT
                        cell.AddElement(myPara001)
                        cell.Border = 0
                        cell.VerticalAlignment = Element.ALIGN_LEFT
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        Debug.WriteLine(cetak)
                        myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                        myPara001.Alignment = Element.ALIGN_LEFT
                        cell.AddElement(myPara001)
                        cell.Border = 0
                        cell.VerticalAlignment = Element.ALIGN_LEFT
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = ""
                        cell.AddElement(New Paragraph(cetak))
                        cell.Border = 0
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = ""
                        cell.AddElement(New Paragraph(cetak))
                        cell.Border = 0
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = ""
                        cell.AddElement(New Paragraph(cetak))
                        cell.Border = 0
                        table.AddCell(cell)

                        myDocument.Add(table)



                        table = New PdfPTable(3)
                        table.WidthPercentage = 100
                        table.SetWidths({35, 30, 35})
                        table.DefaultCell.Border = 0

                        cell = New PdfPCell()

                        If countsubj = 1 Then

                            cetak = Environment.NewLine
                            cetak += Environment.NewLine
                            cetak += Environment.NewLine
                            cetak += Environment.NewLine

                        ElseIf countsubj = 2 Then

                            cetak = Environment.NewLine
                            cetak += Environment.NewLine
                            cetak += Environment.NewLine

                        ElseIf countsubj = 3 Then

                            cetak = Environment.NewLine
                            cetak += Environment.NewLine

                        Else

                            cetak = Environment.NewLine

                        End If


                        cetak += "Jumlah Mata Pelajaran : " & countsubj
                        myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 9))
                        myPara001.Alignment = Element.ALIGN_LEFT
                        cell.AddElement(myPara001)
                        cell.Border = 0
                        cell.VerticalAlignment = Element.ALIGN_BOTTOM
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = ""
                        cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 9)))
                        cell.Border = 0
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = Environment.NewLine
                        myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 9))
                        myPara001.Alignment = Element.ALIGN_CENTER
                        cell.AddElement(myPara001)
                        cell.Border = 0
                        cell.VerticalAlignment = Element.ALIGN_BOTTOM
                        table.AddCell(cell)

                        myDocument.Add(table)

                    Else

                        For iloop As Integer = 0 To 3

                            If ddlSemester.Text = 3 Or ddlSemester.Text = 4 Then

                                If ds.Tables(0).Rows(iloop).Item(0).ToString = "MATEMATIK" Then

                                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                    strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%MATEMATIK%' AND (Jenis = '" & strJenisKursus & "' OR Jenis IS NULL OR Jenis = '') AND Tahun='" & ddlTahun.SelectedValue & "'"
                                    strRet = oCommon.getFieldValueEx(strSQL)

                                    Dim ar_KODMT As Array
                                    ar_KODMT = strRet.Split("|")
                                    Dim strKodMT As String = ar_KODMT(0)
                                    Dim strNamaMT As String = ar_KODMT(1)

                                    countsubj += 1

                                    cetakNum += countsubj & ". " & strKodMT & " - " & Environment.NewLine

                                    cetak += strNamaMT & Environment.NewLine

                                ElseIf ds.Tables(0).Rows(iloop).Item(0).ToString = "SAINS" Then

                                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                    strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%SAINS%' AND (Jenis = '" & strJenisKursus & "' OR Jenis IS NULL OR Jenis = '') AND Tahun='" & ddlTahun.SelectedValue & "'"
                                    strRet = oCommon.getFieldValueEx(strSQL)

                                    Dim ar_KodNamaMP As Array
                                    ar_KodNamaMP = strRet.Split("|")
                                    Dim strKodMP As String = ar_KodNamaMP(0)
                                    Dim strNamaMP As String = ar_KodNamaMP(1)

                                    countsubj += 1

                                    cetakNum += countsubj & ". " & strKodMP & " - " & Environment.NewLine

                                    cetak += strNamaMP & Environment.NewLine

                                Else

                                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                    strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%" & ds.Tables(0).Rows(iloop).Item(0).ToString & "%' AND Tahun='" & ddlTahun.SelectedValue & "'"
                                    strRet = oCommon.getFieldValueEx(strSQL)

                                    Dim ar_KodNamaMP As Array
                                    ar_KodNamaMP = strRet.Split("|")
                                    Dim strKodMP As String = ar_KodNamaMP(0)
                                    Dim strNamaMP As String = ar_KodNamaMP(1)

                                    countsubj += 1

                                    cetakNum += countsubj & ". " & strKodMP & " - " & Environment.NewLine

                                    cetak += strNamaMP & Environment.NewLine

                                End If

                            Else

                                strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%" & ds.Tables(0).Rows(iloop).Item(0).ToString & "%' AND Tahun='" & ddlTahun.SelectedValue & "'"
                                strRet = oCommon.getFieldValueEx(strSQL)

                                Dim ar_KodNamaMP As Array
                                ar_KodNamaMP = strRet.Split("|")
                                Dim strKodMP As String = ar_KodNamaMP(0)
                                Dim strNamaMP As String = ar_KodNamaMP(1)

                                countsubj += 1

                                cetakNum += countsubj & ". " & strKodMP & " - " & Environment.NewLine

                                cetak += strNamaMP & Environment.NewLine

                            End If

                        Next

                        cell = New PdfPCell()
                        Debug.WriteLine(cetakNum)
                        myPara001 = New Paragraph(cetakNum, FontFactory.GetFont("Arial", 8))
                        myPara001.Alignment = Element.ALIGN_LEFT
                        cell.AddElement(myPara001)
                        cell.Border = 0
                        cell.VerticalAlignment = Element.ALIGN_LEFT
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        Debug.WriteLine(cetak)
                        myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                        myPara001.Alignment = Element.ALIGN_LEFT
                        cell.AddElement(myPara001)
                        cell.Border = 0
                        cell.VerticalAlignment = Element.ALIGN_LEFT
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = ""
                        cell.AddElement(New Paragraph(cetak))
                        cell.Border = 0
                        table.AddCell(cell)

                        cetak = ""
                        cetakNum = ""

                        For iloop As Integer = 4 To ds.Tables(0).Rows.Count - 1

                            If ddlSemester.Text = 3 Or ddlSemester.Text = 4 Then

                                If ds.Tables(0).Rows(iloop).Item(0).ToString = "MATEMATIK" Then

                                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                    strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%MATEMATIK%' AND (Jenis = '" & strJenisKursus & "' OR Jenis IS NULL OR Jenis = '') AND Tahun='" & ddlTahun.SelectedValue & "'"
                                    strRet = oCommon.getFieldValueEx(strSQL)

                                    Dim ar_KODMT As Array
                                    ar_KODMT = strRet.Split("|")
                                    Dim strKodMT As String = ar_KODMT(0)
                                    Dim strNamaMT As String = ar_KODMT(1)

                                    countsubj += 1

                                    cetakNum += countsubj & ". " & strKodMT & " - " & Environment.NewLine

                                    cetak += strNamaMT & Environment.NewLine

                                ElseIf ds.Tables(0).Rows(iloop).Item(0).ToString = "SAINS" Then

                                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                    strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%SAINS%' AND (Jenis = '" & strJenisKursus & "' OR Jenis IS NULL OR Jenis = '') AND Tahun='" & ddlTahun.SelectedValue & "'"
                                    strRet = oCommon.getFieldValueEx(strSQL)

                                    Dim ar_KodNamaMP As Array
                                    ar_KodNamaMP = strRet.Split("|")
                                    Dim strKodMP As String = ar_KodNamaMP(0)
                                    Dim strNamaMP As String = ar_KodNamaMP(1)

                                    countsubj += 1

                                    cetakNum += countsubj & ". " & strKodMP & " - " & Environment.NewLine

                                    cetak += strNamaMP & Environment.NewLine

                                Else

                                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                    strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%" & ds.Tables(0).Rows(iloop).Item(0).ToString & "%' AND Tahun='" & ddlTahun.SelectedValue & "'"
                                    strRet = oCommon.getFieldValueEx(strSQL)

                                    Dim ar_KodNamaMP As Array
                                    ar_KodNamaMP = strRet.Split("|")
                                    Dim strKodMP As String = ar_KodNamaMP(0)
                                    Dim strNamaMP As String = ar_KodNamaMP(1)

                                    countsubj += 1

                                    cetakNum += countsubj & ". " & strKodMP & " - " & Environment.NewLine

                                    cetak += strNamaMP & Environment.NewLine

                                End If

                            Else

                                strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%" & ds.Tables(0).Rows(iloop).Item(0).ToString & "%' AND Tahun='" & ddlTahun.SelectedValue & "'"
                                strRet = oCommon.getFieldValueEx(strSQL)

                                Dim ar_KodNamaMP As Array
                                ar_KodNamaMP = strRet.Split("|")
                                Dim strKodMP As String = ar_KodNamaMP(0)
                                Dim strNamaMP As String = ar_KodNamaMP(1)

                                countsubj += 1

                                cetakNum += countsubj & ". " & strKodMP & " - " & Environment.NewLine

                                cetak += strNamaMP & Environment.NewLine

                            End If

                        Next

                        cell = New PdfPCell()
                        Debug.WriteLine(cetakNum)
                        myPara001 = New Paragraph(cetakNum, FontFactory.GetFont("Arial", 8))
                        myPara001.Alignment = Element.ALIGN_LEFT
                        cell.AddElement(myPara001)
                        cell.Border = 0
                        cell.VerticalAlignment = Element.ALIGN_LEFT
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        Debug.WriteLine(cetak)
                        myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                        myPara001.Alignment = Element.ALIGN_LEFT
                        cell.AddElement(myPara001)
                        cell.Border = 0
                        cell.VerticalAlignment = Element.ALIGN_LEFT
                        table.AddCell(cell)

                        myDocument.Add(table)

                        table = New PdfPTable(3)
                        table.WidthPercentage = 100
                        table.SetWidths({35, 30, 35})
                        table.DefaultCell.Border = 0

                        cell = New PdfPCell()
                        cetak = Environment.NewLine
                        cetak += "Jumlah Mata Pelajaran : " & countsubj
                        myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 9))
                        myPara001.Alignment = Element.ALIGN_LEFT
                        cell.AddElement(myPara001)
                        cell.Border = 0
                        cell.VerticalAlignment = Element.ALIGN_BOTTOM
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = ""
                        cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 9)))
                        cell.Border = 0
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = Environment.NewLine
                        myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 9))
                        myPara001.Alignment = Element.ALIGN_CENTER
                        cell.AddElement(myPara001)
                        cell.Border = 0
                        cell.VerticalAlignment = Element.ALIGN_BOTTOM
                        table.AddCell(cell)

                        myDocument.Add(table)

                    End If

                    table = New PdfPTable(3)
                    table.WidthPercentage = 100
                    table.SetWidths({35, 30, 35})
                    table.DefaultCell.Border = 0

                    cell = New PdfPCell()
                    cetak = "______________________________"
                    cetak += Environment.NewLine
                    cetak += "Tandatangan Calon"
                    cetak += Environment.NewLine
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 9))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = 0
                    cell.VerticalAlignment = Element.ALIGN_BOTTOM
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = Environment.NewLine & Environment.NewLine
                    cetak += Environment.NewLine & Environment.NewLine
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 9)))
                    cell.Border = 0
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = "______________________________"
                    cetak += Environment.NewLine
                    cetak += "Tandatangan Ibubapa / Penjaga"
                    cetak += Environment.NewLine
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 9))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = 0
                    cell.VerticalAlignment = Element.ALIGN_BOTTOM
                    table.AddCell(cell)

                    myDocument.Add(table)
                    cetak = ""
                    cetak += Environment.NewLine
                    cetak += Environment.NewLine
                    cetak += "_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _"
                    cetak += Environment.NewLine
                    cetak += Environment.NewLine
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 9))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    myDocument.Add(myPara001)
                    Debug.WriteLine(cetak)

                    table = New PdfPTable(2)
                    table.WidthPercentage = 100
                    table.SetWidths({80, 20})
                    table.DefaultCell.Border = 0

                    cell = New PdfPCell()
                    cetak = ""
                    cell.AddElement(New Paragraph(cetak))
                    cell.Border = 0
                    table.AddCell(cell)

                    cetak = "SALINAN SEKOLAH/JPN"

                    cell = New PdfPCell()
                    Debug.Write(cetak)
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 7))
                    myPara001.Alignment = Element.ALIGN_RIGHT
                    cell.AddElement(myPara001)
                    cell.Border = 0
                    cell.VerticalAlignment = Element.ALIGN_RIGHT
                    table.AddCell(cell)

                    myDocument.Add(table)

                    cetak = "LEMBAGA PEPERIKSAAN"
                    cetak += Environment.NewLine & "KEMENTERIAN PENDIDIKAN MALAYSIA"
                    cetak += Environment.NewLine & "PERNYATAAN PENDAFTARAN PEPERIKSAAN (ULANGAN)"
                    cetak += Environment.NewLine & "TAHUN PEPERIKSAAN : SEMESTER " & ddlSemester.SelectedValue & ", SESI " & ddlSesi_Semasa.SelectedValue & ", TAHUN " & ddlTahun_Semasa.SelectedValue & Environment.NewLine
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 10))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    myDocument.Add(myPara001)
                    Debug.WriteLine(cetak)

                    ''PROFILE STARTS HERE

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    table = New PdfPTable(2)
                    table.WidthPercentage = 100
                    table.SetWidths({20, 80})

                    cell = New PdfPCell()
                    cetak = Environment.NewLine & "Nama Sekolah / Pusat"
                    cetak += Environment.NewLine & "Angka Giliran"
                    cetak += Environment.NewLine & "Nama Calon"
                    cetak += Environment.NewLine & "No. Pengenalan Diri"
                    cetak += Environment.NewLine & "Nama Bidang"
                    cetak += Environment.NewLine & "Program"
                    cetak += Environment.NewLine
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
                    cell.Border = 0
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = Environment.NewLine & ": " & strKolejnama
                    cetak += Environment.NewLine & ": " & strag
                    cetak += Environment.NewLine & ": " & strname
                    cetak += Environment.NewLine & ": " & strmykad
                    cetak += Environment.NewLine & ": " & strbidang
                    cetak += Environment.NewLine & ": " & strprogram
                    cetak += Environment.NewLine
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
                    cell.Border = 0
                    table.AddCell(cell)
                    Debug.WriteLine(cetak)

                    myDocument.Add(table)

                    ''profile ends here
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    ''mata pelajaran yang didaftarkan
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    table = New PdfPTable(5)
                    table.WidthPercentage = 100
                    table.SetWidths({96, 1, 1, 1, 1})
                    table.DefaultCell.Border = 0

                    cell = New PdfPCell()
                    cetak = "Mata Pelajaran Yang Didaftarkan"
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10, Font.UNDERLINE)))
                    cell.Border = 0
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 8)))
                    cell.Border = 0
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 8)))
                    cell.Border = 0
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 8)))
                    cell.Border = 0
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 8)))
                    cell.Border = 0
                    table.AddCell(cell)

                    myDocument.Add(table)

                    ''table matapelajaran

                    table = New PdfPTable(5)
                    table.WidthPercentage = 100
                    table.SetWidths({10, 35, 1, 10, 45})
                    table.DefaultCell.Border = 0

                    cell = New PdfPCell()
                    cetak = ""

                    cetakNum = ""
                    countsubj = 0

                    strSQL = "  SELECT 
                                kpmkv_pelajar_ulang.NamaMataPelajaran 		
                                FROM 
                                kpmkv_pelajar_ulang
                                WHERE kpmkv_pelajar_ulang.PelajarID = '" & strkey & "'
                                AND kpmkv_pelajar_ulang.Tahun = '" & ddlTahun.SelectedValue & "'
                                AND kpmkv_pelajar_ulang.Semester = '" & ddlSemester.SelectedValue & "'"

                    strRet = oCommon.ExecuteSQL(strSQL)

                    Dim sqlDA2 As New SqlDataAdapter(strSQL, objConn)
                    Dim ds2 As DataSet = New DataSet
                    sqlDA2.Fill(ds2, "AnyTable")

                    If ds2.Tables(0).Rows.Count <= 4 Then

                        For iloop As Integer = 0 To ds2.Tables(0).Rows.Count - 1
                            If ddlSemester.Text = 3 Or ddlSemester.Text = 4 Then

                                If ds.Tables(0).Rows(iloop).Item(0).ToString = "MATEMATIK" Then

                                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                    strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%MATEMATIK%' AND (Jenis = '" & strJenisKursus & "' OR Jenis IS NULL OR Jenis = '') AND Tahun='" & ddlTahun.SelectedValue & "'"
                                    strRet = oCommon.getFieldValueEx(strSQL)

                                    Dim ar_KODMT As Array
                                    ar_KODMT = strRet.Split("|")
                                    Dim strKodMT As String = ar_KODMT(0)
                                    Dim strNamaMT As String = ar_KODMT(1)

                                    countsubj += 1

                                    cetakNum += countsubj & ". " & strKodMT & " - " & Environment.NewLine

                                    cetak += strNamaMT & Environment.NewLine

                                ElseIf ds.Tables(0).Rows(iloop).Item(0).ToString = "SAINS" Then

                                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                    strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%SAINS%' AND (Jenis = '" & strJenisKursus & "' OR Jenis IS NULL OR Jenis = '') AND Tahun='" & ddlTahun.SelectedValue & "'"
                                    strRet = oCommon.getFieldValueEx(strSQL)

                                    Dim ar_KodNamaMP As Array
                                    ar_KodNamaMP = strRet.Split("|")
                                    Dim strKodMP As String = ar_KodNamaMP(0)
                                    Dim strNamaMP As String = ar_KodNamaMP(1)

                                    countsubj += 1

                                    cetakNum += countsubj & ". " & strKodMP & " - " & Environment.NewLine

                                    cetak += strNamaMP & Environment.NewLine

                                Else

                                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                    strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%" & ds.Tables(0).Rows(iloop).Item(0).ToString & "%' AND Tahun='" & ddlTahun.SelectedValue & "'"
                                    strRet = oCommon.getFieldValueEx(strSQL)

                                    Dim ar_KodNamaMP As Array
                                    ar_KodNamaMP = strRet.Split("|")
                                    Dim strKodMP As String = ar_KodNamaMP(0)
                                    Dim strNamaMP As String = ar_KodNamaMP(1)

                                    countsubj += 1

                                    cetakNum += countsubj & ". " & strKodMP & " - " & Environment.NewLine

                                    cetak += strNamaMP & Environment.NewLine

                                End If

                            Else

                                strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%" & ds.Tables(0).Rows(iloop).Item(0).ToString & "%' AND Tahun='" & ddlTahun.SelectedValue & "'"
                                strRet = oCommon.getFieldValueEx(strSQL)

                                Dim ar_KodNamaMP As Array
                                ar_KodNamaMP = strRet.Split("|")
                                Dim strKodMP As String = ar_KodNamaMP(0)
                                Dim strNamaMP As String = ar_KodNamaMP(1)

                                countsubj += 1

                                cetakNum += countsubj & ". " & strKodMP & " - " & Environment.NewLine

                                cetak += strNamaMP & Environment.NewLine

                            End If

                        Next

                        cell = New PdfPCell()
                        Debug.WriteLine(cetakNum)
                        myPara001 = New Paragraph(cetakNum, FontFactory.GetFont("Arial", 8))
                        myPara001.Alignment = Element.ALIGN_LEFT
                        cell.AddElement(myPara001)
                        cell.Border = 0
                        cell.VerticalAlignment = Element.ALIGN_LEFT
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        Debug.WriteLine(cetak)
                        myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                        myPara001.Alignment = Element.ALIGN_LEFT
                        cell.AddElement(myPara001)
                        cell.Border = 0
                        cell.VerticalAlignment = Element.ALIGN_LEFT
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = ""
                        cell.AddElement(New Paragraph(cetak))
                        cell.Border = 0
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = ""
                        cell.AddElement(New Paragraph(cetak))
                        cell.Border = 0
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = ""
                        cell.AddElement(New Paragraph(cetak))
                        cell.Border = 0
                        table.AddCell(cell)

                        myDocument.Add(table)

                        table = New PdfPTable(3)
                        table.WidthPercentage = 100
                        table.SetWidths({35, 30, 35})
                        table.DefaultCell.Border = 0

                        cell = New PdfPCell()

                        If countsubj = 1 Then

                            cetak = Environment.NewLine
                            cetak += Environment.NewLine
                            cetak += Environment.NewLine
                            cetak += Environment.NewLine

                        ElseIf countsubj = 2 Then

                            cetak = Environment.NewLine
                            cetak += Environment.NewLine
                            cetak += Environment.NewLine

                        ElseIf countsubj = 3 Then

                            cetak = Environment.NewLine
                            cetak += Environment.NewLine

                        Else

                            cetak = Environment.NewLine

                        End If

                        cetak += "Jumlah Mata Pelajaran : " & countsubj
                        myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 9))
                        myPara001.Alignment = Element.ALIGN_LEFT
                        cell.AddElement(myPara001)
                        cell.Border = 0
                        cell.VerticalAlignment = Element.ALIGN_BOTTOM
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = ""
                        cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 9)))
                        cell.Border = 0
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = Environment.NewLine
                        myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 9))
                        myPara001.Alignment = Element.ALIGN_CENTER
                        cell.AddElement(myPara001)
                        cell.Border = 0
                        cell.VerticalAlignment = Element.ALIGN_BOTTOM
                        table.AddCell(cell)

                        myDocument.Add(table)

                    Else

                        For iloop As Integer = 0 To 3

                            If ddlSemester.Text = 3 Or ddlSemester.Text = 4 Then

                                If ds.Tables(0).Rows(iloop).Item(0).ToString = "MATEMATIK" Then

                                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                    strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%MATEMATIK%' AND (Jenis = '" & strJenisKursus & "' OR Jenis IS NULL OR Jenis = '') AND Tahun='" & ddlTahun.SelectedValue & "'"
                                    strRet = oCommon.getFieldValueEx(strSQL)

                                    Dim ar_KODMT As Array
                                    ar_KODMT = strRet.Split("|")
                                    Dim strKodMT As String = ar_KODMT(0)
                                    Dim strNamaMT As String = ar_KODMT(1)

                                    countsubj += 1

                                    cetakNum += countsubj & ". " & strKodMT & " - " & Environment.NewLine

                                    cetak += strNamaMT & Environment.NewLine

                                ElseIf ds.Tables(0).Rows(iloop).Item(0).ToString = "SAINS" Then

                                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                    strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%SAINS%' AND (Jenis = '" & strJenisKursus & "' OR Jenis IS NULL OR Jenis = '') AND Tahun='" & ddlTahun.SelectedValue & "'"
                                    strRet = oCommon.getFieldValueEx(strSQL)

                                    Dim ar_KodNamaMP As Array
                                    ar_KodNamaMP = strRet.Split("|")
                                    Dim strKodMP As String = ar_KodNamaMP(0)
                                    Dim strNamaMP As String = ar_KodNamaMP(1)

                                    countsubj += 1

                                    cetakNum += countsubj & ". " & strKodMP & " - " & Environment.NewLine

                                    cetak += strNamaMP & Environment.NewLine

                                Else

                                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                    strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%" & ds.Tables(0).Rows(iloop).Item(0).ToString & "%' AND Tahun='" & ddlTahun.SelectedValue & "'"
                                    strRet = oCommon.getFieldValueEx(strSQL)

                                    Dim ar_KodNamaMP As Array
                                    ar_KodNamaMP = strRet.Split("|")
                                    Dim strKodMP As String = ar_KodNamaMP(0)
                                    Dim strNamaMP As String = ar_KodNamaMP(1)

                                    countsubj += 1

                                    cetakNum += countsubj & ". " & strKodMP & " - " & Environment.NewLine

                                    cetak += strNamaMP & Environment.NewLine

                                End If

                            Else

                                strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%" & ds.Tables(0).Rows(iloop).Item(0).ToString & "%' AND Tahun='" & ddlTahun.SelectedValue & "'"
                                strRet = oCommon.getFieldValueEx(strSQL)

                                Dim ar_KodNamaMP As Array
                                ar_KodNamaMP = strRet.Split("|")
                                Dim strKodMP As String = ar_KodNamaMP(0)
                                Dim strNamaMP As String = ar_KodNamaMP(1)

                                countsubj += 1

                                cetakNum += countsubj & ". " & strKodMP & " - " & Environment.NewLine

                                cetak += strNamaMP & Environment.NewLine

                            End If

                        Next

                        cell = New PdfPCell()
                        Debug.WriteLine(cetakNum)
                        myPara001 = New Paragraph(cetakNum, FontFactory.GetFont("Arial", 8))
                        myPara001.Alignment = Element.ALIGN_LEFT
                        cell.AddElement(myPara001)
                        cell.Border = 0
                        cell.VerticalAlignment = Element.ALIGN_LEFT
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        Debug.WriteLine(cetak)
                        myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                        myPara001.Alignment = Element.ALIGN_LEFT
                        cell.AddElement(myPara001)
                        cell.Border = 0
                        cell.VerticalAlignment = Element.ALIGN_LEFT
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = ""
                        cell.AddElement(New Paragraph(cetak))
                        cell.Border = 0
                        table.AddCell(cell)

                        cetak = ""
                        cetakNum = ""

                        For iloop As Integer = 4 To ds2.Tables(0).Rows.Count - 1

                            If ddlSemester.Text = 3 Or ddlSemester.Text = 4 Then

                                If ds.Tables(0).Rows(iloop).Item(0).ToString = "MATEMATIK" Then

                                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                    strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%MATEMATIK%' AND (Jenis = '" & strJenisKursus & "' OR Jenis IS NULL OR Jenis = '') AND Tahun='" & ddlTahun.SelectedValue & "'"
                                    strRet = oCommon.getFieldValueEx(strSQL)

                                    Dim ar_KODMT As Array
                                    ar_KODMT = strRet.Split("|")
                                    Dim strKodMT As String = ar_KODMT(0)
                                    Dim strNamaMT As String = ar_KODMT(1)

                                    countsubj += 1

                                    cetakNum += countsubj & ". " & strKodMT & " - " & Environment.NewLine

                                    cetak += strNamaMT & Environment.NewLine

                                ElseIf ds.Tables(0).Rows(iloop).Item(0).ToString = "SAINS" Then

                                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                    strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%SAINS%' AND (Jenis = '" & strJenisKursus & "' OR Jenis IS NULL OR Jenis = '') AND Tahun='" & ddlTahun.SelectedValue & "'"
                                    strRet = oCommon.getFieldValueEx(strSQL)

                                    Dim ar_KodNamaMP As Array
                                    ar_KodNamaMP = strRet.Split("|")
                                    Dim strKodMP As String = ar_KodNamaMP(0)
                                    Dim strNamaMP As String = ar_KodNamaMP(1)

                                    countsubj += 1

                                    cetakNum += countsubj & ". " & strKodMP & " - " & Environment.NewLine

                                    cetak += strNamaMP & Environment.NewLine

                                Else

                                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                    strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%" & ds.Tables(0).Rows(iloop).Item(0).ToString & "%' AND Tahun='" & ddlTahun.SelectedValue & "'"
                                    strRet = oCommon.getFieldValueEx(strSQL)

                                    Dim ar_KodNamaMP As Array
                                    ar_KodNamaMP = strRet.Split("|")
                                    Dim strKodMP As String = ar_KodNamaMP(0)
                                    Dim strNamaMP As String = ar_KodNamaMP(1)

                                    countsubj += 1

                                    cetakNum += countsubj & ". " & strKodMP & " - " & Environment.NewLine

                                    cetak += strNamaMP & Environment.NewLine

                                End If

                            Else

                                strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran FROM kpmkv_matapelajaran"
                                strSQL += " WHERE Semester='" & ddlSemester.Text & "' AND NamaMataPelajaran LIKE '%" & ds.Tables(0).Rows(iloop).Item(0).ToString & "%' AND Tahun='" & ddlTahun.SelectedValue & "'"
                                strRet = oCommon.getFieldValueEx(strSQL)

                                Dim ar_KodNamaMP As Array
                                ar_KodNamaMP = strRet.Split("|")
                                Dim strKodMP As String = ar_KodNamaMP(0)
                                Dim strNamaMP As String = ar_KodNamaMP(1)

                                countsubj += 1

                                cetakNum += countsubj & ". " & strKodMP & " - " & Environment.NewLine

                                cetak += strNamaMP & Environment.NewLine

                            End If

                        Next

                        cell = New PdfPCell()
                        Debug.WriteLine(cetakNum)
                        myPara001 = New Paragraph(cetakNum, FontFactory.GetFont("Arial", 8))
                        myPara001.Alignment = Element.ALIGN_LEFT
                        cell.AddElement(myPara001)
                        cell.Border = 0
                        cell.VerticalAlignment = Element.ALIGN_LEFT
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        Debug.WriteLine(cetak)
                        myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                        myPara001.Alignment = Element.ALIGN_LEFT
                        cell.AddElement(myPara001)
                        cell.Border = 0
                        cell.VerticalAlignment = Element.ALIGN_LEFT
                        table.AddCell(cell)

                        myDocument.Add(table)

                        table = New PdfPTable(3)
                        table.WidthPercentage = 100
                        table.SetWidths({35, 30, 35})
                        table.DefaultCell.Border = 0

                        cell = New PdfPCell()
                        cetak = Environment.NewLine
                        cetak += "Jumlah Mata Pelajaran : " & countsubj
                        myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 9))
                        myPara001.Alignment = Element.ALIGN_LEFT
                        cell.AddElement(myPara001)
                        cell.Border = 0
                        cell.VerticalAlignment = Element.ALIGN_BOTTOM
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = ""
                        cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 9)))
                        cell.Border = 0
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = Environment.NewLine
                        myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 9))
                        myPara001.Alignment = Element.ALIGN_CENTER
                        cell.AddElement(myPara001)
                        cell.Border = 0
                        cell.VerticalAlignment = Element.ALIGN_BOTTOM
                        table.AddCell(cell)

                        myDocument.Add(table)

                    End If

                    table = New PdfPTable(3)
                    table.WidthPercentage = 100
                    table.SetWidths({35, 30, 35})
                    table.DefaultCell.Border = 0

                    cell = New PdfPCell()
                    cetak = "______________________________"
                    cetak += Environment.NewLine
                    cetak += "Tandatangan Calon"
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 9))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = 0
                    cell.VerticalAlignment = Element.ALIGN_BOTTOM
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = Environment.NewLine & Environment.NewLine
                    cetak += Environment.NewLine & Environment.NewLine
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 9)))
                    cell.Border = 0
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = "______________________________"
                    cetak += Environment.NewLine
                    cetak += "Tandatangan Ibubapa / Penjaga"
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 9))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = 0
                    cell.VerticalAlignment = Element.ALIGN_BOTTOM
                    table.AddCell(cell)

                    myDocument.Add(table)

                End If
            Next

            myDocument.Close()

            HttpContext.Current.Response.Write(myDocument)
            HttpContext.Current.Response.End()

        Catch ex As Exception

            lblMsg.Text = "error" & ex.Message

        End Try
    End Sub

    Private Sub ddlTahun_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTahun.SelectedIndexChanged
        kpmkv_semester_list()
    End Sub
End Class