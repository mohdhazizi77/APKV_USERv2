Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Public Class admin_reportNegeri
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                kpmkv_tahun_list()
                ddlTahun.Text = Now.Year

                kpmkv_negeri_list()
                ddlNegeri.Text = "0"

                'strRet = BindData(datRespondent)
            End If

        Catch ex As Exception
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
    Private Sub kpmkv_negeri_list()
        strSQL = "SELECT Negeri FROM kpmkv_negeri  ORDER BY Negeri"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlNegeri.DataSource = ds
            ddlNegeri.DataTextField = "Negeri"
            ddlNegeri.DataValueField = "Negeri"
            ddlNegeri.DataBind()

            '--ALL
            ddlNegeri.Items.Add(New ListItem("-Pilih-", "0"))

        Catch ex As Exception
            'lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
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
        Dim strGroup As String = " GROUP BY kpmkv_kolej.Negeri,kpmkv_pelajar.Tahun"

        tmpSQL = " SELECT kpmkv_kolej.Negeri,kpmkv_pelajar.Tahun,"
        tmpSQL += " COUNT(*) total,"
        tmpSQL += " SUM(case when kpmkv_pelajar.Semester = '1' then 1 else 0 end) SEM1,"
        tmpSQL += " SUM(case when kpmkv_pelajar.Semester = '2' then 1 else 0 end) SEM2,"
        tmpSQL += " SUM(case when kpmkv_pelajar.Semester = '3' then 1 else 0 end) SEM3,"
        tmpSQL += " SUM(case when kpmkv_pelajar.Semester = '4' then 1 else 0 end) SEM4"
        tmpSQL += " FROM kpmkv_pelajar, kpmkv_kolej"
        strWhere += " WHERE kpmkv_kolej.RecordID = kpmkv_pelajar.KolejRecordID AND kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.StatusID='2' AND kpmkv_pelajar.Tahun ='" & ddlTahun.Text & "'"

        '--kolej
        If Not ddlNegeri.Text = "0" Then
            strWhere += " AND kpmkv_kolej.Negeri='" & ddlNegeri.Text & "'"
        End If

        getSQL = tmpSQL & strWhere & strGroup
        ''--debug
        'Response.Write(getSQL)

        Return getSQL

    End Function
    Public Sub SummaryReportSEM1()
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strGroup As String = " GROUP BY kpmkv_pelajar.Tahun"

        tmpSQL = " SELECT COUNT(kpmkv_pelajar.Semester) SEM1"
        tmpSQL += " FROM kpmkv_pelajar, kpmkv_kolej WHERE kpmkv_kolej.RecordID = kpmkv_pelajar.KolejRecordID AND kpmkv_pelajar.IsDeleted='N' "
        tmpSQL += " AND kpmkv_pelajar.Semester = '1' AND kpmkv_pelajar.StatusID='2' AND kpmkv_pelajar.Tahun ='" & ddlTahun.Text & "'"

        '--kolej
        If Not ddlNegeri.Text = "0" Then
            strWhere += " AND kpmkv_kolej.Negeri='" & ddlNegeri.Text & "'"
        End If

        strSQL = tmpSQL + strWhere + strGroup
        lblSem1.Text = oCommon.getFieldValue(strSQL)

    End Sub
    Public Sub SummaryReportSEM2()
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strGroup As String = " GROUP BY kpmkv_pelajar.Tahun"

        tmpSQL = " SELECT COUNT(kpmkv_pelajar.Semester) SEM2"
        tmpSQL += " FROM kpmkv_pelajar, kpmkv_kolej WHERE kpmkv_kolej.RecordID = kpmkv_pelajar.KolejRecordID AND kpmkv_pelajar.IsDeleted='N' "
        tmpSQL += " AND kpmkv_pelajar.Semester = '2' AND kpmkv_pelajar.StatusID='2' AND kpmkv_pelajar.Tahun='" & ddlTahun.Text & "'"

        '--kolej
        If Not ddlNegeri.Text = "0" Then
            strWhere += " AND kpmkv_kolej.Negeri='" & ddlNegeri.Text & "'"
        End If

        strSQL = tmpSQL + strWhere + strGroup
        lblSem2.Text = oCommon.getFieldValue(strSQL)
    End Sub
    Public Sub SummaryReportSEM3()
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strGroup As String = " GROUP BY kpmkv_pelajar.Tahun"

        tmpSQL = " SELECT COUNT(kpmkv_pelajar.Semester) SEM3"
        tmpSQL += " FROM kpmkv_pelajar, kpmkv_kolej WHERE kpmkv_kolej.RecordID = kpmkv_pelajar.KolejRecordID AND kpmkv_pelajar.IsDeleted='N' "
        tmpSQL += " AND kpmkv_pelajar.Semester = '3' AND kpmkv_pelajar.StatusID='2' AND kpmkv_pelajar.Tahun ='" & ddlTahun.Text & "'"

        '--kolej
        If Not ddlNegeri.Text = "0" Then
            strWhere += " AND kpmkv_kolej.Negeri='" & ddlNegeri.Text & "'"
        End If

        strSQL = tmpSQL + strWhere + strGroup
        lblSem3.Text = oCommon.getFieldValue(strSQL)
    End Sub
    Public Sub SummaryReportSEM4()
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strGroup As String = " GROUP BY kpmkv_pelajar.Tahun"

        tmpSQL = " SELECT COUNT(kpmkv_pelajar.Semester) SEM4"
        tmpSQL += " FROM kpmkv_pelajar, kpmkv_kolej WHERE kpmkv_kolej.RecordID = kpmkv_pelajar.KolejRecordID AND kpmkv_pelajar.IsDeleted='N' "
        tmpSQL += " AND kpmkv_pelajar.Semester = '4' AND kpmkv_pelajar.StatusID='2' AND kpmkv_pelajar.Tahun='" & ddlTahun.Text & "'"

        '--kolej
        If Not ddlNegeri.Text = "0" Then
            strWhere += " AND kpmkv_kolej.Negeri='" & ddlNegeri.Text & "'"
        End If

        strSQL = tmpSQL + strWhere + strGroup
        lblSem4.Text = oCommon.getFieldValue(strSQL)
    End Sub
    Public Sub SummaryReport()
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strGroup As String = " GROUP BY kpmkv_pelajar.Tahun"

        tmpSQL = " SELECT COUNT(kpmkv_pelajar.Semester) JUMLAH"
        tmpSQL += " FROM kpmkv_pelajar, kpmkv_kolej WHERE kpmkv_kolej.RecordID = kpmkv_pelajar.KolejRecordID AND kpmkv_pelajar.IsDeleted='N' "
        tmpSQL += " AND kpmkv_pelajar.StatusID='2' AND kpmkv_pelajar.Tahun='" & ddlTahun.Text & "'"

        '--kolej
        If Not ddlNegeri.Text = "0" Then
            strWhere += " AND kpmkv_kolej.Negeri='" & ddlNegeri.Text & "'"
        End If

        strSQL = tmpSQL + strWhere + strGroup
        lblJumlah.Text = oCommon.getFieldValue(strSQL)
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

    Private Sub datRespondent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondent.PageIndexChanging
        datRespondent.PageIndex = e.NewPageIndex
        strRet = BindData(datRespondent)

    End Sub


End Class