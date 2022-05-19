Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Data
Imports System.Text
Imports System.Windows

Public Class pentaksir_bmsetara_daftar1
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Not IsPostBack Then
                lblMsg.Text = ""
                lblMsg2.Text = ""

                kpmkv_tahun_list()

                strRet = BindData(datRespondent)

            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try
    End Sub
    Private Sub kpmkv_tahun_list()
        strSQL = "SELECT value FROM kpmkv_pentaksir_bmsetara_setting WHERE type = 'Tahun'"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlTahun.DataSource = ds
            ddlTahun.DataTextField = "value"
            ddlTahun.DataValueField = "value"
            ddlTahun.DataBind()

        Catch ex As Exception

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub btnDaftar_Click(sender As Object, e As EventArgs) Handles btnDaftar.Click

        strSQL = "SELECT UserID FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
        Dim UserID As String = oCommon.getFieldValue(strSQL)

        strSQL = "SELECT RecordID FROM kpmkv_users WHERE UserID = '" & UserID & "'"
        Dim RecordID As String = oCommon.getFieldValue(strSQL)

        ''PASSWORD CREATION
        Dim _allowedChars As String = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789$&!"
        Dim randomNumber As New Random()
        Dim chars(10 - 1) As Char
        Dim allowedCharCount As Integer = _allowedChars.Length
        For i As Integer = 0 To 10 - 1
            chars(i) = _allowedChars.Chars(CInt(Fix((_allowedChars.Length) * randomNumber.NextDouble())))
        Next i
        ''PASSWORD CREATION

        strSQL = "SELECT UserID FROM kpmkv_users WHERE LoginID = '" & txtMYKAD.Text & "' AND UserType = 'PENTAKSIR-BMSETARA'"
        Dim PentaksirUserID As String = oCommon.getFieldValue(strSQL)

        If PentaksirUserID = "" Then

            strSQL = "  INSERT INTO kpmkv_users
                       (LoginID, Pwd, UserType, Nama, Mykad, StatusID)
                        VALUES
                       ('" & txtEmail.Text & "','" & chars & "','PENTAKSIR-BMSETARA',UPPER('" & txtNama.Text & "'),'" & txtMYKAD.Text & "','2')"
            strRet = oCommon.ExecuteSQL(strSQL)

            strSQL = "SELECT UserID FROM kpmkv_users WHERE LoginID = '" & txtEmail.Text & "' AND Pwd = '" & chars & "' AND UserType = 'PENTAKSIR-BMSETARA'"
            PentaksirUserID = oCommon.getFieldValue(strSQL)

            strSQL = "  SELECT id FROM kpmkv_pentaksir_bmsetara WHERE 
                        KolejRecordID = '" & RecordID & "' AND Kohort = '" & ddlTahun.SelectedValue & "' AND Semester = '4'
                        AND MYKAD = '" & txtMYKAD.Text & "' AND Email = '" & txtEmail.Text & "'"
            Dim id As String = oCommon.getFieldValue(strSQL)

            If id = "" Then

                strSQL = "  INSERT INTO kpmkv_pentaksir_bmsetara
                           (UserID, KolejRecordID, MataPelajaran, Kohort,Semester,Nama,MYKAD,Email,create_timestamp,create_by)
                            VALUES
                           ('" & PentaksirUserID & "','" & RecordID & "', 'BAHASA MELAYU 3', '" & ddlTahun.SelectedValue & "','4',UPPER('" & txtNama.Text & "'),'" & txtMYKAD.Text & "','" & txtEmail.Text & "',CURRENT_TIMESTAMP,'" & UserID & "')"
                strRet = oCommon.ExecuteSQL(strSQL)

                strSQL = "  INSERT INTO kpmkv_pentaksir_bmsetara
                           (UserID, KolejRecordID, MataPelajaran, Kohort,Semester,Nama,MYKAD,Email,create_timestamp,create_by)
                            VALUES
                           ('" & PentaksirUserID & "','" & RecordID & "', 'BAHASA MELAYU 4', '" & ddlTahun.SelectedValue & "','4',UPPER('" & txtNama.Text & "'),'" & txtMYKAD.Text & "','" & txtEmail.Text & "',CURRENT_TIMESTAMP,'" & UserID & "')"
                strRet = oCommon.ExecuteSQL(strSQL)

                If strRet = "0" Then

                    divMsg2.Attributes("class") = "info"
                    lblMsg2.Text = "Pendaftaran Pentaksir BM Setara Berjaya!"
                    EmailRelay()

                Else

                    divMsg2.Attributes("class") = "error"
                    lblMsg2.Text = "Pendaftaran Pentaksir BM Setara Tidak Berjaya!"

                End If

            Else

                divMsg2.Attributes("class") = "error"
                lblMsg2.Text = "Pentaksir BM Setara telah didaftarkan di dalam sistem sebelum ini."

            End If

        Else

            strSQL = "  SELECT id FROM kpmkv_pentaksir_bmsetara WHERE 
                        KolejRecordID = '" & RecordID & "' AND Kohort = '" & ddlTahun.SelectedValue & "' AND Semester = '4'
                        AND MYKAD = '" & txtMYKAD.Text & "' AND Email = '" & txtEmail.Text & "'"
            Dim id As String = oCommon.getFieldValue(strSQL)

            If id = "" Then

                strSQL = "  INSERT INTO kpmkv_pentaksir_bmsetara
                           (UserID, KolejRecordID, MataPelajaran, Kohort,Semester,Nama,MYKAD,Email,create_timestamp,create_by)
                            VALUES
                           ('" & PentaksirUserID & "','" & RecordID & "', 'BAHASA MELAYU 3', '" & ddlTahun.SelectedValue & "','4',UPPER('" & txtNama.Text & "'),'" & txtMYKAD.Text & "','" & txtEmail.Text & "',CURRENT_TIMESTAMP,'" & UserID & "')"
                strRet = oCommon.ExecuteSQL(strSQL)

                strSQL = "  INSERT INTO kpmkv_pentaksir_bmsetara
                           (UserID, KolejRecordID, MataPelajaran, Kohort,Semester,Nama,MYKAD,Email,create_timestamp,create_by)
                            VALUES
                           ('" & PentaksirUserID & "','" & RecordID & "', 'BAHASA MELAYU 4', '" & ddlTahun.SelectedValue & "','4',UPPER('" & txtNama.Text & "'),'" & txtMYKAD.Text & "','" & txtEmail.Text & "',CURRENT_TIMESTAMP,'" & UserID & "')"
                strRet = oCommon.ExecuteSQL(strSQL)

                If strRet = "0" Then

                    divMsg2.Attributes("class") = "info"
                    lblMsg2.Text = "Pendaftaran Pentaksir BM Setara Berjaya!"
                    EmailRelay()


                Else

                    divMsg2.Attributes("class") = "error"
                    lblMsg2.Text = "Pendaftaran Pentaksir BM Setara Tidak Berjaya!"

                End If

            Else

                divMsg2.Attributes("class") = "error"
                lblMsg2.Text = "Pentaksir BM Setara telah didaftarkan di dalam sistem sebelum ini."

            End If

        End If

        strRet = BindData(datRespondent)


    End Sub

    Private Sub EmailRelay()

        Try
            Dim Smtp_Server As New SmtpClient("10.46.50.39")
            Dim e_mail As New MailMessage()
            Smtp_Server.UseDefaultCredentials = True
            Smtp_Server.Port = 25
            Smtp_Server.EnableSsl = True
            Smtp_Server.Host = "postmaster.1govuc.gov.my"
            'Smtp_Server.Host = "smtp.gmail.com"

            e_mail = New MailMessage()
            e_mail.From = New MailAddress("peperiksaan@moe.gov.my")
            e_mail.To.Add(txtEmail.Text)
            e_mail.Subject = "Pendaftaran Pentaksir bagi Sistem APKV"
            e_mail.IsBodyHtml = True

            Dim sb As New StringBuilder
            sb.AppendLine("<p>Pendaftaran akaun anda telah berjaya!</p>")
            sb.AppendLine("<p></p>")
            sb.AppendLine("<p>Berikut adalah senarai pusat peperiksaan dan mata pelajaran / kertas yang telah dipohon untuk diberi penilaian:</p>")
            sb.AppendLine("<p></p>")
            sb.AppendLine("<p></p>")

            Dim dt As DataTable = New DataTable
            dt.Columns.AddRange(New DataColumn() {New DataColumn("Mata Pelajaran / Kertas", GetType(System.String)), New DataColumn("Peranan", GetType(System.String)), New DataColumn("Pusat Peperiksaan", GetType(System.String))})

            strSQL = "  SELECT kpmkv_pentaksir_bmsetara.MataPelajaran, kpmkv_kolej.Kod, kpmkv_kolej.Nama
                        FROM kpmkv_pentaksir_bmsetara
                        JOIN kpmkv_kolej ON kpmkv_kolej.RecordID = kpmkv_pentaksir_bmsetara.KolejRecordID
                        WHERE kpmkv_pentaksir_bmsetara.MYKAD = '" & txtMYKAD.Text & "'"

            strSQL = strSQL
            Dim dtb As New DataTable
            Using cnn As New SqlConnection(strConn)
                cnn.Open()
                Using dad As New SqlDataAdapter(strSQL, cnn)
                    dad.Fill(dtb)
                End Using
                cnn.Close()
            End Using

            For i As Integer = 0 To dtb.Rows.Count - 1

                dt.Rows.Add(dtb.Rows(i).Item(0).ToString, "Pentaksir", dtb.Rows(i).Item(1).ToString + " - " + dtb.Rows(i).Item(2).ToString)

            Next

            'Table start.
            sb.Append("<table cellpadding='5' cellspacing='0' style='border: 1px solid;font-size: 9pt;font-family:Arial'>")
            'Adding HeaderRow.
            sb.Append("<tr>")
            For Each column As DataColumn In dt.Columns
                sb.Append(("<th style='border: 1px solid'>" + (column.ColumnName + "</th>")))
            Next
            sb.Append("</tr>")
            'Adding DataRow.
            For Each row As DataRow In dt.Rows
                sb.Append("<tr>")
                For Each column As DataColumn In dt.Columns
                    sb.Append(("<td style='border: 1px solid'>" + (row(column.ColumnName).ToString + "</td>")))
                Next
                sb.Append("</tr>")
            Next
            'Table end.
            sb.Append("</table>")

            sb.AppendLine("<p></p>")
            sb.AppendLine("<p></p>")

            sb.AppendLine("<p>Berikut adalah Login ID dan Kata Laluan akaun anda untuk log masuk ke Pentaksiran Bahasa Melayu Setara APKV (sila klik di <a href='http://apkv.moe.gov.my/apkv_v2_admin/pentaksirbm_default.aspx'>sini</a>).</p>")
            sb.AppendLine("<p></p>")
            sb.AppendLine("<p>Login ID: <b>" & txtEmail.Text & "</b></p>")

            strSQL = "SELECT Pwd FROM kpmkv_users WHERE LoginID = '" & txtEmail.Text & "' AND Mykad = '" & txtMYKAD.Text & "'"
            Dim Pwd As String = oCommon.getFieldValue(strSQL)

            sb.AppendLine("<p>Kata Laluan: <b>" & Pwd & "</b></p>")
            sb.AppendLine("<p></p>")
            sb.AppendLine("<p></p>")
            sb.AppendLine("<b>Nota:</b>")
            sb.AppendLine("<p>1. Kata Laluan yang dijana oleh sistem akan digunakan untuk log masuk sepanjang masa anda memasukkan markah penilaian untuk semua Mata Pelajaran / Kertas <br>bagi semua pusat peperiksaan yang diagihkan kepada anda. Sila simpan Kata Laluan atau e-mel ini.</p>")
            sb.AppendLine("<p></p>")
            sb.AppendLine("<p>Sekian, Terima Kasih.</p>")
            sb.AppendLine("<p></p>")
            sb.AppendLine("<b>Email ini adalah janaan komputer dan tidak perlu dibalas.</b>")


            e_mail.Body = sb.ToString()
            Smtp_Server.Send(e_mail)

        Catch error_t As Exception
            lblMsg.Text = error_t.ToString
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

        strSQL = "SELECT RecordID FROM kpmkv_users WHERE LoginID = '" & Session("LoginID") & "' AND Pwd = '" & Session("Password") & "'"
        Dim RecordID As String = oCommon.getFieldValue(strSQL)

        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY kpmkv_users.UserID ASC"

        tmpSQL = "  SELECT DISTINCT kpmkv_users.UserID, kpmkv_users.Nama, kpmkv_users.Mykad, kpmkv_users.LoginID
                    FROM kpmkv_users
                    LEFT JOIN kpmkv_pentaksir_bmsetara ON kpmkv_pentaksir_bmsetara.UserID = kpmkv_users.UserID
                    WHERE
                    kpmkv_users.UserType = 'PENTAKSIR-BMSETARA'
                    AND kpmkv_pentaksir_bmsetara.KolejRecordID = '" & RecordID & "'"

        getSQL = tmpSQL
        ''--debug
        'Response.Write(getSQL)

        Return getSQL

    End Function

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

    Private Sub datRespondent_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles datRespondent.SelectedIndexChanging
        Dim strKey As String = datRespondent.DataKeys(e.NewSelectedIndex).Value.ToString

        strSQL = "  DELETE FROM kpmkv_users
                    WHERE UserID = '" & strKey & "'"

        strRet = oCommon.ExecuteSQL(strSQL)

            strSQL = "  DELETE FROM kpmkv_pentaksir_bmsetara
                    WHERE UserID = '" & strKey & "'"

            strRet = oCommon.ExecuteSQL(strSQL)

            If strRet = "0" Then

                divMsg.Attributes("class") = "info"
                lblMsg.Text = "Pentaksir telah berjaya dipadam!"

                divMsg2.Attributes("class") = "info"
                lblMsg2.Text = "Pentaksir telah berjaya dipadam!"

            Else

                divMsg.Attributes("class") = "error"
                lblMsg.Text = "Pentaksir tidak berjaya dipadam!"

                divMsg2.Attributes("class") = "error"
                lblMsg2.Text = "Pentaksir tidak berjaya dipadam!"

            End If

        strRet = BindData(datRespondent)

    End Sub

End Class