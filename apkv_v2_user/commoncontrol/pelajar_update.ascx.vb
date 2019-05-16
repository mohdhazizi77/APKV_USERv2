Imports System.Data.SqlClient
Imports System.Globalization

Partial Public Class pelajar_update
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""
    Dim IntTakwim As Integer = 0

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnDelete.Attributes.Add("onclick", "return confirm('Pasti ingin menghapuskan rekod ini?');")

        Try
            If Not IsPostBack Then

                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)



                takwim()


                strRet = BindData(datRespondent)

            End If


        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try

    End Sub
    Private Sub takwim()
        '------exist takwim
        strSQL = "SELECT * FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Calon Carian' AND Aktif='1'"
        If oCommon.isExist(strSQL) = True Then

            'count data takwim
            'Get the data from database into datatable
            Dim cmd As New SqlCommand("SELECT TakwimID FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Calon Carian' AND Aktif='1'")
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

                        lblMsg.Text = ""

                        kpmkv_kaum_list()
                        kpmkv_status_list()
                        kpmkv_jeniscalon_list()
                        LoadPage()

                        strSQL = "SELECT KlusterID FROM kpmkv_kursus WHERE KodKursus='" & lblKodKursus.Text & "' AND Tahun = '" & lblTahun.Text & "' AND Sesi = '" & lblSesi.Text & "'"
                        Dim strKlusterID As String = oCommon.getFieldValue(strSQL)

                        strSQL = "SELECT NamaKluster FROM kpmkv_kluster WHERE KlusterID='" & strKlusterID & "' AND Tahun = '" & lblTahun.Text & "'"
                        lblKluster.Text = oCommon.getFieldValue(strSQL)

                        'personal hidden
                        txtNama.Enabled = False
                        txtMYKAD.Enabled = False
                        lblMykad.Enabled = False
                        lblAngkaGiliran.Enabled = False
                        chkJantina.Enabled = False
                        ddlKaum.Enabled = False
                        chkAgama.Enabled = False
                        txtEmail.Enabled = False
                    End If
                Else
                    lblMsg.Text = ""

                    kpmkv_kaum_list()
                    kpmkv_status_list()
                    kpmkv_jeniscalon_list()
                    LoadPage()

                    strSQL = "SELECT KlusterID FROM kpmkv_kursus WHERE KodKursus='" & lblKodKursus.Text & "' AND Tahun = '" & lblTahun.Text & "' AND Sesi = '" & lblSesi.Text & "'"
                    Dim strKlusterID As String = oCommon.getFieldValue(strSQL)

                    strSQL = "SELECT NamaKluster FROM kpmkv_kluster WHERE KlusterID='" & strKlusterID & "' AND Tahun = '" & lblTahun.Text & "'"
                    lblKluster.Text = oCommon.getFieldValue(strSQL)

                    'personal hidden
                    txtNama.Enabled = False
                    txtMYKAD.Enabled = False
                    lblMykad.Enabled = False
                    lblAngkaGiliran.Enabled = False
                    chkJantina.Enabled = False
                    ddlKaum.Enabled = False
                    chkAgama.Enabled = False
                    txtEmail.Enabled = False

                    lblMsg.Text = "Calon Carian Akademik telah ditutup!"
                End If
            Next
        Else
            lblMsg.Text = ""
            LoadPage()
            kpmkv_kaum_list()
            kpmkv_status_list()
            kpmkv_jeniscalon_list()

            strSQL = "SELECT KlusterID FROM kpmkv_kursus WHERE KodKursus='" & lblKodKursus.Text & "' AND Tahun = '" & lblTahun.Text & "' AND Sesi = '" & lblSesi.Text & "'"
            Dim strKlusterID As String = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT NamaKluster FROM kpmkv_kluster WHERE KlusterID='" & strKlusterID & "' AND Tahun = '" & lblTahun.Text & "'"
            lblKluster.Text = oCommon.getFieldValue(strSQL)

            If lblSemester.Text = "1" Then
                txtNama.Enabled = True
                txtMYKAD.Enabled = True
                lblMykad.Enabled = True
                lblAngkaGiliran.Enabled = True
                chkJantina.Enabled = True
                ddlKaum.Enabled = True
                chkAgama.Enabled = True
                txtEmail.Enabled = True
            Else
                ''personal hidden
                txtNama.Enabled = False
                txtMYKAD.Enabled = False
                lblMykad.Enabled = False
                lblAngkaGiliran.Enabled = False
                chkJantina.Enabled = False
                ddlKaum.Enabled = False
                chkAgama.Enabled = False
                txtEmail.Enabled = False
                ''txtCatatan.Enabled = False
            End If


            lblMsg.Text = ""
        End If

    End Sub

    Private Sub kpmkv_kaum_list()
        strSQL = "SELECT Kaum FROM kpmkv_kaum ORDER BY Kaum"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlKaum.DataSource = ds
            ddlKaum.DataTextField = "Kaum"
            ddlKaum.DataValueField = "Kaum"
            ddlKaum.DataBind()

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_status_list()
        strSQL = "SELECT StatusID, Status FROM kpmkv_status ORDER BY StatusID"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlStatus.DataSource = ds
            ddlStatus.DataTextField = "Status"
            ddlStatus.DataValueField = "StatusID"
            ddlStatus.DataBind()

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_jeniscalon_list()
        strSQL = "SELECT JenisCalonID, JenisCalon FROM kpmkv_jeniscalon WHERE JenisCalonID <>'4' ORDER BY JenisCalonID"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlJenisCalon.DataSource = ds
            ddlJenisCalon.DataTextField = "Jeniscalon"
            ddlJenisCalon.DataValueField = "JenisCalonID"
            ddlJenisCalon.DataBind()

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Sub LoadPage()
        strSQL = "SELECT kpmkv_pelajar.Pengajian, kpmkv_pelajar.Tahun, kpmkv_pelajar.Semester, kpmkv_pelajar.Sesi, kpmkv_pelajar.Nama, kpmkv_pelajar.MYKAD, kpmkv_pelajar.AngkaGiliran, "
        strSQL += " kpmkv_jeniscalon.JenisCalonID, kpmkv_kursus.KursusID, kpmkv_kursus.KodKursus, kpmkv_kursus.NamaKursus, kpmkv_pelajar.Kaum, kpmkv_pelajar.Jantina, "
        strSQL += " kpmkv_pelajar.Agama, kpmkv_status.StatusID, kpmkv_pelajar.Email, kpmkv_pelajar.Catatan, kpmkv_kelas.NamaKelas"
        strSQL += " FROM  kpmkv_pelajar LEFT OUTER JOIN kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
        strSQL += " kpmkv_status ON kpmkv_pelajar.StatusID = kpmkv_status.StatusID LEFT OUTER JOIN kpmkv_kelas ON kpmkv_pelajar.KelasID = kpmkv_kelas.KelasID"
        strSQL += " LEFT OUTER JOIN kpmkv_jeniscalon ON kpmkv_pelajar.JenisCalonID = kpmkv_jeniscalon.JenisCalonID "
        strSQL += " WHERE kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.PelajarID='" & Request.QueryString("PelajarID") & "'"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            Dim nRows As Integer = 0
            Dim nCount As Integer = 1
            Dim MyTable As DataTable = New DataTable
            MyTable = ds.Tables(0)
            If MyTable.Rows.Count > 0 Then

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Pengajian")) Then
                    lblPengajian.Text = ds.Tables(0).Rows(0).Item("Pengajian")
                Else
                    lblPengajian.Text = ""
                End If

                '--Account Details 
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Tahun")) Then
                    lblTahun.Text = ds.Tables(0).Rows(0).Item("Tahun")
                Else
                    lblTahun.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Sesi")) Then
                    lblSesi.Text = ds.Tables(0).Rows(0).Item("Sesi")
                Else
                    lblSesi.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("NamaKursus")) Then
                    lblNamaKursus.Text = ds.Tables(0).Rows(0).Item("NamaKursus")
                Else
                    lblNamaKursus.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("KodKursus")) Then
                    lblKodKursus.Text = ds.Tables(0).Rows(0).Item("KodKursus")
                Else
                    lblKodKursus.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("NamaKelas")) Then
                    lblNamaKelas.Text = ds.Tables(0).Rows(0).Item("NamaKelas")
                Else
                    lblNamaKelas.Text = ""
                End If
                'personal
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Nama")) Then
                    txtNama.Text = ds.Tables(0).Rows(0).Item("Nama")
                Else
                    txtNama.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Mykad")) Then
                    txtMYKAD.Text = ds.Tables(0).Rows(0).Item("Mykad")
                    lblMykad.Text = txtMYKAD.Text
                Else
                    txtMYKAD.Text = ""
                    lblMykad.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("AngkaGiliran")) Then
                    lblAngkaGiliran.Text = ds.Tables(0).Rows(0).Item("AngkaGiliran")
                Else
                    lblAngkaGiliran.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Jantina")) Then
                    chkJantina.Text = ds.Tables(0).Rows(0).Item("Jantina")
                Else
                    chkJantina.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Kaum")) Then
                    ddlKaum.Text = ds.Tables(0).Rows(0).Item("Kaum")
                Else
                    ddlKaum.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Agama")) Then
                    chkAgama.Text = ds.Tables(0).Rows(0).Item("Agama")
                Else
                    chkAgama.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("StatusID")) Then
                    ddlStatus.Text = ds.Tables(0).Rows(0).Item("StatusID")
                Else
                    ddlStatus.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("JenisCalonID")) Then
                    ddlJenisCalon.Text = ds.Tables(0).Rows(0).Item("JenisCalonID")
                Else
                    ddlJenisCalon.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Email")) Then
                    txtEmail.Text = ds.Tables(0).Rows(0).Item("Email")
                Else
                    txtEmail.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Catatan")) Then
                    txtCatatan.Text = ds.Tables(0).Rows(0).Item("Catatan")
                Else
                    txtCatatan.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Semester")) Then
                    lblSemester.Text = ds.Tables(0).Rows(0).Item("Semester")
                Else
                    lblSemester.Text = ""
                End If
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        lblMsg.Text = ""

        Try
            '--validate
            If ValidatePage() = False Then
                divMsg.Attributes("class") = "error"
                Exit Sub
            End If

            '--execute aktif

            If kpmkv_pelajar_update() = True Then
                If kpmkv_pelajar_update_status() = True Then
                    divMsg.Attributes("class") = "info"
                    lblMsg.Text = "Berjaya mengemaskini maklumat pelajar."
                Else
                    divMsg.Attributes("class") = "error"
                End If
            Else
                divMsg.Attributes("class") = "error"
            End If
        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try

    End Sub

    Private Function ValidatePage() As Boolean

        '--txtNama
        If txtNama.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan Nama Calon!"
            txtNama.Focus()
            Return False
        End If

        '--txtMYKAD
        If txtMYKAD.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan MYKAD Calon!"
            txtMYKAD.Focus()
            Return False
        End If

        '--changes made
        If Not txtMYKAD.Text = lblMykad.Text Then
            strSQL = "SELECT MYKAD FROM kpmkv_pelajar WHERE MYKAD='" & oCommon.FixSingleQuotes(txtMYKAD.Text) & "'"
            If oCommon.isExist(strSQL) = True Then
                lblMsg.Text = "MYKAD# telah digunakan. Sila masukkan MYKAD# yang baru."
                txtMYKAD.Focus()
                Return False
            End If
        End If

        '--ddlAgama
        If chkAgama.Text = "" Then
            lblMsg.Text = "Sila pilih jenis Agama!"
            chkAgama.Focus()
            Return False
        End If

        '--ddlJantina
        If chkJantina.Text = "" Then
            lblMsg.Text = "Sila pilih jenis Jantina!"
            chkJantina.Focus()
            Return False
        End If

        '--txtEmail
        If txtEmail.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan Email Calon!"
        ElseIf oCommon.isEmail(txtEmail.Text) = False Then
            lblMsg.Text = "Emel Calon tidak sah. (Contoh: Emel@contoh.com)"
            Return False
        End If
        Return True
    End Function

    Private Function kpmkv_pelajar_update() As Boolean
        strSQL = "UPDATE kpmkv_pelajar SET Nama='" & oCommon.FixSingleQuotes(txtNama.Text.ToUpper) & "', "
        strSQL += " Mykad='" & oCommon.FixSingleQuotes(txtMYKAD.Text) & "', "
        strSQL += " Agama='" & chkAgama.Text & "', "
        strSQL += " Kaum='" & ddlKaum.Text & "', "
        strSQL += " Jantina='" & chkJantina.Text & "', "
        strSQL += " Email='" & txtEmail.Text & "'"
        strSQL += " WHERE PelajarID='" & Request.QueryString("PelajarID") & "'"
        strRet = oCommon.ExecuteSQL(strSQL)
        If strRet = "0" Then
            Return True
        Else
            lblMsg.Text = "System Error:" & strRet
            Return False
        End If

    End Function

    Private Function kpmkv_pelajar_update_status() As Boolean
        strSQL = "UPDATE kpmkv_pelajar SET StatusID='" & ddlStatus.SelectedValue & "',JenisCalonID='" & ddlJenisCalon.SelectedValue & "',"
        strSQL += " Catatan='" & oCommon.FixSingleQuotes(txtCatatan.Text.ToUpper) & "' "
        strSQL += " WHERE PelajarID='" & Request.QueryString("PelajarID") & "'"
        strRet = oCommon.ExecuteSQL(strSQL)
        If strRet = "0" Then
            Return True
        Else
            lblMsg.Text = "System Error:" & strRet
            Return False
        End If

    End Function

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        strSQL = "DELETE FROM kpmkv_pelajar WHERE PelajarID='" & Request.QueryString("PelajarID") & "'"
        strRet = oCommon.ExecuteSQL(strSQL)
        If strRet = "0" Then

            strSQL = "DELETE FROM kpmkv_pelajar_markah WHERE PelajarID='" & Request.QueryString("PelajarID") & "'"
            strRet = oCommon.ExecuteSQL(strSQL)

            lblMsg.Text = "Berjaya meghapuskan rekod pelajar tersebut."
        Else
            lblMsg.Text = "System Error:" & strRet
        End If

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
        Dim strOrder As String = ""

        tmpSQL = "SELECT p.pelajarID,p.Tahun,p.Semester ,p.sesi,s.Status"
        tmpSQL += " FROM kpmkv_pelajar p "
        tmpSQL += " LEFT JOIN kpmkv_status s on s.StatusID =p.statusID "
        tmpSQL += " WHERE p.Mykad='" & lblMykad.Text & "' AND p.KolejRecordID='" & lblKolejID.Text & "'"

        getSQL = tmpSQL & strWhere & strOrder

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

    Protected Sub CheckUncheckAll(sender As Object, e As System.EventArgs)
        Dim chk1 As CheckBox
        chk1 = DirectCast(datRespondent.HeaderRow.Cells(0).FindControl("chkAll"), CheckBox)
        For Each row As GridViewRow In datRespondent.Rows
            Dim chk As CheckBox
            chk = DirectCast(row.Cells(0).FindControl("chkSelect"), CheckBox)
            chk.Checked = chk1.Checked
        Next
    End Sub

    Protected Sub btndeactivate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btndeactivate.Click
        Try
            For i As Integer = 0 To datRespondent.Rows.Count - 1

                Dim cb As CheckBox = datRespondent.Rows(i).FindControl("chkSelect")

                If cb.Checked = True Then

                    Dim strkey As String = datRespondent.DataKeys(i).Value.ToString

                    strSQL = " UPDATE kpmkv_pelajar SET StatusID='1' , JenisCalonID='1' WHERE PelajarID='" & strkey & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)

                    If Not strRet = 0 Then
                        divMsg.Attributes("class") = "error"
                        lblMsg.Text = "Kemaskini Status Tidak Pelajar Berjaya"

                    End If
                End If
            Next
            divMsg.Attributes("class") = "info"
            lblMsg.Text = "Kemaskini Status Pelajar Berjaya"


            strRet = BindData(datRespondent)
        Catch ex As Exception
            divMsg.Attributes("class") = "error"
            lblMsg.Text = "System Error. " & ex.Message
        End Try


    End Sub
End Class