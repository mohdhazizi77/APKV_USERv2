Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Public Class bmsetara_calon_ulang_daftar
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""
    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then

                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)

                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                kpmkv_tahun_list()
                ddlTahun.Text = "-Pilih-"


                kpmkv_kelas_list()

                kpmkv_kodkursus_list()

            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try
    End Sub

    Private Sub kpmkv_tahun_list()
        strSQL = "SELECT Tahun FROM kpmkv_tahun ORDER BY TahunID"
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

            ddlTahun.Items.Insert(0, "-Pilih-")

        Catch ex As Exception

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Sub kpmkv_kodkursus_list()

        strSQL = "SELECT kpmkv_kursus.KodKursus, kpmkv_kursus.KursusID FROM kpmkv_kursus_kolej LEFT OUTER JOIN"
        strSQL += " kpmkv_kursus ON kpmkv_kursus_kolej.KursusID = kpmkv_kursus.KursusID"
        strSQL += " WHERE kpmkv_kursus_kolej.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kursus.Tahun='" & ddlTahun.SelectedValue & "' "
        strSQL += " AND kpmkv_kursus.Sesi='" & chkSesi.SelectedValue & "' GROUP BY kpmkv_kursus.KodKursus, kpmkv_kursus.KursusID ORDER BY kpmkv_kursus.KodKursus"
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

            ddlKodKursus.Items.Insert(0, "-Pilih-")


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
        strSQL += " WHERE kpmkv_kelas.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kelas_kursus.KursusID= '" & ddlKodKursus.SelectedValue & "' ORDER BY  kpmkv_kelas.NamaKelas"
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

            ddlNamaKelas.Items.Insert(0, "-Pilih-")


        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Sub Year()

        For i As Integer = ddlTahun.Text To Now.Year
            ddlTahunSemasa.Items.Add(i.ToString())
        Next
        ddlTahunSemasa.Items.FindByValue(System.DateTime.Now.Year.ToString()).Selected = True

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
        tmpSQL = "SELECT kpmkv_pelajar.PelajarID, kpmkv_pelajar.Tahun, kpmkv_pelajar.Semester, kpmkv_pelajar.Sesi, "
        tmpSQL += " kpmkv_pelajar.Nama, kpmkv_pelajar.MYKAD, kpmkv_pelajar.AngkaGiliran, "
        tmpSQL += " kpmkv_kursus.KodKursus"
        tmpSQL += " FROM  kpmkv_pelajar "
        tmpSQL += " LEFT OUTER JOIN kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID"
        tmpSQL += " LEFT OUTER JOIN kpmkv_kluster ON kpmkv_kursus.KlusterID=kpmkv_kluster.KlusterID"
        tmpSQL += " LEFT OUTER JOIN kpmkv_status ON kpmkv_pelajar.StatusID = kpmkv_status.StatusID "
        tmpSQL += " LEFT OUTER JOIN kpmkv_kelas ON kpmkv_pelajar.KelasID = kpmkv_kelas.KelasID"
        strWhere = " WHERE kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.StatusID='2' AND kpmkv_pelajar.KelasID IS NOT NULL"
        strWhere += " AND kpmkv_pelajar.KolejRecordID='" & lblKolejID.Text & "'"
        strWhere += " AND kpmkv_pelajar.Tahun ='" & ddlTahun.Text & "' "
        strWhere += " AND kpmkv_pelajar.Semester ='" & ddlsemester.SelectedValue & "'"
        strWhere += " AND kpmkv_pelajar.Sesi='" & chkSesi.Text & "'"

        If ddlmp.SelectedValue = "BM" Then
            strWhere += " AND kpmkv_pelajar.IsCalon='1'"

        ElseIf ddlmp.SelectedValue = "SJ" Then
            strWhere += " AND kpmkv_pelajar.IsSJCalon='1'"
        End If



        '--kodkursus
        If Not ddlKodKursus.Text = "" Then
            strWhere += " AND kpmkv_pelajar.KursusID ='" & ddlKodKursus.SelectedValue & "'"
        End If

        '--txtNama
        If Not txtNama.Text.Length = 0 Then
            strWhere += " AND kpmkv_pelajar.Nama LIKE '%" & oCommon.FixSingleQuotes(txtNama.Text) & "%'"
        End If

        '--txtMYKAD
        If Not txtMYKAD.Text.Length = 0 Then
            strWhere += " AND kpmkv_pelajar.MYKAD='" & oCommon.FixSingleQuotes(txtMYKAD.Text) & "'"
        End If

        getSQL = tmpSQL & strWhere & strOrder

        Return getSQL

    End Function

    Private Sub status_daftar()

        For i = 0 To datRespondent2.Rows.Count - 1

            Dim strkey As String = datRespondent2.DataKeys(i).Value.ToString

            Dim lblstatusBM As Label = datRespondent2.Rows(i).FindControl("lblStatusBM")
            Dim lblstatusSJ As Label = datRespondent2.Rows(i).FindControl("lblStatusSJ")


            strSQL = "SELECT isCalon FROM kpmkv_markah_bmsj_setara WHERE PelajarID = '" & strkey & "' AND MataPelajaran = 'BAHASA MELAYU' AND Tahun = '" & ddlTahun.Text & "' AND KodKursus = '" & ddlKodKursus.SelectedValue & "'"
            Dim statusBM As String = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT isCalon FROM kpmkv_markah_bmsj_setara WHERE PelajarID = '" & strkey & "' AND MataPelajaran = 'SEJARAH' AND Tahun = '" & ddlTahun.Text & "' AND KodKursus = '" & ddlKodKursus.SelectedValue & "'"
            Dim statusSJ As String = oCommon.getFieldValue(strSQL)

            If statusBM = "1" Then

                lblstatusBM.Text = "Telah Daftar"

            End If

            If statusSJ = "1" Then

                lblstatusSJ.Text = "Telah Daftar"

            End If

        Next

    End Sub

    Private Sub datRespondent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondent2.PageIndexChanging
        datRespondent2.PageIndex = e.NewPageIndex
        strRet = BindData(datRespondent2)

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

    Protected Sub OnConfirm(ByVal sender As Object, ByVal e As EventArgs) Handles btnConfirm.Click
        Dim confirmValue As String = Request.Form("confirm_value")
        If confirmValue = "Yes" Then
            returnconfirm()
            status_daftar()
        Else
            strRet = BindData(datRespondent2)
        End If
    End Sub

    Protected Sub CheckUncheckAll(sender As Object, e As System.EventArgs)
        Dim chk1 As CheckBox
        chk1 = DirectCast(datRespondent2.HeaderRow.Cells(0).FindControl("chkAll"), CheckBox)
        For Each row As GridViewRow In datRespondent2.Rows
            Dim chk As CheckBox
            chk = DirectCast(row.Cells(0).FindControl("chkSelect"), CheckBox)
            chk.Checked = chk1.Checked
        Next
    End Sub

    Private Sub returnconfirm()

        Try
            For i As Integer = 0 To datRespondent2.Rows.Count - 1

                Dim cb As CheckBox = datRespondent2.Rows(i).FindControl("chkSelect")

                If cb.Checked = True Then


                    Dim strkey As String = datRespondent2.DataKeys(i).Value.ToString

                    Dim strMp As String = ""

                    If ddlmp.SelectedValue = "BM" Then
                        strMp = "BAHASA MELAYU"
                    ElseIf ddlmp.SelectedValue = "SJ" Then
                        strMp = "SEJARAH"

                    End If

                    strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang WHERE PelajarID='" & strkey & "'"
                    strSQL += " AND Tahun='" & ddlTahun.SelectedValue & "' AND Semester='" & ddlsemester.SelectedValue & "' "
                    strSQL += " AND Sesi='" & chkSesi.Text & "'"
                    strSQL += " AND NamaMataPelajaran='" & strMp & "'"

                    If ddlmp.SelectedValue = "BM" Then
                        strSQL += " AND IsCalon='1'"
                        strSQL += " AND IsBMTahun ='" & ddlTahunSemasa.SelectedValue & "'"

                    ElseIf ddlmp.SelectedValue = "SJ" Then
                        strSQL += " AND IsSJCalon='1'"
                        strSQL += " AND IsSJTahun ='" & ddlTahunSemasa.SelectedValue & "'"
                    End If

                    strRet = oCommon.isExist(strSQL)

                    If strRet = True Then
                        divMsg.Attributes("class") = "error"
                        lblMsg.Text = "Data Calon Sudah wujud!"

                        Exit Sub
                    End If

                    strSQL = "SELECT Pengajian FROM kpmkv_pelajar WHERE PelajarID='" & strkey & "' "
                    Dim strPengajian As String = oCommon.getFieldValue(strSQL)

                    strSQL = "  SELECT PelajarID FROM kpmkv_markah_bmsj_setara WHERE PelajarID = '" & strkey & "' AND MataPelajaran = '" & strMp & "' AND Tahun = '" & ddlTahun.SelectedValue & "' AND Sesi = '" & chkSesi.Text & "'"
                    strRet = oCommon.isExist(strSQL)

                    If strRet = True Then

                        divMsg.Attributes("class") = "error"
                        lblMsg.Text = "Data Calon Sudah wujud!"

                        Exit Sub

                    End If

                    strSQL = "  INSERT INTO kpmkv_markah_bmsj_setara 
                               (PelajarID, KolejRecordID, Tahun, Sesi, Kodkursus, MataPelajaran, IsCalon, IsAKATahun, IsAKASesi, IsAKADated)
                                VALUES
                               ('" & strkey & "', '" & lblKolejID.Text & "', '" & ddlTahun.SelectedValue & "', '" & chkSesi.Text & "', '" & ddlKodKursus.SelectedValue & "', '" & strMp & "', '1', '" & ddlTahunSemasa.SelectedValue & "', '1',  GETDATE() )"

                    strRet = oCommon.ExecuteSQL(strSQL)

                    If ddlmp.SelectedValue = "BM" Then

                        strSQL = " UPDATE kpmkv_pelajar SET IsBMUlang = '1' WHERE PelajarID = '" & strkey & "'"

                    ElseIf ddlmp.SelectedValue = "SJ" Then

                        strSQL = " UPDATE kpmkv_pelajar SET IsSJUlang = '1' WHERE PelajarID = '" & strkey & "'"

                    End If

                    strRet = oCommon.ExecuteSQL(strSQL)

                    'strSQL = " INSERT INTO kpmkv_pelajar_ulang (PelajarID,KolejRecordID,Pengajian,Tahun,Semester,Sesi,"
                    'strSQL += " KursusID,KelasID,NamaMataPelajaran,PA,"
                    'If ddlmp.SelectedValue = "BM" Then
                    '    strSQL += " IsCalon,IsBMTahun,IsBMDated"
                    'ElseIf ddlmp.SelectedValue = "SJ" Then
                    '    strSQL += " IsSJCalon,IsSJTahun,IsSJDated"
                    'End If

                    'strSQL += " )"

                    'strSQL += " VALUES('" & strkey & "','" & lblKolejID.Text & "','" & strPengajian & "' ,"
                    'strSQL += " '" & ddlTahun.SelectedValue & "' ,'" & ddlsemester.SelectedValue & "','" & chkSesi.Text & "' ,"
                    'strSQL += " '" & ddlKodKursus.SelectedValue & "','" & ddlNamaKelas.SelectedValue & "','" & strMp & "',"
                    'strSQL += "'1','1','" & ddlTahunSemasa.SelectedValue & "','" & Date.Now.ToString("yyyy/MM/dd") & "'"

                    'strSQL += " )"



                    'strRet = oCommon.ExecuteSQL(strSQL)
                    If strRet = "0" Then
                        divMsg.Attributes("class") = "info"
                        lblMsg.Text = "Berjaya! Daftar Calon Ulang Berjaya"

                        divMsgResult.Attributes("class") = "info"
                        lblMsgResult.Text = "Berjaya! Daftar Calon Ulang Berjaya"
                    Else
                        divMsg.Attributes("class") = "error"
                        lblMsg.Text = "Tidak Berjaya!Daftar Calon Ulang Tidak Berjaya"

                        divMsgResult.Attributes("class") = "error"
                        lblMsgResult.Text = "Tidak Berjaya! Daftar Calon Ulang Tidak Berjaya"
                    End If
                End If

            Next
            divMsg.Attributes("class") = "info"
            lblMsg.Text = "Berjaya! Daftar Calon Ulang  Berjaya"

            divMsgResult.Attributes("class") = "info"
            lblMsgResult.Text = "Berjaya! Daftar Calon Ulang Berjaya"
        Catch ex As Exception
            divMsg.Attributes("class") = "error"
            lblMsg.Text = "System Error. " & ex.Message
        End Try

    End Sub

    Private Sub btnCari_Click(sender As Object, e As EventArgs) Handles btnCari.Click
        lblMsg.Text = ""
        strRet = BindData(datRespondent2)
        status_daftar()
        Year()
    End Sub

    Protected Sub chkSesi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkSesi.SelectedIndexChanged
        kpmkv_kodkursus_list()
        kpmkv_kelas_list()
    End Sub

    Private Sub ddlKodKursus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlKodKursus.SelectedIndexChanged
        kpmkv_kelas_list()
    End Sub

    Private Sub ddlTahun_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTahun.SelectedIndexChanged
        kpmkv_kodkursus_list()
    End Sub
End Class