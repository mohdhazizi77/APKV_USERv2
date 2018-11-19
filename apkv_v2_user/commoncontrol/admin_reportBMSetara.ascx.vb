Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Public Class admin_reportBMSetara
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
        SummaryReportSEM4()
        SummaryReportULANG()
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
        Dim strGroup As String = " GROUP BY  kpmkv_pelajar.Tahun,kpmkv_pelajar.Sesi"
        Dim strOrder As String = " ORDER BY  kpmkv_pelajar.Tahun,kpmkv_pelajar.Sesi ASC"

        tmpSQL = " SELECT kpmkv_pelajar.Tahun,kpmkv_pelajar.Sesi,"
        tmpSQL += " COUNT(*) total,"
        tmpSQL += " SUM(case when kpmkv_pelajar.Semester = '1' then 1 else 0 end) SEM1,"
        tmpSQL += " SUM(case when kpmkv_pelajar.Semester = '2' then 1 else 0 end) SEM2,"
        tmpSQL += " SUM(case when kpmkv_pelajar.Semester = '3' then 1 else 0 end) SEM3,"
        tmpSQL += " SUM(case when kpmkv_pelajar.Semester = '4' then 1 else 0 end) SEM4,"
        tmpSQL += " SUM(case when kpmkv_pelajar.Semester = '0' then 1 else 0 end) ULANG"
        tmpSQL += " FROM kpmkv_pelajar, kpmkv_kolej"
        strWhere += " WHERE kpmkv_kolej.RecordID = kpmkv_pelajar.KolejRecordID AND kpmkv_pelajar.IsDeleted='N' "
        strWhere += " AND kpmkv_pelajar.StatusID='2' AND kpmkv_pelajar.Tahun ='" & ddlTahun.Text & "' AND kpmkv_pelajar.Semester ='4'"
        strWhere += " AND kpmkv_pelajar.IsCalon='1' AND kpmkv_pelajar.KolejRecordID ='" & lblKolejID.Text & "'"

        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere += " AND kpmkv_pelajar.Sesi ='" & chkSesi.Text & "'"
        End If

        Dim tmpUNION As String = "  UNION  "

        Dim tmpSQL1 As String
        Dim strWhere1 As String = ""
        Dim strGroup1 As String = " GROUP BY kpmkv_kolej.Negeri,kpmkv_kolej.Nama,kpmkv_pelajar_ulang.Tahun,kpmkv_pelajar_ulang.Sesi"

        tmpSQL1 = " SELECT kpmkv_pelajar_ulang.Tahun,kpmkv_pelajar_ulang.Sesi,"
        tmpSQL1 += " COUNT(*) total,"
        tmpSQL1 += " SUM(case when kpmkv_pelajar_ulang.Semester = '1' then 1 else 0 end) SEM1,"
        tmpSQL1 += " SUM(case when kpmkv_pelajar_ulang.Semester = '2' then 1 else 0 end) SEM2,"
        tmpSQL1 += " SUM(case when kpmkv_pelajar_ulang.Semester = '3' then 1 else 0 end) SEM3,"
        tmpSQL1 += " SUM(case when kpmkv_pelajar_ulang.Semester = '0' then 1 else 0 end) SEM4,"
        tmpSQL1 += " SUM(case when kpmkv_pelajar_ulang.Semester = '4' then 1 else 0 end) ULANG"
        tmpSQL1 += " FROM kpmkv_pelajar_ulang, kpmkv_kolej"
        strWhere1 += " WHERE kpmkv_kolej.RecordID = kpmkv_pelajar_ulang.KolejRecordID AND kpmkv_pelajar_ulang.NamaMataPelajaran='BAHASA MELAYU'"
        strWhere1 += " AND kpmkv_pelajar_ulang.Tahun ='" & ddlTahun.Text & "' AND kpmkv_pelajar_ulang.Semester ='4' AND kpmkv_pelajar_ulang.IsCalon='1'"
        strWhere1 += " AND kpmkv_pelajar_ulang.KolejRecordID ='" & lblKolejID.Text & "'"
      
        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere1 += " AND kpmkv_pelajar_ulang.Sesi ='" & chkSesi.Text & "'"
        End If


        getSQL = tmpSQL & strWhere & strGroup & tmpUNION & tmpSQL1 & strWhere1 & strGroup1 & strOrder
        ''--debug
        'Response.Write(getSQL)

        Return getSQL

    End Function
    Public Sub SummaryReportSEM4()
        Dim tmpSQL As String
        Dim strWhere As String = ""

        tmpSQL = " SELECT COUNT(kpmkv_pelajar.Semester) SEM4"
        tmpSQL += " FROM kpmkv_pelajar, kpmkv_kolej"
        strWhere += " WHERE kpmkv_kolej.RecordID = kpmkv_pelajar.KolejRecordID AND kpmkv_pelajar.IsDeleted='N' "
        strWhere += " AND kpmkv_pelajar.StatusID='2' AND kpmkv_pelajar.Tahun ='" & ddlTahun.Text & "' AND kpmkv_pelajar.Semester ='4'"
        strWhere += " AND kpmkv_pelajar.KolejRecordID ='" & lblKolejID.Text & "'"
       
        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere += " AND kpmkv_pelajar.Sesi ='" & chkSesi.Text & "'"
        End If


        strSQL = tmpSQL + strWhere
        strRet = oCommon.getFieldValue(strSQL)

        If Not String.IsNullOrEmpty(strRet) Then
            lblSem1.Text = strRet
        Else
            lblSem1.Text = 0
        End If

    End Sub
    Public Sub SummaryReportULANG()

        Dim tmpSQL1 As String
        Dim strWhere1 As String = ""

        tmpSQL1 = " SELECT COUNT(kpmkv_pelajar_ulang.Semester) SEM4"
        tmpSQL1 += " FROM kpmkv_pelajar_ulang, kpmkv_kolej"
        strWhere1 += " WHERE kpmkv_kolej.RecordID = kpmkv_pelajar_ulang.KolejRecordID AND kpmkv_pelajar_ulang.NamaMataPelajaran='BAHASA MELAYU'"
        strWhere1 += " AND kpmkv_pelajar_ulang.Tahun ='" & ddlTahun.Text & "' AND kpmkv_pelajar_ulang.Semester ='4'"
        strWhere1 += " AND kpmkv_pelajar_ulang.KolejRecordID ='" & lblKolejID.Text & "'"
       
        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere1 += " AND kpmkv_pelajar_ulang.Sesi ='" & chkSesi.Text & "'"
        End If

        strSQL = tmpSQL1 + strWhere1
        strRet = oCommon.getFieldValue(strSQL)

        If Not String.IsNullOrEmpty(strRet) Then
            lblSem2.Text = strRet
        Else
            lblSem2.Text = 0
        End If
    End Sub

    Public Sub SummaryReport()
        lblJumlah.Text = CInt(lblSem1.Text) + CInt(lblSem2.Text)
    End Sub
    Private Sub datRespondent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondent.PageIndexChanging
        datRespondent.PageIndex = e.NewPageIndex
        strRet = BindData(datRespondent)

    End Sub
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