Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class penetapan_pemeriksa
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""
    Dim strKolejnama As String = ""
    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Not IsPostBack Then
               
                kpmkv_tahun_list()
                ddlTahun.Text = Now.Year

                kpmkv_pemeriksa_list()

                strRet = BindData2(datRespondent2)
                strRet = BindData(datRespondent)
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try

    End Sub
    Private Sub kpmkv_tahun_list()
        strSQL = "SELECT Tahun FROM kpmkv_tahun WITH (NOLOCK) ORDER BY TahunID"
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

        Catch ex As Exception

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_pemeriksa_list()
        strSQL = "SELECT Nama FROM kpmkv_users WHERE UserType='PEMERIKSA' ORDER BY Nama"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlPemeriksa.DataSource = ds
            ddlPemeriksa.DataTextField = "Nama"
            ddlPemeriksa.DataValueField = "Nama"
            ddlPemeriksa.DataBind()

        Catch ex As Exception

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Protected Sub btnCari_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCari.Click
        lblmsg.Text = ""
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
    Private Function BindData2(ByVal gvTable As GridView) As Boolean
        Dim myDataSet As New DataSet
        Dim myDataAdapter As New SqlDataAdapter(getSQL2, strConn)
        myDataAdapter.SelectCommand.CommandTimeout = 120

        Try
            myDataAdapter.Fill(myDataSet, "myaccount")

            If myDataSet.Tables(0).Rows.Count = 0 Then
                divMsg2.Attributes("class") = "error"
                lblMsg2.Text = "Rekod tidak dijumpai!"
            Else
                divMsg2.Attributes("class") = "info"
                lblMsg2.Text = "Jumlah Rekod#:" & myDataSet.Tables(0).Rows.Count
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
    Private Function getSQL2() As String
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY kpmkv_pemeriksa.PemeriksaID, kpmkv_file.Tahun, kpmkv_file.Sesi ASC"
        'FileID,Tahun,Sesi,NamaKolej,KolejID
        '--not deleted
        tmpSQL = "SELECT kpmkv_pemeriksa.PemeriksaID, kpmkv_file.Tahun, kpmkv_file.Sesi,kpmkv_file.KolejID,kpmkv_file.NamaFail,kpmkv_pemeriksa.NamaPemeriksa"
        tmpSQL += " FROM kpmkv_pemeriksa INNER JOIN kpmkv_file ON kpmkv_file.FileID=kpmkv_pemeriksa.FileID"
        strWhere = " WHERE kpmkv_pemeriksa.FileID IS NOT NULL"

        '--tahun
        If Not ddlTahun.Text = "PILIH" Then
            strWhere += " AND kpmkv_file.Tahun ='" & ddlTahun.Text & "'"
        End If
        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere += " AND kpmkv_file.Sesi ='" & chkSesi.Text & "'"
        End If

        getSQL2 = tmpSQL & strWhere & strOrder
        ''--debug
        'Response.Write(getSQL)

        Return getSQL2

    End Function
    Private Function getSQL() As String
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY kpmkv_file.FileID, kpmkv_file.Tahun, kpmkv_file.Sesi ASC"

        '--not deleted
        tmpSQL = "SELECT kpmkv_file.FileID, kpmkv_file.Tahun, kpmkv_file.Sesi,kpmkv_file.NamaKolej,kpmkv_file.KolejID,kpmkv_file.NamaFail"
        tmpSQL += " FROM  kpmkv_file"
        strWhere = " WHERE kpmkv_file.kpmkv_file IS NOT NULL"

        '--tahun
        If Not ddlTahun.Text = "PILIH" Then
            strWhere += " AND kpmkv_file.Tahun ='" & ddlTahun.Text & "'"
        End If
        '--sesi
        If Not chkSesi.Text = "" Then
            strWhere += " AND kpmkv_file.Sesi ='" & chkSesi.Text & "'"
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

    Private Sub datRespondent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondent.PageIndexChanging
        datRespondent.PageIndex = e.NewPageIndex
        strRet = BindData(datRespondent)

    End Sub

    Private Sub datRespondent_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles datRespondent.SelectedIndexChanging
        Dim strKeyID As String = datRespondent.DataKeys(e.NewSelectedIndex).Value.ToString
        ' Response.Redirect("pelajar.view.aspx?PelajarID=" & strKeyID)

    End Sub

    Protected Sub datRespondent2_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles datRespondent2.RowCommand
        lblMsg.Text = ""
        If (e.CommandName = "Batal") = True Then

            Dim PemeriksaID = Int32.Parse(e.CommandArgument.ToString())

            strSQL = "DELETE FROM kpmkv_pemeriksa WHERE PemeriksaID='" & PemeriksaID & "'"
            strRet = oCommon.ExecuteSQL(strSQL)
            If strRet = "0" Then
                divMsg2.Attributes("class") = "error"
                lblMsg2.Text = "Pemeriksa berjaya dipadamkan"
            Else
                divMsg2.Attributes("class") = "error"
                lblMsg2.Text = "Pemeriksa tidak berjaya dipadamkan"
            End If

        End If

        strRet = BindData2(datRespondent2)

    End Sub

    Protected Sub btnSimpan_Click(sender As Object, e As EventArgs) Handles btnSimpan.Click
        strSQL = "SELECT UserID FROM kpmkv_users WHERE Nama='" & ddlPemeriksa.Text & "'"
        Dim strUserID As Integer = oCommon.getFieldValue(strSQL)

        For i As Integer = 0 To datRespondent.Rows.Count - 1
            Dim row As GridViewRow = datRespondent.Rows(i)
            Dim isChecked As Boolean = DirectCast(row.FindControl("chkSelect"), CheckBox).Checked

            Dim str As String
            str = datRespondent.DataKeys(i).Value.ToString
            If isChecked Then
                'create with isApproved='Y'
                strSQL = "INSERT INTO kpmkv_pemeriksa (NamaPemeriksa,FileID,UserID)"
                strSQL += "VALUES ('" & ddlPemeriksa.Text & "','" & str & "','" & strUserID & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
                If strRet = "0" Then
                    divMsg.Attributes("class") = "info"
                    lblmsg.Text = "Fail sudah dijana."
                End If
            End If
        Next
        strRet = BindData(datRespondent)
        strRet = BindData2(datRespondent2)
    End Sub
End Class

















