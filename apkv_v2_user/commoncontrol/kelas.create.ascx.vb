Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class kelas_create
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

                lblMsg.Text = ""
                lblMsgTop.Text = ""

                '------exist takwim
                strSQL = "SELECT * FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Pendaftaran Kelas' AND Aktif='1'"
                If oCommon.isExist(strSQL) = True Then

                    'count data takwim
                    'Get the data from database into datatable
                    Dim cmd As New SqlCommand("SELECT TakwimID FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Pendaftaran Kelas' AND Aktif='1'")
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
                                strRet = BindData(datRespondent)
                                btnCreate.Enabled = True
                            End If
                        Else
                            btnCreate.Enabled = False
                            lblMsg.Text = "Pendaftaran Program telah ditutup!"
                        End If
                    Next
                Else
                    btnCreate.Enabled = False
                    lblMsg.Text = "Pendaftaran Program telah ditutup!"
                End If
                RepoveDuplicate(ddlTahun)
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
            lblMsgTop.Text = "System Error:" & ex.Message
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
    Private Function BindData(ByVal gvTable As GridView) As Boolean
        Dim myDataSet As New DataSet
        Dim myDataAdapter As New SqlDataAdapter(getSQL, strConn)
        myDataAdapter.SelectCommand.CommandTimeout = 120

        Try
            myDataAdapter.Fill(myDataSet, "myaccount")

            If myDataSet.Tables(0).Rows.Count = 0 Then
                DivTopMsg.Attributes("class") = "error"
                lblMsgTop.Text = "Rekod tidak dijumpai!"
            Else
                divMsg.Attributes("class") = "info"
                lblMsg.Text = "Jumlah Rekod#:" & myDataSet.Tables(0).Rows.Count
            End If

            gvTable.DataSource = myDataSet
            gvTable.DataBind()
            objConn.Close()
        Catch ex As Exception
            lblMsgTop.Text = "System Error:" & ex.Message
            Return False
        End Try

        Return True

    End Function
    Private Function getSQL() As String
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY Tahun ASC"

        tmpSQL = "SELECT * FROM kpmkv_kelas"
        strWhere = " WHERE IsDeleted='N' AND KolejRecordID='" & lblKolejID.Text & "'"

        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        ''Response.Write(getSQL)

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
    Protected Sub btnCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCreate.Click
        lblMsg.Text = ""

        Try
            '--validate
            If ValidatePage() = False Then
                divMsg.Attributes("class") = "error"
                Exit Sub
            End If

            '--execute
            If kpmkv_kelas_create() = True Then
                divMsg.Attributes("class") = "info"
                DivTopMsg.Attributes("class") = "info"
                lblMsgTop.Text = "Kelas berjaya didaftarkan"
                lblMsg.Text = "Kelas berjaya didaftarkan"
            Else
                divMsg.Attributes("class") = "error"
                DivTopMsg.Attributes("class") = "error"
                lblMsgTop.Text = "Kelas tidak berjaya didaftarkan"
                lblMsg.Text = "Kelas tidak berjaya didaftarkan"
            End If
            strRet = BindData(datRespondent)

        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try

    End Sub
    Private Function ValidatePage() As Boolean

        '--txtNama
        If txtNamaKelas.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan Nama Kelas!"
            txtNamaKelas.Focus()
            Return False
        End If

        strSQL = "SELECT * FROM kpmkv_kelas WHERE Tahun='" & ddlTahun.SelectedValue & "' AND NamaKelas='" & txtNamaKelas.Text & "' and KolejRecordID='" & lblKolejID.Text & "' AND IsDeleted='N'"
        If oCommon.isExist(strSQL) = True Then
            lblMsg.Text = "Nama Kelas telah digunakan. Sila masukkan Nama Kelas yang baru."
            Return False
        End If

        Return True
    End Function
    Private Function kpmkv_kelas_create() As Boolean
        'kpmkv_kelas
        strSQL = "INSERT INTO kpmkv_kelas(KolejRecordID,Tahun,NamaKelas,IsDeleted) "
        strSQL += "VALUES ('" & lblKolejID.Text & "','" & ddlTahun.SelectedValue & "','" & oCommon.FixSingleQuotes(txtNamaKelas.Text) & "','N')"
        strRet = oCommon.ExecuteSQL(strSQL)
        If strRet = "0" Then
            Return True
        Else
            lblMsg.Text = "System Error:" & strRet
            Return False
        End If

    End Function

    Private Sub datRespondent_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles datRespondent.SelectedIndexChanging
        Dim strKeyID As String = datRespondent.DataKeys(e.NewSelectedIndex).Value
        strRet = BindData(datRespondent)
    End Sub

    Private Sub datRespondent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondent.PageIndexChanging
        datRespondent.PageIndex = e.NewPageIndex
        strRet = BindData(datRespondent)
    End Sub

    Private Sub datRespondent_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles datRespondent.RowDeleting
        lblMsg.Text = ""
        lblMsgTop.Text = ""
        Dim strKelasID As Integer = datRespondent.DataKeys(e.RowIndex).Values("KelasID")
            Try
            If Not strKelasID = Session("KelasID") Then
                'kpmkv_pelajar
                strSQL = "SELECT KelasID FROM kpmkv_pelajar WHERE KelasID='" & strKelasID & "'"
                If oCommon.isExist(strSQL) = True Then
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Kelas tidak berjaya dipadam. Rekod kelas masih wujud pada Calon"
                    lblMsgTop.Text = "Kelas tidak berjaya dipadam. Rekod kelas masih wujud pada Calon"
                    Session("strKelasID") = ""
                    Exit Sub
                End If

                'kpmkv_pelajar_history
                strSQL = "SELECT KelasID FROM kpmkv_pelajar_history WHERE KelasID='" & strKelasID & "'"
                If oCommon.isExist(strSQL) = True Then
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Kelas tidak berjaya dipadam. Rekod kelas masih wujud pada sejarah calon"
                    lblMsgTop.Text = "Kelas tidak berjaya dipadam. Rekod kelas masih wujud pada sejarah calon"
                    Session("strKelasID") = ""
                    Exit Sub
                End If

                'kpmkv_pelajar_ulang
                strSQL = "SELECT KelasID FROM kpmkv_pelajar_ulang WHERE KelasID='" & strKelasID & "'"
                If oCommon.isExist(strSQL) = True Then
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Kelas tidak berjaya dipadam. Rekod kelas masih wujud pada calon ulang"
                    lblMsgTop.Text = "Kelas tidak berjaya dipadam. Rekod kelas masih wujud pada calon ulang"
                    Session("strKelasID") = ""
                    Exit Sub
                End If

                'kpmkv_pensyarah_matapelajaran
                strSQL = "SELECT KelasID FROM kpmkv_pensyarah_matapelajaran WHERE KelasID='" & strKelasID & "'"
                If oCommon.isExist(strSQL) = True Then
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Kelas tidak berjaya dipadam. Rekod kelas masih wujud pada Matapelajaran"
                    lblMsgTop.Text = "Kelas tidak berjaya dipadam. Rekod kelas masih wujud pada matapelajaran"
                    Session("strKelasID") = ""
                    Exit Sub
                End If

                'kpmkv_pensyarah_modul
                strSQL = "SELECT KelasID FROM kpmkv_pensyarah_modul WHERE KelasID='" & strKelasID & "'"
                If oCommon.isExist(strSQL) = True Then
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Kelas tidak berjaya dipadam. Rekod kelas masih wujud pada modul"
                    lblMsgTop.Text = "Kelas tidak berjaya dipadam. Rekod kelas masih wujud pada modul"
                    Session("strKelasID") = ""
                    Exit Sub
                End If

                'kpmkv_pensyarah_modul_history()
                strSQL = "SELECT KelasID FROM kpmkv_pensyarah_modul_history WHERE KelasID='" & strKelasID & "'"
                If oCommon.isExist(strSQL) = True Then
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Kelas tidak berjaya dipadam. Rekod kelas masih wujud pada sejarah modul"
                    lblMsgTop.Text = "Kelas tidak berjaya dipadam. Rekod kelas masih wujud pada sejarah modul"
                    Session("strKelasID") = ""
                    Exit Sub
                End If

                'delete 
                strSQL = "DELETE FROM kpmkv_kelas WHERE KelasID='" & strKelasID & "'"

                If strRet = oCommon.ExecuteSQL(strSQL) = 0 Then
                    divMsg.Attributes("class") = "info"
                    lblMsg.Text = "Kelas berjaya dipadamkan"
                    Session("strKelasID") = ""
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Kelas tidak berjaya dipadamkan"
                    Session("strKelasID") = ""
                End If
            Else
                Session("strKelasID") = ""
            End If
            Catch ex As Exception
                divMsg.Attributes("class") = "error"
        End Try

        strRet = BindData(datRespondent)
    End Sub
End Class