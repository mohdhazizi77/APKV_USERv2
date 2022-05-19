Imports System.Data.SqlClient

Public Class pentaksir_bmsetara_penyerahan_markah_bm41
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String
    Dim strRet As String
    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then

                strSQL = "SELECT MataPelajaran FROM kpmkv_pentaksir_bmsetara WHERE id = '" & Request.QueryString("id") & "'"
                lblMP.Text = oCommon.getFieldValue(strSQL)

                strSQL = "SELECT KolejRecordID FROM kpmkv_pentaksir_bmsetara WHERE id = '" & Request.QueryString("id") & "'"
                Dim KolejRecordID As String = oCommon.getFieldValue(strSQL)

                strSQL = "SELECT Nama FROM kpmkv_kolej WHERE RecordID = '" & KolejRecordID & "'"
                lblPP.Text = oCommon.getFieldValue(strSQL)

                lblMsg.Text = ""
                strRet = BindData(datRespondent)
                MarkahElemen()
                StatusHantar()

            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try
    End Sub

    Private Sub StatusHantar()

        strSQL = "SELECT UserID FROM kpmkv_users WHERE LoginID = '" & Session("LoginID") & "' AND Pwd = '" & Session("Password") & "'"
        Dim UserID As String = oCommon.getFieldValue(strSQL)

        strSQL = "SELECT KolejRecordID FROM kpmkv_pentaksir_bmsetara WHERE id = '" & Request.QueryString("id") & "'"
        Dim KolejRecordID As String = oCommon.getFieldValue(strSQL)

        strSQL = "SELECT Kohort FROM kpmkv_pentaksir_bmsetara WHERE id = '" & Request.QueryString("id") & "'"
        Dim Tahun As String = oCommon.getFieldValue(strSQL)

        strSQL = "  SELECT id FROM kpmkv_pentaksir_bmsetara_calon
                    WHERE StatusSerahBM4KJPP = 'BELUM SERAH'
                    AND KolejRecordID = '" & KolejRecordID & "'
                    AND Tahun = '" & Tahun & "'"
        strRet = oCommon.getFieldValue(strSQL)

        If strRet = "" Then

            lblBelumSerah.Visible = False

        End If

    End Sub

    Private Sub MarkahElemen()

        For i As Integer = 0 To datRespondent.Rows.Count - 1

            Dim strKey As String = datRespondent.DataKeys(i).Value.ToString
            Dim lblME As Label = CType(datRespondent.Rows(i).FindControl("lblMarkahElemen"), Label)

            strSQL = "  SELECT BM4 FROM kpmkv_pentaksir_bmsetara_calon
                        WHERE id = '" & strKey & "'"
            Dim BM4 As String = oCommon.getFieldValue(strSQL)

            If BM4 = "" Then
                BM4 = "-"
            ElseIf BM4 = "T" Then
                BM4 = "T"
            Else
                BM4 = "*"
            End If

            lblME.Text = "[" & BM4 & "]"

        Next

    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click

        Response.Redirect("pentaksir_bmsetara_penyerahan_markah_list.aspx")

    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click

        Try

            strSQL = "SELECT UserID FROM kpmkv_users WHERE LoginID = '" & Session("LoginID") & "' AND Pwd = '" & Session("Password") & "'"
            Dim UserID As String = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT KolejRecordID FROM kpmkv_pentaksir_bmsetara WHERE id = '" & Request.QueryString("id") & "'"
            Dim KolejRecordID As String = oCommon.getFieldValue(strSQL)

            strSQL = "SELECT Kohort FROM kpmkv_pentaksir_bmsetara WHERE id = '" & Request.QueryString("id") & "'"
            Dim Tahun As String = oCommon.getFieldValue(strSQL)

            strSQL = "  UPDATE kpmkv_pentaksir_bmsetara_calon
                        SET
                        StatusSerahBM4KJPP = 'TELAH SERAH',
                        StatusSerahBM4KJPPBy = '" & UserID & "',
                        StatusSerahBM4KJPP_timestamp = CURRENT_TIMESTAMP
                        WHERE KolejRecordID = '" & KolejRecordID & "'
                        AND Tahun = '" & Tahun & "'"

            strRet = oCommon.ExecuteSQL(strSQL)

            If strRet = "0" Then
                lblMsg.Attributes("class") = "info"
                lblMsg.Text = "Markah berjaya diserah"
            Else
                lblMsg.Attributes("class") = "error"
                lblMsg.Text = "Markah tidak berjaya diserah"
            End If

        Catch ex As Exception
            lblMsg.Text = "Error:" & ex.Message
        End Try

        strRet = BindData(datRespondent)
        MarkahElemen()
        StatusHantar()

    End Sub

    Private Sub btnCari_Click(sender As Object, e As EventArgs) Handles btnCari.Click

        strRet = BindData(datRespondent)
        MarkahElemen()

    End Sub

    Private Function BindData(ByVal gvTable As GridView) As Boolean
        Dim myDataSet As New DataSet
        Dim myDataAdapter As New SqlDataAdapter(getSQL, strConn)
        myDataAdapter.SelectCommand.CommandTimeout = 120
        Try
            myDataAdapter.Fill(myDataSet, "myaccount")

            If myDataSet.Tables(0).Rows.Count = 0 Then
                'divMsg.Attributes("class") = "error"
                'lblMsg.Text = "Tiada rekod pelajar."
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

        strSQL = "SELECT KolejRecordID FROM kpmkv_pentaksir_bmsetara WHERE id = '" & Request.QueryString("id") & "'"
        Dim KolejRecordID As String = oCommon.getFieldValue(strSQL)

        strSQL = "SELECT Kohort FROM kpmkv_pentaksir_bmsetara WHERE id = '" & Request.QueryString("id") & "'"
        Dim Kohort As String = oCommon.getFieldValue(strSQL)

        strSQL = "SELECT Semester FROM kpmkv_pentaksir_bmsetara WHERE id = '" & Request.QueryString("id") & "'"
        Dim Semester As String = oCommon.getFieldValue(strSQL)


        Dim tmpSQL As String = ""
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY AngkaGiliran, Nama ASC"

        '--not deleted
        tmpSQL = "SELECT id, KolejRecordID, Tahun, Nama, MYKAD, AngkaGiliran, BM4, StatusSerahBM4KJPP FROM kpmkv_pentaksir_bmsetara_calon"
        strWhere = " WHERE KolejRecordID='" & KolejRecordID & "' AND Tahun ='" & Kohort & "'"

        If Not txtCarian.Text = "" Then
            strWhere += "AND (MYKAD = '" & txtCarian.Text & "' OR AngkaGiliran = '" & txtCarian.Text & "')"
        End If


        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        'Response.Write(getSQL)

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

End Class

