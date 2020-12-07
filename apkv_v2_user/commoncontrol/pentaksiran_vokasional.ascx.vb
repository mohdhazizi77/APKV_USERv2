Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Imports RKLib.ExportData

Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class pentaksiran_vokasional
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""
    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)
    Private isChecked As List(Of Boolean) = Nothing


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

                kpmkv_semester_list()

                kpmkv_kodkursus_list()

                kpmkv_kelas_list()

                kpmkv_tahun_1_list()
                ddlTahun_1.Text = Now.Year

                kpmkv_tahun_2_list()
                ddlTahun_Semasa.Text = Now.Year

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
    Private Sub kpmkv_tahun_1_list()
        strSQL = "SELECT Tahun FROM kpmkv_tahun ORDER BY TahunID"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlTahun_1.DataSource = ds
            ddlTahun_1.DataTextField = "Tahun"
            ddlTahun_1.DataValueField = "Tahun"
            ddlTahun_1.DataBind()

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

    Private Sub datRespondent_PageIndexChanged(sender As Object, e As EventArgs) Handles datRespondent.PageIndexChanged
        strRet = BindData(datRespondent)
        datRespondent.SelectedIndex = -1
    End Sub

    Private Sub datRespondent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondent.PageIndexChanging
        ' datRespondent.AllowPaging = False
        datRespondent.PageIndex = e.NewPageIndex

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

        lblMsg.Text = ""
        Try

            Dim tableColumn As DataColumnCollection
            Dim tableRows As DataRowCollection

            Dim myDataSet As New DataSet
            Dim myDataAdapter As New SqlDataAdapter(getSQL, strConn)
            myDataAdapter.Fill(myDataSet, "Pentaksiran Vokasional")
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
    '--CHECK form validation.
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
            Dim imageHeader As String = Server.MapPath("~/img/cert_header-new.png")
            Dim imgHeader As Image = Image.GetInstance(imageHeader)
            imgHeader.Alignment = Image.ALIGN_CENTER  'left
            imgHeader.Border = 1

            ''--loop thru records here

            Dim strNama As String = ""
            Dim strMykad As String = ""
            Dim strAgama As String = ""
            Dim strAngkaGiliran As String = ""
            Dim strKodKursus As String = ""
            Dim strKluster As String = ""
            Dim strKey As String


            'Get the data from database into datatable 
            ' Dim cmd As New SqlCommand(getSQL)
            'Dim dt As DataTable = GetData(cmd)

            'append new line 
            For i As Integer = 0 To datRespondent.Rows.Count - 1
                Dim row As GridViewRow = datRespondent.Rows(0)
                Dim strKohortTahun As String = ""
                Dim strKohortCount As Integer = 0
                Dim strTahunSemester As Integer = 0

                Dim cb As CheckBox = datRespondent.Rows(i).FindControl("chkSelect")

                If cb.Checked = True Then
                    strKey = datRespondent.DataKeys(i).Value.ToString

                    ''--Tokenid,ClassCode,Q001Remarks
                    strSQL = "SELECT Nama FROM kpmkv_pelajar WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strNama = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT Mykad FROM kpmkv_pelajar WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strMykad = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT Agama FROM kpmkv_pelajar WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strAgama = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT AngkaGiliran FROM kpmkv_pelajar WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strAngkaGiliran = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT NamaKursus FROM kpmkv_kursus WHERE KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strKodKursus = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT KlusterID FROM kpmkv_kursus WHERE KursusID = '" & ddlKodKursus.SelectedValue & "'"
                    Dim strKlusterID As String = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT NamaKluster FROM kpmkv_kluster WHERE KlusterID ='" & strKlusterID & "'"
                    strKluster = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT Tahun FROM kpmkv_pelajar WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strKohortTahun = oCommon.getFieldValue(strSQL)

                    strKohortCount = CInt(Now.Year) - CInt(strKohortTahun)
                    Select Case ddlSemester.Text
                        Case "1"
                            strTahunSemester = CInt(strKohortTahun) ' 2014
                        Case "2"
                            strTahunSemester = CInt(strKohortTahun) ' 2014
                        Case "3"
                            strTahunSemester = strKohortCount + CInt(strKohortTahun) '2015
                        Case "4"
                            strTahunSemester = strKohortCount + CInt(strKohortTahun) '2015
                    End Select


                    strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
                    Dim strKolejnama As String = oCommon.getFieldValue(strSQL)

                    'kolejnegeri
                    strSQL = "SELECT Negeri FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                    Dim strKolejnegeri As String = oCommon.getFieldValue(strSQL)

                    'set RPN
                    Dim strRPN As String = i + 1
                    ''--start here
                    Dim myRef As New Paragraph("(RPN.000" & strRPN & ")", FontFactory.GetFont("Arial", 8, Font.NORMAL))
                    myRef.Alignment = Element.ALIGN_LEFT
                    myDocument.Add(myRef)

                    Dim myPara001 As New Paragraph("" & strKolejnama, FontFactory.GetFont("Arial", 8, Font.BOLD))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    myDocument.Add(myPara001)

                    Dim myPara01 As New Paragraph("KEMENTERIAN PENDIDIKAN MALAYSIA", FontFactory.GetFont("Arial", 8, Font.BOLD))
                    myPara01.Alignment = Element.ALIGN_CENTER
                    myDocument.Add(myPara01)

                    myDocument.Add(imgSpacing)
                    Dim myPara02 As New Paragraph("KEPUTUSAN PENTAKSIRAN KOLEJ VOKASIONAL", FontFactory.GetFont("Arial", 8, Font.NORMAL))
                    myPara02.Alignment = Element.ALIGN_CENTER
                    myDocument.Add(myPara02)

                    Dim myPara03 As New Paragraph("SEMESTER  " & ddlSemester.Text & "  TAHUN  " & ddlTahun_Semasa.Text & "  (KOHORT " & ddlTahun.Text & ")", FontFactory.GetFont("Arial", 8, Font.NORMAL))
                    myPara03.Alignment = Element.ALIGN_CENTER
                    myDocument.Add(myPara03)

                    myDocument.Add(imgSpacing)
                    ''

                    myDocument.Add(New Paragraph("NAMA                              : " & strNama, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(New Paragraph("NO.KAD PENGENALAN : " & strMykad, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(New Paragraph("ANGKAGILIRAN             : " & strAngkaGiliran, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(New Paragraph("INSTITUSI                      : " & strKolejnama & ", " & strKolejnegeri, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(New Paragraph("BIDANG                          : " & strKluster, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(New Paragraph("PROGRAM:                    : " & strKodKursus, FontFactory.GetFont("Arial", 8, Font.NORMAL)))

                    myDocument.Add(imgSpacing)
                    'modul n matapelajaran
                    Dim myTable As New PdfPTable(7)
                    myTable.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTable.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    'myTable.HorizontalAlignment = Rectangle.NO_BORDER
                    Dim intTblWidth() As Integer = {15, 50, 10, 10, 10, 10, 36}
                    myTable.SetWidths(intTblWidth)

                    Dim Cell1Hdr As New PdfPCell(New Phrase("KOD ", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell1Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell1Hdr)

                    Dim Cell2Hdr As New PdfPCell(New Phrase("MATAPELAJARAN / KURSUS", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell2Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell2Hdr)

                    Dim Cell3Hdr As New PdfPCell(New Phrase("JAM KREDIT", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell3Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell3Hdr)

                    Dim Cell4Hdr As New PdfPCell(New Phrase("GRED", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell4Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell4Hdr)

                    Dim Cell5Hdr As New PdfPCell(New Phrase("NILAI GRED", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell5Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell5Hdr)

                    Dim Cell6Hdr As New PdfPCell(New Phrase("NILAI MATA", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell6Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell6Hdr)

                    Dim Cell7Hdr As New PdfPCell(New Phrase("PENCAPAIAN / TAHAP KOMPETENSI", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    Cell7Hdr.Border = Rectangle.NO_BORDER
                    myTable.AddCell(Cell7Hdr)

                    myDocument.Add(myTable)

                    Dim strGred As String = ""
                    Dim strNilaiGred As Double = 0.0
                    Dim strNilaiGredBM As Double = 0.0
                    Dim strTtlNilaiGredBM As Double = 0.0
                    Dim strKodBM As String = ""
                    Dim strKodBI As String = ""
                    Dim strKodMT As String = ""
                    Dim strKodSC As String = ""
                    Dim strKodSJ As String = ""
                    Dim strKodPI As String = ""
                    Dim strKodPM As String = ""

                    Dim strNamaBM As String = ""
                    Dim strNamaBI As String = ""
                    Dim strNamaMT As String = ""
                    Dim strNamaSC As String = ""
                    Dim strNamaSJ As String = ""
                    Dim strNamaPI As String = ""
                    Dim strNamaPM As String = ""

                    Dim strJamKreditBM As String = ""
                    Dim strJamKreditBI As String = ""
                    Dim strJamKreditMT As String = ""
                    Dim strJamKreditSC As String = ""
                    Dim strJamKreditSJ As String = ""
                    Dim strJamKreditPI As String = ""
                    Dim strJamKreditPM As String = ""

                    Dim strNilaiMataBM As Double = 0.0
                    Dim strNilaiMataBI As Double = 0.0
                    Dim strNilaiMataMT As Double = 0.0
                    Dim strNilaiMataSC As Double = 0.0
                    Dim strNilaiMataSJ As Double = 0.0
                    Dim strNilaiMataPI As Double = 0.0
                    Dim strNilaiMataPM As Double = 0.0

                    Dim strKompentasiBM As String = ""
                    Dim strKompentasiBI As String = ""
                    Dim strKompentasiMT As String = ""
                    Dim strKompentasiSC As String = ""
                    Dim strKompentasiSJ As String = ""
                    Dim strKompentasiPI As String = ""
                    Dim strKompentasiPM As String = ""
                    Dim strKompentasi As String = ""
                    Dim strTotalNilaiMata As Double = 0.0
                    Dim strKodMP As String = ""
                    Dim strNamaMP As String = ""
                    'vokay
                    Dim strJamKredit As String = ""
                    Dim strNilaiMataV As Double = 0.0
                    Dim strJamKreditV As Double = 0.0

                    Dim namaMP_BM As String = ""
                    Dim namaMP_BI As String = ""
                    Dim namaMP_MT As String = ""
                    Dim namaMP_SC As String = ""
                    Dim namaMP_SJ As String = ""
                    Dim namaMP_PI As String = ""
                    Dim namaMP_PM As String = ""

                    strSQL = "SELECT  JenisKursus From kpmkv_kursus Where KursusID='" & ddlKodKursus.SelectedValue & "'"
                    Dim strJenisKursusMT As String = oCommon.getFieldValue(strSQL)

                    strSQL = "  SELECT
                                NamaMataPelajaran, PelajarMarkahGred
                                FROM 
                                kpmkv_matapelajaran
                                WHERE
                                Tahun = '" & ddlTahun.Text & "'
                                AND Semester = '" & ddlSemester.Text & "'
                                ORDER BY 
                                KodMataPelajaran"

                    strRet = oCommon.ExecuteSQL(strSQL)

                    Dim sqlDA As New SqlDataAdapter(strSQL, objConn)
                    Dim ds As DataSet = New DataSet
                    sqlDA.Fill(ds, "AnyTable")

                    For iloop As Integer = 0 To ds.Tables(0).Rows.Count - 1

                        If ds.Tables(0).Rows(iloop).Item(1).ToString() = "GredBM" Then

                            namaMP_BM = ds.Tables(0).Rows(iloop).Item(0).ToString()

                        ElseIf ds.Tables(0).Rows(iloop).Item(1).ToString() = "GredBI" Then

                            namaMP_BI = ds.Tables(0).Rows(iloop).Item(0).ToString()

                        ElseIf ds.Tables(0).Rows(iloop).Item(1).ToString() = "GredMT" Then

                            Select Case ddlSemester.Text
                                Case "1"
                                    namaMP_MT = ds.Tables(0).Rows(iloop).Item(0).ToString()
                                Case "2"
                                    namaMP_MT = ds.Tables(0).Rows(iloop).Item(0).ToString()
                                Case "3"
                                    If strJenisKursusMT = "SOCIAL" Then
                                        strSQL = "  SELECT NamaMataPelajaran FROM kpmkv_matapelajaran
                                                    WHERE
                                                    Tahun = '" & ddlTahun.Text & "'
                                                    AND Semester = '" & ddlSemester.Text & "'
                                                    AND PelajarMarkahGred = 'GredMT'
                                                    AND (Jenis ='SOCIAL' OR Jenis IS NULL OR Jenis ='')"
                                        namaMP_MT = oCommon.getFieldValue(strSQL)
                                    Else
                                        strSQL = "  SELECT NamaMataPelajaran FROM kpmkv_matapelajaran
                                                    WHERE
                                                    Tahun = '" & ddlTahun.Text & "'
                                                    AND Semester = '" & ddlSemester.Text & "'
                                                    AND PelajarMarkahGred = 'GredMT'
                                                    AND (Jenis = 'TEKNOLOGI' OR Jenis IS NULL OR Jenis ='')"
                                        namaMP_MT = oCommon.getFieldValue(strSQL)
                                    End If
                                Case "4"
                                    If strJenisKursusMT = "SOCIAL" Then
                                        strSQL = "  SELECT NamaMataPelajaran FROM kpmkv_matapelajaran
                                                    WHERE
                                                    Tahun = '" & ddlTahun.Text & "'
                                                    AND Semester = '" & ddlSemester.Text & "'
                                                    AND PelajarMarkahGred = 'GredMT'
                                                    AND (Jenis ='SOCIAL' OR Jenis IS NULL OR Jenis ='')"
                                        ''strNamaMT = oCommon.getFieldValue(strSQL)
                                        namaMP_MT = oCommon.getFieldValue(strSQL)
                                    Else
                                        strSQL = "  SELECT NamaMataPelajaran FROM kpmkv_matapelajaran
                                                    WHERE
                                                    Tahun = '" & ddlTahun.Text & "'
                                                    AND Semester = '" & ddlSemester.Text & "'
                                                    AND PelajarMarkahGred = 'GredMT'
                                                    AND (Jenis = 'TEKNOLOGI' OR Jenis IS NULL OR Jenis ='')"
                                        namaMP_MT = oCommon.getFieldValue(strSQL)
                                    End If
                            End Select

                        ElseIf ds.Tables(0).Rows(iloop).Item(1).ToString() = "GredSC" Then

                            namaMP_SC = ds.Tables(0).Rows(iloop).Item(0).ToString()

                        ElseIf ds.Tables(0).Rows(iloop).Item(1).ToString() = "GredSJ" Then

                            namaMP_SJ = ds.Tables(0).Rows(iloop).Item(0).ToString()

                        ElseIf ds.Tables(0).Rows(iloop).Item(1).ToString() = "GredPI" Then

                            namaMP_PI = ds.Tables(0).Rows(iloop).Item(0).ToString()

                        ElseIf ds.Tables(0).Rows(iloop).Item(1).ToString() = "GredPM" Then

                            namaMP_PM = ds.Tables(0).Rows(iloop).Item(0).ToString()

                        End If

                    Next

                    'BahasaMelayu
                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE Tahun = '" & ddlTahun.Text & "' AND Semester = '" & ddlSemester.Text & "' AND NamaMataPelajaran='" & namaMP_BM & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    'matapelajaran info
                    Dim ar_MP As Array
                    ar_MP = strRet.Split("|")
                    strKodBM = ar_MP(0)
                    strNamaBM = ar_MP(1)
                    strJamKreditBM = ar_MP(2)
                    'markah
                    strSQL = "SELECT  GredBM FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    Dim strGredBM As String = oCommon.getFieldValue(strSQL)
                    If Not strGredBM = "" Then
                        'gred
                        strSQL = "SELECT  Pointer, Status FROM kpmkv_gred WHERE Jenis='AKADEMIK' AND Gred='" & strGredBM & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        ' ''--get Gred akademik info
                        Dim ar_akademik As Array
                        ar_akademik = strRet.Split("|")

                        strNilaiGred = ar_akademik(0)
                        strNilaiGredBM = ar_akademik(0)
                        strNilaiMataBM = ar_akademik(0) * (strJamKreditBM) 'edit 0608
                        strTtlNilaiGredBM = ar_akademik(0) * (strJamKreditBM)
                        strKompentasiBM = ar_akademik(1)
                        'strJamKredit_Vokasional = ar_akademik(4)

                        Dim myTableMP1 As New PdfPTable(7)
                        myTableMP1.WidthPercentage = 100 ' Table size is set to 100% of the page
                        myTableMP1.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                        myTableMP1.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                        Dim intTblCell() As Integer = {15, 50, 10, 10, 10, 10, 36}
                        myTableMP1.SetWidths(intTblCell)

                        myTableMP1.AddCell(New Phrase(strKodBM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP1.AddCell(New Phrase(strNamaBM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP1.AddCell(New Phrase(strJamKreditBM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP1.AddCell(New Phrase(strGredBM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP1.AddCell(New Phrase(String.Format("{0:0.00}", strNilaiGred), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP1.AddCell(New Phrase(String.Format("{0:0.00}", strNilaiMataBM), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP1.AddCell(New Phrase(strKompentasiBM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myDocument.Add(myTableMP1)
                    End If
                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE Tahun = '" & ddlTahun.Text & "' AND Semester = '" & ddlSemester.Text & "' AND NamaMataPelajaran='" & namaMP_BI & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    ' ''--get Gred akademik info
                    Dim ar_MPBI As Array
                    ar_MPBI = strRet.Split("|")
                    strKodBI = ar_MPBI(0)
                    strNamaBI = ar_MPBI(1)
                    strJamKreditBI = ar_MPBI(2)
                    'markah
                    strSQL = "SELECT  GredBI FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    Dim strGredBI As String = oCommon.getFieldValue(strSQL)
                    If Not strGredBI = "" Then
                        'gred
                        strSQL = "SELECT  Pointer, Status FROM kpmkv_gred WHERE Jenis='AKADEMIK' AND Gred='" & strGredBI & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        ' ''--get Gred akademik info
                        Dim ar_akademikBI As Array
                        ar_akademikBI = strRet.Split("|")

                        strNilaiGred = ar_akademikBI(0)
                        strNilaiMataBI = ar_akademikBI(0) * CDbl(strJamKreditBI)
                        strKompentasiBI = ar_akademikBI(1)
                        'strJamKredit_Vokasional = ar_akademik(4)

                        Dim myTableMP2 As New PdfPTable(7)
                        myTableMP2.WidthPercentage = 100 ' Table size is set to 100% of the page
                        myTableMP2.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                        myTableMP2.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                        Dim intTblCell2() As Integer = {15, 50, 10, 10, 10, 10, 36}
                        myTableMP2.SetWidths(intTblCell2)

                        myTableMP2.AddCell(New Phrase(strKodBI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP2.AddCell(New Phrase(strNamaBI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP2.AddCell(New Phrase(strJamKreditBI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP2.AddCell(New Phrase(strGredBI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP2.AddCell(New Phrase(String.Format("{0:0.00}", strNilaiGred), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP2.AddCell(New Phrase(String.Format("{0:0.00}", strNilaiMataBI), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP2.AddCell(New Phrase(strKompentasiBI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myDocument.Add(myTableMP2)
                    End If

                    'MATHEMATIC
                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE Tahun = '" & ddlTahun.Text & "' AND Semester = '" & ddlSemester.Text & "' AND NamaMataPelajaran='" & namaMP_MT & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)

                    Dim ar_KODMT As Array
                    ar_KODMT = strRet.Split("|")
                    strKodMT = ar_KODMT(0)
                    strNamaMT = ar_KODMT(1)
                    strJamKreditMT = ar_KODMT(2)
                    'markah
                    strSQL = "SELECT  GredMT FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    Dim strGredMT As String = oCommon.getFieldValue(strSQL)
                    If Not strGredMT = "" Then
                        'gred
                        strSQL = "SELECT  Pointer, Status FROM kpmkv_gred WHERE Jenis='AKADEMIK' AND Gred='" & strGredMT & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        ' ''--get Gred akademik info
                        Dim ar_akademikMT As Array
                        ar_akademikMT = strRet.Split("|")

                        strNilaiGred = ar_akademikMT(0)
                        strNilaiMataMT = ar_akademikMT(0) * CDbl(strJamKreditMT)
                        strKompentasiMT = ar_akademikMT(1)
                        'strJamKredit_Vokasional = ar_akademik(4)

                        Select Case ddlSemester.Text
                            Case "1"
                                strNamaMT = strNamaMT
                            Case "2"
                                strNamaMT = strNamaMT
                            Case "3"
                                If strJenisKursusMT = "SOCIAL" Then
                                    strSQL = "  SELECT NamaMataPelajaran FROM kpmkv_matapelajaran
                                                WHERE
                                                Tahun = '" & ddlTahun.Text & "'
                                                AND Semester = '" & ddlSemester.Text & "'
                                                AND PelajarMarkahGred = 'GredMT'
                                                AND (Jenis ='SOCIAL' OR Jenis IS NULL OR Jenis ='')"
                                    strNamaMT = oCommon.getFieldValue(strSQL)

                                    strSQL = "  SELECT KodMataPelajaran FROM kpmkv_matapelajaran
                                                WHERE
                                                Tahun = '" & ddlTahun.Text & "'
                                                AND Semester = '" & ddlSemester.Text & "'
                                                AND PelajarMarkahGred = 'GredMT'
                                                AND (Jenis ='SOCIAL' OR Jenis IS NULL OR Jenis ='')"
                                    strKodMT = oCommon.getFieldValue(strSQL)

                                Else
                                    strSQL = "  SELECT NamaMataPelajaran FROM kpmkv_matapelajaran
                                                WHERE
                                                Tahun = '" & ddlTahun.Text & "'
                                                AND Semester = '" & ddlSemester.Text & "'
                                                AND PelajarMarkahGred = 'GredMT'
                                                AND (Jenis = 'TEKNOLOGI' OR Jenis IS NULL OR Jenis ='')"
                                    strNamaMT = oCommon.getFieldValue(strSQL)

                                    strSQL = "  SELECT KodMataPelajaran FROM kpmkv_matapelajaran
                                                WHERE
                                                Tahun = '" & ddlTahun.Text & "'
                                                AND Semester = '" & ddlSemester.Text & "'
                                                AND PelajarMarkahGred = 'GredMT'
                                                AND (Jenis = 'TEKNOLOGI' OR Jenis IS NULL OR Jenis ='')"
                                    strKodMT = oCommon.getFieldValue(strSQL)
                                End If
                            Case "4"
                                If strJenisKursusMT = "SOCIAL" Then
                                    strSQL = "  SELECT NamaMataPelajaran FROM kpmkv_matapelajaran
                                                WHERE
                                                Tahun = '" & ddlTahun.Text & "'
                                                AND Semester = '" & ddlSemester.Text & "'
                                                AND PelajarMarkahGred = 'GredMT'
                                                AND (Jenis ='SOCIAL' OR Jenis IS NULL OR Jenis ='')"
                                    strNamaMT = oCommon.getFieldValue(strSQL)

                                    strSQL = "  SELECT KodMataPelajaran FROM kpmkv_matapelajaran
                                                WHERE
                                                Tahun = '" & ddlTahun.Text & "'
                                                AND Semester = '" & ddlSemester.Text & "'
                                                AND PelajarMarkahGred = 'GredMT'
                                                AND (Jenis ='SOCIAL' OR Jenis IS NULL OR Jenis ='')"
                                    strKodMT = oCommon.getFieldValue(strSQL)

                                Else
                                    strSQL = "  SELECT NamaMataPelajaran FROM kpmkv_matapelajaran
                                                WHERE
                                                Tahun = '" & ddlTahun.Text & "'
                                                AND Semester = '" & ddlSemester.Text & "'
                                                AND PelajarMarkahGred = 'GredMT'
                                                AND (Jenis = 'TEKNOLOGI' OR Jenis IS NULL OR Jenis ='')"
                                    strNamaMT = oCommon.getFieldValue(strSQL)

                                    strSQL = "  SELECT KodMataPelajaran FROM kpmkv_matapelajaran
                                                WHERE
                                                Tahun = '" & ddlTahun.Text & "'
                                                AND Semester = '" & ddlSemester.Text & "'
                                                AND PelajarMarkahGred = 'GredMT'
                                                AND (Jenis = 'TEKNOLOGI' OR Jenis IS NULL OR Jenis ='')"
                                    strKodMT = oCommon.getFieldValue(strSQL)
                                End If
                        End Select
                        Dim myTableMP3 As New PdfPTable(7)
                        myTableMP3.WidthPercentage = 100 ' Table size is set to 100% of the page
                        myTableMP3.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                        myTableMP3.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                        Dim intTblCell3() As Integer = {15, 50, 10, 10, 10, 10, 36}
                        myTableMP3.SetWidths(intTblCell3)

                        myTableMP3.AddCell(New Phrase(strKodMT, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP3.AddCell(New Phrase(strNamaMT, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP3.AddCell(New Phrase(strJamKreditMT, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP3.AddCell(New Phrase(strGredMT, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP3.AddCell(New Phrase(String.Format("{0:0.00}", strNilaiGred), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP3.AddCell(New Phrase(String.Format("{0:0.00}", strNilaiMataMT), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP3.AddCell(New Phrase(strKompentasiMT, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myDocument.Add(myTableMP3)
                    End If

                    'SCIENCE
                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE Tahun = '" & ddlTahun.Text & "' AND Semester = '" & ddlSemester.Text & "' AND NamaMataPelajaran='" & namaMP_SC & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPSC As Array
                    ar_MPSC = strRet.Split("|")
                    strKodSC = ar_MPSC(0)
                    strNamaSC = ar_MPSC(1)
                    strJamKreditSC = ar_MPSC(2)
                    'markah
                    strSQL = "SELECT  GredSC FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    Dim strGredSC As String = oCommon.getFieldValue(strSQL)
                    If Not strGredSC = "" Then
                        'gred
                        strSQL = "SELECT  Pointer, Status FROM kpmkv_gred WHERE Jenis='AKADEMIK' AND Gred='" & strGredSC & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        ' ''--get Gred akademik info
                        Dim ar_akademikSC As Array
                        ar_akademikSC = strRet.Split("|")

                        strNilaiGred = ar_akademikSC(0)
                        strNilaiMataSC = ar_akademikSC(0) * CDbl(strJamKreditSC)
                        strKompentasiSC = ar_akademikSC(1)
                        'strJamKredit_Vokasional = ar_akademik(4)

                        'Sc for teknologi or social  
                        strSQL = "SELECT  JenisKursus From kpmkv_kursus Where KursusID='" & ddlKodKursus.SelectedValue & "'"
                        Dim strJenisKursus As String = oCommon.getFieldValue(strSQL)

                        Select Case ddlSemester.Text
                            Case "1"
                                strNamaSC = strNamaSC
                            Case "2"
                                strNamaSC = strNamaSC
                            Case "3"
                                If strJenisKursus = "SOCIAL" Then
                                    strSQL = "  SELECT NamaMataPelajaran FROM kpmkv_matapelajaran
                                                WHERE
                                                Tahun = '" & ddlTahun.Text & "'
                                                AND Semester = '" & ddlSemester.Text & "'
                                                AND PelajarMarkahGred = 'GredSC'
                                                AND Jenis = 'SOCIAL'"
                                    strNamaSC = oCommon.getFieldValue(strSQL)

                                    strSQL = "  SELECT KodMataPelajaran FROM kpmkv_matapelajaran
                                                WHERE
                                                Tahun = '" & ddlTahun.Text & "'
                                                AND Semester = '" & ddlSemester.Text & "'
                                                AND PelajarMarkahGred = 'GredSC'
                                                AND Jenis = 'SOCIAL'"
                                    strKodSC = oCommon.getFieldValue(strSQL)

                                Else
                                    strSQL = "  SELECT NamaMataPelajaran FROM kpmkv_matapelajaran
                                                WHERE
                                                Tahun = '" & ddlTahun.Text & "'
                                                AND Semester = '" & ddlSemester.Text & "'
                                                AND PelajarMarkahGred = 'GredSC'
                                                AND Jenis = 'TEKNOLOGI'"
                                    strNamaSC = oCommon.getFieldValue(strSQL)

                                    strSQL = "  SELECT KodMataPelajaran FROM kpmkv_matapelajaran
                                                WHERE
                                                Tahun = '" & ddlTahun.Text & "'
                                                AND Semester = '" & ddlSemester.Text & "'
                                                AND PelajarMarkahGred = 'GredSC'
                                                AND Jenis = 'TEKNOLOGI'"
                                    strKodSC = oCommon.getFieldValue(strSQL)
                                End If
                            Case "4"
                                If strJenisKursus = "SOCIAL" Then
                                    strSQL = "  SELECT NamaMataPelajaran FROM kpmkv_matapelajaran
                                                WHERE
                                                Tahun = '" & ddlTahun.Text & "'
                                                AND Semester = '" & ddlSemester.Text & "'
                                                AND PelajarMarkahGred = 'GredSC'
                                                AND Jenis = 'SOCIAL'"
                                    strNamaSC = oCommon.getFieldValue(strSQL)

                                    strSQL = "  SELECT KodMataPelajaran FROM kpmkv_matapelajaran
                                                WHERE
                                                Tahun = '" & ddlTahun.Text & "'
                                                AND Semester = '" & ddlSemester.Text & "'
                                                AND PelajarMarkahGred = 'GredSC'
                                                AND Jenis = 'SOCIAL'"
                                    strKodSC = oCommon.getFieldValue(strSQL)

                                Else
                                    strSQL = "  SELECT NamaMataPelajaran FROM kpmkv_matapelajaran
                                                WHERE
                                                Tahun = '" & ddlTahun.Text & "'
                                                AND Semester = '" & ddlSemester.Text & "'
                                                AND PelajarMarkahGred = 'GredSC'
                                                AND Jenis = 'TEKNOLOGI'"
                                    strNamaSC = oCommon.getFieldValue(strSQL)

                                    strSQL = "  SELECT KodMataPelajaran FROM kpmkv_matapelajaran
                                                WHERE
                                                Tahun = '" & ddlTahun.Text & "'
                                                AND Semester = '" & ddlSemester.Text & "'
                                                AND PelajarMarkahGred = 'GredSC'
                                                AND Jenis = 'TEKNOLOGI'"
                                    strKodSC = oCommon.getFieldValue(strSQL)
                                End If
                        End Select

                        Dim myTableMP4 As New PdfPTable(7)
                        myTableMP4.WidthPercentage = 100 ' Table size is set to 100% of the page
                        myTableMP4.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                        myTableMP4.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                        Dim intTblCell4() As Integer = {15, 50, 10, 10, 10, 10, 36}
                        myTableMP4.SetWidths(intTblCell4)

                        myTableMP4.AddCell(New Phrase(strKodSC, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP4.AddCell(New Phrase(strNamaSC, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP4.AddCell(New Phrase(strJamKreditSC, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP4.AddCell(New Phrase(strGredSC, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP4.AddCell(New Phrase(String.Format("{0:0.00}", strNilaiGred), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP4.AddCell(New Phrase(String.Format("{0:0.00}", strNilaiMataSC), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP4.AddCell(New Phrase(strKompentasiSC, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myDocument.Add(myTableMP4)

                    End If
                    'SEJARAH
                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE Tahun = '" & ddlTahun.Text & "' AND Semester = '" & ddlSemester.Text & "' AND NamaMataPelajaran='" & namaMP_SJ & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_KODSJ As Array
                    ar_KODSJ = strRet.Split("|")
                    strKodSJ = ar_KODSJ(0)
                    strNamaSJ = ar_KODSJ(1)
                    strJamKreditSJ = ar_KODSJ(2)
                    'markah
                    strSQL = "SELECT  GredSJ FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    Dim strGredSJ As String = oCommon.getFieldValue(strSQL)
                    If Not strGredSJ = "" Then
                        'gred
                        strSQL = "SELECT  Pointer, Status FROM kpmkv_gred WHERE Jenis='AKADEMIK' AND Gred='" & strGredSJ & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        ' ''--get Gred akademik info
                        Dim ar_akademikSJ As Array
                        ar_akademikSJ = strRet.Split("|")

                        strNilaiGred = ar_akademikSJ(0)
                        strNilaiMataSJ = ar_akademikSJ(0) * CDbl(strJamKreditSJ)
                        strKompentasiSJ = ar_akademikSJ(1)
                        'strJamKredit_Vokasional = ar_akademik(4)

                        Dim myTableMP5 As New PdfPTable(7)
                        myTableMP5.WidthPercentage = 100 ' Table size is set to 100% of the page
                        myTableMP5.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                        myTableMP5.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                        Dim intTblCell5() As Integer = {15, 50, 10, 10, 10, 10, 36}
                        myTableMP5.SetWidths(intTblCell5)

                        myTableMP5.AddCell(New Phrase(strKodSJ, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP5.AddCell(New Phrase(strNamaSJ, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP5.AddCell(New Phrase(strJamKreditSJ, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP5.AddCell(New Phrase(strGredSJ, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP5.AddCell(New Phrase(String.Format("{0:0.00}", strNilaiGred), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP5.AddCell(New Phrase(String.Format("{0:0.00}", strNilaiMataSJ), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP5.AddCell(New Phrase(strKompentasiSJ, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myDocument.Add(myTableMP5)
                    End If
                    'PENDIDIKAN ISLAM
                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE Tahun = '" & ddlTahun.Text & "' AND Semester = '" & ddlSemester.Text & "' AND NamaMataPelajaran='" & namaMP_PI & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPPI As Array
                    ar_MPPI = strRet.Split("|")
                    strKodPI = ar_MPPI(0)
                    strNamaPI = ar_MPPI(1)
                    strJamKreditPI = ar_MPPI(2)
                    'markah
                    strSQL = "SELECT  GredPI FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    Dim strGredPI As String = oCommon.getFieldValue(strSQL)

                    'gred
                    If Not strGredPI = "" Then
                        strSQL = "SELECT  Pointer, Status FROM kpmkv_gred WHERE Jenis='AKADEMIK' AND Gred='" & strGredPI & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        ' ''--get Gred akademik info
                        Dim ar_akademikPI As Array
                        ar_akademikPI = strRet.Split("|")
                        strNilaiGred = ar_akademikPI(0)
                        strNilaiMataPI = ar_akademikPI(0) * CDbl(strJamKreditPI)
                        strKompentasiPI = ar_akademikPI(1)
                        'strJamKredit_Vokasio

                        Dim myTableMP6 As New PdfPTable(7)
                        myTableMP6.WidthPercentage = 100 ' Table size is set to 100% of the page
                        myTableMP6.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                        myTableMP6.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                        Dim intTblCell6() As Integer = {15, 50, 10, 10, 10, 10, 36}
                        myTableMP6.SetWidths(intTblCell6)

                        myTableMP6.AddCell(New Phrase(strKodPI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP6.AddCell(New Phrase(strNamaPI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP6.AddCell(New Phrase(strJamKreditPI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP6.AddCell(New Phrase(strGredPI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP6.AddCell(New Phrase(String.Format("{0:0.00}", strNilaiGred), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP6.AddCell(New Phrase(String.Format("{0:0.00}", strNilaiMataPI), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP6.AddCell(New Phrase(strKompentasiPI, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myDocument.Add(myTableMP6)

                    End If
                    'PENDIDIKAN MORAL
                    strSQL = "SELECT  KODMatapelajaran, NamaMataPelajaran,JamKredit FROM kpmkv_matapelajaran WHERE Tahun = '" & ddlTahun.Text & "' AND Semester = '" & ddlSemester.Text & "' AND NamaMataPelajaran='" & namaMP_PM & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)
                    Dim ar_MPPM As Array
                    ar_MPPM = strRet.Split("|")
                    strKodPM = ar_MPPM(0)
                    strNamaPM = ar_MPPM(1)
                    strJamKreditPM = ar_MPPM(2)
                    'markah
                    strSQL = "SELECT  GredPM FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    Dim strGredPM As String = oCommon.getFieldValue(strSQL)
                    'gred
                    If Not strGredPM = "" Then
                        strSQL = "SELECT  Pointer, Status FROM kpmkv_gred WHERE Jenis='AKADEMIK' AND Gred='" & strGredPM & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        ' ''--get Gred akademik info
                        Dim ar_akademikPM As Array
                        ar_akademikPM = strRet.Split("|")
                        strNilaiGred = ar_akademikPM(0)
                        strNilaiMataPM = ar_akademikPM(0) * CDbl(strJamKreditPM)
                        strKompentasiPM = ar_akademikPM(1)
                        'strJamKredit_Vokasio

                        Dim myTableMP7 As New PdfPTable(7)
                        myTableMP7.WidthPercentage = 100 ' Table size is set to 100% of the page
                        myTableMP7.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                        myTableMP7.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                        Dim intTblCell7() As Integer = {15, 50, 10, 10, 10, 10, 36}
                        myTableMP7.SetWidths(intTblCell7)

                        myTableMP7.AddCell(New Phrase(strKodPM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP7.AddCell(New Phrase(strNamaPM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP7.AddCell(New Phrase(strJamKreditPM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP7.AddCell(New Phrase(strGredPM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP7.AddCell(New Phrase(String.Format("{0:0.00}", strNilaiGred), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP7.AddCell(New Phrase(String.Format("{0:0.00}", strNilaiMataPM), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myTableMP7.AddCell(New Phrase(strKompentasiPM, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        myDocument.Add(myTableMP7)

                    End If
                    '----VOKASIONAL
                    strSQL = " SELECT COUNT(kpmkv_modul.KODMODUL) AS BILMODUL FROM kpmkv_modul LEFT OUTER JOIN "
                    strSQL += " kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID"
                    strSQL += " WHERE kpmkv_modul.Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND kpmkv_modul.Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND kpmkv_modul.Sesi='" & chkSesi.Text & "'"
                    strSQL += " AND kpmkv_modul.KursusID ='" & ddlKodKursus.Text & "'"
                    Dim strBilModul As Integer = oCommon.getFieldValue(strSQL)


                    For j As Integer = 1 To strBilModul

                        'Modul1
                        strSQL = " SELECT kpmkv_modul.KodModul, kpmkv_modul.NamaModul, kpmkv_modul.JamKredit"
                        strSQL += " FROM  kpmkv_modul LEFT OUTER JOIN kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
                        strSQL += " kpmkv_pelajar ON kpmkv_kursus.KursusID = kpmkv_pelajar.KursusID"
                        strSQL += " WHERE kpmkv_modul.Tahun='" & ddlTahun.Text & "'"
                        strSQL += " AND kpmkv_modul.Semester='" & ddlSemester.Text & "'"
                        strSQL += " AND kpmkv_modul.Sesi='" & chkSesi.Text & "'"
                        strSQL += " AND SUBSTRING(kpmkv_modul.KodModul,6,1)='" & j & "'"
                        strSQL += " AND kpmkv_modul.KursusID='" & ddlKodKursus.Text & "'"
                        strSQL += " AND kpmkv_pelajar.PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " ORDER BY  kpmkv_modul.KodModul, kpmkv_modul.NamaModul, kpmkv_modul.JamKredit ASC"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        Dim ar_VokM1 As Array
                        ar_VokM1 = strRet.Split("|")
                        strKodMP = ar_VokM1(0)
                        strNamaMP = ar_VokM1(1)
                        strJamKredit = ar_VokM1(2)
                        strJamKreditV += CDbl(strJamKredit)

                        strSQL = "SELECT GredV" & j & " FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                        strGred = oCommon.getFieldValue(strSQL)
                        If Not strGred = "" Then
                            'gred
                            strSQL = "SELECT  Pointer, Kompentasi FROM kpmkv_gred WHERE Jenis='VOKASIONAL' AND Gred='" & strGred & "'"
                            strRet = oCommon.getFieldValueEx(strSQL)
                            ' ''--get Gred akademik info
                            Dim ar_VOKGred As Array
                            ar_VOKGred = strRet.Split("|")
                            strNilaiGred = ar_VOKGred(0)
                            strNilaiMataV = ar_VOKGred(0) * CDbl(strJamKredit)
                            strKompentasi = ar_VOKGred(1)
                            strTotalNilaiMata += strNilaiMataV

                            Dim myTableMP8 As New PdfPTable(7)
                            myTableMP8.WidthPercentage = 100 ' Table size is set to 100% of the page
                            myTableMP8.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                            myTableMP8.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                            Dim intTblCell8() As Integer = {15, 50, 10, 10, 10, 10, 36}
                            myTableMP8.SetWidths(intTblCell8)

                            myTableMP8.AddCell(New Phrase(strKodMP, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                            myTableMP8.AddCell(New Phrase(strNamaMP, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                            myTableMP8.AddCell(New Phrase(strJamKredit, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                            myTableMP8.AddCell(New Phrase(strGred, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                            myTableMP8.AddCell(New Phrase(String.Format("{0:0.00}", strNilaiGred), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                            myTableMP8.AddCell(New Phrase(String.Format("{0:0.00}", strNilaiMataV), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                            myTableMP8.AddCell(New Phrase(strKompentasi, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                            myDocument.Add(myTableMP8)
                        End If

                    Next 'modul

                    '' 19062019 MATAPELAJARAN VOKASIONAL-----------------------------------------------------

                    myDocument.Add(imgSpacing)

                    strSQL = "  SELECT KodMPVOK, NamaMPVOK FROM kpmkv_matapelajaran_v
                                WHERE 
                                Tahun = '" & ddlTahun.SelectedValue & "'
                                AND Semester = '" & ddlSemester.Text & "'
                                AND KursusID = '" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.getFieldValueEx(strSQL)

                    Dim MP_VOK As Array
                    MP_VOK = strRet.Split("|")
                    Dim strKodMPV As String = MP_VOK(0)
                    Dim strNamaMPV As String = MP_VOK(1)

                    strSQL = " SELECT SMP_Grade FROM kpmkv_pelajar_markah WHERE PelajarID = '" & strKey & "'"
                    Dim SMP_Grade As String = oCommon.getFieldValue(strSQL)

                    strSQL = "  SELECT Status FROM kpmkv_gred_vokasional 
                                WHERE
                                Gred = '" & SMP_Grade & "'
                                AND Tahun = '" & ddlTahun.SelectedValue & "'
                                AND Semester = '" & ddlSemester.Text & "'"
                    Dim strStatusMPV As String = oCommon.getFieldValue(strSQL)

                    Dim myTableMP9 As New PdfPTable(7)
                    myTableMP9.WidthPercentage = 100 ' Table size is set to 100% of the page
                    myTableMP9.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                    myTableMP9.DefaultCell.BorderWidth = Rectangle.NO_BORDER
                    Dim intTblCell9() As Integer = {15, 50, 10, 10, 10, 10, 36}
                    myTableMP9.SetWidths(intTblCell9)

                    myTableMP9.AddCell(New Phrase(strKodMPV, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP9.AddCell(New Phrase(strNamaMPV, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP9.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP9.AddCell(New Phrase(SMP_Grade, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP9.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP9.AddCell(New Phrase("", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myTableMP9.AddCell(New Phrase(strStatusMPV, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(myTableMP9)

                    '' 19062019 MATAPELAJARAN VOKASIONAL-----------------------------------------------------

                    Dim strJamKreditAkademik As Double

                    If Not strGredPI = "" Then
                        strJamKreditAkademik = CDbl(strJamKreditBM) + CDbl(strJamKreditBI) + CDbl(strJamKreditMT) + CDbl(strJamKreditSC) + CDbl(strJamKreditSJ) + CDbl(strJamKreditPI)
                    ElseIf Not strGredPM = "" Then
                        strJamKreditAkademik = CDbl(strJamKreditBM) + CDbl(strJamKreditBI) + CDbl(strJamKreditMT) + CDbl(strJamKreditSC) + CDbl(strJamKreditSJ) + CDbl(strJamKreditPI)
                    End If

                    'check by semester

                    Dim strTotalNilaiMataAkademik As Double = CDbl(strNilaiMataBM) + CDbl(strNilaiMataBI) + CDbl(strNilaiMataMT) + CDbl(strNilaiMataSC) + CDbl(strNilaiMataSJ) + CDbl(strNilaiMataPI) + CDbl(strNilaiMataPM)
                    Dim strTotalNilaiMataPNGA As Double = Math.Round((strTotalNilaiMataAkademik / strJamKreditAkademik), 2)
                    Dim strTotalNilaiMataPNGV As Double = Math.Round((strTotalNilaiMata / strJamKreditV), 2)
                    Dim strTotalNilaiMataPNGK As Double = Math.Round((strTotalNilaiMataAkademik + strTotalNilaiMata) / (strJamKreditAkademik + strJamKreditV), 2)
                    Dim strTotalNilaiMataPNGKBM As Double = 0.0

                    If ddlSemester.Text = 1 Then
                        strTotalNilaiMataPNGKBM = Math.Round((strNilaiMataBM / strJamKreditBM), 2)

                    ElseIf ddlSemester.Text = 2 Then
                        'checkin PelajarID for other semester
                        'change on 28July2016
                        strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='1'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        If oCommon.isExist(strSQL) = True Then
                            strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                            strSQL += " AND Semester='1'"
                            strSQL += " AND IsDeleted='N' AND StatusID='2'"
                            strRet = oCommon.getFieldValueEx(strSQL)

                            Dim ar_HPelajar As Array
                            ar_HPelajar = strRet.Split("|")
                            Dim strP_ID As String = ar_HPelajar(0)
                            Dim strP_Tahun As String = ar_HPelajar(1)
                            Dim strP_Sesi As String = ar_HPelajar(2)

                            strSQL = "SELECT Jum_NilaiMata_Akademik_BM,Jum_JamKredit_Akademik_BM FROM kpmkv_pelajar_markah"
                            strSQL += " WHERE Tahun='" & strP_Tahun & "'"
                            strSQL += " AND Semester='1'"
                            strSQL += " AND Sesi='" & strP_Sesi & "'"
                            strSQL += " AND PelajarID='" & strP_ID & "'"
                            strRet = oCommon.getFieldValueEx(strSQL)

                            Dim ar_HMarkah2 As Array
                            ar_HMarkah2 = strRet.Split("|")
                            Dim strNilaiMata_Akademik_BM2 As Double = ar_HMarkah2(0)
                            Dim strJamKredit_Akademik_BM2 As Double = ar_HMarkah2(1)
                            strTotalNilaiMataPNGKBM = Math.Round((strNilaiMataBM + strNilaiMata_Akademik_BM2) / (strJamKreditBM + strJamKredit_Akademik_BM2), 2)
                        End If

                    ElseIf ddlSemester.Text = 3 Then
                        'change on 28July2016
                        strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='1'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        If oCommon.isExist(strSQL) = True Then
                            strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                            strSQL += " AND Semester='1'"
                            strSQL += " AND IsDeleted='N' AND StatusID='2'"
                            strRet = oCommon.getFieldValueEx(strSQL)

                            Dim ar_HPelajar2 As Array
                            ar_HPelajar2 = strRet.Split("|")
                            Dim strP_ID2 As String = ar_HPelajar2(0)
                            Dim strP_Tahun2 As String = ar_HPelajar2(1)
                            Dim strP_Sesi2 As String = ar_HPelajar2(2)

                            strSQL = "SELECT Jum_NilaiMata_Akademik_BM,Jum_JamKredit_Akademik_BM FROM kpmkv_pelajar_markah"
                            strSQL += " WHERE Tahun='" & strP_Tahun2 & "'"
                            strSQL += " AND Semester='1'"
                            strSQL += " AND Sesi='" & strP_Sesi2 & "'"
                            strSQL += " AND PelajarID='" & strP_ID2 & "'"
                            strRet = oCommon.getFieldValueEx(strSQL)
                            Dim ar_HMarkah1_3 As Array
                            ar_HMarkah1_3 = strRet.Split("|")
                            Dim strNilaiMata_Akademik_BM1_3 As Double = ar_HMarkah1_3(0)
                            Dim strJamKredit_Akademik_BM1_3 As Double = ar_HMarkah1_3(1)

                            'if exist
                            'change on 28July2016
                            strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                            strSQL += " AND Semester='2'"
                            strSQL += " AND IsDeleted='N' AND StatusID='2'"
                            If oCommon.isExist(strSQL) = True Then
                                strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                                strSQL += " AND Semester='2'"
                                strSQL += " AND IsDeleted='N' AND StatusID='2'"
                                strRet = oCommon.getFieldValueEx(strSQL)

                                Dim ar_HPelajar3 As Array
                                ar_HPelajar3 = strRet.Split("|")
                                Dim strP_ID3 As String = ar_HPelajar3(0)
                                Dim strP_Tahun3 As String = ar_HPelajar3(1)
                                Dim strP_Sesi3 As String = ar_HPelajar3(2)

                                strSQL = "SELECT Jum_NilaiMata_Akademik_BM,Jum_JamKredit_Akademik_BM FROM kpmkv_pelajar_markah"
                                strSQL += " WHERE Tahun='" & strP_Tahun3 & "'"
                                strSQL += " AND Semester='2'"
                                strSQL += " AND Sesi='" & strP_Sesi3 & "'"
                                strSQL += " AND PelajarID='" & strP_ID3 & "'"
                                strRet = oCommon.getFieldValueEx(strSQL)

                                Dim ar_HMarkah3 As Array
                                ar_HMarkah3 = strRet.Split("|")
                                Dim strNilaiMata_Akademik_BM3 As Double = CDbl(ar_HMarkah3(0))
                                Dim strJamKredit_Akademik_BM3 As Double = ar_HMarkah3(1)
                                strTotalNilaiMataPNGKBM = Math.Round((strNilaiMataBM + strNilaiMata_Akademik_BM3 + strNilaiMata_Akademik_BM1_3) / (strJamKreditBM + strJamKredit_Akademik_BM3 + strJamKredit_Akademik_BM1_3), 2)
                            End If
                        End If

                    ElseIf ddlSemester.Text = 4 Then
                        'checkin PelajarID for other semester
                        'change on 28July2016
                        strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='1'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        If oCommon.isExist(strSQL) = True Then
                            strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                            strSQL += " AND Semester='1'"
                            strSQL += " AND IsDeleted='N' AND StatusID='2'"
                            strRet = oCommon.getFieldValueEx(strSQL)

                            Dim ar_HPelajar4 As Array
                            ar_HPelajar4 = strRet.Split("|")
                            Dim strP_ID4 As String = ar_HPelajar4(0)
                            Dim strP_Tahun4 As String = ar_HPelajar4(1)
                            Dim strP_Sesi4 As String = ar_HPelajar4(2)

                            strSQL = "SELECT Jum_NilaiMata_Akademik_BM,Jum_JamKredit_Akademik_BM FROM kpmkv_pelajar_markah"
                            strSQL += " WHERE Tahun='" & strP_Tahun4 & "'"
                            strSQL += " AND Semester='1'"
                            strSQL += " AND Sesi='" & strP_Sesi4 & "'"
                            strSQL += " AND PelajarID='" & strP_ID4 & "'"
                            strRet = oCommon.getFieldValueEx(strSQL)

                            Dim ar_HMarkah1_4 As Array
                            ar_HMarkah1_4 = strRet.Split("|")
                            Dim strNilaiMata_Akademik_BM1_4 As Double = ar_HMarkah1_4(0)
                            Dim strJamKredit_Akademik_BM1_4 As Double = ar_HMarkah1_4(1)

                            'if exist
                            'change on 28July2016
                            strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                            strSQL += " AND Semester='2'"
                            strSQL += " AND IsDeleted='N' AND StatusID='2'"
                            If oCommon.isExist(strSQL) = True Then
                                strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                                strSQL += " AND Semester='2'"
                                strSQL += " AND IsDeleted='N' AND StatusID='2'"
                                strRet = oCommon.getFieldValueEx(strSQL)

                                Dim ar_HPelajar5 As Array
                                ar_HPelajar5 = strRet.Split("|")
                                Dim strP_ID5 As String = ar_HPelajar5(0)
                                Dim strP_Tahun5 As String = ar_HPelajar5(1)
                                Dim strP_Sesi5 As String = ar_HPelajar5(2)

                                strSQL = "SELECT Jum_NilaiMata_Akademik_BM,Jum_JamKredit_Akademik_BM FROM kpmkv_pelajar_markah"
                                strSQL += " WHERE Tahun='" & strP_Tahun5 & "'"
                                strSQL += " AND Semester='2'"
                                strSQL += " AND Sesi='" & strP_Sesi5 & "'"
                                strSQL += " AND PelajarID='" & strP_ID5 & "'"
                                strRet = oCommon.getFieldValueEx(strSQL)
                                Dim ar_HMarkah2_4 As Array
                                ar_HMarkah2_4 = strRet.Split("|")
                                Dim strNilaiMata_Akademik_BM2_4 As Double = CDbl(ar_HMarkah2_4(0))
                                Dim strJamKredit_Akademik_BM2_4 As Double = ar_HMarkah2_4(1)

                                'if exist
                                'change on 28July2016
                                strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                                strSQL += " AND Semester='3'"
                                strSQL += " AND IsDeleted='N' AND StatusID='2'"
                                If oCommon.isExist(strSQL) = True Then
                                    strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                                    strSQL += " AND Semester='3'"
                                    strSQL += " AND IsDeleted='N' AND StatusID='2'"
                                    strRet = oCommon.getFieldValueEx(strSQL)

                                    Dim ar_HPelajar6 As Array
                                    ar_HPelajar6 = strRet.Split("|")
                                    Dim strP_ID6 As String = ar_HPelajar6(0)
                                    Dim strP_Tahun6 As String = ar_HPelajar6(1)
                                    Dim strP_Sesi6 As String = ar_HPelajar6(2)

                                    strSQL = "SELECT Jum_NilaiMata_Akademik_BM,Jum_JamKredit_Akademik_BM FROM kpmkv_pelajar_markah"
                                    strSQL += " WHERE Tahun='" & strP_Tahun6 & "'"
                                    strSQL += " AND Semester='3'"
                                    strSQL += " AND Sesi='" & strP_Sesi6 & "'"
                                    strSQL += " AND PelajarID='" & strP_ID6 & "'"
                                    strRet = oCommon.getFieldValueEx(strSQL)
                                    ' ''--get Gred akademik info
                                    Dim ar_HMarkah3_4 As Array
                                    ar_HMarkah3_4 = strRet.Split("|")
                                    Dim strNilaiMata_Akademik_BM3_4 As Double = CDbl(ar_HMarkah3_4(0))
                                    Dim strJamKredit_Akademik_BM3_4 As Double = ar_HMarkah3_4(1)
                                    strTotalNilaiMataPNGKBM = Math.Round((strNilaiMataBM + strNilaiMata_Akademik_BM3_4 + strNilaiMata_Akademik_BM2_4 + strNilaiMata_Akademik_BM1_4) / (strJamKreditBM + strJamKredit_Akademik_BM3_4 + strJamKredit_Akademik_BM2_4 + strJamKredit_Akademik_BM1_4), 2)
                                End If
                            End If
                        End If
                    End If
                    Dim strNilaiMata_Akademik1 As Double = 0.0
                    Dim strNilaiMata_Vokasional1 As Double = 0.0
                    Dim strJamKredit_Akademik1 As Double = 0.0
                    Dim strJamKredit_Vokasional1 As Double = 0.0
                    Dim strTotalNilaiMataPNGKA As Double = 0.0
                    Dim strTotalNilaiMataPNGKV As Double = 0.0
                    Dim strTotalNilaiMataPNGKK As Double = 0.0
                    Dim strP_ID_P As String = ""
                    Dim strP_Tahun_P As String = ""
                    Dim strP_Sesi_P As String = ""

                    If ddlSemester.Text = 1 Then
                        strTotalNilaiMataPNGKA = Math.Round((strTotalNilaiMataAkademik / strJamKreditAkademik), 2)
                        strTotalNilaiMataPNGKV = Math.Round((strTotalNilaiMata / strJamKreditV), 2)
                        strTotalNilaiMataPNGKK = Math.Round((strTotalNilaiMataAkademik + strTotalNilaiMata) / (strJamKreditAkademik + strJamKreditV), 2)

                    ElseIf ddlSemester.Text = 2 Then
                        'if exist
                        'change on 28July2016
                        strSQL = "SELECT PelajarID FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                        strSQL += " AND Semester='1'"
                        strSQL += " AND IsDeleted='N' AND StatusID='2'"
                        If oCommon.isExist(strSQL) = True Then
                            strSQL = "SELECT PelajarID,Tahun,Sesi FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliran & "'"
                            strSQL += " AND Semester='1'"
                            strSQL += " AND IsDeleted='N' AND StatusID='2'"
                            strRet = oCommon.getFieldValueEx(strSQL)

                            Dim ar_HPelajar As Array
                            ar_HPelajar = strRet.Split("|")
                            strP_ID_P = ar_HPelajar(0)
                            strP_Tahun_P = ar_HPelajar(1)
                            strP_Sesi_P = ar_HPelajar(2)

                            strSQL = "SELECT Jum_NilaiMata_Akademik,Jum_NilaiMata_Vokasional,Jum_JamKredit_Akademik,Jum_JamKredit_Vokasional FROM kpmkv_pelajar_markah"
                            strSQL += " WHERE Tahun='" & strP_Tahun_P & "'"
                            strSQL += " AND Semester='1'"
                            strSQL += " AND Sesi='" & strP_Sesi_P & "'"
                            strSQL += " AND PelajarID='" & strP_ID_P & "'"
                            strRet = oCommon.getFieldValueEx(strSQL)

                            ' ''--get Gred VOK info
                            Dim ar_HMarkah2 As Array
                            ar_HMarkah2 = strRet.Split("|")
                            strNilaiMata_Akademik1 = CDbl(ar_HMarkah2(0))
                            strNilaiMata_Vokasional1 = ar_HMarkah2(1)
                            strJamKredit_Akademik1 = CDbl(ar_HMarkah2(2))
                            strJamKredit_Vokasional1 = ar_HMarkah2(3)
                        End If

                    ElseIf ddlSemester.Text = 3 Then
                        'if exist
                        'change on 28July2016
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
                            strP_ID_P = ar_HPelajar(0)
                            strP_Tahun_P = ar_HPelajar(1)
                            strP_Sesi_P = ar_HPelajar(2)

                            strSQL = "SELECT Jum_NilaiMata_Akademik,Jum_NilaiMata_Vokasional,Jum_JamKredit_Akademik,Jum_JamKredit_Vokasional FROM kpmkv_pelajar_markah"
                            strSQL += " WHERE Tahun='" & strP_Tahun_P & "'"
                            strSQL += " AND Semester='2'"
                            strSQL += " AND Sesi='" & strP_Sesi_P & "'"
                            strSQL += " AND PelajarID='" & strP_ID_P & "'"
                            strRet = oCommon.getFieldValueEx(strSQL)

                            ' ''--get Gred VOK info
                            Dim ar_HMarkah3 As Array
                            ar_HMarkah3 = strRet.Split("|")
                            strNilaiMata_Akademik1 = CDbl(ar_HMarkah3(0))
                            strNilaiMata_Vokasional1 = ar_HMarkah3(1)
                            strJamKredit_Akademik1 = CDbl(ar_HMarkah3(2))
                            strJamKredit_Vokasional1 = ar_HMarkah3(3)
                        End If

                    ElseIf ddlSemester.Text = 4 Then
                        'if exist
                        'change on 28July2016
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
                            strP_ID_P = ar_HPelajar(0)
                            strP_Tahun_P = ar_HPelajar(1)
                            strP_Sesi_P = ar_HPelajar(2)

                            strSQL = "SELECT Jum_NilaiMata_Akademik,Jum_NilaiMata_Vokasional,Jum_JamKredit_Akademik,Jum_JamKredit_Vokasional FROM kpmkv_pelajar_markah"
                            strSQL += " WHERE Tahun='" & strP_Tahun_P & "'"
                            strSQL += " AND Semester='3'"
                            strSQL += " AND Sesi='" & strP_Sesi_P & "'"
                            strSQL += " AND PelajarID='" & strP_ID_P & "'"
                            strRet = oCommon.getFieldValueEx(strSQL)

                            ' ''--get Gred VOK info
                            Dim ar_HMarkah4 As Array
                            ar_HMarkah4 = strRet.Split("|")
                            strNilaiMata_Akademik1 = CDbl(ar_HMarkah4(0))
                            strNilaiMata_Vokasional1 = ar_HMarkah4(1)
                            strJamKredit_Akademik1 = CDbl(ar_HMarkah4(2))
                            strJamKredit_Vokasional1 = ar_HMarkah4(3)
                            'end for semester
                        End If
                    End If
                    strTotalNilaiMataPNGKA = Math.Round((strNilaiMata_Akademik1 + strTotalNilaiMataAkademik) / (strJamKredit_Akademik1 + strJamKreditAkademik), 2)
                    strTotalNilaiMataPNGKV = Math.Round((strNilaiMata_Vokasional1 + strTotalNilaiMata) / (strJamKredit_Vokasional1 + strJamKreditV), 2)
                    strTotalNilaiMataPNGKK = Math.Round((strNilaiMata_Akademik1 + strTotalNilaiMataAkademik + strNilaiMata_Vokasional1 + strTotalNilaiMata) / (strJamKredit_Akademik1 + strJamKreditAkademik + strJamKredit_Vokasional1 + strJamKreditV), 2)

                    ' Format(Val(txtA.Text) * 1000 / Val(txtG.Text), "0.00")
                    Dim strMata_A As Double = Format(strTotalNilaiMataAkademik + strNilaiMata_Akademik1, "0.00")
                    Dim strJamKredit_A As Double = Format(strJamKredit_Akademik1 + strJamKreditAkademik, "0.00")
                    Dim strMata_V As Double = Format(strNilaiMata_Vokasional1 + strTotalNilaiMata, "0.00")
                    Dim strJamKredit_V As Double = Format(strJamKredit_Vokasional1 + strJamKreditV, "0.00")

                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)
                    myDocument.Add(New Paragraph("PURATA NILAI GRED BAHASA MELAYU (PNGBM)                                   :" & String.Format("{0:0.00}", strNilaiGredBM), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(New Paragraph("PURATA NILAI GRED KUMULATIF BAHASA MELAYU (PNGKBM)           :" & String.Format("{0:0.00}", strTotalNilaiMataPNGKBM), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(imgSpacing)
                    myDocument.Add(New Paragraph("PURATA NILAI GRED AKADEMIK (PNGA)                                                  :" & String.Format("{0:0.00}", strTotalNilaiMataPNGA), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(New Paragraph("PURATA NILAI GRED KUMULATIF AKADEMIK (PNGKA)                          :" & String.Format("{0:0.00}", strTotalNilaiMataPNGKA), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(imgSpacing)
                    myDocument.Add(New Paragraph("PURATA NILAI GRED VOKASIONAL (PNGV)                                              :" & String.Format("{0:0.00}", strTotalNilaiMataPNGV), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(New Paragraph("PURATA NILAI GRED KUMULATIF VOKASIONAL (PNGKV)                      :" & String.Format("{0:0.00}", strTotalNilaiMataPNGKV), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(imgSpacing)
                    myDocument.Add(New Paragraph("PURATA NILAI GRED KESELURUHAN (PNGK)                                           :" & String.Format("{0:0.00}", strTotalNilaiMataPNGK), FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                    myDocument.Add(New Paragraph("PURATA NILAI GRED KUMULATIF KESELURUHAN (PNGKK)                   :" & String.Format("{0:0.00}", strTotalNilaiMataPNGKK), FontFactory.GetFont("Arial", 8, Font.NORMAL)))

                    strSQL = "UPDATE kpmkv_pelajar_markah SET Jum_NilaiMata_Akademik_BM ='" & strTtlNilaiGredBM & "', Jum_JamKredit_Akademik_BM ='" & strJamKreditBM & "', "
                    strSQL += " Jum_NilaiMata_Akademik ='" & strMata_A & "', Jum_NilaiMata_Vokasional ='" & strMata_V & "', Jum_JamKredit_Akademik ='" & strJamKredit_A & "', Jum_JamKredit_Vokasional ='" & strJamKredit_V & "',"
                    strSQL += " PNG_Akademik ='" & strTotalNilaiMataAkademik & "', PNG_Vokasional ='" & strTotalNilaiMata & "', JamKredit_Akademik ='" & strJamKreditAkademik & "', JamKredit_Vokasional ='" & strJamKreditV & "',"
                    strSQL += " PNGBM ='" & strNilaiGredBM & "', PNGKBM ='" & strTotalNilaiMataPNGKBM & "', PNGA ='" & strTotalNilaiMataPNGA & "', PNGKA ='" & strTotalNilaiMataPNGKA & "',"
                    strSQL += " PNGV ='" & strTotalNilaiMataPNGV & "', PNGKV ='" & strTotalNilaiMataPNGKV & "', PNGK ='" & strTotalNilaiMataPNGK & "', PNGKK ='" & strTotalNilaiMataPNGKK & "'"
                    strSQL += " WHERE Tahun='" & ddlTahun.Text & "'"
                    strSQL += " AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    strSQL += " AND PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)

                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)
                    myDocument.Add(New Paragraph("TARIKH: " & ddlHari.Text & "/" & ddlBulan.Text & "/" & ddlTahun_1.Text & "                                                                                                                                                      PENGARAH", FontFactory.GetFont("Arial", 8, Font.BOLD)))
                    Dim myPengarah As New Paragraph("" & strKolejnama, FontFactory.GetFont("Arial", 8, Font.BOLD))
                    myPengarah.Alignment = Element.ALIGN_RIGHT
                    myDocument.Add(myPengarah)

                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)
                    Dim myslip As New Paragraph("Slip ini adalah cetakan komputer, tandatangan tidak diperlukan", FontFactory.GetFont("Arial", 8, Font.ITALIC))
                    myslip.Alignment = Element.ALIGN_CENTER
                    myDocument.Add(myslip)
                    myDocument.NewPage()
                    ''--content end
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


    Private Sub datRespondent_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles datRespondent.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim chk1 As CheckBox
            chk1 = DirectCast(datRespondent.HeaderRow.Cells(0).FindControl("chkAll"), CheckBox)
            For Each row As GridViewRow In datRespondent.Rows
                Dim chk As CheckBox
                chk = DirectCast(row.Cells(0).FindControl("chkSelect"), CheckBox)
                chk.Checked = chk1.Checked
            Next
        End If
    End Sub
End Class