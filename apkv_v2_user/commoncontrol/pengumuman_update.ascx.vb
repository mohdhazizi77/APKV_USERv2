Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient

Public Class pengumuman_update
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String
    Dim strRet As String
    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Not IsPostBack Then
                kpmkv_tahun_list()
                ddlTahun.Text = Now.Year

                kpmkv_pengumuman_load()
            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub kpmkv_tahun_list()
        strSQL = "SELECT Tahun FROM kpmkv_tahun WITH (NOLOCK) ORDER BY Tahun"
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
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
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
                    ddlTahun.Text = ds.Tables(0).Rows(0).Item("Tahun")
                Else
                    ddlTahun.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("IsDisplay")) Then
                    selIsDisplay.Value = ds.Tables(0).Rows(0).Item("IsDisplay")
                Else
                    selIsDisplay.Value = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Title")) Then
                    txtTitle.Text = ds.Tables(0).Rows(0).Item("Title")
                Else
                    txtTitle.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Body")) Then
                    txtBody.Text = ds.Tables(0).Rows(0).Item("Body")
                Else
                    txtBody.Text = ""
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
        Try
            'check form validation. if failed exit
            If ValidateForm() = False Then
                Exit Sub
            End If

            'insert into course list
            strSQL = "UPDATE kpmkv_pengumuman SET Tahun='" & ddlTahun.Text & "',IsDisplay='" & selIsDisplay.Value & "',Title='" & oCommon.FixSingleQuotes(txtTitle.Text.ToUpper) & "',Body='" & oCommon.FixSingleQuotes(txtBody.Text) & "' WHERE PengumumanID=" & Request.QueryString("pengumumanid")
            strRet = oCommon.ExecuteSQL(strSQL)
            If strRet = "0" Then
                lblMsg.Text = "Kemaskini berjaya!"
            Else
                lblMsg.Text = "Gagal mengemaskini rekod. " & strRet
            End If

        Catch ex As Exception
            lblMsg.Text = "System error:" & ex.Message

        End Try

    End Sub

    '--CHECK form validation.
    Private Function ValidateForm() As Boolean

        If txtTitle.Text.Length = 0 Then
            lblMsg.Text = "Medan ini mesti diisi."
            txtTitle.Focus()
            Return False
        End If

        If txtBody.Text.Length = 0 Then
            lblMsg.Text = "Medan ini mesti diisi."
            txtBody.Focus()
            Return False
        End If

        Return True
    End Function


End Class