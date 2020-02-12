Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Imports RKLib.ExportData

Imports iTextSharp.text
Imports iTextSharp.text.pdf
Public Class pernyataan_keputusan_BM
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
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                kpmkv_tahun_list()
                ddlTahun.Text = Now.Year

                kpmkv_kelas_list()

                kpmkv_kodkursus_list()

                strRet = BindData(datRespondent)
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
        strWhere += " AND kpmkv_pelajar.Semester ='4'"

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
            myDataAdapter.Fill(myDataSet, "Pernyataan Keputusan Vokasional")
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
        Dim myDocument As New Document(PageSize.A4.Rotate)

        Try
            HttpContext.Current.Response.ContentType = "application/pdf"
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=PernyataanKeputusan.pdf")
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
            Dim imageHeader As String = Server.MapPath("~/img/GLogo.png")
            Dim imgHeader As Image = Image.GetInstance(imageHeader)
            imgHeader.Alignment = Image.ALIGN_CENTER  'left
            imgHeader.Border = 1

            ''--loop thru records here

            Dim strNama As String = ""
            Dim strMykad As String = ""
            Dim strAngkaGiliran As String = ""
            Dim strKodKursus As String = ""
            Dim strKluster As String = ""

            ' strRet = BindData(datRespondent)
            For i As Integer = 0 To datRespondent.Rows.Count - 1
                Dim strStudentID As String = ""

                ''--Tokenid,ClassCode,Q001Remarks
                strSQL = "SELECT Nama FROM kpmkv_pelajar WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strNama = oCommon.getFieldValue(strSQL)

                strSQL = "SELECT Mykad FROM kpmkv_pelajar WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strMykad = oCommon.getFieldValue(strSQL)

                strSQL = "SELECT AngkaGiliran FROM kpmkv_pelajar WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strAngkaGiliran = oCommon.getFieldValue(strSQL)

                strSQL = "SELECT NamaKursus FROM kpmkv_kursus WHERE KursusID='" & ddlKodKursus.SelectedValue & "'"
                strKodKursus = oCommon.getFieldValue(strSQL)

                strSQL = "SELECT Top 1 kpmkv_kluster.NamaKluster FROM  kpmkv_kluster LEFT OUTER JOIN kpmkv_kursus "
                strSQL += " ON kpmkv_kluster.KlusterID = kpmkv_kursus.KlusterID "
                strSQL += " WHERE kpmkv_kursus.NamaKursus ='" & strKodKursus & "' "
                strKluster = oCommon.getFieldValue(strSQL)

                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)

                'kolejnegeri
                strSQL = "SELECT Negeri FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                Dim strKolejnegeri As String = oCommon.getFieldValue(strSQL)

                'KodModul
                strSQL = "SELECT KodMataPelajaran FROM kpmkv_matapelajaran WHERE KodMataPelajaran LIKE '%A01'+'400'"
                Dim strKodModul As String = oCommon.getFieldValue(strSQL)

                'NamaModul
                strSQL = "SELECT NamaMataPelajaran FROM kpmkv_matapelajaran WHERE KodMataPelajaran='" & strKodModul & "'"
                Dim strNamaMataPelajaran As String = oCommon.getFieldValue(strSQL)

                'Title
                Dim myTable As New PdfPTable(3)
                myTable.WidthPercentage = 100 ' Table size is set to 100% of the page
                myTable.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                'myTable.HorizontalAlignment = Rectangle.NO_BORDER
                Dim intTblWidth() As Integer = {36, 36, 36}
                myTable.SetWidths(intTblWidth)

                Dim Cell1Hdr As New PdfPCell(New Phrase("KEMENTERIAN PENDIDIKAN MALAYSIA ", FontFactory.GetFont("Arial", 9, Font.NORMAL)))
                Cell1Hdr.Border = Rectangle.NO_BORDER
                myTable.AddCell(Cell1Hdr)

                myTable.AddCell(imgHeader)

                Dim Cell3Hdr As New PdfPCell(New Phrase("MINISTRY OF EDUCATION MALAYSIA", FontFactory.GetFont("Arial", 9, Font.NORMAL)))
                Cell3Hdr.Border = Rectangle.NO_BORDER
                myTable.AddCell(Cell3Hdr)
                myDocument.Add(myTable)

                myDocument.Add(imgSpacing)
                myDocument.Add(imgSpacing)

                ''Title 1st
                Dim mySubTittle1 As New Paragraph("PERNYATAAN KEPUTUSAN", FontFactory.GetFont("Arial", 8, Font.NORMAL))
                mySubTittle1.Alignment = Element.ALIGN_CENTER
                myDocument.Add(mySubTittle1)

                myDocument.Add(imgSpacing)
                myDocument.Add(imgSpacing)

                ''Title 2nd
                Dim mySubTittle2 As New Paragraph("Calon yang tersebut namanya di bawah telah mengambil peperiksaan " & strNamaMataPelajaran & " Kolej Vokasional Kod " & strKodModul & " dan menunjukkan prestasi seperti yang tercatat di bawah", FontFactory.GetFont("Arial", 8, Font.NORMAL))
                mySubTittle2.Alignment = Element.ALIGN_CENTER
                myDocument.Add(mySubTittle2)

                myDocument.Add(imgSpacing)
                myDocument.Add(imgSpacing)
                myDocument.Add(imgSpacing)

                'Detail
                myDocument.Add(New Paragraph("NAMA                              :" & strNama, FontFactory.GetFont("Arial", 9, Font.NORMAL)))
                myDocument.Add(New Paragraph("NO.KAD PENGENALAN :" & strMykad, FontFactory.GetFont("Arial", 9, Font.NORMAL)))
                myDocument.Add(New Paragraph("ANGKAGILIRAN             :" & strAngkaGiliran, FontFactory.GetFont("Arial", 9, Font.NORMAL)))
                myDocument.Add(New Paragraph("INSTITUSI                      :" & strKolejnama & "  " & ", " & strKolejnegeri, FontFactory.GetFont("Arial", 9, Font.NORMAL)))
                myDocument.Add(New Paragraph("KLUSTER                       :" & strKluster, FontFactory.GetFont("Arial", 9, Font.NORMAL)))
                myDocument.Add(New Paragraph("KURSUS:                       :" & strKodKursus, FontFactory.GetFont("Arial", 9, Font.NORMAL)))

                myDocument.Add(imgSpacing)
                myDocument.Add(imgSpacing)
                myDocument.Add(imgSpacing)

                'header column
                Dim myTable1 As New PdfPTable(3)
                myTable1.WidthPercentage = 100 ' Table size is set to 100% of the page
                myTable1.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                myTable.HorizontalAlignment = Rectangle.MULTI_COLUMN_TEXT
                Dim intTblWidth1() As Integer = {36, 36, 36}
                myTable1.SetWidths(intTblWidth1)

                Dim CellAHdr As New PdfPCell(New Phrase("KOD ", FontFactory.GetFont("Arial", 9, Font.NORMAL)))
                myTable1.AddCell(CellAHdr)

                Dim CellBHdr As New PdfPCell(New Phrase("MATAPELAJARAN", FontFactory.GetFont("Arial", 9, Font.NORMAL)))
                myTable1.AddCell(CellBHdr)

                Dim CellCHdr As New PdfPCell(New Phrase("GRED", FontFactory.GetFont("Arial", 9, Font.NORMAL)))
                myTable1.AddCell(CellCHdr)

                myTable1.DefaultCell.BorderWidth = 1
                myTable1.DefaultCell.Border = Rectangle.BOX
                myDocument.Add(myTable1)

                myDocument.Add(imgSpacing)
                Dim strGred As String = ""
                'BahasaMelayu
                'Gred
                strSQL = "SELECT  GredBM FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "'"
                strSQL += " AND Semester='4'"
                strSQL += " AND Sesi='" & chkSesi.Text & "'"
                Dim strGredBM As String = oCommon.getFieldValue(strSQL)

                Dim myTableMP1 As New PdfPTable(3)
                myTableMP1.WidthPercentage = 100 ' Table size is set to 100% of the page
                myTableMP1.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                Dim intTblCell() As Integer = {36, 36, 36}
                myTableMP1.SetWidths(intTblCell)

                myTableMP1.AddCell(New Phrase(strKodModul, FontFactory.GetFont("Arial", 9, Font.NORMAL)))
                myTableMP1.AddCell(New Phrase(strNamaMataPelajaran, FontFactory.GetFont("Arial", 9, Font.NORMAL)))
                myTableMP1.AddCell(New Phrase(strGredBM, FontFactory.GetFont("Arial", 9, Font.NORMAL)))
                myTableMP1.DefaultCell.Border = Rectangle.NO_BORDER
                myDocument.Add(myTableMP1)

                myDocument.Add(imgSpacing)
                myDocument.Add(imgSpacing)

                'footer column
                Dim myFooter As New PdfPTable(3)
                myFooter.WidthPercentage = 100 ' Table size is set to 100% of the page
                myFooter.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                myFooter.HorizontalAlignment = Rectangle.NO_BORDER
                Dim intTblWidth2() As Integer = {36, 36, 36}
                myFooter.SetWidths(intTblWidth2)

                Dim CellDHdr As New PdfPCell(New Phrase(" ", FontFactory.GetFont("Arial", 9, Font.NORMAL)))
                CellDHdr.Border = Rectangle.NO_BORDER
                myFooter.AddCell(CellDHdr)

                Dim CellEHdr As New PdfPCell(New Phrase("", FontFactory.GetFont("Arial", 9, Font.NORMAL)))
                CellEHdr.Border = Rectangle.NO_BORDER
                myFooter.AddCell(CellEHdr)

                Dim CellFHdr As New PdfPCell(New Phrase("Pengarah Peperiksaan Kementerian Pendidikan Malaysia", FontFactory.GetFont("Arial", 9, Font.NORMAL)))
                CellFHdr.Border = Rectangle.NO_BORDER
                myFooter.AddCell(CellFHdr)
                myDocument.Add(myFooter)

                myDocument.Add(imgSpacing)
                myDocument.Add(imgSpacing)

                ''Footer 2nd
                Dim mySubFooter As New Paragraph("Pernyataan ini dikeluarkan kepada calon yang telah memenuhi kelulusan " & strNamaMataPelajaran & " Kolej Vokasional Kod " & strKodModul & " yang setara dengan [lblModul3]Sijil Pelajaran Malaysia Kod [lblKodModul3]", FontFactory.GetFont("Arial", 8, Font.NORMAL))
                mySubFooter.Alignment = Element.ALIGN_CENTER
                myDocument.Add(mySubFooter)

                ''Footer 3nd
                Dim mySubFooter2 As New Paragraph("" & ddlTahun.Text & "  ", 4, FontFactory.GetFont("Arial", 8, Font.NORMAL))
                mySubFooter2.Alignment = Element.ALIGN_LEFT
                myDocument.Add(mySubFooter2)
                myDocument.NewPage()
                ''--content end
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