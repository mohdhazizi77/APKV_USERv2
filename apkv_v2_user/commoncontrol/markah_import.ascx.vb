Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Imports System.Data.Common

Partial Public Class markah_import
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""
    Dim IntTakwim As Integer = 0

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Not IsPostBack Then
                lblMsg.Text = ""

                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                '------exist takwim
                strSQL = "SELECT * FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Import Markah Akademik' AND Aktif='1'"
                If oCommon.isExist(strSQL) = True Then

                    'count data takwim
                    'Get the data from database into datatable
                    Dim cmd As New SqlCommand("SELECT TakwimID FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Import Markah Akademik' AND Aktif='1'")
                    Dim dt As DataTable = GetData(cmd)

                    For i As Integer = 0 To dt.Rows.Count - 1
                        IntTakwim = dt.Rows(i)("TakwimID")

                        strSQL = "SELECT TarikhMula,TarikhAkhir FROM kpmkv_takwim WHERE TakwimID='" & IntTakwim & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_user_login As Array
                        ar_user_login = strRet.Split("|")
                        Dim strMula As String = ar_user_login(0)
                        Dim strAkhir As String = ar_user_login(1)

                        Dim strdateNow As Date = Date.Now
                        Dim startDate = DateTime.ParseExact(strMula, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                        Dim endDate = DateTime.ParseExact(strAkhir, "dd-MM-yyyy", CultureInfo.InvariantCulture)

                        Dim ts As New TimeSpan
                        ts = endDate.Subtract(strdateNow)
                        Dim dayDiff = ts.Days

                        If strMula IsNot Nothing Then
                            If strAkhir IsNot Nothing And dayDiff >= 0 Then
                                kpmkv_tahun_list()
                                kpmkv_semester_list()

                                'checkinbox
                                strSQL = "SELECT Sesi FROM kpmkv_takwim WHERE TakwimId='" & IntTakwim & "'ORDER BY Kohort ASC"
                                strRet = oCommon.getFieldValue(strSQL)

                                If strRet = 1 Then
                                    chkSesi.Items(0).Enabled = True
                                    'chkSesi.Items(1).Enabled = False
                                Else
                                    'chkSesi.Items(0).Enabled = False
                                    chkSesi.Items(1).Enabled = True
                                End If
                            End If
                        Else
                            lblMsg.Text = "Import Markah Akademik telah ditutup!"
                        End If
                    Next
                Else
                    lblMsg.Text = "Import Markah Akademik telah ditutup!"
                End If
                RepoveDuplicate(ddlTahun)
                RepoveDuplicate(ddlSemester)
            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try
    End Sub
    Private Sub btnFile_Click(sender As Object, e As EventArgs) Handles btnFile.Click
        lblMsg.Text = ""
        'lblMsgTop.Text = ""
        'btnApprove.Enabled = True
        If ddlSemester.Text = "1" Then
            Response.ContentType = "Application/xlsx"
            Response.AppendHeader("Content-Disposition", "attachment; filename=PELAJARIMPORT.xlsx")
            Response.TransmitFile(Server.MapPath("~/sample_data/PELAJARIMPORT.xlsx"))
            Response.End()
        ElseIf ddlSemester.Text = "2" Then
            Response.ContentType = "Application/xlsx"
            Response.AppendHeader("Content-Disposition", "attachment; filename=PELAJARIMPORT.xlsx")
            Response.TransmitFile(Server.MapPath("~/sample_data/PELAJARIMPORT.xlsx"))
        ElseIf ddlSemester.Text = "3" Then
            Response.ContentType = "Application/xlsx"
            Response.AppendHeader("Content-Disposition", "attachment; filename=PELAJARIMPORT.xlsx")
            Response.TransmitFile(Server.MapPath("~/sample_data/PELAJARIMPORT.xlsx"))
        ElseIf ddlSemester.Text = "4" Then
            Response.ContentType = "Application/xlsx"
            Response.AppendHeader("Content-Disposition", "attachment; filename=PELAJARIMPORT.xlsx")
            Response.TransmitFile(Server.MapPath("~/sample_data/PELAJARIMPORT.xlsx"))
        End If

    End Sub
    Private Sub hiddencolumn()
        Select Case ddlSemester.Text
            Case "1"
                datRespondent.Columns.Item("5").Visible = True
                datRespondent.Columns.Item("6").Visible = False
                datRespondent.Columns.Item("7").Visible = False
                datRespondent.Columns.Item("8").Visible = False 'bm3
                datRespondent.Columns.Item("9").Visible = True 'bi
                datRespondent.Columns.Item("10").Visible = True 'sc
                datRespondent.Columns.Item("11").Visible = True
                datRespondent.Columns.Item("12").Visible = True
                datRespondent.Columns.Item("13").Visible = True
                datRespondent.Columns.Item("14").Visible = True
            Case "2"
                datRespondent.Columns.Item("5").Visible = True
                datRespondent.Columns.Item("6").Visible = False
                datRespondent.Columns.Item("7").Visible = False
                datRespondent.Columns.Item("8").Visible = False 'bm3
                datRespondent.Columns.Item("9").Visible = True
                datRespondent.Columns.Item("10").Visible = True
                datRespondent.Columns.Item("11").Visible = True
                datRespondent.Columns.Item("12").Visible = True
                datRespondent.Columns.Item("13").Visible = True
                datRespondent.Columns.Item("14").Visible = True

            Case "3"
                datRespondent.Columns.Item("5").Visible = False
                datRespondent.Columns.Item("6").Visible = True
                datRespondent.Columns.Item("7").Visible = True
                datRespondent.Columns.Item("8").Visible = False 'bm3
                datRespondent.Columns.Item("9").Visible = True
                datRespondent.Columns.Item("10").Visible = True
                datRespondent.Columns.Item("11").Visible = True
                datRespondent.Columns.Item("12").Visible = True
                datRespondent.Columns.Item("13").Visible = True
                datRespondent.Columns.Item("14").Visible = True

            Case "4"
                datRespondent.Columns.Item("5").Visible = False
                datRespondent.Columns.Item("6").Visible = False
                datRespondent.Columns.Item("7").Visible = False
                datRespondent.Columns.Item("8").Visible = True 'bm3
                datRespondent.Columns.Item("9").Visible = True
                datRespondent.Columns.Item("10").Visible = True
                datRespondent.Columns.Item("11").Visible = True
                datRespondent.Columns.Item("12").Visible = True
                datRespondent.Columns.Item("13").Visible = True
                datRespondent.Columns.Item("14").Visible = True


        End Select

    End Sub

    Private Sub kpmkv_tahun_list()
        strSQL = "SELECT Kohort FROM kpmkv_takwim WHERE TakwimId='" & IntTakwim & "'ORDER BY Kohort ASC"
        strRet = oCommon.getFieldValue(strSQL)
        Try
            If Not ddlTahun.Text = strRet Then
                ddlTahun.Items.Add(strRet)
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try
    End Sub
    Private Sub kpmkv_semester_list()
        strSQL = "SELECT Semester FROM kpmkv_takwim WHERE TakwimId='" & IntTakwim & "'ORDER BY Semester ASC"
        strRet = oCommon.getFieldValue(strSQL)
        Try
            If Not ddlSemester.Text = strRet Then
                ddlSemester.Items.Add(strRet)
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try
    End Sub
     Private Shared Function RepoveDuplicate(ByVal ddl As DropDownList) As DropDownList
        For Row As Int16 = 0 To ddl.Items.Count - 2
            For RowAgain As Int16 = ddl.Items.Count - 1 To Row + 1 Step -1
                If ddl.Items(Row).ToString = ddl.Items(RowAgain).ToString Then
                    ddl.Items.RemoveAt(RowAgain)
                End If
            Next
        Next
        Return ddl
    End Function
    Private Sub kpmkv_kodkursus_list()

        strSQL = "SELECT kpmkv_kursus.KodKursus, kpmkv_kursus.KursusID"
        strSQL += " FROM kpmkv_kelas_kursus INNER JOIN kpmkv_kursus ON kpmkv_kelas_kursus.KursusID = kpmkv_kursus.KursusID INNER JOIN"
        strSQL += " kpmkv_kelas ON kpmkv_kelas_kursus.KelasID = kpmkv_kelas.KelasID"
        strSQL += " WHERE kpmkv_kelas.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kursus.Tahun='" & ddlTahun.Text & "' AND kpmkv_kursus.Sesi='" & chkSesi.SelectedValue & "' ORDER BY kpmkv_kursus.KodKursus"
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

            '--ALL
            ' ddlKodKursus.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_kelas_list()
        strSQL = " SELECT kpmkv_kelas.NamaKelas, kpmkv_kelas.KelasID"
        strSQL += " FROM  kpmkv_kelas_kursus LEFT OUTER JOIN kpmkv_kelas ON kpmkv_kelas_kursus.KelasID = kpmkv_kelas.KelasID LEFT OUTER JOIN"
        strSQL += " kpmkv_kursus ON kpmkv_kelas_kursus.KursusID = kpmkv_kursus.KursusID"
        strSQL += " WHERE kpmkv_kelas.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kelas_kursus.KursusID= '" & ddlKodKursus.SelectedValue & "' ORDER BY KelasID"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlKelas.DataSource = ds
            ddlKelas.DataTextField = "NamaKelas"
            ddlKelas.DataValueField = "KelasID"
            ddlKelas.DataBind()

            '--ALL
            'ddlNamaKelas.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Sub datRespondent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondent.PageIndexChanging
        datRespondent.PageIndex = e.NewPageIndex
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

    Private Function getSQL() As String

        Dim tmpSQL As String = ""
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY kpmkv_pelajar.Nama ASC"

        '--not deleted
        tmpSQL = "SELECT kpmkv_pelajar.PelajarID, kpmkv_pelajar.Nama, kpmkv_pelajar.MYKAD, kpmkv_pelajar.AngkaGiliran, "
        tmpSQL += " kpmkv_kursus.KodKursus, kpmkv_pelajar_markah.B_BahasaMelayu1, kpmkv_pelajar_markah.B_BahasaMelayu2, kpmkv_pelajar_markah.B_BahasaMelayu3, "
        tmpSQL += " kpmkv_pelajar_markah.B_BahasaInggeris, kpmkv_pelajar_markah.B_Science1, kpmkv_pelajar_markah.B_Sejarah, "
        tmpSQL += " kpmkv_pelajar_markah.B_PendidikanIslam1, kpmkv_pelajar_markah.B_PendidikanMoral, kpmkv_pelajar_markah.B_Mathematics, kpmkv_pelajar_markah.B_BahasaMelayu"
        tmpSQL += " FROM kpmkv_pelajar_markah LEFT OUTER JOIN kpmkv_pelajar ON kpmkv_pelajar_markah.PelajarID = kpmkv_pelajar.PelajarID"
        tmpSQL += " LEFT OUTER Join kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID"
        strWhere = " WHERE kpmkv_pelajar.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.StatusID='2' AND kpmkv_pelajar.JenisCalonID='2'"

        '--tahun
        If Not ddlTahun.Text = "PILIH" Then
            strWhere += " AND kpmkv_pelajar.Tahun ='" & ddlTahun.Text & "'"
        End If
        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere += " AND kpmkv_pelajar.Sesi ='" & chkSesi.Text & "'"
        End If
        '--semester
        If Not ddlSemester.Text = "" Then
            strWhere += " AND kpmkv_pelajar.Semester ='" & ddlSemester.Text & "'"
        End If
        '--Kod
        If Not ddlKodKursus.Text = "" Then
            strWhere += " AND kpmkv_pelajar.KursusID='" & ddlKodKursus.SelectedValue & "'"
        End If
        '--sesi
        If Not ddlKelas.Text = "" Then
            strWhere += " AND kpmkv_pelajar.KelasID ='" & ddlKelas.SelectedValue & "'"
        End If
        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        ''Response.Write(getSQL)

        Return getSQL

    End Function

    Private Sub datRespondent_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles datRespondent.SelectedIndexChanging
        Dim strKeyID As String = datRespondent.DataKeys(e.NewSelectedIndex).Value.ToString

    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Try
            ExportToCSV(getSQL)

        Catch ex As Exception
            lblMsg.Text = "Error:" & ex.Message
        End Try
    End Sub

    Private Sub ExportToCSV(ByVal strQuery As String)
        'Get the data from database into datatable 
        Dim cmd As New SqlCommand(strQuery)
        Dim dt As DataTable = GetData(cmd)

        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=KOKO_File.csv")
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

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        lblMsg.Text = ""

        Try
            If ValidateForm() = False Then
                lblMsgResult.Text = "Sila masukkan NOMBOR SAHAJA"
                lblMsg.Text = "Sila masukkan NOMBOR 0-100 SAHAJA"
                Exit Sub
            End If

            For i As Integer = 0 To datRespondent.Rows.Count - 1
                Dim row As GridViewRow = datRespondent.Rows(i)
                Dim strBahasaMelayu As TextBox = datRespondent.Rows(i).FindControl("B_BahasaMelayu")
                Dim strBahasaMelayu1 As TextBox = datRespondent.Rows(i).FindControl("B_BahasaMelayu1")
                Dim strBahasaMelayu2 As TextBox = datRespondent.Rows(i).FindControl("B_BahasaMelayu2")
                Dim strBahasaMelayu3 As TextBox = datRespondent.Rows(i).FindControl("B_BahasaMelayu3")
                Dim strBahasaInggeris As TextBox = datRespondent.Rows(i).FindControl("B_BahasaInggeris")
                Dim strScience As TextBox = datRespondent.Rows(i).FindControl("B_Science1")
                Dim strSejarah As TextBox = datRespondent.Rows(i).FindControl("B_Sejarah")
                Dim strPendidikanIslam As TextBox = datRespondent.Rows(i).FindControl("B_PendidikanIslam1")
                Dim strPendidikanMoral As TextBox = datRespondent.Rows(i).FindControl("B_PendidikanMoral")
                Dim strMatematik As TextBox = datRespondent.Rows(i).FindControl("B_Mathematics")


                'assign value to integer
                Dim BM As Integer = strBahasaMelayu.Text
                Dim BM1 As Integer = strBahasaMelayu1.Text
                Dim BM2 As Integer = strBahasaMelayu2.Text
                Dim BM3 As Integer = strBahasaMelayu3.Text
                Dim BI As Integer = strBahasaInggeris.Text
                Dim SC As Integer = strScience.Text
                Dim SEJ As Integer = strSejarah.Text
                Dim PI As Integer = strPendidikanIslam.Text
                Dim PM As Integer = strPendidikanMoral.Text
                Dim Matematik As Integer = strMatematik.Text


                strSQL = "UPDATE kpmkv_pelajar_markah SET B_BahasaMelayu='" & BM & "', B_BahasaMelayu1='" & BM1 & "', B_BahasaMelayu2='" & BM2 & "', "
                strSQL += " B_BahasaMelayu3='" & BM3 & "', B_BahasaInggeris='" & BI & "', B_Science1='" & SC & "', B_Sejarah='" & SEJ & "',"
                strSQL += " B_PendidikanIslam1='" & PI & "', B_PendidikanMoral='" & PM & "',"
                strSQL += " B_Mathematics='" & Matematik & "' WHERE KolejRecordID='" & lblKolejID.Text & "' AND PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"

                strRet = oCommon.ExecuteSQL(strSQL)
                If strRet = "0" Then
                    divMsgResult.Attributes("class") = "info"
                    lblMsgResult.Text = "Berjaya!.Kemaskini markah Pentaksiran Berterusan Akademik."
                Else
                    divMsgResult.Attributes("class") = "error"
                    lblMsgResult.Text = "Tidak Berjaya!.Kemaskini markah Pentaksiran Berterusan Akademik."
                End If
            Next

        Catch ex As Exception
            lblMsg.Text = "Error:" & ex.Message
        End Try

        strRet = BindData(datRespondent)
    End Sub
    Private Function ValidateForm() As Boolean
        For i As Integer = 0 To datRespondent.Rows.Count - 1
            Dim row As GridViewRow = datRespondent.Rows(i)
            Dim strBahasaMelayu As TextBox = CType(row.FindControl("B_BahasaMelayu"), TextBox)
            Dim strBahasaMelayu1 As TextBox = CType(row.FindControl("B_BahasaMelayu1"), TextBox)
            Dim strBahasaMelayu2 As TextBox = CType(row.FindControl("B_BahasaMelayu2"), TextBox)
            Dim strBahasaMelayu3 As TextBox = CType(row.FindControl("B_BahasaMelayu3"), TextBox)
            Dim strBahasaInggeris As TextBox = CType(row.FindControl("B_BahasaInggeris"), TextBox)
            Dim strScience As TextBox = CType(row.FindControl("B_Science1"), TextBox)
            Dim strSejarah As TextBox = CType(row.FindControl("B_Sejarah"), TextBox)
            Dim strPendidikanIslam As TextBox = CType(row.FindControl("B_PendidikanIslam1"), TextBox)
            Dim strPendidikanMoral As TextBox = CType(row.FindControl("B_PendidikanMoral"), TextBox)
            Dim strMatematik As TextBox = CType(row.FindControl("B_Mathematics"), TextBox)

            '--validate NUMBER and less than 100
            '--strBahasaMelayu
            If Not strBahasaMelayu.Text.Length = 0 Then
                If oCommon.IsCurrency(strBahasaMelayu.Text) = False Then
                    Return False
                End If
                If CInt(strBahasaMelayu.Text) > 100 Then
                    Return False
                End If
                If CInt(strBahasaMelayu.Text) = -1 Then
                    Return False
                End If
            Else
                strBahasaMelayu.Text = "0"
            End If

            '--strBahasaMelayu1
            If Not strBahasaMelayu1.Text.Length = 0 Then
                If oCommon.IsCurrency(strBahasaMelayu1.Text) = False Then
                    Return False
                End If
                If CInt(strBahasaMelayu1.Text) > 100 Then
                    Return False
                End If
                If CInt(strBahasaMelayu1.Text) = -1 Then
                    Return False
                End If
            Else
                strBahasaMelayu1.Text = "0"
            End If

            '--strBahasaMelayu2
            If Not strBahasaMelayu2.Text.Length = 0 Then
                If oCommon.IsCurrency(strBahasaMelayu2.Text) = False Then
                    Return False
                End If
                If CInt(strBahasaMelayu2.Text) > 100 Then
                    Return False
                End If
                If CInt(strBahasaMelayu2.Text) = -1 Then
                    Return False
                End If
            Else
                strBahasaMelayu2.Text = "0"
            End If

            '--strBahasaMelayu3
            If Not strBahasaMelayu3.Text.Length = 0 Then
                If oCommon.IsCurrency(strBahasaMelayu3.Text) = False Then
                    Return False
                End If
                If CInt(strBahasaMelayu3.Text) > 100 Then
                    Return False
                End If
                If CInt(strBahasaMelayu3.Text) = -1 Then
                    Return False
                End If
            Else
                strBahasaMelayu3.Text = "0"
            End If
            '--strBahasaInggeris
            If Not strBahasaInggeris.Text.Length = 0 Then
                If oCommon.IsCurrency(strBahasaInggeris.Text) = False Then
                    Return False
                End If
                If CInt(strBahasaInggeris.Text) > 100 Then
                    Return False
                End If
                If CInt(strBahasaInggeris.Text) = -1 Then
                    Return False
                End If
            Else
                strBahasaInggeris.Text = "0"
            End If

            '--strScience
            If Not strScience.Text.Length = 0 Then
                If oCommon.IsCurrency(strScience.Text) = False Then
                    Return False
                End If
                If CInt(strScience.Text) > 100 Then
                    Return False
                End If
                If CInt(strScience.Text) = -1 Then
                    Return False
                End If
            Else
                strScience.Text = "0"
            End If

            '--strSejarah
            If Not strSejarah.Text.Length = 0 Then
                If oCommon.IsCurrency(strSejarah.Text) = False Then
                    Return False
                End If
                If CInt(strSejarah.Text) > 100 Then
                    Return False
                End If
                If CInt(strSejarah.Text) = -1 Then
                    Return False
                End If
            Else
                strSejarah.Text = "0"
            End If

            '--strPendidikanIslam
            If Not strPendidikanIslam.Text.Length = 0 Then
                If oCommon.IsCurrency(strPendidikanIslam.Text) = False Then
                    Return False
                End If
                If CInt(strPendidikanIslam.Text) > 100 Then
                    Return False
                End If
                If CInt(strPendidikanIslam.Text) = -1 Then
                    Return False
                End If
            Else
                strPendidikanIslam.Text = "0"
            End If

            '--strPendidikanMoral
            If Not strPendidikanMoral.Text.Length = 0 Then
                If oCommon.IsCurrency(strPendidikanMoral.Text) = False Then
                    Return False
                End If
                If CInt(strPendidikanMoral.Text) > 100 Then
                    Return False
                End If
                If CInt(strPendidikanMoral.Text) = -1 Then
                    Return False
                End If
            Else
                strPendidikanMoral.Text = "0"
            End If

            'strMatematik
            If Not strMatematik.Text.Length = 0 Then
                If oCommon.IsCurrency(strMatematik.Text) = False Then
                    Return False
                End If
                If CInt(strMatematik.Text) > 100 Then
                    Return False
                End If
                If CInt(strMatematik.Text) = -1 Then
                    Return False
                End If
            Else
                strMatematik.Text = "0"
            End If
        Next

        Return True
    End Function
    
    Private Sub chkSesi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkSesi.SelectedIndexChanged
        kpmkv_kodkursus_list()
        kpmkv_kelas_list()
    End Sub
End Class

