﻿Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class kelas_calon_year
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""
    Dim IntTakwim As Integer = 0

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Dim SubMenuText As String = "Penetapan Kelas Calon"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then

                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                '------exist takwim
                strSQL = "SELECT * FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText = '" & SubMenuText & "' AND Aktif='1' AND GETDATE() BETWEEN CONVERT(date, TarikhMula, 103) AND DATEADD(day,1,CONVERT(date, TarikhAkhir, 103))"

                If oCommon.isExist(strSQL) = True Then

                    chkSesi.Enabled = True

                    kpmkv_tahun_list()
                    kpmkv_semester_list()
                    kpmkv_kodkursus_list()
                    kpmkv_kelas_list()

                    btnCreate.Enabled = True

                Else

                    btnCreate.Enabled = False
                    lblMsg.Text = "Penetapan Kelas Calon telah ditutup!"

                End If

            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try
    End Sub
    Private Sub tbl_menu_check()

        Dim str As String
        For i As Integer = 0 To datRespondent.Rows.Count - 1
            Dim row As GridViewRow = datRespondent.Rows(0)
            Dim cb As CheckBox = datRespondent.Rows(i).FindControl("chkSelect")

            str = datRespondent.DataKeys(i).Value.ToString
            Dim strMykad As String = CType(datRespondent.Rows(i).FindControl("Mykad"), Label).Text

            strSQL = "SELECT KelasID FROM kpmkv_pelajar Where Mykad='" & strMykad & "' AND IsDeleted='N' AND KelasID IS NOT NULL"
            If oCommon.isExist(strSQL) = False Then
                cb.Checked = True
            End If
        Next

    End Sub
    Private Sub kpmkv_tahun_list()

        strSQL = "  SELECT DISTINCT kpmkv_takwim.Kohort FROM kpmkv_takwim
                    LEFT JOIN kpmkv_takwim_kv ON kpmkv_takwim_kv.TakwimID = kpmkv_takwim.TakwimID
                    WHERE
                    kpmkv_takwim.SubMenuText = '" & SubMenuText & "'
                    AND kpmkv_takwim.Aktif = '1'
                    AND kpmkv_takwim.Tahun = '" & Now.Year & "'
                    AND kpmkv_takwim_kv.TakwimKVID IS NULL
                    AND GETDATE() BETWEEN CONVERT(date, TarikhMula, 103) AND DATEADD(day,1,CONVERT(date, TarikhAkhir, 103))

                    UNION

                    SELECT DISTINCT kpmkv_takwim.Kohort FROM kpmkv_takwim
                    LEFT JOIN kpmkv_takwim_kv ON kpmkv_takwim_kv.TakwimID = kpmkv_takwim.TakwimID
                    WHERE
                    kpmkv_takwim.SubMenuText = '" & SubMenuText & "'
                    AND kpmkv_takwim.Aktif = '1'
                    AND kpmkv_takwim.Tahun = '" & Now.Year & "'
                    AND kpmkv_takwim_kv.TakwimKVID IS NOT NULL
                    AND kpmkv_takwim_kv.KolejRecordID = '" & lblKolejID.Text & "'
                    AND GETDATE() BETWEEN CONVERT(date, TarikhMula, 103) AND DATEADD(day,1,CONVERT(date, TarikhAkhir, 103))"

        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlTahun.DataSource = ds
            ddlTahun.DataTextField = "Kohort"
            ddlTahun.DataValueField = "Kohort"
            ddlTahun.DataBind()

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_semester_list()

        strSQL = "  SELECT DISTINCT Semester FROM kpmkv_takwim 
                    WHERE 
                    SubMenuText = '" & SubMenuText & "' 
                    AND Aktif = '1'
                    AND Tahun = '" & Now.Year & "'
                    AND Kohort = '" & ddlTahun.Text & "'
                    AND GETDATE() BETWEEN CONVERT(date, TarikhMula, 103) AND DATEADD(day,1,CONVERT(date, TarikhAkhir, 103))"

        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlSemester.DataSource = ds
            ddlSemester.DataTextField = "Semester"
            ddlSemester.DataValueField = "Semester"
            ddlSemester.DataBind()

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
    Protected Sub btnCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCreate.Click
        lblMsgTop.Text = ""

        If ddlKelas.Text = "0" Then
            DivMsgTop.Attributes("class") = "error"
            lblMsgTop.Text = "Sila pilih Nama Kelas"
            Exit Sub
        End If

        Dim strKey As String
        Try

            For i As Integer = 0 To datRespondent.Rows.Count - 1
                Dim row As GridViewRow = datRespondent.Rows(i)
                Dim isChecked As Boolean = DirectCast(row.FindControl("chkSelect"), CheckBox).Checked

                strKey = datRespondent.DataKeys(i).Value.ToString

                If isChecked Then
                    'validate

                    strSQL = "UPDATE kpmkv_pelajar SET  KelasID ='" & ddlKelas.SelectedValue & "' WHERE PelajarID='" & strKey & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                    If strRet = "0" Then
                        DivMsgTop.Attributes("class") = "info"
                        lblMsgTop.Text = "Penetapan calon berjaya didaftarkan"
                    Else
                        lblMsgTop.Text = "System Error:" & strRet
                    End If
                End If
            Next

        Catch ex As Exception
            divMsg.Attributes("class") = "error"

        End Try
        strRet = BindData(datRespondent)
        tbl_menu_check()

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
        Dim strOrder As String = " ORDER BY kpmkv_pelajar.Tahun, kpmkv_pelajar.Semester, kpmkv_pelajar.Sesi ASC"

        tmpSQL = "SELECT kpmkv_pelajar.PelajarID, kpmkv_pelajar.Tahun, kpmkv_pelajar.Semester, kpmkv_pelajar.Sesi, kpmkv_pelajar.Nama, kpmkv_pelajar.MYKAD, kpmkv_pelajar.AngkaGiliran, "
        tmpSQL += " kpmkv_kursus.KodKursus, kpmkv_kelas.NamaKelas"
        tmpSQL += " FROM  kpmkv_pelajar LEFT OUTER JOIN kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
        tmpSQL += " kpmkv_status ON kpmkv_pelajar.StatusID = kpmkv_status.StatusID LEFT OUTER JOIN kpmkv_kelas ON kpmkv_pelajar.KelasID = kpmkv_kelas.KelasID"
        strWhere = " WHERE kpmkv_pelajar.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.StatusID='2'"

        'tahun
        If Not ddlTahun.Text = "0" Then
            strWhere += " AND kpmkv_pelajar.Tahun='" & ddlTahun.Text & "'"
        End If

        '--Semester
        If Not ddlSemester.Text = "0" Then
            strWhere += " AND kpmkv_pelajar.Semester='" & ddlSemester.SelectedValue & "'"
        End If

        '--Kod
        If Not ddlKodKursus.Text = "0" Then
            strWhere += " AND kpmkv_pelajar.KursusID='" & ddlKodKursus.SelectedValue & "'"
        End If

        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        'Response.Write(getSQL)

        Return getSQL

    End Function

    Private Sub datRespondent_PageIndexChanged(sender As Object, e As EventArgs) Handles datRespondent.PageIndexChanged
        datRespondent.DataBind()
        tbl_menu_check()
    End Sub
    Private Sub datRespondent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondent.PageIndexChanging
        datRespondent.PageIndex = e.NewPageIndex
        strRet = BindData(datRespondent)
        tbl_menu_check()
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
        strRet = BindData(datRespondent)
        tbl_menu_check()
    End Sub
    Protected Sub chkSesi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkSesi.SelectedIndexChanged
        kpmkv_kodkursus_list()
        kpmkv_kelas_list()
    End Sub

    Private Sub ddlKodKursus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlKodKursus.SelectedIndexChanged
        kpmkv_kelas_list()
    End Sub

    Protected Sub CheckUncheckAll(sender As Object, e As System.EventArgs)
        Dim chk1 As CheckBox
        chk1 = DirectCast(datRespondent.HeaderRow.Cells(0).FindControl("chkAll"), CheckBox)
        For Each row As GridViewRow In datRespondent.Rows
            Dim chk As CheckBox
            chk = DirectCast(row.Cells(0).FindControl("chkSelect"), CheckBox)
            chk.Checked = chk1.Checked
        Next
    End Sub

    Protected Sub btnBatal_Click(sender As Object, e As EventArgs) Handles btnBatal.Click
        lblMsgTop.Text = ""

        'If ddlKelas.Text = "0" Then
        '    DivMsgTop.Attributes("class") = "error"
        '    lblMsgTop.Text = "Sila pilih Nama Kelas"
        '    Exit Sub
        'End If

        Dim strKey As String
        Try

            For i As Integer = 0 To datRespondent.Rows.Count - 1
                Dim row As GridViewRow = datRespondent.Rows(i)
                Dim isChecked As Boolean = DirectCast(row.FindControl("chkSelect"), CheckBox).Checked

                strKey = datRespondent.DataKeys(i).Value.ToString

                If isChecked Then
                    'validate

                    strSQL = "UPDATE kpmkv_pelajar SET  KelasID ='' WHERE PelajarID='" & strKey & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                    If strRet = "0" Then
                        DivMsgTop.Attributes("class") = "info"
                        lblMsgTop.Text = "Pembatalan calon berjaya dibatalkan"
                    Else
                        lblMsgTop.Text = "System Error:" & strRet
                    End If
                End If
            Next

        Catch ex As Exception
            divMsg.Attributes("class") = "error"

        End Try
        strRet = BindData(datRespondent)
        tbl_menu_check()
    End Sub

    Private Sub ddlTahun_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTahun.SelectedIndexChanged
        kpmkv_semester_list()
        kpmkv_kodkursus_list()
        kpmkv_kelas_list()
    End Sub

    Private Sub ddlSemester_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSemester.SelectedIndexChanged
        kpmkv_kodkursus_list()
        kpmkv_kelas_list()
    End Sub
End Class