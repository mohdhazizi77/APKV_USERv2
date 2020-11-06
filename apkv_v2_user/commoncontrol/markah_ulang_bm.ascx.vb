Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient

Public Class markah_ulang_bm1
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String
    Dim strRet As String
    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then

                chkSesi.SelectedIndex = 0

                lblMsg.Text = ""

            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try
    End Sub

    'Private Sub kpmkv_tahun_list()
    '    strSQL = "SELECT Tahun FROM kpmkv_tahun  ORDER BY Tahun"
    '    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    '    Dim objConn As SqlConnection = New SqlConnection(strConn)
    '    Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

    '    Try
    '        Dim ds As DataSet = New DataSet
    '        sqlDA.Fill(ds, "AnyTable")

    '        ddlTahun.DataSource = ds
    '        ddlTahun.DataTextField = "Tahun"
    '        ddlTahun.DataValueField = "Tahun"
    '        ddlTahun.DataBind()

    '        '--ALL
    '        ddlTahun.Items.Add(New ListItem("-Pilih-", "-Pilih-"))

    '    Catch ex As Exception
    '        lblMsg.Text = "System Error:" & ex.Message

    '    Finally
    '        objConn.Dispose()
    '    End Try

    'End Sub
    'Private Sub kpmkv_semester_list()
    '    strSQL = "SELECT Semester FROM kpmkv_semester  ORDER BY Semester"
    '    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    '    Dim objConn As SqlConnection = New SqlConnection(strConn)
    '    Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

    '    Try
    '        Dim ds As DataSet = New DataSet
    '        sqlDA.Fill(ds, "AnyTable")

    '        ddlSemester.DataSource = ds
    '        ddlSemester.DataTextField = "Semester"
    '        ddlSemester.DataValueField = "Semester"
    '        ddlSemester.DataBind()

    '        ddlSemester.Items.Add(New ListItem("-Pilih-", "-Pilih-"))

    '    Catch ex As Exception
    '        lblMsg.Text = "System Error:" & ex.Message

    '    Finally
    '        objConn.Dispose()
    '    End Try

    'End Sub

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

    Private Function getSQL() As String

        strSQL = "SELECT RecordID FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
        Dim strRecordID As String = oCommon.getFieldValue(strSQL)

        strSQL = "  SELECT 
                    kpmkv_pelajar.PelajarID,
                    kpmkv_pelajar.Mykad,
                    kpmkv_pelajar.AngkaGiliran,
                    kpmkv_markah_bmsj_setara.Kertas3,
                    kpmkv_markah_bmsj_setara.Catatan3
                    FROM 
                    kpmkv_markah_bmsj_setara 
                    LEFT OUTER JOIN kpmkv_pelajar ON kpmkv_pelajar.PelajarID = kpmkv_markah_bmsj_setara.PelajarID
                    WHERE 
                    kpmkv_pelajar.MYKAD = '" & txtMykad.Text & "'
                    OR kpmkv_pelajar.AngkaGiliran = '" & txtAngkaGiliran.Text & "'
                    AND kpmkv_markah_bmsj_setara.IsCalon='1' 
                    AND kpmkv_pelajar.Semester = '4' 
                    AND kpmkv_pelajar.IsBMUlang = '1'
                    AND kpmkv_markah_bmsj_setara.MataPelajaran = 'BAHASA MELAYU'
                    AND kpmkv_pelajar.KolejRecordID = '" & strRecordID & "'
                    --AND kpmkv_markah_bmsj_setara.IsAKATahun = '" & Now.Year & "'"

        getSQL = strSQL
        '--debug
        Debug.Write(getSQL)

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
        lblMsg2.Text = ""

        strSQL = "SELECT RecordID FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
        Dim strRecordID As String = oCommon.getFieldValue(strSQL)

        Try
            If ValidateForm() = False Then
                lblMsg.Text = "Sila masukkan NOMBOR SAHAJA. 0-130"
                lblMsg2.Text = "Sila masukkan NOMBOR SAHAJA. 0-130"
                Exit Sub
            End If

            For i As Integer = 0 To datRespondent.Rows.Count - 1
                Dim row As GridViewRow = datRespondent.Rows(i)
                Dim strPelajarID As Integer = datRespondent.DataKeys(i).Value.ToString
                Dim Kertas3 As TextBox = datRespondent.Rows(i).FindControl("txtKertas3")
                Dim Catatan3 As TextBox = datRespondent.Rows(i).FindControl("txtCatatan3")

                'assign value to integer
                Dim BM3 As String = Kertas3.Text
                Dim strCatatan As String = Catatan3.Text

                strSQL = "  UPDATE kpmkv_markah_bmsj_setara SET Kertas3 = '" & BM3 & "', Catatan3 = '" & strCatatan & "' 
                            WHERE PelajarID = '" & strPelajarID & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

                'strSQL = "UPDATE kpmkv_pelajar_markah SET A_BahasaMelayu1='" & BM3 & "', Catatan='" & strCatatan & "' WHERE kpmkv_pelajar_markah.PelajarID='" & strPelajarID & "'"
                'strSQL += " AND kpmkv_pelajar_markah.KolejRecordID='" & lblKolejRecorID.Text & "'"
                'strRet = oCommon.ExecuteSQL(strSQL)

            Next
            divMsg2.Attributes("class") = "info"
            lblMsg2.Text = "Berjaya!.Kemaskini markah BM Setara Kertas 3."

            divMsg.Attributes("class") = "info"
            lblMsg.Text = "Berjaya!.Kemaskini markah BM Setara Kertas 3."

            strRet = BindData(datRespondent)

        Catch ex As Exception
            divMsg.Attributes("class") = "Error:" & ex.Message
            lblMsg.Text = "Tidak Berjaya!.Kemaskini markah BM Setara Kertas 3."

            divMsg2.Attributes("class") = "Error:" & ex.Message
            lblMsg2.Text = "Tidak Berjaya!.Kemaskini markah BM Setara Kertas 3."
        End Try


        ' Akademik_markah()
        'Akademik_gred()
    End Sub

    Private Function ValidateForm() As Boolean
        For i As Integer = 0 To datRespondent.Rows.Count - 1

            Dim Kertas3 As TextBox = datRespondent.Rows(i).FindControl("txtKertas3")
            Dim Catatan3 As TextBox = datRespondent.Rows(i).FindControl("txtCatatan3")

            If Not Kertas3.Text.Length = 0 Then
                If oCommon.IsCurrency(Kertas3.Text) = False Then
                    Return False
                End If
                If CInt(Kertas3.Text) > 100 Then
                    Return False
                End If
                If CInt(Kertas3.Text) = -1 Then
                    Return False
                End If
            Else
                Kertas3.Text = "0"
            End If

        Next

        Return True

    End Function

    Protected Sub btnCari_Click(sender As Object, e As EventArgs) Handles btnCari.Click
        lblMsg.Text = ""

        strRet = BindData(datRespondent)

    End Sub

End Class