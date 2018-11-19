Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Imports System.Data.Common
Public Class pensyarah_import
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strBil As String = ""
    Dim strNama As String = ""
    Dim strJawatan As String = ""
    Dim strGred As String = ""
    Dim strMykad As String = ""
    Dim strEmail As String = ""
    Dim strJantina As String = ""
    Dim strAgama As String = ""
    Dim strKaum As String = ""
    Dim strTel As String = ""
    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'btnCancel.Attributes.Add("onclick", "return confirm('Pasti ingin MEMBATALKAN kemasukan data tersebut?');")
        'btnApprove.Attributes.Add("onclick", "return confirm('Pasti ingin MENGESAHKAN kemasukan data tersebut?');")

        Try
            If Not IsPostBack Then
                'btnApprove.Enabled = True
                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejRecordID.Text = oCommon.getFieldValue(strSQL)

            End If

        Catch ex As Exception
            lblMsg.Text = "Error Message:" & ex.Message
        End Try

    End Sub
    Private Sub btnFile_Click(sender As Object, e As EventArgs) Handles btnFile.Click
        Response.ContentType = "Application/xlsx"
        Response.AppendHeader("Content-Disposition", "attachment; filename=PENSYARAHIMPORT.xlsx")
        Response.TransmitFile(Server.MapPath("~/sample_data/PENSYARAHIMPORT.xlsx"))
        Response.End()
    End Sub
    Private Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        lblMsg.Text = ""
        'lblMsgResult.Text = ""
        'btnApprove.Enabled = True
        Try
            '--upload excel
            lblUploadCode.Text = oCommon.getRandom
            If ImportExcel() = True Then
                divMsg.Attributes("class") = "info"
                'tbl_menu_check()
                '--display rekod if success
            Else
                divMsg.Attributes("class") = "error"
                'lblMsg.Text = "Gagal untuk memuatnaik fail"
            End If
        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

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
        Dim strOrder As String = " ORDER BY Nama ASC"

        tmpSQL = "SELECT * FROM kpmkv_pensyarah"
        strWhere = " WITH (NOLOCK) WHERE KolejRecordID='" & Request.QueryString("RecordID") & "' AND IsApproved='N'"

        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        ''Response.Write(getSQL)

        Return getSQL

    End Function
    Private Function ImportExcel() As Boolean
        Dim path As String = String.Concat(Server.MapPath("~/inbox/"))

        If FlUploadcsv.HasFile Then
            Dim rand As Random = New Random()
            Dim randNum = rand.Next(1000)
            Dim fullFileName As String = path + oCommon.getRandom + "-" + FlUploadcsv.FileName
            FlUploadcsv.PostedFile.SaveAs(fullFileName)

            '--required ms access engine
            Dim excelConnectionString As String = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & fullFileName & ";Extended Properties=Excel 12.0;")
            'Dim excelConnectionString As String = ("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & fullFileName & ";Extended Properties=Excel 8.0;HDR=YES;")
            'Response.Write("excelConnectionString:" & excelConnectionString)

            Dim connection As OleDbConnection = New OleDbConnection(excelConnectionString)
            Dim command As OleDbCommand = New OleDbCommand("SELECT * FROM [pensyarah$]", connection)
            Dim da As OleDbDataAdapter = New OleDbDataAdapter(command)
            Dim ds As DataSet = New DataSet

            Try
                connection.Open()
                da.Fill(ds)
                Dim validationMessage As String = ValidateSiteData(ds)
                If validationMessage = "" Then
                    '    datRespondent.DataSource = ds.Tables(0)
                    '    datRespondent.DataBind()
                    'Else
                    '    btnApprove.Visible = False
                    '    lblMsgResult.Text = "Muatnaik GAGAL!. Lihat mesej dibawah."
                    '    lblMsg.Text = "Kesalahan Kemasukkan Maklumat Pensyarah:<br />" & validationMessage
                    '    Return False
                    'End If
                    SaveSiteData(ds)
                    
                Else
                    'lblMsgTop.Text = "Muatnaik GAGAL!. Lihat mesej dibawah."
                    lblMsg.Text = "Kesalahan Kemasukkan Maklumat Pensyarah:<br />" & validationMessage
                    Return False
                End If

                da.Dispose()
                connection.Close()
                command.Dispose()

            Catch ex As Exception
                lblMsg.Text = "System Error:" & ex.Message
                Return False
            Finally
                If connection.State = ConnectionState.Open Then
                    connection.Close()
                End If
            End Try

        Else
            divMsg.Attributes("class") = "error"
            lblMsg.Text = "Please select file to upload!"
            Return False
        End If

        Return True
    End Function
    Private Sub refreshVar()
        strBil = ""
        strNama = ""
        strJawatan = ""
        strGred = ""
        strMykad = ""
        strEmail = ""
        strJantina = ""
        strAgama = ""
        strKaum = ""
        strTel = ""

    End Sub

    Protected Function ValidateSiteData(ByVal SiteData As DataSet) As String
        Try
            'Loop through DataSet and validate data
            'If data is bad, bail out, otherwise continue on with the bulk copy
            Dim sb As StringBuilder = New StringBuilder()
            Dim strMsg As String = ""
            For i As Integer = 0 To SiteData.Tables(0).Rows.Count - SiteData.Tables(0).Rows(i).Item("BIL")
                'refresh variable
                refreshVar()
                strMsg = ""

                'bil
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("BIL")) Then
                    strBil = SiteData.Tables(0).Rows(i).Item("BIL")
                End If
                'Jawatan
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Jawatan")) Then
                    strJawatan = SiteData.Tables(0).Rows(i).Item("Jawatan")
                Else
                    strMsg += "Sila isi Jawatan|"
                End If
                'Gred
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Gred")) Then
                    strGred = SiteData.Tables(0).Rows(i).Item("Gred")
                Else
                    strMsg += "Sila isi Gred|"
                End If
                'Nama
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Nama")) Then
                    strNama = SiteData.Tables(0).Rows(i).Item("Nama")
                Else
                    strMsg += "Sila isi Nama|"
                End If
                'Jantina
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Jantina")) Then
                    strJantina = SiteData.Tables(0).Rows(i).Item("Jantina")
                Else
                    strMsg += "Sila isi Jantina|"
                End If
                'Agama
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Agama")) Then
                    strAgama = SiteData.Tables(0).Rows(i).Item("Agama")
                Else
                    strMsg += "Sila isi Agama|"
                End If
                'Kaum
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Kaum")) Then
                    strKaum = SiteData.Tables(0).Rows(i).Item("Kaum")
                Else
                    strMsg += "Sila isi Kaum|"
                End If
                '--MYKAD is required!
                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Mykad")) Then
                    strMykad = SiteData.Tables(0).Rows(i).Item("Mykad")
                Else
                    strMsg += "Sila isi mykad|"
                End If

                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Email")) Then
                    strEmail = SiteData.Tables(0).Rows(i).Item("Email")
                    'Else
                    '    strMsg += "Sila isi Email|"
                End If

                If Not IsDBNull(SiteData.Tables(0).Rows(i).Item("Tel")) Then
                    strTel = SiteData.Tables(0).Rows(i).Item("Tel")
                    'Else
                    '    strMsg += "Sila isi Email|"
                End If
                strSQL = "SELECT Mykad FROM kpmkv_pensyarah Where Mykad='" & strMykad & "' AND IsDeleted='N'"
                If oCommon.isExist(strSQL) = True Then
                    strMsg += "Mykad:" & strMykad & ":" & strNama & ". Mykad ini telah wujud."
                End If

                If strMsg.Length = 0 Then
                    'strMsg = "Record#:" & i.ToString & "OK"
                    'strMsg += "<br/>"
                Else
                    strMsg = "Bil#:" & strBil & ":" & strMsg
                    strMsg += "<br/>"
                End If

                sb.Append(strMsg)
                'disp bil
            Next

            Return sb.ToString()
        Catch ex As Exception
            Return ex.Message
        End Try

    End Function
    'Private Sub tbl_menu_check()

    '    Dim str As String
    '    For i As Integer = 0 To datRespondent.Rows.Count - 1
    '        Dim row As GridViewRow = datRespondent.Rows(0)
    '        Dim cb As CheckBox = datRespondent.Rows(i).FindControl("chkSelect")

    '        str = datRespondent.DataKeys(i).Value.ToString
    '        Dim strMykad As String = CType(datRespondent.Rows(i).FindControl("Mykad"), Label).Text

    '        strSQL = "SELECT Mykad FROM kpmkv_pensyarah Where Mykad='" & strMykad & "' AND IsDeleted='N'"
    '        If oCommon.isExist(strSQL) = False Then
    '            cb.Checked = True
    '        End If
    '    Next

    'End Sub
    'Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
    '    lblMsgResult.Text = ""
    '    lblMsg.Text = ""
    '    btnApprove.Enabled = True
    '    datRespondent.DataSource = Nothing
    '    datRespondent.DataBind()
    'End Sub

    'Private Sub datRespondent_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondent.PageIndexChanging
    '    datRespondent.PageIndex = e.NewPageIndex
    '    strRet = BindData(datRespondent)

    'End Sub

    'Private Sub datRespondent_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles datRespondent.SelectedIndexChanging
    '    Dim strKeyID As String = datRespondent.DataKeys(e.NewSelectedIndex).Value.ToString

    'End Sub

    Public Function FileIsLocked(ByVal strFullFileName As String) As Boolean
        Dim blnReturn As Boolean = False
        Dim fs As System.IO.FileStream

        Try
            fs = System.IO.File.Open(strFullFileName, IO.FileMode.OpenOrCreate, IO.FileAccess.Read, IO.FileShare.None)
            fs.Close()
        Catch ex As System.IO.IOException
            divMsg.Attributes("class") = "error"
            lblMsg.Text = "Error Message FileIsLocked:" & ex.Message
            blnReturn = True
        End Try

        Return blnReturn
    End Function

    Private Function SaveSiteData(ByVal SiteData As DataSet) As String
        lblMsg.Text = ""

        Try

            Dim sb As StringBuilder = New StringBuilder()
            For i As Integer = 0 To SiteData.Tables(0).Rows.Count - SiteData.Tables(0).Rows(i).Item("BIL")

                strNama = SiteData.Tables(0).Rows(i).Item("Nama")
                strJawatan = SiteData.Tables(0).Rows(i).Item("Jawatan")
                strGred = SiteData.Tables(0).Rows(i).Item("Gred")
                strMykad = SiteData.Tables(0).Rows(i).Item("Mykad")
                strEmail = SiteData.Tables(0).Rows(i).Item("Email")
                strJantina = SiteData.Tables(0).Rows(i).Item("Jantina")
                strAgama = SiteData.Tables(0).Rows(i).Item("Agama")
                strKaum = SiteData.Tables(0).Rows(i).Item("Kaum")
                strTel = SiteData.Tables(0).Rows(i).Item("Tel")


                strSQL = "INSERT INTO kpmkv_pensyarah (KolejRecordID,Nama,Jawatan,Gred,Mykad,Jantina,Agama,Kaum,Tel,Email,StatusID,IsDeleted,IsApproved)"
                strSQL += " VALUES ('" & lblKolejRecordID.Text & "','" & oCommon.FixSingleQuotes(strNama) & "','" & oCommon.FixSingleQuotes(strJawatan) & "','" & oCommon.FixSingleQuotes(strGred) & "','" & oCommon.FixSingleQuotes(strMykad) & "','" & oCommon.FixSingleQuotes(strJantina) & "','" & oCommon.FixSingleQuotes(strAgama) & "','" & oCommon.FixSingleQuotes(strKaum) & "','" & oCommon.FixSingleQuotes(strTel) & "','" & oCommon.FixSingleQuotes(strEmail) & "','2','N','Y')"
                strRet = oCommon.ExecuteSQL(strSQL)

                'Response.Write(strSQL)
                If strRet = "0" Then


                    '-1.get nama Kolej in tbl kolej-'
                    strSQL = "SELECT Nama FROM kpmkv_kolej WHERE RecordID='" & oCommon.FixSingleQuotes(lblKolejRecordID.Text) & "'"
                    Dim strNamaKolej As String = oCommon.getFieldValue(strSQL)
                    '-2.get negeri Kolej in tbl kolej-'
                    strSQL = "SELECT Negeri FROM kpmkv_kolej WHERE RecordID='" & oCommon.FixSingleQuotes(lblKolejRecordID.Text) & "'"
                    Dim strNegeriKolej As String = oCommon.getFieldValue(strSQL)

                    '-- create if user not exist in tbl user-'
                    strSQL = " INSERT INTO kpmkv_users(RecordID,LoginID,Pwd,UserType,Nama,Negeri)"
                    strSQL += " VALUES('" & oCommon.FixSingleQuotes(lblKolejRecordID.Text) & "','" & oCommon.FixSingleQuotes(strMykad) & "','pwd',"
                    strSQL += " 'PENSYARAH','" & oCommon.FixSingleQuotes(strNamaKolej) & "',"
                    strSQL += " '" & oCommon.FixSingleQuotes(strNegeriKolej) & "')"
                    strRet = oCommon.ExecuteSQL(strSQL)

                    divMsg.Attributes("class") = "info"
                    lblMsg.Text = "Pensyarah berjaya didaftarkan"
                Else

                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Pensyarah tidak berjaya didaftarkan"
                    Exit For
                End If

            Next

        Catch ex As Exception
            divMsg.Attributes("class") = "error"

        End Try
        Return True
    End Function

    'Private Sub btnApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApprove.Click
    '    lblMsgResult.Text = ""
    '    lblMsg.Text = ""

    '    Dim str As String
    '    Try

    '        For i As Integer = 0 To datRespondent.Rows.Count - 1
    '            Dim row As GridViewRow = datRespondent.Rows(i)
    '            Dim isChecked As Boolean = DirectCast(row.FindControl("chkSelect"), CheckBox).Checked
    '            str = datRespondent.DataKeys(i).Value.ToString

    '            strNama = CType(datRespondent.Rows(i).FindControl("Nama"), Label).Text
    '            strJawatan = CType(datRespondent.Rows(i).FindControl("Jawatan"), Label).Text
    '            strGred = CType(datRespondent.Rows(i).FindControl("Gred"), Label).Text
    '            strMykad = CType(datRespondent.Rows(i).FindControl("Mykad"), Label).Text
    '            strEmail = CType(datRespondent.Rows(i).FindControl("Email"), Label).Text
    '            strJantina = CType(datRespondent.Rows(i).FindControl("Jantina"), Label).Text
    '            strAgama = CType(datRespondent.Rows(i).FindControl("Agama"), Label).Text
    '            strKaum = CType(datRespondent.Rows(i).FindControl("Kaum"), Label).Text
    '            strTel = CType(datRespondent.Rows(i).FindControl("Telefon"), Label).Text

    '            If isChecked Then
    '                strSQL = "INSERT INTO kpmkv_pensyarah (KolejRecordID,Nama,Jawatan,Gred,Mykad,Jantina,Agama,Kaum,Tel,Email,StatusID,IsDeleted,IsApproved)"
    '                strSQL += " VALUES ('" & lblKolejRecordID.Text & "','" & strNama & "','" & strJawatan & "','" & strGred & "','" & strMykad & "','" & strJantina & "','" & strAgama & "','" & strKaum & "','" & strTel & "','" & strEmail & "','2','N','Y')"
    '                strRet = oCommon.ExecuteSQL(strSQL)
    '                If strRet = "0" Then
    '                Else
    '                    divMsgResult.Attributes("class") = "error"
    '                    lblMsgResult.Text = "Tidak Berjaya!. Pendaftaran Pensyarah baru."
    '                    divMsg.Attributes("class") = "error"
    '                    lblMsg.Text = "Tidak Berjaya!. Pendaftaran Pensyarah baru."
    '                    Exit For
    '                End If
    '            End If
    '        Next

    '    Catch ex As Exception
    '        divMsg.Attributes("class") = "error"

    '    End Try

    '    '--ALL OK. refresh screen

    '    divMsgResult.Attributes("class") = "info"
    '    lblMsgResult.Text = "Berjaya!. Pendaftaran Pensyarah baru."
    '    divMsg.Attributes("class") = "info"
    '    lblMsg.Text = "Berjaya!. Pendaftaran Pensyarah baru."
    '    btnApprove.Enabled = False
    '    btnApprove.ForeColor = Drawing.Color.DimGray
    'End Sub

End Class