Imports System.Data.SqlClient
Imports System.IO
Public Class muatturun_bahan
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strSQL2 As String = ""
    Dim strRet As String = ""

    Dim tstDate As DateTime = DateTime.Now
    Dim strdate As String = tstDate.ToString("yyyy-MM-dd")

    Dim fileSavePath As String = ConfigurationManager.AppSettings("FolderPath")
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

                kpmkv_kategori_list()
                ddlKategory.SelectedValue = ""

                kpmkv_semester_list()
                kpmkv_kohort_list()
                ddlKohort.Text = Now.Year

                strRet = BindData2(datRespondent2)
                strRet = BindData(datRespondent)

            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try
    End Sub

    Private Sub kpmkv_kategori_list()
        strSQL = "SELECT ID,Parameter FROM tbl_Settings WHERE type='KATEGORIMUATNAIKBAHAN'  ORDER BY idx ASC"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlKategory.DataSource = ds
            ddlKategory.DataTextField = "Parameter"
            ddlKategory.DataValueField = "ID"
            ddlKategory.DataBind()


            ddlKategory.Items.Add(New ListItem("-Pilih-", ""))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try
    End Sub

    Private Sub kpmkv_kohort_list()
        strSQL = "SELECT Tahun FROM kpmkv_tahun  ORDER BY TahunID"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlKohort.DataSource = ds
            ddlKohort.DataTextField = "Tahun"
            ddlKohort.DataValueField = "Tahun"
            ddlKohort.DataBind()

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

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

            ddlSemester.Items.Insert(0, New ListItem("-Pilih-", ""))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Function BindData(ByVal gvTable As GridView) As Boolean
        Dim myDataSet As New DataSet
        Dim myDataAdapter As New SqlDataAdapter(getSQL, strConn)
        myDataAdapter.SelectCommand.CommandTimeout = 120

        Try
            myDataAdapter.Fill(myDataSet, "myaccount")

            If myDataSet.Tables(0).Rows.Count = 0 Then
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Rekod tidak dijumpai2!"
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

        strSQL = " SELECT bahan.ID,bahan.Kohort ,bahan.Semester ,bahan.Sesi ,bahan.Komponen ,"
        strSQL += " kursus.KodKursus ,bahan.Tajuk ,bahan.Catatan ,bahan.FilePath ,"
        strSQL += " bahan.STarikh ,bahan.ETarikh "

        strSQL += " FROM kpmkv_bahan AS bahan"
        strSQL += " LEFT JOIN tbl_Settings AS setting ON bahan.kategori=setting.ID"
        strSQL += " LEFT JOIN kpmkv_kursus AS kursus ON bahan.KursusID=kursus.KursusID"
        strSQL += " WHERE (Convert(DateTime,STarikh ,103) <= '" & strdate & "') AND ( Convert(DateTime,ETarikh ,103) >= '" & strdate & "')"
        strSQL += " AND isVerified='Y'"
        strSQL += " AND bahan.Kohort='" & ddlKohort.SelectedValue & "'"
        strSQL += " AND bahan.Semester = '" & ddlSemester.SelectedValue & "'"

        If Not ddlKategory.SelectedValue = "" Then
            strSQL += " AND bahan.kategori='" & ddlKategory.SelectedValue & "'"
        End If

        strSQL += " AND bahan.KursusID  IN (SELECT kpmkv_kursus.KursusID "
        strSQL += " FROM kpmkv_kursus_kolej "
        strSQL += " LEFT OUTER Join kpmkv_kursus On kpmkv_kursus_kolej.KursusID = kpmkv_kursus.KursusID "
        strSQL += " WHERE kpmkv_kursus_kolej.KolejRecordID ='" & lblKolejID.Text & "'  "
        strSQL += " AND kpmkv_kursus.Tahun ='" & ddlKohort.SelectedValue & "' )"

        strSQL += " ORDER BY STarikh ASC"

        getSQL = strSQL

        Return getSQL

    End Function

    Private Function getSQL2() As String

        strSQL = " SELECT bahan.ID as umumID,st.parameter as kategori,bahan.Tajuk ,bahan.Catatan ,bahan.FilePath,"
        strSQL += " bahan.Kohort ,bahan.Semester ,bahan.Sesi ,bahan.STarikh ,bahan.ETarikh "
        strSQL += " FROM kpmkv_bahan AS bahan"
        strSQL += " LEFT JOIN tbl_settings as st on st.ID =bahan .Kategori "
        strSQL += " WHERE (Convert(DateTime,STarikh ,103) <= '" & strdate & "') AND ( Convert(DateTime,ETarikh ,103) >= '" & strdate & "')"
        strSQL += " AND isVerified='Y'"
        strSQL += " AND bahan.Kohort='" & ddlKohort.SelectedValue & "'"
        strSQL += " AND bahan.Semester = '" & ddlSemester.SelectedValue & "'"

        strSQL += " AND st.parameter IN ('UMUM','AKADEMIK')"
        strSQL += " ORDER BY STarikh ASC,st.parameter desc"

        getSQL2 = strSQL

        Return getSQL2

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

    Protected Sub datRespondent_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "DownloadBahan" Then
            'Determine the RowIndex of the Row whose Button was clicked.
            Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)

            'Reference the GridView Row.
            Dim row As GridViewRow = datRespondent.Rows(rowIndex)

            'Fetch value of Name.
            Dim strID As String = TryCast(row.FindControl("lblID"), Label).Text

            strSQL = "SELECT FilePath FROM kpmkv_bahan WHERE ID='" & strID & "'"
            Dim fullFileName As String = oCommon.getFieldValue(strSQL)


            fileSavePath = fileSavePath & "//" & fullFileName


            Response.ContentType = "Application/octet-stream"
            Response.AppendHeader("Content-Disposition", "attachment; filename=" & fullFileName & "")
            Response.TransmitFile(fileSavePath)
                Response.End()


        End If
    End Sub

    Protected Sub datRespondent2_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "DownloadBahan2" Then
            'Determine the RowIndex of the Row whose Button was clicked.
            Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)

            'Reference the GridView Row.
            Dim row As GridViewRow = datRespondent2.Rows(rowIndex)

            'Fetch value of Name.
            Dim strID As String = TryCast(row.FindControl("lblID"), Label).Text

            strSQL = "SELECT FilePath FROM kpmkv_bahan WHERE ID='" & strID & "'"
            Dim fullFileName As String = oCommon.getFieldValue(strSQL)


            fileSavePath = fileSavePath & "//" & fullFileName



            Response.ContentType = "Application/octet-stream"
            Response.AppendHeader("Content-Disposition", "attachment; filename=" & fullFileName & "")
            Response.TransmitFile(fileSavePath)
            Response.End()

        End If
    End Sub



    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        lblMsg.Text = ""

        strRet = BindData(datRespondent)
        strRet = BindData2(datRespondent2)

    End Sub
End Class