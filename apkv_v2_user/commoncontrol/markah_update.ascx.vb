Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization

Partial Public Class markah_update
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnDelete.Attributes.Add("onclick", "return confirm('Pasti ingin menghapuskan rekod ini?');")

        Try
            If Not IsPostBack Then
                LoadPage()
                lblNama.Text = getNama()
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try

    End Sub

    Private Function getNama() As String
        strSQL = "SELECT Nama FROM kpmkv_kolej WHERE RecordID='" & lblKolejRecordID.Text & "'"
        getNama = oCommon.getFieldValue(strSQL)

        Return getNama
    End Function

    Private Sub LoadPage()
        strSQL = "SELECT * FROM kpmkv_markah WHERE MarkahID=" & Request.QueryString("MarkahID")
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
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("KolejRecordID")) Then
                    lblKolejRecordID.Text = ds.Tables(0).Rows(0).Item("KolejRecordID")
                Else
                    lblKolejRecordID.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Tahun")) Then
                    lblTahun.Text = ds.Tables(0).Rows(0).Item("Tahun")
                Else
                    lblTahun.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Semester")) Then
                    lblSemester.Text = ds.Tables(0).Rows(0).Item("Semester")
                Else
                    lblSemester.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Sesi")) Then
                    lblSesi.Text = ds.Tables(0).Rows(0).Item("Sesi")
                Else
                    lblSesi.Text = ""
                End If
                '---TEORI
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("M1_TEORI")) Then
                    M1_TEORI.Text = ds.Tables(0).Rows(0).Item("M1_TEORI")
                Else
                    M1_TEORI.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("M2_TEORI")) Then
                    M2_TEORI.Text = ds.Tables(0).Rows(0).Item("M2_TEORI")
                Else
                    M2_TEORI.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("M3_TEORI")) Then
                    M3_TEORI.Text = ds.Tables(0).Rows(0).Item("M3_TEORI")
                Else
                    M3_TEORI.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("M4_TEORI")) Then
                    M4_TEORI.Text = ds.Tables(0).Rows(0).Item("M4_TEORI")
                Else
                    M4_TEORI.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("M5_TEORI")) Then
                    M5_TEORI.Text = ds.Tables(0).Rows(0).Item("M5_TEORI")
                Else
                    M5_TEORI.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("M6_TEORI")) Then
                    M6_TEORI.Text = ds.Tables(0).Rows(0).Item("M6_TEORI")
                Else
                    M6_TEORI.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("M7_TEORI")) Then
                    M7_TEORI.Text = ds.Tables(0).Rows(0).Item("M7_TEORI")
                Else
                    M7_TEORI.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("M8_TEORI")) Then
                    M8_TEORI.Text = ds.Tables(0).Rows(0).Item("M8_TEORI")
                Else
                    M8_TEORI.Text = ""
                End If

                '---AMALI
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("M1_AMALI")) Then
                    M1_AMALI.Text = ds.Tables(0).Rows(0).Item("M1_AMALI")
                Else
                    M1_AMALI.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("M2_AMALI")) Then
                    M2_AMALI.Text = ds.Tables(0).Rows(0).Item("M2_AMALI")
                Else
                    M2_AMALI.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("M3_AMALI")) Then
                    M3_AMALI.Text = ds.Tables(0).Rows(0).Item("M3_AMALI")
                Else
                    M3_AMALI.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("M4_AMALI")) Then
                    M4_AMALI.Text = ds.Tables(0).Rows(0).Item("M4_AMALI")
                Else
                    M4_AMALI.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("M5_AMALI")) Then
                    M5_AMALI.Text = ds.Tables(0).Rows(0).Item("M5_AMALI")
                Else
                    M5_AMALI.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("M6_AMALI")) Then
                    M6_AMALI.Text = ds.Tables(0).Rows(0).Item("M6_AMALI")
                Else
                    M6_AMALI.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("M7_AMALI")) Then
                    M7_AMALI.Text = ds.Tables(0).Rows(0).Item("M7_AMALI")
                Else
                    M7_AMALI.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("M8_AMALI")) Then
                    M8_AMALI.Text = ds.Tables(0).Rows(0).Item("M8_AMALI")
                Else
                    M8_AMALI.Text = ""
                End If

            End If

        Catch ex As Exception
            divMsg.Attributes("class") = "error"
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        lblMsg.Text = ""

        Try
            '--validate
            If ValidatePage() = False Then
                divMsg.Attributes("class") = "error"
                Exit Sub
            End If

            '--execute
            If kpmkv_markah_update() = True Then
                divMsg.Attributes("class") = "info"
                lblMsg.Text = "Berjaya mengemaskini maklumat Markah."
            Else
                divMsg.Attributes("class") = "error"
            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try

    End Sub

    Private Function ValidatePage() As Boolean

        Return True
    End Function

    Private Function kpmkv_markah_update() As Boolean
        strSQL = "UPDATE kpmkv_markah SET M1_TEORI=" & M1_TEORI.Text & ",M2_TEORI=" & M2_TEORI.Text & ",M3_TEORI=" & M3_TEORI.Text & ",M4_TEORI=" & M4_TEORI.Text & ",M5_TEORI=" & M5_TEORI.Text & ",M6_TEORI=" & M6_TEORI.Text & ",M7_TEORI=" & M7_TEORI.Text & ",M8_TEORI=" & M8_TEORI.Text & ",M1_AMALI=" & M1_AMALI.Text & ",M2_AMALI=" & M2_AMALI.Text & ",M3_AMALI=" & M3_AMALI.Text & ",M4_AMALI=" & M4_AMALI.Text & ",M5_AMALI=" & M5_AMALI.Text & ",M6_AMALI=" & M6_AMALI.Text & ",M7_AMALI=" & M7_AMALI.Text & ",M8_AMALI=" & M8_AMALI.Text & " WHERE MarkahID=" & Request.QueryString("MarkahID")
        strRet = oCommon.ExecuteSQL(strSQL)
        If strRet = "0" Then
            Return True
        Else
            lblMsg.Text = "System Error:" & strRet
            Return False
        End If

    End Function

    Protected Sub lnkList_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkList.Click
        Response.Redirect("markah.import.aspx?RecordID=" & lblKolejRecordID.Text)

    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        strSQL = "DELETE kpmkv_markah WHERE MarkahID=" & Request.QueryString("MarkahID")
        strRet = oCommon.ExecuteSQL(strSQL)
        If strRet = "0" Then
            lblMsg.Text = "Berjaya meghapuskan rekod pelajar tersebut."
        Else
            lblMsg.Text = "System Error:" & strRet
        End If

    End Sub
End Class