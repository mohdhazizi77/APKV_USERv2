Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization

Public Class kursus_view
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""
    Dim strTxnKursusKolejID As String = ""
    Dim strKursusID As String = ""
    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnExecute.Attributes.Add("onclick", "return confirm('Pasti ingin menghapuskan rekod ini?');")
        Try
            If Not IsPostBack Then

                strTxnKursusKolejID = Request.QueryString("TxnKursusKolejID")

                'kursusID
                strSQL = "SELECT KursusID FROM kpmkv_kursus_kolej WHERE TxnKursusKolejID='" & strTxnKursusKolejID & "'"
                Dim strKursusID As String = oCommon.getFieldValue(strSQL)

                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                lblMsg.Text = ""
                LoadPage()



            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try

    End Sub
    Private Sub LoadPage()


        strSQL = "SELECT kpmkv_kursus_kolej.KursusID,kpmkv_kursus_kolej.TxnKursusKolejID, kpmkv_kluster.NamaKluster, kpmkv_kursus.Tahun, kpmkv_kursus.Sesi,"
        strSQL += "kpmkv_kursus_kolej.KolejRecordID, kpmkv_kursus.KodKursus,kpmkv_kursus.NamaKursus FROM kpmkv_kursus_kolej LEFT OUTER JOIN kpmkv_kursus "
        strSQL += "ON kpmkv_kursus_kolej.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN kpmkv_kluster ON kpmkv_kursus.KlusterID = kpmkv_kluster.KlusterID"
        strSQL += " WHERE kpmkv_kursus_kolej.TxnKursusKolejID='" & Request.QueryString("TxnKursusKolejID") & "' AND kpmkv_kursus_kolej.IsDeleted='N'"


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

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Namakursus")) Then
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
        Dim strOrder As String = " ORDER BY kpmkv_modul.Semester, kpmkv_modul.KodModul"

        tmpSQL = "SELECT kpmkv_modul.ModulID, kpmkv_modul.Semester, kpmkv_modul.KodModul, kpmkv_modul.NamaModul, kpmkv_modul.JamKredit FROM kpmkv_kursus LEFT OUTER JOIN"
        tmpSQL += " kpmkv_modul ON kpmkv_kursus.KursusID = kpmkv_modul.KursusID	"
        strWhere = " WHERE kpmkv_kursus.KursusID='" & strKursusID & "' AND kpmkv_modul.IsDeleted<>'Y'"


        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        'Response.Write(getSQL)

        Return getSQL

    End Function
    'Private Sub datRespondent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondent.PageIndexChanging
    '    'datRespondent.PageIndex = e.NewPageIndex
    '    'strRet = BindData(datRespondent)

    'End Sub
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

    Protected Sub btnExecute_Click(sender As Object, e As EventArgs) Handles btnExecute.Click
        Try
            'cheking data
            'kolejnama
            strSQL = "SELECT KursusID FROM kpmkv_kursus_kolej WHERE TxnKursusKolejID='" & Request.QueryString("TxnKursusKolejID") & "'"
            Dim Kursus As String = oCommon.getFieldValue(strSQL)


            ''kpmkv_pelajar
            'strSQL = "SELECT KursusID FROM kpmkv_pelajar WHERE KursusID='" & Kursus & "'"

            'If oCommon.isExist(strSQL) = True Then
            '    divMsg.Attributes("class") = "error"
            '    lblMsg.Text = "Kursus tidak berjaya dipadamkan. Rekod kursus masih wujud pada Calon."

            '    Exit Sub
            'End If

            ''kpmkv_pelajar_ulang
            'strSQL = "SELECT KursusID FROM kpmkv_pelajar_ulang WHERE KursusID='" & Kursus & "'"
            'If oCommon.isExist(strSQL) = True Then
            '    divMsg.Attributes("class") = "error"
            '    lblMsg.Text = "Kursus tidak berjaya dipadamkan. Rekod kursus masih wujud pada Calon Ulang."

            '    Exit Sub
            'End If

            ''kpmkv_pensyarah_modul
            'strSQL = "SELECT KursusID FROM kpmkv_pensyarah_modul WHERE KursusID='" & Kursus & "'"
            'If oCommon.isExist(strSQL) = True Then
            '    divMsg.Attributes("class") = "error"
            '    lblMsg.Text = "Kursus tidak berjaya dipadamkan. Rekod kursus masih wujud pada Modul."

            '    Exit Sub
            'End If

           

            strSQL = "DELETE kpmkv_kursus_kolej  WHERE TxnKursusKolejID='" & Request.QueryString("TxnKursusKolejID") & "'"
            strRet = oCommon.ExecuteSQL(strSQL)
            If strRet = "0" Then
                lblMsg.Text = "Kursus berjaya dipadamkan"
            Else
                lblMsg.Text = "System Error:" & strRet
            End If

        Catch ex As Exception
            divMsg.Attributes("class") = "error"
        End Try
    End Sub
End Class