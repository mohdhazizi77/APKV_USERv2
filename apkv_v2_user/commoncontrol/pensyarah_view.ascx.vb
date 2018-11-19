Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class pensyarah_view
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then

                lblPensyarahID.Text = Request.QueryString("PensyarahID")

                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                LoadPage()
                strRet = BindData(datRespondent)
                strRet = BindData2(datRespondent2)
              
            End If

        Catch ex As Exception
            'lblMsg.Text = "System Error:" & ex.Message
        End Try
    End Sub
    Private Sub LoadPage()
        strSQL = "SELECT kpmkv_pensyarah.*,kpmkv_status.Status FROM kpmkv_pensyarah "
        strSQL += " LEFT OUTER JOIN kpmkv_status ON kpmkv_pensyarah.StatusID=kpmkv_status.StatusID"
        strSQL += " WHERE kpmkv_pensyarah.PensyarahID='" & Request.QueryString("PensyarahID") & "'"
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
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Nama")) Then
                    lblNama.Text = ds.Tables(0).Rows(0).Item("Nama")
                Else
                    lblNama.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Jawatan")) Then
                    lblJawatan.Text = ds.Tables(0).Rows(0).Item("Jawatan")
                Else
                    lblJawatan.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Gred")) Then
                    lblGred.Text = ds.Tables(0).Rows(0).Item("Gred")
                Else
                    lblGred.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("MYKAD")) Then
                    lblMYKAD.Text = ds.Tables(0).Rows(0).Item("MYKAD")
                Else
                    lblMYKAD.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Tel")) Then
                    lblTel.Text = ds.Tables(0).Rows(0).Item("Tel")
                Else
                    lblTel.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Email")) Then
                    lblEmail.Text = ds.Tables(0).Rows(0).Item("Email")
                Else
                    lblEmail.Text = ""
                End If
                '--
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Jantina")) Then
                    lblJantina.Text = ds.Tables(0).Rows(0).Item("Jantina")
                Else
                    lblJantina.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Kaum")) Then
                    lblKaum.Text = ds.Tables(0).Rows(0).Item("Kaum")
                Else
                    lblKaum.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Agama")) Then
                    lblAgama.Text = ds.Tables(0).Rows(0).Item("Agama")
                Else
                    lblAgama.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Status")) Then
                    lblStatus2.Text = ds.Tables(0).Rows(0).Item("Status")
                Else
                    lblStatus2.Text = ""
                End If

            End If

        Catch ex As Exception
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
                'lblMsg.Text = "Rekod tidak dijumpai!"
            Else
                divMsg.Attributes("class") = "info"
                'lblMsg.Text = "Jumlah Rekod#:" & myDataSet.Tables(0).Rows.Count
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
    Private Function BindData2(ByVal gvTable As GridView) As Boolean
        Dim myDataSet As New DataSet
        Dim myDataAdapter As New SqlDataAdapter(getSQL2, strConn)
        myDataAdapter.SelectCommand.CommandTimeout = 120

        Try
            myDataAdapter.Fill(myDataSet, "myaccount")

            If myDataSet.Tables(0).Rows.Count = 0 Then
                divMsg.Attributes("class") = "error"

            Else
                divMsg.Attributes("class") = "info"
                'lblMsg.Text = "Jumlah Rekod#:" & myDataSet.Tables(0).Rows.Count
            End If

            gvTable.DataSource = myDataSet
            gvTable.DataBind()
            objConn.Close()
        Catch ex As Exception
            'lblMsg.Text = "System Error:" & ex.Message
            Return False
        End Try

        Return True

    End Function
    Private Function getSQL() As String
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY kpmkv_kursus.Tahun, kpmkv_kursus.Sesi, kpmkv_modul.Semester, kpmkv_kluster.NamaKluster ASC"

        tmpSQL = "SELECT kpmkv_kelas.NamaKelas,kpmkv_pensyarah_modul.PensyarahModulID, kpmkv_kursus.Tahun, kpmkv_kursus.Sesi, kpmkv_modul.Semester, kpmkv_kluster.NamaKluster, "
        tmpSQL += " kpmkv_kursus.KodKursus, kpmkv_kursus.NamaKursus, kpmkv_modul.KodModul, kpmkv_modul.NamaModul, kpmkv_modul.JamKredit"
        tmpSQL += " FROM  kpmkv_pensyarah_modul LEFT OUTER JOIN kpmkv_kelas ON kpmkv_pensyarah_modul.KelasID = kpmkv_kelas.KelasID LEFT OUTER  JOIN"
        tmpSQL += " kpmkv_kursus ON kpmkv_pensyarah_modul.KursusID = kpmkv_kursus.KursusID LEFT OUTER  JOIN kpmkv_kluster ON kpmkv_kursus.KlusterID = kpmkv_kluster.KlusterID LEFT OUTER  JOIN"
        tmpSQL += " kpmkv_modul ON kpmkv_pensyarah_modul.ModulID = kpmkv_modul.ModulID LEFT OUTER  JOIN kpmkv_pensyarah ON kpmkv_pensyarah_modul.PensyarahID = kpmkv_pensyarah.PensyarahID"
        strWhere = "  WHERE kpmkv_pensyarah.PensyarahID='" & Request.QueryString("PensyarahID") & "'"


        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        ''Response.Write(getSQL)

        Return getSQL

    End Function
    Private Function getSQL2() As String
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY Tahun ASC"

        tmpSQL = "SELECT kpmkv_pensyarah_matapelajaran.PensyarahMataPelajaranID, kpmkv_pensyarah_matapelajaran.Tahun, kpmkv_pensyarah_matapelajaran.Semester, kpmkv_pensyarah_matapelajaran.Sesi, "
        tmpSQL += " kpmkv_matapelajaran.KodMataPelajaran, kpmkv_matapelajaran.NamaMataPelajaran, kpmkv_kelas.NamaKelas FROM kpmkv_pensyarah_matapelajaran "
        tmpSQL += " LEFT OUTER JOIN kpmkv_pensyarah ON kpmkv_pensyarah_matapelajaran.PensyarahID = kpmkv_pensyarah.PensyarahID LEFT OUTER JOIN"
        tmpSQL += " kpmkv_kelas ON kpmkv_pensyarah_matapelajaran.KelasID = kpmkv_kelas.KelasID LEFT OUTER JOIN kpmkv_matapelajaran ON kpmkv_pensyarah_matapelajaran.MataPelajaranID= kpmkv_matapelajaran.MataPelajaranId"
        strWhere = "  WHERE kpmkv_pensyarah.PensyarahID='" & Request.QueryString("PensyarahID") & "'"


        getSQL2 = tmpSQL & strWhere & strOrder
        ''--debug
        'Response.Write(getSQL2)

        Return getSQL2

    End Function
    Private Sub datRespondent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondent.PageIndexChanging
        datRespondent.PageIndex = e.NewPageIndex
        strRet = BindData(datRespondent)

    End Sub
    Private Sub datRespondent2_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondent2.PageIndexChanging
        datRespondent2.PageIndex = e.NewPageIndex
        strRet = BindData2(datRespondent2)

    End Sub
    Private Sub btnExecute_Click(sender As Object, e As EventArgs) Handles btnExecute.Click
        Response.Redirect("pensyarah.update.aspx?PensyarahID=" & lblPensyarahID.Text)
    End Sub
End Class