Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class kelas_view
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

                lblMsg.Text = ""
                LoadPage()
                strSQL = "SELECT KursusID FROM kpmkv_kursus WHERE KodKursus='" & lblKodKursus.Text & "'"
                lblKursusID.Text = oCommon.getFieldValue(strSQL)


                strRet = BindData(datRespondent)
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try
    End Sub
    Private Sub LoadPage()
        strSQL = "SELECT kpmkv_kursus.Tahun, kpmkv_kursus.Sesi, kpmkv_kluster.NamaKluster, kpmkv_kursus.KodKursus,kpmkv_kursus.NamaKursus, kpmkv_kelas.NamaKelas, kpmkv_kelas_kursus.KelasID FROM kpmkv_kelas_kursus "
        strSQL += " LEFT OUTER JOIN kpmkv_kursus ON kpmkv_kelas_kursus.KursusID = kpmkv_kursus.KursusID "
        strSQL += " LEFT OUTER JOIN kpmkv_kelas ON kpmkv_kelas_kursus.KelasID = kpmkv_kelas.KelasID "
        strSQL += " LEFT OUTER JOIN kpmkv_kluster ON kpmkv_kursus.KlusterID = kpmkv_kluster.KlusterID "
        strSQL += " WHERE kpmkv_kelas.IsDeleted='N' and kpmkv_kelas.KelasID='" & Request.QueryString("KelasID") & "'"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)
        'Response.Write(strSQL)
        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            Dim nRows As Integer = 0
            Dim nCount As Integer = 1
            Dim MyTable As DataTable = New DataTable
            MyTable = ds.Tables(0)
            If MyTable.Rows.Count > 0 Then

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("NamaKelas")) Then
                    lblNamaKelas.Text = ds.Tables(0).Rows(0).Item("NamaKelas")
                Else
                    lblNamaKelas.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("KelasID")) Then
                    lblKelasID.Text = ds.Tables(0).Rows(0).Item("KelasID")
                Else
                    lblKelasID.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Tahun")) Then
                    lblTahun.Text = ds.Tables(0).Rows(0).Item("Tahun")
                Else
                    lblTahun.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Sesi")) Then
                    lblSesi.Text = ds.Tables(0).Rows(0).Item("Sesi")
                Else
                    lblSesi.Text = ""
                End If
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

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("NamaKursus")) Then
                    lblNamaKursus.Text = ds.Tables(0).Rows(0).Item("NamaKursus")
                Else
                    lblNamaKursus.Text = ""
                End If

            End If

        Catch ex As Exception
            divMsg.Attributes("class") = "error"
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
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
        Dim strOrder As String = " ORDER BY kpmkv_modul.Semester,kpmkv_kursus.KodKursus ASC"

        tmpSQL = "SELECT kpmkv_modul.ModulID,  kpmkv_modul.Semester, kpmkv_modul.KodModul, kpmkv_modul.NamaModul, kpmkv_modul.JamKredit, "
        tmpSQL += " kpmkv_pensyarah.Nama FROM kpmkv_pensyarah_modul INNER JOIN"
        tmpSQL += " kpmkv_kelas ON kpmkv_pensyarah_modul.KelasID = kpmkv_kelas.KelasID INNER JOIN"
        tmpSQL += " kpmkv_kursus ON kpmkv_pensyarah_modul.KursusID = kpmkv_kursus.KursusID INNER JOIN"
        tmpSQL += " kpmkv_pensyarah ON kpmkv_pensyarah_modul.PensyarahID = kpmkv_pensyarah.PensyarahID INNER JOIN"
        tmpSQL += " kpmkv_modul ON kpmkv_pensyarah_modul.ModulID = kpmkv_modul.ModulID INNER JOIN"
        tmpSQL += " kpmkv_kluster ON kpmkv_kursus.KlusterID = kpmkv_kluster.KlusterID"
        strWhere = " WHERE kpmkv_pensyarah.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kursus.KursusID='" & lblKursusID.Text & "'"
        strWhere += " AND kpmkv_pensyarah_modul.KelasID='" & lblKelasID.Text & "'"

        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        ' Response.Write(getSQL)
        getSQL = tmpSQL & strWhere & strOrder

        Return getSQL

    End Function
    Private Sub datRespondent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondent.PageIndexChanging
        datRespondent.PageIndex = e.NewPageIndex
        strRet = BindData(datRespondent)

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

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Response.Redirect("kelas.update.aspx?KelasID=" & lblKelasID.Text)
    End Sub
End Class