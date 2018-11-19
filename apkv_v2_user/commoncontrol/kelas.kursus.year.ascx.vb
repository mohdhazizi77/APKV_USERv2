Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class kelas_kursus_year
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
                lblMsgTop.Text = ""
                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)
                '------exist takwim
                strSQL = "SELECT * FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Penetapan Program' AND Aktif='1'"
                If oCommon.isExist(strSQL) = True Then

                    'count data takwim
                    'Get the data from database into datatable
                    Dim cmd As New SqlCommand("SELECT TakwimID FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Penetapan Program' AND Aktif='1'")
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
                                    'chkSesi.Items(1).Enabled = False
                                Else
                                    'chkSesi.Items(0).Enabled = False
                                    chkSesi.Items(1).Enabled = True
                                End If
                                btnCreate.Enabled = True
                            End If
                        Else
                            btnCreate.Enabled = False
                            lblMsg.Text = "Penetapan Program telah ditutup!"
                        End If
                    Next
                Else
                    btnCreate.Enabled = False
                    lblMsg.Text = "Penetapan Program telah ditutup!"
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

        strSQL = "SELECT kpmkv_kursus.KodKursus, kpmkv_kursus.KursusID "
        strSQL += " FROM kpmkv_kursus_kolej LEFT OUTER JOIN kpmkv_kursus "
        strSQL += " ON kpmkv_kursus_kolej.KursusID= kpmkv_kursus.KursusID "
        strSQL += " WHERE kpmkv_kursus.Tahun='" & ddlTahun.SelectedValue & "' AND kpmkv_kursus.Sesi='" & chkSesi.SelectedValue & "'"
        strSQL += " AND kpmkv_kursus_kolej.KolejRecordID='" & lblKolejID.Text & "' "
        strSQL += " GROUP BY kpmkv_kursus.KodKursus, kpmkv_kursus.KursusID"

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
        strSQL = " SELECT NamaKelas, KelasID FROM kpmkv_kelas"
        strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "'  AND Tahun= '" & ddlTahun.SelectedValue & "' AND IsDeleted='N' ORDER BY  NamaKelas"

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
            ddlNamaKelas.Items.Add(New ListItem("-Pilih-", "0"))

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
        Dim strOrder As String = " ORDER BY kpmkv_kursus.Tahun, kpmkv_kursus.Sesi,kpmkv_kursus.KodKursus ASC"

        tmpSQL = "SELECT kpmkv_kelas_kursus.KelasID, kpmkv_kelas_kursus.KelasKursusID, kpmkv_kursus.Tahun, kpmkv_kursus.Sesi, kpmkv_kursus.KodKursus, kpmkv_kelas.NamaKelas"
        tmpSQL += " FROM  kpmkv_kelas_kursus LEFT OUTER JOIN kpmkv_kelas ON kpmkv_kelas_kursus.KelasID = kpmkv_kelas.KelasID LEFT OUTER JOIN"
        tmpSQL += " kpmkv_kursus ON kpmkv_kelas_kursus.KursusID = kpmkv_kursus.KursusID"
        strWhere = " WHERE kpmkv_kelas.KolejRecordID='" & lblKolejID.Text & "'  AND kpmkv_kelas_kursus.IsDeleted='N'"

        'tahun()
        If Not ddlTahun.Text = "" Then
            strWhere += " AND kpmkv_kursus.Tahun='" & ddlTahun.Text & "'"
        End If
      
        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        'Response.Write(getSQL)

        Return getSQL

    End Function
 
    Private Sub datRespondent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondent.PageIndexChanging
        datRespondent.PageIndex = e.NewPageIndex
        strRet = BindData(datRespondent)

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

    Protected Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        lblMsg.Text = ""
        lblMsgTop.Text = ""
        Try
            If ValidatePage() = False Then
                divMsg.Attributes("class") = "error"
                DivMsgTop.Attributes("class") = "error"
                Exit Sub
            End If
            'duplicate
            'validate
            strSQL = "SELECT kpmkv_kelas.KelasID FROM  kpmkv_kelas_kursus LEFT OUTER JOIN kpmkv_kelas ON "
            strSQL += " kpmkv_kelas_kursus.KelasID = kpmkv_kelas.KelasID LEFT OUTER JOIN"
            strSQL += " kpmkv_kursus ON kpmkv_kelas_kursus.KursusID = kpmkv_kursus.KursusID"
            strSQL += " WHERE kpmkv_kelas_kursus.KelasID ='" & ddlNamaKelas.SelectedValue & "' AND kpmkv_kelas_kursus.KursusID='" & ddlKodKursus.SelectedValue & "'"
            'Response.Write(strSQL)

            If oCommon.isExist(strSQL) = True Then
                divMsg.Attributes("class") = "eror"
                lblMsgTop.Text = "Kelas telah didaftarkan.Sila Pilih Kelas baru"
                Exit Sub
            Else
                'insert kpmkv_kursus_txn table
                strSQL = "INSERT INTO kpmkv_kelas_kursus(KursusID,KelasID,IsDeleted)"
                strSQL += "VALUES ('" & ddlKodKursus.SelectedValue & "','" & ddlNamaKelas.SelectedValue & "','N')"
                strRet = oCommon.ExecuteSQL(strSQL)
                If strRet = "0" Then
                    divMsg.Attributes("class") = "info"
                    lblMsg.Text = "Pemilihan Kursus berjaya didaftarkan"
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Pemilihan Kursus tidak berjaya"
                End If
            End If
            ''debug
            'Response.Write(strSQL)
            strRet = BindData(datRespondent)

        Catch ex As Exception
            divMsg.Attributes("class") = "error"

        End Try
    End Sub
    Private Function ValidatePage() As Boolean

        If chkSesi.Text = "" Then
            lblMsg.Text = "Sila pilih sesi"
            lblMsgTop.Text = "Sila pilih sesi"
            chkSesi.Focus()
            Return False
        End If

        If ddlKodKursus.Text = "0" Then
            lblMsg.Text = "Sila pilih kod kursus"
            lblMsgTop.Text = "Sila pilih kod kursus"
            ddlKodKursus.Focus()
            Return False
        End If

        If ddlNamaKelas.Text = "0" Then
            lblMsg.Text = "Sila pilih Kelas"
            lblMsgTop.Text = "Sila pilih kelas"
            ddlNamaKelas.Focus()
            Return False
        End If


        Return True
    End Function

    Protected Sub chkSesi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkSesi.SelectedIndexChanged
        kpmkv_kodkursus_list()
        ddlKodKursus.Text = "0"

        'kpmkv_kelas_list()
        ddlNamaKelas.Text = "0"
    End Sub

    Private Sub datRespondent_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles datRespondent.RowCommand
        lblMsg.Text = ""

        If (e.CommandName = "Batal") Then
            Dim strKelasID = Int32.Parse(e.CommandArgument.ToString())
            Try

                'kpmkv_pelajar
                strSQL = "SELECT KelasID FROM kpmkv_pelajar WHERE KelasID='" & strKelasID & "'"
                If oCommon.isExist(strSQL) = True Then
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Kursus tidak berjaya dipadam. Rekod kelas masih wujud pada Calon"
                    lblMsgTop.Text = "Kursus tidak berjaya dipadam. Rekod kelas masih wujud pada Calon"
                    Exit Sub
                End If

                'kpmkv_pelajar_history
                strSQL = "SELECT KelasID FROM kpmkv_pelajar_history WHERE KelasID='" & strKelasID & "'"
                If oCommon.isExist(strSQL) = True Then
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Kursus tidak berjaya dipadam. Rekod kelas masih wujud pada sejarah calon"
                    lblMsgTop.Text = "Kursus tidak berjaya dipadam. Rekod kelas masih wujud pada sejarah calon"
                    Exit Sub
                End If

                'kpmkv_pelajar_ulang
                strSQL = "SELECT KelasID FROM kpmkv_pelajar_ulang WHERE KelasID='" & strKelasID & "'"
                If oCommon.isExist(strSQL) = True Then
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Kursus tidak berjaya dipadam. Rekod kelas masih wujud pada calon ulang"
                    lblMsgTop.Text = "Kursus tidak berjaya dipadam. Rekod kelas masih wujud pada calon ulang"
                    Exit Sub
                End If

                'kpmkv_pensyarah_matapelajaran
                strSQL = "SELECT KelasID FROM kpmkv_pensyarah_matapelajaran WHERE KelasID='" & strKelasID & "'"
                If oCommon.isExist(strSQL) = True Then
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Kursus tidak berjaya dipadam. Rekod kelas masih wujud pada Matapelajaran"
                    lblMsgTop.Text = "Kursus tidak berjaya dipadam. Rekod kelas masih wujud pada matapelajaran"
                    Exit Sub
                End If

                'kpmkv_pensyarah_modul
                strSQL = "SELECT KelasID FROM kpmkv_pensyarah_modul WHERE KelasID='" & strKelasID & "'"
                If oCommon.isExist(strSQL) = True Then
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Kursus tidak berjaya dipadam. Rekod kelas masih wujud pada modul"
                    lblMsgTop.Text = "Kursus tidak berjaya dipadam. Rekod kelas masih wujud pada modul"
                    Exit Sub
                End If

                'kpmkv_pensyarah_modul_history()
                strSQL = "SELECT KelasID FROM kpmkv_pensyarah_modul_history WHERE KelasID='" & strKelasID & "'"
                If oCommon.isExist(strSQL) = True Then
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Kursus tidak berjaya dipadam. Rekod kelas masih wujud pada sejarah modul"
                    lblMsgTop.Text = "Kursus tidak berjaya dipadam. Rekod kelas masih wujud pada sejarah modul"
                    Exit Sub
                End If

                strSQL = "DELETE FROM kpmkv_kelas_kursus WHERE KelasID='" & strKelasID & "'"

                strRet = oCommon.ExecuteSQL(strSQL)
                If strRet = "0" Then
                    divMsg.Attributes("class") = "info"
                    lblMsg.Text = "Kursus berjaya dipadamkan"
                    lblMsgTop.Text = "Kursus berjaya dipadamkan"
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Kursus tidak berjaya dipadam"
                    lblMsgTop.Text = "Kursus tidak berjaya dipadam"

                End If
            Catch ex As Exception
                divMsg.Attributes("class") = "error"
            End Try
        End If
        strRet = BindData(datRespondent)
    End Sub

    Private Sub datRespondent_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles datRespondent.SelectedIndexChanging
        Dim strKeyID As String = datRespondent.DataKeys(e.NewSelectedIndex).Value
        strRet = BindData(datRespondent)
    End Sub

    Private Sub ddlKodKursus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlKodKursus.SelectedIndexChanged
        kpmkv_kelas_list()
        ddlNamaKelas.Text = "0"
    End Sub
End Class