Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class pelajar_kpmkv_transfrer
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
                strSQL = "SELECT * FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Pindah Semester' AND Aktif='1'"
                If oCommon.isExist(strSQL) = True Then

                    'count data takwim
                    'Get the data from database into datatable
                    Dim cmd As New SqlCommand("SELECT TakwimID FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Pindah Semester' AND Aktif='1'")
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
                                kpmkv_transfer_semester_list()
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
                                btnPindah.Enabled = True
                            End If
                        Else
                            btnPindah.Enabled = False
                            lblMsg.Text = "Pindah Semester telah ditutup!"
                        End If
                    Next
                Else
                    btnPindah.Enabled = False
                    lblMsg.Text = "Pindah Semester telah ditutup!"
                End If
                RepoveDuplicate(ddlTahun)
                RepoveDuplicate(ddlSemester)
            End If

            kpmkv_tahunsem_list()
            ddlTahunSem.SelectedValue = Now.Year

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

    Private Sub kpmkv_tahunsem_list()
        strSQL = "SELECT Tahun FROM kpmkv_tahun ORDER BY TahunID"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlTahunSem.DataSource = ds
            ddlTahunSem.DataTextField = "Tahun"
            ddlTahunSem.DataValueField = "Tahun"
            ddlTahunSem.DataBind()


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
    Private Sub kpmkv_transfer_semester_list()
        strSQL = "SELECT Semester FROM kpmkv_semester"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlSemesterTransfer.DataSource = ds
            ddlSemesterTransfer.DataTextField = "Semester"
            ddlSemesterTransfer.DataValueField = "Semester"
            ddlSemesterTransfer.DataBind()

            '--ALL
            ddlSemesterTransfer.Items.Insert(0, New ListItem("-Pilih-", "0"))

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
    Protected Sub btnPindah_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPindah.Click
        lblMsg.Text = ""

        Try
            '--validate
            If ValidatePage() = False Then
                divMsg.Attributes("class") = "error"
                DivMsgTop.Attributes("class") = "error"
                Exit Sub
            End If

            '--execute
            If kpmkv_pelajar_transfer() = True Then

                divMsg.Attributes("class") = "info"
                lblMsg.Text = "Semester Calon berjaya dipindahkan"

                DivMsgTop.Attributes("class") = "info"
                lblMsgTop.Text = "Semester Calon berjaya dipindahkan"
            Else
                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Semester Calon tidak berjaya dipindahkan"

                DivMsgTop.Attributes("class") = "error"
                lblMsgTop.Text = "Semester Calon tidak berjaya dipindahkan"

            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try

    End Sub
    Private Function kpmkv_pelajar_transfer() As Boolean

        'kolejnama
        strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
        Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
        'kolejid
        strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
        lblKolejID.Text = oCommon.getFieldValue(strSQL)

        Dim strmykad As String
        Dim cmd As New SqlCommand(getSQL)
        Dim dt As DataTable = GetData(cmd)

        Try
            For i As Integer = 0 To dt.Rows.Count - 1
                strmykad = dt.Rows(i)("Mykad").ToString()
                Dim strPelajarID As Integer
                strSQL = "SELECT Mykad FROM kpmkv_pelajar "
                strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' AND Tahun='" & ddlTahun.SelectedValue & "'"
                strSQL += " AND Semester='" & ddlSemesterTransfer.SelectedValue & "' AND Sesi ='" & chkSesi.Text & "' AND StatusID='2'"
                strSQL += " AND Mykad='" & strmykad & "'"
                If oCommon.isExist(strSQL) = True Then
                Else
                    strSQL = "INSERT INTO kpmkv_pelajar (KolejRecordID,Pengajian,Tahun,Semester,Sesi,KursusID,AngkaGiliran,"
                    strSQL += "Nama,Mykad,Tel,Email,Jantina,Kaum,Agama,StatusID,JenisCalonID,IsDeleted,"
                    strSQL += " IsCalon,IsBMTahun,IsBMDated,IsSJCalon,IsSJTahun,IsSJDated,TahunSem)"
                    strSQL += " SELECT KolejRecordID,Pengajian,'" & ddlTahun.SelectedValue & "',"
                    strSQL += "'" & ddlSemesterTransfer.SelectedValue & "',Sesi,KursusID,AngkaGiliran,"
                    strSQL += " Nama,'" & strmykad & "',Tel,Email,Jantina,Kaum,Agama,StatusID,JenisCalonID,IsDeleted, "
                    strSQL += " IsCalon,IsBMTahun,IsBMDated,IsSJCalon,IsSJTahun,IsSJDated,'" & ddlTahunSem.SelectedValue & "'"
                    strSQL += " FROM Kpmkv_pelajar "
                    strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' AND Tahun='" & ddlTahun.SelectedValue & "' "
                    strSQL += " AND Semester='" & ddlSemester.SelectedValue & "' AND Sesi ='" & chkSesi.Text & "' AND StatusID='2' "
                    strSQL += " And Mykad='" & strmykad & "' And IsDeleted='N'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                End If

                'check pelajarid
                strSQL = "SELECT PelajarID FROM kpmkv_pelajar "
                strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' AND Tahun='" & ddlTahun.SelectedValue & "'"
                strSQL += " AND Semester='" & ddlSemesterTransfer.SelectedValue & "' AND Sesi ='" & chkSesi.Text & "' AND StatusID='2'"
                strSQL += " AND Mykad='" & strmykad & "'"
                strPelajarID = oCommon.getFieldValue(strSQL)

                strSQL = "SELECT PelajarID FROM kpmkv_pelajar_markah "
                strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' AND Tahun='" & ddlTahun.SelectedValue & "'"
                strSQL += " AND Semester='" & ddlSemesterTransfer.SelectedValue & "' AND Sesi ='" & chkSesi.Text & "'"
                strSQL += " AND PelajarID='" & strPelajarID & "'"
                If oCommon.isExist(strSQL) = True Then
                Else
                    strSQL = "INSERT INTO kpmkv_pelajar_markah (PelajarID,KolejRecordID,KursusID,Tahun,Semester,Sesi)"
                    strSQL += " SELECT PelajarID,KolejRecordID,KursusID,Tahun,Semester,Sesi FROM Kpmkv_pelajar"
                    strSQL += " WHERE PelajarID='" & strPelajarID & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                End If

            Next

        Catch ex As Exception
            divMsg.Attributes("class") = "error"
            lblMsg.Text = "Calon sudah wujud pada semester berkenaan"
            Return False
        End Try

        Return True
    End Function
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

        'kolejnama
        strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
        Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
        'kolejid
        strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
        lblKolejID.Text = oCommon.getFieldValue(strSQL)

        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY kpmkv_pelajar.Tahun, kpmkv_pelajar.Semester, kpmkv_pelajar.Sesi ASC"

        tmpSQL = "SELECT kpmkv_pelajar.PelajarID, kpmkv_pelajar.Tahun, kpmkv_pelajar.Semester, kpmkv_pelajar.Sesi,"
        tmpSQL += " kpmkv_pelajar.Nama, kpmkv_pelajar.MYKAD, kpmkv_pelajar.AngkaGiliran, "
        tmpSQL += " kpmkv_kursus.KodKursus, kpmkv_kelas.NamaKelas"
        tmpSQL += " FROM  kpmkv_pelajar "
        tmpSQL += " LEFT OUTER JOIN kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID "
        tmpSQL += " LEFT OUTER JOIN kpmkv_status ON kpmkv_pelajar.StatusID = kpmkv_status.StatusID"
        tmpSQL += " LEFT OUTER JOIN kpmkv_kelas ON kpmkv_pelajar.KelasID = kpmkv_kelas.KelasID"
        tmpSQL += " LEFT OUTER JOIN kpmkv_pelajar_markah ON kpmkv_pelajar.PelajarID =kpmkv_pelajar_markah.PelajarID"
        strWhere = " WHERE kpmkv_pelajar.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_pelajar.IsDeleted='N' "
        strWhere += " AND kpmkv_pelajar.StatusID='2'"

        'tahun
        If Not ddlTahun.Text = "0" Then
            strWhere += " AND kpmkv_pelajar.Tahun='" & ddlTahun.Text & "'"
        End If

        '--semester
        If Not ddlSemester.Text = "0" Then
            strWhere += " AND kpmkv_pelajar.Semester='" & ddlSemester.SelectedValue & " '"
        End If

        'sesi
        If Not chkSesi.Text = "" Then
            strWhere += " AND kpmkv_pelajar.Sesi ='" & chkSesi.Text & "'"
        End If

        strWhere += " AND kpmkv_pelajar_markah.gredv1 IN ('A', 'A-', 'B+', 'B', 'B-') "
        strWhere += " AND (kpmkv_pelajar_markah.gredv2 IN ('A', 'A-', 'B+', 'B', 'B-','') OR kpmkv_pelajar_markah.gredv2 IS NULL)"
        strWhere += " AND (kpmkv_pelajar_markah.gredv3 IN ('A', 'A-', 'B+', 'B', 'B-','') OR kpmkv_pelajar_markah.gredv3 IS NULL)"
        strWhere += " AND (kpmkv_pelajar_markah.gredv4 IN ('A', 'A-', 'B+', 'B', 'B-','') OR kpmkv_pelajar_markah.gredv4 IS NULL)"
        strWhere += " AND (kpmkv_pelajar_markah.gredv5 IN ('A', 'A-', 'B+', 'B', 'B-','') OR kpmkv_pelajar_markah.gredv5 IS NULL)"
        strWhere += " AND (kpmkv_pelajar_markah.gredv6 IN ('A', 'A-', 'B+', 'B', 'B-','') OR kpmkv_pelajar_markah.gredv6 IS NULL)"
        strWhere += " AND (kpmkv_pelajar_markah.gredv7 IN ('A', 'A-', 'B+', 'B', 'B-','') OR kpmkv_pelajar_markah.gredv7 IS NULL)"
        strWhere += " AND (kpmkv_pelajar_markah.gredv8 IN ('A', 'A-', 'B+', 'B', 'B-','') OR kpmkv_pelajar_markah.gredv8 IS NULL)"

        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        'Response.Write(getSQL)

        Return getSQL

    End Function

    Private Sub datRespondent_PageIndexChanged(sender As Object, e As EventArgs) Handles datRespondent.PageIndexChanged
        datRespondent.DataBind()

    End Sub
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

    Protected Sub btnCari_Click(sender As Object, e As EventArgs) Handles btnCari.Click
        lblMsg.Text = ""
        lblMsgTop.Text = ""
        strRet = BindData(datRespondent)

    End Sub

    Private Function ValidatePage() As Boolean

        If ddlSemesterTransfer.Text <= ddlSemester.Text Then
            lblMsg.Text = "Sila pilih semester akan datang"
            lblMsgTop.Text = "Sila pilih semester akan datang"
            ddlSemesterTransfer.Focus()
            Return False
        End If


        Return True
    End Function

End Class