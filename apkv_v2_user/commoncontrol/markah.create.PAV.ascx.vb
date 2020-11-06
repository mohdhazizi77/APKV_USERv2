Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Globalization
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class markah_create_PAV
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
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                '------exist takwim
                strSQL = "SELECT * FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Penilaian Akhir Vokasional' AND Aktif='1'"
                If oCommon.isExist(strSQL) = True Then

                    'count data takwim
                    'Get the data from database into datatable
                    Dim cmd As New SqlCommand("SELECT TakwimID FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Penilaian Akhir Vokasional' AND Aktif='1'")
                    Dim dt As DataTable = GetData(cmd)

                    For i As Integer = 0 To dt.Rows.Count - 1
                        IntTakwim = dt.Rows(i)("TakwimID")

                        strSQL = "SELECT TarikhMula,TarikhAkhir FROM kpmkv_takwim WHERE TakwimID='" & IntTakwim & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_user_login As Array
                        ar_user_login = strRet.Split("|")
                        Dim strMula As String = ar_user_login(0)
                        Dim strAkhir As String = ar_user_login(1)

                        Dim strdateNow As Date = Date.Now.Date
                        Dim startDate = DateTime.ParseExact(strMula, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                        Dim endDate = DateTime.ParseExact(strAkhir, "dd-MM-yyyy", CultureInfo.InvariantCulture)

                        Dim ts As New TimeSpan
                        ts = startDate.Subtract(strdateNow)
                        Dim dayDiffMula = ts.Days
                        ts = endDate.Subtract(strdateNow)
                        Dim dayDiffAkhir = ts.Days

                        If strMula IsNot Nothing And dayDiffMula <= 0 Then
                            If strAkhir IsNot Nothing And dayDiffAkhir >= 0 Then
                                kpmkv_tahun_list()
                                kpmkv_semester_list()

                                'checkinbox
                                strSQL = "SELECT Sesi FROM kpmkv_takwim WHERE TakwimId='" & IntTakwim & "'ORDER BY Kohort ASC"
                                strRet = oCommon.getFieldValue(strSQL)

                                If strRet = 1 Then
                                    chkSesi.Items(0).Enabled = True
                                    'chkSesi.Items(1).Enabled = False
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
                            lblMsg.Text = "Penilaian Akhir Vokasional telah ditutup!"
                        End If
                    Next
                Else
                    btnGred.Enabled = False
                    btnUpdate.Enabled = False
                    lblMsg.Text = "Penilaian Akhir Vokasional telah ditutup!"
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
    Private Function getSQLEks() As String

        Dim tmpSQL As String = ""
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY kpmkv_pelajar.Nama ASC"

        tmpSQL = "SELECT kpmkv_pelajar.PelajarID,  kpmkv_pelajar.Nama, kpmkv_pelajar.MYKAD, kpmkv_pelajar.AngkaGiliran, kpmkv_kursus.KodKursus, "
        tmpSQL += " kpmkv_pelajar_markah.A_Amali1"
        tmpSQL += " FROM  kpmkv_pelajar_markah LEFT OUTER JOIN kpmkv_pelajar ON kpmkv_pelajar_markah.PelajarID = kpmkv_pelajar.PelajarID"
        tmpSQL += " LEFT OUTER Join kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID"
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
        If Not ddlKodKursus.Text = "" Then
            strWhere += " AND kpmkv_pelajar.KursusID ='" & ddlKodKursus.SelectedValue & "'"
        End If
        '--jantina
        If Not ddlKelas.Text = "" Then
            strWhere += " AND kpmkv_pelajar.KelasID ='" & ddlKelas.SelectedValue & "'"
        End If

        getSQLEks = tmpSQL & strWhere & strOrder
        ''--debug
        ''Response.Write(getSQL)

        Return getSQLEks

    End Function
    Private Function getSQL() As String

        Dim tmpSQL As String = ""
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY kpmkv_pelajar.Nama ASC"

        tmpSQL = "SELECT kpmkv_pelajar.PelajarID,  kpmkv_pelajar.Nama, kpmkv_pelajar.MYKAD, kpmkv_pelajar.AngkaGiliran, kpmkv_kursus.KodKursus, "
        tmpSQL += " kpmkv_pelajar_markah.A_Amali1, kpmkv_pelajar_markah.A_Amali2, kpmkv_pelajar_markah.A_Amali3, kpmkv_pelajar_markah.A_Amali4,"
        tmpSQL += " kpmkv_pelajar_markah.A_Amali5, kpmkv_pelajar_markah.A_Amali6, kpmkv_pelajar_markah.A_Amali7, kpmkv_pelajar_markah.A_Amali8,"
        tmpSQL += " kpmkv_pelajar_markah.A_Teori1, kpmkv_pelajar_markah.A_Teori2, kpmkv_pelajar_markah.A_Teori3, kpmkv_pelajar_markah.A_Teori4,"
        tmpSQL += " kpmkv_pelajar_markah.A_Teori5, kpmkv_pelajar_markah.A_Teori6, kpmkv_pelajar_markah.A_Teori7, kpmkv_pelajar_markah.A_Teori8"
        tmpSQL += " FROM  kpmkv_pelajar_markah LEFT OUTER JOIN kpmkv_pelajar ON kpmkv_pelajar_markah.PelajarID = kpmkv_pelajar.PelajarID"
        tmpSQL += " LEFT OUTER Join kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID"
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
        If Not ddlKodKursus.Text = "" Then
            strWhere += " AND kpmkv_pelajar.KursusID ='" & ddlKodKursus.SelectedValue & "'"
        End If
        '--jantina
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
        If ValidateForm() = False Then
            divMsg.Attributes("class") = "error"
            lblMsg.Text = "Sila Semak Data Markah.[Huruf],[Kosong] adalah tidak dibenarkan!  Markah mestilah diantara -1-100 sahaja"
            divMsgResult.Attributes("class") = "error"
            lblMsgResult.Text = "Sila Semak Data Markah.[Huruf],[Kosong] adalah tidak dibenarkan!  Markah mestilah diantara -1-100 sahaja"


            Exit Sub
        End If

        '--count no of modul
        Dim nCount As Integer = 0
        strSQL = "SELECT COUNT(kpmkv_modul.KodModul) as CModul "
        strSQL += " FROM kpmkv_modul LEFT OUTER JOIN"
        strSQL += " kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID"
        strSQL += " WHERE kpmkv_modul.KursusID='" & ddlKodKursus.SelectedValue & "'"
        strSQL += " AND kpmkv_modul.Semester='" & ddlSemester.SelectedValue & "'"
        strSQL += " AND kpmkv_modul.Sesi='" & chkSesi.Text & "'"
        nCount = oCommon.getFieldValueInt(strSQL)

        For i As Integer = 0 To datRespondent.Rows.Count - 1
            Dim row As GridViewRow = datRespondent.Rows(i)
            Dim strAmali1 As TextBox = datRespondent.Rows(i).FindControl("txtAmali1")


            'assign value to integer
            Dim Amali1 As String = strAmali1.Text


            If nCount = 2 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET  A_Amali1='" & Amali1 & "', A_Amali2='" & Amali1 & "'"
                strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' AND PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            ElseIf nCount = 3 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET  A_Amali1='" & Amali1 & "', A_Amali2='" & Amali1 & "',"
                strSQL += " A_Amali3='" & Amali1 & "'"
                strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' AND PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            ElseIf nCount = 4 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET  A_Amali1='" & Amali1 & "', A_Amali2='" & Amali1 & "',"
                strSQL += " A_Amali3='" & Amali1 & "', A_Amali4='" & Amali1 & "'"
                strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' AND PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            ElseIf nCount = 5 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET  A_Amali1='" & Amali1 & "', A_Amali2='" & Amali1 & "',"
                strSQL += " A_Amali3='" & Amali1 & "', A_Amali4='" & Amali1 & "'"
                strSQL += " A_Amali5='" & Amali1 & "'"
                strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' AND PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            ElseIf nCount = 6 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET  A_Amali1='" & Amali1 & "', A_Amali2='" & Amali1 & "',"
                strSQL += " A_Amali3='" & Amali1 & "', A_Amali4='" & Amali1 & "',"
                strSQL += " A_Amali5='" & Amali1 & "', A_Amali6='" & Amali1 & "'"
                strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' AND PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            ElseIf nCount = 7 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET  A_Amali1='" & Amali1 & "', A_Amali2='" & Amali1 & "',"
                strSQL += " A_Amali3='" & Amali1 & "', A_Amali4='" & Amali1 & "',"
                strSQL += " A_Amali5='" & Amali1 & "', A_Amali6='" & Amali1 & "',"
                strSQL += " A_Amali7='" & Amali1 & "'"
                strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' AND PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            ElseIf nCount = 8 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET  A_Amali1='" & Amali1 & "', A_Amali2='" & Amali1 & "',"
                strSQL += " A_Amali3='" & Amali1 & "', A_Amali4='" & Amali1 & "',"
                strSQL += " A_Amali5='" & Amali1 & "', A_Amali6='" & Amali1 & "',"
                strSQL += " A_Amali7='" & Amali1 & "', A_Amali8='" & Amali1 & "'"
                strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' AND PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            End If
            strRet = oCommon.ExecuteSQL(strSQL)
            If Not strRet = "0" Then
                divMsgResult.Attributes("class") = "error"
                lblMsgResult.Text = "Tidak Berjaya mengemaskini markah Pentaksiran Akhir Vokasional"
            End If
        Next

        divMsgResult.Attributes("class") = "info"
        lblMsgResult.Text = "Berjaya mengemaskini markah Pentaksiran Akhir Vokasional"
        strRet = BindData((datRespondent))
        ' hiddencolumn()
    End Sub

    Private Function ValidateForm() As Boolean
        For i As Integer = 0 To datRespondent.Rows.Count - 1
            Dim row As GridViewRow = datRespondent.Rows(i)
            Dim strAmali1 As TextBox = CType(row.FindControl("txtAmali1"), TextBox)


            '--validate NUMBER and less than 100
            '--amali1
            If Not strAmali1.Text = "" Then

                If oCommon.IsCurrency(strAmali1.Text) = False Then
                    Return False
                End If
                If CInt(strAmali1.Text) > 100 Then
                    Return False
                End If
                If CInt(strAmali1.Text) < -1 Then
                    Return False
                End If
            End If

        Next

        Return True
    End Function
    Private Sub Vokasional_markah()

        strSQL = "SELECT KodModul from kpmkv_modul WHERE KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "' AND KodModul Like '%01%' "
        Dim strModul1 As String = oCommon.getFieldValue(strSQL) '1

        strSQL = "SELECT KodModul from kpmkv_modul WHERE KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "' AND KodModul Like '%02%' "
        Dim strModul2 As String = oCommon.getFieldValue(strSQL) '2

        strSQL = "SELECT KodModul from kpmkv_modul WHERE KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "' AND KodModul Like '%03%' "
        Dim strModul3 As String = oCommon.getFieldValue(strSQL) '3

        strSQL = "SELECT KodModul from kpmkv_modul WHERE KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "' AND KodModul Like '%04%' "
        Dim strModul4 As String = oCommon.getFieldValue(strSQL) '4

        strSQL = "SELECT KodModul from kpmkv_modul WHERE KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "' AND KodModul Like '%05%' "
        Dim strModul5 As String = oCommon.getFieldValue(strSQL) '5

        strSQL = "SELECT KodModul from kpmkv_modul WHERE KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "' AND KodModul Like '%06%' "
        Dim strModul6 As String = oCommon.getFieldValue(strSQL) '6

        strSQL = "SELECT KodModul from kpmkv_modul WHERE KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "' AND KodModul Like '%07%' "
        Dim strModul7 As String = oCommon.getFieldValue(strSQL) '6

        strSQL = "SELECT KodModul from kpmkv_modul WHERE KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "' AND KodModul Like '%08%' "
        Dim strModul8 As String = oCommon.getFieldValue(strSQL) '6

        For i As Integer = 0 To datRespondent.Rows.Count - 1


            Dim PBAmali1 As Integer
            Dim PBTeori1 As Integer
            Dim PAAmali1 As Integer
            Dim PATeori1 As Integer

            Dim B_Amali1 As Double
            Dim B_Teori1 As Double
            Dim A_Amali1 As Double
            Dim A_Teori1 As Double

            Dim PBA1 As Double
            Dim PBT1 As Double
            Dim PAA1 As Double
            Dim PAT1 As Double
            Dim PointerM1 As Integer

            'B_Amali1, B_Amali2, B_Amali3,B_Amali4, B_Amali5, B_Amali6, B_Amali7, B_Amali8, 
            'B_Teori1, B_Teori2, B_Teori3, B_Teori4, B_Teori5, B_Teori6, B_Teori7, B_Teori8, 
            'A_Amali1, A_Amali2, A_Amali3, A_Amali4, A_Amali5, A_Amali6, A_Amali7, A_Amali8,
            'A_Teori1, A_Teori2, A_Teori3, A_Teori4, A_Teori5, A_Teori6, A_Teori7, A_Teori8,
            strSQL = "Select PBAmali from kpmkv_modul Where KodModul= '" & strModul1 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
            PBAmali1 = oCommon.getFieldValue(strSQL)

            strSQL = "Select PBTeori from kpmkv_modul Where KodModul= '" & strModul1 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
            PBTeori1 = oCommon.getFieldValue(strSQL)

            strSQL = "Select PAAmali from kpmkv_modul Where KodModul= '" & strModul1 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
            PAAmali1 = oCommon.getFieldValue(strSQL)

            strSQL = "Select PATeori from kpmkv_modul Where KodModul= '" & strModul1 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
            PATeori1 = oCommon.getFieldValue(strSQL)

            strSQL = "Select B_Amali1 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            B_Amali1 = oCommon.getFieldValue(strSQL)
            'round up
            B_Amali1 = (B_Amali1)

            strSQL = "Select B_Teori1 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            B_Teori1 = oCommon.getFieldValue(strSQL)
            'round up
            B_Teori1 = (B_Teori1)

            strSQL = "Select A_Amali1 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            A_Amali1 = oCommon.getFieldValue(strSQL)
            'round up
            A_Amali1 = (A_Amali1)

            strSQL = "Select A_Teori1 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            A_Teori1 = oCommon.getFieldValueInt(strSQL)
            'round up
            A_Teori1 = (A_Teori1)

            'convert 0 if null
            If (String.IsNullOrEmpty(B_Amali1.ToString())) Then
                B_Amali1 = 0
            End If

            If (String.IsNullOrEmpty(B_Teori1.ToString())) Then
                B_Teori1 = 0
            End If

            If (String.IsNullOrEmpty(A_Amali1.ToString())) Then
                A_Amali1 = 0
            End If

            If (String.IsNullOrEmpty(A_Teori1.ToString())) Then
                A_Teori1 = 0
            End If

            If (B_Amali1 = "-1" Or B_Teori1 = "-1" Or A_Amali1 = "-1" Or A_Teori1 = "-1") Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET PBPAM1='-1',"
                strSQL += "PBAV1='-1',PBTV1='-1',PAAV1='-1',PATV1='-1'"
                strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            Else
                'PBM1 = (((B_Amali1 / 100) * PBAmali1) + ((B_Teori1 / 100) * PBTeori1))
                'PAM1 = (((A_Amali1 / 100) * PAAmali1) + ((A_Teori1 / 100) * PATeori1))
                PBA1 = ((B_Amali1 / 100) * PBAmali1)
                PBT1 = ((B_Teori1 / 100) * PBTeori1)
                PAA1 = ((A_Amali1 / 100) * PAAmali1)
                PAT1 = ((A_Teori1 / 100) * PATeori1)

                'change on 31/7/2017 PBAV1,PBTV1,PAAV1,PATV1
                PointerM1 = Math.Ceiling(PBA1 + PBT1 + PAA1 + PAT1)
                strSQL = "UPDATE kpmkv_pelajar_markah SET PBPAM1='" & PointerM1 & "',"
                strSQL += "PBAV1='" & PBA1 & "',PBTV1='" & PBT1 & "',PAAV1='" & PAA1 & "',PATV1='" & PAT1 & "'"
                strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            'Modu1------------------------
            If Not String.IsNullOrEmpty(strModul2) Then
                Dim PBAmali2 As Integer
                Dim PBTeori2 As Integer
                Dim PAAmali2 As Integer
                Dim PATeori2 As Integer

                Dim B_Amali2 As Double
                Dim B_Teori2 As Double
                Dim A_Amali2 As Double
                Dim A_Teori2 As Double
                Dim PBA2 As Double
                Dim PBT2 As Double
                Dim PAA2 As Double
                Dim PAT2 As Double
                Dim PointerM2 As Integer

                strSQL = "Select PBAmali from kpmkv_modul Where KodModul= '" & strModul2 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PBAmali2 = oCommon.getFieldValue(strSQL)

                strSQL = "Select PBTeori from kpmkv_modul Where KodModul= '" & strModul2 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PBTeori2 = oCommon.getFieldValue(strSQL)

                strSQL = "Select PAAmali from kpmkv_modul Where KodModul= '" & strModul2 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PAAmali2 = oCommon.getFieldValue(strSQL)

                strSQL = "Select PATeori from kpmkv_modul Where KodModul= '" & strModul2 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PATeori2 = oCommon.getFieldValue(strSQL)

                strSQL = "Select B_Amali2 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_Amali2 = oCommon.getFieldValue(strSQL)
                'round up
                B_Amali2 = (B_Amali2)

                strSQL = "Select B_Teori2 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_Teori2 = oCommon.getFieldValue(strSQL)
                'round up
                B_Teori2 = (B_Teori2)

                strSQL = "Select A_Amali2 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                A_Amali2 = oCommon.getFieldValue(strSQL)
                'round up
                A_Amali2 = (A_Amali2)

                strSQL = "Select A_Teori2 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                A_Teori2 = oCommon.getFieldValue(strSQL)
                'round up
                A_Teori2 = (A_Teori2)

                'convert 0 if null
                If (String.IsNullOrEmpty(B_Amali2.ToString())) Then
                    B_Amali2 = 0
                End If

                If (String.IsNullOrEmpty(B_Teori2.ToString())) Then
                    B_Teori2 = 0
                End If

                If (String.IsNullOrEmpty(A_Amali2.ToString())) Then
                    A_Amali2 = 0
                End If

                If (String.IsNullOrEmpty(A_Teori2.ToString())) Then
                    A_Teori2 = 0
                End If

                If (B_Amali2 = "-1" Or B_Teori2 = "-1" Or A_Amali2 = "-1" Or A_Teori2 = "-1") Then
                    strSQL = "UPDATE kpmkv_pelajar_markah SET PBPAM2='-1',"
                    strSQL += "PBAV2='-1',PBTV2='-1',PAAV2='-1',PATV2='-1'"
                    strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)

                Else
                    PBA2 = ((B_Amali2 / 100) * PBAmali2)
                    PBT2 = ((B_Teori2 / 100) * PBTeori2)
                    PAA2 = ((A_Amali2 / 100) * PAAmali2)
                    PAT2 = ((A_Teori2 / 100) * PATeori2)

                    'change on 31/7/2017 PBAV1,PBTV1,PAAV1,PATV1
                    PointerM2 = Math.Ceiling(PBA2 + PBT2 + PAA2 + PAT2)
                    strSQL = "UPDATE kpmkv_pelajar_markah SET PBPAM2='" & PointerM2 & "',"
                    strSQL += "PBAV2='" & PBA2 & "',PBTV2='" & PBT2 & "',PAAV2='" & PAA2 & "',PATV2='" & PAT2 & "'"
                    strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                End If
            End If
            'Modul2---------------------------------
            If Not String.IsNullOrEmpty(strModul3) Then
                Dim PBAmali3 As Integer
                Dim PBTeori3 As Integer
                Dim PAAmali3 As Integer
                Dim PATeori3 As Integer

                Dim B_Amali3 As Double
                Dim B_Teori3 As Double
                Dim A_Amali3 As Double
                Dim A_Teori3 As Double
                Dim PBA3 As Double
                Dim PBT3 As Double
                Dim PAA3 As Double
                Dim PAT3 As Double
                Dim PointerM3 As Integer

                strSQL = "Select PBAmali from kpmkv_modul Where KodModul= '" & strModul3 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PBAmali3 = oCommon.getFieldValue(strSQL)

                strSQL = "Select PBTeori from kpmkv_modul Where KodModul= '" & strModul3 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PBTeori3 = oCommon.getFieldValue(strSQL)

                strSQL = "Select PAAmali from kpmkv_modul Where KodModul= '" & strModul3 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PAAmali3 = oCommon.getFieldValue(strSQL)

                strSQL = "Select PATeori from kpmkv_modul Where KodModul= '" & strModul3 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PATeori3 = oCommon.getFieldValue(strSQL)

                strSQL = "Select B_Amali3 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_Amali3 = oCommon.getFieldValue(strSQL)
                'round up
                B_Amali3 = (B_Amali3)

                strSQL = "Select B_Teori3 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_Teori3 = oCommon.getFieldValue(strSQL)
                'round up
                B_Teori3 = (B_Teori3)

                strSQL = "Select A_Amali3 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                A_Amali3 = oCommon.getFieldValue(strSQL)
                'round up
                A_Amali3 = (A_Amali3)

                strSQL = "Select A_Teori3 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                A_Teori3 = oCommon.getFieldValue(strSQL)
                'round up
                A_Teori3 = (A_Teori3)

                'convert 0 if null
                If (String.IsNullOrEmpty(B_Amali3.ToString())) Then
                    B_Amali3 = 0
                End If

                If (String.IsNullOrEmpty(B_Teori3.ToString())) Then
                    B_Teori3 = 0
                End If

                If (String.IsNullOrEmpty(A_Amali3.ToString())) Then
                    A_Amali3 = 0
                End If

                If (String.IsNullOrEmpty(A_Teori3.ToString())) Then
                    A_Teori3 = 0
                End If

                If (B_Amali3 = "-1" Or B_Teori3 = "-1" Or A_Amali3 = "-1" Or A_Teori3 = "-1") Then
                    strSQL = "UPDATE kpmkv_pelajar_markah SET PBPAM3='-1',"
                    strSQL += "PBAV3='-1',PBTV3='-1',PAAV3='-1',PATV3='-1'"
                    strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else

                    PBA3 = ((B_Amali3 / 100) * PBAmali3)
                    PBT3 = ((B_Teori3 / 100) * PBTeori3)
                    PAA3 = ((A_Amali3 / 100) * PAAmali3)
                    PAT3 = ((A_Teori3 / 100) * PATeori3)

                    'change on 31/7/2017 PBAV1,PBTV1,PAAV1,PATV1
                    PointerM3 = Math.Ceiling(PBA3 + PBT3 + PAA3 + PAT3)
                    strSQL = "UPDATE kpmkv_pelajar_markah SET PBPAM3='" & PointerM3 & "',"
                    strSQL += "PBAV3='" & PBA3 & "',PBTV3='" & PBT3 & "',PAAV3='" & PAA3 & "',PATV3='" & PAT3 & "'"
                    strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                End If
            End If
            'Modul3---------------------------------
            If Not String.IsNullOrEmpty(strModul4) Then
                Dim PBAmali4 As Integer
                Dim PBTeori4 As Integer
                Dim PAAmali4 As Integer
                Dim PATeori4 As Integer

                Dim B_Amali4 As Double
                Dim B_Teori4 As Double
                Dim A_Amali4 As Double
                Dim A_Teori4 As Double
                Dim PBA4 As Double
                Dim PBT4 As Double
                Dim PAA4 As Double
                Dim PAT4 As Double
                Dim PointerM4 As Integer

                strSQL = "Select PBAmali from kpmkv_modul Where KodModul= '" & strModul4 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PBAmali4 = oCommon.getFieldValue(strSQL)

                strSQL = "Select PBTeori from kpmkv_modul Where KodModul= '" & strModul4 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PBTeori4 = oCommon.getFieldValue(strSQL)

                strSQL = "Select PAAmali from kpmkv_modul Where KodModul= '" & strModul4 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PAAmali4 = oCommon.getFieldValue(strSQL)

                strSQL = "Select PATeori from kpmkv_modul Where KodModul= '" & strModul4 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PATeori4 = oCommon.getFieldValue(strSQL)

                strSQL = "Select B_Amali4 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_Amali4 = oCommon.getFieldValue(strSQL)
                'round up
                B_Amali4 = (B_Amali4)

                strSQL = "Select B_Teori4 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_Teori4 = oCommon.getFieldValue(strSQL)
                'round up
                B_Teori4 = (B_Teori4)

                strSQL = "Select A_Amali4 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                A_Amali4 = oCommon.getFieldValue(strSQL)
                'round up
                A_Amali4 = (A_Amali4)

                strSQL = "Select A_Teori4 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                A_Teori4 = oCommon.getFieldValue(strSQL)
                'round up
                A_Teori4 = (A_Teori4)

                'convert 0 if null
                If (String.IsNullOrEmpty(B_Amali4.ToString())) Then
                    B_Amali4 = 0
                End If

                If (String.IsNullOrEmpty(B_Teori4.ToString())) Then
                    B_Teori4 = 0
                End If

                If (String.IsNullOrEmpty(A_Amali4.ToString())) Then
                    A_Amali4 = 0
                End If

                If (String.IsNullOrEmpty(A_Teori4.ToString())) Then
                    A_Teori4 = 0
                End If

                If (B_Amali4 = "-1" Or B_Teori4 = "-1" Or A_Amali4 = "-1" Or A_Teori4 = "-1") Then
                    strSQL = "UPDATE kpmkv_pelajar_markah SET PBPAM4='-1',"
                    strSQL += "PBAV4='-1',PBTV4='-1',PAAV4='-1',PATV4='-1'"
                    strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else

                    PBA4 = ((B_Amali4 / 100) * PBAmali4)
                    PBT4 = ((B_Teori4 / 100) * PBTeori4)
                    PAA4 = ((A_Amali4 / 100) * PAAmali4)
                    PAT4 = ((A_Teori4 / 100) * PATeori4)

                    'change on 31/7/2017 PBAV1,PBTV1,PAAV1,PATV1
                    PointerM4 = Math.Ceiling(PBA4 + PBT4 + PAA4 + PAT4)
                    strSQL = "UPDATE kpmkv_pelajar_markah SET PBPAM4='" & PointerM4 & "',"
                    strSQL += "PBAV4='" & PBA4 & "',PBTV4='" & PBT4 & "',PAAV4='" & PAA4 & "',PATV4='" & PAT4 & "'"
                    strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                End If
            End If
            'Modul4---------------------------------
            If Not String.IsNullOrEmpty(strModul5) Then

                Dim PBAmali5 As Integer
                Dim PBTeori5 As Integer
                Dim PAAmali5 As Integer
                Dim PATeori5 As Integer

                Dim B_Amali5 As Double
                Dim B_Teori5 As Double
                Dim A_Amali5 As Double
                Dim A_Teori5 As Double
                Dim PBA5 As Double
                Dim PBT5 As Double
                Dim PAA5 As Double
                Dim PAT5 As Double
                Dim PointerM5 As Integer

                strSQL = "Select PBAmali from kpmkv_modul Where KodModul= '" & strModul5 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PBAmali5 = oCommon.getFieldValue(strSQL)

                strSQL = "Select PBTeori from kpmkv_modul Where KodModul= '" & strModul5 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PBTeori5 = oCommon.getFieldValue(strSQL)

                strSQL = "Select PAAmali from kpmkv_modul Where KodModul= '" & strModul5 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PAAmali5 = oCommon.getFieldValue(strSQL)

                strSQL = "Select PATeori from kpmkv_modul Where KodModul= '" & strModul5 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PATeori5 = oCommon.getFieldValue(strSQL)

                strSQL = "Select B_Amali5 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_Amali5 = oCommon.getFieldValue(strSQL)
                'round up
                B_Amali5 = (B_Amali5)

                strSQL = "Select B_Teori5 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_Teori5 = oCommon.getFieldValue(strSQL)
                'round up
                B_Teori5 = (B_Teori5)

                strSQL = "Select A_Amali5 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                A_Amali5 = oCommon.getFieldValue(strSQL)
                'round up
                A_Amali5 = (A_Amali5)

                strSQL = "Select A_Teori5 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                A_Teori5 = oCommon.getFieldValue(strSQL)
                'round up
                A_Teori5 = (A_Teori5)

                'convert 0 if null
                If (String.IsNullOrEmpty(B_Amali5.ToString())) Then
                    B_Amali5 = 0
                End If

                If (String.IsNullOrEmpty(B_Teori5.ToString())) Then
                    B_Teori5 = 0
                End If

                If (String.IsNullOrEmpty(A_Amali5.ToString())) Then
                    A_Amali5 = 0
                End If

                If (String.IsNullOrEmpty(A_Teori5.ToString())) Then
                    A_Teori5 = 0
                End If

                If (B_Amali5 = "-1" Or B_Teori5 = "-1" Or A_Amali5 = "-1" Or A_Teori5 = "-1") Then
                    strSQL = "UPDATE kpmkv_pelajar_markah SET PBPAM5='-1',"
                    strSQL += "PBAV5='-1',PBTV5='-1',PAAV5='-1',PATV5='-1'"
                    strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else

                    PBA5 = ((B_Amali5 / 100) * PBAmali5)
                    PBT5 = ((B_Teori5 / 100) * PBTeori5)
                    PAA5 = ((A_Amali5 / 100) * PAAmali5)
                    PAT5 = ((A_Teori5 / 100) * PATeori5)

                    'change on 31/7/2017 PBAV1,PBTV1,PAAV1,PATV1
                    PointerM5 = Math.Ceiling(PBA5 + PBT5 + PAA5 + PAT5)
                    strSQL = "UPDATE kpmkv_pelajar_markah SET PBPAM5='" & PointerM5 & "',"
                    strSQL += "PBAV5='" & PBA5 & "',PBTV5='" & PBT5 & "',PAAV5='" & PAA5 & "',PATV5='" & PAT5 & "'"
                    strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                End If
            End If
            'Modul6---------------------------------
            If Not String.IsNullOrEmpty(strModul6) Then

                Dim PBAmali6 As Integer
                Dim PBTeori6 As Integer
                Dim PAAmali6 As Integer
                Dim PATeori6 As Integer

                Dim B_Amali6 As Double
                Dim B_Teori6 As Double
                Dim A_Amali6 As Double
                Dim A_Teori6 As Double
                Dim PBA6 As Double
                Dim PBT6 As Double
                Dim PAA6 As Double
                Dim PAT6 As Double
                Dim PointerM6 As Integer

                strSQL = "Select PBAmali from kpmkv_modul Where KodModul= '" & strModul6 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PBAmali6 = oCommon.getFieldValue(strSQL)

                strSQL = "Select PBTeori from kpmkv_modul Where KodModul= '" & strModul6 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PBTeori6 = oCommon.getFieldValue(strSQL)

                strSQL = "Select PAAmali from kpmkv_modul Where KodModul= '" & strModul6 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PAAmali6 = oCommon.getFieldValue(strSQL)

                strSQL = "Select PATeori from kpmkv_modul Where KodModul= '" & strModul6 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PATeori6 = oCommon.getFieldValue(strSQL)

                strSQL = "Select B_Amali6 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_Amali6 = oCommon.getFieldValue(strSQL)
                'round up
                B_Amali6 = (B_Amali6)

                strSQL = "Select B_Teori6 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_Teori6 = oCommon.getFieldValue(strSQL)
                'round up
                B_Teori6 = (B_Teori6)

                strSQL = "Select A_Amali6 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                A_Amali6 = oCommon.getFieldValue(strSQL)
                'round up
                A_Amali6 = (A_Amali6)

                strSQL = "Select A_Teori6 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                A_Teori6 = oCommon.getFieldValue(strSQL)
                'round up
                A_Teori6 = (A_Teori6)

                'convert 0 if null
                If (String.IsNullOrEmpty(B_Amali6.ToString())) Then
                    B_Amali6 = 0
                End If

                If (String.IsNullOrEmpty(B_Teori6.ToString())) Then
                    B_Teori6 = 0
                End If

                If (String.IsNullOrEmpty(A_Amali6.ToString())) Then
                    A_Amali6 = 0
                End If

                If (String.IsNullOrEmpty(A_Teori6.ToString())) Then
                    A_Teori6 = 0
                End If

                If (B_Amali6 = "-1" Or B_Teori6 = "-1" Or A_Amali6 = "-1" Or A_Teori6 = "-1") Then
                    strSQL = "UPDATE kpmkv_pelajar_markah SET PBPAM6='-1',"
                    strSQL += "PBAV6='-1',PBTV6='-1',PAAV6='-1',PATV6='-1'"
                    strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else

                    PBA6 = ((B_Amali6 / 100) * PBAmali6)
                    PBT6 = ((B_Teori6 / 100) * PBTeori6)
                    PAA6 = ((A_Amali6 / 100) * PAAmali6)
                    PAT6 = ((A_Teori6 / 100) * PATeori6)

                    'change on 31/7/2017 PBAV1,PBTV1,PAAV1,PATV1
                    PointerM6 = Math.Ceiling(PBA6 + PBT6 + PAA6 + PAT6)
                    strSQL = "UPDATE kpmkv_pelajar_markah SET PBPAM6='" & PointerM6 & "',"
                    strSQL += "PBAV6='" & PBA6 & "',PBTV6='" & PBT6 & "',PAAV6='" & PAA6 & "',PATV6='" & PAT6 & "'"
                    strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                End If
            End If
            'Modul7---------------------------------
            If Not String.IsNullOrEmpty(strModul7) Then

                Dim PBAmali7 As Integer
                Dim PBTeori7 As Integer
                Dim PAAmali7 As Integer
                Dim PATeori7 As Integer

                Dim B_Amali7 As Double
                Dim B_Teori7 As Double
                Dim A_Amali7 As Double
                Dim A_Teori7 As Double
                Dim PBA7 As Double
                Dim PBT7 As Double
                Dim PAA7 As Double
                Dim PAT7 As Double
                Dim PointerM7 As Integer

                strSQL = "Select PBAmali from kpmkv_modul Where KodModul= '" & strModul7 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PBAmali7 = oCommon.getFieldValue(strSQL)

                strSQL = "Select PBTeori from kpmkv_modul Where KodModul= '" & strModul7 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PBTeori7 = oCommon.getFieldValue(strSQL)

                strSQL = "Select PAAmali from kpmkv_modul Where KodModul= '" & strModul7 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PAAmali7 = oCommon.getFieldValue(strSQL)

                strSQL = "Select PATeori from kpmkv_modul Where KodModul= '" & strModul7 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PATeori7 = oCommon.getFieldValue(strSQL)

                strSQL = "Select B_Amali7 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_Amali7 = oCommon.getFieldValue(strSQL)
                'round up
                B_Amali7 = (B_Amali7)

                strSQL = "Select B_Teori7 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_Teori7 = oCommon.getFieldValue(strSQL)
                'round up
                B_Teori7 = (B_Teori7)

                strSQL = "Select A_Amali7 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                A_Amali7 = oCommon.getFieldValue(strSQL)
                'round up
                A_Amali7 = (A_Amali7)

                strSQL = "Select A_Teori7 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                A_Teori7 = oCommon.getFieldValue(strSQL)
                'round up
                A_Teori7 = (A_Teori7)

                'convert 0 if null
                If (String.IsNullOrEmpty(B_Amali7.ToString())) Then
                    B_Amali7 = 0
                End If

                If (String.IsNullOrEmpty(B_Teori7.ToString())) Then
                    B_Teori7 = 0
                End If

                If (String.IsNullOrEmpty(A_Amali7.ToString())) Then
                    A_Amali7 = 0
                End If

                If (String.IsNullOrEmpty(A_Teori7.ToString())) Then
                    A_Teori7 = 0
                End If

                If (B_Amali7 = "-1" Or B_Teori7 = "-1" Or A_Amali7 = "-1" Or A_Teori7 = "-1") Then
                    strSQL = "UPDATE kpmkv_pelajar_markah SET PBPAM7='-1',"
                    strSQL += "PBAV7='-1',PBTV7='-1',PAAV7='-1',PATV7='-1'"
                    strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else

                    PBA7 = ((B_Amali7 / 100) * PBAmali7)
                    PBT7 = ((B_Teori7 / 100) * PBTeori7)
                    PAA7 = ((A_Amali7 / 100) * PAAmali7)
                    PAT7 = ((A_Teori7 / 100) * PATeori7)

                    'change on 31/7/2017 PBAV1,PBTV1,PAAV1,PATV1
                    PointerM7 = Math.Ceiling(PBA7 + PBT7 + PAA7 + PAT7)
                    strSQL = "UPDATE kpmkv_pelajar_markah SET PBPAM7='" & PointerM7 & "',"
                    strSQL += "PBAV7='" & PBA7 & "',PBTV7='" & PBT7 & "',PAAV7='" & PAA7 & "',PATV7='" & PAT7 & "'"
                    strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                End If
            End If
            'Modul8---------------------------------
            If Not String.IsNullOrEmpty(strModul8) Then

                Dim PBAmali8 As Integer
                Dim PBTeori8 As Integer
                Dim PAAmali8 As Integer
                Dim PATeori8 As Integer

                Dim B_Amali8 As Double
                Dim B_Teori8 As Double
                Dim A_Amali8 As Double
                Dim A_Teori8 As Double
                Dim PBA8 As Double
                Dim PBT8 As Double
                Dim PAA8 As Double
                Dim PAT8 As Double
                Dim PointerM8 As Integer

                strSQL = "Select PBAmali from kpmkv_modul Where KodModul= '" & strModul8 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PBAmali8 = oCommon.getFieldValue(strSQL)

                strSQL = "Select PBTeori from kpmkv_modul Where KodModul= '" & strModul8 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PBTeori8 = oCommon.getFieldValue(strSQL)

                strSQL = "Select PAAmali from kpmkv_modul Where KodModul= '" & strModul8 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PAAmali8 = oCommon.getFieldValue(strSQL)

                strSQL = "Select PATeori from kpmkv_modul Where KodModul= '" & strModul8 & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
                PATeori8 = oCommon.getFieldValue(strSQL)

                strSQL = "Select B_Amali8 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_Amali8 = oCommon.getFieldValue(strSQL)
                'round up
                B_Amali8 = (B_Amali8)

                strSQL = "Select B_Teori8 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_Teori8 = oCommon.getFieldValue(strSQL)
                'round up
                B_Teori8 = (B_Teori8)

                strSQL = "Select A_Amali8 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                A_Amali8 = oCommon.getFieldValue(strSQL)
                'round up
                A_Amali8 = (A_Amali8)

                strSQL = "Select A_Teori8 from kpmkv_pelajar_markah Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                A_Teori8 = oCommon.getFieldValue(strSQL)
                'round up
                A_Teori8 = (A_Teori8)

                'convert 0 if null
                If (String.IsNullOrEmpty(B_Amali8.ToString())) Then
                    B_Amali8 = 0
                End If

                If (String.IsNullOrEmpty(B_Teori8.ToString())) Then
                    B_Teori8 = 0
                End If

                If (String.IsNullOrEmpty(A_Amali8.ToString())) Then
                    A_Amali8 = 0
                End If

                If (String.IsNullOrEmpty(A_Teori8.ToString())) Then
                    A_Teori8 = 0
                End If

                If (B_Amali8 = "-1" Or B_Teori8 = "-1" Or A_Amali8 = "-1" Or A_Teori8 = "-1") Then
                    strSQL = "UPDATE kpmkv_pelajar_markah SET PBPAM8='-1',"
                    strSQL += "PBAV8='-1',PBTV8='-1',PAAV8='-1',PATV8='-1'"
                    strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else

                    PBA8 = ((B_Amali8 / 100) * PBAmali8)
                    PBT8 = ((B_Teori8 / 100) * PBTeori8)
                    PAA8 = ((A_Amali8 / 100) * PAAmali8)
                    PAT8 = ((A_Teori8 / 100) * PATeori8)

                    'change on 31/8/2018 PBAV1,PBTV1,PAAV1,PATV1
                    PointerM8 = Math.Ceiling(PBA8 + PBT8 + PAA8 + PAT8)
                    strSQL = "UPDATE kpmkv_pelajar_markah SET PBPAM8='" & PointerM8 & "',"
                    strSQL += "PBAV8='" & PBA8 & "',PBTV8='" & PBT8 & "',PAAV8='" & PAA8 & "',PATV8='" & PAT8 & "'"
                    strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                End If
            End If
        Next

    End Sub
    Private Sub Vokasional_gred()
        strRet = BindData(datRespondent)
        For i As Integer = 0 To datRespondent.Rows.Count - 1
            Dim PBPAM1 As String
            Dim GredPBPAM1 As String

            strSQL = "SELECT PBPAM1 FROM kpmkv_pelajar_markah"
            strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            PBPAM1 = oCommon.getFieldValueInt(strSQL)

            If String.IsNullOrEmpty(PBPAM1) Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV1='' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            ElseIf PBPAM1 = 0 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV1='E' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            ElseIf PBPAM1 = -1 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV1='T' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            Else
                strSQL = "SELECT TOP ( 1 ) Gred FROM  kpmkv_gred WHERE '" & PBPAM1 & "' BETWEEN MarkahFrom AND MarkahTo AND Jenis='VOKASIONAL'"
                GredPBPAM1 = oCommon.getFieldValue(strSQL)

                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV1='" & GredPBPAM1 & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

            End If

            '-----------------------------------------------------------------
            Dim PBPAM2 As String
            Dim GredPBPAM2 As String

            strSQL = "SELECT PBPAM2 FROM kpmkv_pelajar_markah"
            strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            PBPAM2 = oCommon.getFieldValueInt(strSQL)

            If String.IsNullOrEmpty(PBPAM2) Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV2='' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            ElseIf PBPAM2 = 0 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV2='E' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            ElseIf PBPAM2 = -1 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV2='T' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            Else
                strSQL = "SELECT TOP ( 1 ) Gred FROM  kpmkv_gred WHERE '" & PBPAM2 & "' BETWEEN MarkahFrom AND MarkahTo AND Jenis='VOKASIONAL'"
                GredPBPAM2 = oCommon.getFieldValue(strSQL)

                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV2='" & GredPBPAM2 & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

            End If
            '------------------------------------------------------------------------------------------------------------------------
            Dim PBPAM3 As String
            Dim GredPBPAM3 As String

            strSQL = "SELECT PBPAM3 FROM kpmkv_pelajar_markah"
            strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            PBPAM3 = oCommon.getFieldValueInt(strSQL)

            If String.IsNullOrEmpty(PBPAM3) Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV3='' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            ElseIf PBPAM3 = 0 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV3='E' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            ElseIf PBPAM3 = -1 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV3='T' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            Else
                strSQL = "SELECT TOP ( 1 ) Gred FROM  kpmkv_gred WHERE '" & PBPAM3 & "' BETWEEN MarkahFrom AND MarkahTo AND Jenis='VOKASIONAL'"
                GredPBPAM3 = oCommon.getFieldValue(strSQL)

                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV3='" & GredPBPAM3 & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

            End If
            '------------------------------------------------------------------------------------------------------------

            Dim PBPAM4 As String
            Dim GredPBPAM4 As String

            strSQL = "SELECT PBPAM4 FROM kpmkv_pelajar_markah"
            strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            PBPAM4 = oCommon.getFieldValueInt(strSQL)

            If String.IsNullOrEmpty(PBPAM4) Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV4='' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            ElseIf PBPAM4 = 0 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV4='E' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            ElseIf PBPAM4 = -1 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV4='T' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            Else
                strSQL = "SELECT TOP ( 1 ) Gred FROM  kpmkv_gred WHERE '" & PBPAM4 & "' BETWEEN MarkahFrom AND MarkahTo AND Jenis='VOKASIONAL'"
                GredPBPAM4 = oCommon.getFieldValue(strSQL)

                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV4='" & GredPBPAM4 & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

            End If
            '-------------------------------------------------------------------------------------------------------------

            Dim PBPAM5 As String
            Dim GredPBPAM5 As String

            strSQL = "SELECT PBPAM5 FROM kpmkv_pelajar_markah"
            strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            PBPAM5 = oCommon.getFieldValueInt(strSQL)

            If String.IsNullOrEmpty(PBPAM5) Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV5='' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            ElseIf PBPAM5 = 0 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV5='E' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            ElseIf PBPAM5 = -1 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV5='T' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            Else
                strSQL = "SELECT TOP ( 1 ) Gred FROM  kpmkv_gred WHERE '" & PBPAM5 & "' BETWEEN MarkahFrom AND MarkahTo AND Jenis='VOKASIONAL'"
                GredPBPAM5 = oCommon.getFieldValue(strSQL)

                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV5='" & GredPBPAM5 & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            '-------------------------------------------------------------------------------------------------------------

            Dim PBPAM6 As String
            Dim GredPBPAM6 As String

            strSQL = "SELECT PBPAM6 FROM kpmkv_pelajar_markah"
            strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
            PBPAM6 = oCommon.getFieldValueInt(strSQL)

            If String.IsNullOrEmpty(PBPAM6) Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV6='' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            ElseIf PBPAM6 = 0 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV6='E' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            ElseIf PBPAM6 = -1 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV6='T' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            Else
                strSQL = "SELECT TOP ( 1 ) Gred FROM  kpmkv_gred WHERE '" & PBPAM6 & "' BETWEEN MarkahFrom AND MarkahTo AND Jenis='VOKASIONAL'"
                GredPBPAM6 = oCommon.getFieldValue(strSQL)

                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV6='" & GredPBPAM6 & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            '-------------------------------------------------------------------------------------------------------------

        Next
    End Sub
    Private Sub Vokasional_gredSMP()
        Try

            '--count no of modul
            Dim nCount As Integer = 0
            strSQL = "SELECT COUNT(kpmkv_modul.KodModul) as CModul "
            strSQL += " FROM kpmkv_modul LEFT OUTER JOIN"
            strSQL += " kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID"
            strSQL += " WHERE kpmkv_modul.KursusID='" & ddlKodKursus.SelectedValue & "'"
            strSQL += " AND  kpmkv_modul.Sesi='" & chkSesi.Text & "'"
            strSQL += " AND  kpmkv_modul.Semester='" & ddlSemester.Text & "'"
            strSQL += " AND  kpmkv_modul.Tahun='" & ddlTahun.Text & "'"
            nCount = oCommon.getFieldValueInt(strSQL)

            strRet = BindData(datRespondent)
            For i As Integer = 0 To datRespondent.Rows.Count - 1
                'PB
                Dim PBAV1 As String
                Dim PBTV1 As String
                Dim PAAV1 As String
                Dim PATV1 As String

                Dim PBAV2 As String
                Dim PBTV2 As String
                Dim PAAV2 As String
                Dim PATV2 As String

                Dim PBAV3 As String
                Dim PBTV3 As String
                Dim PAAV3 As String
                Dim PATV3 As String

                Dim PBAV4 As String
                Dim PBTV4 As String
                Dim PAAV4 As String
                Dim PATV4 As String

                Dim PBAV5 As String
                Dim PBTV5 As String
                Dim PAAV5 As String
                Dim PATV5 As String

                Dim PBAV6 As String
                Dim PBTV6 As String
                Dim PAAV6 As String
                Dim PATV6 As String

                Dim PBAV7 As String
                Dim PBTV7 As String
                Dim PAAV7 As String
                Dim PATV7 As String

                'Dim PBAV8 As String
                'Dim PBTV8 As String
                'Dim PAAV8 As String
                'Dim PATV8 As String

                Dim SMP_PB As Double
                Dim SMP_PA As Double

                Dim SMP_Total As Double

                Select Case nCount
                    Case "2"
                        strSQL = "SELECT PBAV1,PBTV1,PAAV1,PATV1,PBAV2,PBTV2,PAAV2,PATV2 FROM kpmkv_pelajar_markah"
                        strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        'Response.Write(strSQL)
                        ''--get total
                        Dim ar_total As Array
                        ar_total = strRet.Split("|")
                        PBAV1 = ar_total(0)
                        PBTV1 = ar_total(1)
                        PAAV1 = ar_total(2)
                        PATV1 = ar_total(3)
                        PBAV2 = ar_total(4)
                        PBTV2 = ar_total(5)
                        PAAV2 = ar_total(6)
                        PATV2 = ar_total(7)

                        If ((PBAV1) = -1 Or (PBTV1) = -1 Or (PAAV1) = -1 Or (PATV1) = -1 Or (PBAV2) = -1 Or (PBTV2) = -1 Or (PAAV2) = -1 Or (PATV2) = -1) Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_Total='-1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        Else
                            If String.IsNullOrEmpty(PBAV1) Then PBAV1 = 0
                            If String.IsNullOrEmpty(PBTV1) Then PBTV1 = 0
                            If String.IsNullOrEmpty(PAAV1) Then PAAV1 = 0
                            If String.IsNullOrEmpty(PATV1) Then PATV1 = 0
                            If String.IsNullOrEmpty(PBAV2) Then PBAV2 = 0
                            If String.IsNullOrEmpty(PBTV2) Then PBTV2 = 0
                            If String.IsNullOrEmpty(PAAV2) Then PAAV2 = 0
                            If String.IsNullOrEmpty(PATV2) Then PATV2 = 0

                            SMP_PA = (Convert.ToDouble(PAAV1) + CDbl(PATV1) + CDbl(PAAV2) + CDbl(PATV2)) / 2
                            SMP_PB = (CDbl(PBAV1) + CDbl(PBTV1) + CDbl(PBAV2) + CDbl(PBTV2)) / 2
                            SMP_Total = Math.Ceiling(SMP_PB + SMP_PA)

                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_Total='" & SMP_Total & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        End If

                    Case "3"
                        strSQL = "SELECT PBAV1,PBTV1,PAAV1,PATV1,PBAV2,PBTV2,PAAV2,PATV2,PBAV3,PBTV3,PAAV3,PATV3 FROM kpmkv_pelajar_markah"
                        strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        'Response.Write(strSQL)
                        ''--get total
                        Dim ar_total As Array
                        ar_total = strRet.Split("|")
                        PBAV1 = ar_total(0)
                        PBTV1 = ar_total(1)
                        PAAV1 = ar_total(2)
                        PATV1 = ar_total(3)
                        PBAV2 = ar_total(4)
                        PBTV2 = ar_total(5)
                        PAAV2 = ar_total(6)
                        PATV2 = ar_total(7)
                        PBAV3 = ar_total(8)
                        PBTV3 = ar_total(9)
                        PAAV3 = ar_total(10)
                        PATV3 = ar_total(11)


                        If ((PBAV1) = -1 Or (PBTV1) = -1 Or (PAAV1) = -1 Or (PATV1) = -1 Or (PBAV2) = -1 Or (PBTV2) = -1 Or (PAAV2) = -1 Or (PATV2) = -1 Or (PBAV3) = -1 Or (PBTV3) = -1 Or (PAAV3) = -1 Or (PATV3) = -1) Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_Total='-1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        Else

                            If String.IsNullOrEmpty(PBAV1) Then PBAV1 = 0
                            If String.IsNullOrEmpty(PBTV1) Then PBTV1 = 0
                            If String.IsNullOrEmpty(PAAV1) Then PAAV1 = 0
                            If String.IsNullOrEmpty(PATV1) Then PATV1 = 0
                            If String.IsNullOrEmpty(PBAV2) Then PBAV2 = 0
                            If String.IsNullOrEmpty(PBTV2) Then PBTV2 = 0
                            If String.IsNullOrEmpty(PAAV2) Then PAAV2 = 0
                            If String.IsNullOrEmpty(PATV2) Then PATV2 = 0
                            If String.IsNullOrEmpty(PBAV3) Then PBAV3 = 0
                            If String.IsNullOrEmpty(PBTV3) Then PBTV3 = 0
                            If String.IsNullOrEmpty(PAAV3) Then PAAV3 = 0
                            If String.IsNullOrEmpty(PATV3) Then PATV3 = 0

                            SMP_PA = (CDbl(PAAV1) + CDbl(PATV1) + CDbl(PAAV2) + CDbl(PATV2) + CDbl(PAAV3) + CDbl(PATV3)) / 3
                            SMP_PB = (CDbl(PBAV1) + CDbl(PBTV1) + CDbl(PBAV2) + CDbl(PBTV2) + CDbl(PBAV3) + CDbl(PBTV3)) / 3
                            SMP_Total = Math.Ceiling(SMP_PB + SMP_PA)

                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_Total='" & SMP_Total & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        End If

                    Case "4"
                        strSQL = "SELECT PBAV1,PBTV1,PAAV1,PATV1,PBAV2,PBTV2,PAAV2,PATV2,PBAV3,PBTV3,PAAV3,PATV3,PBAV4,PBTV4,PAAV4,PATV4 FROM kpmkv_pelajar_markah"
                        strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        'Response.Write(strSQL)
                        ''--get total
                        Dim ar_total As Array
                        ar_total = strRet.Split("|")
                        PBAV1 = ar_total(0)
                        PBTV1 = ar_total(1)
                        PAAV1 = ar_total(2)
                        PATV1 = ar_total(3)
                        PBAV2 = ar_total(4)
                        PBTV2 = ar_total(5)
                        PAAV2 = ar_total(6)
                        PATV2 = ar_total(7)
                        PBAV3 = ar_total(8)
                        PBTV3 = ar_total(9)
                        PAAV3 = ar_total(10)
                        PATV3 = ar_total(11)
                        PBAV4 = ar_total(12)
                        PBTV4 = ar_total(13)
                        PAAV4 = ar_total(14)
                        PATV4 = ar_total(15)


                        If ((PBAV1) = -1 Or (PBTV1) = -1 Or (PAAV1) = -1 Or (PATV1) = -1 Or (PBAV2) = -1 Or (PBTV2) = -1 Or (PAAV2) = -1 Or (PATV2) = -1 Or (PBAV3) = -1 Or (PBTV3) = -1 Or (PAAV3) = -1 Or (PATV3) = -1 Or (PBAV4) = -1 Or (PBTV4) = -1 Or (PAAV4) = -1 Or (PATV4) = -1) Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_Total='-1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        Else
                            If String.IsNullOrEmpty(PBAV1) Then PBAV1 = 0
                            If String.IsNullOrEmpty(PBTV1) Then PBTV1 = 0
                            If String.IsNullOrEmpty(PAAV1) Then PAAV1 = 0
                            If String.IsNullOrEmpty(PATV1) Then PATV1 = 0
                            If String.IsNullOrEmpty(PBAV2) Then PBAV2 = 0
                            If String.IsNullOrEmpty(PBTV2) Then PBTV2 = 0
                            If String.IsNullOrEmpty(PAAV2) Then PAAV2 = 0
                            If String.IsNullOrEmpty(PATV2) Then PATV2 = 0
                            If String.IsNullOrEmpty(PBAV3) Then PBAV3 = 0
                            If String.IsNullOrEmpty(PBTV3) Then PBTV3 = 0
                            If String.IsNullOrEmpty(PAAV3) Then PAAV3 = 0
                            If String.IsNullOrEmpty(PATV3) Then PATV3 = 0
                            If String.IsNullOrEmpty(PBAV4) Then PBAV4 = 0
                            If String.IsNullOrEmpty(PBTV4) Then PBTV4 = 0
                            If String.IsNullOrEmpty(PAAV4) Then PAAV4 = 0
                            If String.IsNullOrEmpty(PATV4) Then PATV4 = 0

                            SMP_PA = (Convert.ToDouble(PAAV1) + Convert.ToDouble(PATV1) + Convert.ToDouble(PAAV2) + Convert.ToDouble(PATV2) + Convert.ToDouble(PAAV3) + Convert.ToDouble(PATV3) + Convert.ToDouble(PAAV4) + Convert.ToDouble(PATV4)) / 4
                            SMP_PB = (Convert.ToDouble(PBAV1) + Convert.ToDouble(PBTV1) + Convert.ToDouble(PBAV2) + Convert.ToDouble(PBTV2) + Convert.ToDouble(PBAV3) + Convert.ToDouble(PBTV3) + Convert.ToDouble(PBAV4) + Convert.ToDouble(PBTV4)) / 4
                            SMP_Total = Math.Ceiling(SMP_PB + SMP_PA)

                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_Total='" & SMP_Total & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        End If

                    Case "5"
                        strSQL = "SELECT PBAV1,PBTV1,PAAV1,PATV1,PBAV2,PBTV2,PAAV2,PATV2,PBAV3,PBTV3,PAAV3,PATV3,PBAV4,PBTV4,PAAV4,PATV4,PBAV5,PBTV5,PAAV5,PATV5 FROM kpmkv_pelajar_markah"
                        strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        'Response.Write(strSQL)
                        ''--get total
                        Dim ar_total As Array
                        ar_total = strRet.Split("|")
                        PBAV1 = ar_total(0)
                        PBTV1 = ar_total(1)
                        PAAV1 = ar_total(2)
                        PATV1 = ar_total(3)
                        PBAV2 = ar_total(4)
                        PBTV2 = ar_total(5)
                        PAAV2 = ar_total(6)
                        PATV2 = ar_total(7)
                        PBAV3 = ar_total(8)
                        PBTV3 = ar_total(9)
                        PAAV3 = ar_total(10)
                        PATV3 = ar_total(11)
                        PBAV4 = ar_total(12)
                        PBTV4 = ar_total(13)
                        PAAV4 = ar_total(14)
                        PATV4 = ar_total(15)
                        PBAV5 = ar_total(16)
                        PBTV5 = ar_total(17)
                        PAAV5 = ar_total(18)
                        PATV5 = ar_total(19)


                        If ((PBAV1) = -1 Or (PBTV1) = -1 Or (PAAV1) = -1 Or (PATV1) = -1 Or (PBAV2) = -1 Or (PBTV2) = -1 Or (PAAV2) = -1 Or (PATV2) = -1 Or (PBAV3) = -1 Or (PBTV3) = -1 Or (PAAV3) = -1 Or (PATV3) = -1 Or (PBAV4) = -1 Or (PBTV4) = -1 Or (PAAV4) = -1 Or (PATV4) = -1 Or (PBAV5) = -1 Or (PBTV5) = -1 Or (PAAV5) = -1 Or (PATV5) = -1) Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_Total='-1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        Else
                            If String.IsNullOrEmpty(PBAV1) Then PBAV1 = 0
                            If String.IsNullOrEmpty(PBTV1) Then PBTV1 = 0
                            If String.IsNullOrEmpty(PAAV1) Then PAAV1 = 0
                            If String.IsNullOrEmpty(PATV1) Then PATV1 = 0
                            If String.IsNullOrEmpty(PBAV2) Then PBAV2 = 0
                            If String.IsNullOrEmpty(PBTV2) Then PBTV2 = 0
                            If String.IsNullOrEmpty(PAAV2) Then PAAV2 = 0
                            If String.IsNullOrEmpty(PATV2) Then PATV2 = 0
                            If String.IsNullOrEmpty(PBAV3) Then PBAV3 = 0
                            If String.IsNullOrEmpty(PBTV3) Then PBTV3 = 0
                            If String.IsNullOrEmpty(PAAV3) Then PAAV3 = 0
                            If String.IsNullOrEmpty(PATV3) Then PATV3 = 0
                            If String.IsNullOrEmpty(PBAV4) Then PBAV4 = 0
                            If String.IsNullOrEmpty(PBTV4) Then PBTV4 = 0
                            If String.IsNullOrEmpty(PAAV4) Then PAAV4 = 0
                            If String.IsNullOrEmpty(PATV4) Then PATV4 = 0
                            If String.IsNullOrEmpty(PBAV5) Then PBAV5 = 0
                            If String.IsNullOrEmpty(PBTV5) Then PBTV5 = 0
                            If String.IsNullOrEmpty(PAAV5) Then PAAV5 = 0
                            If String.IsNullOrEmpty(PATV5) Then PATV5 = 0

                            SMP_PA = (CDbl(PAAV1) + CDbl(PATV1) + CDbl(PAAV2) + CDbl(PATV2) + CDbl(PAAV3) + CDbl(PATV3) + CDbl(PAAV4) + CDbl(PATV4) + CDbl(PAAV5) + CDbl(PATV5)) / 5
                            SMP_PB = (CDbl(PBAV1) + CDbl(PBTV1) + CDbl(PBAV2) + CDbl(PBTV2) + CDbl(PBAV3) + CDbl(PBTV3) + CDbl(PBAV4) + CDbl(PBTV4) + CDbl(PBAV5) + CDbl(PBTV5) / 5)
                            SMP_Total = Math.Ceiling(SMP_PB + SMP_PA)

                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_Total='" & SMP_Total & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        End If

                    Case "6"
                        strSQL = "SELECT PBAV1,PBTV1,PAAV1,PATV1,PBAV2,PBTV2,PAAV2,PATV2,PBAV3,PBTV3,PAAV3,PATV3,PBAV4,PBTV4,PAAV4,PATV4,PBAV5,PBTV5,PAAV5,PATV5,PBAV6,PBTV6,PAAV6,PATV6 FROM kpmkv_pelajar_markah"
                        strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        'Response.Write(strSQL)
                        ''--get total
                        Dim ar_total As Array
                        ar_total = strRet.Split("|")
                        PBAV1 = ar_total(0)
                        PBTV1 = ar_total(1)
                        PAAV1 = ar_total(2)
                        PATV1 = ar_total(3)
                        PBAV2 = ar_total(4)
                        PBTV2 = ar_total(5)
                        PAAV2 = ar_total(6)
                        PATV2 = ar_total(7)
                        PBAV3 = ar_total(8)
                        PBTV3 = ar_total(9)
                        PAAV3 = ar_total(10)
                        PATV3 = ar_total(11)
                        PBAV4 = ar_total(12)
                        PBTV4 = ar_total(13)
                        PAAV4 = ar_total(14)
                        PATV4 = ar_total(15)
                        PBAV5 = ar_total(16)
                        PBTV5 = ar_total(17)
                        PAAV5 = ar_total(18)
                        PATV5 = ar_total(19)
                        PBAV6 = ar_total(20)
                        PBTV6 = ar_total(21)
                        PAAV6 = ar_total(22)
                        PATV6 = ar_total(23)


                        If ((PBAV1) = -1 Or (PBTV1) = -1 Or (PAAV1) = -1 Or (PATV1) = -1 Or (PBAV2) = -1 Or (PBTV2) = -1 Or (PAAV2) = -1 Or (PATV2) = -1 Or (PBAV3) = -1 Or (PBTV3) = -1 Or (PAAV3) = -1 Or (PATV3) = -1 Or (PBAV4) = -1 Or (PBTV4) = -1 Or (PAAV4) = -1 Or (PATV4) = -1 Or (PBAV5) = -1 Or (PBTV5) = -1 Or (PAAV5) = -1 Or (PATV5) = -1 Or (PBAV6) = -1 Or (PBTV6) = -1 Or (PAAV6) = -1 Or (PATV6) = -1) Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_Total='-1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        Else
                            If String.IsNullOrEmpty(PBAV1) Then PBAV1 = 0
                            If String.IsNullOrEmpty(PBTV1) Then PBTV1 = 0
                            If String.IsNullOrEmpty(PAAV1) Then PAAV1 = 0
                            If String.IsNullOrEmpty(PATV1) Then PATV1 = 0
                            If String.IsNullOrEmpty(PBAV2) Then PBAV2 = 0
                            If String.IsNullOrEmpty(PBTV2) Then PBTV2 = 0
                            If String.IsNullOrEmpty(PAAV2) Then PAAV2 = 0
                            If String.IsNullOrEmpty(PATV2) Then PATV2 = 0
                            If String.IsNullOrEmpty(PBAV3) Then PBAV3 = 0
                            If String.IsNullOrEmpty(PBTV3) Then PBTV3 = 0
                            If String.IsNullOrEmpty(PAAV3) Then PAAV3 = 0
                            If String.IsNullOrEmpty(PATV3) Then PATV3 = 0
                            If String.IsNullOrEmpty(PBAV4) Then PBAV4 = 0
                            If String.IsNullOrEmpty(PBTV4) Then PBTV4 = 0
                            If String.IsNullOrEmpty(PAAV4) Then PAAV4 = 0
                            If String.IsNullOrEmpty(PATV4) Then PATV4 = 0
                            If String.IsNullOrEmpty(PBAV5) Then PBAV5 = 0
                            If String.IsNullOrEmpty(PBTV5) Then PBTV5 = 0
                            If String.IsNullOrEmpty(PAAV5) Then PAAV5 = 0
                            If String.IsNullOrEmpty(PATV5) Then PATV5 = 0
                            If String.IsNullOrEmpty(PBAV6) Then PBAV6 = 0
                            If String.IsNullOrEmpty(PBTV6) Then PBTV6 = 0
                            If String.IsNullOrEmpty(PAAV6) Then PAAV6 = 0
                            If String.IsNullOrEmpty(PATV6) Then PATV6 = 0


                            SMP_PA = (CDbl(PAAV1) + CDbl(PATV1) + CDbl(PAAV2) + CDbl(PATV2) + CDbl(PAAV3) + CDbl(PATV3) + CDbl(PAAV4) + CDbl(PATV4) + CDbl(PAAV5) + CDbl(PATV5) + CDbl(PAAV6) + CDbl(PATV6)) / 6
                            SMP_PB = (CDbl(PBAV1) + CDbl(PBTV1) + CDbl(PBAV2) + CDbl(PBTV2) + CDbl(PBAV3) + CDbl(PBTV3) + CDbl(PBAV4) + CDbl(PBTV4) + CDbl(PBAV5) + CDbl(PBTV5) + CDbl(PBAV6) + CDbl(PBTV6)) / 6
                            SMP_Total = Math.Ceiling(SMP_PB + SMP_PA)

                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_Total='" & SMP_Total & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        End If

                    Case "7"
                        strSQL = "SELECT PBAV1,PBTV1,PAAV1,PATV1,PBAV2,PBTV2,PAAV2,PATV2,PBAV3,PBTV3,PAAV3,PATV3,PBAV4,PBTV4,PAAV4,PATV4,PBAV5,PBTV5,PAAV5,PATV5,PBAV6,PBTV6,PAAV6,PATV6,PBAV7,PBTV7,PAAV7,PATV7 FROM kpmkv_pelajar_markah"
                        strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        'Response.Write(strSQL)
                        ''--get total
                        Dim ar_total As Array
                        ar_total = strRet.Split("|")
                        PBAV1 = ar_total(0)
                        PBTV1 = ar_total(1)
                        PAAV1 = ar_total(2)
                        PATV1 = ar_total(3)
                        PBAV2 = ar_total(4)
                        PBTV2 = ar_total(5)
                        PAAV2 = ar_total(6)
                        PATV2 = ar_total(7)
                        PBAV3 = ar_total(8)
                        PBTV3 = ar_total(9)
                        PAAV3 = ar_total(10)
                        PATV3 = ar_total(11)
                        PBAV4 = ar_total(12)
                        PBTV4 = ar_total(13)
                        PAAV4 = ar_total(14)
                        PATV4 = ar_total(15)
                        PBAV5 = ar_total(16)
                        PBTV5 = ar_total(17)
                        PAAV5 = ar_total(18)
                        PATV5 = ar_total(19)
                        PBAV6 = ar_total(20)
                        PBTV6 = ar_total(21)
                        PAAV6 = ar_total(22)
                        PATV6 = ar_total(23)
                        PBAV7 = ar_total(24)
                        PBTV7 = ar_total(25)
                        PAAV7 = ar_total(26)
                        PATV7 = ar_total(27)


                        If ((PBAV1) = -1 Or (PBTV1) = -1 Or (PAAV1) = -1 Or (PATV1) = -1 Or (PBAV2) = -1 Or (PBTV2) = -1 Or (PAAV2) = -1 Or (PATV2) = -1 Or (PBAV3) = -1 Or (PBTV3) = -1 Or (PAAV3) = -1 Or (PATV3) = -1 Or (PBAV4) = -1 Or (PBTV4) = -1 Or (PAAV4) = -1 Or (PATV4) = -1 Or (PBAV5) = -1 Or (PBTV5) = -1 Or (PAAV5) = -1 Or (PATV5) = -1 Or (PBAV6) = -1 Or (PBTV6) = -1 Or (PAAV6) = -1 Or (PATV6) = -1 Or (PBAV7) = -1 Or (PBTV7) = -1 Or (PAAV7) = -1 Or (PATV7) = -1) Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_Total='-1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        Else
                            If String.IsNullOrEmpty(PBAV1) Then PBAV1 = 0
                            If String.IsNullOrEmpty(PBTV1) Then PBTV1 = 0
                            If String.IsNullOrEmpty(PAAV1) Then PAAV1 = 0
                            If String.IsNullOrEmpty(PATV1) Then PATV1 = 0
                            If String.IsNullOrEmpty(PBAV2) Then PBAV2 = 0
                            If String.IsNullOrEmpty(PBTV2) Then PBTV2 = 0
                            If String.IsNullOrEmpty(PAAV2) Then PAAV2 = 0
                            If String.IsNullOrEmpty(PATV2) Then PATV2 = 0
                            If String.IsNullOrEmpty(PBAV3) Then PBAV3 = 0
                            If String.IsNullOrEmpty(PBTV3) Then PBTV3 = 0
                            If String.IsNullOrEmpty(PAAV3) Then PAAV3 = 0
                            If String.IsNullOrEmpty(PATV3) Then PATV3 = 0
                            If String.IsNullOrEmpty(PBAV4) Then PBAV4 = 0
                            If String.IsNullOrEmpty(PBTV4) Then PBTV4 = 0
                            If String.IsNullOrEmpty(PAAV4) Then PAAV4 = 0
                            If String.IsNullOrEmpty(PATV4) Then PATV4 = 0
                            If String.IsNullOrEmpty(PBAV5) Then PBAV5 = 0
                            If String.IsNullOrEmpty(PBTV5) Then PBTV5 = 0
                            If String.IsNullOrEmpty(PAAV5) Then PAAV5 = 0
                            If String.IsNullOrEmpty(PATV5) Then PATV5 = 0
                            If String.IsNullOrEmpty(PBAV6) Then PBAV6 = 0
                            If String.IsNullOrEmpty(PBTV6) Then PBTV6 = 0
                            If String.IsNullOrEmpty(PAAV6) Then PAAV6 = 0
                            If String.IsNullOrEmpty(PATV6) Then PATV6 = 0
                            If String.IsNullOrEmpty(PBAV7) Then PBAV7 = 0
                            If String.IsNullOrEmpty(PBTV7) Then PBTV7 = 0
                            If String.IsNullOrEmpty(PAAV7) Then PAAV7 = 0
                            If String.IsNullOrEmpty(PATV7) Then PATV7 = 0

                            SMP_PA = (CDbl(PAAV1) + CDbl(PATV1) + CDbl(PAAV2) + CDbl(PATV2) + CDbl(PAAV3) + CDbl(PATV3) + CDbl(PAAV4) + CDbl(PATV4) + CDbl(PAAV5) + CDbl(PATV5) + CDbl(PAAV6) + CDbl(PATV6) + CDbl(PAAV7) + CDbl(PATV7)) / 7
                            SMP_PB = (CDbl(PBAV1) + CDbl(PBTV1) + CDbl(PBAV2) + CDbl(PBTV2) + CDbl(PBAV3) + CDbl(PBTV3) + CDbl(PBAV4) + CDbl(PBTV4) + CDbl(PBAV5) + CDbl(PBTV5) + CDbl(PBAV6) + CDbl(PBTV6) + CDbl(PBAV7) + CDbl(PBTV7)) / 7
                            SMP_Total = Math.Ceiling(SMP_PB + SMP_PA)

                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_Total='" & SMP_Total & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        End If

                        'Case "8"
                        '    strSQL = "SELECT PBAV1,PBTV1,PAAV1,PATV1,PBAV2,PBTV2,PAAV2,PATV2,PBAV3,PBTV3,PAAV3,PATV3,PBAV4,PBTV4,PAAV4,PATV4,PBAV5,PBTV5,PAAV5,PATV5,PBAV6,PBTV6,PAAV6,PATV6,PBAV7,PBTV7,PAAV7,PATV7,PBAV8,PBTV8,PAAV8,PATV8 FROM kpmkv_pelajar_markah"
                        '    strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        '    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        '    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        '    strRet = oCommon.getFieldValueEx(strSQL)
                        '    'Response.Write(strSQL)
                        '    ''--get total
                        '    Dim ar_total As Array
                        '    ar_total = strRet.Split("|")
                        '    PBAV1 = ar_total(0)
                        '    PBTV1 = ar_total(1)
                        '    PAAV1 = ar_total(2)
                        '    PATV1 = ar_total(3)
                        '    PBAV2 = ar_total(4)
                        '    PBTV2 = ar_total(5)
                        '    PAAV2 = ar_total(6)
                        '    PATV2 = ar_total(7)
                        '    PBAV3 = ar_total(8)
                        '    PBTV3 = ar_total(9)
                        '    PAAV3 = ar_total(10)
                        '    PATV3 = ar_total(11)
                        '    PBAV4 = ar_total(12)
                        '    PBTV4 = ar_total(13)
                        '    PAAV4 = ar_total(14)
                        '    PATV4 = ar_total(15)
                        '    PBAV5 = ar_total(16)
                        '    PBTV5 = ar_total(17)
                        '    PAAV5 = ar_total(18)
                        '    PATV5 = ar_total(19)
                        '    PBAV6 = ar_total(20)
                        '    PBTV6 = ar_total(21)
                        '    PAAV6 = ar_total(22)
                        '    PATV6 = ar_total(23)
                        '    PBAV7 = ar_total(24)
                        '    PBTV7 = ar_total(25)
                        '    PAAV7 = ar_total(26)
                        '    PATV7 = ar_total(27)
                        '    PBAV8 = ar_total(28)
                        '    PBTV8 = ar_total(29)
                        '    PAAV8 = ar_total(30)
                        '    PATV8 = ar_total(31)


                        '    If String.IsNullOrEmpty(PBAV1) Then PBAV1 = 0
                        '    If String.IsNullOrEmpty(PBTV1) Then PBTV1 = 0
                        '    If String.IsNullOrEmpty(PAAV1) Then PAAV1 = 0
                        '    If String.IsNullOrEmpty(PATV1) Then PATV1 = 0
                        '    If String.IsNullOrEmpty(PBAV2) Then PBAV2 = 0
                        '    If String.IsNullOrEmpty(PBTV2) Then PBTV2 = 0
                        '    If String.IsNullOrEmpty(PAAV2) Then PAAV2 = 0
                        '    If String.IsNullOrEmpty(PATV2) Then PATV2 = 0
                        '    If String.IsNullOrEmpty(PBAV3) Then PBAV3 = 0
                        '    If String.IsNullOrEmpty(PBTV3) Then PBTV3 = 0
                        '    If String.IsNullOrEmpty(PAAV3) Then PAAV3 = 0
                        '    If String.IsNullOrEmpty(PATV3) Then PATV3 = 0
                        '    If String.IsNullOrEmpty(PBAV4) Then PBAV4 = 0
                        '    If String.IsNullOrEmpty(PBTV4) Then PBTV4 = 0
                        '    If String.IsNullOrEmpty(PAAV4) Then PAAV4 = 0
                        '    If String.IsNullOrEmpty(PATV4) Then PATV4 = 0
                        '    If String.IsNullOrEmpty(PBAV5) Then PBAV5 = 0
                        '    If String.IsNullOrEmpty(PBTV5) Then PBTV5 = 0
                        '    If String.IsNullOrEmpty(PAAV5) Then PAAV5 = 0
                        '    If String.IsNullOrEmpty(PATV5) Then PATV5 = 0
                        '    If String.IsNullOrEmpty(PBAV6) Then PBAV6 = 0
                        '    If String.IsNullOrEmpty(PBTV6) Then PBTV6 = 0
                        '    If String.IsNullOrEmpty(PAAV6) Then PAAV6 = 0
                        '    If String.IsNullOrEmpty(PATV6) Then PATV6 = 0
                        '    If String.IsNullOrEmpty(PBAV7) Then PBAV7 = 0
                        '    If String.IsNullOrEmpty(PBTV7) Then PBTV7 = 0
                        '    If String.IsNullOrEmpty(PAAV7) Then PAAV7 = 0
                        '    If String.IsNullOrEmpty(PATV7) Then PATV7 = 0
                        '    If String.IsNullOrEmpty(PBAV8) Then PBAV8 = 0
                        '    If String.IsNullOrEmpty(PBTV8) Then PBTV8 = 0
                        '    If String.IsNullOrEmpty(PAAV8) Then PAAV8 = 0
                        '    If String.IsNullOrEmpty(PATV8) Then PATV8 = 0

                        '    If (PBAV1) = -1 Or (PBTV1) = -1 Or (PAAV1) = -1 Or (PATV1) = -1 Or (PBAV2) = -1 Or (PBTV2) = -1 Or (PAAV2) = -1 Or (PATV2) = -1 Or (PBAV3) = -1 Or (PBTV3) = -1 Or (PAAV3) = -1 Or (PATV3) = -1 Or (PBAV4) = -1 Or (PBTV4) = -1 Or (PAAV4) = -1 Or (PATV4) = -1 Or (PBAV5) = -1 Or (PBTV5) = -1 Or (PAAV5) = -1 Or (PATV5) = -1 Or (PBAV6) = -1 Or (PBTV6) = -1 Or (PAAV6) = -1 Or (PATV6) = -1 Or (PBAV7) = -1 Or (PBTV7) = -1 Or (PAAV7) = -1 Or (PATV7) = -1 Or (PBAV8) = -1 Or (PBTV8) = -1 Or (PAAV8) = -1 Or (PATV8) = -1 Then
                        '        strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_Total='-1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        '        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        '        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        '        strRet = oCommon.ExecuteSQL(strSQL)
                        '    Else
                        '        SMP_PB = ((PBAV1 + PBTV1 + PBAV2 + PBTV2 + PBAV3 + PBTV3 + PBAV4 + PBTV4 + PBAV5 + PBTV5 + PBAV6 + PBTV6 + PBAV7 + PBTV7 + PBAV8 + PBTV8) / 8)
                        '        SMP_PA = ((PAAV1 + PATV1 + PAAV2 + PATV2 + PAAV3 + PATV3 + PAAV4 + PATV4 + PAAV5 + PATV5 + PAAV6 + PATV6 + PAAV7 + PATV7 + PAAV8 + PATV8) / 8)
                        '        SMP_Total = Math.Ceiling(SMP_PB + SMP_PA)

                        '        strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_Total='" & SMP_Total & "' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        '        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        '        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        '        strRet = oCommon.ExecuteSQL(strSQL)
                        '    End If

                End Select
            Next
        Catch ex As Exception
            lblMsg.Text = "System Error.Gred:" & ex.Message
        End Try

    End Sub
    Private Sub Vokasional_SMP_PB()
        Try

            '--count no of modul
            Dim nCount As Integer = 0
            strSQL = "SELECT COUNT(kpmkv_modul.KodModul) as CModul "
            strSQL += " FROM kpmkv_modul LEFT OUTER JOIN"
            strSQL += " kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID"
            strSQL += " WHERE kpmkv_modul.KursusID='" & ddlKodKursus.SelectedValue & "'"
            strSQL += " AND  kpmkv_modul.Sesi='" & chkSesi.Text & "'"
            strSQL += " AND  kpmkv_modul.Semester='" & ddlSemester.Text & "'"
            strSQL += " AND  kpmkv_modul.Tahun='" & ddlTahun.Text & "'"
            nCount = oCommon.getFieldValueInt(strSQL)
            'PB
            Dim PBA1 As String
            Dim PBA2 As String
            Dim PBA3 As String
            Dim PBA4 As String
            Dim PBA5 As String
            Dim PBA6 As String
            Dim PBA7 As String
            Dim PBA8 As String
            'PB
            Dim PBT1 As String
            Dim PBT2 As String
            Dim PBT3 As String
            Dim PBT4 As String
            Dim PBT5 As String
            Dim PBT6 As String
            Dim PBT7 As String
            Dim PBT8 As String

            Dim PB_Amali As Double
            Dim PB_Amali_K As Double
            Dim PB_Teori As Double
            Dim PB_Teori_K As Double
            Dim SMP_PB As Double

            Dim Skor As String

            'get score SMP_PB
            strSQL = "SELECT SMP_PB FROM kpmkv_skor_svm"
            strSQL += " WHERE Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
            Skor = oCommon.getFieldValue(strSQL)

            strRet = BindData(datRespondent)
            For i As Integer = 0 To datRespondent.Rows.Count - 1


                'PBAV1,PBTV1
                'PBAV2,PBTV2
                Select Case nCount
                    Case "2"
                        strSQL = "SELECT PBAV1,PBAV2,PBTV1,PBTV2 FROM kpmkv_pelajar_markah"
                        strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        'Response.Write(strSQL)
                        ''--get total
                        Dim ar_total As Array
                        ar_total = strRet.Split("|")
                        PBA1 = ar_total(0)
                        PBA2 = ar_total(1)
                        PBT1 = ar_total(2)
                        PBT2 = ar_total(3)

                        'check pb
                        If ((PBA1) = -1 Or (PBA2) = -1 Or (PBT1) = -1 Or (PBT2) = -1) Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PB='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        Else
                            PB_Amali = CDbl(PBA1) + CDbl(PBA2)
                            PB_Teori = CDbl(PBT1) + CDbl(PBT2)

                            PB_Amali_K = (PB_Amali / 2)
                            PB_Teori_K = (PB_Teori / 2)

                            SMP_PB = (PB_Amali_K + PB_Teori_K)

                            If SMP_PB >= CDbl(Skor) Then
                                strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PB='1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                                strRet = oCommon.ExecuteSQL(strSQL)
                            Else
                                strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PB='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                                strRet = oCommon.ExecuteSQL(strSQL)
                            End If
                        End If

                    Case "3"
                        strSQL = "SELECT PBAV1,PBAV2,PBAV3,PBTV1,PBTV2,PBTV3 FROM kpmkv_pelajar_markah"
                        strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        'Response.Write(strSQL)
                        ''--get total
                        Dim ar_total As Array
                        ar_total = strRet.Split("|")
                        PBA1 = ar_total(0)
                        PBA2 = ar_total(1)
                        PBA3 = ar_total(2)
                        PBT1 = ar_total(3)
                        PBT2 = ar_total(4)
                        PBT3 = ar_total(5)

                        If ((PBA1) = -1 Or (PBA2) = -1 Or (PBA3) = -1 Or (PBT1) = -1 Or (PBT2) = -1 Or (PBT3) = -1) Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PB='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        Else

                            PB_Amali = CDbl(PBA1) + CDbl(PBA2) + CDbl(PBA3)
                            PB_Teori = CDbl(PBT1) + CDbl(PBT2) + CDbl(PBT3)

                            PB_Amali_K = (PB_Amali / 3)
                            PB_Teori_K = (PB_Teori / 3)

                            SMP_PB = (PB_Amali_K + PB_Teori_K)

                            If SMP_PB >= CDbl(Skor) Then
                                strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PB='1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                                strRet = oCommon.ExecuteSQL(strSQL)
                            Else
                                strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PB='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                                strRet = oCommon.ExecuteSQL(strSQL)
                            End If
                        End If
                    Case "4"
                        strSQL = "SELECT PBAV1,PBAV2,PBAV3,PBAV4,PBTV1,PBTV2,PBTV3,PBTV4 FROM kpmkv_pelajar_markah"
                        strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        'Response.Write(strSQL)
                        ''--get total
                        Dim ar_total As Array
                        ar_total = strRet.Split("|")
                        PBA1 = ar_total(0)
                        PBA2 = ar_total(1)
                        PBA3 = ar_total(2)
                        PBA4 = ar_total(3)
                        PBT1 = ar_total(4)
                        PBT2 = ar_total(5)
                        PBT3 = ar_total(6)
                        PBT4 = ar_total(7)

                        If ((PBA1) = -1 Or (PBA2) = -1 Or (PBA3) = -1 Or (PBA4) = -1 Or (PBT1) = -1 Or (PBT2) = -1 Or (PBT3) = -1 Or (PBT4) = -1) Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PB='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        Else

                            PB_Amali = CDbl(PBA1) + CDbl(PBA2) + CDbl(PBA3) + CDbl(PBA4)
                            PB_Teori = CDbl(PBT1) + CDbl(PBT2) + CDbl(PBT3) + CDbl(PBT4)

                            PB_Amali_K = (PB_Amali / 4)
                            PB_Teori_K = (PB_Teori / 4)

                            SMP_PB = (PB_Amali_K + PB_Teori_K)

                            If SMP_PB >= CDbl(Skor) Then
                                strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PB='1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                                strRet = oCommon.ExecuteSQL(strSQL)
                            Else
                                strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PB='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                                strRet = oCommon.ExecuteSQL(strSQL)
                            End If

                        End If
                    Case "5"
                        strSQL = "SELECT PBAV1,PBAV2,PBAV3,PBAV4,PBAV5,PBTV1,PBTV2,PBTV3,PBTV4,PBTV5 FROM kpmkv_pelajar_markah"
                        strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        'Response.Write(strSQL)
                        ''--get total
                        Dim ar_total As Array
                        ar_total = strRet.Split("|")
                        PBA1 = ar_total(0)
                        PBA2 = ar_total(1)
                        PBA3 = ar_total(2)
                        PBA4 = ar_total(3)
                        PBA5 = ar_total(4)
                        PBT1 = ar_total(5)
                        PBT2 = ar_total(6)
                        PBT3 = ar_total(7)
                        PBT4 = ar_total(8)
                        PBT5 = ar_total(9)

                        If ((PBA1) = -1 Or (PBA2) = -1 Or (PBA3) = -1 Or (PBA4) = -1 Or (PBA5) = -1 Or (PBT1) = -1 Or (PBT2) = -1 Or (PBT3) = -1 Or (PBT4) = -1 Or (PBT5) = -1) Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PB='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        Else

                            PB_Amali = CDbl(PBA1) + CDbl(PBA2) + CDbl(PBA3) + CDbl(PBA4) + CDbl(PBA5)
                            PB_Teori = CDbl(PBT1) + CDbl(PBT2) + CDbl(PBT3) + CDbl(PBT4) + CDbl(PBT5)

                            PB_Amali_K = (PB_Amali / 5)
                            PB_Teori_K = (PB_Teori / 5)

                            SMP_PB = (PB_Amali_K + PB_Teori_K)

                            If SMP_PB >= CDbl(Skor) Then
                                strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PB='1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                                strRet = oCommon.ExecuteSQL(strSQL)
                            Else
                                strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PB='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                                strRet = oCommon.ExecuteSQL(strSQL)
                            End If
                        End If
                    Case "6"
                        strSQL = "SELECT PBAV1,PBAV2,PBAV3,PBAV4,PBAV5,PBAV6,PBTV1,PBTV2,PBTV3,PBTV4,PBTV5,PBTV6 FROM kpmkv_pelajar_markah"
                        strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        'Response.Write(strSQL)
                        ''--get total
                        Dim ar_total As Array
                        ar_total = strRet.Split("|")
                        PBA1 = ar_total(0)
                        PBA2 = ar_total(1)
                        PBA3 = ar_total(2)
                        PBA4 = ar_total(3)
                        PBA5 = ar_total(4)
                        PBA6 = ar_total(5)
                        PBT1 = ar_total(6)
                        PBT2 = ar_total(7)
                        PBT3 = ar_total(8)
                        PBT4 = ar_total(9)
                        PBT5 = ar_total(10)
                        PBT6 = ar_total(11)

                        If ((PBA1) = -1 Or (PBA2) = -1 Or (PBA3) = -1 Or (PBA4) = -1 Or (PBA5) = -1 Or (PBA6) = -1 Or (PBT1) = -1 Or (PBT2) = -1 Or (PBT3) = -1 Or (PBT4) = -1 Or (PBT5) = -1 Or (PBT6) = -1) Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PB='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        Else

                            PB_Amali = CDbl(PBA1) + CDbl(PBA2) + CDbl(PBA3) + CDbl(PBA4) + CDbl(PBA5) + CDbl(PBA6)
                            PB_Teori = CDbl(PBT1) + CDbl(PBT2) + CDbl(PBT3) + CDbl(PBT4) + CDbl(PBT5) + CDbl(PBT6)

                            PB_Amali_K = (PB_Amali / 6)
                            PB_Teori_K = (PB_Teori / 6)

                            SMP_PB = (PB_Amali_K + PB_Teori_K)

                            If SMP_PB >= CDbl(Skor) Then
                                strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PB='1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                                strRet = oCommon.ExecuteSQL(strSQL)
                            Else
                                strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PB='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                                strRet = oCommon.ExecuteSQL(strSQL)
                            End If
                        End If
                    Case "7"
                        strSQL = "SELECT PBAV1,PBAV2,PBAV3,PBAV4,PBAV5,PBAV6,PBAV7,PBTV1,PBTV2,PBTV3,PBTV4,PBTV5,PBTV6,PBTV7 FROM kpmkv_pelajar_markah"
                        strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        'Response.Write(strSQL)
                        ''--get total
                        Dim ar_total As Array
                        ar_total = strRet.Split("|")
                        PBA1 = ar_total(0)
                        PBA2 = ar_total(1)
                        PBA3 = ar_total(2)
                        PBA4 = ar_total(3)
                        PBA5 = ar_total(4)
                        PBA6 = ar_total(5)
                        PBA7 = ar_total(6)
                        PBT1 = ar_total(7)
                        PBT2 = ar_total(8)
                        PBT3 = ar_total(9)
                        PBT4 = ar_total(10)
                        PBT5 = ar_total(11)
                        PBT6 = ar_total(12)
                        PBT7 = ar_total(13)

                        If ((PBA1) = -1 Or (PBA2) = -1 Or (PBA3) = -1 Or (PBA4) = -1 Or (PBA5) = -1 Or (PBA6) = -1 Or (PBA7) = -1 Or (PBT1) = -1 Or (PBT2) = -1 Or (PBT3) = -1 Or (PBT4) = -1 Or (PBT5) = -1 Or (PBT6) = -1 Or (PBT7) = -1) Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PB='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        Else

                            PB_Amali = CDbl(PBA1) + CDbl(PBA2) + CDbl(PBA3) + CDbl(PBA4) + CDbl(PBA5) + CDbl(PBA6) + CDbl(PBA7)
                            PB_Teori = CDbl(PBT1) + CDbl(PBT2) + CDbl(PBT3) + CDbl(PBT4) + CDbl(PBT5) + CDbl(PBT6) + CDbl(PBT7)

                            PB_Amali_K = (PB_Amali / 7)
                            PB_Teori_K = (PB_Teori / 7)

                            SMP_PB = (PB_Amali_K + PB_Teori_K)

                            If SMP_PB >= CDbl(Skor) Then
                                strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PB='1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                                strRet = oCommon.ExecuteSQL(strSQL)
                            Else
                                strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PB='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                                strRet = oCommon.ExecuteSQL(strSQL)
                            End If
                        End If

                    Case "8"
                        strSQL = "SELECT PBAV1,PBAV2,PBAV3,PBAV4,PBAV5,PBAV6,PBAV7,PBAV8,PBTV1,PBTV2,PBTV3,PBTV4,PBTV5,PBTV6,PBTV7,PBTV8 FROM kpmkv_pelajar_markah"
                        strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)
                        'Response.Write(strSQL)
                        ''--get total
                        Dim ar_total As Array
                        ar_total = strRet.Split("|")
                        PBA1 = ar_total(0)
                        PBA2 = ar_total(1)
                        PBA3 = ar_total(2)
                        PBA4 = ar_total(3)
                        PBA5 = ar_total(4)
                        PBA6 = ar_total(5)
                        PBA7 = ar_total(6)
                        PBA8 = ar_total(7)
                        PBT1 = ar_total(8)
                        PBT2 = ar_total(9)
                        PBT3 = ar_total(10)
                        PBT4 = ar_total(11)
                        PBT5 = ar_total(12)
                        PBT6 = ar_total(13)
                        PBT7 = ar_total(14)
                        PBT8 = ar_total(15)

                        If ((PBA1) = -1 Or (PBA2) = -1 Or (PBA3) = -1 Or (PBA4) = -1 Or (PBA5) = -1 Or (PBA6) = -1 Or (PBA7) = -1 Or (PBA8) = -1 Or (PBT1) = -1 Or (PBT2) = -1 Or (PBT3) = -1 Or (PBT4) = -1 Or (PBT5) = -1 Or (PBT6) = -1 Or (PBT7) = -1 Or (PBT8) = -1) Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PB='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        Else

                            PB_Amali = CDbl(PBA1) + CDbl(PBA2) + CDbl(PBA3) + CDbl(PBA4) + CDbl(PBA5) + CDbl(PBA6) + CDbl(PBA7) + CDbl(PBA8)
                            PB_Teori = CDbl(PBT1) + CDbl(PBT2) + CDbl(PBT3) + CDbl(PBT4) + CDbl(PBT5) + CDbl(PBT6) + CDbl(PBT7) + CDbl(PBT8)

                            PB_Amali_K = (PB_Amali / 8)
                            PB_Teori_K = (PB_Teori / 8)

                            SMP_PB = (PB_Amali_K + PB_Teori_K)

                            If SMP_PB >= CDbl(Skor) Then
                                strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PB='1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                                strRet = oCommon.ExecuteSQL(strSQL)
                            Else
                                strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PB='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                                strRet = oCommon.ExecuteSQL(strSQL)
                            End If
                        End If

                End Select
            Next

        Catch ex As Exception
            lblMsg.Text = "System Error.PB:" & ex.Message
        End Try

    End Sub
    Private Sub Vokasional_SMP_PA()
        Try
            'get score SMP_PAA
            strSQL = "SELECT SMP_PAA FROM kpmkv_skor_svm"
            strSQL += " WHERE Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
            Dim Skor As String = oCommon.getFieldValue(strSQL)

            'get score SMP_PAA
            strSQL = "SELECT SMP_PAT FROM kpmkv_skor_svm"
            strSQL += " WHERE Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
            Dim SkorT As String = oCommon.getFieldValue(strSQL)

            'PA A
            Dim PAA As String
            'PA T
            Dim PAT As String

            strRet = BindData(datRespondent)
            For i As Integer = 0 To datRespondent.Rows.Count - 1

                strSQL = "SELECT PAAV1,PATV1 FROM kpmkv_pelajar_markah"
                strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strRet = oCommon.getFieldValueEx(strSQL)
                'Response.Write(strSQL)
                ''--get total
                Dim ar_total As Array
                ar_total = strRet.Split("|")
                PAA = ar_total(0)
                PAT = ar_total(1)

                'check pa
                If PAA = -1 Then
                    strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PAA='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    If Not PAA = "" Then
                        If CDbl(PAA) >= CDbl(Skor) Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PAA='1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        Else
                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PAA='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        End If
                    End If

                End If

                If PAT = -1 Then
                    strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PAT='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    If Not PAT = "" Then
                        If CDbl(PAT) >= CDbl(SkorT) Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PAT='1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        Else
                            strSQL = "UPDATE kpmkv_pelajar_markah SET SMP_PAT='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        End If
                    End If
                End If

            Next

        Catch ex As Exception
            lblMsg.Text = "System Error.PA:" & ex.Message
        End Try

    End Sub


    Private Sub Vokasional_gredKompeten()

        Try

            '--count no of modul
            Dim nCount As Integer = 0
            strSQL = "SELECT COUNT(kpmkv_modul.KodModul) as CModul "
            strSQL += " FROM kpmkv_modul LEFT OUTER JOIN"
            strSQL += " kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID"
            strSQL += " WHERE kpmkv_modul.KursusID='" & ddlKodKursus.SelectedValue & "'"
            strSQL += " AND  kpmkv_modul.Sesi='" & chkSesi.Text & "'"
            strSQL += " AND  kpmkv_modul.Semester='" & ddlSemester.Text & "'"
            strSQL += " AND  kpmkv_modul.Tahun='" & ddlTahun.Text & "'"
            nCount = oCommon.getFieldValueInt(strSQL)

            strRet = BindData(datRespondent)
            For i As Integer = 0 To datRespondent.Rows.Count - 1
                Dim strGredV1 As String
                Dim strGredV2 As String
                Dim strGredV3 As String
                Dim strGredV4 As String
                Dim strGredV5 As String
                Dim strGredV6 As String
                Dim strGredV7 As String
                Dim strGredV8 As String

                Select Case nCount
                    Case "2"
                        strSQL = "SELECT GredV1,GredV2 FROM kpmkv_pelajar_markah"
                        strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_total As Array
                        ar_total = strRet.Split("|")
                        strGredV1 = ar_total(0)
                        strGredV2 = ar_total(1)


                        If (strGredV1 = "C+" Or strGredV1 = "C" Or strGredV1 = "C-" Or strGredV1 = "D+" Or strGredV1 = "D-" Or strGredV1 = "E" Or strGredV1 = "G" Or strGredV1 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV2 = "C+" Or strGredV2 = "C" Or strGredV2 = "C-" Or strGredV2 = "D+" Or strGredV2 = "D-" Or strGredV2 = "E" Or strGredV2 = "G" Or strGredV2 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        Else

                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        End If

                    Case "3"

                        strSQL = "SELECT GredV1,GredV2,GredV3 FROM kpmkv_pelajar_markah"
                        strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_total As Array
                        ar_total = strRet.Split("|")
                        strGredV1 = ar_total(0)
                        strGredV2 = ar_total(1)
                        strGredV3 = ar_total(2)


                        If (strGredV1 = "C+" Or strGredV1 = "C" Or strGredV1 = "C-" Or strGredV1 = "D+" Or strGredV1 = "D-" Or strGredV1 = "E" Or strGredV1 = "G" Or strGredV1 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV2 = "C+" Or strGredV2 = "C" Or strGredV2 = "C-" Or strGredV2 = "D+" Or strGredV2 = "D-" Or strGredV2 = "E" Or strGredV2 = "G" Or strGredV2 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV3 = "C+" Or strGredV3 = "C" Or strGredV3 = "C-" Or strGredV3 = "D+" Or strGredV3 = "D-" Or strGredV3 = "E" Or strGredV3 = "G" Or strGredV3 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        Else

                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        End If

                    Case "4"

                        strSQL = "SELECT GredV1,GredV2,GredV3,GredV4 FROM kpmkv_pelajar_markah"
                        strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_total As Array
                        ar_total = strRet.Split("|")
                        strGredV1 = ar_total(0)
                        strGredV2 = ar_total(1)
                        strGredV3 = ar_total(2)
                        strGredV4 = ar_total(3)


                        If (strGredV1 = "C+" Or strGredV1 = "C" Or strGredV1 = "C-" Or strGredV1 = "D+" Or strGredV1 = "D-" Or strGredV1 = "E" Or strGredV1 = "G" Or strGredV1 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV2 = "C+" Or strGredV2 = "C" Or strGredV2 = "C-" Or strGredV2 = "D+" Or strGredV2 = "D-" Or strGredV2 = "E" Or strGredV2 = "G" Or strGredV2 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV3 = "C+" Or strGredV3 = "C" Or strGredV3 = "C-" Or strGredV3 = "D+" Or strGredV3 = "D-" Or strGredV3 = "E" Or strGredV3 = "G" Or strGredV3 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV4 = "C+" Or strGredV4 = "C" Or strGredV4 = "C-" Or strGredV4 = "D+" Or strGredV4 = "D-" Or strGredV4 = "E" Or strGredV4 = "G" Or strGredV4 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        Else

                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        End If


                    Case "5"
                        strSQL = "SELECT GredV1,GredV2,GredV3,GredV4,GredV5 FROM kpmkv_pelajar_markah"
                        strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_total As Array
                        ar_total = strRet.Split("|")
                        strGredV1 = ar_total(0)
                        strGredV2 = ar_total(1)
                        strGredV3 = ar_total(2)
                        strGredV4 = ar_total(3)
                        strGredV5 = ar_total(4)


                        If (strGredV1 = "C+" Or strGredV1 = "C" Or strGredV1 = "C-" Or strGredV1 = "D+" Or strGredV1 = "D-" Or strGredV1 = "E" Or strGredV1 = "G" Or strGredV1 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV2 = "C+" Or strGredV2 = "C" Or strGredV2 = "C-" Or strGredV2 = "D+" Or strGredV2 = "D-" Or strGredV2 = "E" Or strGredV2 = "G" Or strGredV2 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV3 = "C+" Or strGredV3 = "C" Or strGredV3 = "C-" Or strGredV3 = "D+" Or strGredV3 = "D-" Or strGredV3 = "E" Or strGredV3 = "G" Or strGredV3 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV4 = "C+" Or strGredV4 = "C" Or strGredV4 = "C-" Or strGredV4 = "D+" Or strGredV4 = "D-" Or strGredV4 = "E" Or strGredV4 = "G" Or strGredV4 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV5 = "C+" Or strGredV5 = "C" Or strGredV5 = "C-" Or strGredV5 = "D+" Or strGredV5 = "D-" Or strGredV5 = "E" Or strGredV5 = "G" Or strGredV5 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        Else
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        End If


                    Case "6"

                        strSQL = "SELECT GredV1,GredV2,GredV3,GredV4,GredV5,GredV6 FROM kpmkv_pelajar_markah"
                        strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_total As Array
                        ar_total = strRet.Split("|")
                        strGredV1 = ar_total(0)
                        strGredV2 = ar_total(1)
                        strGredV3 = ar_total(2)
                        strGredV4 = ar_total(3)
                        strGredV5 = ar_total(4)
                        strGredV6 = ar_total(5)


                        If (strGredV1 = "C+" Or strGredV1 = "C" Or strGredV1 = "C-" Or strGredV1 = "D+" Or strGredV1 = "D-" Or strGredV1 = "E" Or strGredV1 = "G" Or strGredV1 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV2 = "C+" Or strGredV2 = "C" Or strGredV2 = "C-" Or strGredV2 = "D+" Or strGredV2 = "D-" Or strGredV2 = "E" Or strGredV2 = "G" Or strGredV2 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV3 = "C+" Or strGredV3 = "C" Or strGredV3 = "C-" Or strGredV3 = "D+" Or strGredV3 = "D-" Or strGredV3 = "E" Or strGredV3 = "G" Or strGredV3 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV4 = "C+" Or strGredV4 = "C" Or strGredV4 = "C-" Or strGredV4 = "D+" Or strGredV4 = "D-" Or strGredV4 = "E" Or strGredV4 = "G" Or strGredV4 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV5 = "C+" Or strGredV5 = "C" Or strGredV5 = "C-" Or strGredV5 = "D+" Or strGredV5 = "D-" Or strGredV5 = "E" Or strGredV5 = "G" Or strGredV5 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV6 = "C+" Or strGredV6 = "C" Or strGredV6 = "C-" Or strGredV6 = "D+" Or strGredV6 = "D-" Or strGredV6 = "E" Or strGredV6 = "G" Or strGredV6 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        Else
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        End If

                    Case "7"

                        strSQL = "SELECT GredV1,GredV2,GredV3,GredV4,GredV5,GredV6,GredV7 FROM kpmkv_pelajar_markah"
                        strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_total As Array
                        ar_total = strRet.Split("|")
                        strGredV1 = ar_total(0)
                        strGredV2 = ar_total(1)
                        strGredV3 = ar_total(2)
                        strGredV4 = ar_total(3)
                        strGredV5 = ar_total(4)
                        strGredV6 = ar_total(5)
                        strGredV7 = ar_total(6)

                        If (strGredV1 = "C+" Or strGredV1 = "C" Or strGredV1 = "C-" Or strGredV1 = "D+" Or strGredV1 = "D-" Or strGredV1 = "E" Or strGredV1 = "G" Or strGredV1 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV2 = "C+" Or strGredV2 = "C" Or strGredV2 = "C-" Or strGredV2 = "D+" Or strGredV2 = "D-" Or strGredV2 = "E" Or strGredV2 = "G" Or strGredV2 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV3 = "C+" Or strGredV3 = "C" Or strGredV3 = "C-" Or strGredV3 = "D+" Or strGredV3 = "D-" Or strGredV3 = "E" Or strGredV3 = "G" Or strGredV3 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV4 = "C+" Or strGredV4 = "C" Or strGredV4 = "C-" Or strGredV4 = "D+" Or strGredV4 = "D-" Or strGredV4 = "E" Or strGredV4 = "G" Or strGredV4 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV5 = "C+" Or strGredV5 = "C" Or strGredV5 = "C-" Or strGredV5 = "D+" Or strGredV5 = "D-" Or strGredV5 = "E" Or strGredV5 = "G" Or strGredV5 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV6 = "C+" Or strGredV6 = "C" Or strGredV6 = "C-" Or strGredV6 = "D+" Or strGredV6 = "D-" Or strGredV6 = "E" Or strGredV6 = "G" Or strGredV6 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV7 = "C+" Or strGredV7 = "C" Or strGredV7 = "C-" Or strGredV7 = "D+" Or strGredV7 = "D-" Or strGredV7 = "E" Or strGredV7 = "G" Or strGredV7 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        Else
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        End If


                    Case "8"
                        strSQL = "SELECT GredV1,GredV2,GredV3,GredV4,GredV5,GredV6,GredV7,GredV8 FROM kpmkv_pelajar_markah"
                        strSQL += " Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                        strRet = oCommon.getFieldValueEx(strSQL)

                        Dim ar_total As Array
                        ar_total = strRet.Split("|")
                        strGredV1 = ar_total(0)
                        strGredV2 = ar_total(1)
                        strGredV3 = ar_total(2)
                        strGredV4 = ar_total(3)
                        strGredV5 = ar_total(4)
                        strGredV6 = ar_total(5)
                        strGredV7 = ar_total(6)
                        strGredV8 = ar_total(7)

                        If (strGredV1 = "C+" Or strGredV1 = "C" Or strGredV1 = "C-" Or strGredV1 = "D+" Or strGredV1 = "D-" Or strGredV1 = "E" Or strGredV1 = "G" Or strGredV1 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV2 = "C+" Or strGredV2 = "C" Or strGredV2 = "C-" Or strGredV2 = "D+" Or strGredV2 = "D-" Or strGredV2 = "E" Or strGredV2 = "G" Or strGredV2 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV3 = "C+" Or strGredV3 = "C" Or strGredV3 = "C-" Or strGredV3 = "D+" Or strGredV3 = "D-" Or strGredV3 = "E" Or strGredV3 = "G" Or strGredV3 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV4 = "C+" Or strGredV4 = "C" Or strGredV4 = "C-" Or strGredV4 = "D+" Or strGredV4 = "D-" Or strGredV4 = "E" Or strGredV4 = "G" Or strGredV4 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV5 = "C+" Or strGredV5 = "C" Or strGredV5 = "C-" Or strGredV5 = "D+" Or strGredV5 = "D-" Or strGredV5 = "E" Or strGredV5 = "G" Or strGredV5 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV6 = "C+" Or strGredV6 = "C" Or strGredV6 = "C-" Or strGredV6 = "D+" Or strGredV6 = "D-" Or strGredV6 = "E" Or strGredV6 = "G" Or strGredV6 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV7 = "C+" Or strGredV7 = "C" Or strGredV7 = "C-" Or strGredV7 = "D+" Or strGredV7 = "D-" Or strGredV7 = "E" Or strGredV7 = "G" Or strGredV7 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        ElseIf (strGredV8 = "C+" Or strGredV8 = "C" Or strGredV8 = "C-" Or strGredV8 = "D+" Or strGredV8 = "D-" Or strGredV8 = "E" Or strGredV8 = "G" Or strGredV8 = "T") Then
                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='0' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)

                        Else

                            strSQL = "UPDATE kpmkv_pelajar_markah SET Gred_Kompeten='1' Where PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                            strRet = oCommon.ExecuteSQL(strSQL)
                        End If



                End Select
            Next
        Catch ex As Exception
            lblMsg.Text = "System Error.Gred:" & ex.Message
        End Try

    End Sub
    Protected Sub btnCari_Click(sender As Object, e As EventArgs) Handles btnCari.Click
        lblMsg.Text = ""
        datRespondent.Columns.Item("5").Visible = True

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

        Vokasional_markah()
        Vokasional_gred()
        Vokasional_SMP_PB()
        Vokasional_SMP_PA()
        Vokasional_gredKompeten()
        Vokasional_gredSMP()

        If Not strRet = "0" Then
            divMsgResult.Attributes("class") = "error"
            lblMsgResult.Text = "Tidak Berjaya mengemaskini gred Pentaksiran Akhir Vokasional"
        Else
            divMsgResult.Attributes("class") = "info"
            lblMsgResult.Text = "Berjaya mengemaskini gred Pentaksiran Akhir Vokasional"
            strRet = BindData((datRespondent))
            ' hiddencolumn()
        End If
    End Sub

    'Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
    '    Try
    '        ExportToCSV(getSQLEks)

    '    Catch ex As Exception
    '        lblMsg.Text = "Error:" & ex.Message
    '    End Try
    'End Sub
    'Private Sub ExportToCSV(ByVal strQuery As String)
    '    'Get the data from database into datatable 
    '    Dim cmd As New SqlCommand(strQuery)
    '    Dim dt As DataTable = GetData(cmd)

    '    Response.Clear()
    '    Response.Buffer = True
    '    Response.AddHeader("content-disposition", "attachment;filename=KOKO_File.csv")
    '    Response.Charset = ""
    '    Response.ContentType = "application/text"


    '    Dim sb As New StringBuilder()
    '    For k As Integer = 0 To dt.Columns.Count - 1
    '        'add separator 
    '        sb.Append(dt.Columns(k).ColumnName + ","c)
    '    Next

    '    'append new line 
    '    sb.Append(vbCr & vbLf)
    '    For i As Integer = 0 To dt.Rows.Count - 1
    '        For k As Integer = 0 To dt.Columns.Count - 1
    '            '--add separator 
    '            'sb.Append(dt.Rows(i)(k).ToString().Replace(",", ";") + ","c)

    '            'cleanup here
    '            If k <> 0 Then
    '                sb.Append(",")
    '            End If

    '            Dim columnValue As Object = dt.Rows(i)(k).ToString()
    '            If columnValue Is Nothing Then
    '                sb.Append("")
    '            Else
    '                Dim columnStringValue As String = columnValue.ToString()

    '                Dim cleanedColumnValue As String = CleanCSVString(columnStringValue)

    '                If columnValue.[GetType]() Is GetType(String) AndAlso Not columnStringValue.Contains(",") Then
    '                    ' Prevents a number stored in a string from being shown as 8888E+24 in Excel. Example use is the AccountNum field in CI that looks like a number but is really a string.
    '                    cleanedColumnValue = "=" & cleanedColumnValue
    '                End If
    '                sb.Append(cleanedColumnValue)
    '            End If

    '        Next
    '        'append new line 
    '        sb.Append(vbCr & vbLf)
    '    Next
    '    Response.Output.Write(sb.ToString())
    '    Response.Flush()
    '    Response.End()

    'End Sub

    'Protected Function CleanCSVString(ByVal input As String) As String
    '    Dim output As String = """" & input.Replace("""", """""").Replace(vbCr & vbLf, " ").Replace(vbCr, " ").Replace(vbLf, "") & """"
    '    Return output

    'End Function

    Protected Sub btnSah_Click(sender As Object, e As EventArgs) Handles btnSah.Click
        lblMsg.Text = ""
        If ValidateForm() = False Then
            divMsg.Attributes("class") = "error"
            lblMsg.Text = "Sila Semak Data Markah.[Huruf],[Kosong] adalah tidak dibenarkan!  Markah mestilah diantara -1-100 sahaja"
            divMsgResult.Attributes("class") = "error"
            lblMsgResult.Text = "Sila Semak Data Markah.[Huruf],[Kosong] adalah tidak dibenarkan!  Markah mestilah diantara -1-100 sahaja"


            Exit Sub
        End If

        '--count no of modul
        Dim nCount As Integer = 0
        strSQL = "SELECT COUNT(kpmkv_modul.KodModul) as CModul "
        strSQL += " FROM kpmkv_modul LEFT OUTER JOIN"
        strSQL += " kpmkv_kursus ON kpmkv_modul.KursusID = kpmkv_kursus.KursusID"
        strSQL += " WHERE kpmkv_modul.KursusID='" & ddlKodKursus.SelectedValue & "'"
        strSQL += " AND  kpmkv_modul.Sesi='" & chkSesi.Text & "'"
        strSQL += " AND  kpmkv_modul.Semester='" & ddlSemester.Text & "'"
        strSQL += " AND  kpmkv_modul.Tahun='" & ddlTahun.Text & "'"
        nCount = oCommon.getFieldValueInt(strSQL)

        For i As Integer = 0 To datRespondent.Rows.Count - 1
            Dim row As GridViewRow = datRespondent.Rows(i)
            Dim strAmali1 As TextBox = datRespondent.Rows(i).FindControl("txtAmali1")


            'assign value to integer
            Dim Amali1 As String = strAmali1.Text

            If Amali1 = "" Then
                divMsgResult.Attributes("class") = "error"
                lblMsgResult.Text = "Semua markah perlu diisi sebelum pengesahan markah dilakukan"
                Exit Sub
            End If


            If nCount = 2 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET  A_Amali1='" & Amali1 & "', A_Amali2='" & Amali1 & "',"
                strSQL += " isSahPAV='1' , isSahPAV_Date=GETDATE()"
                strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' AND PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            ElseIf nCount = 3 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET  A_Amali1='" & Amali1 & "', A_Amali2='" & Amali1 & "',"
                strSQL += " A_Amali3='" & Amali1 & "',"
                strSQL += " isSahPAV='1' , isSahPAV_Date=GETDATE()"
                strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' AND PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            ElseIf nCount = 4 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET  A_Amali1='" & Amali1 & "', A_Amali2='" & Amali1 & "',"
                strSQL += " A_Amali3='" & Amali1 & "', A_Amali4='" & Amali1 & "',"
                strSQL += " isSahPAV='1' , isSahPAV_Date=GETDATE()"
                strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' AND PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            ElseIf nCount = 5 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET  A_Amali1='" & Amali1 & "', A_Amali2='" & Amali1 & "',"
                strSQL += " A_Amali3='" & Amali1 & "', A_Amali4='" & Amali1 & "',"
                strSQL += " A_Amali5='" & Amali1 & "',"
                strSQL += " isSahPAV='1' , isSahPAV_Date=GETDATE()"
                strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' AND PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            ElseIf nCount = 6 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET  A_Amali1='" & Amali1 & "', A_Amali2='" & Amali1 & "',"
                strSQL += " A_Amali3='" & Amali1 & "', A_Amali4='" & Amali1 & "',"
                strSQL += " A_Amali5='" & Amali1 & "', A_Amali6='" & Amali1 & "',"
                strSQL += " isSahPAV='1' , isSahPAV_Date=GETDATE()"
                strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' AND PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            ElseIf nCount = 7 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET  A_Amali1='" & Amali1 & "', A_Amali2='" & Amali1 & "',"
                strSQL += " A_Amali3='" & Amali1 & "', A_Amali4='" & Amali1 & "',"
                strSQL += " A_Amali5='" & Amali1 & "', A_Amali6='" & Amali1 & "',"
                strSQL += " A_Amali7='" & Amali1 & "',"
                strSQL += " isSahPAV='1' , isSahPAV_Date=GETDATE()"
                strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' AND PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            ElseIf nCount = 8 Then
                strSQL = "UPDATE kpmkv_pelajar_markah SET  A_Amali1='" & Amali1 & "', A_Amali2='" & Amali1 & "',"
                strSQL += " A_Amali3='" & Amali1 & "', A_Amali4='" & Amali1 & "',"
                strSQL += " A_Amali5='" & Amali1 & "', A_Amali6='" & Amali1 & "',"
                strSQL += " A_Amali7='" & Amali1 & "', A_Amali8='" & Amali1 & "',"
                strSQL += " isSahPAV='1' , isSahPAV_Date=GETDATE()"
                strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' AND PelajarID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            End If
            strRet = oCommon.ExecuteSQL(strSQL)
            If strRet = "0" Then
                divMsgResult.Attributes("class") = "info"
                lblMsgResult.Text = "Pengesahan kemasukan markah berjaya dikemaskini"
            Else
                divMsgResult.Attributes("class") = "error"
                lblMsgResult.Text = "Pengesahan kemasukan markah tidak berjaya dikemaskini"
            End If
        Next

        getDateSah()
        strRet = BindData(datRespondent)

    End Sub

    Private Sub getDateSah()
        strSQL = "SELECT MAX(isSahPAV_Date) FROM kpmkv_pelajar_markah "
        strSQL += " WHERE KolejRecordID='" & lblKolejID.Text & "' "
        strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
        strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
        strSQL += " AND isSahPAV='1'"
        strSQL += " GROUP BY isSahPAV_Date"
        strSQL += " ORDER BY isSahPAV_Date  DESC"

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
        Dim myDocument As New Document(PageSize.A4)

        Try
            HttpContext.Current.Response.ContentType = "application/pdf"
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=pa amali.pdf")
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)

            PdfWriter.GetInstance(myDocument, HttpContext.Current.Response.OutputStream)

            myDocument.Open()

            ''--draw spacing
            Dim imgdrawSpacing As String = Server.MapPath("~/img/spacing.png")
            Dim imgSpacing As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(imgdrawSpacing)
            imgSpacing.Alignment = iTextSharp.text.Image.LEFT_ALIGN  'left
            imgSpacing.Border = 0

            strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
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
            table.SetWidths({20, 5, 75})
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
            table.SetWidths({20, 5, 75})
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
            table.SetWidths({20, 5, 75})
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

            table = New PdfPTable(5)
            table.WidthPercentage = 105
            table.SetWidths({5, 55, 15, 15, 10})
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

            'Amali1
            cell = New PdfPCell()
            cetak = "Amali"
            para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD))
            para.Alignment = Element.ALIGN_CENTER
            cell.AddElement(para)
            cell.Border = 0
            table.AddCell(cell)
            myDocument.Add(table)

            For i As Integer = 0 To datRespondent.Rows.Count - 1

                Dim strkey As String = datRespondent.DataKeys(i).Value.ToString

                strSQL = "SELECT kpmkv_pelajar.PelajarID,  kpmkv_pelajar.Nama, kpmkv_pelajar.MYKAD, kpmkv_pelajar.AngkaGiliran, kpmkv_kursus.KodKursus, "
                strSQL += " kpmkv_pelajar_markah.A_Amali1, kpmkv_pelajar_markah.A_Amali2, kpmkv_pelajar_markah.A_Amali3, kpmkv_pelajar_markah.A_Amali4,"
                strSQL += " kpmkv_pelajar_markah.A_Amali5, kpmkv_pelajar_markah.A_Amali6, kpmkv_pelajar_markah.A_Amali7, kpmkv_pelajar_markah.A_Amali8,"
                strSQL += " kpmkv_pelajar_markah.A_Teori1, kpmkv_pelajar_markah.A_Teori2, kpmkv_pelajar_markah.A_Teori3, kpmkv_pelajar_markah.A_Teori4,"
                strSQL += " kpmkv_pelajar_markah.A_Teori5, kpmkv_pelajar_markah.A_Teori6, kpmkv_pelajar_markah.A_Teori7, kpmkv_pelajar_markah.A_Teori8"
                strSQL += " FROM  kpmkv_pelajar_markah LEFT OUTER JOIN kpmkv_pelajar ON kpmkv_pelajar_markah.PelajarID = kpmkv_pelajar.PelajarID"
                strSQL += " LEFT OUTER Join kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID"
                strSQL += " WHERE kpmkv_pelajar.PelajarID = '" & strkey & "' AND kpmkv_pelajar.KolejRecordID='" & lblKolejID.Text & "' and kpmkv_pelajar.IsDeleted='N' and kpmkv_pelajar.StatusID='2'"

                '--tahun
                If Not ddlTahun.Text = "PILIH" Then
                    strSQL += " AND kpmkv_pelajar.Tahun ='" & ddlTahun.Text & "'"
                End If
                '--semester
                If Not ddlSemester.Text = "PILIH" Then
                    strSQL += " AND kpmkv_pelajar.Semester ='" & ddlSemester.Text & "'"
                End If
                '--sesi
                If Not chkSesi.Text = "" Then
                    strSQL += " AND kpmkv_pelajar.Sesi ='" & chkSesi.Text & "'"
                End If
                '--kursus
                If Not ddlKodKursus.Text = "" Then
                    strSQL += " AND kpmkv_pelajar.KursusID ='" & ddlKodKursus.SelectedValue & "'"
                End If
                '--jantina
                If Not ddlKelas.Text = "" Then
                    strSQL += " AND kpmkv_pelajar.KelasID ='" & ddlKelas.SelectedValue & "'"
                End If

                strSQL += " ORDER BY kpmkv_pelajar.Nama ASC"
                strRet = oCommon.getFieldValueEx(strSQL)

                Dim PAV_info As Array
                PAV_info = strRet.Split("|")

                Dim strPelajarID As String = PAV_info(0)
                Dim strNama As String = PAV_info(1)
                Dim strMykad As String = PAV_info(2)
                Dim strAngkaGiliran As String = PAV_info(3)
                strKodKursus = PAV_info(4)
                Dim strAmali1 As String = PAV_info(5)

                table = New PdfPTable(5)
                table.WidthPercentage = 105
                table.SetWidths({5, 55, 15, 15, 10})
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

                'Amali1
                cell = New PdfPCell()
                cetak = strAmali1
                para = New Paragraph(cetak, FontFactory.GetFont("Arial", 8))
                para.Alignment = Element.ALIGN_CENTER
                cell.AddElement(para)
                cell.Border = 0
                table.AddCell(cell)
                myDocument.Add(table)

            Next

            myDocument.Close()

            HttpContext.Current.Response.Write(myDocument)
            HttpContext.Current.Response.End()

        Catch ex As Exception

        End Try
    End Sub

End Class