Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Public Class pentaksiran_roster
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

                kpmkv_semester_list()

                ' strRet = BindData(datRespondent)
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
    Private Sub kpmkv_semester_list()
        strSQL = "SELECT Semester FROM kpmkv_semester ORDER BY SemesterID"
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
        tmpSQL = "SELECT kpmkv_pelajar.PelajarID, kpmkv_pelajar.Tahun, kpmkv_pelajar.Semester, kpmkv_pelajar.Sesi, kpmkv_pelajar.Nama, kpmkv_pelajar.MYKAD, kpmkv_pelajar.AngkaGiliran, "
        tmpSQL += " kpmkv_kluster.NamaKluster, kpmkv_kursus.NamaKursus, kpmkv_pelajar.Kaum, kpmkv_pelajar.Jantina, kpmkv_pelajar.Agama, kpmkv_status.Status, kpmkv_kelas.NamaKelas"
        tmpSQL += " FROM  kpmkv_pelajar LEFT OUTER JOIN kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN kpmkv_kluster ON kpmkv_kursus.KlusterID=kpmkv_kluster.KlusterID"
        tmpSQL += " LEFT OUTER JOIN kpmkv_status ON kpmkv_pelajar.StatusID = kpmkv_status.StatusID LEFT OUTER JOIN kpmkv_kelas ON kpmkv_pelajar.KelasID = kpmkv_kelas.KelasID"
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
            myDataAdapter.Fill(myDataSet, "Slip Roster")
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
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=SlipRoster.pdf")
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)

            PdfWriter.GetInstance(myDocument, HttpContext.Current.Response.OutputStream)

            'Step 3: Open the document now using
            myDocument.Open()

            ' ''--drawline
            Dim imgdrawLine As String = Server.MapPath("~/img/drawline.png")
            Dim imgLine As Image = Image.GetInstance(imgdrawLine)
            imgLine.Alignment = Image.LEFT_ALIGN  'left
            imgLine.Border = 0

            ''--draw spacing
            Dim imgdrawSpacing As String = Server.MapPath("~/img/spacing.png")
            Dim imgSpacing As Image = Image.GetInstance(imgdrawSpacing)
            imgSpacing.Alignment = Image.LEFT_ALIGN  'left
            imgSpacing.Border = 0

            ''--page header
            'Dim imageHeader As String = Server.MapPath("~/img/cert_header-new.png")
            'Dim imgHeader As Image = Image.GetInstance(imageHeader)
            'imgHeader.Alignment = Image.ALIGN_CENTER  'left
            'imgHeader.Border = 1

            Dim strNama As String = ""
            Dim strMykad As String = ""
            Dim strAngkaGiliran As String = ""
            Dim strJantina As String = ""
            Dim a As Integer = 0
            Dim strKey As String
            
                    '1'--start here
                    strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
                    Dim strKolejnama As String = oCommon.getFieldValue(strSQL)

                    'kolejnegeri
                    strSQL = "SELECT Negeri FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                    Dim strKolejnegeri As String = oCommon.getFieldValue(strSQL)
            'looping
            For i As Integer = 0 To datRespondent.Rows.Count - 1
                Dim row As GridViewRow = datRespondent.Rows(0)
                Dim cb As CheckBox = datRespondent.Rows(i).FindControl("chkSelect")

                strKey = datRespondent.DataKeys(i).Value.ToString

                If cb.Checked = True Then
                    If a = 0 Then
                        Dim myPara001 As New Paragraph("LEMBAGA PEPERIKSAAN", FontFactory.GetFont("Arial", 8, Font.BOLD))
                        myPara001.Alignment = Element.ALIGN_CENTER
                        myDocument.Add(myPara001)

                        Dim myPara01 As New Paragraph("KEMENTERIAN PENDIDIKAN MALAYSIA", FontFactory.GetFont("Arial", 8, Font.BOLD))
                        myPara01.Alignment = Element.ALIGN_CENTER
                        myDocument.Add(myPara01)

                        myDocument.Add(imgSpacing)
                        Dim myPara02 As New Paragraph("KEPUTUSAN PENTAKSIRAN KOLEJ VOKASIONAL", FontFactory.GetFont("Arial", 8, Font.NORMAL))
                        myPara02.Alignment = Element.ALIGN_CENTER
                        myDocument.Add(myPara02)

                        Dim myPara03 As New Paragraph("SEMESTER  " & ddlSemester.Text & "  KOHORT  " & ddlTahun.Text, FontFactory.GetFont("Arial", 8, Font.NORMAL))
                        myPara03.Alignment = Element.ALIGN_CENTER
                        myDocument.Add(myPara03)

                        Dim myPara04 As New Paragraph("INSTITUSI: " & strKolejnama, FontFactory.GetFont("Arial", 8, Font.NORMAL))
                        myPara04.Alignment = Element.ALIGN_LEFT
                        myDocument.Add(myPara04)
                        myDocument.Add(imgSpacing)
                    End If

                    a = a + 1

                    Dim strStudentID As String = ""
                    ''--Tokenid,ClassCode,Q001Remarks
                    strSQL = "SELECT Nama FROM kpmkv_pelajar WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strNama = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT Mykad FROM kpmkv_pelajar WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strMykad = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT AngkaGiliran FROM kpmkv_pelajar WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strAngkaGiliran = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT Jantina FROM kpmkv_pelajar WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strJantina = oCommon.getFieldValue(strSQL)

                    ''
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

                    Dim strKodBM As String = ""
                    Dim strKodBI As String = ""
                    Dim strKodMT As String = ""
                    Dim strKodSC As String = ""
                    Dim strKodSJ As String = ""
                    Dim strKodPI As String = ""
                    Dim strKodPM As String = ""

                    Dim strGredBM As String = ""
                    Dim strGredBI As String = ""
                    Dim strGredMT As String = ""
                    Dim strGredSC As String = ""
                    Dim strGredSJ As String = ""
                    Dim strGredPI As String = ""
                    Dim strGredPM As String = ""

                    Dim strGred As String = ""
                    Dim strKodMP As String = ""
                    Dim strBilJM As Integer = 0

                    strSQL = " SELECT COUNT(kpmkv_modul.KODMODUL) AS BILMODUL FROM kpmkv_modul LEFT OUTER JOIN "
                    strSQL += " kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID"
                    strSQL += " WHERE kpmkv_modul.Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND kpmkv_modul.Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND kpmkv_modul.Sesi='" & chkSesi.Text & "'"
                    strSQL += " AND kpmkv_modul.KursusID ='" & ddlKodKursus.Text & "'"
                    Dim strBilModul As Integer = oCommon.getFieldValue(strSQL)

                    'getvalue
                    strSQL = "SELECT PNGBM,PNGKBM,PNGA,PNGKA,PNGV,PNGKV,PNGK,PNGKK FROM kpmkv_pelajar_markah"
                    strSQL += " WHERE Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    strSQL += " AND PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    ' ''--get PNG info
                    Dim ar_HPNG As Array
                    ar_HPNG = strRet.Split("|")
                    strJum_NilaiMata_Akademik_BM = FormatNumber(CDbl(ar_HPNG(0)), 2)
                    strTotalNilaiMataPNGKBM = FormatNumber(CDbl(ar_HPNG(1)), 2)
                    strTotalNilaiMataPNGA = FormatNumber(CDbl(ar_HPNG(2)), 2)
                    strTotalNilaiMataPNGKA = FormatNumber(CDbl(ar_HPNG(3)), 2)
                    strTotalNilaiMataPNGV = FormatNumber(CDbl(ar_HPNG(4)), 2)
                    strTotalNilaiMataPNGKV = FormatNumber(CDbl(ar_HPNG(5)), 2)
                    strTotalNilaiMataPNGK = FormatNumber(CDbl(ar_HPNG(6)), 2)
                    strTotalNilaiMataPNGKK = FormatNumber(CDbl(ar_HPNG(7)), 2)
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'header1
                    Dim myTable As New PdfPTable(12)
                    myTable.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTable.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    'myTable.HorizontalAlignment = Rectangle.NO_BORDER
                    Dim intTblWidth() As Integer = {5, 12, 7, 30, 0, 9, 0, 9, 0, 9, 0, 9}
                    myTable.SetWidths(intTblWidth)

                    Dim Cell1Hdr As New PdfPCell(New Phrase("AG", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell1Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell1Hdr)

                    Dim CellHdr1 As New PdfPCell(New Phrase(":" & strAngkaGiliran, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    CellHdr1.Border = Rectangle.NO_BORDER
                    myTable.AddCell(CellHdr1)

                    Dim Cell2Hdr As New PdfPCell(New Phrase("NAMA", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell2Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell2Hdr)

                    Dim CellHdr2 As New PdfPCell(New Phrase(":" & strNama, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    CellHdr2.Border = Rectangle.NO_BORDER
                    myTable.AddCell(CellHdr2)

                    Dim Cell3Hdr As New PdfPCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell3Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell3Hdr)

                    Dim CellHdr3 As New PdfPCell(New Phrase("PNGBM:" & strJum_NilaiMata_Akademik_BM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    CellHdr3.Border = Rectangle.NO_BORDER
                    myTable.AddCell(CellHdr3)

                    Dim Cell4Hdr As New PdfPCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell4Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell4Hdr)


                    Dim CellHdr4 As New PdfPCell(New Phrase("PNGA:" & strTotalNilaiMataPNGA, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    CellHdr4.Border = Rectangle.NO_BORDER
                    myTable.AddCell(CellHdr4)

                    Dim Cell5Hdr As New PdfPCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell5Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell5Hdr)

                    Dim CellHdr5 As New PdfPCell(New Phrase("PNGV:" & strTotalNilaiMataPNGV, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    CellHdr5.Border = Rectangle.NO_BORDER
                    myTable.AddCell(CellHdr5)

                    Dim Cell6Hdr As New PdfPCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell6Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell6Hdr)

                    Dim CellHdr6 As New PdfPCell(New Phrase("PNG:" & strTotalNilaiMataPNGK, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    CellHdr6.Border = Rectangle.NO_BORDER
                    myTable.AddCell(CellHdr6)
                    myDocument.Add(myTable)

                    strBilJM = strBilModul + 6

                    Dim myTablerow2 As New PdfPTable(14)
                    myTablerow2.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTablerow2.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    'myTable.HorizontalAlignment = Rectangle.NO_BORDER
                    Dim intTblWidth2() As Integer = {5, 12, 7, 10, 0, 10, 8, 10, 0, 9, 0, 9, 0, 9}
                    myTablerow2.SetWidths(intTblWidth2)

                    Dim Cell1Hdr1 As New PdfPCell(New Phrase("NO KP", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell1Hdr1.Border = Rectangle.NO_BORDER
                    myTablerow2.AddCell(Cell1Hdr1)

                    Dim Cell1Hdr10 As New PdfPCell(New Phrase(":" & strMykad, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell1Hdr10.Border = Rectangle.NO_BORDER
                    myTablerow2.AddCell(Cell1Hdr10)

                    Dim Cell2Hdr2 As New PdfPCell(New Phrase("JANTINA", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell2Hdr2.Border = Rectangle.NO_BORDER
                    myTablerow2.AddCell(Cell2Hdr2)

                    Dim Cell2Hdr20 As New PdfPCell(New Phrase(":" & strJantina, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell2Hdr20.Border = Rectangle.NO_BORDER
                    myTablerow2.AddCell(Cell2Hdr20)

                    Dim Cell3Hdr3 As New PdfPCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell3Hdr3.Border = Rectangle.NO_BORDER
                    myTablerow2.AddCell(Cell3Hdr3)

                    Dim Cell3Hdr30 As New PdfPCell(New Phrase("JUM MP:" & strBilJM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell3Hdr30.Border = Rectangle.NO_BORDER
                    myTablerow2.AddCell(Cell3Hdr30)

                    Dim Cell4Hdr4 As New PdfPCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell4Hdr4.Border = Rectangle.NO_BORDER
                    myTablerow2.AddCell(Cell4Hdr4)

                    Dim Cell4Hdr40 As New PdfPCell(New Phrase("PNGKBM:" & strTotalNilaiMataPNGKBM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell4Hdr40.Border = Rectangle.NO_BORDER
                    myTablerow2.AddCell(Cell4Hdr40)

                    Dim Cell5Hdr5 As New PdfPCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell5Hdr5.Border = Rectangle.NO_BORDER
                    myTablerow2.AddCell(Cell5Hdr5)

                    Dim Cell5Hdr50 As New PdfPCell(New Phrase("PNGKA:" & strTotalNilaiMataPNGKA, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell5Hdr50.Border = Rectangle.NO_BORDER
                    myTablerow2.AddCell(Cell5Hdr50)

                    Dim Cell6Hdr6 As New PdfPCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell6Hdr6.Border = Rectangle.NO_BORDER
                    myTablerow2.AddCell(Cell6Hdr6)

                    Dim Cell6Hdr60 As New PdfPCell(New Phrase("PNGKV:" & strTotalNilaiMataPNGKV, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell6Hdr60.Border = Rectangle.NO_BORDER
                    myTablerow2.AddCell(Cell6Hdr60)

                    Dim Cell6Hdr7 As New PdfPCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell6Hdr7.Border = Rectangle.NO_BORDER
                    myTablerow2.AddCell(Cell6Hdr7)

                    Dim Cell6Hdr70 As New PdfPCell(New Phrase("PNGK:" & strTotalNilaiMataPNGKK, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell6Hdr70.Border = Rectangle.NO_BORDER
                    myTablerow2.AddCell(Cell6Hdr70)
                    myDocument.Add(myTablerow2)
                    'myDocument.Add(imgSpacing)

                    'print gred
                    strSQL = "SELECT KODMatapelajaran FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='" & ddlSemester.Text & "' AND NamaMataPelajaran='BAHASA MELAYU'"
                    strKodBM = oCommon.getFieldValue(strSQL)

                    'markah
                    strSQL = "SELECT  GredBM FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    strGredBM = oCommon.getFieldValue(strSQL)

                    'BI
                    strSQL = "SELECT  KODMatapelajaran FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='" & ddlSemester.Text & "' AND NamaMataPelajaran='BAHASA INGGERIS'"
                    strKodBI = oCommon.getFieldValue(strSQL)

                    'markah
                    strSQL = "SELECT  GredBI FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    strGredBI = oCommon.getFieldValue(strSQL)

                    'MATHEMATIC
                    'change on 05082016
                    strSQL = "SELECT  JenisKursus From kpmkv_kursus Where KursusID='" & ddlKodKursus.SelectedValue & "'"
                    Dim strJenisKursusMT As String = oCommon.getFieldValue(strSQL)

                    If ddlSemester.Text = "3" Then
                        If strJenisKursusMT = "SOCIAL" Then
                            strKodMT = "AMT3101"
                        Else
                            strKodMT = "AMT3091"
                        End If
                    ElseIf ddlSemester.Text = "4" Then
                        If strJenisKursusMT = "SOCIAL" Then
                            strKodMT = "AMT4101"
                        Else
                            strKodMT = "AMT4091"
                        End If
                    Else
                        strSQL = "SELECT  KODMatapelajaran FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='" & ddlSemester.Text & "' AND NamaMataPelajaran='MATHEMATIC'"
                        strKodMT = oCommon.getFieldValue(strSQL)
                    End If
                    'markah
                    strSQL = "SELECT  GredMT FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    strGredMT = oCommon.getFieldValue(strSQL)

                    'SCIENCE
                    'change on 05082016
                    strSQL = "SELECT  JenisKursus From kpmkv_kursus Where KursusID='" & ddlKodKursus.SelectedValue & "'"
                    Dim strJenisKursusSC As String = oCommon.getFieldValue(strSQL)

                    If ddlSemester.Text = "3" Then
                        If strJenisKursusSC = "SOCIAL" Then
                            strKodSC = "AMT3131"
                        Else
                            strKodSC = "AMT3121"
                        End If
                    ElseIf ddlSemester.Text = "4" Then
                        If strJenisKursusSC = "SOCIAL" Then
                            strKodSC = "AMT4131"
                        Else
                            strKodSC = "AMT4121"
                        End If
                    Else
                        strSQL = "SELECT  KODMatapelajaran FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='" & ddlSemester.Text & "' AND NamaMataPelajaran='SCIENCE'"
                        strKodSC = oCommon.getFieldValue(strSQL)
                    End If
                    'markah
                    strSQL = "SELECT  GredSC FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    strGredSC = oCommon.getFieldValue(strSQL)

                    'SEJARAH
                    strSQL = "SELECT  KODMatapelajaran FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='" & ddlSemester.Text & "' AND NamaMataPelajaran='SEJARAH'"
                    strKodSJ = oCommon.getFieldValue(strSQL)

                    'markah
                    strSQL = "SELECT  GredSJ FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    strGredSJ = oCommon.getFieldValue(strSQL)

                    'PENDIDIKAN ISLAM
                    strSQL = "SELECT  KODMatapelajaran FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='" & ddlSemester.Text & "' AND NamaMataPelajaran='PENDIDIKAN ISLAM'"
                    strKodPI = oCommon.getFieldValue(strSQL)

                    'markah
                    strSQL = "SELECT  GredPI FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    strGredPI = oCommon.getFieldValue(strSQL)

                    'PENDIDIKAN MORAL
                    strSQL = "SELECT  KODMatapelajaran FROM kpmkv_matapelajaran WHERE SUBSTRING(KodMataPelajaran,4,1)='" & ddlSemester.Text & "' AND NamaMataPelajaran='PENDIDIKAN MORAL'"
                    strKodPM = oCommon.getFieldValue(strSQL)

                    'markah
                    strSQL = "SELECT  GredPM FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    strGredPM = oCommon.getFieldValue(strSQL)

                    'Modul1
                    strSQL = " SELECT kpmkv_modul.KodModul"
                    strSQL += " FROM  kpmkv_modul LEFT OUTER JOIN kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
                    strSQL += " kpmkv_pelajar ON kpmkv_kursus.KursusID = kpmkv_pelajar.KursusID"
                    strSQL += " WHERE kpmkv_modul.Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND kpmkv_modul.Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND kpmkv_modul.Sesi='" & chkSesi.Text & "'"
                    strSQL += " AND SUBSTRING(kpmkv_modul.KodModul,6,1)=1 "
                    strSQL += " AND kpmkv_modul.KursusID='" & ddlKodKursus.Text & "'"
                    strSQL += " AND kpmkv_pelajar.PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " ORDER BY  kpmkv_modul.KodModul, kpmkv_modul.NamaModul, kpmkv_modul.JamKredit ASC"
                    Dim strKodMP1 As String = oCommon.getFieldValue(strSQL)


                    'Modul2
                    strSQL = " SELECT kpmkv_modul.KodModul"
                    strSQL += " FROM  kpmkv_modul LEFT OUTER JOIN kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
                    strSQL += " kpmkv_pelajar ON kpmkv_kursus.KursusID = kpmkv_pelajar.KursusID"
                    strSQL += " WHERE kpmkv_modul.Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND kpmkv_modul.Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND kpmkv_modul.Sesi='" & chkSesi.Text & "'"
                    strSQL += " AND SUBSTRING(kpmkv_modul.KodModul,6,1)=2 "
                    strSQL += " AND kpmkv_modul.KursusID='" & ddlKodKursus.Text & "'"
                    strSQL += " AND kpmkv_pelajar.PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " ORDER BY  kpmkv_modul.KodModul, kpmkv_modul.NamaModul, kpmkv_modul.JamKredit ASC"
                    Dim strKodMP2 As String = oCommon.getFieldValue(strSQL)

                    'Modul3
                    strSQL = " SELECT kpmkv_modul.KodModul"
                    strSQL += " FROM  kpmkv_modul LEFT OUTER JOIN kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
                    strSQL += " kpmkv_pelajar ON kpmkv_kursus.KursusID = kpmkv_pelajar.KursusID"
                    strSQL += " WHERE kpmkv_modul.Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND kpmkv_modul.Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND kpmkv_modul.Sesi='" & chkSesi.Text & "'"
                    strSQL += " AND SUBSTRING(kpmkv_modul.KodModul,6,1)=3 "
                    strSQL += " AND kpmkv_modul.KursusID='" & ddlKodKursus.Text & "'"
                    strSQL += " AND kpmkv_pelajar.PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " ORDER BY  kpmkv_modul.KodModul, kpmkv_modul.NamaModul, kpmkv_modul.JamKredit ASC"
                    Dim strKodMP3 As String = oCommon.getFieldValue(strSQL)


                    'Modul4
                    strSQL = " SELECT kpmkv_modul.KodModul"
                    strSQL += " FROM  kpmkv_modul LEFT OUTER JOIN kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
                    strSQL += " kpmkv_pelajar ON kpmkv_kursus.KursusID = kpmkv_pelajar.KursusID"
                    strSQL += " WHERE kpmkv_modul.Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND kpmkv_modul.Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND kpmkv_modul.Sesi='" & chkSesi.Text & "'"
                    strSQL += " AND SUBSTRING(kpmkv_modul.KodModul,6,1)=4 "
                    strSQL += " AND kpmkv_modul.KursusID='" & ddlKodKursus.Text & "'"
                    strSQL += " AND kpmkv_pelajar.PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " ORDER BY  kpmkv_modul.KodModul, kpmkv_modul.NamaModul, kpmkv_modul.JamKredit ASC"
                    Dim strKodMP4 As String = oCommon.getFieldValue(strSQL)

                    'Modul5
                    strSQL = " SELECT kpmkv_modul.KodModul"
                    strSQL += " FROM  kpmkv_modul LEFT OUTER JOIN kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
                    strSQL += " kpmkv_pelajar ON kpmkv_kursus.KursusID = kpmkv_pelajar.KursusID"
                    strSQL += " WHERE kpmkv_modul.Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND kpmkv_modul.Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND kpmkv_modul.Sesi='" & chkSesi.Text & "'"
                    strSQL += " AND SUBSTRING(kpmkv_modul.KodModul,6,1)=5 "
                    strSQL += " AND kpmkv_modul.KursusID='" & ddlKodKursus.Text & "'"
                    strSQL += " AND kpmkv_pelajar.PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " ORDER BY  kpmkv_modul.KodModul, kpmkv_modul.NamaModul, kpmkv_modul.JamKredit ASC"
                    Dim strKodMP5 As String = oCommon.getFieldValue(strSQL)

                    'Modul6
                    strSQL = " SELECT kpmkv_modul.KodModul"
                    strSQL += " FROM  kpmkv_modul LEFT OUTER JOIN kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
                    strSQL += " kpmkv_pelajar ON kpmkv_kursus.KursusID = kpmkv_pelajar.KursusID"
                    strSQL += " WHERE kpmkv_modul.Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND kpmkv_modul.Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND kpmkv_modul.Sesi='" & chkSesi.Text & "'"
                    strSQL += " AND SUBSTRING(kpmkv_modul.KodModul,6,1)=6 "
                    strSQL += " AND kpmkv_modul.KursusID='" & ddlKodKursus.Text & "'"
                    strSQL += " AND kpmkv_pelajar.PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " ORDER BY  kpmkv_modul.KodModul, kpmkv_modul.NamaModul, kpmkv_modul.JamKredit ASC"
                    Dim strKodMP6 As String = oCommon.getFieldValue(strSQL)

                    ''Print Row 1
                    ' ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Dim myTableMP8 As New PdfPTable(13)
                    myTableMP8.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTableMP8.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    myTableMP8.DefaultCell.BorderWidth = 1
                    myTableMP8.DefaultCell.Border = Rectangle.BOX
                    Dim intTblCell8() As Integer = {10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10}
                    myTableMP8.SetWidths(intTblCell8)

                    myTableMP8.AddCell(New Phrase(strKodBM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP8.AddCell(New Phrase(strKodBI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP8.AddCell(New Phrase(strKodMT, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP8.AddCell(New Phrase(strKodSC, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP8.AddCell(New Phrase(strKodSJ, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP8.AddCell(New Phrase(strKodPI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP8.AddCell(New Phrase(strKodPM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP8.AddCell(New Phrase(strKodMP1, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP8.AddCell(New Phrase(strKodMP2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP8.AddCell(New Phrase(strKodMP3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP8.AddCell(New Phrase(strKodMP4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP8.AddCell(New Phrase(strKodMP5, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP8.AddCell(New Phrase(strKodMP6, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(myTableMP8)


                    strSQL = "SELECT GredV1 FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                    Dim strGred1 As String = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT GredV2 FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                    Dim strGred2 As String = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT GredV3 FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                    Dim strGred3 As String = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT GredV4 FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                    Dim strGred4 As String = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT GredV5 FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                    Dim strGred5 As String = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT GredV6 FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                    Dim strGred6 As String = oCommon.getFieldValue(strSQL)


                    Dim myTableMP9 As New PdfPTable(13)
                    myTableMP9.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTableMP9.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    Dim intTblCell9() As Integer = {10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10}
                    myTableMP9.DefaultCell.BorderWidth = 1
                    myTableMP9.DefaultCell.Border = Rectangle.BOX
                    myTableMP9.SetWidths(intTblCell9)

                    myTableMP9.AddCell(New Phrase(strGredBM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP9.AddCell(New Phrase(strGredBI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP9.AddCell(New Phrase(strGredMT, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP9.AddCell(New Phrase(strGredSC, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP9.AddCell(New Phrase(strGredSJ, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP9.AddCell(New Phrase(strGredPI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP9.AddCell(New Phrase(strGredPM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP9.AddCell(New Phrase(strGred1, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP9.AddCell(New Phrase(strGred2, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP9.AddCell(New Phrase(strGred3, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP9.AddCell(New Phrase(strGred4, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP9.AddCell(New Phrase(strGred5, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP9.AddCell(New Phrase(strGred6, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(myTableMP9)
                    myDocument.Add(imgSpacing)

                End If 'is check box

                If a = 8 Then
                    Dim myslip As New Paragraph("Slip ini adalah cetakan komputer,tandatangan tidak diperlukan", FontFactory.GetFont("Arial", 8, Font.ITALIC))
                    myslip.Alignment = Element.ALIGN_CENTER
                    myDocument.Add(myslip)
                    myDocument.NewPage()
                    a = 0
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
            'pdfDoc.Close()

            HttpContext.Current.Response.Write(myDocument)
            HttpContext.Current.Response.End()

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