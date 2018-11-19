Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Imports System.Data.Common

Public Class slip_view
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                slip_view()
            End If

        Catch ex As Exception
            'lblMsg.Text = "Error Message:" & ex.Message
        End Try

    End Sub

    Private Sub slip_view()
        ''--Kolej info
        'strSQL = "SELECT KolejRecordID FROM kpmkv_pelajar_kolej WHERE PelajarKolejID='" & Request.QueryString("PelajarKolejID") & "'"
        'Dim strKolejRecordID As String = oCommon.getFieldValue(strSQL)

        'strSQL = "SELECT Tahun FROM kpmkv_pelajar_kolej WHERE PelajarKolejID='" & Request.QueryString("PelajarKolejID") & "'"
        'lblTahun.Text = oCommon.getFieldValue(strSQL)

        'strSQL = "SELECT Semester FROM kpmkv_pelajar_kolej WHERE PelajarKolejID='" & Request.QueryString("PelajarKolejID") & "'"
        'lblSemester.Text = oCommon.getFieldValue(strSQL)

        'strSQL = "SELECT Nama FROM kpmkv_kolej WHERE RecordID='" & strKolejRecordID & "'"
        'lblNama.Text = oCommon.getFieldValue(strSQL)

        'strSQL = "SELECT Kod FROM kpmkv_kolej WHERE RecordID='" & strKolejRecordID & "'"
        'lblKod.Text = oCommon.getFieldValue(strSQL)


    End Sub

End Class