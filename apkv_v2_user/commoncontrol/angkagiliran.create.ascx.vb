Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class angkagiliran_create
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""
    Dim IntTakwim As Integer = 0

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblMsgResult.Text = ""
        lblMsg.Text = ""
        Try
            If Not IsPostBack Then


                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)
                '------exist takwim
                strSQL = "SELECT * FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Jana AngkaGiliran' AND Aktif='1'"
                If oCommon.isExist(strSQL) = True Then

                    'count data takwim
                    'Get the data from database into datatable
                    Dim cmd As New SqlCommand("SELECT TakwimID FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Jana AngkaGiliran' AND Aktif='1'")
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
                                btnAngkagiliran.Enabled = True
                                btnBatal.Enabled = True
                            End If
                        Else
                            btnAngkagiliran.Enabled = False
                            btnBatal.Enabled = False
                            lblMsg.Text = "Jana AngkaGiliran telah ditutup!"
                        End If
                    Next
                Else
                    btnAngkagiliran.Enabled = False
                    btnBatal.Enabled = False
                    lblMsg.Text = "Jana AngkaGiliran telah ditutup!"
                End If
                RepoveDuplicate(ddlTahun)
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try
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
        strSQL += " FROM kpmkv_kelas_kursus LEFT OUTER JOIN kpmkv_kursus ON kpmkv_kelas_kursus.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
        strSQL += " kpmkv_kelas ON kpmkv_kelas_kursus.KelasID = kpmkv_kelas.KelasID"
        strSQL += " WHERE kpmkv_kelas.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kursus.Tahun='" & ddlTahun.Text & "' AND kpmkv_kursus.Sesi='" & chkSesi.SelectedValue & "'  GROUP BY kpmkv_kursus.KodKursus, kpmkv_kursus.KursusID"
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

            ddlNamaKelas.DataSource = ds
            ddlNamaKelas.DataTextField = "NamaKelas"
            ddlNamaKelas.DataValueField = "KelasID"
            ddlNamaKelas.DataBind()

            '--ALL
            'ddlNamaKelas.Items.Add(New ListItem("PILIH", "PILIH"))

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
        Dim strOrder As String = " ORDER BY kpmkv_pelajar.Nama ASC"

        '--not deleted
        tmpSQL = "SELECT kpmkv_pelajar.PelajarID, kpmkv_pelajar.Tahun, kpmkv_pelajar.Semester, kpmkv_pelajar.Sesi, kpmkv_pelajar.Nama, kpmkv_pelajar.MYKAD, kpmkv_pelajar.AngkaGiliran, "
        tmpSQL += " kpmkv_kursus.KodKursus, kpmkv_kelas.NamaKelas"
        tmpSQL += " FROM  kpmkv_pelajar LEFT OUTER JOIN kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
        tmpSQL += " kpmkv_status ON kpmkv_pelajar.StatusID = kpmkv_status.StatusID LEFT OUTER JOIN kpmkv_kelas ON kpmkv_pelajar.KelasID = kpmkv_kelas.KelasID"
        strWhere = " WHERE kpmkv_pelajar.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_pelajar.IsDeleted='N'"

        '--tahun
        If Not ddlTahun.Text = "PILIH" Then
            strWhere += " AND kpmkv_pelajar.Tahun ='" & ddlTahun.Text & "'"
        End If
        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere += " AND kpmkv_pelajar.Sesi ='" & chkSesi.Text & "'"
        End If
        '--Kod
        If Not ddlKodKursus.Text = "" Then
            strWhere += " AND kpmkv_pelajar.KursusID='" & ddlKodKursus.SelectedValue & "'"
        End If
        '--sesi
        If Not ddlNamaKelas.Text = "" Then
            strWhere += " AND kpmkv_pelajar.KelasID ='" & ddlNamaKelas.SelectedValue & "'"
        End If


        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        ' 'Response.Write(getSQL)

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

    Protected Sub btnCari_Click(sender As Object, e As EventArgs) Handles btnCari.Click
        lblMsg.Text = ""
        strRet = BindData(datRespondent)
    End Sub

    Protected Sub btnAngkagiliran_Click(sender As Object, e As EventArgs) Handles btnAngkagiliran.Click
        lblMsg.Text = ""
        lblMsgResult.Text = ""

        Try
            Dim str As String
            strSQL = "SELECT  TOP 1 AngkaGiliran FROM kpmkv_pelajar WHERE Tahun='" & ddlTahun.Text & "' AND Sesi='" & chkSesi.Text & "' "
            strSQL += " AND KursusID='" & ddlKodKursus.SelectedValue & "' AND KelasID='" & ddlNamaKelas.SelectedValue & "' AND KolejRecordID='" & lblKolejID.Text & "' ORDER BY AngkaGiliran DESC"
            strRet = oCommon.getFieldValue(strSQL)

            If Not strRet = "" Then
                divMsgResult.Attributes("class") = "info"
                lblMsgResult.Text = "AngkaGiliran Telah Dijana!"
                Exit Try
            Else
            End If

            'No. Kolej 0	1
            Dim strMedan123 As String = ""
            strSQL = "SELECT Kod FROM kpmkv_kolej WHERE RecordID='" & lblKolejID.Text & "'"
            strMedan123 = oCommon.getFieldValue(strSQL)

            'Pengambilan
            Dim strMedan4 As String = ""
            If chkSesi.Text = "1" Then
                strMedan4 = "1"
            Else
                strMedan4 = "2"
            End If

            'Tahun
            Dim strMedan5 As String = ""
            strSQL = "SELECT Kod FROM kpmkv_tahun WHERE Tahun='" & ddlTahun.SelectedValue & "'"
            strMedan5 = oCommon.getFieldValue(strSQL)

            'Kursus W	T	P
            Dim strMedan6_7_8 As String = ""
            strMedan6_7_8 = ddlKodKursus.SelectedItem.Text

            'No Siri Pelajar 0	0	1
            Dim strcheckID As String
            Dim strMedan9_10_11 As String
            strSQL = "SELECT  TOP 1 AngkaGiliran FROM kpmkv_pelajar WHERE Tahun='" & ddlTahun.Text & "' AND Sesi='" & chkSesi.Text & "'  AND KursusID='" & ddlKodKursus.SelectedValue & "' AND KolejRecordID='" & lblKolejID.Text & "' ORDER BY AngkaGiliran DESC"
            strRet = oCommon.getFieldValue(strSQL)
            If strRet = "" Then
                strcheckID = ""
            Else
                strcheckID = strRet.Substring(8, 3)
            End If
            'Dim getNum As String = strcheckID.Substring(9, 3)

            ' data pelajar 

            For i As Integer = 0 To datRespondent.Rows.Count - 1
                Dim row As GridViewRow = datRespondent.Rows(i)
                str = datRespondent.DataKeys(i).Value.ToString


                If Not strcheckID = "" Then
                    strMedan9_10_11 = CInt(strcheckID + 1)
                Else
                    strMedan9_10_11 = "001"
                End If

                'set number

                If (strMedan9_10_11.Length = 1) Then
                    strMedan9_10_11 = "00" + strMedan9_10_11.ToString()

                ElseIf (strMedan9_10_11.Length = 2) Then
                    strMedan9_10_11 = "0" + strMedan9_10_11.ToString()
                End If
                strcheckID = strMedan9_10_11

                Dim strAngkaGiliranBaru As String = strMedan123 + strMedan4 + strMedan5 + strMedan6_7_8 + strMedan9_10_11
                'validate
                strSQL = "SELECT AngkaGiliran FROM kpmkv_pelajar WHERE AngkaGiliran='" & strAngkaGiliranBaru & "' AND PelajarID='" & str & "'"
                If oCommon.isExist(strSQL) = False Then
                    strSQL = "UPDATE kpmkv_pelajar SET AngkaGiliran='" & strAngkaGiliranBaru & "' WHERE PelajarID='" & str & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                    If Not strRet = "0" Then
                        divMsgResult.Attributes("class") = "error"
                        lblMsgResult.Text = "Jana AngkaGiliran Calon Tidak Berjaya!"
                        Exit Sub
                    Else
                        divMsgResult.Attributes("class") = "info"
                        lblMsgResult.Text = "Jana AngkaGiliran Calon Berjaya!"
                    End If
                Else
                    divMsgResult.Attributes("class") = "error"
                    lblMsgResult.Text = "Tidak Berjaya! AngkaGiliran Calon sudah wujud."
                End If
            Next


        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

        strRet = BindData(datRespondent)
    End Sub

    Private Sub chkSesi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkSesi.SelectedIndexChanged
        kpmkv_kodkursus_list()
        kpmkv_kelas_list()
    End Sub

    Private Sub ddlKodKursus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlKodKursus.SelectedIndexChanged
        kpmkv_kelas_list()
    End Sub

    Protected Sub btnBatal_Click(sender As Object, e As EventArgs) Handles btnBatal.Click
        lblMsg.Text = ""
        lblMsgResult.Text = ""

        Dim str As String

        For i As Integer = 0 To datRespondent.Rows.Count - 1
            Dim row As GridViewRow = datRespondent.Rows(0)
            Dim cb As CheckBox = datRespondent.Rows(i).FindControl("chkSelect")
            str = datRespondent.DataKeys(i).Value.ToString
            strSQL = "UPDATE kpmkv_pelajar SET AngkaGiliran='' WHERE Tahun='" & ddlTahun.Text & "' AND Sesi='" & chkSesi.Text & "'  AND KursusID='" & ddlKodKursus.SelectedValue & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            ' strSQL = "UPDATE kpmkv_pelajar SET AngkaGiliran='' WHERE PelajarID='" & str & "'"
            strRet = oCommon.ExecuteSQL(strSQL)
        Next
        divMsgResult.Attributes("class") = "info"
        lblMsgResult.Text = "Jana AngkaGiliran Calon berjaya dibatalkan!"
        strRet = BindData(datRespondent)
    End Sub
End Class