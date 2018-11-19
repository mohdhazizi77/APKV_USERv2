Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Imports RKLib.ExportData
Imports Excel = Microsoft.Office.Interop.Excel
Imports ExcelAutoFormat = Microsoft.Office.Interop.Excel.XlRangeAutoFormat
Public Class bmSetara_create
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""
    Dim strKolejnama As String
    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' bt.bAttributes.Add("onclick", "return confirm('Pasti ingin menghapuskan rekod ini?');")
        Try

            If Not IsPostBack Then
                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
                strKolejnama = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                kpmkv_tahun_list()
                ddlTahun.Text = Now.Year

                kpmkv_semester_list()

                strRet = BindData2(datRespondent2)
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
    Private Sub kpmkv_semester_list()
        strSQL = "SELECT Semester FROM kpmkv_semester WITH (NOLOCK) ORDER BY SemesterID"
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
    Private Function BindData2(ByVal gvTable As GridView) As Boolean
        Dim myDataSet As New DataSet
        Dim myDataAdapter As New SqlDataAdapter(getSQL2, strConn)
        myDataAdapter.SelectCommand.CommandTimeout = 120

        Try
            myDataAdapter.Fill(myDataSet, "myaccount")

            If myDataSet.Tables(0).Rows.Count = 0 Then
                divMsg.Attributes("class") = "error"
                ' lblMsg.Text = "Rekod tidak dijumpai!"
            Else
                divMsg.Attributes("class") = "info"
                'lblMsg.Text = "Jumlah Rekod#:" & myDataSet.Tables(0).Rows.Count
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
    Private Function getSQL2() As String
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY kpmkv_file.FileID, kpmkv_file.Tahun, kpmkv_file.Sesi ASC"

        '--not deleted
        tmpSQL = "SELECT kpmkv_file.FileID, kpmkv_file.Tahun, kpmkv_file.Sesi,kpmkv_file.NamaKolej,kpmkv_file.KolejID,kpmkv_file.NamaFail"
        tmpSQL += " FROM  kpmkv_file"
        strWhere = " WHERE kpmkv_file.KolejID='" & lblKolejID.Text & "'"

        '--tahun
        If Not ddlTahun.Text = "PILIH" Then
            strWhere += " AND kpmkv_file.Tahun ='" & ddlTahun.Text & "'"
        End If
        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere += " AND kpmkv_pelajar.Sesi ='" & chkSesi.Text & "'"
        End If


        getSQL2 = tmpSQL & strWhere & strOrder
        ''--debug
        'Response.Write(getSQL)

        Return getSQL2

    End Function
    Private Function getSQL() As String
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY kpmkv_pelajar.Tahun, kpmkv_pelajar.Semester, kpmkv_pelajar.Sesi, kpmkv_pelajar.Nama ASC"

        '--not deleted
        tmpSQL = "SELECT kpmkv_pelajar.PelajarID, kpmkv_pelajar.AngkaGiliran, kpmkv_pelajar.Nama "
        tmpSQL += " FROM  kpmkv_pelajar LEFT OUTER JOIN kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
        tmpSQL += " kpmkv_status ON kpmkv_pelajar.StatusID = kpmkv_status.StatusID LEFT OUTER JOIN kpmkv_kelas ON kpmkv_pelajar.KelasID = kpmkv_kelas.KelasID"
        strWhere = " WHERE kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.StatusID='2' AND kpmkv_pelajar.KolejRecordID='" & lblKolejID.Text & "'"

        '--tahun
        If Not ddlTahun.Text = "PILIH" Then
            strWhere += " AND kpmkv_pelajar.Tahun ='" & ddlTahun.Text & "'"
        End If
        '--semester
        If Not ddlSemester.Text = "PILIH" Then
            strWhere += " AND kpmkv_pelajar.Semester ='" & ddlSemester.Text & "'"
        End If
        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere += " AND kpmkv_pelajar.Sesi ='" & chkSesi.Text & "'"
        End If

        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        'Response.Write(getSQL)

        Return getSQL

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

    Private Sub datRespondent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondent.PageIndexChanging
        datRespondent.PageIndex = e.NewPageIndex
        strRet = BindData(datRespondent)

    End Sub
    Private Sub datRespondent_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles datRespondent.SelectedIndexChanging
        Dim strKeyID As String = datRespondent.DataKeys(e.NewSelectedIndex).Value.ToString
        ' Response.Redirect("pelajar.view.aspx?PelajarID=" & strKeyID)

    End Sub

    Protected Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click

        'Get the data from database into datatable 
        Dim cmd As New SqlCommand(getSQL)
        ' Dim dt As DataTable = GetData(cmd)

        Dim dt As New DataTable()
        Dim strConnString As [String] = ConfigurationManager.AppSettings("ConnectionString")
        Dim con As New SqlConnection(strConnString)
        Dim sda As New SqlDataAdapter()


        'Dim cmd As New SqlCommand(strGetSql, con)
        'cmd = New SqlCommand(strGetSql, con)

        cmd.Connection = con
        Try
            con.Open()
            sda.SelectCommand = cmd
            sda.Fill(dt)

            If dt.Rows.Count > 0 Then


                Dim msFileName As String = "Borang" & oCommon.getRandom & ".xlsx"
                Dim path As String = Server.MapPath("borang\")

                ' CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
                If Not Directory.Exists(path) Then
                    Directory.CreateDirectory(path)
                End If

                ' File.Delete(path & msFileName)      ' DELETE THE FILE BEFORE CREATING A NEW ONE.

                ' ADD A WORKBOOK USING THE EXCEL APPLICATION.
                Dim xlAppToExport As New Excel.Application
                xlAppToExport.Workbooks.Add()

                ' ADD A WORKSHEET.
                Dim xlWorkSheetToExport As Excel.Worksheet
                xlWorkSheetToExport = xlAppToExport.Sheets("Sheet1")

                ' ROW ID FROM WHERE THE DATA STARTS SHOWING.
                Dim iRowCnt As Integer = 2

                With xlWorkSheetToExport
                    '' SHOW THE HEADER.
                    ' .Cells(1, 1).value = "Pelajar"
                    '.Cells(1, 1).FONT.NAME = "Calibri"
                    '.Cells(1, 1).Font.Bold = True
                    '.Cells(1, 1).Font.Size = 10

                    '.Range("A1:F1").Font.Size = 10       ' MERGE CELLS OF THE HEADER.

                    ' SHOW COLUMNS ON THE TOP.
                    .Cells(iRowCnt - 1, 1).value = "Bil"
                    .Cells(iRowCnt - 1, 2).value = "AngkaGiliran"
                    .Cells(iRowCnt - 1, 3).value = "Nama"
                    .Cells(iRowCnt - 1, 4).value = "Markah 1"
                    .Cells(iRowCnt - 1, 5).value = "Markah 2"
                    .Cells(iRowCnt - 1, 6).value = "Catatan"


                    For i As Integer = 0 To dt.Rows.Count - 1
                        .Cells(iRowCnt, 1).value = i
                        .Cells(iRowCnt, 2).value = dt.Rows(i).Item("AngkaGiliran")
                        .Cells(iRowCnt, 3).value = dt.Rows(i).Item("Nama")
                        '.Cells(iRowCnt, 3).value = dt.Rows(i).Item("Markah 1")
                        '.Cells(iRowCnt, 4).value = dt.Rows(i).Item("Markah 2")
                        '.Cells(iRowCnt, 5).value = dt.Rows(i).Item("Catatan")
                        iRowCnt = iRowCnt + 1
                    Next

                    ' FINALLY, FORMAT THE EXCEL SHEET USING EXCEL'S AUTOFORMAT FUNCTION.
                    'xlAppToExport.ActiveCell.Worksheet.Cells(6, 1).AutoFormat( _
                    '    ExcelAutoFormat.xlRangeAutoFormatList3)
                End With

                ' SAVE THE FILE IN A FOLDER.
                xlWorkSheetToExport.SaveAs(path & msFileName)

                ' CLEAR.
                xlAppToExport.Workbooks.Close() : xlAppToExport.Quit()
                xlAppToExport = Nothing : xlWorkSheetToExport = Nothing

                lblMsg.Text = "Data Exported Sucessfully"
                lblMsg.Attributes.Add("style", "color:green; font: normal 14px Verdana;")
                'btView.Attributes.Add("style", "display:block")
                'btDownLoadFile.Attributes.Add("style", "display:block")


                'save to database
                Dim strQuery As String = "INSERT INTO kpmkv_file(Tahun, Sesi, NamaKolej, KolejID, NamaFail) VALUES (@Tahun, @Sesi, @NamaKolej, @KolejID, @NamaFail)"
                Dim cmd2 As SqlCommand = New SqlCommand(strQuery)
                cmd2.Parameters.Add("@Tahun", SqlDbType.VarChar).Value = ddlTahun.Text
                cmd2.Parameters.Add("@Sesi", SqlDbType.VarChar).Value = chkSesi.Text
                cmd2.Parameters.Add("@NamaKolej", SqlDbType.VarChar).Value = strKolejnama
                cmd2.Parameters.Add("@KolejID", SqlDbType.VarChar).Value = lblKolejID.Text
                cmd2.Parameters.Add("@NamaFail", SqlDbType.VarChar).Value = msFileName
                InsertUpdateData(cmd2)
            End If

        Catch ex As Exception
            lblMsg.Text = "There was an error."
            lblMsg.Attributes.Add("style", "color:red; font: bold 14px/16px Sans-Serif,Arial")
        Finally
            sda.Dispose() : sda = Nothing

        End Try

    End Sub
    Public Function InsertUpdateData(ByVal cmd As SqlCommand) As Boolean
        Dim dt As New DataTable()
        Dim strConnString As [String] = ConfigurationManager.AppSettings("ConnectionString")
        Dim con As New SqlConnection(strConnString)
        Dim sda As New SqlDataAdapter()
        cmd.CommandType = CommandType.Text
        cmd.Connection = con
        Try
            con.Open()
            cmd.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            Response.Write(ex.Message)
            Return False
        Finally
            con.Close()
            con.Dispose()
        End Try
    End Function
    ' VIEW THE EXPORTED EXCEL DATA.
    'Protected Sub btView_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) _
    '        Handles btView.ServerClick
    '    Dim path As String = Server.MapPath("exportedfiles\")
    '    Try
    '        If Directory.Exists(path) Then                  ' CHECK IF THE FOLDER EXISTS.
    '            If File.Exists(path & "EmployeeDetails.xlsx") Then  ' CHECK IF THE FILE EXISTS.

    '                ' SHOW (NOT DOWNLOAD) THE EXCEL FILE.
    '                Dim xlAppToView As New Excel.Application
    '                xlAppToView.Workbooks.Open(path & "EmployeeDetails.xlsx")
    '                xlAppToView.Visible = True

    '            End If
    '        End If
    '    Catch ex As Exception
    '        '
    '    End Try
    'End Sub

    ' DOWNLOAD THE FILE.
    Protected Sub DownLoadFile(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim sPath As String = Server.MapPath("exportedfiles\")

            Response.AppendHeader("Content-Disposition", "attachment;filename=EmployeeDetails.xlsx")
            Response.TransmitFile(sPath & "EmployeeDetails.xlsx")
            Response.[End]()

        Catch ex As Exception
        End Try
    End Sub
    Private Sub datRespondent2_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles datRespondent2.RowDeleting
        lblMsg.Text = ""
            Dim FileID As Integer = datRespondent2.DataKeys(e.RowIndex).Values("FileID")
            Try
            If Not FileID = Session("FileID") Then
                strSQL = "DELETE FROM kpmkv_file WHERE fileID='" & FileID & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
                If strRet = "0" Then
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Fail berjaya dipadamkan"
                    Session("FileID") = ""
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Fail tidak berjaya dipadamkn"
                    Session("FileID") = ""
                End If
            Else
                Session("FileID") = ""
            End If

            Catch ex As Exception
                divMsg.Attributes("class") = "error"
            End Try

        strRet = BindData2(datRespondent2)
    End Sub
End Class