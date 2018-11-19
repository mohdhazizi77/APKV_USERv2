Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Imports System.Data.Common

Partial Public Class pelajar_markah_list
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                lblMsg.Text = getSQL()

                'strRet = BindData(datRespondent)
            End If

        Catch ex As Exception
            lblMsg.Text = "Error Message:" & ex.Message
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
        Dim strOrder As String = " ORDER BY Nama ASC"

        tmpSQL = "SELECT kpmkv_markah.MarkahID,RecordID,Nama,MYKAD,ANGKAGILIRAN,Tahun,Semester,M1_TEORI,M2_TEORI,M3_TEORI,M4_TEORI,M5_TEORI,M6_TEORI,M7_TEORI,M8_TEORI,M1_AMALI,M2_AMALI,M3_AMALI,M4_AMALI,M5_AMALI,M6_AMALI,M7_AMALI,M8_AMALI FROM kpmkv_pelajar"
        strWhere = " INNER JOIN kpmkv_markah ON kpmkv_pelajar.RecordID=kpmkv_markah.PelajarRecordID"
        strWhere += " WHERE kpmkv_pelajar.KolejRecordID='" & Request.QueryString("RecordID") & "' AND kpmkv_markah.IsApproved='Y'"

        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        ''Response.Write(getSQL)

        Return getSQL()

    End Function

End Class