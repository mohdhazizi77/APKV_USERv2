Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization

Public Class kursus_list
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

                kpmkv_namakluster_list()
                ddlNamaKluster.Text = "0"

                kpmkv_kodkursus_list()
                ddlKodKursus.Text = "0"

                lblMsg.Text = ""
                strRet = BindData(datRespondent)
            End If


        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try
    End Sub
    Private Sub kpmkv_namakluster_list()
        strSQL = "SELECT kpmkv_kluster.NamaKluster,kpmkv_kluster.KlusterID FROM kpmkv_kursus_kolej LEFT OUTER JOIN kpmkv_kursus "
        strSQL += " ON kpmkv_kursus_kolej.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN kpmkv_kluster "
        strSQL += " ON kpmkv_kursus.KlusterID = kpmkv_kluster.KlusterID"
        strSQL += " WHERE kpmkv_kursus.Tahun='" & ddlTahun.SelectedValue & "' AND  kpmkv_kursus.Sesi='" & chkSesi.SelectedValue & "' AND kpmkv_kursus_kolej.KolejRecordID='" & lblKolejID.Text & "'"
        strSQL += " GROUP BY kpmkv_kluster.NamaKluster,kpmkv_kluster.KlusterID ORDER BY kpmkv_kluster.NamaKluster "

        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlNamaKluster.DataSource = ds
            ddlNamaKluster.DataTextField = "NamaKluster"
            ddlNamaKluster.DataValueField = "KlusterID"
            ddlNamaKluster.DataBind()

            '--ALL
            ddlNamaKluster.Items.Add(New ListItem("-Semua Kluster-", "0"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_kodkursus_list()
        strSQL = "SELECT kpmkv_kursus.KodKursus,kpmkv_kursus.KursusID FROM kpmkv_kursus_kolej LEFT OUTER JOIN kpmkv_kursus "
        strSQL += " ON kpmkv_kursus_kolej.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN kpmkv_kluster "
        strSQL += " ON kpmkv_kursus.KlusterID = kpmkv_kluster.KlusterID"
        strSQL += " WHERE kpmkv_kursus_kolej.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kursus.Tahun='" & ddlTahun.SelectedValue & "'"
        strSQL += " AND kpmkv_kursus.Sesi='" & chkSesi.SelectedValue & "'  AND kpmkv_kluster.KlusterID='" & ddlNamaKluster.SelectedValue & "'"
        strSQL += " GROUP BY kpmkv_kursus.KodKursus,kpmkv_kursus.KursusID ORDER BY kpmkv_kursus.KodKursus"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlKodKursus.DataSource = ds
            ddlKodKursus.DataTextField = "KodKursus"
            ddlKodKursus.DataValueField = "KodKursus"
            ddlKodKursus.DataBind()

            '--ALL
            ddlKodKursus.Items.Add(New ListItem("-Semua KodKursus-", "0"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_tahun_list()
        strSQL = "SELECT Tahun FROM kpmkv_tahun WITH (NOLOCK) ORDER BY TahunID"
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

            '--ALL
            'ddlTahun.Items.Add(New ListItem("PILIH", "PILIH"))

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
        Dim strOrder As String = " ORDER BY kpmkv_kursus.Tahun, kpmkv_kursus.KodKursus ASC"

        tmpSQL = "SELECT kpmkv_kursus_kolej.KursusID,kpmkv_kursus_kolej.TxnKursusKolejID, kpmkv_kluster.NamaKluster, kpmkv_kursus.Tahun, kpmkv_kursus.Sesi,"
        tmpSQL += " kpmkv_kursus_kolej.KolejRecordID, kpmkv_kursus.KodKursus,kpmkv_kursus.NamaKursus FROM kpmkv_kursus_kolej LEFT OUTER JOIN kpmkv_kursus "
        tmpSQL += " ON kpmkv_kursus_kolej.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN kpmkv_kluster "
        tmpSQL += " ON kpmkv_kursus.KlusterID = kpmkv_kluster.KlusterID"
        strWhere = "  WHERE kpmkv_kursus_kolej.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kursus_kolej.IsDeleted='N'"

        '--kluster
        If Not ddlNamaKluster.Text = "0" Then
            strWhere += " AND kpmkv_kursus.KlusterID='" & ddlNamaKluster.SelectedValue & "' "
        End If

        '--KodKursus
        If Not ddlKodKursus.Text = "0" Then
            strWhere += " AND kpmkv_kursus.Kodkursus='" & ddlKodKursus.SelectedValue & "'"
        End If


        '--NamaKursus
        If Not txtNamaKursus.Text = "" Then
            strWhere += " AND kpmkv_kursus.NamaKursus LIKE '%" & oCommon.FixSingleQuotes(txtNamaKursus.Text) & "%'"
        End If

        '--Tahun
        If Not ddlTahun.Text = "PILIH" Then
            strWhere += " AND kpmkv_kursus.Tahun='" & ddlTahun.Text & "'"
        End If

        If Not chkSesi.SelectedValue = "" Then
            strWhere += " AND kpmkv_kursus.Sesi='" & chkSesi.SelectedValue & "'"
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
        End Try
        con.Close()
        sda.Dispose()
        con.Dispose()

    End Function
    Private Sub datRespondent_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles datRespondent.SelectedIndexChanging
        Dim strKeyID As String = datRespondent.DataKeys(e.NewSelectedIndex).Value

        Response.Redirect("kursus.view.aspx?TxnKursusKolejID=" & strKeyID)
    End Sub

    Protected Sub chkSesi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkSesi.SelectedIndexChanged
        kpmkv_namakluster_list()
        kpmkv_kodkursus_list()

        strSQL = "SELECT NamaKursus FROM kpmkv_kursus WHERE KodKursus='" & ddlKodKursus.SelectedValue & "'"
        txtNamaKursus.Text = oCommon.getFieldValue(strSQL)
    End Sub

    Protected Sub ddlNamaKluster_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlNamaKluster.SelectedIndexChanged
        kpmkv_kodkursus_list()
        strSQL = "SELECT NamaKursus FROM kpmkv_kursus WHERE KodKursus='" & ddlKodKursus.SelectedValue & "'"
        txtNamaKursus.Text = oCommon.getFieldValue(strSQL)

    End Sub

    Private Sub ddlKodKursus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlKodKursus.SelectedIndexChanged
        strSQL = "SELECT NamaKursus FROM kpmkv_kursus WHERE KodKursus='" & ddlKodKursus.SelectedValue & "'"
        txtNamaKursus.Text = oCommon.getFieldValue(strSQL)

    End Sub
End Class