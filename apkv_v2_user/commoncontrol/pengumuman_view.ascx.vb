Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient

Public Class pengumuman_view
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String
    Dim strRet As String
    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnDelete.Attributes.Add("onclick", "return confirm('Pasti ingin menghapuskan rekod tersebut?');")

        Try
            If Not IsPostBack Then
                kpmkv_pengumuman_load()
            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub kpmkv_pengumuman_load()
        strSQL = "SELECT * FROM kpmkv_pengumuman WHERE PengumumanID=" & Request.QueryString("pengumumanid")
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
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Tahun")) Then
                    lblTahun.Text = ds.Tables(0).Rows(0).Item("Tahun")
                Else
                    lblTahun.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("IsDisplay")) Then
                    lblIsDisplay.Text = ds.Tables(0).Rows(0).Item("IsDisplay")
                Else
                    lblIsDisplay.Text = ""
                End If

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
                    lblDateCreated.Text = ds.Tables(0).Rows(0).Item("DateCreated")
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

    Protected Sub lnkList_Click(sender As Object, e As EventArgs) Handles lnkList.Click

        Response.Redirect("admin.pengumuman.list.aspx")

    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Response.Redirect("admin.pengumuman.update.aspx?pengumumanid=" & Request.QueryString("pengumumanid"))

    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        strSQL = "DELETE kpmkv_pengumuman WHERE PengumumanID=" & Request.QueryString("pengumumanid")
        strRet = oCommon.ExecuteSQL(strSQL)
        If strRet = "0" Then
            lblMsg.Text = "Rekod berjaya dihapuskan."
        Else
            lblMsg.Text = "system error:" & strRet
        End If

    End Sub

End Class