Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient

Public Class pengumuman_list_pub
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String
    Dim strRet As String
    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                '--default
                strRet = BindData(datRespondent)
            End If

        Catch ex As Exception

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

            Else

            End If

            gvTable.DataSource = myDataSet
            gvTable.DataBind()
            objConn.Close()

        Catch ex As Exception
            Return False
        End Try

        Return True

    End Function

    Private Function getSQL() As String
        'A left outer join will give all rows in A, plus any common rows in B.

        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY PengumumanID DESC"
        tmpSQL = "SELECT * FROM kpmkv_pengumuman"
        strWhere = " WHERE IsDisplay='Y' AND Tahun='" & Now.Year & "'"

        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        'Response.Write(getSQL)

        Return getSQL

    End Function

    Private Sub datRespondent_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles datRespondent.SelectedIndexChanging
        Dim strKeyID As String = datRespondent.DataKeys(e.NewSelectedIndex).Value.ToString
        Response.Redirect("public.pengumuman.view.aspx?pengumumanid=" & strKeyID)

    End Sub

    Protected Function EvalWithMaxLength(fieldName As String, maxLength As Integer) As String
        Dim value As Object = Me.Eval(fieldName)
        If value Is Nothing Then
            Return Nothing
        End If

        Dim str As String = value.ToString()
        If value.Length > maxLength Then
            Return value.SubString(0, maxLength) & "..."
        Else
            Return value
        End If
    End Function

    Protected Sub lnkRead_Click(sender As Object, e As EventArgs)
        Dim btn As LinkButton = DirectCast(sender, LinkButton)
        Dim row As GridViewRow = DirectCast(btn.NamingContainer, GridViewRow)

        Dim strMsg As String = datRespondent.DataKeys(row.RowIndex).Value.ToString()
        'strMsg = "Row Index of Link button: " + row.RowIndex + "DataKey value:" + datRespondent.DataKeys(row.RowIndex).Value.ToString()
        Response.Redirect("public.pengumuman.view.aspx?pengumumanid=" & strMsg)

    End Sub


End Class