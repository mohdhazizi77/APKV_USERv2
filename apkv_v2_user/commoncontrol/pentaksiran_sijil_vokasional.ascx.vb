Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Imports RKLib.ExportData

Imports iTextSharp.text
Imports iTextSharp.text.pdf
Public Class pentaksiran_sijil_vokasional
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

                kpmkv_kelas_list()

                kpmkv_kodkursus_list()

                ' strRet = BindData(datRespondent)
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

        Catch ex As Exception

        Finally
            objConn.Dispose()
        End Try

    End Sub
    
    Private Sub kpmkv_kodkursus_list()

        strSQL = "SELECT kpmkv_kursus.KodKursus, kpmkv_kursus.KursusID FROM kpmkv_kursus_kolej LEFT OUTER JOIN"
        strSQL += " kpmkv_kursus ON kpmkv_kursus_kolej.KursusID = kpmkv_kursus.KursusID"
        strSQL += " WHERE kpmkv_kursus_kolej.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kursus.Tahun='" & ddlTahun.SelectedValue & "' "
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
        Dim strOrder As String = " ORDER BY kpmkv_pelajar.Nama ASC"

        '--not deleted
        tmpSQL = "SELECT kpmkv_pelajar.PelajarID, kpmkv_pelajar.Tahun, kpmkv_pelajar.Sesi, kpmkv_pelajar.Nama, kpmkv_pelajar.MYKAD, kpmkv_pelajar.AngkaGiliran, "
        tmpSQL += " kpmkv_kluster.NamaKluster, kpmkv_kursus.NamaKursus, kpmkv_pelajar.Kaum, kpmkv_pelajar.Jantina, kpmkv_pelajar.Agama, kpmkv_status.Status, kpmkv_kelas.NamaKelas"
        tmpSQL += " FROM  kpmkv_pelajar LEFT OUTER JOIN kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN kpmkv_kluster ON kpmkv_kursus.KlusterID=kpmkv_kluster.KlusterID"
        tmpSQL += " LEFT OUTER JOIN kpmkv_status ON kpmkv_pelajar.StatusID = kpmkv_status.StatusID LEFT OUTER JOIN kpmkv_kelas ON kpmkv_pelajar.KelasID = kpmkv_kelas.KelasID"
        strWhere = " WHERE kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.StatusID='2' AND kpmkv_pelajar.KolejRecordID='" & lblKolejID.Text & "'"

        '--tahun
        If Not ddlTahun.Text = "" Then
            strWhere += " AND kpmkv_pelajar.Tahun ='" & ddlTahun.Text & "'"
        End If
        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere += " AND kpmkv_pelajar.Sesi ='" & chkSesi.Text & "'"
        End If
        '--kodkursus
        If Not ddlKodKursus.Text = "" Then
            strWhere += " AND kpmkv_pelajar.KursusID ='" & ddlKodKursus.SelectedValue & "'"
        End If
        '--NamaKelas
        If Not ddlNamaKelas.Text = "" Then
            strWhere += " AND kpmkv_pelajar.KelasID ='" & ddlNamaKelas.SelectedValue & "'"
        End If
        '--txtNama
        If Not txtNama.Text.Length = 0 Then
            strWhere += " AND kpmkv_pelajar.Nama LIKE '%" & oCommon.FixSingleQuotes(txtNama.Text) & "%'"
        End If

        '--txtMYKAD
        If Not txtMYKAD.Text.Length = 0 Then
            strWhere += " AND kpmkv_pelajar.MYKAD='" & oCommon.FixSingleQuotes(txtMYKAD.Text) & "'"
        End If

        '--txtAngkaGiliran
        If Not txtAngkaGiliran.Text.Length = 0 Then
            strWhere += " AND kpmkv_pelajar.AngkaGiliran LIKE '%" & oCommon.FixSingleQuotes(txtAngkaGiliran.Text) & "%'"
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

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click
        Try

            Dim tableColumn As DataColumnCollection
            Dim tableRows As DataRowCollection

            Dim myDataSet As New DataSet
            Dim myDataAdapter As New SqlDataAdapter(getSQL, strConn)
            myDataAdapter.Fill(myDataSet, "Slip Sijil Vokasional")
            myDataAdapter.SelectCommand.CommandTimeout = 80000
            objConn.Close()

            '--transfer to an object
            tableColumn = myDataSet.Tables(0).Columns
            tableRows = myDataSet.Tables(0).Rows

            CreatePDF(tableColumn, tableRows)

        Catch ex As Exception
            '--display on screen
            lblMsg.Text = "System Error." & ex.Message

            ''--write to file
            'Dim strMsg As String = Now.ToString & ":" & Request.UserHostAddress & ":" & ex.Message
            'Dim strPath As String = Server.MapPath(".") & "\log\Error" & oCommon.getToday & ".log"
            'oCommon.WriteLogFile(strPath, strMsg)

        End Try
    End Sub
    Private Sub CreatePDF(ByVal tableColumns As DataColumnCollection, ByVal tableRows As DataRowCollection)

        'Step 1: First create an instance of document object
        Dim myDocument As New Document(PageSize.A4)

        Try
            HttpContext.Current.Response.ContentType = "application/pdf"
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=SlipPentaksiranVokasional.pdf")
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)

            PdfWriter.GetInstance(myDocument, HttpContext.Current.Response.OutputStream)

            'Step 3: Open the document now using
            myDocument.Open()
            'myDocument.AddTitle("kpmkv")
            'myDocument.AddAuthor("Jamain Johari (ARAKEN SDN BHD)")
            'myDocument.AddSubject("Laporan Akhir PPCS")

            ' ''--drawline
            Dim imgdrawSpacing As String = Server.MapPath("~/img/spacing.png")
            Dim imgSpacing As Image = Image.GetInstance(imgdrawSpacing)
            imgSpacing.Alignment = Image.ALIGN_LEFT  'ALIGN_LEFT
            imgSpacing.Border = 0


            ''--loop thru records here

            Dim strNama As String = ""
            Dim strMykad As String = ""
            Dim strAngkaGiliran As String = ""
            Dim strJantina As String = ""
            Dim strKodKursus As String = ""
            Dim strKluster As String = ""

            ''--start here 
            'markah
            Dim strJum_NilaiMata_Akademik_BM As Double = 0.0
            Dim strJum_JamKredit_Akademik_BM As Double = 0.0
            Dim strJum_NilaiMata_Akademik As Double = 0.0
            Dim strJum_NilaiMata_Vokasional As Double = 0.0
            Dim strJum_JamKredit_Akademik As Double = 0.0
            Dim strJum_JamKredit_Vokasional As Double = 0.0

            Dim strTotalNilaiMataPNGA As Double = 0.0
            Dim strTotalNilaiMataPNGV As Double = 0.0
            Dim strTotalNilaiMataPNGK As Double = 0.0
            Dim strTotalNilaiMataPNGKA As Double = 0.0
            Dim strTotalNilaiMataPNGKBM As Double = 0.0
            Dim strTotalNilaiMataPNGKV As Double = 0.0
            Dim strTotalNilaiMataPNGKK As Double = 0.0

            strSQL = "SELECT NamaKursus FROM kpmkv_kursus WHERE KursusID='" & ddlKodKursus.SelectedValue & "'"
            strKodKursus = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT Top 1 kpmkv_kluster.NamaKluster FROM  kpmkv_kluster LEFT OUTER JOIN kpmkv_kursus "
            strSQL += " ON kpmkv_kluster.KlusterID = kpmkv_kursus.KlusterID "
            strSQL += " WHERE kpmkv_kursus.NamaKursus ='" & strKodKursus & "' "
            strKluster = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
            Dim strKolejnama As String = oCommon.getFieldValue(strSQL)

            'kolejnegeri
            strSQL = "SELECT Negeri FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
            Dim strKolejnegeri As String = oCommon.getFieldValue(strSQL)
            Dim strKey As String

            'count data
            'Get the data from database into datatable 
            ' Dim cmd As New SqlCommand(getSQL)
            ' Dim dt As DataTable = GetData(cmd)


            'append new line 
            For i As Integer = 0 To datRespondent.Rows.Count - 1
                Dim row As GridViewRow = datRespondent.Rows(0)
                Dim cb As CheckBox = datRespondent.Rows(i).FindControl("chkSelect")

                If cb.Checked = True Then
                    strKey = datRespondent.DataKeys(i).Value.ToString
                    ''--Tokenid,ClassCode,Q001Remarks
                    strSQL = "SELECT Nama FROM kpmkv_pelajar WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strNama = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT Mykad FROM kpmkv_pelajar WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strMykad = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT AngkaGiliran FROM kpmkv_pelajar WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strAngkaGiliran = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT Jantina FROM kpmkv_pelajar WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strJantina = oCommon.getFieldValue(strSQL)

                    ''--page header
                    Dim imageHeader As String = Server.MapPath("~/img/logo kpm baru.png")
                    Dim imgHeader As Image = Image.GetInstance(imageHeader)
                    imgHeader.Alignment = Image.ALIGN_LEFT
                    myDocument.Add(imgHeader)

                    Dim myPara03 As New Paragraph("Transkrip Sijil Vokasional Malaysia", FontFactory.GetFont("Arial", 10, Font.BOLD))
                    myPara03.Alignment = Element.ALIGN_CENTER
                    myDocument.Add(myPara03)

                    myDocument.Add(imgSpacing)

                    myDocument.Add(New Paragraph("NAMA                              :" & strNama, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(New Paragraph("NO.KAD PENGENALAN :" & strMykad, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(New Paragraph("ANGKA GILIRAN             :" & strAngkaGiliran, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(New Paragraph("INSTITUSI                       :" & strKolejnama & "  " & ",  " & strKolejnegeri, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(New Paragraph("BIDANG                         :" & strKluster, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(New Paragraph("PROGRAM:                    :" & strKodKursus, FontFactory.GetFont("Arial", 8, Font.NORMAL)))

                    myDocument.Add(imgSpacing)

                    Dim myTableTitle As New PdfPTable(10)
                    myTableTitle.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTableTitle.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    'myTable.HorizontalAlignment = Rectangle.NO_BORDER
                    Dim intTblWidthTitle() As Integer = {36, 12, 12, 12, 10, 10, 36, 12, 12, 12}
                    myTableTitle.SetWidths(intTblWidthTitle)

                    Dim Cell1TTL As New PdfPCell(New Phrase("SEMESTER ", FontFactory.GetFont("Arial", 7, Font.NORMAL)))
                    Cell1TTL.Border = Rectangle.NO_BORDER
                    myTableTitle.AddCell(Cell1TTL)

                    Dim Cell2TTL As New PdfPCell(New Phrase("1", FontFactory.GetFont("Arial", 7, Font.NORMAL)))
                    Cell2TTL.Border = Rectangle.NO_BORDER
                    myTableTitle.AddCell(Cell2TTL)

                    Dim Cell3TTL As New PdfPCell(New Phrase("TAHUN", FontFactory.GetFont("Arial", 7, Font.NORMAL)))
                    Cell3TTL.Border = Rectangle.NO_BORDER
                    myTableTitle.AddCell(Cell3TTL)

                    Dim Cell4TTL As New PdfPCell(New Phrase("" & ddlTahun.Text, FontFactory.GetFont("Arial", 7, Font.NORMAL)))
                    Cell4TTL.Border = Rectangle.NO_BORDER
                    myTableTitle.AddCell(Cell4TTL)

                    Dim Cell5TTL As New PdfPCell(New Phrase("", FontFactory.GetFont("Arial", 7, Font.NORMAL)))
                    Cell5TTL.Border = Rectangle.NO_BORDER
                    myTableTitle.AddCell(Cell5TTL)

                    Dim Cell6TTL As New PdfPCell(New Phrase("", FontFactory.GetFont("Arial", 7, Font.NORMAL)))
                    Cell6TTL.Border = Rectangle.NO_BORDER
                    myTableTitle.AddCell(Cell6TTL)

                    Dim Cell7TTL As New PdfPCell(New Phrase("SEMESTER", FontFactory.GetFont("Arial", 7, Font.NORMAL)))
                    Cell7TTL.Border = Rectangle.NO_BORDER
                    myTableTitle.AddCell(Cell7TTL)

                    Dim Cell8TTL As New PdfPCell(New Phrase("2", FontFactory.GetFont("Arial", 7, Font.NORMAL)))
                    Cell8TTL.Border = Rectangle.NO_BORDER
                    myTableTitle.AddCell(Cell8TTL)

                    Dim Cell9TTL As New PdfPCell(New Phrase("TAHUN", FontFactory.GetFont("Arial", 7, Font.NORMAL)))
                    Cell9TTL.Border = Rectangle.NO_BORDER
                    myTableTitle.AddCell(Cell9TTL)

                    Dim Cell10TTL As New PdfPCell(New Phrase("" & ddlTahun.Text, FontFactory.GetFont("Arial", 7, Font.NORMAL)))
                    Cell10TTL.Border = Rectangle.NO_BORDER
                    myTableTitle.AddCell(Cell10TTL)
                    myDocument.Add(myTableTitle)

                    Dim myTable As New PdfPTable(10)
                    myTable.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTable.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    'myTable.HorizontalAlignment = Rectangle.NO_BORDER
                    Dim intTblWidth() As Integer = {15, 40, 10, 10, 10, 10, 15, 40, 10, 10}
                    myTable.SetWidths(intTblWidth)

                    Dim Cell1Hdr As New PdfPCell(New Phrase("KOD ", FontFactory.GetFont("Arial", 7, Font.NORMAL)))
                    Cell1Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell1Hdr)

                    Dim Cell2Hdr As New PdfPCell(New Phrase("MATAPELAJARAN", FontFactory.GetFont("Arial", 7, Font.NORMAL)))
                    Cell2Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell2Hdr)

                    Dim Cell3Hdr As New PdfPCell(New Phrase("JAM KREDIT", FontFactory.GetFont("Arial", 7, Font.NORMAL)))
                    Cell3Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell3Hdr)

                    Dim Cell4Hdr As New PdfPCell(New Phrase("GRED", FontFactory.GetFont("Arial", 7, Font.NORMAL)))
                    Cell4Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell4Hdr)

                    Dim Cell5Hdr As New PdfPCell(New Phrase("", FontFactory.GetFont("Arial", 7, Font.NORMAL)))
                    Cell5Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell5Hdr)

                    Dim Cell5_1Hdr As New PdfPCell(New Phrase("", FontFactory.GetFont("Arial", 7, Font.NORMAL)))
                    Cell5_1Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell5_1Hdr)

                    Dim Cell6Hdr As New PdfPCell(New Phrase("KOD", FontFactory.GetFont("Arial", 7, Font.NORMAL)))
                    Cell6Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell6Hdr)

                    Dim Cell7Hdr As New PdfPCell(New Phrase("MATAPELAJARAN", FontFactory.GetFont("Arial", 7, Font.NORMAL)))
                    Cell7Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell7Hdr)

                    Dim Cell8Hdr As New PdfPCell(New Phrase("JAM KREDIT", FontFactory.GetFont("Arial", 7, Font.NORMAL)))
                    Cell8Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell8Hdr)

                    Dim Cell9Hdr As New PdfPCell(New Phrase("GRED", FontFactory.GetFont("Arial", 7, Font.NORMAL)))
                    Cell9Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell9Hdr)

                    myDocument.Add(myTable)
                    'S1
                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='1' AND NamaMataPelajaran='BAHASA MELAYU' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPBM As Array
                    ar_MPBM = strRet.Split("|")
                    Dim strKodBM As String = ar_MPBM(0)
                    Dim strNamaBM As String = ar_MPBM(1)
                    Dim strJamKreditBM As String = ar_MPBM(2)

                    'S1
                    strSQL = "SELECT  GredBM FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='1'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    Dim strGredBM As String = oCommon.getFieldValue(strSQL)

                    'S2
                    Dim strGredBM2 As String = ""
                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='2' AND NamaMataPelajaran='BAHASA MELAYU' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPBM2 As Array
                    ar_MPBM2 = strRet.Split("|")
                    Dim strKodBM2 As String = ar_MPBM2(0)
                    Dim strNamaBM2 As String = ar_MPBM2(1)
                    Dim strJamKreditBM2 As String = ar_MPBM2(2)

                    'S2
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                    strSQL += " AND Semester='2'"
                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                    If oCommon.isExist(strSQL) = True Then
                        strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='2'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_HPelajar As Array
                        ar_HPelajar = strRet.Split("|")
                        Dim strP_ID As String = ar_HPelajar(0)
                        Dim strP_Tahun As String = ar_HPelajar(1)
                        Dim strP_Sesi As String = ar_HPelajar(2)

                        strSQL = "SELECT  GredBM FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "'"
                        strSQL += " AND Tahun='" & strP_Tahun & "'"
                        strSQL += " AND Semester='2'"
                        strSQL += " AND Sesi='" & strP_Sesi & "'"
                        strGredBM2 = oCommon.getFieldValue(strSQL)
                    End If

                    Dim myTableMP8 As New PdfPTable(10)
                    myTableMP8.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTableMP8.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    myTableMP8.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                    Dim intTblCell8() As Integer = {15, 40, 10, 10, 10, 10, 15, 40, 10, 10}
                    myTableMP8.SetWidths(intTblCell8)

                    myTableMP8.AddCell(New Phrase(strKodBM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP8.AddCell(New Phrase(strNamaBM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP8.AddCell(New Phrase(strJamKreditBM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP8.AddCell(New Phrase(strGredBM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP8.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP8.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP8.AddCell(New Phrase(strKodBM2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP8.AddCell(New Phrase(strNamaBM2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP8.AddCell(New Phrase(strJamKreditBM2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP8.AddCell(New Phrase(strGredBM2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(myTableMP8)

                    'BI S1
                    strSQL = "SELECT KODMatapelajaran,NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='1' AND NamaMataPelajaran='BAHASA INGGERIS' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPBI As Array
                    ar_MPBI = strRet.Split("|")
                    Dim strKodBI As String = ar_MPBI(0)
                    Dim strNamaBI As String = ar_MPBI(1)
                    Dim strJamKreditBI As String = ar_MPBI(2)

                    'BI S1
                    strSQL = "SELECT  GredBI FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='1'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    Dim strGredBI As String = oCommon.getFieldValue(strSQL)

                    'BI S2
                    strSQL = "SELECT KODMatapelajaran,NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='2' AND NamaMataPelajaran='BAHASA INGGERIS' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPBI2 As Array
                    ar_MPBI2 = strRet.Split("|")
                    Dim strKodBI2 As String = ar_MPBI2(0)
                    Dim strNamaBI2 As String = ar_MPBI2(1)
                    Dim strJamKreditBI2 As String = ar_MPBI2(2)

                    'BI S2
                    Dim strGredBI2 As String = ""
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                    strSQL += " AND Semester='2'"
                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                    If oCommon.isExist(strSQL) = True Then
                        strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='2'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_HPelajar As Array
                        ar_HPelajar = strRet.Split("|")
                        Dim strP_ID As String = ar_HPelajar(0)
                        Dim strP_Tahun As String = ar_HPelajar(1)
                        Dim strP_Sesi As String = ar_HPelajar(2)

                        strSQL = "SELECT  GredBI FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "'"
                        strSQL += " AND Tahun='" & strP_Tahun & "'"
                        strSQL += " AND Semester='2'"
                        strSQL += " AND Sesi='" & strP_Sesi & "'"
                        strGredBI2 = oCommon.getFieldValue(strSQL)
                    End If

                    Dim myTableBI As New PdfPTable(10)
                    myTableBI.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTableBI.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    myTableBI.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                    Dim intTblCellBI() As Integer = {15, 40, 10, 10, 10, 10, 15, 40, 10, 10}
                    myTableBI.SetWidths(intTblCellBI)

                    myTableBI.AddCell(New Phrase(strKodBI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBI.AddCell(New Phrase(strNamaBI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBI.AddCell(New Phrase(strJamKreditBI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBI.AddCell(New Phrase(strGredBI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBI.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBI.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBI.AddCell(New Phrase(strKodBI2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBI.AddCell(New Phrase(strNamaBI2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBI.AddCell(New Phrase(strJamKreditBI2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBI.AddCell(New Phrase(strGredBI2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(myTableBI)

                    'MATHEMATIC S1
                    strSQL = "SELECT KODMatapelajaran,NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='1' AND NamaMataPelajaran='MATHEMATIC' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPMT As Array
                    ar_MPMT = strRet.Split("|")
                    Dim strKodMT As String = ar_MPMT(0)
                    Dim strNamaMT As String = ar_MPMT(1)
                    Dim strJamKreditMT As String = ar_MPMT(2)

                    'MATHEMATIC S1
                    strSQL = "SELECT  GredMT FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='1'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    Dim strGredMT As String = oCommon.getFieldValue(strSQL)

                    'MATHEMATIC S2
                    strSQL = "SELECT KODMatapelajaran,NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='2' AND NamaMataPelajaran='MATHEMATIC' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPMT2 As Array
                    ar_MPMT2 = strRet.Split("|")
                    Dim strKodMT2 As String = ar_MPMT2(0)
                    Dim strNamaMT2 As String = ar_MPMT2(1)
                    Dim strJamKreditMT2 As String = ar_MPMT2(2)

                    'MATHEMATIC S2
                    Dim strGredMT2 As String = ""
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                    strSQL += " AND Semester='2'"
                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                    If oCommon.isExist(strSQL) = True Then
                        strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='2'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_HPelajar As Array
                        ar_HPelajar = strRet.Split("|")
                        Dim strP_ID As String = ar_HPelajar(0)
                        Dim strP_Tahun As String = ar_HPelajar(1)
                        Dim strP_Sesi As String = ar_HPelajar(2)

                        strSQL = "SELECT  GredMT FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "'"
                        strSQL += " AND Tahun='" & strP_Tahun & "'"
                        strSQL += " AND Semester='2'"
                        strSQL += " AND Sesi='" & strP_Sesi & "'"
                        strGredMT2 = oCommon.getFieldValue(strSQL)
                    End If

                    Dim myTableMT As New PdfPTable(10)
                    myTableMT.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTableMT.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    myTableMT.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                    Dim intTblCellMT() As Integer = {15, 40, 10, 10, 10, 10, 15, 40, 10, 10}
                    myTableMT.SetWidths(intTblCellMT)

                    myTableMT.AddCell(New Phrase(strKodMT, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMT.AddCell(New Phrase(strNamaMT, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMT.AddCell(New Phrase(strJamKreditMT, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMT.AddCell(New Phrase(strGredMT, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMT.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMT.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMT.AddCell(New Phrase(strKodMT2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMT.AddCell(New Phrase(strNamaMT2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMT.AddCell(New Phrase(strJamKreditMT2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMT.AddCell(New Phrase(strGredMT2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(myTableMT)

                    'SCIENCE  S1
                    strSQL = "SELECT KODMatapelajaran,NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='1' AND NamaMataPelajaran='SCIENCE' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPSC As Array
                    ar_MPSC = strRet.Split("|")
                    Dim strKodSC As String = ar_MPSC(0)
                    Dim strNamaSC As String = ar_MPSC(1)
                    Dim strJamKreditSC As String = ar_MPSC(2)

                    'SCIENCE  S1
                    strSQL = "SELECT  GredSC FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='1'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    Dim strGredSC As String = oCommon.getFieldValue(strSQL)

                    'SCIENCE  S2
                    strSQL = "SELECT KODMatapelajaran,NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='2' AND NamaMataPelajaran='SCIENCE' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPSC2 As Array
                    ar_MPSC2 = strRet.Split("|")
                    Dim strKodSC2 As String = ar_MPSC2(0)
                    Dim strNamaSC2 As String = ar_MPSC2(1)
                    Dim strJamKreditSC2 As String = ar_MPSC2(2)

                    'SCIENCE  S2
                    Dim strGredSC2 As String = ""
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                    strSQL += " AND Semester='2'"
                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                    If oCommon.isExist(strSQL) = True Then
                        strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='2'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_HPelajar As Array
                        ar_HPelajar = strRet.Split("|")
                        Dim strP_ID As String = ar_HPelajar(0)
                        Dim strP_Tahun As String = ar_HPelajar(1)
                        Dim strP_Sesi As String = ar_HPelajar(2)

                        strSQL = "SELECT  GredSC FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "'"
                        strSQL += " AND Tahun='" & strP_Tahun & "'"
                        strSQL += " AND Semester='2'"
                        strSQL += " AND Sesi='" & strP_Sesi & "'"
                        strGredSC2 = oCommon.getFieldValue(strSQL)
                    End If

                    Dim myTableSC As New PdfPTable(10)
                    myTableSC.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTableSC.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    myTableSC.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                    Dim intTblCellSC() As Integer = {15, 40, 10, 10, 10, 10, 15, 40, 10, 10}
                    myTableSC.SetWidths(intTblCellSC)

                    myTableSC.AddCell(New Phrase(strKodSC, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSC.AddCell(New Phrase(strNamaSC, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSC.AddCell(New Phrase(strJamKreditSC, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSC.AddCell(New Phrase(strGredSC, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSC.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSC.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSC.AddCell(New Phrase(strKodSC2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSC.AddCell(New Phrase(strNamaSC2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSC.AddCell(New Phrase(strJamKreditSC2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSC.AddCell(New Phrase(strGredSC2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(myTableSC)

                    'SEJARAH S1
                    strSQL = "SELECT KODMatapelajaran,NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='1' AND NamaMataPelajaran='SEJARAH' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPSJ As Array
                    ar_MPSJ = strRet.Split("|")
                    Dim strKodSJ As String = ar_MPSJ(0)
                    Dim strNamaSJ As String = ar_MPSJ(1)
                    Dim strJamKreditSJ As String = ar_MPSJ(2)

                    'SEJARAH S1
                    strSQL = "SELECT  GredSJ FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='1'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    Dim strGredSJ As String = oCommon.getFieldValue(strSQL)

                    'SEJARAH S2
                    strSQL = "SELECT KODMatapelajaran,NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='2' AND NamaMataPelajaran='SEJARAH' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPSJ2 As Array
                    ar_MPSJ2 = strRet.Split("|")
                    Dim strKodSJ2 As String = ar_MPSJ2(0)
                    Dim strNamaSJ2 As String = ar_MPSJ2(1)
                    Dim strJamKreditSJ2 As String = ar_MPSJ2(2)

                    'SEJARAH S2
                    Dim strGredSJ2 As String = ""
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                    strSQL += " AND Semester='2'"
                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                    If oCommon.isExist(strSQL) = True Then
                        strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='2'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_HPelajar As Array
                        ar_HPelajar = strRet.Split("|")
                        Dim strP_ID As String = ar_HPelajar(0)
                        Dim strP_Tahun As String = ar_HPelajar(1)
                        Dim strP_Sesi As String = ar_HPelajar(2)

                        strSQL = "SELECT  GredSJ FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "'"
                        strSQL += " AND Tahun='" & strP_Tahun & "'"
                        strSQL += " AND Semester='2'"
                        strSQL += " AND Sesi='" & strP_Sesi & "'"
                        strGredSJ2 = oCommon.getFieldValue(strSQL)
                    End If

                    Dim myTableSJ As New PdfPTable(10)
                    myTableSJ.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTableSJ.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    myTableSJ.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                    Dim intTblCellSJ() As Integer = {15, 40, 10, 10, 10, 10, 15, 40, 10, 10}
                    myTableSJ.SetWidths(intTblCellSJ)

                    myTableSJ.AddCell(New Phrase(strKodSJ, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSJ.AddCell(New Phrase(strNamaSJ, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSJ.AddCell(New Phrase(strJamKreditSJ, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSJ.AddCell(New Phrase(strGredSJ, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSJ.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSJ.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSJ.AddCell(New Phrase(strKodSJ2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSJ.AddCell(New Phrase(strNamaSJ2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSJ.AddCell(New Phrase(strJamKreditSJ2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSJ.AddCell(New Phrase(strGredSJ2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(myTableSJ)

                    'PENDIDIKAN ISLAM S1
                    strSQL = "SELECT  KODMatapelajaran,NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='1' AND NamaMataPelajaran='PENDIDIKAN ISLAM' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPPI As Array
                    ar_MPPI = strRet.Split("|")
                    Dim strKodPI As String = ar_MPPI(0)
                    Dim strNamaPI As String = ar_MPPI(1)
                    Dim strJamKreditPI As String = ar_MPPI(2)

                    'PENDIDIKAN ISLAM S1
                    strSQL = "SELECT  GredPI FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='1'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    Dim strGredPI As String = oCommon.getFieldValue(strSQL)

                    'PENDIDIKAN ISLAM S2
                    strSQL = "SELECT  KODMatapelajaran,NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='2' AND NamaMataPelajaran='PENDIDIKAN ISLAM' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPPI2 As Array
                    ar_MPPI2 = strRet.Split("|")
                    Dim strKodPI2 As String = ar_MPPI2(0)
                    Dim strNamaPI2 As String = ar_MPPI2(1)
                    Dim strJamKreditPI2 As String = ar_MPPI2(2)

                    'PENDIDIKAN ISLAM S2
                    Dim strGredPI2 As String = ""
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                    strSQL += " AND Semester='2'"
                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                    If oCommon.isExist(strSQL) = True Then
                        strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='2'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_HPelajar As Array
                        ar_HPelajar = strRet.Split("|")
                        Dim strP_ID As String = ar_HPelajar(0)
                        Dim strP_Tahun As String = ar_HPelajar(1)
                        Dim strP_Sesi As String = ar_HPelajar(2)

                        strSQL = "SELECT  GredPI FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "'"
                        strSQL += " AND Tahun='" & strP_Tahun & "'"
                        strSQL += " AND Semester='2'"
                        strSQL += " AND Sesi='" & strP_Sesi & "'"
                        strGredPI2 = oCommon.getFieldValue(strSQL)
                    End If

                    Dim myTablePI As New PdfPTable(10)
                    myTablePI.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTablePI.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    myTablePI.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                    Dim intTblCellPI() As Integer = {15, 40, 10, 10, 10, 10, 15, 40, 10, 10}
                    myTablePI.SetWidths(intTblCellPI)

                    myTablePI.AddCell(New Phrase(strKodPI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePI.AddCell(New Phrase(strNamaPI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePI.AddCell(New Phrase(strJamKreditPI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePI.AddCell(New Phrase(strGredPI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePI.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePI.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePI.AddCell(New Phrase(strKodPI2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePI.AddCell(New Phrase(strNamaPI2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePI.AddCell(New Phrase(strJamKreditPI2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePI.AddCell(New Phrase(strGredPI2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(myTablePI)

                    'PENDIDIKAN MORAL1
                    strSQL = "SELECT  KODMatapelajaran,NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='1' AND NamaMataPelajaran='PENDIDIKAN MORAL' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPPM As Array
                    ar_MPPM = strRet.Split("|")
                    Dim strKodPM As String = ar_MPPM(0)
                    Dim strNamaPM As String = ar_MPPM(1)
                    Dim strJamKreditPM As String = ar_MPPM(2)

                    'PENDIDIKAN MORAL1
                    strSQL = "SELECT  GredPM FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='1'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    Dim strGredPM As String = oCommon.getFieldValue(strSQL)

                    'PENDIDIKAN MORAL2
                    strSQL = "SELECT  KODMatapelajaran,NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='2' AND NamaMataPelajaran='PENDIDIKAN MORAL' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPPM2 As Array
                    ar_MPPM2 = strRet.Split("|")
                    Dim strKodPM2 As String = ar_MPPM2(0)
                    Dim strNamaPM2 As String = ar_MPPM2(1)
                    Dim strJamKreditPM2 As String = ar_MPPM2(2)

                    'PENDIDIKAN MORAL2
                    Dim strGredPM2 As String = ""
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                    strSQL += " AND Semester='2'"
                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                    If oCommon.isExist(strSQL) = True Then
                        strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='2'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_HPelajar As Array
                        ar_HPelajar = strRet.Split("|")
                        Dim strP_ID As String = ar_HPelajar(0)
                        Dim strP_Tahun As String = ar_HPelajar(1)
                        Dim strP_Sesi As String = ar_HPelajar(2)

                        strSQL = "SELECT  GredPM FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "'"
                        strSQL += " AND Tahun='" & strP_Tahun & "'"
                        strSQL += " AND Semester='2'"
                        strSQL += " AND Sesi='" & strP_Sesi & "'"
                        strGredPM2 = oCommon.getFieldValue(strSQL)
                    End If

                    Dim myTablePM As New PdfPTable(10)
                    myTablePM.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTablePM.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    myTablePM.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                    Dim intTblCellPM() As Integer = {15, 40, 10, 10, 10, 10, 15, 40, 10, 10}
                    myTablePM.SetWidths(intTblCellPM)

                    myTablePM.AddCell(New Phrase(strKodPM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePM.AddCell(New Phrase(strNamaPM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePM.AddCell(New Phrase(strJamKreditPM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePM.AddCell(New Phrase(strGredPM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePM.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePM.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePM.AddCell(New Phrase(strKodPM2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePM.AddCell(New Phrase(strNamaPM2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePM.AddCell(New Phrase(strJamKreditPM2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePM.AddCell(New Phrase(strGredPM2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(myTablePM)

                    ' ''Vokasional 1
                    ' '' ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Dim strKodV1 As String = ""
                    Dim strNamaKV1 As String = ""
                    Dim strJamKredit1 As String = ""
                    Dim strGred1 As String = ""

                    Dim strKodV2 As String = ""
                    Dim strNamaKV2 As String = ""
                    Dim strJamKredit2 As String = ""
                    Dim strGred2 As String = ""

                    strSQL = " SELECT COUNT(kpmkv_modul.KODMODUL) AS BILMODUL FROM kpmkv_modul LEFT OUTER JOIN "
                    strSQL += " kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID"
                    strSQL += " WHERE kpmkv_modul.Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND kpmkv_modul.Semester='1'"
                    strSQL += " AND kpmkv_modul.Sesi='" & chkSesi.Text & "'"
                    strSQL += " AND kpmkv_modul.KursusID ='" & ddlKodKursus.Text & "'"
                    Dim strBilModul1 As Integer = oCommon.getFieldValue(strSQL)

                    strSQL = " SELECT COUNT(kpmkv_modul.KODMODUL) AS BILMODUL FROM kpmkv_modul LEFT OUTER JOIN "
                    strSQL += " kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID"
                    strSQL += " WHERE kpmkv_modul.Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND kpmkv_modul.Semester='2'"
                    strSQL += " AND kpmkv_modul.Sesi='" & chkSesi.Text & "'"
                    strSQL += " AND kpmkv_modul.KursusID ='" & ddlKodKursus.Text & "'"
                    Dim strBilModul2 As Integer = oCommon.getFieldValue(strSQL)

                    For M As Integer = 1 To strBilModul1
                        strSQL = " SELECT kpmkv_modul.KodModul, kpmkv_modul.NamaModul, kpmkv_modul.JamKredit"
                        strSQL += " FROM  kpmkv_modul LEFT OUTER JOIN kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
                        strSQL += " kpmkv_pelajar ON kpmkv_kursus.KursusID = kpmkv_pelajar.KursusID"
                        strSQL += " WHERE kpmkv_modul.Tahun='" & ddlTahun.Text & "'"
                        strSQL += " AND kpmkv_modul.Semester='1'"
                        strSQL += " AND kpmkv_modul.Sesi='" & chkSesi.Text & "'"
                        strSQL += " AND SUBSTRING(kpmkv_modul.KodModul,6,1)='" & M & "'"
                        strSQL += " AND kpmkv_modul.KursusID='" & ddlKodKursus.Text & "'"
                        strSQL += " AND kpmkv_pelajar.PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " ORDER BY  kpmkv_modul.KodModul, kpmkv_modul.NamaModul, kpmkv_modul.JamKredit ASC"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        Dim ar_VokM1 As Array
                        ar_VokM1 = strRet.Split("|")
                        strKodV1 = ar_VokM1(0)
                        strNamaKV1 = ar_VokM1(1)
                        strJamKredit1 = ar_VokM1(2)

                        strSQL = "SELECT GredV" & M & " FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "' AND Tahun='" & ddlTahun.Text & "'  AND Semester='1' AND Sesi='" & chkSesi.Text & "'"
                        strGred1 = oCommon.getFieldValue(strSQL)

                        ' ''Vokasional 1
                        ' '' ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        For M2 As Integer = 1 To strBilModul2

                            strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                            strSQL += " AND Semester='2'"
                            strSQL += " AND IsDeleted='N' AND StatusID='2'"
                            If oCommon.isExist(strSQL) = True Then
                                strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                                strSQL += " AND Semester='2'"
                                strSQL += " AND IsDeleted='N' AND StatusID='2'"
                                strRet = oCommon.getFieldValueEx(strSQL)

                                Dim ar_HPelajar As Array
                                ar_HPelajar = strRet.Split("|")
                                Dim strP_ID As String = ar_HPelajar(0)
                                Dim strP_Tahun As String = ar_HPelajar(1)
                                Dim strP_Sesi As String = ar_HPelajar(2)

                                strSQL = " SELECT kpmkv_modul.KodModul, kpmkv_modul.NamaModul, kpmkv_modul.JamKredit"
                                strSQL += " FROM  kpmkv_modul LEFT OUTER JOIN kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
                                strSQL += " kpmkv_pelajar ON kpmkv_kursus.KursusID = kpmkv_pelajar.KursusID"
                                strSQL += " WHERE kpmkv_modul.Tahun='" & strP_Tahun & "'"
                                strSQL += " AND kpmkv_modul.Semester='2'"
                                strSQL += " AND kpmkv_modul.Sesi='" & strP_Sesi & "'"
                                strSQL += " AND SUBSTRING(kpmkv_modul.KodModul,6,1)='" & M2 & "'"
                                strSQL += " AND kpmkv_modul.KursusID='" & ddlKodKursus.Text & "'"
                                strSQL += " AND kpmkv_pelajar.PelajarID='" & strP_ID & "'"
                                strSQL += " ORDER BY  kpmkv_modul.KodModul, kpmkv_modul.NamaModul, kpmkv_modul.JamKredit ASC"
                                strRet = oCommon.getFieldValueEx(strSQL)
                                Dim ar_VokM2 As Array
                                ar_VokM2 = strRet.Split("|")
                                strKodV2 = ar_VokM2(0)
                                strNamaKV2 = ar_VokM2(1)
                                strJamKredit2 = ar_VokM2(2)

                                strSQL = "SELECT GredV" & M2 & " FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "' AND Tahun='" & strP_Tahun & "'  AND Semester='2' AND Sesi='" & strP_Sesi & "'"
                                strGred2 = oCommon.getFieldValue(strSQL)
                            End If

                        Next M2
                        Dim myTableV As New PdfPTable(10)
                        myTableV.WidthPercentage = 100 ' Table size is set to 100% of the page
                        myTableV.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                        myTableV.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                        Dim intTblCellV() As Integer = {15, 40, 10, 10, 10, 10, 15, 40, 10, 10}
                        myTableV.SetWidths(intTblCellV)
                        myTableV.AddCell(New Phrase(strKodV1, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableV.AddCell(New Phrase(strNamaKV1, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableV.AddCell(New Phrase(strJamKredit1, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableV.AddCell(New Phrase(strGred1, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableV.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableV.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableV.AddCell(New Phrase(strKodV2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableV.AddCell(New Phrase(strNamaKV2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableV.AddCell(New Phrase(strJamKredit2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableV.AddCell(New Phrase(strGred2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myDocument.Add(myTableV)


                    Next M
                    myDocument.Add(imgSpacing)

                    '--------------------------------------------------------------------------------------------------------------------
                    'Semester 3 & 4

                    Dim myTableSubTitle As New PdfPTable(10)
                    myTableSubTitle.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTableSubTitle.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    'myTable.HorizontalAlignment = Rectangle.NO_BORDER
                    Dim intTblWidthSubTitle() As Integer = {36, 12, 12, 12, 10, 10, 36, 12, 12, 12}
                    myTableSubTitle.SetWidths(intTblWidthSubTitle)

                    Dim Cell1Sub As New PdfPCell(New Phrase("SEMESTER ", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell1Sub.Border = Rectangle.NO_BORDER
                    myTableSubTitle.AddCell(Cell1Sub)

                    Dim Cell2Sub As New PdfPCell(New Phrase("3", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell2Sub.Border = Rectangle.NO_BORDER
                    myTableSubTitle.AddCell(Cell2Sub)

                    Dim Cell3Sub As New PdfPCell(New Phrase("KOHORT", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell3Sub.Border = Rectangle.NO_BORDER
                    myTableSubTitle.AddCell(Cell3Sub)

                    Dim Cell4Sub As New PdfPCell(New Phrase("" & ddlTahun.Text, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell4Sub.Border = Rectangle.NO_BORDER
                    myTableSubTitle.AddCell(Cell4Sub)

                    Dim Cell5Sub As New PdfPCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell5Sub.Border = Rectangle.NO_BORDER
                    myTableSubTitle.AddCell(Cell5Sub)

                    Dim Cell6Sub As New PdfPCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell6Sub.Border = Rectangle.NO_BORDER
                    myTableSubTitle.AddCell(Cell6Sub)

                    Dim Cell7Sub As New PdfPCell(New Phrase("SEMESTER", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell7Sub.Border = Rectangle.NO_BORDER
                    myTableSubTitle.AddCell(Cell7Sub)

                    Dim Cell8Sub As New PdfPCell(New Phrase("4", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell8Sub.Border = Rectangle.NO_BORDER
                    myTableSubTitle.AddCell(Cell8Sub)

                    Dim Cell9Sub As New PdfPCell(New Phrase("KOHORT", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell9Sub.Border = Rectangle.NO_BORDER
                    myTableSubTitle.AddCell(Cell9Sub)

                    Dim Cell10Sub As New PdfPCell(New Phrase("" & ddlTahun.Text, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell10Sub.Border = Rectangle.NO_BORDER
                    myTableSubTitle.AddCell(Cell10Sub)
                    myDocument.Add(myTableSubTitle)

                    Dim myTableSub1 As New PdfPTable(10)
                    myTableSub1.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTableSub1.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    'myTable.HorizontalAlignment = Rectangle.NO_BORDER
                    Dim intTblWidthSub() As Integer = {15, 40, 10, 10, 10, 10, 15, 40, 10, 10}
                    myTableSub1.SetWidths(intTblWidthSub)

                    Dim Cell1SubHdr As New PdfPCell(New Phrase("KOD ", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell1SubHdr.Border = Rectangle.NO_BORDER
                    myTableSub1.AddCell(Cell1SubHdr)

                    Dim Cell2SubHdr As New PdfPCell(New Phrase("MATAPELAJARAN", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell2SubHdr.Border = Rectangle.NO_BORDER
                    myTableSub1.AddCell(Cell2SubHdr)

                    Dim Cell3SubHdr As New PdfPCell(New Phrase("JAM KREDIT", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell3SubHdr.Border = Rectangle.NO_BORDER
                    myTableSub1.AddCell(Cell3SubHdr)

                    Dim Cell4SubHdr As New PdfPCell(New Phrase("GRED", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell4SubHdr.Border = Rectangle.NO_BORDER
                    myTableSub1.AddCell(Cell4SubHdr)

                    Dim Cell5SubHdr As New PdfPCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell5SubHdr.Border = Rectangle.NO_BORDER
                    myTableSub1.AddCell(Cell5SubHdr)

                    Dim Cell5_1SubHdr As New PdfPCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell5_1SubHdr.Border = Rectangle.NO_BORDER
                    myTableSub1.AddCell(Cell5_1SubHdr)

                    Dim Cell6SubHdr As New PdfPCell(New Phrase("KOD", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell6SubHdr.Border = Rectangle.NO_BORDER
                    myTableSub1.AddCell(Cell6SubHdr)

                    Dim Cell7SubHdr As New PdfPCell(New Phrase("MATAPELAJARAN", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell7SubHdr.Border = Rectangle.NO_BORDER
                    myTableSub1.AddCell(Cell7SubHdr)

                    Dim Cell8SubHdr As New PdfPCell(New Phrase("JAM KREDIT", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell8SubHdr.Border = Rectangle.NO_BORDER
                    myTableSub1.AddCell(Cell8SubHdr)

                    Dim Cell9SubHdr As New PdfPCell(New Phrase("GRED", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell9SubHdr.Border = Rectangle.NO_BORDER
                    myTableSub1.AddCell(Cell9SubHdr)
                    myDocument.Add(myTable)

                    'S3
                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='3' AND NamaMataPelajaran='BAHASA MELAYU'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPBM3 As Array
                    ar_MPBM3 = strRet.Split("|")
                    Dim strKodBM3 As String = ar_MPBM3(0)
                    Dim strNamaBM3 As String = ar_MPBM3(1)
                    Dim strJamKreditBM3 As String = ar_MPBM3(2)

                    'S3
                    Dim strGredBM3 As String = ""
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                    strSQL += " AND Semester='3'"
                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                    If oCommon.isExist(strSQL) = True Then
                        strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='3'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_HPelajar As Array
                        ar_HPelajar = strRet.Split("|")
                        Dim strP_ID As String = ar_HPelajar(0)
                        Dim strP_Tahun As String = ar_HPelajar(1)
                        Dim strP_Sesi As String = ar_HPelajar(2)

                        strSQL = "SELECT  GredBM FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "'"
                        strSQL += " AND Tahun='" & strP_Tahun & "'"
                        strSQL += " AND Semester='3'"
                        strSQL += " AND Sesi='" & strP_Sesi & "'"
                        strGredBM3 = oCommon.getFieldValue(strSQL)
                    End If

                    'S4
                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='4' AND NamaMataPelajaran='BAHASA MELAYU' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPBM4 As Array
                    ar_MPBM4 = strRet.Split("|")
                    Dim strKodBM4 As String = ar_MPBM4(0)
                    Dim strNamaBM4 As String = ar_MPBM4(1)
                    Dim strJamKreditBM4 As String = ar_MPBM4(2)

                    'S4
                    Dim strGredBM4 As String = ""
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                    strSQL += " AND Semester='4'"
                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                    If oCommon.isExist(strSQL) = True Then
                        strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='4'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_HPelajar As Array
                        ar_HPelajar = strRet.Split("|")
                        Dim strP_ID As String = ar_HPelajar(0)
                        Dim strP_Tahun As String = ar_HPelajar(1)
                        Dim strP_Sesi As String = ar_HPelajar(2)

                        strSQL = "SELECT  GredBM FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "'"
                        strSQL += " AND Tahun='" & strP_Tahun & "'"
                        strSQL += " AND Semester='4'"
                        strSQL += " AND Sesi='" & strP_Sesi & "'"
                        strGredBM4 = oCommon.getFieldValue(strSQL)
                    End If

                    Dim myTableBM As New PdfPTable(10)
                    myTableBM.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTableBM.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    myTableBM.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                    Dim intTblCellBM() As Integer = {15, 40, 10, 10, 10, 10, 15, 40, 10, 10}
                    myTableBM.SetWidths(intTblCellBM)

                    myTableBM.AddCell(New Phrase(strKodBM3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBM.AddCell(New Phrase(strNamaBM3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBM.AddCell(New Phrase(strJamKreditBM3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBM.AddCell(New Phrase(strGredBM3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBM.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBM.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBM.AddCell(New Phrase(strKodBM4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBM.AddCell(New Phrase(strNamaBM4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBM.AddCell(New Phrase(strJamKreditBM4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBM.AddCell(New Phrase(strGredBM4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(myTableBM)

                    'BI S3
                    strSQL = "SELECT KODMatapelajaran,NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='3' AND NamaMataPelajaran='BAHASA INGGERIS' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPBI3 As Array
                    ar_MPBI3 = strRet.Split("|")
                    Dim strKodBI3 As String = ar_MPBI3(0)
                    Dim strNamaBI3 As String = ar_MPBI3(1)
                    Dim strJamKreditBI3 As String = ar_MPBI3(2)

                    'BI S3
                    Dim strGredBI3 As String = ""
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                    strSQL += " AND Semester='3'"
                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                    If oCommon.isExist(strSQL) = True Then
                        strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='3'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_HPelajar As Array
                        ar_HPelajar = strRet.Split("|")
                        Dim strP_ID As String = ar_HPelajar(0)
                        Dim strP_Tahun As String = ar_HPelajar(1)
                        Dim strP_Sesi As String = ar_HPelajar(2)

                        strSQL = "SELECT  GredBI FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "'"
                        strSQL += " AND Tahun='" & strP_Tahun & "'"
                        strSQL += " AND Semester='3'"
                        strSQL += " AND Sesi='" & strP_Sesi & "'"
                        strGredBI3 = oCommon.getFieldValue(strSQL)
                    End If

                    'BI S4
                    strSQL = "SELECT KODMatapelajaran,NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='4' AND NamaMataPelajaran='BAHASA INGGERIS' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPBI4 As Array
                    ar_MPBI4 = strRet.Split("|")
                    Dim strKodBI4 As String = ar_MPBI4(0)
                    Dim strNamaBI4 As String = ar_MPBI4(1)
                    Dim strJamKreditBI4 As String = ar_MPBI4(2)

                    'BI S4
                    Dim strGredBI4 As String = ""
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                    strSQL += " AND Semester='4'"
                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                    If oCommon.isExist(strSQL) = True Then
                        strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='4'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_HPelajar As Array
                        ar_HPelajar = strRet.Split("|")
                        Dim strP_ID As String = ar_HPelajar(0)
                        Dim strP_Tahun As String = ar_HPelajar(1)
                        Dim strP_Sesi As String = ar_HPelajar(2)

                        strSQL = "SELECT  GredBI FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "'"
                        strSQL += " AND Tahun='" & strP_Tahun & "'"
                        strSQL += " AND Semester='4'"
                        strSQL += " AND Sesi='" & strP_Sesi & "'"
                        strGredBI4 = oCommon.getFieldValue(strSQL)
                    End If

                    Dim myTableBI4 As New PdfPTable(10)
                    myTableBI4.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTableBI4.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    myTableBI4.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                    Dim intTblCellBI4() As Integer = {15, 40, 10, 10, 10, 10, 15, 40, 10, 10}
                    myTableBI4.SetWidths(intTblCellBI4)

                    myTableBI4.AddCell(New Phrase(strKodBI3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBI4.AddCell(New Phrase(strNamaBI3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBI4.AddCell(New Phrase(strJamKreditBI3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBI4.AddCell(New Phrase(strGredBI3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBI4.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBI4.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBI4.AddCell(New Phrase(strKodBI4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBI4.AddCell(New Phrase(strNamaBI4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBI4.AddCell(New Phrase(strJamKreditBI4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableBI4.AddCell(New Phrase(strGredBI4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(myTableBI4)

                    'MATHEMATIC S3
                    'change on 28July2016

                    'Sc for teknologi or social  
                    strSQL = "SELECT  JenisKursus From kpmkv_kursus Where KursusID='" & ddlKodKursus.SelectedValue & "'"
                    Dim strJenisKursusMT As String = oCommon.getFieldValue(strSQL)
                    Dim strKodMT3 As String = ""
                    strSQL = "SELECT JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='3' AND NamaMataPelajaran='MATHEMATIC' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPMT3 As Array
                    ar_MPMT3 = strRet.Split("|")
                    Dim strJamKreditMT3 As String = ar_MPMT3(0)

                    Dim strNamaMT3 As String
                    If strJenisKursusMT = "SOCIAL" Then
                        strNamaMT3 = "MATHEMATIC FOR SOCIAL"
                        strKodMT3 = "AMT3101"
                    Else
                        strNamaMT3 = "MATHEMATIC FOR TECHNOLOGY"
                        strKodMT3 = "AMT3121"
                    End If

                    'MATHEMATIC S3

                    Dim strGredMT3 As String = ""
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                    strSQL += " AND Semester='3'"
                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                    If oCommon.isExist(strSQL) = True Then
                        strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='3'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_HPelajar As Array
                        ar_HPelajar = strRet.Split("|")
                        Dim strP_ID As String = ar_HPelajar(0)
                        Dim strP_Tahun As String = ar_HPelajar(1)
                        Dim strP_Sesi As String = ar_HPelajar(2)

                        strSQL = "SELECT  GredMT FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "'"
                        strSQL += " AND Tahun='" & strP_Tahun & "'"
                        strSQL += " AND Semester='3'"
                        strSQL += " AND Sesi='" & strP_Sesi & "'"
                        strGredMT3 = oCommon.getFieldValue(strSQL)
                    End If

                    'MATHEMATIC S4
                    Dim strKodMT4 As String = ""
                    strSQL = "SELECT JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='4' AND NamaMataPelajaran='MATHEMATIC' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPMT4 As Array
                    ar_MPMT4 = strRet.Split("|")
                    Dim strJamKreditMT4 As String = ar_MPMT4(0)

                    Dim strNamaMT4 As String
                    If strJenisKursusMT = "SOCIAL" Then
                        strNamaMT4 = "MATHEMATIC FOR SOCIAL"
                        strKodMT4 = "AMT4101"
                    Else
                        strNamaMT4 = "MATHEMATIC FOR TECHNOLOGY"
                        strKodMT4 = "AMT4091"
                    End If

                    'MATHEMATIC S2
                    Dim strGredMT4 As String = ""
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                    strSQL += " AND Semester='4'"
                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                    If oCommon.isExist(strSQL) = True Then
                        strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='4'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_HPelajar As Array
                        ar_HPelajar = strRet.Split("|")
                        Dim strP_ID As String = ar_HPelajar(0)
                        Dim strP_Tahun As String = ar_HPelajar(1)
                        Dim strP_Sesi As String = ar_HPelajar(2)

                        strSQL = "SELECT  GredMT FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "'"
                        strSQL += " AND Tahun='" & strP_Tahun & "'"
                        strSQL += " AND Semester='4'"
                        strSQL += " AND Sesi='" & strP_Sesi & "'"
                        strGredMT4 = oCommon.getFieldValue(strSQL)
                    End If

                    Dim myTableMT4 As New PdfPTable(10)
                    myTableMT4.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTableMT4.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    myTableMT4.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                    Dim intTblCellMT4() As Integer = {15, 40, 10, 10, 10, 10, 15, 40, 10, 10}
                    myTableMT4.SetWidths(intTblCellMT4)

                    myTableMT4.AddCell(New Phrase(strKodMT3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMT4.AddCell(New Phrase(strNamaMT3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMT4.AddCell(New Phrase(strJamKreditMT3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMT4.AddCell(New Phrase(strGredMT3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMT4.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMT4.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMT4.AddCell(New Phrase(strKodMT4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMT4.AddCell(New Phrase(strNamaMT4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMT4.AddCell(New Phrase(strJamKreditMT4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMT4.AddCell(New Phrase(strGredMT4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(myTableMT4)

                    'SCIENCE  S3
                    'change on 28July2016

                    'Sc for teknologi or social  

                    strSQL = "SELECT  JenisKursus From kpmkv_kursus Where KursusID='" & ddlKodKursus.SelectedValue & "'"
                    Dim strJenisKursusSC As String = oCommon.getFieldValue(strSQL)

                    Dim strKodSC3 As String = ""

                    strSQL = "SELECT JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='3' AND NamaMataPelajaran='SCIENCE' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPSC3 As Array
                    ar_MPSC3 = strRet.Split("|")
                    Dim strJamKreditSC3 As String = ar_MPSC3(0)

                    Dim strNamaSC3 As String
                    If strJenisKursusMT = "SOCIAL" Then
                        strNamaSC3 = "SCIENCE FOR SOCIAL"
                        strKodSC3 = "AMT3131"
                    Else
                        strNamaSC3 = "SCIENCE FOR TECHNOLOGY"
                        strKodSC3 = "AMT3121"
                    End If
                    'SCIENCE  S3
                    Dim strGredSC3 As String = ""
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                    strSQL += " AND Semester='3'"
                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                    If oCommon.isExist(strSQL) = True Then
                        strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='3'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_HPelajar As Array
                        ar_HPelajar = strRet.Split("|")
                        Dim strP_ID As String = ar_HPelajar(0)
                        Dim strP_Tahun As String = ar_HPelajar(1)
                        Dim strP_Sesi As String = ar_HPelajar(2)

                        strSQL = "SELECT  GredSC FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "'"
                        strSQL += " AND Tahun='" & strP_Tahun & "'"
                        strSQL += " AND Semester='3'"
                        strSQL += " AND Sesi='" & strP_Sesi & "'"
                        strGredSC3 = oCommon.getFieldValue(strSQL)
                    End If

                    'SCIENCE  S4
                    Dim strKodSC4 As String = ""
                    strSQL = "SELECT KODMatapelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='4' AND NamaMataPelajaran='SCIENCE' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPSC4 As Array
                    ar_MPSC4 = strRet.Split("|")
                    Dim strJamKreditSC4 As String = ar_MPSC4(0)

                    Dim strNamaSC4 As String
                    If strJenisKursusMT = "SOCIAL" Then
                        strNamaSC4 = "SCIENCE FOR SOCIAL"
                        strKodSC4 = "AMT4131"
                    Else
                        strNamaSC4 = "SCIENCE FOR TECHNOLOGY"
                        strKodSC4 = "AMT4121"
                    End If

                    Dim strGredSC4 As String = ""
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                    strSQL += " AND Semester='4'"
                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                    If oCommon.isExist(strSQL) = True Then
                        strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='4'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_HPelajar As Array
                        ar_HPelajar = strRet.Split("|")
                        Dim strP_ID As String = ar_HPelajar(0)
                        Dim strP_Tahun As String = ar_HPelajar(1)
                        Dim strP_Sesi As String = ar_HPelajar(2)

                        strSQL = "SELECT  GredSC FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "'"
                        strSQL += " AND Tahun='" & strP_Tahun & "'"
                        strSQL += " AND Semester='4'"
                        strSQL += " AND Sesi='" & strP_Sesi & "'"
                        strGredSC4 = oCommon.getFieldValue(strSQL)
                    End If

                    Dim myTableSC4 As New PdfPTable(10)
                    myTableSC4.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTableSC4.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    myTableSC4.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                    Dim intTblCellSC4() As Integer = {15, 40, 10, 10, 10, 10, 15, 40, 10, 10}
                    myTableSC4.SetWidths(intTblCellSC4)

                    myTableSC4.AddCell(New Phrase(strKodSC3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSC4.AddCell(New Phrase(strNamaSC3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSC4.AddCell(New Phrase(strJamKreditSC3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSC4.AddCell(New Phrase(strGredSC3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSC4.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSC4.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSC4.AddCell(New Phrase(strKodSC4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSC4.AddCell(New Phrase(strNamaSC4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSC4.AddCell(New Phrase(strJamKreditSC4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSC4.AddCell(New Phrase(strGredSC4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(myTableSC4)

                    'SEJARAH S3
                    strSQL = "SELECT KODMatapelajaran,NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='3' AND NamaMataPelajaran='SEJARAH' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPSJ3 As Array
                    ar_MPSJ3 = strRet.Split("|")
                    Dim strKodSJ3 As String = ar_MPSJ3(0)
                    Dim strNamaSJ3 As String = ar_MPSJ3(1)
                    Dim strJamKreditSJ3 As String = ar_MPSJ3(2)

                    'SEJARAH S3
                    Dim strGredSJ3 As String = ""
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                    strSQL += " AND Semester='3'"
                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                    If oCommon.isExist(strSQL) = True Then
                        strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='3'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_HPelajar As Array
                        ar_HPelajar = strRet.Split("|")
                        Dim strP_ID As String = ar_HPelajar(0)
                        Dim strP_Tahun As String = ar_HPelajar(1)
                        Dim strP_Sesi As String = ar_HPelajar(2)

                        strSQL = "SELECT  GredSJ FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "'"
                        strSQL += " AND Tahun='" & strP_Tahun & "'"
                        strSQL += " AND Semester='3'"
                        strSQL += " AND Sesi='" & strP_Sesi & "'"
                        strGredSJ3 = oCommon.getFieldValue(strSQL)
                    End If

                    'SEJARAH S4
                    strSQL = "SELECT KODMatapelajaran,NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='4' AND NamaMataPelajaran='SEJARAH' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPSJ4 As Array
                    ar_MPSJ4 = strRet.Split("|")
                    Dim strKodSJ4 As String = ar_MPSJ4(0)
                    Dim strNamaSJ4 As String = ar_MPSJ4(1)
                    Dim strJamKreditSJ4 As String = ar_MPSJ4(2)

                    'SEJARAH S4
                    Dim strGredSJ4 As String = ""
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                    strSQL += " AND Semester='4'"
                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                    If oCommon.isExist(strSQL) = True Then
                        strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='4'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_HPelajar As Array
                        ar_HPelajar = strRet.Split("|")
                        Dim strP_ID As String = ar_HPelajar(0)
                        Dim strP_Tahun As String = ar_HPelajar(1)
                        Dim strP_Sesi As String = ar_HPelajar(2)

                        strSQL = "SELECT  GredSJ FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "'"
                        strSQL += " AND Tahun='" & strP_Tahun & "'"
                        strSQL += " AND Semester='4'"
                        strSQL += " AND Sesi='" & strP_Sesi & "'"
                        strGredSJ4 = oCommon.getFieldValue(strSQL)
                    End If

                    Dim myTableSJ4 As New PdfPTable(10)
                    myTableSJ4.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTableSJ4.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    myTableSJ4.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                    Dim intTblCellSJ4() As Integer = {15, 40, 10, 10, 10, 10, 15, 40, 10, 10}
                    myTableSJ4.SetWidths(intTblCellSJ4)

                    myTableSJ4.AddCell(New Phrase(strKodSJ3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSJ4.AddCell(New Phrase(strNamaSJ3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSJ4.AddCell(New Phrase(strJamKreditSJ3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSJ4.AddCell(New Phrase(strGredSJ3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSJ4.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSJ4.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSJ4.AddCell(New Phrase(strKodSJ4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSJ4.AddCell(New Phrase(strNamaSJ4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSJ4.AddCell(New Phrase(strJamKreditSJ4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableSJ4.AddCell(New Phrase(strGredSJ4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(myTableSJ)

                    'PENDIDIKAN ISLAM S3
                    strSQL = "SELECT  KODMatapelajaran,NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='3' AND NamaMataPelajaran='PENDIDIKAN ISLAM' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPPI3 As Array
                    ar_MPPI3 = strRet.Split("|")
                    Dim strKodPI3 As String = ar_MPPI3(0)
                    Dim strNamaPI3 As String = ar_MPPI3(1)
                    Dim strJamKreditPI3 As String = ar_MPPI3(2)

                    'PENDIDIKAN ISLAM S3
                    Dim strGredPI3 As String = ""
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                    strSQL += " AND Semester='3'"
                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                    If oCommon.isExist(strSQL) = True Then
                        strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='3'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_HPelajar As Array
                        ar_HPelajar = strRet.Split("|")
                        Dim strP_ID As String = ar_HPelajar(0)
                        Dim strP_Tahun As String = ar_HPelajar(1)
                        Dim strP_Sesi As String = ar_HPelajar(2)

                        strSQL = "SELECT  GredPI FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "'"
                        strSQL += " AND Tahun='" & strP_Tahun & "'"
                        strSQL += " AND Semester='3'"
                        strSQL += " AND Sesi='" & strP_Sesi & "'"
                        strGredPI3 = oCommon.getFieldValue(strSQL)
                    End If

                    'PENDIDIKAN ISLAM S4
                    strSQL = "SELECT  KODMatapelajaran,NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='4' AND NamaMataPelajaran='PENDIDIKAN ISLAM' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPPI4 As Array
                    ar_MPPI4 = strRet.Split("|")
                    Dim strKodPI4 As String = ar_MPPI4(0)
                    Dim strNamaPI4 As String = ar_MPPI4(1)
                    Dim strJamKreditPI4 As String = ar_MPPI4(2)

                    'PENDIDIKAN ISLAM S4
                    Dim strGredPI4 As String = ""
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                    strSQL += " AND Semester='4'"
                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                    If oCommon.isExist(strSQL) = True Then
                        strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='4'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_HPelajar As Array
                        ar_HPelajar = strRet.Split("|")
                        Dim strP_ID As String = ar_HPelajar(0)
                        Dim strP_Tahun As String = ar_HPelajar(1)
                        Dim strP_Sesi As String = ar_HPelajar(2)

                        strSQL = "SELECT  GredPI FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "'"
                        strSQL += " AND Tahun='" & strP_Tahun & "'"
                        strSQL += " AND Semester='4'"
                        strSQL += " AND Sesi='" & strP_Sesi & "'"
                        strGredPI4 = oCommon.getFieldValue(strSQL)
                    End If

                    Dim myTablePI4 As New PdfPTable(10)
                    myTablePI4.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTablePI4.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    myTablePI4.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                    Dim intTblCellPI4() As Integer = {15, 40, 10, 10, 10, 10, 15, 40, 10, 10}
                    myTablePI4.SetWidths(intTblCellPI4)

                    myTablePI4.AddCell(New Phrase(strKodPI3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePI4.AddCell(New Phrase(strNamaPI3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePI4.AddCell(New Phrase(strJamKreditPI3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePI4.AddCell(New Phrase(strGredPI3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePI4.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePI4.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePI4.AddCell(New Phrase(strKodPI4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePI4.AddCell(New Phrase(strNamaPI4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePI4.AddCell(New Phrase(strJamKreditPI4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePI4.AddCell(New Phrase(strGredPI4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(myTablePI4)

                    'PENDIDIKAN MORAL3
                    strSQL = "SELECT  KODMatapelajaran,NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='3' AND NamaMataPelajaran='PENDIDIKAN MORAL' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPPM3 As Array
                    ar_MPPM3 = strRet.Split("|")
                    Dim strKodPM3 As String = ar_MPPM3(0)
                    Dim strNamaPM3 As String = ar_MPPM3(1)
                    Dim strJamKreditPM3 As String = ar_MPPM3(2)

                    'PENDIDIKAN MORAL3
                    Dim strGredPM3 As String = ""
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                    strSQL += " AND Semester='3'"
                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                    If oCommon.isExist(strSQL) = True Then
                        strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='3'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_HPelajar As Array
                        ar_HPelajar = strRet.Split("|")
                        Dim strP_ID As String = ar_HPelajar(0)
                        Dim strP_Tahun As String = ar_HPelajar(1)
                        Dim strP_Sesi As String = ar_HPelajar(2)

                        strSQL = "SELECT  GredPM FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "'"
                        strSQL += " AND Tahun='" & strP_Tahun & "'"
                        strSQL += " AND Semester='3'"
                        strSQL += " AND Sesi='" & strP_Sesi & "'"
                        strGredPM3 = oCommon.getFieldValue(strSQL)
                    End If

                    'PENDIDIKAN MORAL4
                    strSQL = "SELECT  KODMatapelajaran,NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='4' AND NamaMataPelajaran='PENDIDIKAN MORAL' AND Tahun='" & ddlTahun.Text & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPPM4 As Array
                    ar_MPPM4 = strRet.Split("|")
                    Dim strKodPM4 As String = ar_MPPM4(0)
                    Dim strNamaPM4 As String = ar_MPPM4(1)
                    Dim strJamKreditPM4 As String = ar_MPPM4(2)

                    'PENDIDIKAN MORAL4
                    Dim strGredPM4 As String = ""
                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                    strSQL += " AND Semester='4'"
                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                    If oCommon.isExist(strSQL) = True Then
                        strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='4'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_HPelajar As Array
                        ar_HPelajar = strRet.Split("|")
                        Dim strP_ID As String = ar_HPelajar(0)
                        Dim strP_Tahun As String = ar_HPelajar(1)
                        Dim strP_Sesi As String = ar_HPelajar(2)

                        strSQL = "SELECT  GredPM FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "'"
                        strSQL += " AND Tahun='" & strP_Tahun & "'"
                        strSQL += " AND Semester='4'"
                        strSQL += " AND Sesi='" & strP_Sesi & "'"
                        strGredPM4 = oCommon.getFieldValue(strSQL)
                    End If

                    Dim myTablePM4 As New PdfPTable(10)
                    myTablePM4.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTablePM4.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    myTablePM4.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                    Dim intTblCellPM4() As Integer = {15, 40, 10, 10, 10, 10, 15, 40, 10, 10}
                    myTablePM4.SetWidths(intTblCellPM4)

                    myTablePM4.AddCell(New Phrase(strKodPM3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePM4.AddCell(New Phrase(strNamaPM3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePM4.AddCell(New Phrase(strJamKreditPM3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePM4.AddCell(New Phrase(strGredPM3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePM4.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePM4.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePM4.AddCell(New Phrase(strKodPM4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePM4.AddCell(New Phrase(strNamaPM4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePM4.AddCell(New Phrase(strJamKreditPM4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTablePM4.AddCell(New Phrase(strGredPM4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(myTablePM4)

                    ' ''Vokasional 1
                    ' '' ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Dim strKodV3 As String = ""
                    Dim strNamaKV3 As String = ""
                    Dim strJamKredit3 As String = ""
                    Dim strGred3 As String = ""

                    Dim strKodV4 As String = ""
                    Dim strNamaKV4 As String = ""
                    Dim strJamKredit4 As String = ""
                    Dim strGred4 As String = ""

                    strSQL = " SELECT COUNT(kpmkv_modul.KODMODUL) AS BILMODUL FROM kpmkv_modul LEFT OUTER JOIN "
                    strSQL += " kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID"
                    strSQL += " WHERE kpmkv_modul.Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND kpmkv_modul.Semester='3'"
                    strSQL += " AND kpmkv_modul.Sesi='" & chkSesi.Text & "'"
                    strSQL += " AND kpmkv_modul.KursusID ='" & ddlKodKursus.Text & "'"
                    Dim strBilModul3 As Integer = oCommon.getFieldValue(strSQL)

                    strSQL = " SELECT COUNT(kpmkv_modul.KODMODUL) AS BILMODUL FROM kpmkv_modul LEFT OUTER JOIN "
                    strSQL += " kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID"
                    strSQL += " WHERE kpmkv_modul.Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND kpmkv_modul.Semester='4'"
                    strSQL += " AND kpmkv_modul.Sesi='" & chkSesi.Text & "'"
                    strSQL += " AND kpmkv_modul.KursusID ='" & ddlKodKursus.Text & "'"
                    Dim strBilModul4 As Integer = oCommon.getFieldValue(strSQL)

                    For M3 As Integer = 1 To strBilModul3

                        strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='3'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        If oCommon.isExist(strSQL) = True Then
                            strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                            strSQL += " AND Semester='3'"
                            strSQL += " AND IsDeleted='N' AND StatusID='2'"
                            strRet = oCommon.getFieldValueEx(strSQL)

                            Dim ar_HPelajar As Array
                            ar_HPelajar = strRet.Split("|")
                            Dim strP_ID As String = ar_HPelajar(0)
                            Dim strP_Tahun As String = ar_HPelajar(1)
                            Dim strP_Sesi As String = ar_HPelajar(2)

                            strSQL = " SELECT kpmkv_modul.KodModul, kpmkv_modul.NamaModul, kpmkv_modul.JamKredit"
                            strSQL += " FROM  kpmkv_modul LEFT OUTER JOIN kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
                            strSQL += " kpmkv_pelajar ON kpmkv_kursus.KursusID = kpmkv_pelajar.KursusID"
                            strSQL += " WHERE kpmkv_modul.Tahun='" & strP_Tahun & "'"
                            strSQL += " AND kpmkv_modul.Semester='3'"
                            strSQL += " AND kpmkv_modul.Sesi='" & strP_Sesi & "'"
                            strSQL += " AND SUBSTRING(kpmkv_modul.KodModul,6,1)='" & M3 & "'"
                            strSQL += " AND kpmkv_modul.KursusID='" & ddlKodKursus.Text & "'"
                            strSQL += " AND kpmkv_pelajar.PelajarID='" & strP_ID & "'"
                            strSQL += " ORDER BY  kpmkv_modul.KodModul, kpmkv_modul.NamaModul, kpmkv_modul.JamKredit ASC"
                            strRet = oCommon.getFieldValueEx(strSQL)
                            Dim ar_VokM3 As Array
                            ar_VokM3 = strRet.Split("|")
                            strKodV3 = ar_VokM3(0)
                            strNamaKV3 = ar_VokM3(1)
                            strJamKredit3 = ar_VokM3(2)

                            strSQL = "SELECT GredV" & M3 & " FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "' AND Tahun='" & strP_Tahun & "'  AND Semester='3' AND Sesi='" & strP_Sesi & "'"
                            strGred3 = oCommon.getFieldValue(strSQL)
                        End If

                        ' ''Vokasional 1
                        ' '' ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        For M4 As Integer = 1 To strBilModul4
                            strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                            strSQL += " AND Semester='4'"
                            strSQL += " AND IsDeleted='N' AND StatusID='2'"
                            If oCommon.isExist(strSQL) = True Then
                                strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                                strSQL += " AND Semester='4'"
                                strSQL += " AND IsDeleted='N' AND StatusID='2'"
                                strRet = oCommon.getFieldValueEx(strSQL)

                                Dim ar_HPelajar As Array
                                ar_HPelajar = strRet.Split("|")
                                Dim strP_ID As String = ar_HPelajar(0)
                                Dim strP_Tahun As String = ar_HPelajar(1)
                                Dim strP_Sesi As String = ar_HPelajar(2)

                                strSQL = " SELECT kpmkv_modul.KodModul, kpmkv_modul.NamaModul, kpmkv_modul.JamKredit"
                                strSQL += " FROM  kpmkv_modul LEFT OUTER JOIN kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
                                strSQL += " kpmkv_pelajar ON kpmkv_kursus.KursusID = kpmkv_pelajar.KursusID"
                                strSQL += " WHERE kpmkv_modul.Tahun='" & strP_Tahun & "'"
                                strSQL += " AND kpmkv_modul.Semester='4'"
                                strSQL += " AND kpmkv_modul.Sesi='" & strP_Sesi & "'"
                                strSQL += " AND SUBSTRING(kpmkv_modul.KodModul,6,1)='" & M4 & "'"
                                strSQL += " AND kpmkv_modul.KursusID='" & ddlKodKursus.Text & "'"
                                strSQL += " AND kpmkv_pelajar.PelajarID='" & strP_ID & "'"
                                strSQL += " ORDER BY  kpmkv_modul.KodModul, kpmkv_modul.NamaModul, kpmkv_modul.JamKredit ASC"
                                strRet = oCommon.getFieldValueEx(strSQL)
                                Dim ar_VokM4 As Array
                                ar_VokM4 = strRet.Split("|")
                                strKodV4 = ar_VokM4(0)
                                strNamaKV4 = ar_VokM4(1)
                                strJamKredit4 = ar_VokM4(2)

                                strSQL = "SELECT GredV" & M4 & " FROM kpmkv_pelajar_markah WHERE PelajarID='" & strP_ID & "' AND Tahun='" & strP_Tahun & "'  AND Semester='4' AND Sesi='" & strP_Sesi & "'"
                                strGred4 = oCommon.getFieldValue(strSQL)
                            End If

                        Next 'm2

                        Dim myTableV4 As New PdfPTable(10)
                        myTableV4.WidthPercentage = 100 ' Table size is set to 100% of the page
                        myTableV4.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                        myTableV4.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                        Dim intTblCellV4() As Integer = {15, 40, 10, 10, 10, 10, 15, 40, 10, 10}
                        myTableV4.SetWidths(intTblCellV4)

                        myTableV4.AddCell(New Phrase(strKodV3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableV4.AddCell(New Phrase(strNamaKV3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableV4.AddCell(New Phrase(strJamKredit3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableV4.AddCell(New Phrase(strGred3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableV4.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableV4.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableV4.AddCell(New Phrase(strKodV4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableV4.AddCell(New Phrase(strNamaKV4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableV4.AddCell(New Phrase(strJamKredit4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableV4.AddCell(New Phrase(strGred4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myDocument.Add(myTableV4)

                    Next 'm1

                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)


                    'END ''''''''
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Dim myslip As New Paragraph("Slip ini adalah cetakan komputer,tandatangan tidak diperlukan", FontFactory.GetFont("Arial", 8, Font.ITALIC))
                    myslip.Alignment = Element.ALIGN_CENTER
                    myDocument.Add(myslip)
                    myDocument.NewPage()
                End If
            Next



            lblMsg.Text = "PDF siap dijana."
            hyPDF.Visible = True
            hyPDF.Text = "Klik disini untuk buka."
        Catch ex As DocumentException
            '--display on screen
            lblMsg.Text = "System Error. Contact system admin. " & ex.Message

        Catch ioe As IOException
            '--display on screen
            lblMsg.Text = "System Error. Contact system admin. " & ioe.Message.ToString

        Finally
            'Step 5: Remember to close the documnet
            myDocument.Close()

        End Try

    End Sub
    Protected Sub chkSesi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkSesi.SelectedIndexChanged
        kpmkv_kodkursus_list()
        kpmkv_kelas_list()
    End Sub

    Private Sub ddlKodKursus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlKodKursus.SelectedIndexChanged
        kpmkv_kelas_list()
    End Sub

End Class