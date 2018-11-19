Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class pemeriksa_list
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""
    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Not IsPostBack Then

                'strSQL = "SELECT UserID from kpmkv_users where LoginID='" & Response.Cookies("kpmkv_loginid").Value & "'"
                'lblUserID.Text = oCommon.getFieldValue(strSQL)
                strRet = BindData(datRespondent)
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try

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
        Dim strOrder As String = " ORDER BY kpmkv_file.FileID, kpmkv_file.Tahun, kpmkv_file.Sesi ASC"
        'FileID,Tahun,Sesi,NamaKolej,KolejID
        '--not deleted
        tmpSQL = "SELECT kpmkv_file.FileID, kpmkv_file.Tahun, kpmkv_file.Sesi,kpmkv_file.KolejID,kpmkv_file.NamaFail,kpmkv_pemeriksa.NamaPemeriksa"
        tmpSQL += " FROM kpmkv_file INNER JOIN kpmkv_pemeriksa ON kpmkv_pemeriksa.FileID=kpmkv_file.FileID"
        strWhere = " WHERE kpmkv_pemeriksa.UserID IS NOT NULL"


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

    Protected Sub datRespondent_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles datRespondent.RowCommand
        lblMsg.Text = ""
        If (e.CommandName = "MuatNaik") = True Then

            Dim strfileID As String = Int32.Parse(e.CommandArgument.ToString())

            strSQL = "SELECT kpmkv_file.NamaFail FROM kpmkv_file INNER JOIN kpmkv_pemeriksa ON kpmkv_pemeriksa.FileID=kpmkv_file.FileID "
            strSQL += " WHERE kpmkv_pemeriksa.FileID IS NOT NULL AND kpmkv_file.FileID=' " & strfileID & " '"
            Dim strNamaFail As String = oCommon.getFieldValue(strSQL)

            Try
                Dim msFileName As String = strNamaFail
                Dim path As String = Server.MapPath("borang\")

                Response.AppendHeader("Content-Disposition", "attachment;filename=" & msFileName)
                Response.TransmitFile(path & msFileName)
                Response.[End]()

            Catch ex As Exception
            End Try

        End If

        strRet = BindData(datRespondent)

    End Sub

End Class