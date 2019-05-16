Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization

Partial Public Class pelajar_view
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblMsg.Text = ""
        Try
            If Not IsPostBack Then

                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                LoadPage()

                strSQL = "SELECT KlusterID FROM kpmkv_kursus WHERE KodKursus='" & lblKodKursus.Text & "' AND Tahun = '" & lblTahun.Text & "' AND Sesi = '" & lblSesi.Text & "'"
                Dim strKlusterID As String = oCommon.getFieldValue(strSQL)

                strSQL = "SELECT NamaKluster FROM kpmkv_kluster WHERE KlusterID='" & strKlusterID & "' AND Tahun = '" & lblTahun.Text & "'"
                lblKluster.Text = oCommon.getFieldValue(strSQL)


                ' strRet = BindData(datRespondent)
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try

    End Sub
    Private Sub LoadPage()
        strSQL = "SELECT kpmkv_pelajar.Pengajian, kpmkv_pelajar.Tahun, kpmkv_pelajar.Semester, kpmkv_pelajar.Sesi, kpmkv_pelajar.Nama, kpmkv_pelajar.MYKAD, kpmkv_pelajar.AngkaGiliran, "
        strSQL += " kpmkv_kursus.KursusID, kpmkv_jeniscalon.JenisCalon, kpmkv_kursus.KursusID, kpmkv_kursus.KodKursus, kpmkv_kursus.NamaKursus, kpmkv_pelajar.Kaum, kpmkv_pelajar.Jantina, "
        strSQL += " kpmkv_pelajar.Agama, kpmkv_status.Status, kpmkv_pelajar.Email, kpmkv_pelajar.Catatan, kpmkv_kelas.NamaKelas"
        strSQL += " FROM  kpmkv_pelajar LEFT OUTER JOIN kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
        strSQL += " kpmkv_status ON kpmkv_pelajar.StatusID = kpmkv_status.StatusID LEFT OUTER JOIN kpmkv_kelas ON kpmkv_pelajar.KelasID = kpmkv_kelas.KelasID"
        strSQL += " LEFT OUTER JOIN kpmkv_jeniscalon ON kpmkv_pelajar.JenisCalonID = kpmkv_jeniscalon.JenisCalonID "
        strSQL += " WHERE kpmkv_pelajar.IsDeleted='N' AND kpmkv_pelajar.PelajarID='" & Request.QueryString("PelajarID") & "'"


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

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Pengajian")) Then
                    lblPengajian.Text = ds.Tables(0).Rows(0).Item("Pengajian")
                Else
                    lblPengajian.Text = ""
                End If

                '--Account Details 
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
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("KursusID")) Then
                    lblKursusID.Text = ds.Tables(0).Rows(0).Item("KursusID")
                Else
                    lblKursusID.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("NamaKursus")) Then
                    lblNamaKursus.Text = ds.Tables(0).Rows(0).Item("NamaKursus")
                Else
                    lblNamaKursus.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("KodKursus")) Then
                    lblKodKursus.Text = ds.Tables(0).Rows(0).Item("KodKursus")
                Else
                    lblKodKursus.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("NamaKelas")) Then
                    lblNamaKelas.Text = ds.Tables(0).Rows(0).Item("NamaKelas")
                Else
                    lblNamaKelas.Text = ""
                End If
                'personal
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Nama")) Then
                    lblNama.Text = ds.Tables(0).Rows(0).Item("Nama")
                Else
                    lblNama.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Mykad")) Then
                    lblMYKAD.Text = ds.Tables(0).Rows(0).Item("Mykad")
                Else
                    lblMYKAD.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("AngkaGiliran")) Then
                    lblAngkaGiliran.Text = ds.Tables(0).Rows(0).Item("AngkaGiliran")
                Else
                    lblAngkaGiliran.Text = ""
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
                    lblStatus.Text = ds.Tables(0).Rows(0).Item("Status")
                Else
                    lblStatus.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("JenisCalon")) Then
                    lblJenisCalon.Text = ds.Tables(0).Rows(0).Item("JenisCalon")
                Else
                    lblJenisCalon.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Email")) Then
                    lblEmail.Text = ds.Tables(0).Rows(0).Item("Email")
                Else
                    lblEmail.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Catatan")) Then
                    lblCatatan.Text = ds.Tables(0).Rows(0).Item("Catatan")
                Else
                    lblCatatan.Text = ""
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
                lblMsg.Text = "Tiada rekod pelajar."
            Else
                divMsg.Attributes("class") = "info"
                lblMsg.Text = "Jumlah rekod#:" & myDataSet.Tables(0).Rows.Count
            End If

            gvTable.DataSource = myDataSet
            gvTable.DataBind()
            objConn.Close()

        Catch ex As Exception
            lblMsg.Text = "Error:" & ex.Message
            Return False
        End Try

        Return True

    End Function

    Private Function getSQL() As String

        Dim tmpSQL As String = ""
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY Semester, KodModul"

        tmpSQL = " SELECT * FROM kpmkv_modul "
        strWhere = " WHERE KursusID='" & lblKursusID.Text & "'"
        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        ''Response.Write(getSQL)

        Return getSQL

    End Function
    Protected Sub btnExecute_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExecute.Click
        Response.Redirect("pelajar.update.aspx?PelajarID=" & Request.QueryString("PelajarID"))
    End Sub

End Class