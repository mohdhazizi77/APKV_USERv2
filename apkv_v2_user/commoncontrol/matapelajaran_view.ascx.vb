Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class matapelajaran_view
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                LoadPage()
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try
    End Sub

    Private Sub LoadPage()
        strSQL = "SELECT * FROM kpmkv_MataPelajaran WHERE MataPelajaranID='" & Request.QueryString("MataPelajaranID") & "'"
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

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("NamaKluster")) Then
                    lblNamaKluster.Text = ds.Tables(0).Rows(0).Item("NamaKluster")
                Else
                    lblNamaKluster.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("KodKursus")) Then
                    lblKodKursus.Text = ds.Tables(0).Rows(0).Item("KodKursus")
                Else
                    lblKodKursus.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Namakursus")) Then
                    lblNamaKursus.Text = ds.Tables(0).Rows(0).Item("NamaKursus")
                Else
                    lblNamaKursus.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Tahun")) Then
                    lblTahun.Text = ds.Tables(0).Rows(0).Item("Tahun")
                Else
                    lblTahun.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("KodMataPelajaran")) Then
                    lblKodMataPelajaran.Text = ds.Tables(0).Rows(0).Item("KodMataPelajaran")
                Else
                    lblKodMataPelajaran.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("NamaMataPelajaran")) Then
                    lblNamaMataPelajaran.Text = ds.Tables(0).Rows(0).Item("NamaMataPelajaran")
                Else
                    lblNamaMataPelajaran.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("JamKredit")) Then
                    lblJamKredit.Text = ds.Tables(0).Rows(0).Item("JamKredit")
                Else
                    lblJamKredit.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Sesi")) Then
                    lblSesi.Text = ds.Tables(0).Rows(0).Item("Sesi")
                Else
                    lblSesi.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Semester")) Then
                    lblSemester.Text = ds.Tables(0).Rows(0).Item("Semester")
                Else
                    lblSemester.Text = ""
                End If

            End If

        Catch ex As Exception
            divMsg.Attributes("class") = "error"
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Protected Sub btnExecute_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExecute.Click
        Response.Redirect("matapelajaran.update.aspx?MataPelajaranID=" & Request.QueryString("MataPelajaranID"))
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
End Class