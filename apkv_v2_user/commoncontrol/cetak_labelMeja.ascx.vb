Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.pdf.draw
Imports System.Globalization

Public Class cetak_labelMeja
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""
    Dim IntTakwim As Integer = 0

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)

                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                kpmkv_tahun_list()
                ddlTahun.Text = Now.Year

                kpmkv_kelas_list()

                kpmkv_kodkursus_list()

                kpmkv_semester_list()

                kpmkv_tahun_2_list()
                ddlTahun_Semasa.Text = Now.Year

            End If
        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try
    End Sub


    Private Sub kpmkv_tahun_list()
        strSQL = "SELECT Tahun FROM kpmkv_tahun ORDER BY TahunID"
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

    Private Sub kpmkv_semester_list()
        strSQL = "SELECT Semester FROM kpmkv_semester ORDER BY Semester ASC"
        strRet = oCommon.getFieldValue(strSQL)

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
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=LabelMeja.pdf")
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
                    cetak += Environment.NewLine & "PERNYATAAN PENDAFTARAN PEPERIKSAAN"
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
                    table.SetWidths({10, 25, 1, 10, 55})
                    table.DefaultCell.Border = 0

                    strSQL = "select KursusID from kpmkv_pelajar where PelajarID = '" & strkey & "'"
                    Dim kursusid As String = oCommon.getFieldValue(strSQL)
                    strSQL = "select Semester from kpmkv_pelajar where PelajarID = '" & strkey & "'"
                    Dim semester As String = oCommon.getFieldValue(strSQL)
                    strSQL = "select Tahun from kpmkv_pelajar where PelajarID = '" & strkey & "'"
                    Dim tahun As String = oCommon.getFieldValue(strSQL)

                    cell = New PdfPCell()
                    cetak = ""
                    Dim cetakNum As String = ""
                    Dim countsubj As Integer = 0
                    Dim strJenisKursusMT As String = ""

                    If (ddlSemester.SelectedValue = "3" Or ddlSemester.SelectedValue = "4") Then
                        strSQL = "SELECT  JenisKursus From kpmkv_kursus Where KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strJenisKursusMT = oCommon.getFieldValue(strSQL)
                    End If

                    strSQL = "SELECT KodMataPelajaran, NamaMataPelajaran from kpmkv_matapelajaran where Tahun ='" & tahun & "' and substring(KodMataPelajaran,4,1) = '" & semester & "'"
                    If stragama = "ISLAM" Then
                        strSQL += " AND NamaMataPelajaran NOT like ('PENDIDIKAN MORAL%')"
                    Else
                        strSQL += " AND NamaMataPelajaran NOT like ('PENDIDIKAN ISLAM%')"
                    End If
                    If Not strJenisKursusMT = "" Then
                        If strJenisKursusMT = "SOCIAL" Then
                            strSQL += " AND (Jenis ='SOCIAL' OR Jenis IS NULL OR Jenis ='')"
                        Else
                            strSQL += " AND (Jenis ='TEKNOLOGI' OR Jenis IS NULL OR Jenis ='')"
                        End If
                    End If
                    strSQL += " ORDER BY KodMataPelajaran"
                    strRet = oCommon.ExecuteSQL(strSQL)

                    Dim sqlDA As New SqlDataAdapter(strSQL, objConn)
                    Dim ds As DataSet = New DataSet
                    sqlDA.Fill(ds, "AnyTable")

                    For iloop As Integer = 0 To 3
                        Dim subjcode As String = (ds.Tables(0).Rows(iloop).Item(0).ToString())
                        subjcode = subjcode.Replace(vbCr, "").Replace(vbLf, "")

                        countsubj += 1

                        cetakNum += countsubj & ". " & ds.Tables(0).Rows(iloop).Item(0).ToString & " - " & Environment.NewLine

                        cetak += ds.Tables(0).Rows(iloop).Item(1).ToString & Environment.NewLine

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

                        cetakNum += countsubj & ". " & ds.Tables(0).Rows(iloop2).Item(0).ToString & " -" & Environment.NewLine

                        cetak += ds.Tables(0).Rows(iloop2).Item(1).ToString & Environment.NewLine

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
                    cetak += Environment.NewLine & "PERNYATAAN PENDAFTARAN PEPERIKSAAN"
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
                    table.SetWidths({10, 25, 1, 10, 55})
                    table.DefaultCell.Border = 0

                    strSQL = "select KursusID from kpmkv_pelajar where PelajarID = '" & strkey & "'"
                    kursusid = oCommon.getFieldValue(strSQL)
                    strSQL = "select Semester from kpmkv_pelajar where PelajarID = '" & strkey & "'"
                    semester = oCommon.getFieldValue(strSQL)
                    strSQL = "select Tahun from kpmkv_pelajar where PelajarID = '" & strkey & "'"
                    tahun = oCommon.getFieldValue(strSQL)

                    cell = New PdfPCell()
                    cetak = ""

                    cetakNum = ""
                    countsubj = 0
                    strJenisKursusMT = ""

                    If (ddlSemester.SelectedValue = "3" Or ddlSemester.SelectedValue = "4") Then
                        strSQL = "SELECT  JenisKursus From kpmkv_kursus Where KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strJenisKursusMT = oCommon.getFieldValue(strSQL)
                    End If

                    strSQL = "SELECT KodMataPelajaran, NamaMataPelajaran from kpmkv_matapelajaran where Tahun ='" & tahun & "' and substring(KodMataPelajaran,4,1) = '" & semester & "'"
                    If stragama = "ISLAM" Then
                        strSQL += " AND NamaMataPelajaran NOT like ('PENDIDIKAN MORAL%')"
                    Else
                        strSQL += " AND NamaMataPelajaran NOT like ('PENDIDIKAN ISLAM%')"
                    End If
                    If Not strJenisKursusMT = "" Then
                        If strJenisKursusMT = "SOCIAL" Then
                            strSQL += " AND (Jenis ='SOCIAL' OR Jenis IS NULL OR Jenis ='')"
                        Else
                            strSQL += " AND (Jenis ='TEKNOLOGI' OR Jenis IS NULL OR Jenis ='')"
                        End If
                    End If
                    strSQL += " ORDER BY KodMataPelajaran"
                    strRet = oCommon.ExecuteSQL(strSQL)

                    Dim sqlDA2 As New SqlDataAdapter(strSQL, objConn)
                    Dim ds2 As DataSet = New DataSet
                    sqlDA2.Fill(ds2, "AnyTable")

                    For iloop As Integer = 0 To 3

                        Dim subjcode As String = (ds2.Tables(0).Rows(iloop).Item(0).ToString())
                        subjcode = subjcode.Replace(vbCr, "").Replace(vbLf, "")

                        countsubj += 1

                        cetakNum += countsubj & ". " & ds2.Tables(0).Rows(iloop).Item(0).ToString & " - " & Environment.NewLine

                        cetak += ds2.Tables(0).Rows(iloop).Item(1).ToString & Environment.NewLine

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

                        Dim subjcode As String = (ds2.Tables(0).Rows(iloop).Item(0).ToString())
                        subjcode = subjcode.Replace(vbCr, "").Replace(vbLf, "")

                        countsubj += 1

                        cetakNum += countsubj & ". " & ds2.Tables(0).Rows(iloop).Item(0).ToString & " -" & Environment.NewLine

                        cetak += ds2.Tables(0).Rows(iloop).Item(1).ToString & Environment.NewLine

                    Next

                    strSQL = "SELECT KodMPVOK,NamaMPVOK FROM kpmkv_matapelajaran_v WHERE Tahun = '" & tahun & "'"
                    strSQL += " AND KursusID='" & kursusid & "' AND Semester='" & semester & "'"

                    sqlDA2 = New SqlDataAdapter(strSQL, objConn)
                    ds2 = New DataSet
                    sqlDA2.Fill(ds2, "AnyTable")

                    For iloop2 As Integer = 0 To ds2.Tables(0).Rows.Count - 1

                        countsubj += 1

                        cetakNum += countsubj & ". " & ds2.Tables(0).Rows(iloop2).Item(0).ToString & " -" & Environment.NewLine

                        cetak += ds2.Tables(0).Rows(iloop2).Item(1).ToString & Environment.NewLine

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
                    cetak += Environment.NewLine
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
                    cetak += Environment.NewLine
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 9))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = 0
                    cell.VerticalAlignment = Element.ALIGN_BOTTOM
                    table.AddCell(cell)

                    myDocument.Add(table)

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

        End Try
    End Sub
End Class
