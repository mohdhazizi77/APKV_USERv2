Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class roasterSJ1251

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


                'kpmkv_tahun_1_list()
                'ddlTahun_1.Text = Now.Year

                'kpmkv_tahun_2_list()
                'ddlTahun_Semasa.Text = Now.Year

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

    'Private Sub kpmkv_tahun_1_list()
    '    strSQL = "SELECT Tahun FROM kpmkv_tahun ORDER BY TahunID"
    '    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    '    Dim objConn As SqlConnection = New SqlConnection(strConn)
    '    Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

    '    Try
    '        Dim ds As DataSet = New DataSet
    '        sqlDA.Fill(ds, "AnyTable")

    '        ddlTahun_1.DataSource = ds
    '        ddlTahun_1.DataTextField = "Tahun"
    '        ddlTahun_1.DataValueField = "Tahun"
    '        ddlTahun_1.DataBind()

    '    Catch ex As Exception

    '    Finally
    '        objConn.Dispose()
    '    End Try

    'End Sub
    'Private Sub kpmkv_tahun_2_list()
    '    strSQL = "SELECT Tahun FROM kpmkv_tahun ORDER BY TahunID"
    '    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    '    Dim objConn As SqlConnection = New SqlConnection(strConn)
    '    Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

    '    Try
    '        Dim ds As DataSet = New DataSet
    '        sqlDA.Fill(ds, "AnyTable")

    '        ddlTahun_Semasa.DataSource = ds
    '        ddlTahun_Semasa.DataTextField = "Tahun"
    '        ddlTahun_Semasa.DataValueField = "Tahun"
    '        ddlTahun_Semasa.DataBind()

    '    Catch ex As Exception

    '    Finally
    '        objConn.Dispose()
    '    End Try

    'End Sub

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
        strWhere = " WHERE kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.StatusID='2' AND kpmkv_pelajar.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_pelajar.Semester ='4'"

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
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=SlipRosterSJ.pdf")
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


            Dim a As Integer = 1
            Dim strKey As String

            '1'--start here
            strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
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
                    If a = 1 Then

                        Dim myPara01 As New Paragraph("KEMENTERIAN PENDIDIKAN MALAYSIA", FontFactory.GetFont("Arial", 8, Font.BOLD))
                        myPara01.Alignment = Element.ALIGN_CENTER
                        myDocument.Add(myPara01)

                        myDocument.Add(imgSpacing)
                        Dim myPara02 As New Paragraph("KEPUTUSAN PENTAKSIRAN KOLEJ VOKASIONAL", FontFactory.GetFont("Arial", 8, Font.NORMAL))
                        myPara02.Alignment = Element.ALIGN_CENTER
                        myDocument.Add(myPara02)

                        Dim myPara03 As New Paragraph("SEJARAH KOLEJ VOKASIONAL KOD 1251 TAHUN " & ddlTahun.Text, FontFactory.GetFont("Arial", 8, Font.NORMAL))
                        myPara03.Alignment = Element.ALIGN_CENTER
                        myDocument.Add(myPara03)

                        Dim myPara04 As New Paragraph("INSTITUSI: " & strKolejnama, FontFactory.GetFont("Arial", 8, Font.NORMAL))
                        myPara04.Alignment = Element.ALIGN_LEFT
                        myDocument.Add(myPara04)
                        myDocument.Add(imgSpacing)

                        'header1
                        Dim myTable As New PdfPTable(5)
                        myTable.WidthPercentage = 100 ' Table size is set to 100% of the page
                        myTable.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                        'myTable.HorizontalAlignment = Rectangle.NO_BORDER
                        Dim intTblWidth() As Integer = {5, 40, 9, 9, 5}
                        myTable.SetWidths(intTblWidth)

                        Dim Cell1Hdr As New PdfPCell(New Phrase("BIL", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        Cell1Hdr.Border = Rectangle.NO_BORDER
                        myTable.AddCell(Cell1Hdr)

                        Dim CellHdr1 As New PdfPCell(New Phrase("NAMA", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        CellHdr1.Border = Rectangle.NO_BORDER
                        myTable.AddCell(CellHdr1)

                        Dim Cell2Hdr As New PdfPCell(New Phrase("NO KP", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        Cell2Hdr.Border = Rectangle.NO_BORDER
                        myTable.AddCell(Cell2Hdr)

                        Dim CellHdr2 As New PdfPCell(New Phrase("ANGKAGILIRAN", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        CellHdr2.Border = Rectangle.NO_BORDER
                        myTable.AddCell(CellHdr2)

                        Dim Cell3Hdr As New PdfPCell(New Phrase("SJ 1251", FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        Cell3Hdr.Border = Rectangle.NO_BORDER
                        myTable.AddCell(Cell3Hdr)
                        myDocument.Add(myTable)

                    End If

                    Dim strNama As String = ""
                    Dim strMykad As String = ""
                    Dim strAngkaGiliran As String = ""
                    Dim strPointer As String = ""
                    Dim strStudentID As String = ""
                    Dim strGredSJ As String = ""
                    ''--Tokenid,ClassCode,Q001Remarks
                    strSQL = "SELECT Nama FROM kpmkv_pelajar WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strNama = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT Mykad FROM kpmkv_pelajar WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strMykad = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT AngkaGiliran FROM kpmkv_pelajar WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strAngkaGiliran = oCommon.getFieldValue(strSQL)

                    ''--Pointer SJ 
                    strSQL = "SELECT PointerSJSetara FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strPointer = oCommon.getFieldValue(strSQL)

                    ''--Gred SJ 
                    strSQL = "SELECT GredSJSetara FROM kpmkv_pelajar_markah WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    Dim strGredSJSetara As String = oCommon.getFieldValue(strSQL)

                    ''--Gred SJ 
                    If strPointer = "-1" Then
                        strGredSJ = "T"
                    Else
                        strSQL = "SELECT TOP ( 1 ) Gred FROM  kpmkv_gred_sejarah WHERE '" & Integer.Parse(strPointer) & "' BETWEEN MarkahFrom AND MarkahTo AND Tahun='" & ddlTahun.Text & "' AND Sesi='" & chkSesi.Text & "' "
                        strGredSJ = oCommon.getFieldValue(strSQL)
                    End If

                    ''get kompetensi
                    strSQL = "SELECT Status FROM kpmkv_gred_sejarah WHERE Gred = '" & strGredSJ & "' AND Tahun = '" & ddlTahun.Text & "' AND Sesi = '" & chkSesi.Text & "'"
                    Dim strStatus As String = oCommon.getFieldValue(strSQL)

                    ''change on 17082016
                    'strSQL = "UPDATE kpmkv_pelajar_markah SET GredSJSetara='" & strStatus & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    'strRet = oCommon.ExecuteSQL(strSQL)

                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    If Not strGredSJ = "T" Then

                        Dim myTablerow2 As New PdfPTable(5)
                        myTablerow2.WidthPercentage = 100 ' Table size is set to 100% of the page
                        myTablerow2.HorizontalAlignment = 1 '//0=Left, 1=Centre, 2=Right
                        'myTable.HorizontalAlignment = Rectangle.NO_BORDER
                        Dim intTblWidth2() As Integer = {5, 40, 9, 9, 5}
                        myTablerow2.SetWidths(intTblWidth2)

                        Dim Cell1Hdr1 As New PdfPCell(New Phrase("" & i + 1, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        Cell1Hdr1.Border = Rectangle.NO_BORDER
                        myTablerow2.AddCell(Cell1Hdr1)

                        Dim Cell1Hdr10 As New PdfPCell(New Phrase("" & strNama, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        Cell1Hdr10.Border = Rectangle.NO_BORDER
                        myTablerow2.AddCell(Cell1Hdr10)

                        Dim Cell2Hdr2 As New PdfPCell(New Phrase("" & strMykad, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        Cell2Hdr2.Border = Rectangle.NO_BORDER
                        myTablerow2.AddCell(Cell2Hdr2)

                        Dim Cell2Hdr20 As New PdfPCell(New Phrase("" & strAngkaGiliran, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        Cell2Hdr20.Border = Rectangle.NO_BORDER
                        myTablerow2.AddCell(Cell2Hdr20)

                        Dim Cell3Hdr30 As New PdfPCell(New Phrase("" & strGredSJSetara, FontFactory.GetFont("Arial", 8, Font.NORMAL)))
                        Cell3Hdr30.Border = Rectangle.NO_BORDER
                        myTablerow2.AddCell(Cell3Hdr30)
                        myDocument.Add(myTablerow2)

                    End If



                End If 'is check box

                a = a + 1

                If a >= 26 Then
                    myDocument.NewPage()
                    a = 1
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

    'Protected Sub btnPrintSlip_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrintSlip.Click
    '    Dim myDocument As New Document(PageSize.A4)

    '    Try
    '        HttpContext.Current.Response.ContentType = "application/pdf"
    '        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=SlipSJSetara1251.pdf")
    '        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)

    '        PdfWriter.GetInstance(myDocument, HttpContext.Current.Response.OutputStream)

    '        myDocument.Open()

    '        ''--draw spacing
    '        Dim imgdrawSpacing As String = Server.MapPath("~/img/spacing.png")
    '        Dim imgSpacing As Image = Image.GetInstance(imgdrawSpacing)
    '        imgSpacing.Alignment = Image.LEFT_ALIGN  'left
    '        imgSpacing.Border = 0

    '        '1'--start here
    '        strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
    '        Dim strKolejnama As String = oCommon.getFieldValue(strSQL)

    '        'kolejnegeri
    '        strSQL = "SELECT Negeri FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
    '        Dim strKolejnegeri As String = oCommon.getFieldValue(strSQL)


    '        strSQL = "SELECT kpmkv_pelajar.PelajarID,kpmkv_pelajar.MYKAD,kpmkv_pelajar.Nama, kpmkv_pelajar.AngkaGiliran, "
    '        strSQL += " kpmkv_kursus.KodKursus,kpmkv_kluster.NamaKluster, kpmkv_kursus.NamaKursus,kpmkv_pelajar.isBMTahun"
    '        strSQL += " FROM  kpmkv_pelajar LEFT OUTER JOIN kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID "
    '        strSQL += " LEFT OUTER JOIN kpmkv_kluster ON kpmkv_kursus.KlusterID=kpmkv_kluster.KlusterID"
    '        strSQL += " LEFT OUTER JOIN kpmkv_status ON kpmkv_pelajar.StatusID = kpmkv_status.StatusID"
    '        strSQL += " LEFT OUTER JOIN kpmkv_kelas ON kpmkv_pelajar.KelasID = kpmkv_kelas.KelasID"
    '        strSQL += " WHERE kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.StatusID='2' "
    '        strSQL += " And kpmkv_pelajar.KolejRecordID ='" & lblKolejID.Text & "' "
    '        strSQL += " AND kpmkv_pelajar.Semester ='4'"

    '        If Not ddlTahun.Text = "" Then
    '            strSQL += " AND kpmkv_pelajar.Tahun ='" & ddlTahun.Text & "'"
    '        End If

    '        If Not chkSesi.Text = "" Then
    '            strSQL += " AND kpmkv_pelajar.Sesi ='" & chkSesi.Text & "'"
    '        End If

    '        If Not ddlKodKursus.Text = "" Then
    '            strSQL += " AND kpmkv_pelajar.KursusID ='" & ddlKodKursus.SelectedValue & "'"
    '        End If

    '        If Not ddlNamaKelas.Text = "" Then
    '            strSQL += " AND kpmkv_pelajar.KelasID ='" & ddlNamaKelas.SelectedValue & "'"
    '        End If

    '        If Not txtNama.Text.Length = 0 Then
    '            strSQL += " AND kpmkv_pelajar.Nama LIKE '%" & oCommon.FixSingleQuotes(txtNama.Text) & "%'"
    '        End If

    '        If Not txtMYKAD.Text.Length = 0 Then
    '            strSQL += " AND kpmkv_pelajar.MYKAD='" & oCommon.FixSingleQuotes(txtMYKAD.Text) & "'"
    '        End If

    '        If Not txtAngkaGiliran.Text.Length = 0 Then
    '            strSQL += " AND kpmkv_pelajar.AngkaGiliran LIKE '%" & oCommon.FixSingleQuotes(txtAngkaGiliran.Text) & "%'"
    '        End If

    '        strRet = oCommon.ExecuteSQL(strSQL)

    '        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)
    '        Dim ds As DataSet = New DataSet
    '        sqlDA.Fill(ds, "AnyTable")

    '        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1

    '            Dim strkey As String = ds.Tables(0).Rows(i).Item(0).ToString

    '            Dim strmykad As String = ds.Tables(0).Rows(i).Item(1).ToString
    '            Dim strname As String = ds.Tables(0).Rows(i).Item(2).ToString
    '            Dim strag As String = ds.Tables(0).Rows(i).Item(3).ToString
    '            Dim strkodKursus As String = ds.Tables(0).Rows(i).Item(4).ToString
    '            Dim strbidang As String = ds.Tables(0).Rows(i).Item(5).ToString
    '            Dim strprogram As String = ds.Tables(0).Rows(i).Item(6).ToString
    '            Dim strTahun As String = ds.Tables(0).Rows(i).Item(7).ToString
    '            ''getting data end

    '            Dim table As New PdfPTable(3)
    '            table.WidthPercentage = 100
    '            table.SetWidths({42, 16, 42})
    '            table.DefaultCell.Border = 0


    '            myDocument.Add(table)

    '            Dim myPara001 As New Paragraph("LEMBAGA PEPERIKSAAN", FontFactory.GetFont("Arial", 10, Font.BOLD))
    '            myPara001.Alignment = Element.ALIGN_CENTER
    '            myDocument.Add(myPara001)

    '            Dim myPara01 As New Paragraph("KEMENTERIAN PENDIDIKAN MALAYSIA", FontFactory.GetFont("Arial", 10, Font.BOLD))
    '            myPara01.Alignment = Element.ALIGN_CENTER
    '            myDocument.Add(myPara01)

    '            myDocument.Add(imgSpacing)
    '            Dim myPara02 As New Paragraph("SLIP KEPUTUSAN SEJARAH 1251", FontFactory.GetFont("Tw Cen Mt", 12, Font.NORMAL))
    '            myPara02.Alignment = Element.ALIGN_CENTER
    '            myDocument.Add(myPara02)

    '            Dim myPara03 As New Paragraph("TAHUN " & strTahun, FontFactory.GetFont("Tw Cen Mt", 12, Font.NORMAL))
    '            myPara03.Alignment = Element.ALIGN_CENTER
    '            myDocument.Add(myPara03)

    '            myDocument.Add(imgSpacing)

    '            ''PROFILE STARTS HERE

    '            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    '            table = New PdfPTable(2)

    '            table.WidthPercentage = 100
    '            table.SetWidths({30, 70})

    '            Dim cell = New PdfPCell()
    '            Dim cetak = Environment.NewLine & "NAMA"
    '            cetak += Environment.NewLine & "NO.KAD PENGENALAN"
    '            cetak += Environment.NewLine & "ANGKA GILIRAN"
    '            cetak += Environment.NewLine & "INSTITUSI"
    '            cetak += Environment.NewLine & "NAMA BIDANG"
    '            cetak += Environment.NewLine & "PROGRAM"
    '            cetak += Environment.NewLine & ""

    '            cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
    '            cell.Border = 0
    '            table.AddCell(cell)

    '            cell = New PdfPCell()
    '            cetak = Environment.NewLine & ": " & strname
    '            cetak += Environment.NewLine & ": " & strmykad
    '            cetak += Environment.NewLine & ": " & strag
    '            cetak += Environment.NewLine & ": " & strKolejnama
    '            cetak += Environment.NewLine & ": " & strbidang
    '            cetak += Environment.NewLine & ": " & strprogram & " (" & strkodKursus & ")"
    '            cetak += Environment.NewLine & " "

    '            cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
    '            cell.Border = 0
    '            table.AddCell(cell)
    '            Debug.WriteLine(cetak)

    '            myDocument.Add(table)

    '            ''profile ends here
    '            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    '            table = New PdfPTable(4)
    '            table.WidthPercentage = 100
    '            table.SetWidths({30, 42, 18, 10})

    '            cell = New PdfPCell()
    '            cetak = "KOD"
    '            cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
    '            cell.Border = 0
    '            table.AddCell(cell)

    '            cell = New PdfPCell()
    '            cetak = "MATA PELAJARAN"
    '            cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
    '            cell.Border = 0
    '            table.AddCell(cell)

    '            cell = New PdfPCell()
    '            cetak = "KOMPETENSI"
    '            cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
    '            cell.Border = 0
    '            table.AddCell(cell)

    '            cell = New PdfPCell()
    '            cetak = ""
    '            cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
    '            cell.Border = 0
    '            table.AddCell(cell)

    '            myDocument.Add(table)

    '            strSQL = "select GredSJSetara from kpmkv_pelajar_markah where PelajarID = '" & strkey & "'"
    '            strSQL += " AND Tahun='" & ddlTahun.SelectedValue & "'"
    '            strSQL += " AND Sesi='" & chkSesi.SelectedValue & "'"
    '            Dim strgred As String = oCommon.getFieldValue(strSQL)

    '            strSQL = " SELECT Kompetensi FROM kpmkv_gred_sejarah WHERE Gred = '" & strgred & "'"
    '            strSQL += " AND Tahun='" & ddlTahun.SelectedValue & "'"
    '            strSQL += " AND Sesi='" & chkSesi.SelectedValue & "'"
    '            Dim strKompetensi As String = oCommon.getFieldValue(strSQL)

    '            table = New PdfPTable(4)
    '            table.WidthPercentage = 100
    '            table.SetWidths({30, 42, 18, 10})
    '            table.DefaultCell.Border = 0

    '            cell = New PdfPCell()
    '            cetak = ""
    '            cetak += "1251"
    '            cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
    '            cell.Border = 0
    '            table.AddCell(cell)

    '            cell = New PdfPCell()
    '            cetak = ""
    '            cetak += "SEJARAH"
    '            cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
    '            cell.Border = 0
    '            table.AddCell(cell)

    '            cell = New PdfPCell()
    '            cetak = ""
    '            cetak += strKompetensi
    '            cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
    '            cell.Border = 0
    '            table.AddCell(cell)

    '            cell = New PdfPCell()
    '            cetak = ""
    '            cetak += ""
    '            cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
    '            cell.Border = 0
    '            table.AddCell(cell)

    '            Debug.WriteLine(cetak)
    '            myDocument.Add(table)


    '            myDocument.Add(imgSpacing)
    '            myDocument.Add(imgSpacing)
    '            myDocument.Add(imgSpacing)
    '            myDocument.Add(imgSpacing)
    '            myDocument.Add(imgSpacing)
    '            myDocument.Add(imgSpacing)
    '            myDocument.Add(imgSpacing)
    '            myDocument.Add(imgSpacing)
    '            myDocument.Add(imgSpacing)
    '            myDocument.Add(imgSpacing)
    '            myDocument.Add(imgSpacing)
    '            myDocument.Add(imgSpacing)
    '            myDocument.Add(imgSpacing)
    '            myDocument.Add(imgSpacing)
    '            myDocument.Add(imgSpacing)
    '            myDocument.Add(imgSpacing)
    '            myDocument.Add(imgSpacing)
    '            myDocument.Add(imgSpacing)
    '            myDocument.Add(New Paragraph("TARIKH: " & ddlHari.Text & "/" & ddlBulan.Text & "/" & ddlTahun_1.Text & "                                                                                                                                                      PENGARAH PEPERIKSAAN", FontFactory.GetFont("Arial", 8, Font.BOLD)))
    '            'Dim myPengarah As New Paragraph("" & strKolejnama, FontFactory.GetFont("Arial", 8, Font.BOLD))
    '            'myPengarah.Alignment = Element.ALIGN_RIGHT
    '            'myDocument.Add(myPengarah)

    '            myDocument.Add(imgSpacing)
    '            myDocument.Add(imgSpacing)
    '            Dim myslip As New Paragraph("Slip ini adalah cetakan komputer, tandatangan tidak diperlukan", FontFactory.GetFont("Arial", 8, Font.ITALIC))
    '            myslip.Alignment = Element.ALIGN_CENTER
    '            myDocument.Add(myslip)
    '            myDocument.NewPage()
    '            ''--content end


    '            myDocument.NewPage()


    '        Next

    '        myDocument.Close()

    '        HttpContext.Current.Response.Write(myDocument)
    '        HttpContext.Current.Response.End()

    '    Catch ex As Exception

    '    End Try
    'End Sub
End Class