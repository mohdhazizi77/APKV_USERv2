Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient

Public Class pengumuman_view_top
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String
    Dim strRet As String
    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                kpmkv_pengumuman_load()
            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Function getPengumumanID() As String
        strSQL = "SELECT PengumumanID FROM kpmkv_pengumuman ORDER BY PengumumanID DESC"
        strRet = oCommon.getFieldValue(strSQL)

        Return strRet
    End Function

    Private Sub kpmkv_pengumuman_load()
        Dim strDateCreated As String = ""

        strSQL = "SELECT * FROM kpmkv_pengumuman WHERE PengumumanID=" & getPengumumanID()
        '--debug
        'Response.Write(strSQL)

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
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Title")) Then
                    lblTitle.Text = ds.Tables(0).Rows(0).Item("Title")
                Else
                    lblTitle.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Body")) Then
                    ltBody.Text = ds.Tables(0).Rows(0).Item("Body").ToString.Replace(Environment.NewLine, "<br />")
                Else
                    ltBody.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("DateCreated")) Then
                    strDateCreated = ds.Tables(0).Rows(0).Item("DateCreated")
                    ' lblDateCreated.Text = oCommon.formatDateDay(strDateCreated)
                Else
                    lblDateCreated.Text = ""
                End If

            End If

        Catch ex As Exception
            'lblMsg.Text = "System error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try
    End Sub

End Class