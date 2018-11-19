Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class kolej_kursus_list1
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

                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                '------exist takwim
                strSQL = "SELECT * FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Pendaftaran Program' AND Aktif='1'"
                If oCommon.isExist(strSQL) = True Then

                    'count data takwim
                    'Get the data from database into datatable
                    Dim cmd As New SqlCommand("SELECT TakwimID FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Pendaftaran Program' AND Aktif='1'")
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
                                    'chkSesi.Items(0).Enabled = False
                                    chkSesi.Items(1).Enabled = True
                                End If
                            End If
                        Else
                            lblMsg.Text = "Pendaftaran Program telah ditutup!"
                        End If
                    Next
                Else
                    lblMsg.Text = "Pendaftaran Program telah ditutup!"
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
            ddlTahun.Items.Add(strRet)

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
   
    Private Sub kpmkv_kluster_list()
        strSQL = "SELECT KlusterID,NamaKluster FROM kpmkv_kluster WHERE Tahun = '" & ddlTahun.SelectedValue & "' AND IsDeleted='N' ORDER BY NamaKluster"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlKluster.DataSource = ds
            ddlKluster.DataTextField = "NamaKluster"
            ddlKluster.DataValueField = "KlusterID"
            ddlKluster.DataBind()

            '--ALL
            ddlKluster.Items.Insert(0, New ListItem("-Semua Kluster-", "0"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_kodkursus_list()
       
        strSQL = "SELECT KodKursus FROM kpmkv_kursus"
        strSQL += " WHERE Tahun='" & ddlTahun.SelectedValue & "' AND Sesi='" & chkSesi.SelectedValue & "'  AND KlusterID='" & ddlKluster.Text & "' GROUP BY KodKursus"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlKodKursus.DataSource = ds
            ddlKodKursus.DataTextField = "KodKursus"
            ddlKodKursus.DataValueField = "KodKursus"
            ddlKodKursus.DataBind()

                '--ALL
            ddlKodKursus.Items.Insert(0, New ListItem("-Semua KodKursus", "0"))

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
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        lblMsgKursus.Text = ""
        lblMsg.Text = ""
        lblMsgTop.Text = ""

        strRet = BindData(datRespondent)
        strRet = BindDataKursus(datRespondentKursus)
        ' tbl_menu_check()
    End Sub
    Private Function BindDataKursus(ByVal gvTable As GridView) As Boolean
        Dim myDataSet As New DataSet
        Dim myDataAdapter As New SqlDataAdapter(getSQLKursus, strConn)
        myDataAdapter.SelectCommand.CommandTimeout = 120

        Try
            myDataAdapter.Fill(myDataSet, "myaccount")

            If myDataSet.Tables(0).Rows.Count = 0 Then
                divMsgKursus.Attributes("class") = "error"
                lblMsgKursus.Text = "Rekod tidak dijumpai!"
            Else
                divMsgKursus.Attributes("class") = "info"
                lblMsgKursus.Text = "Jumlah Rekod#:" & myDataSet.Tables(0).Rows.Count
            End If

            gvTable.DataSource = myDataSet
            gvTable.DataBind()
            objConn.Close()
        Catch ex As Exception
            lblMsgKursus.Text = "System Error:" & ex.Message
            Return False
        End Try

        Return True

    End Function
    Private Function getSQLKursus() As String
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY kpmkv_kursus.Tahun, kpmkv_kursus.KodKursus ASC"

        tmpSQL = "SELECT kpmkv_kursus_kolej.KursusID,kpmkv_kursus_kolej.TxnKursusKolejID, kpmkv_kluster.NamaKluster, kpmkv_kursus.Tahun, kpmkv_kursus.Sesi,"
        tmpSQL += " kpmkv_kursus_kolej.KolejRecordID, kpmkv_kursus.KodKursus,kpmkv_kursus.NamaKursus FROM kpmkv_kursus_kolej LEFT OUTER JOIN kpmkv_kursus "
        tmpSQL += " ON kpmkv_kursus_kolej.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN kpmkv_kluster "
        tmpSQL += " ON kpmkv_kursus.KlusterID = kpmkv_kluster.KlusterID"
        strWhere = "  WHERE kpmkv_kursus_kolej.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kursus_kolej.IsDeleted='N'"

        '--tahun
        If Not ddlTahun.Text = "PILIH" Then
            strWhere += " AND kpmkv_kursus.Tahun='" & ddlTahun.Text & "'"
        End If

        If Not chkSesi.SelectedValue = "" Then
            strWhere += " AND kpmkv_kursus.Sesi='" & chkSesi.SelectedValue & "'"
        End If

        ''--kod
        'If Not ddlKluster.Text = "0" And Not ddlKluster.Text = "" Then
        '    strWhere += " AND kpmkv_kursus.KlusterID='" & ddlKluster.Text & "'"
        'End If

        ''--kod
        'If Not ddlKodKursus.Text = "0" And Not ddlKodKursus.Text = "" Then
        '    strWhere += " AND kpmkv_kursus.KodKursus='" & ddlKodKursus.Text & "'"
        'End If

        getSQLKursus = tmpSQL & strWhere & strOrder
        ''--debug
        'Response.Write(getSQL)

        Return getSQLKursus

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
        End Try
        con.Close()
        sda.Dispose()
        con.Dispose()

    End Function
    Private Sub datRespondentKursus_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondentKursus.PageIndexChanging
        datRespondentKursus.PageIndex = e.NewPageIndex
        strRet = BindData(datRespondentKursus)

    End Sub
    Private Sub datRespondentKursus_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles datRespondentKursus.RowCommand
        lblMsgKursus.Text = ""
        lblMsg.Text = ""
        lblMsgTop.Text = ""

        If (e.CommandName = "Batal") Then

            Dim strTxnKursusKolejID = Int32.Parse(e.CommandArgument.ToString())

            'delete penetapan pensyarah -kelas
            strSQL = "DELETE FROM kpmkv_kursus_kolej WHERE TxnKursusKolejID='" & strTxnKursusKolejID & "'"
            If strRet = oCommon.ExecuteSQL(strSQL) = 0 Then
                divMsgKursus.Attributes("class") = "info"
                lblMsgKursus.Text = "Kursus berjaya dipadamkan"
            Else
                divMsgKursus.Attributes("class") = "error"
                lblMsgKursus.Text = "Kursus tidak berjaya dipadamkan"
            End If
        End If
        strRet = BindDataKursus(datRespondentKursus)
        strRet = BindData(datRespondent)
    End Sub
    Private Function getSQL() As String
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY kpmkv_kursus.Tahun, kpmkv_kursus.Sesi, kpmkv_kluster.NamaKluster ASC"

        tmpSQL = "SELECT kpmkv_kursus.KursusID, kpmkv_kursus.Tahun, kpmkv_kursus.Sesi, kpmkv_kluster.NamaKluster, kpmkv_kursus.KodKursus, kpmkv_kursus.NamaKursus"
        tmpSQL += " FROM kpmkv_kursus LEFT OUTER JOIN kpmkv_kluster ON kpmkv_kursus.KlusterID = kpmkv_kluster.KlusterID"
        strWhere = " WHERE kpmkv_kursus.IsDeleted='N'"

        '--tahun
        If Not ddlTahun.Text = "PILIH" Then
            strWhere += " AND kpmkv_kursus.Tahun='" & ddlTahun.Text & "'"
        End If

        If Not chkSesi.SelectedValue = "" Then
            strWhere += " AND kpmkv_kursus.Sesi='" & chkSesi.SelectedValue & "'"
        End If

        '--kod
        If Not ddlKluster.Text = "0" And Not ddlKluster.Text = "" Then
            strWhere += " AND kpmkv_kursus.KlusterID='" & ddlKluster.Text & "'"
        End If

        '--kod
        If Not ddlKodKursus.Text = "0" And Not ddlKodKursus.Text = "" Then
            strWhere += " AND kpmkv_kursus.KodKursus='" & ddlKodKursus.Text & "'"
        End If

        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        'Response.Write(getSQL)

        Return getSQL

    End Function
    
    Protected Sub ddlTahun_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTahun.SelectedIndexChanged
        lblMsgKursus.Text = ""
        lblMsg.Text = ""
        lblMsgTop.Text = ""
       
    End Sub
    Protected Sub ddlKluster_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlKluster.SelectedIndexChanged
        lblMsgKursus.Text = ""
        lblMsg.Text = ""
        lblMsgTop.Text = ""
        kpmkv_kodkursus_list()
    End Sub
    Protected Sub chkSesi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkSesi.SelectedIndexChanged
        lblMsgKursus.Text = ""
        lblMsg.Text = ""
        lblMsgTop.Text = ""
        kpmkv_kluster_list()
        ' kpmkv_kodkursus_list()
    End Sub

    Protected Sub datRespondent_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles datRespondent.RowCommand
        lblMsgKursus.Text = ""
        lblMsg.Text = ""
        lblMsgTop.Text = ""
        If (e.CommandName = "Adddata") Then

            Dim KursusID = Int32.Parse(e.CommandArgument.ToString())


            ' If isChecked.Checked Then
            'duplicate
            strSQL = "SELECT KursusID FROM kpmkv_kursus_kolej WHERE KursusID='" & KursusID & "' AND KolejRecordID='" & lblKolejID.Text & "' AND IsDeleted='N'"
            If oCommon.isExist(strSQL) = True Then
                lblMsgTop.Text = "Kursus telah didaftarkan. Sila daftar kursus baru."
            Else
                'insert kpmkv_kursus_kolej
                strSQL = "INSERT INTO kpmkv_kursus_kolej(KursusID, KolejRecordID, IsDeleted)"
                strSQL += "VALUES ('" & KursusID & "','" & lblKolejID.Text & "','N')"
                strRet = oCommon.ExecuteSQL(strSQL)

                'Response.Write(strSQL)
                If strRet = "0" Then
                    divMsg.Attributes("class") = "info"
                    DivMsgTop.Attributes("class") = "info"
                    lblMsg.Text = "Kursus berjaya didaftarkan"
                    lblMsgTop.Text = "Kursus berjaya didaftarkan"
                Else
                    ''-debug
                    ''--lblMsg.Text += isChecked.ToString & vbCrLf
                    divMsg.Attributes("class") = "error"
                    DivMsgTop.Attributes("class") = "error"
                    lblMsg.Text = "Kursus tidak berjaya didaftarkan"
                    lblMsgTop.Text = "Kursus tidak berjaya didaftarkan"

                End If
            End If
        End If
        strRet = BindData(datRespondent)
        strRet = BindDataKursus(datRespondentKursus)
    End Sub

    Private Sub datRespondent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondent.PageIndexChanging
        datRespondent.PageIndex = e.NewPageIndex
        strRet = BindData(datRespondent)

    End Sub
End Class