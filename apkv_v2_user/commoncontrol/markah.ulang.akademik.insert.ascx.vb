Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Globalization

Public Class markah_ulang_akademik_insert
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
                strSQL = "SELECT * FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Markah Ulang Akademik' AND Aktif='1'"
                If oCommon.isExist(strSQL) = True Then

                    'count data takwim
                    'Get the data from database into datatable
                    Dim cmd As New SqlCommand("SELECT TakwimID FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Markah Ulang Akademik' AND Aktif='1'")
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
                                btnExport.Enabled = True
                                btnUpdate.Enabled = True
                            End If
                        Else
                            btnExport.Enabled = False
                            btnUpdate.Enabled = False
                            lblMsg.Text = "Markah Ulang Akademik telah ditutup!"
                        End If
                    Next
                Else
                    btnExport.Enabled = False
                    btnUpdate.Enabled = False
                    lblMsg.Text = "Markah Ulang Akademik telah ditutup!"
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

        strSQL = "SELECT kpmkv_kursus.KodKursus, kpmkv_kursus.KursusID FROM kpmkv_kursus_kolej LEFT OUTER JOIN"
        strSQL += " kpmkv_kursus ON kpmkv_kursus_kolej.KursusID = kpmkv_kursus.KursusID"
        strSQL += " WHERE kpmkv_kursus_kolej.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kursus.Tahun='" & ddlTahun.SelectedValue & "' "
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
        strSQL = " SELECT kpmkv_kelas.NamaKelas, kpmkv_kelas.KelasID FROM kpmkv_kelas_kursus LEFT OUTER JOIN kpmkv_kelas ON kpmkv_kelas_kursus.KelasID = kpmkv_kelas.KelasID LEFT OUTER JOIN"
        strSQL += " kpmkv_kursus ON kpmkv_kelas_kursus.KursusID = kpmkv_kursus.KursusID"
        strSQL += " WHERE kpmkv_kelas.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kelas_kursus.KursusID= '" & ddlKodKursus.SelectedValue & "' AND kpmkv_kursus.Tahun= '" & ddlTahun.SelectedValue & "' ORDER BY  kpmkv_kelas.NamaKelas"
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
                ' divMsg.Attributes("class") = "info"
                ' lblMsg.Text = "Jumlah rekod#:" & myDataSet.Tables(0).Rows.Count
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

        tmpSQL = "SELECT kpmkv_pelajar_ulang.PelajarUlangID, kpmkv_pelajar_ulang.Tahun, kpmkv_pelajar_ulang.Semester, kpmkv_pelajar_ulang.Sesi, kpmkv_pelajar_ulang.NamaMataPelajaran, kpmkv_pelajar_ulang.MarkahPB, kpmkv_pelajar_ulang.MarkahPA, "
        tmpSQL += " kpmkv_pelajar_ulang.Gred, kpmkv_pelajar.Tahun, kpmkv_pelajar.Nama, kpmkv_pelajar.Mykad, kpmkv_pelajar.AngkaGiliran, kpmkv_kursus.KodKursus, kpmkv_kelas.NamaKelas FROM  kpmkv_pelajar_ulang LEFT OUTER JOIN"
        tmpSQL += " kpmkv_pelajar ON kpmkv_pelajar.PelajarID = kpmkv_pelajar_ulang.PelajarID LEFT OUTER JOIN"
        tmpSQL += " kpmkv_kursus ON kpmkv_kursus.KursusID = kpmkv_pelajar_ulang.KursusID LEFT OUTER JOIN"
        tmpSQL += " kpmkv_kelas ON kpmkv_kelas.KelasID = kpmkv_pelajar_ulang.KelasID "
        strWhere = " WHERE kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.StatusID='2' AND kpmkv_pelajar_ulang.KolejRecordID='" & lblKolejID.Text & "'"

        '--tahun
        If Not ddlTahun.Text = "PILIH" Then
            strWhere += " AND kpmkv_pelajar_ulang.Tahun ='" & ddlTahun.Text & "'"
        End If
        '--semester
        If Not ddlSemester.Text = "PILIH" Then
            strWhere += " AND kpmkv_pelajar_ulang.Semester ='" & ddlSemester.Text & "'"
        End If
        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere += " AND kpmkv_pelajar_ulang.Sesi ='" & chkSesi.Text & "'"
        End If
        '--kursus
        If Not ddlKodKursus.Text = "" Then
            strWhere += " AND kpmkv_pelajar_ulang.KursusID ='" & ddlKodKursus.SelectedValue & "'"
        End If
        '--jantina
        If Not ddlKelas.Text = "" Then
            strWhere += " AND kpmkv_pelajar_ulang.KelasID ='" & ddlKelas.SelectedValue & "'"
        End If

        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        ''Response.Write(getSQL)
        Return getSQL

    End Function

    Private Sub datRespondent_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles datRespondent.RowDeleting
        lblMsg.Text = ""
        lblMsgTop.Text = ""
        Dim strPelajarUID As Integer = datRespondent.DataKeys(e.RowIndex).Values("PelajarUlangID")
        Try
            If Not strPelajarUID = Session("strPelajarUID") Then
                strSQL = "Select Gred FROM kpmkv_pelajar_ulang WHERE PelajarUlangID='" & strPelajarUID & "'"
                Dim strGred As String = oCommon.getFieldValue(strSQL)

                If strGred = "NULL" Or strGred = "" Then
                    strSQL = "DELETE FROM kpmkv_pelajar_ulang WHERE PelajarUlangID='" & strPelajarUID & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                    If strRet = "0" Then
                        divTop.Attributes("class") = "info"
                        lblMsgTop.Text = "Calon berjaya dipadamkan"
                        Session("strPelajarUID") = ""
                    Else
                        divTop.Attributes("class") = "error"
                        lblMsgTop.Text = "Calon tidak berjaya dipadam"
                        Session("strPelajarUID") = ""
                    End If
                Else
                    divTop.Attributes("class") = "error"
                    lblMsgTop.Text = "Calon tidak berjaya dipadam. Gred Ulang telah dijana"
                    Session("strPelajarUID") = ""
                End If
            Else
                Session("strPelajarUID") = ""
            End If
        Catch ex As Exception
            divTop.Attributes("class") = "error"
        End Try

        strRet = BindData(datRespondent)

    End Sub
    Private Sub datRespondent_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles datRespondent.SelectedIndexChanging
        Dim strKeyID As String = datRespondent.DataKeys(e.NewSelectedIndex).Value.ToString
        'Response.Redirect("pelajar.ulang.akademik.view.aspx?PelajarID=" & strKeyID)

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
    Private Sub hiddencolumn()

        For i As Integer = 0 To datRespondent.Rows.Count - 1
            Dim row As GridViewRow = datRespondent.Rows(i)
            Dim strPB As TextBox = CType(row.FindControl("MarkahPB"), TextBox)
            Dim strPA As TextBox = CType(row.FindControl("MarkahPA"), TextBox)

            strSQL = "SELECT PB FROM kpmkv_pelajar_ulang WHERE PelajarUlangID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            Dim strPBHide As String = oCommon.getFieldValue(strSQL)

            If strPBHide = "1" Then
                strPB.Enabled = True
            Else
                strPB.Enabled = False
            End If

            strSQL = "SELECT PA FROM kpmkv_pelajar_ulang WHERE PelajarUlangID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            Dim strPAHide As String = oCommon.getFieldValue(strSQL)

            If strPAHide = "1" Then
                strPA.Enabled = True
            Else
                strPA.Enabled = False
            End If
        Next
    End Sub
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click

        If ValidateForm() = False Then
            lblMsg.Text = "Sila masukkan NOMBOR 0-100 SAHAJA"
            Exit Sub
        End If

        Try
            For i As Integer = 0 To datRespondent.Rows.Count - 1
                Dim row As GridViewRow = datRespondent.Rows(i)
                Dim strPBA As TextBox = datRespondent.Rows(i).FindControl("MarkahPB")
                Dim strPAA As TextBox = datRespondent.Rows(i).FindControl("MarkahPA")


                'assign value to integer
                Dim strPB As Integer = strPBA.Text
                Dim strPA As Integer = strPAA.Text

                strSQL = "UPDATE kpmkv_pelajar_ulang SET MarkahPB='" & strPB & "', MarkahPA='" & strPA & "' WHERE PelajarUlangID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
                If strRet = "0" Then
                    divMsg.Attributes("class") = "info"
                    lblMsg.Text = "Berjaya!.Kemaskini markah Ulang  Akademik."
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Tidak Berjaya!.Kemaskini markah Ulang  Akademik."
                End If
            Next

        Catch ex As Exception
            lblMsg.Text = "Error:" & ex.Message
        End Try
        Akademik_markah()
        strRet = BindData(datRespondent)
        hiddencolumn()
    End Sub
    Private Function ValidateForm() As Boolean
        For i As Integer = 0 To datRespondent.Rows.Count - 1
            Dim row As GridViewRow = datRespondent.Rows(i)
            Dim strPB As TextBox = CType(row.FindControl("MarkahPB"), TextBox)
            Dim strPA As TextBox = CType(row.FindControl("MarkahPA"), TextBox)

            If Not strPB.Text.Length = 0 Then
                If oCommon.IsCurrency(strPB.Text) = False Then
                    Return False
                End If
                If CInt(strPB.Text) > 100 Then
                    Return False
                End If
            Else
                strPB.Text = "0"
            End If

            If Not strPA.Text.Length = 0 Then
                If oCommon.IsCurrency(strPA.Text) = False Then
                    Return False
                End If
                If CInt(strPA.Text) > 100 Then
                    Return False
                End If
            Else
                strPA.Text = "0"
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
            Dim row As GridViewRow = datRespondent.Rows(i)
            Dim strPB As TextBox = CType(row.FindControl("MarkahPB"), TextBox)
            Dim strPA As TextBox = CType(row.FindControl("MarkahPA"), TextBox)
            Dim strNamaMataPelajaran As Label = CType(row.FindControl("NamaMataPelajaran"), Label)
            'assign value to integer
            Dim PB1 As Integer = strPB.Text
            Dim PA2 As Integer = strPA.Text
            Dim strGredNama As String = ""
            Dim strpointerNama As String = ""
            Dim strGredMarkah As String = ""
            Dim strMP As String = strNamaMataPelajaran.Text

            Dim strPelajarID As Integer
            Dim PB As String
            Dim PA As String
            Dim PBMarkah As Integer
            Dim PAMarkah As Integer
            Dim Pointer As Integer

            Select Case strNamaMataPelajaran.Text
                Case "BAHASA MELAYU"
                    strGredNama = "BM"
                    strpointerNama = "BahasaMelayu"
                Case "BAHASA INGGERIS"
                    strGredNama = "BI"
                    strpointerNama = "BahasaInggeris"
                Case "SAINS"
                    strGredNama = "SC"
                    strpointerNama = "Science"
                Case "SEJARAH"
                    strGredNama = "SJ"
                    strpointerNama = "Sejarah"
                Case "PENDIDIKAN ISLAM"
                    strGredNama = "PI"
                    strpointerNama = "PendidikanIslam"
                Case "PENDIDIKAN MORAL"
                    strGredNama = "PM"
                    strpointerNama = "PendidikanMoral"
                Case "MATEMATIK"
                    strGredNama = "MT"
                    strpointerNama = "Mathematics"
            End Select

            'get pelajarid from kpmkv_pelajar_ulang
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang Where  PelajarUlangID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND NamaMataPelajaran='" & strMP & "'"
            strPelajarID = oCommon.getFieldValue(strSQL)


            Dim strPointerGredMarkah As String
            'get gred from kpmkv_pelajar_markah
            If Not String.IsNullOrEmpty(strGredNama) Then
                strSQL = "SELECT  Gred" & strGredNama & "  FROM kpmkv_pelajar_markah Where PelajarID='" & strPelajarID & "'"
                strGredMarkah = oCommon.getFieldValue(strSQL)
            End If

            ''get pointer for strGredMarkah
            strSQL = "SELECT Pointer FROM kpmkv_gred WHERE Gred = '" & strGredMarkah & "' AND Jenis = 'AKADEMIK_ULANG'"
            strPointerGredMarkah = oCommon.getFieldValue(strSQL)

            'strSQL = "SELECT PB FROM kpmkv_matapelajaran WHERE NamaMataPelajaran='" & strMP & "' AND SUBSTRING(KodMataPelajaran,4,3)='" & strKodMP & "' AND Tahun='" & ddlTahun.Text & "'"
            strSQL = "SELECT PB FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
            PB = oCommon.getFieldValue(strSQL)

            'strSQL = "SELECT PA FROM kpmkv_matapelajaran WHERE NamaMataPelajaran='" & strMP & "' AND SUBSTRING(KodMataPelajaran,4,3)='" & strKodMP & "' AND Tahun='" & ddlTahun.Text & "'"
            strSQL = "SELECT PA FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
            PA = oCommon.getFieldValue(strSQL)

            If Not String.IsNullOrEmpty(PB1) Then
                PBMarkah = oCommon.DoConvertN((PB1 / 100) * CInt(PB), 2)
            End If
            If Not String.IsNullOrEmpty(PA2) Then
                PAMarkah = oCommon.DoConvertN((PA2 / 100) * CInt(PA), 2)
            End If

            Pointer = PBMarkah + PAMarkah

            'untuk tidak hadir sahaja
            If PB1 = -1 Then
                strSQL = "UPDATE kpmkv_pelajar_ulang SET Gred='T' Where PelajarUlangID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND NamaMataPelajaran='" & strMP & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

                strSQL = "UPDATE kpmkv_pelajar_markah SET Gred" & strGredNama & "='" & strGredMarkah & "' Where PelajarID='" & strPelajarID & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

            ElseIf PA2 = -1 Then
                strSQL = "UPDATE kpmkv_pelajar_ulang SET Gred='T' Where PelajarUlangID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND NamaMataPelajaran='" & strMP & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

                strSQL = "UPDATE kpmkv_pelajar_markah SET Gred" & strGredNama & "='" & strGredMarkah & "' Where PelajarID='" & strPelajarID & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

            Else
                strSQL = "SELECT TOP ( 1 ) Gred FROM  kpmkv_gred WHERE '" & Pointer & "' BETWEEN MarkahFrom AND MarkahTo AND Jenis='AKADEMIK_ULANG'"
                Dim Gred As String = oCommon.getFieldValue(strSQL)
                If Not String.IsNullOrEmpty(Pointer) Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang SET Gred='" & Gred & "' Where PelajarUlangID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND NamaMataPelajaran='" & strMP & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                End If

                'get pointer for Gred
                strSQL = "SELECT Pointer FROM kpmkv_gred WHERE Gred = '" & Gred & "' AND Jenis = 'AKADEMIK_ULANG'"
                Dim strPointerGred As String = oCommon.getFieldValue(strSQL)

                '' if gred less than gredmarkah
                If strPointerGred < strPointerGredMarkah Then
                    strSQL = "UPDATE kpmkv_pelajar_markah SET Gred" & strGredNama & "='" & strGredMarkah & "'," & strpointerNama & "='" & Pointer & "' Where PelajarID='" & strPelajarID & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)

                ElseIf Gred = strGredMarkah Then
                    strSQL = "UPDATE kpmkv_pelajar_markah SET Gred" & strGredNama & "='" & strGredMarkah & "'," & strpointerNama & "='" & Pointer & "' Where PelajarID='" & strPelajarID & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                ElseIf Gred = "C" Then
                    'update bm
                    strSQL = "UPDATE kpmkv_pelajar_markah SET Gred" & strGredNama & "='C'," & strpointerNama & "='" & Pointer & "' Where PelajarID='" & strPelajarID & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    strSQL = "UPDATE kpmkv_pelajar_markah SET Gred" & strGredNama & "='" & strGredMarkah & "'," & strpointerNama & "='" & Pointer & "' Where PelajarID='" & strPelajarID & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                End If
            End If
            '------BM SETARA ULANG 09112016
            If ddlSemester.Text = "4" And strpointerNama = "BahasaMelayu" Then
                Dim BerterusanBM As Integer
                Dim AkhiranBM As Integer
                Dim PB4 As Integer
                Dim PABmSetara As Integer
                Dim PAPB4 As Integer

                Dim B_BahasaMelayuSem1 As Integer
                Dim B_BahasaMelayuSem2 As Integer
                Dim B_BahasaMelayuSem3 As Integer
                Dim B_BahasaMelayuSem4 As Integer
                Dim A_BahasaMelayuSem4 As Integer
                Dim PointerBMSetara As Integer

                ' strSQL = "Select PB from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A04'+'" & strKodMP & "%' AND Tahun='" & ddlTahun.Text & "'"
                strSQL = "SELECT PB FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
                BerterusanBM = oCommon.getFieldValue(strSQL)

                'strSQL = "Select PA from kpmkv_matapelajaran Where KodMataPelajaran LIKE '%A04'+'" & strKodMP & "%' AND Tahun='" & ddlTahun.Text & "'"
                strSQL = "SELECT PA FROM kpmkv_aka_weightage WHERE Tahun='" & ddlTahun.Text & "'"
                AkhiranBM = oCommon.getFieldValue(strSQL)

                'get mykad
                strSQL = " SELECT Mykad FROM kpmkv_pelajar"
                strSQL += " WHERE PelajarID='" & strPelajarID & "'"
                Dim strMYKAD1 As String = oCommon.getFieldValue(strSQL)

                'get pelajarid
                strSQL = " SELECT PelajarID FROM kpmkv_pelajar"
                strSQL += " WHERE Tahun='" & ddlTahun.Text & "' AND Semester='1'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strSQL += " AND Mykad='" & strMYKAD1 & "'"
                Dim strPelajarID1 As String = oCommon.getFieldValue(strSQL)
                'get bm sem 1
                strSQL = "Select BahasaMelayu from kpmkv_pelajar_markah Where PelajarID='" & strPelajarID1 & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='1'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_BahasaMelayuSem1 = oCommon.getFieldValue(strSQL)
                'round up
                B_BahasaMelayuSem1 = Math.Ceiling(B_BahasaMelayuSem1)
                '----------------------------------------------------------------------------

                'get pelajarid
                strSQL = " SELECT PelajarID FROM kpmkv_pelajar"
                strSQL += " WHERE Tahun='" & ddlTahun.Text & "' AND Semester='2'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strSQL += " AND Mykad='" & strMYKAD1 & "'"
                Dim strPelajarID2 As String = oCommon.getFieldValue(strSQL)
                'get Bm sem 2
                strSQL = "Select BahasaMelayu from kpmkv_pelajar_markah Where PelajarID='" & strPelajarID2 & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='2'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_BahasaMelayuSem2 = oCommon.getFieldValue(strSQL)
                'round up
                B_BahasaMelayuSem2 = Math.Ceiling(B_BahasaMelayuSem2)


                'get pelajarid
                strSQL = " SELECT PelajarID FROM kpmkv_pelajar"
                strSQL += " WHERE Tahun='" & ddlTahun.Text & "' AND Semester='3'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                strSQL += " AND Mykad='" & strMYKAD1 & "'"
                Dim strPelajarID3 As String = oCommon.getFieldValue(strSQL)
                'get bm sem 3
                strSQL = "Select BahasaMelayu from kpmkv_pelajar_markah Where PelajarID='" & strPelajarID3 & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='3'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_BahasaMelayuSem3 = oCommon.getFieldValue(strSQL)
                'round up
                B_BahasaMelayuSem3 = Math.Ceiling(B_BahasaMelayuSem3)

                'get bm sem 4 PB
                strSQL = "Select B_BahasaMelayu from kpmkv_pelajar_markah Where PelajarID='" & strPelajarID & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='4'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                B_BahasaMelayuSem4 = oCommon.getFieldValue(strSQL)

                'get bm sem 4 PA
                strSQL = "Select A_BahasaMelayu3 from kpmkv_pelajar_markah Where PelajarID='" & strPelajarID & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='4'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                A_BahasaMelayuSem4 = oCommon.getFieldValue(strSQL)

                Dim Kertas1 As Integer = 0
                Dim Kertas2 As Integer = 0

                strSQL = "SELECT A_BahasaMelayu1, A_BahasaMelayu2 FROM kpmkv_pelajar_markah Where PelajarID='" & strPelajarID & "'"
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

                If Not (B_BahasaMelayuSem4) = "-1" Then
                    PB4 = Math.Ceiling((B_BahasaMelayuSem4 / 100) * BerterusanBM)
                    PABmSetara = Math.Ceiling((A_BahasaMelayuSem4 / 100) * 40)
                    PAPB4 = Math.Ceiling(((Kertas1 + Kertas2 + PABmSetara) / 280) * AkhiranBM)
                    'PAPB4 = Math.Ceiling(PAPB * AkhiranBM)

                    'gred sem 4 
                    Dim PointSem4 As Integer = Math.Ceiling(PB4 + PAPB4)
                    strSQL = "UPDATE kpmkv_pelajar_markah SET BahasaMelayu='" & PointSem4 & "' Where PelajarID='" & strPelajarID & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='4'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)

                    PointerBMSetara = Math.Ceiling((((B_BahasaMelayuSem1 / 100) * 10) + ((B_BahasaMelayuSem2 / 100) * 10) + ((B_BahasaMelayuSem3 / 100) * 10) + ((PointSem4 / 100) * 70)))

                    strSQL = "UPDATE kpmkv_pelajar_markah SET PointerBMSetara='" & PointerBMSetara & "' Where PelajarID='" & strPelajarID & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='4'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)

                ElseIf (B_BahasaMelayuSem4) = "-1" Then
                    strSQL = "UPDATE kpmkv_pelajar_markah SET BahasaMelayu='-1' Where PelajarID='" & strPelajarID & "'"
                    strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='4'"
                    strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                End If
            End If

        Next
    End Sub
End Class