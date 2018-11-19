Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Globalization

Public Class markah_vokasional_list
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

                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                ''------exist takwim
                'strSQL = "SELECT * FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Paparan Keputusan Vokasional' AND Aktif='1'"
                'If oCommon.isExist(strSQL) = True Then

                '    'count data takwim
                '    'Get the data from database into datatable
                '    Dim cmd As New SqlCommand("SELECT TakwimID FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Paparan Keputusan Vokasional' AND Aktif='1'")
                '    Dim dt As DataTable = GetData(cmd)

                '    For i As Integer = 0 To dt.Rows.Count - 1
                '        IntTakwim = dt.Rows(i)("TakwimID")

                '        strSQL = "SELECT TarikhMula,TarikhAkhir FROM kpmkv_takwim WHERE TakwimID='" & IntTakwim & "'"
                '        strRet = oCommon.getFieldValueEx(strSQL)

                '        Dim ar_user_login As Array
                '        ar_user_login = strRet.Split("|")
                '        Dim strMula As String = ar_user_login(0)
                '        Dim strAkhir As String = ar_user_login(1)

                '        Dim strdateNow As Date = Date.Now
                '        Dim startDate = DateTime.ParseExact(strMula, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                '        Dim endDate = DateTime.ParseExact(strAkhir, "dd-MM-yyyy", CultureInfo.InvariantCulture)

                '        Dim ts As New TimeSpan
                '        ts = endDate.Subtract(strdateNow)
                '        Dim dayDiff = ts.Days

                '        If strMula IsNot Nothing Then
                '            If strAkhir IsNot Nothing And dayDiff >= 0 Then
                kpmkv_tahun_list()
                kpmkv_semester_list()

                '    'checkinbox
                '    strSQL = "SELECT Sesi FROM kpmkv_takwim WHERE TakwimId='" & IntTakwim & "'ORDER BY Kohort ASC"
                '    strRet = oCommon.getFieldValue(strSQL)

                '    If strRet = 1 Then
                '        chkSesi.Items(0).Enabled = True
                '        'chkSesi.Items(1).Enabled = False
                '    Else
                '        'chkSesi.Items(0).Enabled = False
                '        chkSesi.Items(1).Enabled = True
                '    End If
                'End If
                '            Else
                'lblMsg.Text = "Paparan Keputusan Vokasional telah ditutup!"
                '            End If
                '        Next
                '    Else
                'lblMsg.Text = "Paparan Keputusan Vokasional telah ditutup!"
                '    End If
                'RepoveDuplicate(ddlTahun)
                'RepoveDuplicate(ddlSemester)
            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try
    End Sub
    Private Sub hiddencolumn()
        strSQL = "SELECT COUNT(kpmkv_modul.KodModul) as CModul "
        strSQL += " FROM kpmkv_modul LEFT OUTER JOIN"
        strSQL += " kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID"
        strSQL += " WHERE kpmkv_modul.Tahun='" & ddlTahun.Text & "'"
        strSQL += " AND kpmkv_modul.Semester='" & ddlSemester.Text & "'"
        strSQL += " AND kpmkv_modul.Sesi='" & chkSesi.Text & "'"
        strSQL += " AND kpmkv_modul.KursusID='" & ddlKodKursus.SelectedValue & "'"
        strRet = oCommon.getFieldValue(strSQL)

        Select Case strRet
            Case "3"
                datRespondent.Columns.Item("5").Visible = True
                datRespondent.Columns.Item("6").Visible = True 'modul 1=5
                datRespondent.Columns.Item("7").Visible = True
                datRespondent.Columns.Item("8").Visible = False
                datRespondent.Columns.Item("9").Visible = False
                datRespondent.Columns.Item("10").Visible = False
                datRespondent.Columns.Item("11").Visible = False
                'datRespondent.Columns.Item("12").Visible = False
                'datRespondent.Columns.Item("13").Visible = False
            Case "4"
                datRespondent.Columns.Item("5").Visible = True
                datRespondent.Columns.Item("6").Visible = True 'modul 1=5
                datRespondent.Columns.Item("7").Visible = True
                datRespondent.Columns.Item("8").Visible = True
                datRespondent.Columns.Item("9").Visible = False
                datRespondent.Columns.Item("10").Visible = False
                datRespondent.Columns.Item("11").Visible = False
                'datRespondent.Columns.Item("12").Visible = False
                'datRespondent.Columns.Item("13").Visible = False

            Case "5"
                datRespondent.Columns.Item("5").Visible = True
                datRespondent.Columns.Item("6").Visible = True 'modul 1=5
                datRespondent.Columns.Item("7").Visible = True
                datRespondent.Columns.Item("8").Visible = True
                datRespondent.Columns.Item("9").Visible = True
                datRespondent.Columns.Item("10").Visible = False
                datRespondent.Columns.Item("11").Visible = False
                'datRespondent.Columns.Item("12").Visible = False
                'datRespondent.Columns.Item("13").Visible = False

            Case "6"
                datRespondent.Columns.Item("5").Visible = True
                datRespondent.Columns.Item("6").Visible = True 'modul 1=5
                datRespondent.Columns.Item("7").Visible = True
                datRespondent.Columns.Item("8").Visible = True
                datRespondent.Columns.Item("9").Visible = True
                datRespondent.Columns.Item("10").Visible = True
                datRespondent.Columns.Item("11").Visible = False
                'datRespondent.Columns.Item("12").Visible = False
                'datRespondent.Columns.Item("13").Visible = False

            Case "7"
                datRespondent.Columns.Item("5").Visible = True
                datRespondent.Columns.Item("6").Visible = True 'modul 1=5
                datRespondent.Columns.Item("7").Visible = True
                datRespondent.Columns.Item("8").Visible = True
                datRespondent.Columns.Item("9").Visible = True
                datRespondent.Columns.Item("10").Visible = True
                datRespondent.Columns.Item("11").Visible = True
                'datRespondent.Columns.Item("12").Visible = False
                'datRespondent.Columns.Item("13").Visible = False
            Case "8"
                datRespondent.Columns.Item("5").Visible = True
                datRespondent.Columns.Item("6").Visible = True 'modul 1=5
                datRespondent.Columns.Item("7").Visible = True
                datRespondent.Columns.Item("8").Visible = True
                datRespondent.Columns.Item("9").Visible = True
                datRespondent.Columns.Item("10").Visible = True
                datRespondent.Columns.Item("11").Visible = True
                datRespondent.Columns.Item("12").Visible = True
                datRespondent.Columns.Item("13").Visible = True
        End Select

    End Sub
    Private Sub kpmkv_tahun_list()
        strSQL = "SELECT Tahun FROM kpmkv_tahun ORDER BY Tahun DESC"
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
            ddlTahun.Items.Insert(0, New ListItem("-Pilih-", "0"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_semester_list()
        strSQL = "SELECT Semester FROM kpmkv_semester"
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
            ddlSemester.Items.Insert(0, New ListItem("-Pilih-", "0"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_kodkursus_list()

        strSQL = "SELECT kpmkv_kursus.KodKursus, kpmkv_kursus.KursusID"
        strSQL += " FROM kpmkv_kelas_kursus LEFT OUTER JOIN kpmkv_kursus ON kpmkv_kelas_kursus.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
        strSQL += " kpmkv_kelas ON kpmkv_kelas_kursus.KelasID = kpmkv_kelas.KelasID"
        strSQL += " WHERE kpmkv_kelas.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kursus.Tahun='" & ddlTahun.Text & "' "
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
            ddlKodKursus.Items.Add(New ListItem("-Pilih-", "0"))

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
            ddlKelas.Items.Add(New ListItem("-Pilih-", "0"))

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


        Dim strOrder As String = " ORDER BY kpmkv_pelajar.Tahun, kpmkv_pelajar.Semester, kpmkv_pelajar.Sesi, kpmkv_pelajar.Nama ASC"

        tmpSQL = "SELECT kpmkv_pelajar.PelajarID,  kpmkv_kursus.KodKursus, kpmkv_pelajar.Nama,kpmkv_pelajar.semester, kpmkv_pelajar.MYKAD, kpmkv_pelajar.AngkaGiliran, kpmkv_pelajar_markah.GredV1, "
        tmpSQL += " kpmkv_pelajar_markah.GredV2, kpmkv_pelajar_markah.GredV3, kpmkv_pelajar_markah.GredV4, kpmkv_pelajar_markah.GredV5, kpmkv_pelajar_markah.GredV6, "
        tmpSQL += " kpmkv_pelajar_markah.GredV7, kpmkv_pelajar_markah.GredV8"
        'If ddlSemester.SelectedValue = "2" Then

        tmpSQL += " , kpmkv_pelajar_markah.SMP_Total, kpmkv_pelajar_markah.SMP_Grade"
        'End If

        tmpSQL += " FROM kpmkv_pelajar_markah LEFT OUTER JOIN  kpmkv_pelajar ON kpmkv_pelajar_markah.PelajarID = kpmkv_pelajar.PelajarID "
        tmpSQL += " LEFT OUTER JOIN kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID"
        strWhere = " WHERE kpmkv_pelajar.KolejRecordID='" & lblKolejID.Text & "' and kpmkv_pelajar.IsDeleted='N' and kpmkv_pelajar.StatusID='2'"
       
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
        '--kursus
        If Not ddlKodKursus.Text = "PILIH" Then
            strWhere += " AND kpmkv_pelajar.KursusID ='" & ddlKodKursus.Text & "'"
        End If
        '--jantina
        If Not ddlKelas.Text = "PILIH" Then
            strWhere += " AND kpmkv_pelajar.KelasID ='" & ddlKelas.Text & "'"
        End If

        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        ''Response.Write(getSQL)

        Return getSQL

    End Function

    Private Sub datRespondent_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles datRespondent.SelectedIndexChanging
        Dim strKeyID As String = datRespondent.DataKeys(e.NewSelectedIndex).Value.ToString
        Response.Redirect("pelajar.ulang.vokasional.view.aspx?PelajarID=" & strKeyID)

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
    Protected Sub btnCari_Click(sender As Object, e As EventArgs) Handles btnCari.Click
        lblMsg.Text = ""
        strRet = BindData(datRespondent)
        hiddencolumn()
    End Sub
    Private Sub chkSesi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkSesi.SelectedIndexChanged
        kpmkv_kodkursus_list()
        kpmkv_kelas_list()
        ddlKelas.Text = "0"
    End Sub

    Private Sub ddlKodKursus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlKodKursus.SelectedIndexChanged
        kpmkv_kelas_list()
        ddlKelas.Text = "0"
    End Sub
End Class
