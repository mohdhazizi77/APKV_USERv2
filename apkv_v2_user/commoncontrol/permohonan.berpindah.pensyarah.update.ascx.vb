Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class permohonan_berpindah_pensyarah_update
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


                '--set default date
                txtEventDate.Text = Format(CDate(Date.Now), "dd-MM-yyyy")

                kpmkv_jeniskolej_list()
                kpmkv_namakolej_list()

                lblMsg.Text = ""
                LoadPage()

            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try
    End Sub

    Private Sub kpmkv_jeniskolej_list()
        strSQL = "SELECT Jenis FROM kpmkv_kolej GROUP BY Jenis"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlJenisKolej.DataSource = ds
            ddlJenisKolej.DataTextField = "Jenis"
            ddlJenisKolej.DataValueField = "Jenis"
            ddlJenisKolej.DataBind()

            '--ALL
            ' ddlJenisKolej.Items.Add(New ListItem("PILIH", "PILIH"))
        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_namakolej_list()
        strSQL = "SELECT Nama,RecordID FROM kpmkv_kolej WHERE Jenis='" & ddlJenisKolej.Text & "' ORDER BY Nama"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlNamaKolej.DataSource = ds
            ddlNamaKolej.DataTextField = "Nama"
            ddlNamaKolej.DataValueField = "RecordID"
            ddlNamaKolej.DataBind()

            '--ALL
            ddlNamaKolej.Items.Add(New ListItem("PILIH", "PILIH"))
        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub LoadPage()
        strSQL = "SELECT kpmkv_pensyarah.MYKAD, kpmkv_pensyarah.Nama, kpmkv_pensyarah.Jawatan, kpmkv_pensyarah.Gred, kpmkv_pensyarah.Tel, "
        strSQL += " kpmkv_pensyarah.Email, kpmkv_pensyarah.Jantina, kpmkv_pensyarah.Bangsa, kpmkv_pensyarah.Kaum, kpmkv_pensyarah.Agama, kpmkv_status.Status"
        strSQL += " FROM  kpmkv_pensyarah LEFT OUTER JOIN"
        strSQL += " kpmkv_status ON kpmkv_pensyarah.StatusID = kpmkv_status.StatusID"
        strSQL += " WHERE kpmkv_pensyarah.IsDeleted='N' AND kpmkv_pensyarah.StatusID='2' AND kpmkv_pensyarah.PensyarahID='" & Request.QueryString("PensyarahID") & "'"

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
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("MYKAD")) Then
                    lblMykad.Text = ds.Tables(0).Rows(0).Item("MYKAD")
                Else
                    lblMykad.Text = ""
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
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Email")) Then
                    lblEmail.Text = ds.Tables(0).Rows(0).Item("Email")
                Else
                    lblEmail.Text = ""
                End If
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub ddlJenisKolej_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlJenisKolej.SelectedIndexChanged
        kpmkv_namakolej_list()

    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        lblMsg.Text = ""

        Try
            'check form validation. if failed exit
            If ValidateForm() = False Then
                Exit Sub
            End If

            '--execute

            If kpmkv_pensyarah_update() = 0 Then
            Else
                strSQL = "UPDATE kpmkv_pensyarah SET KolejRecordID='" & lblKolejID.Text & "', DateTransfer='', RefNoTransfer='' WHERE PensyarahID='" & Request.QueryString("PensyarahID") & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

                strSQL = "Delete From kpmkv_pensyarah_history WHERE PensyarahID='" & Request.QueryString("PensyarahID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Permohonan Pensyarah berpindah tidak berjaya!."
            End If

            If kpmkv_Pensyarah_history() = 0 Then
            Else
                strSQL = "UPDATE kpmkv_pensyarah SET KolejRecordID='" & lblKolejID.Text & "', DateTransfer='', RefNoTransfer='' WHERE PensyarahID='" & Request.QueryString("PensyarahID") & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

                strSQL = "Delete From kpmkv_pensyarah_history WHERE PensyarahID='" & Request.QueryString("PensyarahID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Permohonan Pensyarah berpindah tidak berjaya!."
            End If

            If lblMsg.Text.Length = 0 Then
                lblMsg.Text = "Permohonan Pensyarah berpindah berjaya!."
            End If

        Catch ex As Exception
            lblMsg.Text = "System error:" & ex.Message
        End Try
        btnUpdate.Enabled = False
    End Sub

    '--CHECK form validation.
    Private Function ValidateForm() As Boolean
        If txtEventDate.Text.Length = 0 Then
            lblMsg.Text = "Sila pilih Tarikh."
            txtEventDate.Focus()
            Return False
        End If

        If chkconfirm.Checked = False Then
            lblMsg.Text = "Sila Tick() pada kotak yang disediakan."
            chkconfirm.Focus()
            Return False
        End If

        'If txtRef.Text.Length = 0 Then
        '    lblMsg.Text = "Sila masukkan No. Rujukan."
        '    txtRef.Focus()
        '    Return False
        'End If

        Return True
    End Function
    Private Function kpmkv_Pensyarah_history() As String
        'insert into course list
        strSQL = "INSERT INTO kpmkv_pensyarah_history(GUIDPensyarah, KolejRecordID, MYKAD, Nama, Jawatan, Gred, Tel, Email, Jantina, Bangsa, Kaum, Agama, StatusID, "
        strSQL += " DateCreated, CreateBy, IsDeleted, UploadCode, IsApproved, DateTransfer)"
        strSQL += "Select GUIDPensyarah, KolejRecordID, MYKAD, Nama, Jawatan, Gred, Tel, Email, Jantina, Bangsa, Kaum, Agama, StatusID, "
        strSQL += " DateCreated, CreateBy, IsDeleted, UploadCode, IsApproved, DateTransfer From kpmkv_pensyarah WHERE PensyarahID='" & Request.QueryString("PensyarahID") & "'"

        strRet = oCommon.ExecuteSQL(strSQL)
        ''--debug
        'Response.Write(SQL)

        Return strRet

    End Function
    
    Private Function kpmkv_pensyarah_update() As String
        'insert into course list
        strSQL = "UPDATE kpmkv_pensyarah SET KolejRecordID='" & ddlNamaKolej.SelectedValue & "', DateTransfer='" & Request.Form(txtEventDate.UniqueID) & "' WHERE PensyarahID='" & Request.QueryString("PensyarahID") & "'"
        strRet = oCommon.ExecuteSQL(strSQL)
        ''--debug
        'Response.Write(SQL)

        Return strRet

    End Function

End Class