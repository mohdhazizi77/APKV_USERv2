Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class kelas_update
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then

                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                LoadPage()
               
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try

    End Sub
    Private Sub LoadPage()
        strSQL = "SELECT kpmkv_kelas.Tahun, kpmkv_kelas.NamaKelas FROM kpmkv_kelas"
        strSQL += " WHERE kpmkv_kelas.IsDeleted='N' AND kpmkv_kelas.KelasID='" & Request.QueryString("KelasID") & "'"
        strSQL += " AND kpmkv_kelas.KolejRecordID='" & lblKolejID.Text & "'"
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
                '--Account Details 
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Tahun")) Then
                    txtTahun.Text = ds.Tables(0).Rows(0).Item("Tahun")
                Else
                    txtTahun.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("NamaKelas")) Then
                    txtNamaKelas.Text = ds.Tables(0).Rows(0).Item("NamaKelas")
                Else
                    txtNamaKelas.Text = ""
                End If
                lblNamaKelas.Text = txtNamaKelas.Text

            End If

        Catch ex As Exception
            divMsg.Attributes("class") = "error"
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        lblMsg.Text = ""

        Try
            '--validate
            If ValidatePage() = False Then
                divMsg.Attributes("class") = "error"
                Exit Sub
            End If

            '--execute
            If kpmkv_kelas_update() = True Then
                divMsg.Attributes("class") = "info"
                lblMsg.Text = "Berjaya mengemaskini Kelas baru."
            Else
                divMsg.Attributes("class") = "error"
            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try

    End Sub
    Private Function ValidatePage() As Boolean

        '--txtNama
        If txtNamaKelas.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan Nama Kelas!"
            txtNamaKelas.Focus()
            Return False
        End If
        strSQL = "SELECT * FROM kpmkv_kelas WHERE NamaKelas='" & txtNamaKelas.Text & "' AND Tahun='" & txtTahun.Text & "'  AND KolejRecordID='" & lblKolejID.Text & "'"
        If oCommon.isExist(strSQL) = True Then
            lblMsg.Text = "Nama Kelas telah digunakan. Sila masukkan Nama Kelas yang baru."
            Return False
        End If

        Return True
    End Function
    Private Function kpmkv_kelas_update() As Boolean
        strSQL = "UPDATE kpmkv_kelas SET NamaKelas='" & txtNamaKelas.Text & "' WHERE KelasID='" & Request.QueryString("KelasID") & "'"
        strRet = oCommon.ExecuteSQL(strSQL)
       
            If strRet = "0" Then
                Return True
            Else
                lblMsg.Text = "System Error:" & strRet
                Return False
            End If
    End Function

End Class