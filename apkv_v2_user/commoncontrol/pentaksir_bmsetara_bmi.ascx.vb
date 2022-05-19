Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class pentaksir_bmsetara_bmi1

    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String
    Dim strRet As String
    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Dim strKolejRecordID As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then

                ddlJenisBorang_list()
                ddlPP_list()
                ddlSidang_list()
                ddlBilik_list()
                ddlMasa_list()

                lblMsg.Text = ""

                strRet = BindData(datRespondent)

            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try
    End Sub

    Private Sub ddlJenisBorang_list()

        strSQL = "  SELECT value FROM kpmkv_pentaksir_bmsetara_setting WHERE Type = 'JenisBorang' ORDER BY value"

        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlJenisBorang.DataSource = ds
            ddlJenisBorang.DataTextField = "value"
            ddlJenisBorang.DataValueField = "value"
            ddlJenisBorang.DataBind()

            '--ALL
            'ddlPentaksir.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Sub ddlPP_list()

        strSQL = "SELECT UserID FROM kpmkv_users WHERE LoginID = '" & Session("LoginID") & "' AND Pwd = '" & Session("Password") & "'"
        Dim UserID As String = oCommon.getFieldValue(strSQL)

        strSQL = "  SELECT kpmkv_kolej.Nama, kpmkv_kolej.Kod 
                    FROM kpmkv_kolej
                    LEFT JOIN kpmkv_users ON kpmkv_users.RecordID = kpmkv_kolej.RecordID
                    WHERE kpmkv_users.UserID = '" & UserID & "'
                    AND kpmkv_kolej.Jenis = 'KPM'
                    ORDER BY kpmkv_kolej.Kod"

        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlPP.DataSource = ds
            ddlPP.DataTextField = "Kod"
            ddlPP.DataValueField = "Nama"
            ddlPP.DataBind()

            '--ALL
            'ddlPentaksir.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Sub ddlSidang_list()

        strSQL = "  SELECT value FROM kpmkv_pentaksir_bmsetara_setting WHERE Type = 'Sidang' ORDER BY value"

        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlSidang.DataSource = ds
            ddlSidang.DataTextField = "value"
            ddlSidang.DataValueField = "value"
            ddlSidang.DataBind()

            '--ALL
            'ddlPentaksir.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Sub ddlBilik_list()

        strSQL = "  SELECT value FROM kpmkv_pentaksir_bmsetara_setting WHERE Type = 'Bilik' ORDER BY idx"

        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlBilik.DataSource = ds
            ddlBilik.DataTextField = "value"
            ddlBilik.DataValueField = "value"
            ddlBilik.DataBind()

            '--ALL
            'ddlPentaksir.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Sub ddlMasa_list()

        strSQL = "  SELECT value FROM kpmkv_pentaksir_bmsetara_setting WHERE Type = 'Masa' ORDER BY idx"

        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlMasa.DataSource = ds
            ddlMasa.DataTextField = "value"
            ddlMasa.DataValueField = "value"
            ddlMasa.DataBind()

            '--ALL
            'ddlPentaksir.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click


    End Sub

    Private Sub btnBack2_Click(sender As Object, e As EventArgs) Handles btnBack2.Click

        Response.Redirect("admin.default.aspx")

    End Sub

    Private Function getSQL() As String

        strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Kod = '" & ddlPP.SelectedItem.Text.ToString & "' AND Nama = '" & ddlPP.SelectedValue & "'"
        Dim RecordID As String = oCommon.getFieldValue(strSQL)

        strSQL = "SELECT value FROM kpmkv_pentaksir_bmsetara_setting WHERE Type = 'Tahun'"
        Dim Tahun As String = oCommon.getFieldValue(strSQL)

        Dim tmpSQL As String = ""
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY AngkaGiliran, Nama ASC"

        '--not deleted
        tmpSQL = "SELECT id, Nama, MYKAD, AngkaGiliran FROM kpmkv_pentaksir_bmsetara_calon"
        strWhere = " WHERE KolejRecordID='" & RecordID & "' AND Tahun ='" & Tahun & "' AND StatusPrint_BMI = '0'"

        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        'Response.Write(getSQL)

        Return getSQL

    End Function

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

    Private Sub ddlJenisBorang_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlJenisBorang.SelectedIndexChanged
        strRet = BindData(datRespondent)
    End Sub

    Private Sub ddlPP_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPP.SelectedIndexChanged
        strRet = BindData(datRespondent)
    End Sub

    Private Sub datRespondent_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles datRespondent.PageIndexChanging
        datRespondent.PageIndex = e.NewPageIndex
        strRet = BindData(datRespondent)
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click

        If countChecked() = False Then

            divMsg.Attributes("class") = "error"
            lblMsg.Text = "Pilihan yang dibuat mestilah tidak melebihi 8 orang calon!"
            Exit Sub

        End If

        printBMI()

    End Sub

    Private Sub btnPrint2_Click(sender As Object, e As EventArgs) Handles btnPrint2.Click

        If countChecked() = False Then

            divMsg.Attributes("class") = "error"
            lblMsg.Text = "Pilihan yang dibuat mestilah tidak melebihi 8 orang calon!"
            Exit Sub

        End If

        printBMI()

    End Sub

    Private Sub printBMI()

        Dim myDocument As New Document(PageSize.A4.Rotate)

        Try
            HttpContext.Current.Response.ContentType = "application/pdf"
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=BorangMarkahInduk.pdf")
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)

            PdfWriter.GetInstance(myDocument, HttpContext.Current.Response.OutputStream)

            ''--draw spacing
            Dim imgdrawSpacing As String = Server.MapPath("~/img/spacing.png")
            Dim imgSpacing As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(imgdrawSpacing)
            imgSpacing.Alignment = iTextSharp.text.Image.LEFT_ALIGN  'left
            imgSpacing.Border = 0

            myDocument.Open()

            myDocument.NewPage()
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

            cetak = ""

            cell = New PdfPCell()
            Debug.Write(cetak)
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 7))
            myPara001.Alignment = Element.ALIGN_RIGHT
            cell.AddElement(myPara001)
            cell.Border = 0
            cell.VerticalAlignment = Element.ALIGN_RIGHT
            table.AddCell(cell)

            myDocument.Add(table)

            strSQL = "SELECT value FROM kpmkv_pentaksir_bmsetara_setting WHERE Type = 'Tahun'"
            Dim Tahun As String = oCommon.getFieldValue(strSQL)

            cetak = "LEMBAGA PEPERIKSAAN"
            cetak += Environment.NewLine & "KEMENTERIAN PENDIDIKAN MALAYSIA"
            cetak += Environment.NewLine & "BORANG MARKAH INDUK UJIAN BERTUTUR BAHASA MELAYU"
            cetak += Environment.NewLine & "SIJIL VOKASIONAL MALAYSIA TAHUN  " & Tahun
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
            myPara001.Alignment = Element.ALIGN_CENTER
            myDocument.Add(myPara001)
            Debug.WriteLine(cetak)

            ''PROFILE STARTS HERE

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            table = New PdfPTable(7)
            table.WidthPercentage = 100
            table.SetWidths({15, 5, 25, 20, 15, 5, 25})

            cell = New PdfPCell()
            cetak = Environment.NewLine & "KOD PUSAT"
            cetak += Environment.NewLine & "NAMA KOLEJ"
            cetak += Environment.NewLine
            cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 8)))
            cell.Border = 0
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = Environment.NewLine & ":"
            cetak += Environment.NewLine & ":"
            cetak += Environment.NewLine
            cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 8)))
            cell.Border = 0
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = Environment.NewLine & ddlPP.SelectedItem.Text.ToString
            cetak += Environment.NewLine & ddlPP.SelectedValue
            cetak += Environment.NewLine
            cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 8)))
            cell.Border = 0
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = Environment.NewLine
            cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 8)))
            cell.Border = 0
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = Environment.NewLine & "SIDANG / BILIK UJIAN"
            cetak += Environment.NewLine & "TARIKH / MASA"
            cetak += Environment.NewLine
            cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 8)))
            cell.Border = 0
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = Environment.NewLine & ":"
            cetak += Environment.NewLine & ":"
            cetak += Environment.NewLine
            cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 8)))
            cell.Border = 0
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = Environment.NewLine & "SIDANG " & ddlSidang.SelectedValue & " / " & ddlBilik.SelectedValue
            cetak += Environment.NewLine & txtTarikh.Text.ToUpper & " / " & ddlMasa.SelectedValue
            cetak += Environment.NewLine
            cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 8)))
            cell.Border = 0
            table.AddCell(cell)

            Debug.WriteLine(cetak)

            myDocument.Add(table)

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            table = New PdfPTable(16)
            table.WidthPercentage = 100
            table.SetWidths({3, 30, 3, 15, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 10})
            table.DefaultCell.Border = 0

            cell = New PdfPCell()
            cetak = "BIL"
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
            myPara001.Alignment = Element.ALIGN_CENTER
            cell.AddElement(myPara001)
            cell.Rowspan = 3
            cell.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = "NAMA CALON"
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
            myPara001.Alignment = Element.ALIGN_CENTER
            cell.AddElement(myPara001)
            cell.Border = Rectangle.BOX
            cell.Rowspan = 3
            cell.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = "L/P"
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
            myPara001.Alignment = Element.ALIGN_CENTER
            cell.AddElement(myPara001)
            cell.Border = Rectangle.BOX
            cell.Rowspan = 3
            cell.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = "ANGKA GILIRAN/NO. K.P"
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
            myPara001.Alignment = Element.ALIGN_CENTER
            cell.AddElement(myPara001)
            cell.Border = Rectangle.BOX
            cell.Rowspan = 3
            cell.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = "A. INDIVIDU (ANALITIK)"
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
            myPara001.Alignment = Element.ALIGN_CENTER
            cell.AddElement(myPara001)
            cell.Border = Rectangle.BOX
            cell.Colspan = 6
            cell.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = "B. KUMPULAN (HOLISTIK)"
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
            myPara001.Alignment = Element.ALIGN_CENTER
            cell.AddElement(myPara001)
            cell.Border = Rectangle.BOX
            cell.Colspan = 5
            cell.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = "JUMLAH BESAR"
            cetak += Environment.NewLine & "MARKAH AKHIR"
            cetak += Environment.NewLine & "[70 markah]"
            cetak += Environment.NewLine & "(A+B)"
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
            myPara001.Alignment = Element.ALIGN_CENTER
            cell.AddElement(myPara001)
            cell.Border = Rectangle.BOX
            cell.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(cell)

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''
            cell = New PdfPCell()
            cetak = "Tatabahasa dan Kosa Kata"
            cetak += Environment.NewLine & "[10 markah]"
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
            myPara001.Alignment = Element.ALIGN_CENTER
            cell.Rotation = 270
            cell.AddElement(myPara001)
            cell.Border = Rectangle.BOX
            cell.Rowspan = 2
            cell.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = "Sebutan, Intonasi dan Nada"
            cetak += Environment.NewLine & "[10 markah]"
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
            myPara001.Alignment = Element.ALIGN_CENTER
            cell.Rotation = 270
            cell.AddElement(myPara001)
            cell.Border = Rectangle.BOX
            cell.Rowspan = 2
            cell.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = "Kefasihan dan Kelancaran"
            cetak += Environment.NewLine & "[10 markah]"
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
            myPara001.Alignment = Element.ALIGN_CENTER
            cell.Rotation = 270
            cell.AddElement(myPara001)
            cell.Border = Rectangle.BOX
            cell.Rowspan = 2
            cell.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = "Idea dan Bermakna"
            cetak += Environment.NewLine & "[10 markah]"
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
            myPara001.Alignment = Element.ALIGN_CENTER
            cell.Rotation = 270
            cell.AddElement(myPara001)
            cell.Border = Rectangle.BOX
            cell.Rowspan = 2
            cell.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = "Jumlah Markah"
            cetak += Environment.NewLine & "[40 markah]"
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, Font.BOLD))
            myPara001.Alignment = Element.ALIGN_CENTER
            cell.Rotation = 270
            cell.AddElement(myPara001)
            cell.Border = Rectangle.BOX
            cell.Rowspan = 2
            cell.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = "Markah Penyelarasan"
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, Font.BOLD))
            myPara001.Alignment = Element.ALIGN_CENTER
            cell.Rotation = 270
            cell.AddElement(myPara001)
            cell.Border = Rectangle.BOX
            cell.Rowspan = 2
            cell.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = "Tatabahasa dan Kosa Kata"
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
            myPara001.Alignment = Element.ALIGN_CENTER
            cell.Rotation = 270
            cell.AddElement(myPara001)
            cell.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = "Sebutan, Intonasi"
            cetak += Environment.NewLine & "dan Nada"
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
            myPara001.Alignment = Element.ALIGN_CENTER
            cell.Rotation = 270
            cell.AddElement(myPara001)
            cell.Rowspan = 1
            cell.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = "Kefasihan dan"
            cetak += Environment.NewLine & "Kelancaran"
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
            myPara001.Alignment = Element.ALIGN_CENTER
            cell.Rotation = 270
            cell.AddElement(myPara001)
            cell.Rowspan = 1
            cell.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = "Idea dan Bermakna"
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
            myPara001.Alignment = Element.ALIGN_CENTER
            cell.Rotation = 270
            cell.AddElement(myPara001)
            cell.Rowspan = 1
            cell.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = "Markah Penyelarasan"
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, Font.BOLD))
            myPara001.Alignment = Element.ALIGN_CENTER
            cell.Rotation = 270
            cell.AddElement(myPara001)
            cell.Border = Rectangle.BOX
            cell.Rowspan = 2
            cell.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = ""
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
            myPara001.Alignment = Element.ALIGN_CENTER
            cell.Rotation = 270
            cell.AddElement(myPara001)
            cell.Border = Rectangle.BOX
            cell.Rowspan = 2
            cell.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(cell)

            cell = New PdfPCell()
            cetak = "Jumlah Markah"
            cetak += Environment.NewLine & "[30 markah] "
            myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, Font.BOLD))
            myPara001.Alignment = Element.ALIGN_CENTER
            cell.AddElement(myPara001)
            cell.Border = Rectangle.BOX
            cell.Colspan = 4
            cell.VerticalAlignment = Element.ALIGN_TOP
            table.AddCell(cell)


            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' MAKLUMAT CALON

            Dim count As Integer = 0
            Dim countLoop As Integer = 0

            For i As Integer = 0 To datRespondent.Rows.Count - 1

                Dim row As GridViewRow = datRespondent.Rows(i)
                Dim chkCalon As CheckBox = CType(row.FindControl("chkPrint"), CheckBox)

                If chkCalon.Checked = True Then

                    count += 1
                    countLoop += 1

                    Dim dataKey As String = datRespondent.DataKeys(i).Value.ToString

                    strSQL = "  UPDATE kpmkv_pentaksir_bmsetara_calon 
                                SET StatusPrint_BMI = '1'
                                WHERE id = '" & dataKey & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)


                    strSQL = "  SELECT PelajarID FROM kpmkv_pentaksir_bmsetara_calon WHERE id = '" & dataKey & "'"
                    Dim PelajarID As String = oCommon.getFieldValue(strSQL)

                    cell = New PdfPCell()
                    cetak = count
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    strSQL = "  SELECT Nama FROM kpmkv_pelajar WHERE PelajarID = '" & PelajarID & "'"
                    Dim Nama As String = oCommon.getFieldValue(strSQL)

                    cell = New PdfPCell()
                    cetak = Nama
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_LEFT
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    strSQL = "  SELECT Jantina FROM kpmkv_pelajar WHERE PelajarID = '" & PelajarID & "'"
                    Dim Jantina As String = oCommon.getFieldValue(strSQL)

                    If Jantina = "LELAKI" Then
                        Jantina = "L"
                    Else
                        Jantina = "P"
                    End If

                    cell = New PdfPCell()
                    cetak = Jantina
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    strSQL = "  SELECT MYKAD FROM kpmkv_pelajar WHERE PelajarID = '" & PelajarID & "'"
                    Dim MYKAD As String = oCommon.getFieldValue(strSQL)

                    strSQL = "  SELECT AngkaGiliran FROM kpmkv_pelajar WHERE PelajarID = '" & PelajarID & "'"
                    Dim AngkaGiliran As String = oCommon.getFieldValue(strSQL)

                    cell = New PdfPCell()
                    cetak = AngkaGiliran
                    cetak += Environment.NewLine & MYKAD
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.Colspan = 4
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                End If

            Next

            If countLoop < 8 Then

                For i As Integer = 0 To 8 - countLoop - 1

                    Dim row As GridViewRow = datRespondent.Rows(i)
                    Dim chkCalon As CheckBox = CType(row.FindControl("chkPrint"), CheckBox)

                    count += 1

                    cell = New PdfPCell()
                    cetak = count
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)


                    cell = New PdfPCell()
                    cetak = ""
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_LEFT
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    cetak += Environment.NewLine & ""
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.Colspan = 4
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    myPara001 = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(myPara001)
                    cell.Border = Rectangle.BOX
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE
                    table.AddCell(cell)

                Next

            End If

            myDocument.Add(table)


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            myDocument.Close()

            HttpContext.Current.Response.Write(myDocument)
            HttpContext.Current.Response.End()


        Catch ex As Exception


        End Try

    End Sub

    Private Function countChecked() As Boolean

        Dim count As Integer = 0

        For i As Integer = 0 To datRespondent.Rows.Count - 1

            Dim row As GridViewRow = datRespondent.Rows(i)
            Dim chkCalon As CheckBox = CType(row.FindControl("chkPrint"), CheckBox)

            If chkCalon.Checked = True Then

                count += 1

            End If

        Next

        If count > 8 Then

            Return False

        Else

            Return True

        End If

    End Function

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click

        strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Kod = '" & ddlPP.SelectedItem.Text.ToString & "' AND Nama = '" & ddlPP.SelectedValue & "'"
        Dim RecordID As String = oCommon.getFieldValue(strSQL)

        strSQL = "SELECT value FROM kpmkv_pentaksir_bmsetara_setting WHERE Type = 'Tahun'"
        Dim Tahun As String = oCommon.getFieldValue(strSQL)

        strSQL = "  UPDATE kpmkv_pentaksir_bmsetara_calon
                    SET StatusPrint_BMI = '0'
                    WHERE KolejRecordID='" & RecordID & "' AND Tahun ='" & Tahun & "' AND StatusPrint_BMI = '1'"
        strRet = oCommon.ExecuteSQL(strSQL)

        If strRet = "0" Then
            lblMsg.Text = "Senarai calon berjaya direset!"
        Else
            lblMsg.Text = "Senarai calon tidak berjaya direset!"

        End If

        strRet = BindData(datRespondent)

    End Sub

    Private Sub btnReset2_Click(sender As Object, e As EventArgs) Handles btnReset2.Click

        strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Kod = '" & ddlPP.SelectedItem.Text.ToString & "' AND Nama = '" & ddlPP.SelectedValue & "'"
        Dim RecordID As String = oCommon.getFieldValue(strSQL)

        strSQL = "SELECT value FROM kpmkv_pentaksir_bmsetara_setting WHERE Type = 'Tahun'"
        Dim Tahun As String = oCommon.getFieldValue(strSQL)

        strSQL = "  UPDATE kpmkv_pentaksir_bmsetara_calon
                    SET StatusPrint_BMI = '0'
                    WHERE KolejRecordID='" & RecordID & "' AND Tahun ='" & Tahun & "' AND StatusPrint_BMI = '1'"
        strRet = oCommon.ExecuteSQL(strSQL)

        If strRet = "0" Then
            lblMsg.Text = "Senarai calon berjaya direset!"
        Else
            lblMsg.Text = "Senarai calon tidak berjaya direset!"

        End If

        strRet = BindData(datRespondent)

    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        strRet = BindData(datRespondent)
    End Sub

    Private Sub btnRefresh2_Click(sender As Object, e As EventArgs) Handles btnRefresh2.Click
        strRet = BindData(datRespondent)
    End Sub
End Class

