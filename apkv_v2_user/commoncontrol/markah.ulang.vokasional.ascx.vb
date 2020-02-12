Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Globalization

Public Class markah_ulang_vokasional
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
                strSQL = "SELECT * FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Markah Ulang Vokasional' AND Aktif='1'"
                If oCommon.isExist(strSQL) = True Then

                    'count data takwim
                    'Get the data from database into datatable
                    Dim cmd As New SqlCommand("SELECT TakwimID FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Markah Ulang Vokasional' AND Aktif='1'")
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
                            lblMsg.Text = "Markah Ulang Vokasional telah ditutup!"
                        End If
                    Next
                Else
                    btnExport.Enabled = False
                    btnUpdate.Enabled = False
                    lblMsg.Text = "Markah Ulang Vokasional telah ditutup!"
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
    Private Sub datRespondent_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles datRespondent.RowCommand
        lblMsg.Text = ""

        If (e.CommandName = "Batal") Then
            Dim strPelajarUID = Int32.Parse(e.CommandArgument.ToString())
            Try


                strSQL = "DELETE FROM kpmkv_pelajar_ulang_vokasional WHERE PelajarUlangVID='" & strPelajarUID & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
                If strRet = "0" Then
                    divMsg.Attributes("class") = "info"
                    lblMsg.Text = "Pelajar berjaya dipadamkan"
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Pelajar tidak berjaya dipadam"

                End If
            Catch ex As Exception
                divMsg.Attributes("class") = "error"
            End Try
        End If
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
        Dim strOrder As String = " ORDER BY kpmkv_pelajar.Tahun, kpmkv_pelajar.Semester, kpmkv_pelajar.Sesi ASC"

        '--not deleted
        tmpSQL = "SELECT kpmkv_pelajar_ulang_vokasional.PelajarUlangVID, kpmkv_pelajar_ulang_vokasional.Tahun, kpmkv_pelajar_ulang_vokasional.Semester, kpmkv_pelajar_ulang_vokasional.Sesi, kpmkv_pelajar_ulang_vokasional.NamaModul, "
        tmpSQL += " kpmkv_pelajar_ulang_vokasional.MarkahPBTeori,kpmkv_pelajar_ulang_vokasional.MarkahPBAmali, kpmkv_pelajar_ulang_vokasional.MarkahPATeori, kpmkv_pelajar_ulang_vokasional.MarkahPAAmali,"
        tmpSQL += " kpmkv_pelajar_ulang_vokasional.Gred,kpmkv_pelajar.Tahun, kpmkv_pelajar.Nama, kpmkv_pelajar.Mykad, kpmkv_pelajar.AngkaGiliran, kpmkv_kursus.KodKursus, kpmkv_kelas.NamaKelas FROM  kpmkv_pelajar_ulang_vokasional LEFT OUTER JOIN"
        tmpSQL += " kpmkv_pelajar ON kpmkv_pelajar_ulang_vokasional.PelajarID = kpmkv_pelajar.PelajarID LEFT OUTER JOIN"
        tmpSQL += " kpmkv_kursus ON kpmkv_pelajar_ulang_vokasional.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
        tmpSQL += " kpmkv_kelas ON kpmkv_pelajar_ulang_vokasional.KelasID = kpmkv_kelas.KelasID "
        strWhere = " WHERE kpmkv_pelajar_ulang_vokasional.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.StatusID='2' "

        '--tahun
        If Not ddlTahun.Text = "PILIH" Then
            strWhere += " AND kpmkv_pelajar_ulang_vokasional.Tahun ='" & ddlTahun.Text & "'"
        End If
        '--semester
        If Not ddlSemester.Text = "PILIH" Then
            strWhere += " AND kpmkv_pelajar_ulang_vokasional.Semester ='" & ddlSemester.Text & "'"
        End If
        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere += " AND kpmkv_pelajar_ulang_vokasional.Sesi ='" & chkSesi.Text & "'"
        End If
        '--kursus
        If Not ddlKodKursus.Text = "" Then
            strWhere += " AND kpmkv_pelajar_ulang_vokasional.KursusID ='" & ddlKodKursus.SelectedValue & "'"
        End If
        '--jantina
        If Not ddlKelas.Text = "" Then
            strWhere += " AND kpmkv_pelajar_ulang_vokasional.KelasID ='" & ddlKelas.SelectedValue & "'"
        End If

        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        ''Response.Write(getSQL)


        Return getSQL

    End Function
    Private Sub datRespondent_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles datRespondent.RowDeleting
        lblMsg.Text = ""
        lblMsgTop.Text = ""

        Dim strPelajarUID As Integer = datRespondent.DataKeys(e.RowIndex).Values("PelajarUlangVID")
        Try
            If Not strPelajarUID = 0 Then
                strSQL = "Select Gred FROM kpmkv_pelajar_ulang_vokasional WHERE PelajarUlangVID='" & strPelajarUID & "'"
                Dim strGred As String = oCommon.getFieldValue(strSQL)

                If strGred = "NULL" Or strGred = "" Then
                    strSQL = "DELETE FROM kpmkv_pelajar_ulang_vokasional WHERE PelajarUlangVID='" & strPelajarUID & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                    If strRet = "0" Then
                        divTop.Attributes("class") = "info"
                        lblMsgTop.Text = "Calon berjaya dipadamkan"

                    Else
                        divTop.Attributes("class") = "error"
                        lblMsgTop.Text = "Calon tidak berjaya dipadam"

                    End If
                Else
                    divTop.Attributes("class") = "error"
                    lblMsgTop.Text = "Calon tidak berjaya dipadam. Gred Ulang telah dijana"

                End If
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
            Dim strPBT As TextBox = datRespondent.Rows(i).FindControl("MarkahPBTeori")
            Dim strPBA As TextBox = datRespondent.Rows(i).FindControl("MarkahPBAmali")
            Dim strPAT As TextBox = datRespondent.Rows(i).FindControl("MarkahPATeori")
            Dim strPAA As TextBox = datRespondent.Rows(i).FindControl("MarkahPAAmali")
            Dim strPAG As Label = datRespondent.Rows(i).FindControl("Gred")

            Dim strPBTHide As String = ""
            Dim strPBAHide As String = ""
            Dim strPATHide As String = ""
            Dim strPAAHide As String = ""

            If Not strPAG.Text = "" Then
                strSQL = "SELECT PBTeori FROM kpmkv_pelajar_ulang_vokasional WHERE PelajarUlangVID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strPBTHide = oCommon.getFieldValue(strSQL)

                If strPBTHide = "1" Then
                    strPBT.Enabled = True
                Else
                    strPBT.Enabled = False
                End If

                strSQL = "SELECT PBAmali FROM kpmkv_pelajar_ulang_vokasional WHERE PelajarUlangVID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strPBAHide = oCommon.getFieldValue(strSQL)

                If strPBAHide = "1" Then
                    strPBA.Enabled = True
                Else
                    strPBA.Enabled = False
                End If

                strSQL = "SELECT PATeori FROM kpmkv_pelajar_ulang_vokasional WHERE PelajarUlangVID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strPATHide = oCommon.getFieldValue(strSQL)

                If strPATHide = "1" Then
                    strPAT.Enabled = True
                Else
                    strPAT.Enabled = False
                End If

                strSQL = "SELECT PAAmali FROM kpmkv_pelajar_ulang_vokasional WHERE PelajarUlangVID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strPAAHide = oCommon.getFieldValue(strSQL)

                If strPAAHide = "1" Then
                    strPAA.Enabled = True
                Else
                    strPAA.Enabled = False
                End If

            Else

                strSQL = "SELECT PBTeori FROM kpmkv_pelajar_ulang_vokasional WHERE PelajarUlangVID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strPBTHide = oCommon.getFieldValue(strSQL)

                If strPBTHide = "1" Then
                    strPBT.Enabled = True
                    strPBT.Text = ""
                Else
                    strPBT.Enabled = False
                End If

                strSQL = "SELECT PBAmali FROM kpmkv_pelajar_ulang_vokasional WHERE PelajarUlangVID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strPBAHide = oCommon.getFieldValue(strSQL)

                If strPBAHide = "1" Then
                    strPBA.Enabled = True
                    strPBA.Text = ""
                Else
                    strPBA.Enabled = False
                End If

                strSQL = "SELECT PATeori FROM kpmkv_pelajar_ulang_vokasional WHERE PelajarUlangVID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strPATHide = oCommon.getFieldValue(strSQL)

                If strPATHide = "1" Then
                    strPAT.Enabled = True
                    strPAT.Text = ""
                Else
                    strPAT.Enabled = False
                End If

                strSQL = "SELECT PAAmali FROM kpmkv_pelajar_ulang_vokasional WHERE PelajarUlangVID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strPAAHide = oCommon.getFieldValue(strSQL)

                If strPAAHide = "1" Then
                    strPAA.Enabled = True
                    strPAA.Text = ""
                Else
                    strPAA.Enabled = False
                End If
            End If
        Next
    End Sub
    'Private Sub hiddencolumnNext()

    '    For i As Integer = 0 To datRespondent.Rows.Count - 1
    '        Dim row As GridViewRow = datRespondent.Rows(i)
    '        Dim strPBT As TextBox = datRespondent.Rows(i).FindControl("MarkahPBTeori")
    '        Dim strPBA As TextBox = datRespondent.Rows(i).FindControl("MarkahPBAmali")
    '        Dim strPAT As TextBox = datRespondent.Rows(i).FindControl("MarkahPATeori")
    '        Dim strPAA As TextBox = datRespondent.Rows(i).FindControl("MarkahPAAmali")

    '        strSQL = "SELECT PBTeori FROM kpmkv_pelajar_ulang_vokasional WHERE PelajarUlangVID='" & datRespondent.DataKeys(i).Value.ToString & "'"
    '        Dim strPBTHide As String = oCommon.getFieldValue(strSQL)

    '        If strPBTHide = "1" Then
    '            strPBT.Enabled = True
    '        Else
    '            strPBT.Enabled = False
    '        End If

    '        strSQL = "SELECT PBAmali FROM kpmkv_pelajar_ulang_vokasional WHERE PelajarUlangVID='" & datRespondent.DataKeys(i).Value.ToString & "'"
    '        Dim strPBAHide As String = oCommon.getFieldValue(strSQL)

    '        If strPBAHide = "1" Then
    '            strPBA.Enabled = True
    '        Else
    '            strPBA.Enabled = False
    '        End If

    '        strSQL = "SELECT PATeori FROM kpmkv_pelajar_ulang_vokasional WHERE PelajarUlangVID='" & datRespondent.DataKeys(i).Value.ToString & "'"
    '        Dim strPATHide As String = oCommon.getFieldValue(strSQL)

    '        If strPATHide = "1" Then
    '            strPAT.Enabled = True
    '        Else
    '            strPAT.Enabled = False
    '        End If

    '        strSQL = "SELECT PAAmali FROM kpmkv_pelajar_ulang_vokasional WHERE PelajarUlangVID='" & datRespondent.DataKeys(i).Value.ToString & "'"
    '        Dim strPAAHide As String = oCommon.getFieldValue(strSQL)

    '        If strPAAHide = "1" Then
    '            strPAA.Enabled = True
    '        Else
    '            strPAA.Enabled = False
    '        End If
    '    Next
    'End Sub
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        lblMsg.Text = ""

        If ValidateForm() = False Then
            lblMsg.Text = "Sila masukkan NOMBOR 0-100 SAHAJA"
            Exit Sub
        End If

        Try
            For i As Integer = 0 To datRespondent.Rows.Count - 1
                Dim row As GridViewRow = datRespondent.Rows(i)
                Dim strPBT As TextBox = datRespondent.Rows(i).FindControl("MarkahPBTeori")
                Dim strPBA As TextBox = datRespondent.Rows(i).FindControl("MarkahPBAmali")
                Dim strPAT As TextBox = datRespondent.Rows(i).FindControl("MarkahPATeori")
                Dim strPAA As TextBox = datRespondent.Rows(i).FindControl("MarkahPAAmali")


                'assign value to integer
                Dim strPBT1 As Integer = strPBT.Text
                Dim strPBA1 As Integer = strPBA.Text
                Dim strPAT1 As Integer = strPAT.Text
                Dim strPAA1 As Integer = strPAA.Text


                strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET MarkahPBTeori='" & strPBT1 & "', MarkahPBAmali='" & strPBA1 & "', MarkahPATeori='" & strPAT1 & "', MarkahPAAmali='" & strPAA1 & "' WHERE PelajarUlangVID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
                If strRet = "0" Then
                    divMsg.Attributes("class") = "info"
                    lblMsg.Text = "Berjaya!.Kemaskini markah Ulang Vokasional"
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Tidak Berjaya!.Kemaskini markah Ulang Vokasional"
                End If
            Next

        Catch ex As Exception
            lblMsg.Text = "Error:" & ex.Message
        End Try
        Vokasional_markah()
        strRet = BindData(datRespondent)
        ' hiddencolumnNext()
    End Sub
    Private Function ValidateForm() As Boolean
        For i As Integer = 0 To datRespondent.Rows.Count - 1
            Dim row As GridViewRow = datRespondent.Rows(i)
            Dim strPBT As TextBox = CType(row.FindControl("MarkahPBTeori"), TextBox)
            Dim strPBA As TextBox = CType(row.FindControl("MarkahPBAmali"), TextBox)

            Dim strPAT As TextBox = CType(row.FindControl("MarkahPATeori"), TextBox)
            Dim strPAA As TextBox = CType(row.FindControl("MarkahPAAmali"), TextBox)
            '--validate NUMBER and less than 100
            '--strBahasaMelayu

            If Not strPBT.Text.Length = 0 Then
                If oCommon.IsCurrency(strPBT.Text) = False Then
                    Return False
                End If
                If CInt(strPBT.Text) > 100 Then
                    Return False
                End If
            Else
                strPBT.Text = "0"
            End If

            '--strBahasaMelayu1
            If Not strPBA.Text.Length = 0 Then
                If oCommon.IsCurrency(strPBA.Text) = False Then
                    Return False
                End If
                If CInt(strPBA.Text) > 100 Then
                    Return False
                End If
            Else
                strPBA.Text = "0"
            End If

            If Not strPAT.Text.Length = 0 Then
                If oCommon.IsCurrency(strPAT.Text) = False Then
                    Return False
                End If
                If CInt(strPAT.Text) > 100 Then
                    Return False
                End If
            Else
                strPAT.Text = "0"
            End If

            '--strBahasaMelayu1
            If Not strPAA.Text.Length = 0 Then
                If oCommon.IsCurrency(strPAA.Text) = False Then
                    Return False
                End If
                If CInt(strPAA.Text) > 100 Then
                    Return False
                End If
            Else
                strPAA.Text = "0"
            End If

        Next

        Return True
    End Function

    Private Sub Vokasional_markah()


        For i As Integer = 0 To datRespondent.Rows.Count - 1
            Dim row As GridViewRow = datRespondent.Rows(i)
            Dim strPBT As TextBox = CType(row.FindControl("MarkahPBTeori"), TextBox)
            Dim strPBA As TextBox = CType(row.FindControl("MarkahPBAmali"), TextBox)

            Dim strPAT As TextBox = CType(row.FindControl("MarkahPATeori"), TextBox)
            Dim strPAA As TextBox = CType(row.FindControl("MarkahPAAmali"), TextBox)

            Dim strNamaModul As Label = CType(row.FindControl("NamaModul"), Label)
            'assign value to integer
            Dim PB1 As Integer = strPBT.Text
            Dim PB2 As Integer = strPBA.Text

            Dim PA1 As Integer = strPAT.Text
            Dim PA2 As Integer = strPAA.Text
            Dim strModul As String = strNamaModul.Text

            Dim strKodModul As String
            Dim strGredMarkah As String = ""
            Dim strPelajarID As Integer
            Dim PBAmali As String
            Dim PBTeori As String
            Dim PAAmali As String
            Dim PATeori As String

            Dim PATotal As Integer
            Dim PATotal1 As Integer
            Dim PBTotal As Integer
            Dim PBTotal1 As Integer
            Dim Pointer As Integer
            'get kodmodul
            strSQL = "SELECT SUBSTRING(KodModul,6,1) FROM kpmkv_modul WHERE NamaModul='" & strModul & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
            strKodModul = oCommon.getFieldValue(strSQL)

            'get pelajarid
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional Where PelajarUlangVID='" & datRespondent.DataKeys(i).Value.ToString & "'"
            strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
            strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND NamaModul='" & strModul & "'"
            strPelajarID = oCommon.getFieldValue(strSQL)

            'get gred from kpmkv_pelajar_markah
            If Not String.IsNullOrEmpty(strKodModul) Then
                strSQL = "SELECT  GredV" & strKodModul & "  FROM kpmkv_pelajar_markah Where PelajarID='" & strPelajarID & "'"
                strGredMarkah = oCommon.getFieldValue(strSQL)
            End If

            strSQL = "SELECT PBAmali FROM kpmkv_modul WHERE NamaModul='" & strModul & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
            PBAmali = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT PBTeori FROM kpmkv_modul WHERE NamaModul='" & strModul & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
            PBTeori = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT PAAmali FROM kpmkv_modul WHERE NamaModul='" & strModul & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
            PAAmali = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT PATeori FROM kpmkv_modul WHERE NamaModul='" & strModul & "' AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "' AND Sesi='" & chkSesi.Text & "'"
            PATeori = oCommon.getFieldValue(strSQL)

            'change on 16082016
            'convert 0 if null
            If (String.IsNullOrEmpty(PBAmali.ToString())) Then
                PBAmali = 0
            End If

            If (String.IsNullOrEmpty(PBTeori.ToString())) Then
                PBTeori = 0
            End If

            If (String.IsNullOrEmpty(PAAmali.ToString())) Then
                PAAmali = 0
            End If

            If (String.IsNullOrEmpty(PATeori.ToString())) Then
                PATeori = 0
            End If

            If PB1 = "-1" Or PB2 = "-1" Or PA1 = "-1" Or PA2 = "-1" Then
                strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET Gred='T' Where PelajarUlangVID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND NamaModul='" & strModul & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

                strSQL = "UPDATE kpmkv_pelajar_markah SET GredV" & strKodModul & "='" & strGredMarkah & "' Where PelajarID='" & strPelajarID & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
            Else
                PBTotal = Math.Ceiling((PB2 / 100) * PBAmali)
                PBTotal1 = Math.Ceiling((PB1 / 100) * PBTeori)
                PATotal = Math.Ceiling((PA2 / 100) * PAAmali)
                PATotal1 = Math.Ceiling((PA1 / 100) * PATeori)


                Pointer = CInt(PBTotal) + CInt(PBTotal1) + CInt(PATotal) + CInt(PATotal1)

                strSQL = "SELECT TOP ( 1 ) Gred FROM  kpmkv_gred WHERE '" & Pointer & "' BETWEEN MarkahFrom AND MarkahTo AND Jenis='VOKASIONAL_ULANG'"
                Dim Gred As String = oCommon.getFieldValue(strSQL)

                strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET Gred='" & Gred & "' Where PelajarUlangVID='" & datRespondent.DataKeys(i).Value.ToString & "'"
                strSQL += " AND Tahun='" & ddlTahun.Text & "' AND Semester='" & ddlSemester.Text & "'"
                strSQL += " AND Sesi='" & chkSesi.Text & "' AND KursusID='" & ddlKodKursus.SelectedValue & "' AND NamaModul='" & strModul & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

                If Gred = strGredMarkah Then
                    strSQL = "UPDATE kpmkv_pelajar_markah SET GredV" & strKodModul & "='" & strGredMarkah & "' Where PelajarID='" & strPelajarID & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                ElseIf Gred = "B-" Then
                    strSQL = "UPDATE kpmkv_pelajar_markah SET GredV" & strKodModul & "='B-' Where PelajarID='" & strPelajarID & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    strSQL = "UPDATE kpmkv_pelajar_markah SET GredV" & strKodModul & "='" & strGredMarkah & "' Where PelajarID='" & strPelajarID & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                End If
            End If


        Next
    End Sub

End Class
