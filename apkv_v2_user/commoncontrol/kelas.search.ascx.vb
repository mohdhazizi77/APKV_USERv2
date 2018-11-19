Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class kelas_search
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

                kpmkv_tahun_list()
                ddlTahun.Text = Now.Year

                kpmkv_namakelas_list()
                ddlNamaKelas.Text = "0"

                lblMsg.Text = ""
                strRet = BindData(datRespondent)
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try
    End Sub
    Private Sub kpmkv_tahun_list()
        strSQL = "SELECT Tahun FROM kpmkv_tahun ORDER BY TahunID"
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

            ''--ALL
            'ddlTahun.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_namakelas_list()
        strSQL = "SELECT  kpmkv_kelas.NamaKelas FROM  kpmkv_kelas_kursus LEFT OUTER JOIN kpmkv_kelas ON kpmkv_kelas_kursus.KelasID = kpmkv_kelas.KelasID"
        strSQL += " WHERE kpmkv_kelas.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kelas.Tahun='" & ddlTahun.Text & "'"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlNamaKelas.DataSource = ds
            ddlNamaKelas.DataTextField = "NamaKelas"
            ddlNamaKelas.DataValueField = "NamaKelas"
            ddlNamaKelas.DataBind()

            '--ALL
            ddlNamaKelas.Items.Add(New ListItem("-Semua Kelas-", "0"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        lblMsg.Text = ""
        strRet = BindData(datRespondent)

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
        Dim strOrder As String = " ORDER BY kpmkv_kursus.Tahun ASC"

        tmpSQL = "SELECT kpmkv_kelas_kursus.KelasKursusID, kpmkv_kelas.KelasID, kpmkv_kursus.Tahun, kpmkv_kursus.Sesi, kpmkv_kluster.NamaKluster,"
        tmpSQL += " kpmkv_kursus.KodKursus,kpmkv_kursus.NamaKursus, kpmkv_kelas.NamaKelas FROM kpmkv_kelas_kursus "
        tmpSQL += " LEFT OUTER JOIN kpmkv_kursus ON kpmkv_kelas_kursus.KursusID = kpmkv_kursus.KursusID "
        tmpSQL += " LEFT OUTER JOIN kpmkv_kelas ON kpmkv_kelas_kursus.KelasID = kpmkv_kelas.KelasID "
        tmpSQL += " LEFT OUTER JOIN kpmkv_kluster ON kpmkv_kursus.KlusterID = kpmkv_kluster.KlusterID "
        strWhere = " WHERE kpmkv_kelas.IsDeleted='N' AND kpmkv_kelas.KolejRecordID='" & lblKolejID.Text & "'"

        '--Tahun
        If Not ddlTahun.Text = "" Then
            strWhere += " AND kpmkv_kursus.Tahun='" & ddlTahun.Text & "'"
        End If

        '--NamaKelas
        If Not ddlNamaKelas.Text = "0" Then
            strWhere += " AND kpmkv_kelas.NamaKelas='" & ddlNamaKelas.Text & "'"
        End If
        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        'Response.Write(getSQL)

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

    Private Sub datRespondent_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles datRespondent.RowDeleting
        lblMsg.Text = ""
            Dim strKelasID As Integer = datRespondent.DataKeys(e.RowIndex).Values("KelasID")
            Try
            If Not strKelasID = Session("strKelasID") Then
                strSQL = "DELETE FROM kpmkv_pensyarah_modul WHERE KelasID='" & strKelasID & "'"
                If strRet = oCommon.ExecuteSQL(strSQL) = 0 Then
                    'update kelas pelajar
                    strSQL = "Update kpmkv_pelajar Set KelasID=NULL WHERE KelasID='" & strKelasID & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                    If strRet = oCommon.ExecuteSQL(strSQL) = 0 Then
                        'delete penetapan kursus -kelas
                        strSQL = "DELETE FROM kpmkv_kelas_kursus WHERE KelasID='" & strKelasID & "'"
                        If strRet = oCommon.ExecuteSQL(strSQL) = 0 Then
                            divMsg.Attributes("class") = "info" 'berjaya
                            lblMsg.Text = "Penetapan kelas berjaya dipadamkan"
                            Session("strKelasID") = ""
                        Else
                            divMsg.Attributes("class") = "error"
                            lblMsg.Text = "Penetapan kelas tidak berjaya dipadamkan"
                            Session("strKelasID") = ""
                        End If

                    Else
                        divMsg.Attributes("class") = "error"
                        lblMsg.Text = "Penetapan kelas tidak berjaya dipadamkan"
                        Session("strKelasID") = ""
                    End If

                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Penetapan kelas tidak berjaya dipadamkan"
                    Session("strKelasID") = ""
                End If
            Else
                Session("strKelasID") = ""
            End If

        Catch ex As Exception
            divMsg.Attributes("class") = "error"
        End Try

        strRet = BindData(datRespondent)
    End Sub

    Private Sub datRespondent_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles datRespondent.SelectedIndexChanging
        Dim strKeyID As String = datRespondent.DataKeys(e.NewSelectedIndex).Value.ToString
        Response.Redirect("kelas.view.aspx?KelasID=" & strKeyID)

    End Sub
   
    Private Sub ddlTahun_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTahun.SelectedIndexChanged
        kpmkv_namakelas_list()
    End Sub
End Class