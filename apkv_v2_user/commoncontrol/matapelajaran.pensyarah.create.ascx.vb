Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class matapelajaran_pensyarah_create
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then

                kpmkv_kolej_list()
                ddlKodKolej.Text = "PILIH"

                LoadPage()
                tbl_menu_check()
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try
    End Sub
    Private Sub kpmkv_kolej_list()
        strSQL = "SELECT Nama,RecordID FROM kpmkv_kolej WITH (NOLOCK) WHERE RecordID=(SELECT Distinct KolejRecordID from kpmkv_kolej_kursus)ORDER BY Nama"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlKodKolej.DataSource = ds
            ddlKodKolej.DataTextField = "Nama"
            ddlKodKolej.DataValueField = "RecordID"
            ddlKodKolej.DataBind()

            '--ALL
            ddlKodKolej.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub LoadPage()
        strSQL = "SELECT * FROM kpmkv_matapelajaran WHERE MataPelajaranID='" & Request.QueryString("MataPelajaranID") & "'"
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

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("NamaKluster")) Then
                    lblNamaKluster.Text = ds.Tables(0).Rows(0).Item("NamaKluster")
                Else
                    lblNamaKluster.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("KodKursus")) Then
                    lblKodKursus.Text = ds.Tables(0).Rows(0).Item("KodKursus")
                Else
                    lblKodKursus.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Namakursus")) Then
                    lblNamaKursus.Text = ds.Tables(0).Rows(0).Item("NamaKursus")
                Else
                    lblNamaKursus.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Tahun")) Then
                    lblTahun.Text = ds.Tables(0).Rows(0).Item("Tahun")
                Else
                    lblTahun.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("KodMataPelajaran")) Then
                    lblKod.Text = ds.Tables(0).Rows(0).Item("KodMataPelajaran")
                Else
                    lblKod.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("NamaMataPelajaran")) Then
                    lblNama.Text = ds.Tables(0).Rows(0).Item("NamaMataPelajaran")
                Else
                    lblNama.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("JamKredit")) Then
                    lblJamKredit.Text = ds.Tables(0).Rows(0).Item("JamKredit")
                Else
                    lblJamKredit.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Sesi")) Then
                    lblSesi.Text = ds.Tables(0).Rows(0).Item("Sesi")
                Else
                    lblSesi.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Semester")) Then
                    lblSemester.Text = ds.Tables(0).Rows(0).Item("Semester")
                Else
                    lblSemester.Text = ""
                End If

            End If

        Catch ex As Exception
            divMsg.Attributes("class") = "error"
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        lblMsg.Text = ""
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
        Dim strOrder As String = " ORDER BY Nama,MYKAD ASC"


        '-not deleted
        tmpSQL = "SELECT * FROM kpmkv_pensyarah"
        strWhere = " WITH (NOLOCK) WHERE IsDeleted='N'"

        '--KodKolej
        If Not ddlKodKolej.Text = "PILIH" Then
            '-not deleted
            tmpSQL = "SELECT * FROM kpmkv_pensyarah"
            strWhere = " WITH (NOLOCK) WHERE IsDeleted='N'"
            strWhere += " AND KolejRecordID= '" & ddlKodKolej.SelectedValue & "'"
        ElseIf Not txtKod.Text.Length = 0 Then
            tmpSQL = "SELECT * FROM kpmkv_pensyarah"
            strWhere = " WITH (NOLOCK) WHERE IsDeleted='N'AND KolejRecordID=(Select RecordID From kpmkv_kolej where Nama LIKE '%" & oCommon.FixSingleQuotes(txtKod.Text) & "%')"
        End If

        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        ''Response.Write(getSQL)

        Return getSQL

    End Function
    Private Sub datRespondent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondent.PageIndexChanging
        datRespondent.PageIndex = e.NewPageIndex
        strRet = BindData(datRespondent)

    End Sub
    Private Sub datRespondent_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles datRespondent.SelectedIndexChanging
        Dim strKeyID As String = datRespondent.DataKeys(e.NewSelectedIndex).Value.ToString
        Response.Redirect("pensyarah.view.aspx?KolejRecordID=" & strKeyID)

    End Sub
    Private Sub tbl_menu_check()

        Dim str As String
        For i As Integer = 0 To datRespondent.Rows.Count - 1
            Dim row As GridViewRow = datRespondent.Rows(0)
            Dim cb As CheckBox = datRespondent.Rows(i).FindControl("chkSelect")

            Dim KolejRecord As String = ""

            str = datRespondent.DataKeys(i).Value.ToString
            strSQL = "Select KolejRecordID from kpmkv_pensyarah where Nama='" & str & "'"
            KolejRecord = oCommon.getFieldValue(strSQL)

            strSQL = "Select * from kpmkv_matapelajaran_pensyarah where NamaPensyarah='" & str & "' and Tahun='" & lblTahun.Text & "' and KodMataPelajaran='" & lblKod.Text & "' and KolejRecordID='" & KolejRecord & "'"
            strRet = oCommon.isExist(strSQL)
            If strRet = True Then
                cb.Checked = True
            End If
            ' End If
        Next


    End Sub
    Protected Sub btnCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCreate.Click
        lblMsg.Text = ""
        Dim str As String
        Try

            For i As Integer = 0 To datRespondent.Rows.Count - 1
                Dim row As GridViewRow = datRespondent.Rows(i)
                Dim isChecked As Boolean = DirectCast(row.FindControl("chkSelect"), CheckBox).Checked

                Dim KolejRecord As String = ""

                str = datRespondent.DataKeys(i).Value.ToString
                strSQL = "Select KolejRecordID from kpmkv_pensyarah where Nama='" & str & "'"
                KolejRecord = oCommon.getFieldValue(strSQL)

                If isChecked Then
                    strSQL = "Select * from kpmkv_matapelajaran_pensyarah where NamaPensyarah='" & str & "' and Tahun='" & lblTahun.Text & "' and KodMataPelajaran='" & lblKod.Text & "' and KolejRecordID='" & KolejRecord & "'"
                    If oCommon.isExist(strSQL) = False Then
                        strSQL = "INSERT INTO kpmkv_matapelajaran_pensyarah (KolejRecordID,NamaKluster,Tahun,KodKursus,NamaKursus,Semester,Sesi,KodMataPelajaran,NamaMataPelajaran,JamKredit,NamaPensyarah)"
                        strSQL += "VALUES ('" & KolejRecord & "','" & lblNamaKluster.Text & "','" & lblTahun.Text & "','" & lblKodKursus.Text & "','" & lblNamaKursus.Text & "','" & lblSemester.Text & "','" & lblSesi.Text & "','" & lblKod.Text & "','" & lblNama.Text & "','" & lblJamKredit.Text & "','" & str & "')"
                        strRet = oCommon.ExecuteSQL(strSQL)
                    End If
                End If
            Next

        Catch ex As Exception
            divMsg.Attributes("class") = "error"

        End Try

    End Sub

    Protected Sub lnkList_Click(sender As Object, e As EventArgs) Handles lnkList.Click
        Response.Redirect("matapelajaran.list.aspx")
    End Sub
End Class