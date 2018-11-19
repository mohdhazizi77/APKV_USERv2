Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient

Public Class admin_reportKursus
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                lblMsg.Text = ""

                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                lblTahun.Text = Now.Year

                kpmkv_tahun_list()
                ddlTahun.Text = Now.Year


            End If

        Catch ex As Exception
            lblMsg.Text = "Error Message:" & ex.Message
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
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        lblMsg.Text = ""

        strRet = BindData(datRespondent)
        SummaryReportSEM1()
        SummaryReportSEM2()
        SummaryReportSEM3()
        SummaryReportSEM4()
        SummaryReport()
    End Sub
    Private Function BindData(ByVal gvTable As GridView) As Boolean
        Dim myDataSet As New DataSet
        Dim myDataAdapter As New SqlDataAdapter(getSQL, strConn)
        myDataAdapter.SelectCommand.CommandTimeout = 120

        Try
            myDataAdapter.Fill(myDataSet, "myaccount")

            If myDataSet.Tables(0).Rows.Count = 0 Then
            Else
            End If

            gvTable.DataSource = myDataSet
            gvTable.DataBind()
            objConn.Close()
        Catch ex As Exception
            Return False
        End Try

        Return True

    End Function

    Private Function getSQL() As String
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strGroup As String = " group by kpmkv_kursus.KodKursus,kpmkv_pelajar.Tahun,kpmkv_pelajar.Sesi"
        Dim strOrder As String = " ORDER BY kpmkv_kursus.KodKursus ASC"

        tmpSQL = " select kpmkv_kursus.KodKursus,kpmkv_pelajar.Tahun,kpmkv_pelajar.Sesi,"
        tmpSQL += " count(*) total,"
        tmpSQL += " sum(case when kpmkv_pelajar.Semester = '1' then 1 else 0 end) SEM1,"
        tmpSQL += " sum(case when kpmkv_pelajar.Semester = '2' then 1 else 0 end) SEM2,"
        tmpSQL += " sum(case when kpmkv_pelajar.Semester = '3' then 1 else 0 end) SEM3,"
        tmpSQL += " sum(case when kpmkv_pelajar.Semester = '4' then 1 else 0 end) SEM4"
        tmpSQL += " from kpmkv_pelajar, kpmkv_kursus,kpmkv_kolej"
        strWhere += " WHERE kpmkv_kursus.KursusID = kpmkv_pelajar.KursusID AND kpmkv_kolej.RecordID = kpmkv_pelajar.KolejRecordID"
        strWhere += " AND kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.StatusID='2' AND kpmkv_pelajar.Tahun ='" & ddlTahun.Text & "'"
        strWhere += " AND kpmkv_pelajar.KolejRecordID ='" & lblKolejID.Text & "'"

        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere += " AND kpmkv_pelajar.Sesi ='" & chkSesi.Text & "'"
        End If

        getSQL = tmpSQL & strWhere & strGroup & strOrder
        ''--debug
        'Response.Write(getSQL)

        Return getSQL

    End Function
    Public Sub SummaryReportSEM1()
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strGroup As String = " GROUP BY kpmkv_pelajar.Tahun"

        tmpSQL = " SELECT COUNT(kpmkv_pelajar.Semester) SEM1"
        tmpSQL += " FROM kpmkv_pelajar, kpmkv_kursus,kpmkv_kolej"
        strWhere += " WHERE kpmkv_kolej.RecordID = kpmkv_pelajar.KolejRecordID"
        strWhere += " AND kpmkv_kursus.KursusID = kpmkv_pelajar.KursusID AND kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.StatusID='2' "
        strWhere += " AND kpmkv_pelajar.Semester = '1' AND kpmkv_pelajar.Tahun ='" & ddlTahun.Text & "'"
        strWhere += " AND kpmkv_pelajar.KolejRecordID ='" & lblKolejID.Text & "'"

        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere += " AND kpmkv_pelajar.Sesi ='" & chkSesi.Text & "'"
        End If

        strSQL = tmpSQL + strWhere + strGroup
        lblSem1.Text = oCommon.getFieldValue(strSQL)

    End Sub
    Public Sub SummaryReportSEM2()
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strGroup As String = " GROUP BY kpmkv_pelajar.Tahun"

        tmpSQL = " SELECT COUNT(kpmkv_pelajar.Semester) SEM2"
        tmpSQL += " FROM kpmkv_pelajar, kpmkv_kursus,kpmkv_kolej"
        strWhere += " WHERE kpmkv_kolej.RecordID = kpmkv_pelajar.KolejRecordID"
        strWhere += " AND kpmkv_kursus.KursusID = kpmkv_pelajar.KursusID AND kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.StatusID='2' "
        strWhere += " AND kpmkv_pelajar.Semester = '2' AND kpmkv_pelajar.Tahun ='" & ddlTahun.Text & "'"
        strWhere += " AND kpmkv_pelajar.KolejRecordID ='" & lblKolejID.Text & "'"


        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere += " AND kpmkv_pelajar.Sesi ='" & chkSesi.Text & "'"
        End If

        strSQL = tmpSQL + strWhere + strGroup
        lblSem2.Text = oCommon.getFieldValue(strSQL)

    End Sub
    Public Sub SummaryReportSEM3()
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strGroup As String = " GROUP BY kpmkv_pelajar.Tahun"

        tmpSQL = " SELECT COUNT(kpmkv_pelajar.Semester) SEM3"
        tmpSQL += " FROM kpmkv_pelajar, kpmkv_kursus,kpmkv_kolej"
        strWhere += " WHERE kpmkv_kolej.RecordID = kpmkv_pelajar.KolejRecordID"
        strWhere += " AND kpmkv_kursus.KursusID = kpmkv_pelajar.KursusID AND kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.StatusID='2' "
        strWhere += " AND kpmkv_pelajar.Semester = '3' AND kpmkv_pelajar.Tahun ='" & ddlTahun.Text & "'"
        strWhere += " AND kpmkv_pelajar.KolejRecordID ='" & lblKolejID.Text & "'"


        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere += " AND kpmkv_pelajar.Sesi ='" & chkSesi.Text & "'"
        End If

        strSQL = tmpSQL + strWhere + strGroup
        lblSem3.Text = oCommon.getFieldValue(strSQL)

    End Sub
    Public Sub SummaryReportSEM4()
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strGroup As String = " GROUP BY kpmkv_pelajar.Tahun"

        tmpSQL = " SELECT COUNT(kpmkv_pelajar.Semester) SEM4"
        tmpSQL += " FROM kpmkv_pelajar, kpmkv_kursus,kpmkv_kolej"
        strWhere += " WHERE kpmkv_kolej.RecordID = kpmkv_pelajar.KolejRecordID"
        strWhere += " AND kpmkv_kursus.KursusID = kpmkv_pelajar.KursusID AND kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.StatusID='2' "
        strWhere += " AND kpmkv_pelajar.Semester = '4' AND kpmkv_pelajar.Tahun ='" & ddlTahun.Text & "'"
        strWhere += " AND kpmkv_pelajar.KolejRecordID ='" & lblKolejID.Text & "'"

        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere += " AND kpmkv_pelajar.Sesi ='" & chkSesi.Text & "'"
        End If

        strSQL = tmpSQL + strWhere + strGroup
        lblSem4.Text = oCommon.getFieldValue(strSQL)

    End Sub
    Public Sub SummaryReport()
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strGroup As String = " GROUP BY kpmkv_pelajar.Tahun"

        tmpSQL = " SELECT COUNT(kpmkv_pelajar.Semester) JUMLAH"
        tmpSQL += " FROM kpmkv_pelajar, kpmkv_kursus,kpmkv_kolej"
        strWhere += " WHERE kpmkv_kolej.RecordID = kpmkv_pelajar.KolejRecordID"
        strWhere += " AND kpmkv_kursus.KursusID = kpmkv_pelajar.KursusID AND kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.StatusID='2' "
        strWhere += " AND kpmkv_pelajar.Tahun ='" & ddlTahun.Text & "'"
        strWhere += " AND kpmkv_pelajar.KolejRecordID ='" & lblKolejID.Text & "'"

        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere += " AND kpmkv_pelajar.Sesi ='" & chkSesi.Text & "'"
        End If

        strSQL = tmpSQL + strWhere + strGroup
        ' lblJumlah.Text = oCommon.getFieldValue(strSQL)
    End Sub
    Private Sub datRespondent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondent.PageIndexChanging
        datRespondent.PageIndex = e.NewPageIndex
        strRet = BindData(datRespondent)

    End Sub

    Private Sub datRespondent_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles datRespondent.SelectedIndexChanging
        Dim strKeyID As String = datRespondent.DataKeys(e.NewSelectedIndex).Value.ToString
        ' Response.Redirect("pelajar.view.aspx?PelajarID=" & strKeyID)

    End Sub
    'Protected Sub datRespondent_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles datRespondent.RowCommand
    '    lblMsgTop.Text = ""
    '    If (e.CommandName = "padam") = True Then

    '        Dim PelajarID = Int32.Parse(e.CommandArgument.ToString())

    '        strSQL = "SELECT AngkaGiliran FROM kpmkv_Pelajar WHERE PelajarID='" & PelajarID & "' and AngkaGiliran <> ''"
    '        If oCommon.isExist(strSQL) = True Then
    '        Else
    '            strSQL = "DELETE FROM kpmkv_pelajar WHERE PelajarID='" & PelajarID & "'"
    '            strRet = oCommon.ExecuteSQL(strSQL)
    '            If strRet = "0" Then

    '                strSQL = "DELETE FROM kpmkv_pelajar_markah WHERE PelajarID='" & PelajarID & "'"
    '            Else
    '            End If

    '        End If

    '    End If
    '    strRet = BindData(datRespondent)

    'End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        Try
            lblMsg.Text = ""

            ExportToCSV(getSQL)

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try

    End Sub

    Private Sub ExportToCSV(ByVal strQuery As String)
        'Get the data from database into datatable 
        Dim cmd As New SqlCommand(strQuery)
        Dim dt As DataTable = GetData(cmd)

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=FileExport.csv")
        Response.Charset = ""
        Response.ContentType = "application/text"


        Dim sb As New StringBuilder()
        For k As Integer = 0 To dt.Columns.Count - 1
            'add separator 
            sb.Append(dt.Columns(k).ColumnName + ","c)
        Next

        'append new line 
        sb.Append(vbCr & vbLf)
        For i As Integer = 0 To dt.Rows.Count - 1
            For k As Integer = 0 To dt.Columns.Count - 1
                '--add separator 
                'sb.Append(dt.Rows(i)(k).ToString().Replace(",", ";") + ","c)

                'cleanup here
                If k <> 0 Then
                    sb.Append(",")
                End If

                Dim columnValue As Object = dt.Rows(i)(k).ToString()
                If columnValue Is Nothing Then
                    sb.Append("")
                Else
                    Dim columnStringValue As String = columnValue.ToString()

                    Dim cleanedColumnValue As String = CleanCSVString(columnStringValue)

                    If columnValue.[GetType]() Is GetType(String) AndAlso Not columnStringValue.Contains(",") Then
                        ' Prevents a number stored in a string from being shown as 8888E+24 in Excel. Example use is the AccountNum field in CI that looks like a number but is really a string.
                        cleanedColumnValue = "=" & cleanedColumnValue
                    End If
                    sb.Append(cleanedColumnValue)
                End If

            Next
            'append new line 
            sb.Append(vbCr & vbLf)
        Next
        Response.Output.Write(sb.ToString())
        Response.Flush()
        Response.End()

    End Sub

    Protected Function CleanCSVString(ByVal input As String) As String
        Dim output As String = """" & input.Replace("""", """""").Replace(vbCr & vbLf, " ").Replace(vbCr, " ").Replace(vbLf, "") & """"
        Return output

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
End Class