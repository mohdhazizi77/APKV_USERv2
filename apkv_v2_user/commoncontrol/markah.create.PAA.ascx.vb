Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Globalization
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class markah_create_PAA
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String
    Dim strRet As String
    Dim IntTakwim As Integer = 0

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                lblMsg.Text = ""
                lblMsgResult.Text = ""

                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                '------exist takwim
                strSQL = "SELECT * FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Penilaian Akhir Akademik' AND Aktif='1'"
                If oCommon.isExist(strSQL) = True Then

                    'count data takwim
                    'Get the data from database into datatable
                    Dim cmd As New SqlCommand("SELECT TakwimID FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Penilaian Akhir Akademik' AND Aktif='1'")
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
                                ' DDL_RemoveDuplicateItems(ddlTahun)

                                kpmkv_semester_list()
                                'DDL_RemoveDuplicateItems(ddlSemester)

                                'checkinbox
                                strSQL = "SELECT Sesi FROM kpmkv_takwim WHERE TakwimId='" & IntTakwim & "'ORDER BY Kohort ASC"
                                strRet = oCommon.getFieldValue(strSQL)

                                If strRet = 1 Then
                                    chkSesi.Items(0).Enabled = True
                                    ' chkSesi.Items(1).Enabled = False
                                Else
                                    ' chkSesi.Items(0).Enabled = False
                                    chkSesi.Items(1).Enabled = True
                                End If
                                btnGred.Enabled = True
                                btnUpdate.Enabled = True
                            End If
                        Else
                            btnGred.Enabled = False
                            btnUpdate.Enabled = False
                            lblMsg.Text = "Penilaian Akhir Akademik telah ditutup!"
                        End If
                    Next
                Else
                    btnGred.Enabled = False
                    btnUpdate.Enabled = False
                    lblMsg.Text = "Penilaian Akhir Akademik telah ditutup!"
                End If
                RepoveDuplicate(ddlTahun)
                RepoveDuplicate(ddlSemester)
            End If
           

        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try
    End Sub
    Private Sub kpmkv_tahun_list()
         strSQL = "SELECT Kohort FROM kpmkv_takwim WHERE TakwimId='" & IntTakwim & "'ORDER BY Kohort ASC"
        strRet = oCommon.getFieldValue(strSQL)
        Try

            ddlTahun.Items.Add(strRet)

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
            ddlSemester.Items.Add(strRet)

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
        strSQL += " WHERE kpmkv_kelas.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kursus.Tahun='" & ddlTahun.Text & "'"
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

            '--ALL
            'ddlKodKursus.Items.Add(New ListItem("-Pilih-", "0"))

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
            'ddlKelas.Items.Add(New ListItem("-Pilih-", "0"))

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
        tmpSQL = "SELECT kpmkv_pelajar.PelajarID, kpmkv_pelajar.Tahun, kpmkv_pelajar.Semester, kpmkv_pelajar.Sesi, kpmkv_pelajar.Nama, kpmkv_pelajar.AngkaGiliran, "
        tmpSQL += " kpmkv_pelajar.MYKAD, kpmkv_kursus.KodKursus, kpmkv_pelajar_markah.A_BahasaMelayu, kpmkv_pelajar_markah.A_BahasaMelayu1, kpmkv_pelajar_markah.A_BahasaMelayu2, kpmkv_pelajar_markah.A_BahasaMelayu3, "
        tmpSQL += " kpmkv_pelajar_markah.A_BahasaInggeris, kpmkv_pelajar_markah.A_Science1, kpmkv_pelajar_markah.A_Science2, kpmkv_pelajar_markah.A_Sejarah, kpmkv_pelajar_markah.A_PendidikanIslam1, "
        tmpSQL += " kpmkv_pelajar_markah.A_PendidikanIslam2, kpmkv_pelajar_markah.A_PendidikanMoral, kpmkv_pelajar_markah.A_Mathematics"
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
        lblMsgResult.Text = ""

        Try
            If ValidateForm() = False Then
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Sila Semak Data Markah.[Huruf],[Kosong] adalah tidak dibenarkan!  Markah mestilah diantara -1-100 sahaja"
                divMsgResult.Attributes("class") = "error"
                lblMsgResult.Text = "Sila Semak Data Markah.[Huruf],[Kosong] adalah tidak dibenarkan!  Markah mestilah diantara -1-100 sahaja"

                Exit Sub
            End If

            For i As Integer = 0 To datRespondent.Rows.Count - 1
                Dim row As GridViewRow = datRespondent.Rows(i)
                Dim strBahasaMelayu As TextBox = datRespondent.Rows(i).FindControl("A_BahasaMelayu")
                Dim strBahasaMelayu1 As TextBox = datRespondent.Rows(i).FindControl("A_BahasaMelayu1")
                Dim strBahasaMelayu2 As TextBox = datRespondent.Rows(i).FindControl("A_BahasaMelayu2")
                Dim strBahasaMelayu3 As TextBox = datRespondent.Rows(i).FindControl("A_BahasaMelayu3")
                Dim strBahasaInggeris As TextBox = datRespondent.Rows(i).FindControl("A_BahasaInggeris")
                Dim strScience1 As TextBox = datRespondent.Rows(i).FindControl("A_Science1")
                Dim strScience2 As TextBox = datRespondent.Rows(i).FindControl("A_Science2")
                Dim strSejarah As TextBox = datRespondent.Rows(i).FindControl("A_Sejarah")
                Dim strPendidikanIslam1 As TextBox = datRespondent.Rows(i).FindControl("A_PendidikanIslam1")
                Dim strPendidikanIslam2 As TextBox = datRespondent.Rows(i).FindControl("A_PendidikanIslam2")
                Dim strPendidikanMoral As TextBox = datRespondent.Rows(i).FindControl("A_PendidikanMoral")
                Dim strMatematik As TextBox = datRespondent.Rows(i).FindControl("A_Mathematics")


                'assign value to integer
                Dim BM As String = strBahasaMelayu.Text
                Dim BM1 As String = strBahasaMelayu1.Text
                Dim BM2 As String = strBahasaMelayu2.Text
                Dim BM3 As String = strBahasaMelayu3.Text
                Dim BI As String = strBahasaInggeris.Text
                Dim SC1 As String = strScience1.Text
                Dim SC2 As String = strScience2.Text
                Dim SEJ As String = strSejarah.Text
                Dim PI1 As String = strPendidikanIslam1.Text
                Dim PI2 As String = strPendidikanIslam2.Text
                Dim PM As String = strPendidikanMoral.Text
                Dim Matematik As String = strMatematik.Text

                strSQL = "UPDATE kpmkv_pelajar_markah SET A_BahasaMelayu='" & BM & "', "
                strSQL += " A_BahasaMelayu3='" & BM3 & "', A_BahasaInggeris='" & BI & "', A_Science1='" & SC1 & "',"
                If Not ddlSemester.SelectedValue = "4" Then
                    strSQL += " A_Sejarah='" & SEJ & "',"
                End If
                strSQL += " A_PendidikanIslam1='" & PI1 & "', A_PendidikanMoral='" & PM & "', A_Science2='" & SC2 & "',"
                strSQL += " A_PendidikanIslam2='" & PI2 & "', A_Mathematics='" & Matematik & "'"
                strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' AND PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
                If strRet = "0" Then
                    divMsgResult.Attributes("class") = "info"
                    lblMsgResult.Text = "Berjaya!.Kemaskini markah Pentaksiran Akhir Akademik."
                Else
                    divMsgResult.Attributes("class") = "error"
                    lblMsgResult.Text = "Tidak Berjaya!.Kemaskini markah Pentaksir Akhiran Akademik."
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
            Dim strBahasaMelayu As TextBox = CType(row.FindControl("A_BahasaMelayu"), TextBox)
            Dim strBahasaMelayu1 As TextBox = CType(row.FindControl("A_BahasaMelayu1"), TextBox)
            Dim strBahasaMelayu2 As TextBox = CType(row.FindControl("A_BahasaMelayu2"), TextBox)
            Dim strBahasaMelayu3 As TextBox = CType(row.FindControl("A_BahasaMelayu3"), TextBox)
            Dim strBahasaInggeris As TextBox = CType(row.FindControl("A_BahasaInggeris"), TextBox)
            Dim strScience1 As TextBox = CType(row.FindControl("A_Science1"), TextBox)
            Dim strScience2 As TextBox = CType(row.FindControl("A_Science2"), TextBox)
            Dim strSejarah As TextBox = CType(row.FindControl("A_Sejarah"), TextBox)
            Dim strPendidikanIslam1 As TextBox = CType(row.FindControl("A_PendidikanIslam1"), TextBox)
            Dim strPendidikanIslam2 As TextBox = CType(row.FindControl("A_PendidikanIslam2"), TextBox)
            Dim strPendidikanMoral As TextBox = CType(row.FindControl("A_PendidikanMoral"), TextBox)
            Dim strMatematik As TextBox = CType(row.FindControl("A_Mathematics"), TextBox)


            '--validate NUMBER and less than 100
            '--strBahasaMelayu

            'If strBahasaMelayu.Text.Length = 0 Then
            '    Return False
            'End If
            If Not strBahasaMelayu.Text = "" Then
                If oCommon.IsCurrency(strBahasaMelayu.Text) = False Then
                    Return False
                End If

                If CInt(strBahasaMelayu.Text) > 100 Then
                    Return False
                End If
                If CInt(strBahasaMelayu.Text) < -1 Then
                    Return False
                End If
            End If
            If ddlSemester.SelectedValue = "4" Then

                '--strBahasaMelayu1
                'If strBahasaMelayu1.Text.Length = 0 Then
                '    Return False
                'End If
                'If oCommon.IsCurrency(strBahasaMelayu1.Text) = False Then
                '    Return False
                'End If
                'If CInt(strBahasaMelayu1.Text) > 100 Then
                '    Return False
                'End If
                'If CInt(strBahasaMelayu1.Text) < -1 Then
                '    Return False
                'End If

                ''--strBahasaMelayu2
                'If strBahasaMelayu2.Text.Length = 0 Then
                '    Return False
                'End If
                'If oCommon.IsCurrency(strBahasaMelayu2.Text) = False Then
                '    Return False
                'End If
                'If CInt(strBahasaMelayu2.Text) > 100 Then
                '    Return False
                'End If
                'If CInt(strBahasaMelayu2.Text) < -1 Then
                '    Return False
                'End If

                '--strBahasaMelayu3
                'If strBahasaMelayu3.Text.Length = 0 Then
                '    Return False
                'End If
                If Not strBahasaMelayu3.Text = "" Then
                    If oCommon.IsCurrency(strBahasaMelayu3.Text) = False Then
                        Return False
                    End If

                    If CInt(strBahasaMelayu3.Text) > 100 Then
                        Return False
                    End If
                    If CInt(strBahasaMelayu3.Text) < -1 Then
                        Return False
                    End If
                End If
            End If
            '--strBahasaInggeris

            'If strBahasaInggeris.Text.Length = 0 Then
            '    Return False
            'End If
            If Not strBahasaInggeris.Text = "" Then
                If oCommon.IsCurrency(strBahasaInggeris.Text) = False Then
                    Return False
                End If

                If CInt(strBahasaInggeris.Text) > 100 Then
                    Return False
                End If
                If CInt(strBahasaInggeris.Text) < -1 Then
                    Return False
                End If
            End If
            '--strScience

            'If strScience1.Text.Length = 0 Then
            '    Return False
            'End If
            If Not strScience1.Text = "" Then
                If oCommon.IsCurrency(strScience1.Text) = False Then
                    Return False
                End If

                If CInt(strScience1.Text) > 100 Then
                    Return False
                End If
                If CInt(strScience1.Text) < -1 Then
                    Return False
                End If
            End If
            If ddlSemester.SelectedValue = "1" Or ddlSemester.SelectedValue = "2" Then

                'If strScience2.Text.Length = 0 Then
                '    Return False
                'End If
                If Not strScience2.Text = "" Then
                    If oCommon.IsCurrency(strScience2.Text) = False Then
                        Return False
                    End If

                    If CInt(strScience2.Text) > 100 Then
                        Return False
                    End If
                    If CInt(strScience2.Text) < -1 Then
                        Return False
                    End If
                End If
            End If
            '--strSejarah

            'If strSejarah.Text.Length = 0 Then
            '    Return False
            'End If
            If Not ddlSemester.SelectedValue = "4" Then
                If Not strSejarah.Text = "" Then
                    If oCommon.IsCurrency(strSejarah.Text) = False Then
                        Return False
                    End If

                    If CInt(strSejarah.Text) > 100 Then
                        Return False
                    End If
                    If CInt(strSejarah.Text) < -1 Then
                        Return False
                    End If
                End If
            End If
            '--strPendidikanIslam
            If strPendidikanIslam1.Text.Length = 0 Then
                strPendidikanIslam1.Text = 0
            End If
            If oCommon.IsCurrency(strPendidikanIslam1.Text) = False Then
                Return False
            End If
            If CInt(strPendidikanIslam1.Text) > 100 Then
                Return False
            End If
            If CInt(strPendidikanIslam1.Text) < -1 Then
                Return False
            End If



            If strPendidikanIslam2.Text.Length = 0 Then
                strPendidikanIslam2.Text = 0
            End If
            If oCommon.IsCurrency(strPendidikanIslam2.Text) = False Then
                Return False
            End If
            If CInt(strPendidikanIslam2.Text) > 100 Then
                Return False
            End If
            If CInt(strPendidikanIslam2.Text) < -1 Then
                Return False
            End If

            '--strPendidikanMoral
            If strPendidikanMoral.Text.Length = 0 Then
                strPendidikanMoral.Text = 0
            End If
            If oCommon.IsCurrency(strPendidikanMoral.Text) = False Then
                Return False
            End If
            If CInt(strPendidikanMoral.Text) > 100 Then
                Return False
            End If
            If CInt(strPendidikanMoral.Text) < -1 Then
                Return False
            End If


            'strMatematik
            'If strMatematik.Text.Length = 0 Then
            '    Return False
            'End If
            If Not strMatematik.Text = "" Then
                If oCommon.IsCurrency(strMatematik.Text) = False Then
                    Return False
                End If

                If CInt(strMatematik.Text) > 100 Then
                    Return False
                End If
                If CInt(strMatematik.Text) < -1 Then
                    Return False
                End If
            End If
        Next

        Return True
    End Function

    Private Sub Akademik_markah()

        Dim strKodMP As Integer

        Select Case ddlSemester.Text
            Case "1"
                strKodMP = "100"
            Case "2"
                strKodMP = "200"
            Case "3"
                strKodMP = "300"
            Case "4"
                strKodMP = "400"
        End Select

        For i As Integer = 0 To datRespondent.Rows.Count - 1
            ' Dim GredBM As Integer
            Dim BerterusanBM As Integer
            Dim AkhiranBM As Integer


            If ddlSemester.Text = "1" Then
                Dim AM_BahasaMelayu As Integer
                Dim BM_BahasaMelayu As Integer
                Dim B_BahasaMelayu As Double
                Dim A_BahasaMelayu As Double
                Dim PointerBM As Integer

                'strSQL = "Select PB from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A01'+'" & strKodMP & "%' AND Tahun='" & ddlTahun.Text & "'"
                strSQL = "SELECT PB FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
                BerterusanBM = oCommon.getFieldValue(strSQL)

                ' strSQL = "Select PA from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A01'+'" & strKodMP & "%' AND Tahun='" & ddlTahun.Text & "'"
                strSQL = "SELECT PA FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
                AkhiranBM = oCommon.getFieldValue(strSQL)

                strSQL = "Select B_BahasaMelayu from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='1'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_BahasaMelayu = oCommon.getFieldValue(strSQL)
                'round up
                B_BahasaMelayu = Math.Ceiling(B_BahasaMelayu)

                strSQL = "Select A_BahasaMelayu from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='1'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                A_BahasaMelayu = oCommon.getFieldValue(strSQL)
                'round up
                A_BahasaMelayu = Math.Ceiling(A_BahasaMelayu)

                'checkin Markah
                If Not (B_BahasaMelayu) = "-1" And Not (A_BahasaMelayu) = "-1" Then
                    BM_BahasaMelayu = Math.Ceiling((B_BahasaMelayu / 100) * BerterusanBM)
                    AM_BahasaMelayu = Math.Ceiling((A_BahasaMelayu / 100) * AkhiranBM)
                    PointerBM = Math.Ceiling(BM_BahasaMelayu + AM_BahasaMelayu)
                    strSQL = "UPDATE kpmkv_pelajar_markah SET BahasaMelayu='" & PointerBM & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='1'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)

                ElseIf (B_BahasaMelayu) = "-1" Or (A_BahasaMelayu) = "-1" Then
                    strSQL = "UPDATE kpmkv_pelajar_markah SET BahasaMelayu='-1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='1'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                End If
                'Semester1

            ElseIf ddlSemester.Text = "2" Then
                Dim AM_BahasaMelayu2 As Integer
                Dim BM_BahasaMelayu2 As Integer
                Dim B_BahasaMelayu2 As Double
                Dim A_BahasaMelayu2 As Double
                Dim PointerBM2 As Integer

                'strSQL = "Select PB from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A02'+'" & strKodMP & "%' AND Tahun='" & ddlTahun.Text & "'"
                strSQL = "SELECT PB FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
                BerterusanBM = oCommon.getFieldValue(strSQL)

                'strSQL = "Select PA from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A02'+'" & strKodMP & "%' AND Tahun='" & ddlTahun.Text & "'"
                strSQL = "SELECT PA FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
                AkhiranBM = oCommon.getFieldValue(strSQL)

                strSQL = "Select B_BahasaMelayu from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='2'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_BahasaMelayu2 = oCommon.getFieldValue(strSQL)
                'round up
                B_BahasaMelayu2 = Math.Ceiling(B_BahasaMelayu2)

                strSQL = "Select A_BahasaMelayu from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='2'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                A_BahasaMelayu2 = oCommon.getFieldValue(strSQL)
                'round up
                A_BahasaMelayu2 = Math.Ceiling(A_BahasaMelayu2)

                'checkin Markah
                If Not (B_BahasaMelayu2) = "-1" And Not (A_BahasaMelayu2) = "-1" Then
                    BM_BahasaMelayu2 = Math.Ceiling((B_BahasaMelayu2 / 100) * BerterusanBM)
                    AM_BahasaMelayu2 = Math.Ceiling((A_BahasaMelayu2 / 100) * AkhiranBM)
                    PointerBM2 = Math.Ceiling(BM_BahasaMelayu2 + AM_BahasaMelayu2)
                    strSQL = "UPDATE kpmkv_pelajar_markah SET BahasaMelayu='" & PointerBM2 & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='2'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)

                ElseIf (B_BahasaMelayu2) = "-1" Or (A_BahasaMelayu2) = "-1" Then
                    strSQL = "UPDATE kpmkv_pelajar_markah SET BahasaMelayu='-1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='2'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                End If
                'semester2

            ElseIf ddlSemester.Text = "3" Then

                Dim AM_BahasaMelayu3 As Integer
                Dim BM_BahasaMelayu3 As Integer
                Dim B_BahasaMelayu3 As Double
                Dim A_BahasaMelayu3 As Double
                Dim PointerBM3 As Integer

                'strSQL = "Select PB from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A03'+'" & strKodMP & "%' AND Tahun='" & ddlTahun.Text & "'"
                strSQL = "SELECT PB FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
                BerterusanBM = oCommon.getFieldValue(strSQL)

                'strSQL = "Select PA from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A03'+'" & strKodMP & "%' AND Tahun='" & ddlTahun.Text & "'"
                strSQL = "SELECT PA FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
                AkhiranBM = oCommon.getFieldValue(strSQL)

                strSQL = "Select B_BahasaMelayu from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='3'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_BahasaMelayu3 = oCommon.getFieldValue(strSQL)
                'round up
                B_BahasaMelayu3 = Math.Ceiling(B_BahasaMelayu3)

                strSQL = "Select A_BahasaMelayu from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='3'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                A_BahasaMelayu3 = oCommon.getFieldValue(strSQL)
                'round up
                A_BahasaMelayu3 = Math.Ceiling(A_BahasaMelayu3)

                'checkin Markah
                If Not (B_BahasaMelayu3) = "-1" And Not (A_BahasaMelayu3) = "-1" Then
                    BM_BahasaMelayu3 = Math.Ceiling((B_BahasaMelayu3 / 100) * BerterusanBM)
                    AM_BahasaMelayu3 = Math.Ceiling((A_BahasaMelayu3 / 100) * AkhiranBM)
                    PointerBM3 = Math.Ceiling(BM_BahasaMelayu3 + AM_BahasaMelayu3)
                    strSQL = "UPDATE kpmkv_pelajar_markah SET BahasaMelayu='" & PointerBM3 & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='3'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)

                ElseIf (B_BahasaMelayu3) = "-1" Or (A_BahasaMelayu3) = "-1" Then
                    strSQL = "UPDATE kpmkv_pelajar_markah SET BahasaMelayu='-1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='3'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                End If
                'semester3

            ElseIf ddlSemester.Text = "4" Then
                Dim PB4 As Integer
                ' Dim PA4 As Integer
                Dim PABmSetara As Integer
                Dim PAPB4 As Integer
                ' Dim PAPB As Integer
                Dim B_BahasaMelayuSem1 As Integer
                Dim B_BahasaMelayuSem2 As Integer
                Dim B_BahasaMelayuSem3 As Integer
                Dim B_BahasaMelayuSem4 As Integer
                Dim A_BahasaMelayuSem4 As Integer
                Dim PointerBMSetara As Integer

                'strSQL = "Select PB from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A04'+'" & strKodMP & "%' AND Tahun='" & ddlTahun.Text & "'"
                strSQL = "SELECT PB FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
                BerterusanBM = oCommon.getFieldValue(strSQL)

                'strSQL = "Select PA from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A04'+'" & strKodMP & "%' AND Tahun='" & ddlTahun.Text & "'"
                strSQL = "SELECT PA FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
                AkhiranBM = oCommon.getFieldValue(strSQL)

                'get mykad
                strSQL = " SELECT Mykad FROM kpmkv_pelajar"
                strSQL += " WHERE PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                Dim strMYKAD1 As String = oCommon.getFieldValue(strSQL)


                'get pelajarid
                strSQL = " SELECT PelajarID FROM kpmkv_pelajar"
                strSQL += " WHERE StatusID='2' AND IsDeleted='N' AND Semester='1'"
                strSQL += " AND Mykad='" & strMYKAD1 & "'"
                Dim strPelajarID1 As String = oCommon.getFieldValue(strSQL)

                'get bm sem 1
                strSQL = "Select BahasaMelayu from kpmkv_pelajar_markah Where PelajarID='" & strPelajarID1 & "'"
                strSQL += " AND Semester='1' "
                B_BahasaMelayuSem1 = oCommon.getFieldValue(strSQL)
                'round up
                B_BahasaMelayuSem1 = Math.Ceiling(B_BahasaMelayuSem1)
                '----------------------------------------------------------------------------

                'get pelajarid
                strSQL = " SELECT PelajarID FROM kpmkv_pelajar"
                strSQL += " WHERE StatusID='2' AND IsDeleted='N' AND Semester='2'"
                strSQL += " AND Mykad='" & strMYKAD1 & "'"
                Dim strPelajarID2 As String = oCommon.getFieldValue(strSQL)

                'get Bm sem 2
                strSQL = "Select BahasaMelayu from kpmkv_pelajar_markah Where PelajarID='" & strPelajarID2 & "'"
                strSQL += " AND Semester='2' "
                B_BahasaMelayuSem2 = oCommon.getFieldValue(strSQL)
                'round up
                B_BahasaMelayuSem2 = Math.Ceiling(B_BahasaMelayuSem2)


                'get pelajarid
                strSQL = " SELECT PelajarID FROM kpmkv_pelajar"
                strSQL += " WHERE StatusID='2' AND IsDeleted='N' AND Semester='3'"
                strSQL += " AND Mykad='" & strMYKAD1 & "'"
                Dim strPelajarID3 As String = oCommon.getFieldValue(strSQL)

                'get bm sem 3
                strSQL = "Select BahasaMelayu from kpmkv_pelajar_markah Where PelajarID='" & strPelajarID3 & "'"
                strSQL += " AND Semester='3' "
                B_BahasaMelayuSem3 = oCommon.getFieldValue(strSQL)
                'round up
                B_BahasaMelayuSem3 = Math.Ceiling(B_BahasaMelayuSem3)

                'get bm sem 4 PB
                strSQL = "Select B_BahasaMelayu from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='4'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_BahasaMelayuSem4 = oCommon.getFieldValue(strSQL)

                'get bm sem 4 PA
                strSQL = "Select A_BahasaMelayu3 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='4'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                A_BahasaMelayuSem4 = oCommon.getFieldValue(strSQL)

                Dim Kertas1 As Integer = 0
                Dim Kertas2 As Integer = 0

                strSQL = "SELECT A_BahasaMelayu1, A_BahasaMelayu2 FROM kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='4'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.getFieldValueEx(strSQL)

                ''--get user info
                Dim ar_Kertas As Array
                ar_Kertas = strRet.Split("|")

                If (String.IsNullOrEmpty(ar_Kertas(0).ToString())) Then
                    Kertas1 = 0
                Else
                    Kertas1 = ar_Kertas(0)
                End If

                If (String.IsNullOrEmpty(ar_Kertas(1).ToString())) Then
                    Kertas2 = 0
                Else
                    Kertas2 = ar_Kertas(1)
                End If

                If Not ((B_BahasaMelayuSem4) = "-1" Or (A_BahasaMelayuSem4) = "-1") Then
                    PB4 = Math.Ceiling((B_BahasaMelayuSem4 / 100) * BerterusanBM)
                    'PABmSetara = Math.Ceiling(A_BahasaMelayuSem4)

                    PABmSetara = Math.Ceiling((A_BahasaMelayuSem4 / 100) * 40)
                    PAPB4 = Math.Ceiling(((Kertas1 + Kertas2 + PABmSetara) / 280) * AkhiranBM)
                    'PAPB4 = Math.Ceiling(PAPB * AkhiranBM)

                    'gred sem 4 
                    Dim PointSem4 As Integer = Math.Ceiling(PB4 + PAPB4)
                    strSQL = "UPDATE kpmkv_pelajar_markah SET BahasaMelayu='" & PointSem4 & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='4'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)

                    If (B_BahasaMelayuSem1 = "-1" Or B_BahasaMelayuSem2 = "-1" Or B_BahasaMelayuSem3 = "-1") Then
                        PointerBMSetara = "-1"
                    Else
                        PointerBMSetara = Math.Ceiling((((B_BahasaMelayuSem1 / 100) * 10) + ((B_BahasaMelayuSem2 / 100) * 10) + ((B_BahasaMelayuSem3 / 100) * 10) + ((PointSem4 / 100) * 70)))
                    End If

                    strSQL = "UPDATE kpmkv_pelajar_markah SET PointerBMSetara='" & PointerBMSetara & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='4'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)

                ElseIf ((B_BahasaMelayuSem4) = "-1" Or (A_BahasaMelayuSem4) = "-1") Then
                    strSQL = "UPDATE kpmkv_pelajar_markah SET BahasaMelayu='-1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='4'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)

                    strSQL = "UPDATE kpmkv_pelajar_markah SET PointerBMSetara='-1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='4'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                End If

            End If

            Dim BM_BahasaInggeris As Integer
            Dim AM_BahasaInggeris As Integer
            Dim BerterusanBI As Integer
            Dim AkhiranBI As Integer
            Dim B_BahasaInggeris As Double
            Dim A_BahasaInggeris As Double
            Dim PointerBI As Integer

            'strSQL = "Select PB from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A02'+'" & strKodMP & "%' AND Tahun='" & ddlTahun.Text & "'"
            strSQL = "SELECT PB FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
            BerterusanBI = oCommon.getFieldValue(strSQL)

            'strSQL = "Select PA from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A02'+'" & strKodMP & "%' AND Tahun='" & ddlTahun.Text & "'"
            strSQL = "SELECT PA FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
            AkhiranBI = oCommon.getFieldValue(strSQL)

            strSQL = "Select B_BahasaInggeris from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            B_BahasaInggeris = oCommon.getFieldValue(strSQL)
            'round up
            B_BahasaInggeris = Math.Ceiling(B_BahasaInggeris)

            strSQL = "Select A_BahasaInggeris from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            A_BahasaInggeris = oCommon.getFieldValue(strSQL)
            'round up
            A_BahasaInggeris = Math.Ceiling(A_BahasaInggeris)

            'checkin Markah
            If Not (B_BahasaInggeris) = "-1" And Not (A_BahasaInggeris) = "-1" Then
                BM_BahasaInggeris = Math.Ceiling((B_BahasaInggeris / 100) * BerterusanBI)
                AM_BahasaInggeris = Math.Ceiling((A_BahasaInggeris / 100) * AkhiranBI)
                PointerBI = Math.Ceiling(BM_BahasaInggeris + AM_BahasaInggeris)
                strSQL = "UPDATE kpmkv_pelajar_markah SET BahasaInggeris='" & PointerBI & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

            ElseIf (B_BahasaInggeris) = "-1" Or (A_BahasaInggeris) = "-1" Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET BahasaInggeris='-1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If

            '------------------------------------------------------------------------------------------------------------------------
            Dim BM_Science1 As Integer
            Dim AM_Science1 As Integer
            Dim AM_Science2 As Integer
            Dim BerterusanSc As Integer
            Dim AkhiranSc As Integer
            Dim B_Science1 As Double
            Dim A_Science1 As Double
            Dim A_Science2 As Double
            Dim PointerSC1 As Integer
            Dim PointerSC2 As Integer
            Dim PointerSC As Integer
            'Dim GredSC As Integer 

            'strSQL = "Select PB from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A04'+'" & strKodMP & "%' AND Tahun='" & ddlTahun.Text & "'"
            strSQL = "SELECT PB FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
            BerterusanSc = oCommon.getFieldValue(strSQL)

            'strSQL = "Select PA from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A04'+'" & strKodMP & "%'AND Tahun='" & ddlTahun.Text & "'"
            strSQL = "SELECT PA FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
            AkhiranSc = oCommon.getFieldValue(strSQL)

            strSQL = "Select B_Science1 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            B_Science1 = oCommon.getFieldValue(strSQL)
            'round up
            B_Science1 = Math.Ceiling(B_Science1)

            strSQL = "Select A_Science1 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            A_Science1 = oCommon.getFieldValue(strSQL)
            'round up
            A_Science1 = Math.Ceiling(A_Science1)

            strSQL = "Select A_Science2 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            A_Science2 = oCommon.getFieldValue(strSQL)
            'round up
            A_Science2 = Math.Ceiling(A_Science2)

            'check sem 3 n 4 ada  kertas 1
            BM_Science1 = Math.Ceiling((B_Science1 / 100) * BerterusanSc)

            If ddlSemester.Text = "1" Or ddlSemester.Text = "2" Then

                If Not (A_Science1) = "-1" And Not (A_Science2) = "-1" Then
                    AM_Science1 = Math.Ceiling((A_Science1 / 100) * 50) '50%

                    AM_Science2 = Math.Ceiling((A_Science2 / 100) * 20) '20% 

                    PointerSC1 = Math.Ceiling(BM_Science1)
                    PointerSC2 = Math.Ceiling((AM_Science1) + (AM_Science2))
                    PointerSC = Math.Ceiling((PointerSC1) + (PointerSC2))

                    strSQL = "UPDATE kpmkv_pelajar_markah SET Science='" & PointerSC & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                ElseIf (A_Science1) = "-1" Or (A_Science2) = "-1" Then
                    strSQL = "UPDATE kpmkv_pelajar_markah SET Science='-1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                End If
            Else

                If Not (A_Science1) = "-1" And Not (A_Science2) = "-1" Then
                    AM_Science1 = Math.Ceiling((A_Science1 / 100) * 70) '70%
                    AM_Science2 = Math.Ceiling((A_Science2 / 100) * 70) '70%
                    PointerSC1 = Math.Ceiling(BM_Science1)
                    PointerSC2 = Math.Ceiling((AM_Science1) + (AM_Science2))
                    PointerSC = Math.Ceiling((PointerSC1) + (PointerSC2))

                    strSQL = "UPDATE kpmkv_pelajar_markah SET Science='" & PointerSC & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                ElseIf (A_Science1) = "-1" Or (A_Science2) = "-1" Then
                    strSQL = "UPDATE kpmkv_pelajar_markah SET Science='-1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                End If

            End If

            'SC ------------------------------------------------------------------------------------------------------------

            Dim BM_Sejarah As Integer
            Dim AM_Sejarah As Integer
            Dim BerterusanSJ As Integer
            Dim AkhiranSJ As Integer
            Dim B_Sejarah As Double
            Dim A_Sejarah As Double
            Dim PointerSJ As Integer
            'Dim GredSJ As Integer 

            strSQL = "Select PB from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A05'+'" & strKodMP & "%' AND Tahun='" & ddlTahun.Text & "'"
            BerterusanSJ = oCommon.getFieldValue(strSQL)

            strSQL = "Select PA from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A05'+'" & strKodMP & "%' AND Tahun='" & ddlTahun.Text & "'"
            AkhiranSJ = oCommon.getFieldValue(strSQL)

            strSQL = "Select B_Sejarah from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            B_Sejarah = oCommon.getFieldValue(strSQL)
            'round up
            B_Sejarah = Math.Ceiling(B_Sejarah)

            strSQL = "Select A_Sejarah from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            A_Sejarah = oCommon.getFieldValue(strSQL)
            'round up
            A_Sejarah = Math.Ceiling(A_Sejarah)

            'checkin Markah
            If Not (B_Sejarah) = "-1" And Not (A_Sejarah) = "-1" Then
                BM_Sejarah = Math.Ceiling((B_Sejarah / 100) * BerterusanSJ)
                AM_Sejarah = Math.Ceiling((A_Sejarah / 100) * AkhiranSJ)
                PointerSJ = Math.Ceiling(BM_Sejarah + AM_Sejarah)
                strSQL = "UPDATE kpmkv_pelajar_markah SET Sejarah='" & PointerSJ & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

            ElseIf (B_Sejarah) = "-1" Or (A_Sejarah) = "-1" Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET Sejarah='-1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            '-------------------------------------------------------------------------------------------------------------
            'strSQL = "Select Kaum from kpmkv_pelajar Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            'Dim strKaum As String = oCommon.getFieldValue(strSQL)
            'If strKaum = "BUMIPUTERA" Then

            Dim BM_PendidikanIslam1 As Integer
            Dim BerterusanPI As Integer
            Dim AkhiranPI As Integer
            Dim B_PendidikanIslam1 As Integer
            Dim A_PendidikanIslam1 As Integer
            Dim A_PendidikanIslam2 As Integer
            Dim PointerPI1 As Integer
            Dim PointerPI2 As Integer
            Dim PointerPI As Integer
            ' Dim GredPI As Integer 

            'strSQL = "Select PB from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A06'+'" & strKodMP & "%' AND Tahun='" & ddlTahun.Text & "'"
            strSQL = "SELECT PB FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
            BerterusanPI = oCommon.getFieldValue(strSQL)

            'strSQL = "Select PA from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A06'+'" & strKodMP & "%' AND Tahun='" & ddlTahun.Text & "'"
            strSQL = "SELECT PA FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
            AkhiranPI = oCommon.getFieldValue(strSQL)

            strSQL = "Select B_PendidikanIslam1 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            B_PendidikanIslam1 = oCommon.getFieldValue(strSQL)
            'round up
            B_PendidikanIslam1 = Math.Ceiling(B_PendidikanIslam1)

            strSQL = "Select A_PendidikanIslam1 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            A_PendidikanIslam1 = oCommon.getFieldValue(strSQL)
            'round up
            A_PendidikanIslam1 = Math.Ceiling(A_PendidikanIslam1)

            strSQL = "Select A_PendidikanIslam2 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            A_PendidikanIslam2 = oCommon.getFieldValue(strSQL)
            'round up
            A_PendidikanIslam2 = Math.Ceiling(A_PendidikanIslam2)

            BM_PendidikanIslam1 = Math.Ceiling((B_PendidikanIslam1 / 100) * BerterusanPI)

            If Not (A_PendidikanIslam1) = "-1" And Not (A_PendidikanIslam2) = "-1" Then
                A_PendidikanIslam1 = Math.Ceiling((A_PendidikanIslam1 / 100) * 50) '50%
                A_PendidikanIslam2 = Math.Ceiling((A_PendidikanIslam2 / 100) * 20) '20%

                PointerPI1 = Math.Ceiling(BM_PendidikanIslam1)
                PointerPI2 = Math.Ceiling(A_PendidikanIslam1 + A_PendidikanIslam2)
                PointerPI = Math.Ceiling(PointerPI1 + PointerPI2)

                strSQL = "UPDATE kpmkv_pelajar_markah SET PendidikanIslam='" & PointerPI & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            ElseIf (A_PendidikanIslam1) = "-1" Or (A_PendidikanIslam2) = "-1" Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET PendidikanIslam='-1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            '-------------------------------------------------------------------------------------------------------------

            Dim BM_PendidikanMoral As Integer
            Dim AM_PendidikanMoral As Integer
            Dim BerterusanPM As Integer
            Dim AkhiranPM As Integer
            Dim B_PendidikanMoral As Integer
            Dim A_PendidikanMoral As Integer
            Dim PointerPM As Integer
            'Dim GredPM As Integer 

            'strSQL = "Select PB from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A07'+'" & strKodMP & "%' AND Tahun='" & ddlTahun.Text & "'"
            strSQL = "SELECT PB FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
            BerterusanPM = oCommon.getFieldValue(strSQL)

            'strSQL = "Select PA from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A07'+'" & strKodMP & "%' AND Tahun='" & ddlTahun.Text & "'"
            strSQL = "SELECT PA FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
            AkhiranPM = oCommon.getFieldValue(strSQL)

            strSQL = "Select B_PendidikanMoral from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            B_PendidikanMoral = oCommon.getFieldValue(strSQL)
            'round up
            B_PendidikanMoral = Math.Ceiling(B_PendidikanMoral)

            strSQL = "Select A_PendidikanMoral from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            A_PendidikanMoral = oCommon.getFieldValue(strSQL)
            'round up
            A_PendidikanMoral = Math.Ceiling(A_PendidikanMoral)

            'checkin Markah
            If Not (B_PendidikanMoral) = "-1" And Not (A_PendidikanMoral) = "-1" Then
                BM_PendidikanMoral = Math.Ceiling((B_PendidikanMoral / 100) * BerterusanPM)
                AM_PendidikanMoral = Math.Ceiling((A_PendidikanMoral / 100) * AkhiranPM)
                PointerPM = Math.Ceiling(BM_PendidikanMoral + AM_PendidikanMoral)
                strSQL = "UPDATE kpmkv_pelajar_markah SET PendidikanMoral='" & PointerPM & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            ElseIf (B_PendidikanMoral) = "-1" Or (A_PendidikanMoral) = "-1" Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET PendidikanMoral='-1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            '-------------------------------------------------------------------------------------------------------------

            Dim BM_Mathematics As Integer
            Dim AM_Mathematics As Integer
            Dim BerterusanMT As Integer
            Dim AkhiranMT As Integer
            Dim B_Mathematics As Integer
            Dim A_Mathematics As Integer
            Dim PointerMT As Integer
            'Dim GredMT As Integer 

            'strSQL = "Select PB from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A03'+'" & strKodMP & "%' AND Tahun='" & ddlTahun.Text & "'"
            strSQL = "SELECT PB FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
            BerterusanMT = oCommon.getFieldValue(strSQL)

            'strSQL = "Select PA from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A03'+'" & strKodMP & "%' AND Tahun='" & ddlTahun.Text & "'"
            strSQL = "SELECT PA FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
            AkhiranMT = oCommon.getFieldValue(strSQL)

            strSQL = "Select B_Mathematics from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            B_Mathematics = oCommon.getFieldValue(strSQL)
            'round up
            B_Mathematics = Math.Ceiling(B_Mathematics)

            strSQL = "Select A_Mathematics from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            A_Mathematics = oCommon.getFieldValue(strSQL)
            'round up
            A_Mathematics = Math.Ceiling(A_Mathematics)

            'checkin Markah
            If Not (B_Mathematics) = "-1" And Not (A_Mathematics) = "-1" Then
                BM_Mathematics = Math.Ceiling((B_Mathematics / 100) * BerterusanMT)
                AM_Mathematics = Math.Ceiling((A_Mathematics / 100) * AkhiranMT)
                PointerMT = Math.Ceiling(BM_Mathematics + AM_Mathematics)
                strSQL = "UPDATE kpmkv_pelajar_markah SET Mathematics='" & PointerMT & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            ElseIf (B_Mathematics) = "-1" Or (A_Mathematics) = "-1" Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET Mathematics='-1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
        Next
    End Sub

    Private Sub Akademik_gred()

        For i As Integer = 0 To datRespondent.Rows.Count - 1
            Dim BM As String
            Dim GredBM As String

            strSQL = "SELECT BahasaMelayu as BM FROM kpmkv_pelajar_markah"
            strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            BM = oCommon.getFieldValue(strSQL)

            If String.IsNullOrEmpty(BM) Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET GredBM='' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

            Else
                strSQL = "SELECT TOP ( 1 ) Gred FROM  kpmkv_gred WHERE '" & Integer.Parse(BM) & "' BETWEEN MarkahFrom AND MarkahTo AND Jenis='AKADEMIK'"
                GredBM = oCommon.getFieldValue(strSQL)

                strSQL = "UPDATE kpmkv_pelajar_markah SET GredBM='" & GredBM & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

            End If

            '-----------------------------------------------------------------
            Dim BI As String
            Dim GredBI As String

            strSQL = "SELECT BahasaInggeris FROM kpmkv_pelajar_markah"
            strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            BI = oCommon.getFieldValue(strSQL)

            'If BI = "0" Then
            If String.IsNullOrEmpty(BI) Then
            Else
                strSQL = "SELECT TOP ( 1 ) Gred FROM  kpmkv_gred WHERE '" & Integer.Parse(BI) & "' BETWEEN MarkahFrom AND MarkahTo AND Jenis='AKADEMIK'"
                GredBI = oCommon.getFieldValue(strSQL)

                strSQL = "UPDATE kpmkv_pelajar_markah SET GredBI='" & GredBI & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

            End If
            '------------------------------------------------------------------------------------------------------------------------
            Dim SC As Integer
            Dim GredSC As String

            strSQL = "SELECT Science FROM kpmkv_pelajar_markah"
            strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            SC = oCommon.getFieldValue(strSQL)

            'If SC = 0 Then
            If String.IsNullOrEmpty(SC) Then
            Else
                strSQL = "SELECT TOP ( 1 ) Gred FROM  kpmkv_gred WHERE '" & SC & "' BETWEEN MarkahFrom AND MarkahTo AND Jenis='AKADEMIK'"
                GredSC = oCommon.getFieldValue(strSQL)

                strSQL = "UPDATE kpmkv_pelajar_markah SET GredSC='" & GredSC & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

            End If
            '------------------------------------------------------------------------------------------------------------

            Dim SJ As String
            Dim GredSJ As String

            strSQL = "SELECT Sejarah FROM kpmkv_pelajar_markah"
            strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            SJ = oCommon.getFieldValue(strSQL)

            'If SJ = "0" Then
            If String.IsNullOrEmpty(SJ) Then
            Else
                strSQL = "SELECT TOP ( 1 ) Gred FROM  kpmkv_gred WHERE '" & Integer.Parse(SJ) & "' BETWEEN MarkahFrom AND MarkahTo AND Jenis='AKADEMIK'"
                GredSJ = oCommon.getFieldValue(strSQL)

                strSQL = "UPDATE kpmkv_pelajar_markah SET GredSJ='" & GredSJ & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

            End If
            '-------------------------------------------------------------------------------------------------------------

            Dim PI As String
            Dim GredPI As String

            strSQL = "SELECT PendidikanIslam FROM kpmkv_pelajar_markah"
            strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            PI = oCommon.getFieldValue(strSQL)

            If PI = "0" Then
            ElseIf String.IsNullOrEmpty(PI) Then
            Else
                strSQL = "SELECT TOP ( 1 ) Gred FROM  kpmkv_gred WHERE '" & Integer.Parse(PI) & "' BETWEEN MarkahFrom AND MarkahTo AND Jenis='AKADEMIK'"
                GredPI = oCommon.getFieldValue(strSQL)

                strSQL = "UPDATE kpmkv_pelajar_markah SET GredPI='" & GredPI & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

            End If
            '-------------------------------------------------------------------------------------------------------------

            Dim PM As String
            Dim GredPM As String

            strSQL = "SELECT PendidikanMoral FROM kpmkv_pelajar_markah"
            strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            PM = oCommon.getFieldValue(strSQL)

            If PM = "0" Then
            ElseIf String.IsNullOrEmpty(PM) Then
            Else
                strSQL = "SELECT TOP ( 1 ) Gred FROM  kpmkv_gred WHERE '" & Integer.Parse(PM) & "' BETWEEN MarkahFrom AND MarkahTo AND Jenis='AKADEMIK'"
                GredPM = oCommon.getFieldValue(strSQL)

                strSQL = "UPDATE kpmkv_pelajar_markah SET GredPM='" & GredPM & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

            End If
            '-------------------------------------------------------------------------------------------------------------

            Dim MT As String
            Dim GredMT As String

            strSQL = "SELECT Mathematics FROM kpmkv_pelajar_markah"
            strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            MT = oCommon.getFieldValue(strSQL)

            'If MT = "0" Then
            If String.IsNullOrEmpty(MT) Then
            Else
                strSQL = "SELECT TOP ( 1 ) Gred FROM  kpmkv_gred WHERE '" & Integer.Parse(MT) & "' BETWEEN MarkahFrom AND MarkahTo AND Jenis='AKADEMIK'"
                GredMT = oCommon.getFieldValue(strSQL)

                strSQL = "UPDATE kpmkv_pelajar_markah SET GredMT='" & GredMT & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If

        Next
    End Sub

    Protected Sub btnCari_Click(sender As Object, e As EventArgs) Handles btnCari.Click
        lblMsg.Text = ""
        lblMsgResult.Text = ""
        strRet = BindData(datRespondent)
        getDateSah()
    End Sub

    Private Sub chkSesi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkSesi.SelectedIndexChanged
        kpmkv_kodkursus_list()
        kpmkv_kelas_list()
    End Sub

    Private Sub ddlKodKursus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlKodKursus.SelectedIndexChanged
        kpmkv_kelas_list()
    End Sub



    Private Sub btnGred_Click(sender As Object, e As EventArgs) Handles btnGred.Click
        lblMsg.Text = ""
        lblMsgResult.Text = ""

        Akademik_markah()
        Akademik_gred()
        If Not strRet = "0" Then
            divMsgResult.Attributes("class") = "error"
            lblMsgResult.Text = "Tidak Berjaya mengemaskini gred Pentaksiran Akhir Akademik"
        Else
            divMsgResult.Attributes("class") = "info"
            lblMsgResult.Text = "Berjaya mengemaskini gred Pentaksiran Akhir Akademik"
            strRet = BindData((datRespondent))
        End If

    End Sub

    Private Sub ddlSemester_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSemester.SelectedIndexChanged
        If ddlSemester.SelectedValue = "4" Then
            btnGred.Visible = False

        Else
            btnGred.Visible = True
        End If
    End Sub

    ''pengesahan markah
    Protected Sub btnSah_Click(sender As Object, e As EventArgs) Handles btnSah.Click
        lblMsg.Text = ""
        lblMsgResult.Text = ""
        Try
            If ValidateForm() = False Then
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Sila Semak Data Markah.[Huruf],[Kosong] adalah tidak dibenarkan!  Markah mestilah diantara -1-100 sahaja"
                divMsgResult.Attributes("class") = "error"
                lblMsgResult.Text = "Sila Semak Data Markah.[Huruf],[Kosong] adalah tidak dibenarkan!  Markah mestilah diantara -1-100 sahaja"

                Exit Sub
            End If


            For i As Integer = 0 To datRespondent.Rows.Count - 1
                Dim row As GridViewRow = datRespondent.Rows(i)
                Dim strBahasaMelayu As TextBox = datRespondent.Rows(i).FindControl("A_BahasaMelayu")
                Dim strBahasaMelayu1 As TextBox = datRespondent.Rows(i).FindControl("A_BahasaMelayu1")
                Dim strBahasaMelayu2 As TextBox = datRespondent.Rows(i).FindControl("A_BahasaMelayu2")
                Dim strBahasaMelayu3 As TextBox = datRespondent.Rows(i).FindControl("A_BahasaMelayu3")
                Dim strBahasaInggeris As TextBox = datRespondent.Rows(i).FindControl("A_BahasaInggeris")
                Dim strScience1 As TextBox = datRespondent.Rows(i).FindControl("A_Science1")
                Dim strScience2 As TextBox = datRespondent.Rows(i).FindControl("A_Science2")
                Dim strSejarah As TextBox = datRespondent.Rows(i).FindControl("A_Sejarah")
                Dim strPendidikanIslam1 As TextBox = datRespondent.Rows(i).FindControl("A_PendidikanIslam1")
                Dim strPendidikanIslam2 As TextBox = datRespondent.Rows(i).FindControl("A_PendidikanIslam2")
                Dim strPendidikanMoral As TextBox = datRespondent.Rows(i).FindControl("A_PendidikanMoral")
                Dim strMatematik As TextBox = datRespondent.Rows(i).FindControl("A_Mathematics")


                'assign value to integer
                Dim BM As String = strBahasaMelayu.Text

                If BM = "" Then
                    divMsgResult.Attributes("class") = "error"
                    lblMsgResult.Text = "Semua markah perlu diisi dan dikemaskini sebelum pengesahan markah dilakukan"
                    Exit Sub
                End If

                Dim BM3 As String = strBahasaMelayu3.Text
                If ddlSemester.SelectedValue = "4" Then
                    If BM3 = "" Then
                        divMsgResult.Attributes("class") = "error"
                        lblMsgResult.Text = "Semua markah perlu diisi dan dikemaskini sebelum pengesahan markah dilakukan"
                        Exit Sub
                    End If

                End If

                Dim BI As String = strBahasaInggeris.Text
                If BI = "" Then
                    divMsgResult.Attributes("class") = "error"
                    lblMsgResult.Text = "Semua markah perlu diisi dan dikemaskini sebelum pengesahan markah dilakukan"
                    Exit Sub
                End If
                Dim SC1 As String = strScience1.Text
                If SC1 = "" Then
                    divMsgResult.Attributes("class") = "error"
                    lblMsgResult.Text = "Semua markah perlu diisi dan dikemaskini sebelum pengesahan markah dilakukan"
                    Exit Sub
                End If

                Dim SC2 As String = strScience2.Text
                If ddlSemester.SelectedValue = "1" Or ddlSemester.SelectedValue = "2" Then
                    If SC2 = "" Then
                        divMsgResult.Attributes("class") = "error"
                        lblMsgResult.Text = "Semua markah perlu diisi dan dikemaskini sebelum pengesahan markah dilakukan"
                        Exit Sub
                    End If
                End If
                Dim SEJ As String = strSejarah.Text
                If SEJ = "" Then
                    divMsgResult.Attributes("class") = "error"
                    lblMsgResult.Text = "Semua markah perlu diisi dan dikemaskini sebelum pengesahan markah dilakukan"
                    Exit Sub
                End If
                Dim PI1 As String = strPendidikanIslam1.Text
                If PI1 = "" Then
                    divMsgResult.Attributes("class") = "error"
                    lblMsgResult.Text = "Semua markah perlu diisi dan dikemaskini sebelum pengesahan markah dilakukan"
                    Exit Sub
                End If
                Dim PI2 As String = strPendidikanIslam2.Text
                If PI2 = "" Then
                    divMsgResult.Attributes("class") = "error"
                    lblMsgResult.Text = "Semua markah perlu diisi dan dikemaskini sebelum pengesahan markah dilakukan"
                    Exit Sub
                End If
                Dim PM As String = strPendidikanMoral.Text
                If PM = "" Then
                    divMsgResult.Attributes("class") = "error"
                    lblMsgResult.Text = "Semua markah perlu diisi dan dikemaskini sebelum pengesahan markah dilakukan"
                    Exit Sub
                End If
                Dim Matematik As String = strMatematik.Text
                If Matematik = "" Then
                    divMsgResult.Attributes("class") = "error"
                    lblMsgResult.Text = "Semua markah perlu diisi dan dikemaskini sebelum pengesahan markah dilakukan"
                    Exit Sub
                End If

                strSQL = "UPDATE kpmkv_pelajar_markah SET A_BahasaMelayu='" & BM & "', "
                strSQL += " A_BahasaMelayu3='" & BM3 & "', A_BahasaInggeris='" & BI & "', "
                strSQL += " A_Science1 ='" & SC1 & "', "

                If Not ddlSemester.SelectedValue = "4" Then
                    strSQL += " A_Sejarah='" & SEJ & "',"
                End If
                strSQL += " A_PendidikanIslam1='" & PI1 & "', A_PendidikanMoral='" & PM & "', A_Science2='" & SC2 & "',"
                strSQL += " A_PendidikanIslam2='" & PI2 & "', A_Mathematics='" & Matematik & "',"
                strSQL += " isSahPAA ='1',isSahPAA_Date=GETDATE() "
                strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' "
                strSQL += " AND PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
                If strRet = "0" Then
                    divMsgResult.Attributes("class") = "info"
                    lblMsgResult.Text = "Pengesahan kemasukan markah berjaya dikemaskini"
                Else
                    divMsgResult.Attributes("class") = "error"
                    lblMsgResult.Text = "Pengesahan kemasukan markah tidak berjaya dikemaskini"
                End If
            Next

        Catch ex As Exception
            lblMsg.Text = "Error:" & ex.Message
        End Try
        getDateSah()

        strRet = BindData(datRespondent)

    End Sub

    Private Sub getDateSah()
        strSQL = "SELECT MAX(isSahPAA_Date) FROM kpmkv_pelajar_markah "
        strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' "
        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
        strSQL += " AND isSahPAA='1'"
        strSQL += " GROUP BY isSahPAA_Date"
        strSQL += " ORDER BY isSahPAA_Date  DESC"

        lblTarikhKemaskini.Text = oCommon.getFieldValue(strSQL)
    End Sub

    'download pdf kenyataan
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        Try

            Dim tableColumn As DataColumnCollection
            Dim tableRows As DataRowCollection

            Dim myDataSet As New DataSet
            Dim myDataAdapter As New SqlDataAdapter(getSQL, strConn)
            myDataAdapter.Fill(myDataSet, "Kenyataan Kemasukan Semester")
            myDataAdapter.SelectCommand.CommandTimeout = 80000
            objConn.Close()

            '--transfer to an object
            tableColumn = myDataSet.Tables(0).Columns
            tableRows = myDataSet.Tables(0).Rows

            CreatePDF(tableColumn, tableRows)

        Catch ex As Exception
            '--display on screen
            lblMsg.Text = "System Error." & ex.Message
        End Try
    End Sub

    Private Sub CreatePDF(ByVal tableColumns As DataColumnCollection, ByVal tableRows As DataRowCollection)
        Dim myDocument As New Document(PageSize.A4.Rotate())

        Try
            HttpContext.Current.Response.ContentType = "application/pdf"
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=PBAkademik.pdf")
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)

            PdfWriter.GetInstance(myDocument, HttpContext.Current.Response.OutputStream)

            myDocument.Open()

            ''--draw spacing
            Dim imgdrawSpacing As String = Server.MapPath("~/img/spacing.png")
            Dim imgSpacing As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(imgdrawSpacing)
            imgSpacing.Alignment = iTextSharp.text.Image.LEFT_ALIGN  'left
            imgSpacing.Border = 0

            strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
            Dim strNamaKolej As String = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT Negeri FROM kpmkv_kolej WHERE Nama='" & strNamaKolej & "'"
            Dim strNegeriKolej As String = oCommon.getFieldValue(strSQL)

            'NAMA KOLEJ
            Dim table As New PdfPTable(1)
            table.WidthPercentage = 100
            table.SetWidths({100})
            table.DefaultCell.Border = 0

            Dim cell As New PdfPCell()
            Dim cetak As String
            Dim para As New Paragraph()
            cetak = strNamaKolej & Environment.NewLine
            cetak += "KEMENTERIAN PENDIDIKAN MALAYSIA"
            para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
            para.Alignment = Element.ALIGN_CENTER
            cell.AddElement(para)
            cell.Border = 0
            table.AddCell(cell)
            myDocument.Add(table)

            myDocument.Add(imgSpacing)

            table = New PdfPTable(3)
            table.WidthPercentage = 105
            table.SetWidths({10, 5, 85})
            table.DefaultCell.Border = 0

            cell = New PdfPCell()
            cetak = "KOHORT"
            para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
            cell.AddElement(para)
            cell.Border = 0
            table.AddCell(cell)
            myDocument.Add(table)

            cell = New PdfPCell()
            cetak = " : "
            para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
            cell.AddElement(para)
            cell.Border = 0
            table.AddCell(cell)
            myDocument.Add(table)

            cell = New PdfPCell()
            cetak = ddlTahun.Text
            para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
            cell.AddElement(para)
            cell.Border = 0
            table.AddCell(cell)
            myDocument.Add(table)

            table = New PdfPTable(3)
            table.WidthPercentage = 105
            table.SetWidths({10, 5, 85})
            table.DefaultCell.Border = 0

            cell = New PdfPCell()
            cetak = "SEMESTER"
            para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
            cell.AddElement(para)
            cell.Border = 0
            table.AddCell(cell)
            myDocument.Add(table)

            cell = New PdfPCell()
            cetak = " : "
            para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
            cell.AddElement(para)
            cell.Border = 0
            table.AddCell(cell)
            myDocument.Add(table)

            cell = New PdfPCell()
            cetak = ddlSemester.Text
            para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
            cell.AddElement(para)
            cell.Border = 0
            table.AddCell(cell)
            myDocument.Add(table)

            strSQL = "SELECT KodKursus FROM kpmkv_kursus WHERE KursusID = '" & ddlKodKursus.SelectedValue & "'"
            Dim strKodKursus As String = oCommon.getFieldValue(strSQL)

            table = New PdfPTable(3)
            table.WidthPercentage = 105
            table.SetWidths({10, 5, 85})
            table.DefaultCell.Border = 0

            cell = New PdfPCell()
            cetak = "KOD KURSUS"
            para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
            cell.AddElement(para)
            cell.Border = 0
            table.AddCell(cell)
            myDocument.Add(table)

            cell = New PdfPCell()
            cetak = " : "
            para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
            cell.AddElement(para)
            cell.Border = 0
            table.AddCell(cell)
            myDocument.Add(table)



            cell = New PdfPCell()
            cetak = strKodKursus
            para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
            cell.AddElement(para)
            cell.Border = 0
            table.AddCell(cell)
            myDocument.Add(table)

            myDocument.Add(imgSpacing)

            If Not ddlSemester.Text = "4" Then

                table = New PdfPTable(14)
                table.WidthPercentage = 105
                table.SetWidths({2, 20, 7, 7, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5})
                table.DefaultCell.Border = 0

                'BILANGAN
                cell = New PdfPCell()
                cetak = "Bil"
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                'NAMA
                cell = New PdfPCell()
                cetak = "Nama"
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                'ANGKAGILIRAN
                cell = New PdfPCell()
                cetak = "AngkaGiliran"
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                'MYKAD
                cell = New PdfPCell()
                cetak = "Mykad"
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                ''KODKURSUS
                'cell = New PdfPCell()
                'cetak = "KodKursus"
                'para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                'cell.AddElement(para)
                'cell.Border = 0
                'table.AddCell(cell)
                'myDocument.Add(table)

                'BM
                cell = New PdfPCell()
                cetak = "Bahasa" & Environment.NewLine
                cetak += "Melayu"
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                para.Alignment = Element.ALIGN_CENTER
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                ''BM1
                'cell = New PdfPCell()
                'cetak = "Bahasa" & Environment.NewLine
                'cetak += "Melayu1"
                'para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                'para.Alignment = Element.ALIGN_CENTER
                'cell.AddElement(para)
                'cell.Border = 0
                'table.AddCell(cell)
                'myDocument.Add(table)

                ''BM2
                'cell = New PdfPCell()
                'cetak = "Bahasa" & Environment.NewLine
                'cetak += "Melayu2"
                'para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                'para.Alignment = Element.ALIGN_CENTER
                'cell.AddElement(para)
                'cell.Border = 0
                'table.AddCell(cell)
                'myDocument.Add(table)

                'BM3
                cell = New PdfPCell()
                cetak = "Bahasa" & Environment.NewLine
                cetak += "Melayu3"
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                para.Alignment = Element.ALIGN_CENTER
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                'BI
                cell = New PdfPCell()
                cetak = "Bahasa" & Environment.NewLine
                cetak += "Inggeris"
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                para.Alignment = Element.ALIGN_CENTER
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                'MT
                cell = New PdfPCell()
                cetak = "Matematik" & Environment.NewLine
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                para.Alignment = Element.ALIGN_CENTER
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                'SC1
                cell = New PdfPCell()
                cetak = "Sains1" & Environment.NewLine
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                para.Alignment = Element.ALIGN_CENTER
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                'SC2
                cell = New PdfPCell()
                cetak = "Sains2" & Environment.NewLine
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                para.Alignment = Element.ALIGN_CENTER
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                'SJ
                cell = New PdfPCell()
                cetak = "Sejarah" & Environment.NewLine
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                para.Alignment = Element.ALIGN_CENTER
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                'PI1
                cell = New PdfPCell()
                cetak = "Pendidikan" & Environment.NewLine
                cetak += "Islam1"
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                para.Alignment = Element.ALIGN_CENTER
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                'PI2
                cell = New PdfPCell()
                cetak = "Pendidikan" & Environment.NewLine
                cetak += "Islam2"
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                para.Alignment = Element.ALIGN_CENTER
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                'PM
                cell = New PdfPCell()
                cetak = "Pendidikan" & Environment.NewLine
                cetak += "Moral"
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                para.Alignment = Element.ALIGN_CENTER
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

            ElseIf ddlSemester.Text = "4" Then

                table = New PdfPTable(13)
                table.WidthPercentage = 105
                table.SetWidths({2, 20, 7, 7, 5, 5, 5, 5, 5, 5, 5, 5, 5})
                table.DefaultCell.Border = 0

                'BILANGAN
                cell = New PdfPCell()
                cetak = "Bil"
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                'NAMA
                cell = New PdfPCell()
                cetak = "Nama"
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                'ANGKAGILIRAN
                cell = New PdfPCell()
                cetak = "AngkaGiliran"
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                'MYKAD
                cell = New PdfPCell()
                cetak = "Mykad"
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                ''KODKURSUS
                'cell = New PdfPCell()
                'cetak = "KodKursus"
                'para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                'cell.AddElement(para)
                'cell.Border = 0
                'table.AddCell(cell)
                'myDocument.Add(table)

                'BM
                cell = New PdfPCell()
                cetak = "Bahasa" & Environment.NewLine
                cetak += "Melayu"
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                para.Alignment = Element.ALIGN_CENTER
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                ''BM1
                'cell = New PdfPCell()
                'cetak = "Bahasa" & Environment.NewLine
                'cetak += "Melayu1"
                'para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                'para.Alignment = Element.ALIGN_CENTER
                'cell.AddElement(para)
                'cell.Border = 0
                'table.AddCell(cell)
                'myDocument.Add(table)

                ''BM2
                'cell = New PdfPCell()
                'cetak = "Bahasa" & Environment.NewLine
                'cetak += "Melayu2"
                'para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                'para.Alignment = Element.ALIGN_CENTER
                'cell.AddElement(para)
                'cell.Border = 0
                'table.AddCell(cell)
                'myDocument.Add(table)

                'BM3
                cell = New PdfPCell()
                cetak = "Bahasa" & Environment.NewLine
                cetak += "Melayu3"
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                para.Alignment = Element.ALIGN_CENTER
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                'BI
                cell = New PdfPCell()
                cetak = "Bahasa" & Environment.NewLine
                cetak += "Inggeris"
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                para.Alignment = Element.ALIGN_CENTER
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                'MT
                cell = New PdfPCell()
                cetak = "Matematik" & Environment.NewLine
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                para.Alignment = Element.ALIGN_CENTER
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                'SC1
                cell = New PdfPCell()
                cetak = "Sains1" & Environment.NewLine
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                para.Alignment = Element.ALIGN_CENTER
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                'SC2
                cell = New PdfPCell()
                cetak = "Sains2" & Environment.NewLine
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                para.Alignment = Element.ALIGN_CENTER
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                ''SJ
                'cell = New PdfPCell()
                'cetak = "Sejarah" & Environment.NewLine
                'para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                'para.Alignment = Element.ALIGN_CENTER
                'cell.AddElement(para)
                'cell.Border = 0
                'table.AddCell(cell)
                'myDocument.Add(table)

                'PI1
                cell = New PdfPCell()
                cetak = "Pendidikan" & Environment.NewLine
                cetak += "Islam1"
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                para.Alignment = Element.ALIGN_CENTER
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                'PI2
                cell = New PdfPCell()
                cetak = "Pendidikan" & Environment.NewLine
                cetak += "Islam2"
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                para.Alignment = Element.ALIGN_CENTER
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

                'PM
                cell = New PdfPCell()
                cetak = "Pendidikan" & Environment.NewLine
                cetak += "Moral"
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
                para.Alignment = Element.ALIGN_CENTER
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

            End If

            For i As Integer = 0 To datRespondent.Rows.Count - 1

                Dim strkey As String = datRespondent.DataKeys(i).Value.ToString

                '--not deleted
                strSQL = "  SELECT kpmkv_pelajar.PelajarID, kpmkv_pelajar.Tahun, kpmkv_pelajar.Semester, kpmkv_pelajar.Sesi, kpmkv_pelajar.Nama, kpmkv_pelajar.AngkaGiliran, "
                strSQL += " kpmkv_pelajar.MYKAD, kpmkv_kursus.KodKursus, kpmkv_pelajar_markah.A_BahasaMelayu, kpmkv_pelajar_markah.A_BahasaMelayu1, kpmkv_pelajar_markah.A_BahasaMelayu2, kpmkv_pelajar_markah.A_BahasaMelayu3, "
                strSQL += " kpmkv_pelajar_markah.A_BahasaInggeris, kpmkv_pelajar_markah.A_Science1, kpmkv_pelajar_markah.A_Science2, kpmkv_pelajar_markah.A_Sejarah, kpmkv_pelajar_markah.A_PendidikanIslam1, "
                strSQL += " kpmkv_pelajar_markah.A_PendidikanIslam2, kpmkv_pelajar_markah.A_PendidikanMoral, kpmkv_pelajar_markah.A_Mathematics"
                strSQL += " FROM kpmkv_pelajar_markah LEFT OUTER JOIN kpmkv_pelajar ON kpmkv_pelajar_markah.PelajarID = kpmkv_pelajar.PelajarID"
                strSQL += " LEFT OUTER Join kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID"
                strSQL += " WHERE kpmkv_pelajar.PelajarID = '" & strkey & "' AND kpmkv_pelajar.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.StatusID='2' AND kpmkv_pelajar.JenisCalonID='2'"

                '--tahun
                If Not ddlTahun.Text = "PILIH" Then
                    strSQL += " AND kpmkv_pelajar.Tahun ='" & ddlTahun.Text & "'"
                End If
                '--sesi
                If Not chkSesi.Text = "" Then
                    strSQL += " AND kpmkv_pelajar.Sesi ='" & chkSesi.Text & "'"
                End If
                '--semester
                If Not ddlSemester.Text = "" Then
                    strSQL += " AND kpmkv_pelajar.Semester ='" & ddlSemester.Text & "'"
                End If
                '--Kod
                If Not ddlKodKursus.Text = "" Then
                    strSQL += " AND kpmkv_pelajar.KursusID='" & ddlKodKursus.SelectedValue & "'"
                End If
                '--sesi
                If Not ddlKelas.Text = "" Then
                    strSQL += " AND kpmkv_pelajar.KelasID ='" & ddlKelas.SelectedValue & "'"
                End If

                strSQL += " ORDER BY kpmkv_pelajar.Nama ASC"
                strRet = oCommon.getFieldValueEx(strSQL)

                Dim PAA_info As Array
                PAA_info = strRet.Split("|")

                Dim strPelajarID As String = PAA_info(0)
                Dim strTahun As String = PAA_info(1)
                Dim strSemester As String = PAA_info(2)
                Dim strSesi As String = PAA_info(3)
                Dim strNama As String = PAA_info(4)
                Dim strAngkaGiliran As String = PAA_info(5)
                Dim strMykad As String = PAA_info(6)
                strKodKursus = PAA_info(7)
                Dim A_BM As String = PAA_info(8)
                Dim A_BM1 As String = PAA_info(9)
                Dim A_BM2 As String = PAA_info(10)
                Dim A_BM3 As String = PAA_info(11)
                Dim A_BI As String = PAA_info(12)
                Dim A_SC1 As String = PAA_info(13)
                Dim A_SC2 As String = PAA_info(14)
                Dim A_SJ As String = PAA_info(15)
                Dim A_PI1 As String = PAA_info(16)
                Dim A_PI2 As String = PAA_info(17)
                Dim A_PM As String = PAA_info(18)
                Dim A_MT As String = PAA_info(19)


                If Not ddlSemester.Text = "4" Then

                    table = New PdfPTable(14)
                    table.WidthPercentage = 105
                    table.SetWidths({2, 20, 7, 7, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5})
                    table.DefaultCell.Border = 0

                    'BILANGAN
                    cell = New PdfPCell()
                    cetak = i + 1
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    'NAMA
                    cell = New PdfPCell()
                    cetak = strNama
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    'ANGKAGILIRAN
                    cell = New PdfPCell()
                    cetak = strAngkaGiliran
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    'MYKAD
                    cell = New PdfPCell()
                    cetak = strMykad
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    ''KODKURSUS
                    'cell = New PdfPCell()
                    'cetak = strKodKursus
                    'para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    'cell.AddElement(para)
                    'cell.Border = 0
                    'table.AddCell(cell)
                    'myDocument.Add(table)

                    'BM
                    cell = New PdfPCell()
                    cetak = A_BM
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    para.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    ''BM1
                    'cell = New PdfPCell()
                    'cetak = A_BM1
                    'para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    'para.Alignment = Element.ALIGN_CENTER
                    'cell.AddElement(para)
                    'cell.Border = 0
                    'table.AddCell(cell)
                    'myDocument.Add(table)

                    ''BM2
                    'cell = New PdfPCell()
                    'cetak = A_BM2
                    'para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    'para.Alignment = Element.ALIGN_CENTER
                    'cell.AddElement(para)
                    'cell.Border = 0
                    'table.AddCell(cell)
                    'myDocument.Add(table)

                    'BM3
                    cell = New PdfPCell()
                    cetak = A_BM3
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    para.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    'BI
                    cell = New PdfPCell()
                    cetak = A_BI
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    para.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    'MT
                    cell = New PdfPCell()
                    cetak = A_MT
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    para.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    'SC1
                    cell = New PdfPCell()
                    cetak = A_SC1
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    para.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    'SC2
                    cell = New PdfPCell()
                    cetak = A_SC2
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    para.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    'SJ
                    cell = New PdfPCell()
                    cetak = A_SJ
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    para.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    'PI1
                    cell = New PdfPCell()
                    cetak = A_PI1
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    para.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    'PI2
                    cell = New PdfPCell()
                    cetak = A_PI2
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    para.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    'PM
                    cell = New PdfPCell()
                    cetak = A_PM
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    para.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                ElseIf ddlSemester.Text = "4" Then

                    table = New PdfPTable(13)
                    table.WidthPercentage = 105
                    table.SetWidths({2, 20, 7, 7, 5, 5, 5, 5, 5, 5, 5, 5, 5})
                    table.DefaultCell.Border = 0

                    'BILANGAN
                    cell = New PdfPCell()
                    cetak = i + 1
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    'NAMA
                    cell = New PdfPCell()
                    cetak = strNama
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    'ANGKAGILIRAN
                    cell = New PdfPCell()
                    cetak = strAngkaGiliran
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    'MYKAD
                    cell = New PdfPCell()
                    cetak = strMykad
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    ''KODKURSUS
                    'cell = New PdfPCell()
                    'cetak = strKodKursus
                    'para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    'cell.AddElement(para)
                    'cell.Border = 0
                    'table.AddCell(cell)
                    'myDocument.Add(table)

                    'BM
                    cell = New PdfPCell()
                    cetak = A_BM
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    para.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    ''BM1
                    'cell = New PdfPCell()
                    'cetak = A_BM1
                    'para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    'para.Alignment = Element.ALIGN_CENTER
                    'cell.AddElement(para)
                    'cell.Border = 0
                    'table.AddCell(cell)
                    'myDocument.Add(table)

                    ''BM2
                    'cell = New PdfPCell()
                    'cetak = A_BM2
                    'para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    'para.Alignment = Element.ALIGN_CENTER
                    'cell.AddElement(para)
                    'cell.Border = 0
                    'table.AddCell(cell)
                    'myDocument.Add(table)

                    'BM3
                    cell = New PdfPCell()
                    cetak = A_BM3
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    para.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    'BI
                    cell = New PdfPCell()
                    cetak = A_BI
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    para.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    'MT
                    cell = New PdfPCell()
                    cetak = A_MT
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    para.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    'SC1
                    cell = New PdfPCell()
                    cetak = A_SC1
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    para.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    'SC2
                    cell = New PdfPCell()
                    cetak = A_SC2
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    para.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    ''SJ
                    'cell = New PdfPCell()
                    'cetak = A_SJ
                    'para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    'para.Alignment = Element.ALIGN_CENTER
                    'cell.AddElement(para)
                    'cell.Border = 0
                    'table.AddCell(cell)
                    'myDocument.Add(table)

                    'PI1
                    cell = New PdfPCell()
                    cetak = A_PI1
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    para.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    'PI2
                    cell = New PdfPCell()
                    cetak = A_PI2
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    para.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                    'PM
                    cell = New PdfPCell()
                    cetak = A_PM
                    para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                    para.Alignment = Element.ALIGN_CENTER
                    cell.AddElement(para)
                    cell.Border = 0
                    table.AddCell(cell)
                    myDocument.Add(table)

                End If

            Next

            myDocument.Close()

            HttpContext.Current.Response.Write(myDocument)
            HttpContext.Current.Response.End()

        Catch ex As Exception

        End Try
    End Sub

End Class

