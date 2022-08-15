Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Imports RKLib.ExportData
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class slip_keputusan_ulangan1
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim oCommon2 As New CommonFunction2
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

                kpmkv_tahun_1_list()
                ddlTahun_1.Text = Now.Year

                'kpmkv_tahun_peperiksaan_list()


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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        lblMsg.Text = ""
        strRet = BindData(datRespondent)
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

        strSQL = "SELECT RecordID FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
        Dim strRecordID As String = oCommon.getFieldValue(strSQL)

        '--not deleted
        tmpSQL = "  SELECT DISTINCT b.PelajarID, b.Tahun, b.IsBMTahun, b.Semester, b.Sesi, b.Nama, b.MYKAD, b.AngkaGiliran FROM APKV.dbo.kpmkv_markah_bmsj_setara a
LEFT JOIN APKV.dbo.kpmkv_pelajar b ON b.PelajarID = a.PelajarID
LEFT JOIN APKV.dbo.kpmkv_kursus c ON c.KursusID = b.KursusID
LEFT JOIN APKV.dbo.kpmkv_kluster d ON d.KlusterID = c.KlusterID
LEFT JOIN APKV.dbo.kpmkv_kelas e ON e.KelasID = b.KelasID
LEFT JOIN APKV.dbo.kpmkv_kolej f ON f.RecordID = a.KolejRecordID
WHERE a.KolejRecordID = '" & strRecordID & "'
AND a.IsAKATahun = '" & ddlTahun.Text & "'
AND a.MataPelajaran = '" & ddlMatapelajaran.SelectedItem.Text & "'
AND a.PelajarID IN
(SELECT a.PelajarID FROM kpmkv_svmu a
LEFT JOIN kpmkv_svmu_calon b ON b.svmu_id = a.svmu_id
WHERE a.DatabaseName = 'APKV' AND b.Status = 'DISAHKAN')
UNION
SELECT DISTINCT b.PelajarID, b.Tahun, b.IsBMTahun, b.Semester, b.Sesi, b.Nama, b.MYKAD, b.AngkaGiliran FROM APKV.dbo.kpmkv_markah_bmsj_setara a
LEFT JOIN KPMKV.dbo.kpmkv_pelajar b ON b.PelajarID = a.PelajarID
LEFT JOIN KPMKV.dbo.kpmkv_kursus c ON c.KursusID = b.KursusID
LEFT JOIN KPMKV.dbo.kpmkv_kluster d ON d.KlusterID = c.KlusterID
LEFT JOIN KPMKV.dbo.kpmkv_kelas e ON e.KelasID = b.KelasID
LEFT JOIN KPMKV.dbo.kpmkv_kolej f ON f.RecordID = a.KolejRecordID
WHERE a.KolejRecordID = '" & strRecordID & "'
AND a.IsAKATahun = '" & ddlTahun.Text & "'
AND a.MataPelajaran = '" & ddlMatapelajaran.SelectedItem.Text & "'
AND a.PelajarID IN
(SELECT a.PelajarID FROM kpmkv_svmu a
LEFT JOIN kpmkv_svmu_calon b ON b.svmu_id = a.svmu_id
WHERE a.DatabaseName = 'KPMKV' AND b.Status = 'DISAHKAN')
ORDER BY b.Nama ASC"

        getSQL = tmpSQL
        ''--debug
        'Response.Write(getSQL)

        Return getSQL

    End Function

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click
        Dim myDocument As New Document(PageSize.A4)

        Try
            HttpContext.Current.Response.ContentType = "application/pdf"
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=SlipBM1104danSJA05401.pdf")
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)

            PdfWriter.GetInstance(myDocument, HttpContext.Current.Response.OutputStream)

            myDocument.Open()

            ''--draw spacing
            Dim imgdrawSpacing As String = Server.MapPath("~/img/spacing.png")
            Dim imgSpacing As Image = Image.GetInstance(imgdrawSpacing)
            imgSpacing.Alignment = Image.LEFT_ALIGN  'left
            imgSpacing.Border = 0

            strSQL = "SELECT RecordID FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
            Dim strRecordID As String = oCommon.getFieldValue(strSQL)

            '1'--start here
            strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
            Dim strKolejnama As String = oCommon.getFieldValue(strSQL)

            'kolejnegeri
            strSQL = "SELECT Negeri FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
            Dim strKolejnegeri As String = oCommon.getFieldValue(strSQL)

            Dim strKolejLength As String = strKolejnama + "," + strKolejnegeri
            Dim strtotalLength As Integer = strKolejLength.Length

            For i As Integer = 0 To datRespondent.Rows.Count - 1

                Dim cb As CheckBox = datRespondent.Rows(i).FindControl("chkSelect")

                If cb.Checked = True Then

                    Dim strPelajarID As String = datRespondent.DataKeys(i).Value.ToString

                    strSQL = "SELECT DatabaseName FROM kpmkv_svmu WHERE PelajarID = '" & strPelajarID & "'"
                    Dim DatabaseName As String = oCommon.getFieldValue(strSQL)

                    If DatabaseName = "APKV" Then

                        strSQL = "  SELECT a.Nama, a.MYKAD, a.AngkaGiliran, b.KodKursus, b.NamaKursus, c.NamaKluster, d.NamaKelas FROM kpmkv_pelajar a
LEFT JOIN kpmkv_kursus b ON b.KursusID = a.KursusID
LEFT JOIN kpmkv_kluster c ON c.klusterID = b.KlusterID
LEFT JOIN kpmkv_kelas d ON d.KelasID = a.KelasID
WHERE a.PelajarID = '" & strPelajarID & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                    Else

                        strSQL = "  SELECT a.Nama, a.MYKAD, a.AngkaGiliran, b.KodKursus, b.NamaKursus, c.NamaKluster, d.NamaKelas FROM kpmkv_pelajar a
LEFT JOIN kpmkv_kursus b ON b.KursusID = a.KursusID
LEFT JOIN kpmkv_kluster c ON c.klusterID = b.KlusterID
LEFT JOIN kpmkv_kelas d ON d.KelasID = a.KelasID
WHERE a.PelajarID = '" & strPelajarID & "'"
                        strRet = oCommon2.getFieldValueEx(strSQL)

                    End If

                    Dim Pelajar As Array
                    Pelajar = strRet.Split("|")

                    Dim strNama As String = Pelajar(0)
                    Dim strMykad As String = Pelajar(1)
                    Dim strAngkaGiliran As String = Pelajar(2)
                    Dim strKodKursus As String = Pelajar(3)
                    Dim strNamaKursus As String = Pelajar(4)
                    Dim strNamaKluster As String = Pelajar(5)
                    Dim strNamaKelas As String = Pelajar(6)

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    Dim table As New PdfPTable(3)
                    table.WidthPercentage = 100
                    table.SetWidths({42, 16, 42})
                    table.DefaultCell.Border = 0

                    myDocument.Add(table)

                    Dim myPara001 As New Paragraph("LEMBAGA PEPERIKSAAN", FontFactory.GetFont("Arial", 10, Font.BOLD))
                    myPara001.Alignment = Element.ALIGN_CENTER
                    myDocument.Add(myPara001)

                    Dim myPara01 As New Paragraph("KEMENTERIAN PENDIDIKAN MALAYSIA", FontFactory.GetFont("Arial", 10, Font.BOLD))
                    myPara01.Alignment = Element.ALIGN_CENTER
                    myDocument.Add(myPara01)

                    If ddlMatapelajaran.SelectedValue = 1 Then

                        myDocument.Add(imgSpacing)
                        Dim myPara02 As New Paragraph("SLIP KEPUTUSAN BAHASA MELAYU 1104 ULANGAN", FontFactory.GetFont("Tw Cen Mt", 12, Font.NORMAL))
                        myPara02.Alignment = Element.ALIGN_CENTER
                        myDocument.Add(myPara02)

                    ElseIf ddlMatapelajaran.SelectedValue = 2 Then

                        myDocument.Add(imgSpacing)
                        Dim myPara02 As New Paragraph("SLIP KEPUTUSAN SEJARAH 1251 ULANGAN", FontFactory.GetFont("Tw Cen Mt", 12, Font.NORMAL))
                        myPara02.Alignment = Element.ALIGN_CENTER
                        myDocument.Add(myPara02)

                    End If

                    Dim myPara03 As New Paragraph("TAHUN " & ddlTahun.SelectedValue, FontFactory.GetFont("Tw Cen Mt", 12, Font.NORMAL))
                    myPara03.Alignment = Element.ALIGN_CENTER
                    myDocument.Add(myPara03)

                    myDocument.Add(imgSpacing)

                    ''PROFILE STARTS HERE

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    table = New PdfPTable(2)

                    table.WidthPercentage = 100
                    table.SetWidths({30, 70})

                    Dim cell = New PdfPCell()
                    Dim cetak = Environment.NewLine & "NAMA"
                    cetak += Environment.NewLine & "NO.KAD PENGENALAN"
                    cetak += Environment.NewLine & "ANGKA GILIRAN"
                    cetak += Environment.NewLine & "INSTITUSI"
                    If strtotalLength > 55 Then
                        cetak += Environment.NewLine & ""
                    End If
                    cetak += Environment.NewLine & "NAMA BIDANG"
                    cetak += Environment.NewLine & "PROGRAM"
                    cetak += Environment.NewLine & ""

                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
                    cell.Border = 0
                    table.AddCell(cell)

                    strSQL = "SELECT svmu_id FROM kpmkv_svmu WHERE PelajarID = '" & strPelajarID & "'"
                    Dim svmu_id As String = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT PusatPeperiksaanJPN FROM kpmkv_svmu_calon WHERE svmu_id = '" & svmu_id & "' AND PusatPeperiksaanJPN IS NOT NULL"
                    Dim PusatPeperiksaanJPN As String = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT Nama FROM kpmkv_kolej WHERE RecordID = '" & PusatPeperiksaanJPN & "'"
                    strKolejnama = oCommon.getFieldValue(strSQL)

                    cell = New PdfPCell()
                    cetak = Environment.NewLine & ": " & strNama
                    cetak += Environment.NewLine & ": " & strMykad
                    cetak += Environment.NewLine & ": " & strAngkaGiliran
                    cetak += Environment.NewLine & ": " & strKolejnama & ", " & strKolejnegeri
                    cetak += Environment.NewLine & ": " & strNamaKluster
                    cetak += Environment.NewLine & ": " & strNamaKursus & " (" & strKodKursus & ")"
                    cetak += Environment.NewLine & " "

                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
                    cell.Border = 0
                    table.AddCell(cell)
                    Debug.WriteLine(cetak)

                    myDocument.Add(table)

                    ''profile ends here
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    table = New PdfPTable(4)
                    table.WidthPercentage = 100
                    table.SetWidths({30, 42, 18, 10})

                    cell = New PdfPCell()
                    cetak = "KOD"
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
                    cell.Border = 0
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = "MATA PELAJARAN"
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
                    cell.Border = 0
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = "GRED"
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
                    cell.Border = 0
                    table.AddCell(cell)

                    cell = New PdfPCell()
                    cetak = ""
                    cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
                    cell.Border = 0
                    table.AddCell(cell)

                    myDocument.Add(table)

                    If ddlMatapelajaran.SelectedValue = 1 Then

                        ''get gredBMsetara
                        strSQL = "SELECT Gred FROM kpmkv_markah_bmsj_setara WHERE PelajarID = '" & strPelajarID & "' AND MataPelajaran = 'BAHASA MELAYU'"
                        Dim strGredBMSetara As String = oCommon.getFieldValue(strSQL)

                        ''BM1104
                        table = New PdfPTable(4)
                        table.WidthPercentage = 100
                        table.SetWidths({30, 42, 18, 10})
                        table.DefaultCell.Border = 0

                        cell = New PdfPCell()
                        cetak = ""
                        cetak += "1104"
                        cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
                        cell.Border = 0
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = ""
                        cetak += "BAHASA MELAYU"
                        cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
                        cell.Border = 0
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = ""
                        cetak += strGredBMSetara
                        cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
                        cell.Border = 0
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = ""
                        cetak += ""
                        cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
                        cell.Border = 0
                        table.AddCell(cell)

                        Debug.WriteLine(cetak)
                        myDocument.Add(table)

                    ElseIf ddlMatapelajaran.SelectedValue = 2 Then

                        ''get gredSJSetara
                        strSQL = "SELECT Gred FROM kpmkv_markah_bmsj_setara WHERE PelajarID = '" & strPelajarID & "' AND MataPelajaran = 'SEJARAH'"
                        Dim strGredSJSetara As String = oCommon.getFieldValue(strSQL)

                        ''SJ1251
                        table = New PdfPTable(4)
                        table.WidthPercentage = 100
                        table.SetWidths({30, 42, 18, 10})
                        table.DefaultCell.Border = 0

                        cell = New PdfPCell()
                        cetak = ""
                        cetak += "1251"
                        cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
                        cell.Border = 0
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = ""
                        cetak += "SEJARAH"
                        cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
                        cell.Border = 0
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = ""
                        cetak += strGredSJSetara
                        cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
                        cell.Border = 0
                        table.AddCell(cell)

                        cell = New PdfPCell()
                        cetak = ""
                        cetak += ""
                        cell.AddElement(New Paragraph(cetak, FontFactory.GetFont("Arial", 10)))
                        cell.Border = 0
                        table.AddCell(cell)

                        Debug.WriteLine(cetak)
                        myDocument.Add(table)

                    End If

                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)
                    myDocument.Add(New Paragraph("TARIKH: " & ddlHari.Text & "/" & ddlBulan.Text & "/" & ddlTahun_1.Text & "                                                                                                                                                      PENGARAH PEPERIKSAAN", FontFactory.GetFont("Arial", 8, Font.BOLD)))
                    'Dim myPengarah As New Paragraph("" & strKolejnama, FontFactory.GetFont("Arial", 8, Font.BOLD))
                    'myPengarah.Alignment = Element.ALIGN_RIGHT
                    'myDocument.Add(myPengarah)

                    myDocument.Add(imgSpacing)
                    myDocument.Add(imgSpacing)
                    Dim myslip As New Paragraph("Slip ini adalah cetakan komputer, tandatangan tidak diperlukan", FontFactory.GetFont("Arial", 8, Font.ITALIC))
                    myslip.Alignment = Element.ALIGN_CENTER
                    myDocument.Add(myslip)
                    myDocument.NewPage()
                    ''--content end


                    myDocument.NewPage()


                End If

            Next

            myDocument.Close()

            HttpContext.Current.Response.Write(myDocument)
            HttpContext.Current.Response.End()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub countStudent()
        strSQL = "  SELECT COUNT(kpmkv_markah_bmsj_setara.PelajarID)"
        strSQL += " FROM kpmkv_markah_bmsj_setara"
        strSQL += " FROM
                    kpmkv_markah_bmsj_setara
                    LEFT JOIN kpmkv_pelajar ON kpmkv_pelajar.PelajarID = kpmkv_markah_bmsj_setara.PelajarID
                    LEFT JOIN kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID
                    LEFT JOIN kpmkv_kluster ON kpmkv_kluster.KlusterID = kpmkv_kursus.KlusterID
                    LEFT JOIN kpmkv_kelas ON kpmkv_kelas.KelasID = kpmkv_pelajar.KelasID
                    WHERE
                    kpmkv_markah_bmsj_setara.IsAKATahun ='" & ddlTahun.Text & "'"

        Dim total As String = oCommon.getFieldValue(strSQL)
        If total = "" Then
            total = "Tiada Rekod Ditemui"
        End If

        lblMsg.Text = "Jumlah pelajar : " & total
    End Sub

End Class