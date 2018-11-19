Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class matapelajaran_list
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnExecute.Attributes.Add("onclick", "return confirm('Pasti ingin meneruskan fungsi tersebut?');")

        Try
            If Not IsPostBack Then

                kpmkv_tahun_list()
                ddlTahun.Text = Now.Year

                kpmkv_namakluster_list()
                ddlNamaKluster.Text = "PILIH"

                kpmkv_semester_list()
                ddlSemester.Text = "PILIH"

                kpmkv_sesi_list()
                ddlSesi.Text = "1"

                kpmkv_namakursus_list()
                ddlNamaKursus.Text = "PILIH"

                kpmkv_kod_list()
                ddlKodMataPelajaran.Text = "PILIH"

                kpmkv_menu_list()

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

            '--ALL
            ddlTahun.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_namakluster_list()
        strSQL = "SELECT NamaKluster FROM kpmkv_kluster WITH (NOLOCK) ORDER BY NamaKluster"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlNamaKluster.DataSource = ds
            ddlNamaKluster.DataTextField = "NamaKluster"
            ddlNamaKluster.DataValueField = "NamaKluster"
            ddlNamaKluster.DataBind()

            '--ALL
            ddlNamaKluster.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_namakursus_list()
        strSQL = "SELECT KodKursus,NamaKursus FROM kpmkv_kursus WITH (NOLOCK) ORDER BY NamaKursus"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlNamaKursus.DataSource = ds
            ddlNamaKursus.DataTextField = "KodKursus"
            ddlNamaKursus.DataValueField = "NamaKursus"
            ddlNamaKursus.DataBind()

            '--ALL
            ddlNamaKursus.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_semester_list()
        strSQL = "SELECT Semester FROM kpmkv_semester WITH (NOLOCK) ORDER BY Semester"
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

            '--ALL
            ddlSemester.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_sesi_list()
        strSQL = "SELECT Sesi FROM kpmkv_sesi WITH (NOLOCK) ORDER BY Sesi"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlSesi.DataSource = ds
            ddlSesi.DataTextField = "Sesi"
            ddlSesi.DataValueField = "Sesi"
            ddlSesi.DataBind()


        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_kod_list()
        strSQL = "SELECT Distinct KodMataPelajaran FROM kpmkv_matapelajaran WITH (NOLOCK) ORDER BY KodMataPelajaran"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlKodMataPelajaran.DataSource = ds
            ddlKodMataPelajaran.DataTextField = "KodMataPelajaran"
            ddlKodMataPelajaran.DataValueField = "KodMataPelajaran"
            ddlKodMataPelajaran.DataBind()

            '--ALL
            ddlKodMataPelajaran.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_menu_list()
        strSQL = "SELECT MenuCode,MenuDesc FROM kpmkv_menu WITH (NOLOCK) WHERE MenuCat='MATAPELAJARAN-LIST' ORDER BY SeqNo"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlMenuCode.DataSource = ds
            ddlMenuCode.DataTextField = "MenuDesc"
            ddlMenuCode.DataValueField = "MenuCode"
            ddlMenuCode.DataBind()

            '--Pilih
            ddlMenuCode.Items.Add(New ListItem("--Pilih--", "00"))

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
        Dim strOrder As String = " ORDER BY Tahun ASC"

        tmpSQL = "SELECT * FROM kpmkv_matapelajaran"
        strWhere = " WITH (NOLOCK) WHERE IsDeleted='N'"

        '--Tahun
        If Not ddlTahun.Text = "PILIH" Then
            strWhere += " AND Tahun='" & ddlTahun.Text & "'"
        End If

        '--NamaKluster
        If Not ddlNamaKluster.Text = "PILIH" Then
            strWhere += " AND NamaKluster='" & ddlNamaKluster.Text & "'"
        End If

        '--NamaKursus
        If Not ddlNamaKursus.Text = "PILIH" Then
            strWhere += " AND NamaKursus='" & ddlNamaKursus.Text & "'"
        End If

        '--Semester
        If Not ddlSemester.Text = "PILIH" Then
            strWhere += " AND Semester= '" & ddlSemester.Text & "'"
        End If


        '--Semester
        If Not ddlSesi.Text = "PILIH" Then
            strWhere += " AND Sesi= '" & ddlSesi.Text & "'"
        End If

        '--Kod
        If Not ddlKodMataPelajaran.Text = "PILIH" Then
            strWhere += " AND KodMataPelajaran= '" & ddlKodMataPelajaran.Text & "'"
        End If

        '--Nama
        If Not txtNamaMataPelajaran.Text.Length = 0 Then
            strWhere += " AND NamaMataPelajaran LIKE '%" & oCommon.FixSingleQuotes(txtNamaMataPelajaran.Text) & "%'"
        End If

        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        ' 'Response.Write(getSQL)

        Return getSQL

    End Function

    Private Sub datRespondent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondent.PageIndexChanging
        datRespondent.PageIndex = e.NewPageIndex
        strRet = BindData(datRespondent)

    End Sub

    Protected Sub btnExecute_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExecute.Click
        Select Case ddlMenuCode.SelectedValue
            Case "00"
                lblMsg.Text = "Sila pilih fungsi untuk dilancarkan!"

            Case "01"   '--Daftar Modul Baru
                Execute_01()

            Case "02"   '--Eksport
                Execute_02()

            Case Else
                lblMsg.Text = "Sila pilih fungsi untuk dilancarkan!"
        End Select

    End Sub

    '--Daftar Kolej Baru
    Private Sub Execute_01()
        Response.Redirect("matapelajaran.create.aspx")

    End Sub

    '--Eksport
    Private Sub Execute_02()
        Try
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

    Private Sub datRespondent_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles datRespondent.SelectedIndexChanging
        Dim strKeyID As String = datRespondent.DataKeys(e.NewSelectedIndex).Value.ToString
        Response.Redirect("matapelajaran.view.aspx?MataPelajaranID=" & strKeyID)
    End Sub

    Private Sub ddlNamaKursus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlNamaKursus.SelectedIndexChanged
        kpmkv_kod_list()
    End Sub
End Class