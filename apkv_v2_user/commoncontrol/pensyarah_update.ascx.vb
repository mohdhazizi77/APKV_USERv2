Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class pensyarah_update
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

                lblPensyarahID.Text = Request.QueryString("PensyarahID")

                strSQL = "SELECT KolejRecordID FROM kpmkv_pensyarah WHERE PensyarahID='" & Request.QueryString("PensyarahID") & "'"
                Dim strKolejRecordID As String = oCommon.getFieldValue(strSQL)

                strSQL = "SELECT Nama FROM kpmkv_kolej WHERE RecordID='" & strKolejRecordID & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                kpmkv_jantina_list()
                kpmkv_kaum_list()
                kpmkv_agama_list()
                kpmkv_status_list()

                LoadPage()

            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try

    End Sub
    Private Sub kpmkv_jantina_list()
        strSQL = "SELECT Jantina FROM kpmkv_jantina WITH (NOLOCK) ORDER BY Jantina"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlJantina.DataSource = ds
            ddlJantina.DataTextField = "Jantina"
            ddlJantina.DataValueField = "Jantina"
            ddlJantina.DataBind()

            ddlJantina.Items.Add(New ListItem("--Pilih--", "00"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Sub kpmkv_kaum_list()
        strSQL = "SELECT Kaum FROM kpmkv_kaum WITH (NOLOCK) ORDER BY Kaum"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlKaum.DataSource = ds
            ddlKaum.DataTextField = "Kaum"
            ddlKaum.DataValueField = "Kaum"
            ddlKaum.DataBind()

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Sub kpmkv_agama_list()
        strSQL = "SELECT Agama FROM kpmkv_agama WITH (NOLOCK) ORDER BY Agama"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlAgama.DataSource = ds
            ddlAgama.DataTextField = "Agama"
            ddlAgama.DataValueField = "Agama"
            ddlAgama.DataBind()

            ddlAgama.Items.Add(New ListItem("--Pilih--", "00"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_status_list()
        strSQL = "SELECT Status,StatusID FROM kpmkv_status ORDER BY Status"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlStatus.DataSource = ds
            ddlStatus.DataTextField = "Status"
            ddlStatus.DataValueField = "StatusID"
            ddlStatus.DataBind()

            '--ALL
            ddlStatus.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub LoadPage()
        strSQL = "SELECT * FROM kpmkv_pensyarah WHERE PensyarahID='" & Request.QueryString("PensyarahID") & "'"
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
                    txtNama.Text = ds.Tables(0).Rows(0).Item("Nama")
                Else
                    txtNama.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("MYKAD")) Then
                    txtMYKAD.Text = ds.Tables(0).Rows(0).Item("MYKAD")
                Else
                    txtMYKAD.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Jawatan")) Then
                    txtJawatan.Text = ds.Tables(0).Rows(0).Item("Jawatan")
                Else
                    txtJawatan.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Gred")) Then
                    txtGred.Text = ds.Tables(0).Rows(0).Item("Gred")
                Else
                    txtGred.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Tel")) Then
                    txtTel.Text = ds.Tables(0).Rows(0).Item("Tel")
                Else
                    txtTel.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Email")) Then
                    txtEmail.Text = ds.Tables(0).Rows(0).Item("Email")
                Else
                    txtEmail.Text = ""
                End If
                '--
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Jantina")) Then
                    ddlJantina.Text = ds.Tables(0).Rows(0).Item("Jantina")
                Else
                    ddlJantina.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Kaum")) Then
                    ddlKaum.Text = ds.Tables(0).Rows(0).Item("Kaum")
                Else
                    ddlKaum.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Agama")) Then
                    ddlAgama.Text = ds.Tables(0).Rows(0).Item("Agama")
                Else
                    ddlAgama.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("StatusID")) Then
                    ddlStatus.Text = ds.Tables(0).Rows(0).Item("StatusID")
                Else
                    ddlStatus.Text = ""
                End If

            End If

        Catch ex As Exception
            divMsg.Attributes("class") = "error"
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        lblMsg.Text = ""

        Try
            '--validate
            If ValidatePage() = False Then
                divMsg.Attributes("class") = "error"
                Exit Sub
            End If

            '--execute
            If kpmkv_pensyarah_update() = True Then
                divMsg.Attributes("class") = "info"
                lblMsg.Text = "Berjaya mengemaskini maklumat pensyarah."
            Else
                divMsg.Attributes("class") = "error"
            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try

    End Sub

    Private Function ValidatePage() As Boolean
        '--txtNama
        If txtNama.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan Nama!"
            txtNama.Focus()
            Return False
        End If

        '--txtMYKAD
        If txtMYKAD.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan MYKAD Pensyarah!"
            txtMYKAD.Focus()
            Return False
        ElseIf oCommon.isNumeric(txtMYKAD.Text) = False Then
            lblMsg.Text = "Huruf tidak dibenarkan .Sila masukkan no MYKAD [0###########]"
            txtMYKAD.Focus()
            Return False
        End If

        '--changes made
        If Not txtMYKAD.Text = txtMYKAD.Text Then
            strSQL = "SELECT MYKAD FROM kpmkv_pensyarah WHERE MYKAD='" & oCommon.FixSingleQuotes(txtMYKAD.Text) & "'"
            If oCommon.isExist(strSQL) = True Then
                lblMsg.Text = "MYKAD# telah digunakan. Sila masukkan MYKAD# yang baru."
                txtMYKAD.Focus()
                Return False
            End If
        End If

        '--txtJawatan
        If txtJawatan.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan jawatan!"
            txtJawatan.Focus()
            Return False
        End If

        '--txtGred
        If txtGred.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan Gred!"
            txtGred.Focus()
            Return False
        End If


        '--txtTelefon
        If txtTel.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan Telefon!"
            txtTel.Focus()
            Return False
        ElseIf oCommon.isNumeric(txtTel.Text) = False Then
            lblMsg.Text = "Huruf tidak dibenarkan .Sila masukkan no telefon [0########]"
            txtTel.Focus()
            Return False
        End If

        '--txtEmail
        If txtEmail.Text.Length = 0 Then
            lblMsg.Text = "Sila masukkan Email Pensyarah!"
        ElseIf oCommon.isEmail(txtEmail.Text) = False Then
            lblMsg.Text = "Emel Pensyarah tidak sah. (Contoh: Pensyarah@contoh.com)"
            txtEmail.Focus()
            Return False
        End If

        



        Return True
    End Function

    Private Function kpmkv_pensyarah_update() As Boolean
        strSQL = "UPDATE kpmkv_pensyarah SET Nama='" & oCommon.FixSingleQuotes(txtNama.Text.ToUpper) & "',MYKAD='" & oCommon.FixSingleQuotes(txtMYKAD.Text) & "',Tel='" & oCommon.FixSingleQuotes(txtTel.Text) & "',Email='" & oCommon.FixSingleQuotes(txtEmail.Text) & "',Jantina='" & oCommon.FixSingleQuotes(ddlJantina.Text) & "',Kaum='" & ddlKaum.Text & "',Agama='" & oCommon.FixSingleQuotes(ddlAgama.Text) & "',StatusID='" & ddlStatus.SelectedValue & "' ,Jawatan='" & txtJawatan.Text & "',Gred='" & txtGred.Text & "' WHERE PensyarahID='" & Request.QueryString("PensyarahID") & "'"
        strRet = oCommon.ExecuteSQL(strSQL)
        If strRet = "0" Then

            strSQL = "SELECT MYKAD FROM kpmkv_pensyarah WHERE PensyarahID='" & Request.QueryString("PensyarahID") & "'"
            Dim strMYKAD As String = oCommon.getFieldValue(strSQL)
            If Not strMYKAD = 0 Then

                strSQL = "UPDATE kpmkv_users SET Nama='" & oCommon.FixSingleQuotes(txtNama.Text.ToUpper) & "',MYKAD='" & oCommon.FixSingleQuotes(txtMYKAD.Text) & "',Tel='" & oCommon.FixSingleQuotes(txtTel.Text) & "',Email='" & oCommon.FixSingleQuotes(txtEmail.Text) & "',StatusID='" & ddlStatus.SelectedValue & "' WHERE MyKad='" & strMYKAD & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
                If strRet = "0" Then
                    Return True
                End If
            End If

        Else
            lblMsg.Text = "System Error:" & strRet
            Return False
        End If
        Return True
    End Function

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        strSQL = "UPDATE kpmkv_pensyarah WITH (UPDLOCK) SET IsDeleted='Y' WHERE PensyarahID='" & Request.QueryString("PensyarahID") & "'"
        strRet = oCommon.ExecuteSQL(strSQL)
        If strRet = "0" Then
            strSQL = "SELECT MYKAD FROM kpmkv_pensyarah WHERE PensyarahID='" & Request.QueryString("PensyarahID") & "'"
            Dim strMYKAD As String = oCommon.getFieldValue(strSQL)
            If Not strMYKAD = 0 Then
                strSQL = "UPDATE kpmkv_users SET StatusID='" & ddlStatus.SelectedValue & "' WHERE MyKad='" & strMYKAD & "'"
                strRet = oCommon.ExecuteSQL(strSQL)
                lblMsg.Text = "Berjaya meghapuskan rekod pensyarah tersebut."
            End If
        Else
            lblMsg.Text = "System Error:" & strRet
        End If

    End Sub

End Class