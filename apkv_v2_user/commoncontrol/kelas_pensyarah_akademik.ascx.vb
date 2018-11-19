Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class kelas_pensyarah_akademik1
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

                            kpmkv_namakluster_list()
                            ddlNamaKluster.Text = "0"

                            kpmkv_kodkursus_list()
                            ddlKodKursus.Text = "0"

                            kpmkv_kelas_list()
                            ddlNamaKelas.Text = "0"

                            kpmkv_semester_list()

                            lblmsg.Text = ""
                            strRet = BindData(datRespondent)
                      
            End If


        Catch ex As Exception
            lblmsg.Text = "System Error:" & ex.Message
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

            '--ALL
            'ddlTahun.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblmsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try


    End Sub
    Private Sub kpmkv_semester_list()
        strSQL = "SELECT Semester FROM kpmkv_semester ORDER BY SemesterID"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlSemester.DataSource = ds
            ddlSemester.DataTextField = "Semester"
            ddlSemester.DataValueField = "Semester"
            ddlSemester.DataBind()

        Catch ex As Exception
            lblmsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_namakluster_list()
        strSQL = "SELECT kpmkv_kluster.NamaKluster,kpmkv_kluster.KlusterID FROM kpmkv_kursus_kolej LEFT OUTER JOIN kpmkv_kursus "
        strSQL += " ON kpmkv_kursus_kolej.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN kpmkv_kluster "
        strSQL += " ON kpmkv_kursus.KlusterID = kpmkv_kluster.KlusterID"
        strSQL += " WHERE kpmkv_kursus.Tahun='" & ddlTahun.SelectedValue & "' AND  kpmkv_kursus.Sesi='" & chkSesi.SelectedValue & "' AND kpmkv_kursus_kolej.KolejRecordID='" & lblKolejID.Text & "' GROUP BY kpmkv_kluster.NamaKluster,kpmkv_kluster.KlusterID"
        ' Response.Write(strSQL)
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
            ddlNamaKluster.Items.Add(New ListItem("-Pilih-", "0"))

        Catch ex As Exception
            lblmsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Sub kpmkv_kodkursus_list()
        strSQL = "SELECT kpmkv_kursus.KodKursus,kpmkv_kursus.KursusID FROM kpmkv_kursus_kolej LEFT OUTER JOIN kpmkv_kursus "
        strSQL += " ON kpmkv_kursus_kolej.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN kpmkv_kluster "
        strSQL += " ON kpmkv_kursus.KlusterID = kpmkv_kluster.KlusterID"
        strSQL += " WHERE kpmkv_kursus_kolej.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kursus.Tahun='" & ddlTahun.SelectedValue & "' AND kpmkv_kursus.Sesi='" & chkSesi.SelectedValue & "'  AND kpmkv_kluster.KlusterID='" & ddlNamaKluster.SelectedValue & "' GROUP BY kpmkv_kursus.KodKursus, kpmkv_kursus.KursusID"

        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlKodKursus.DataSource = ds
            ddlKodKursus.DataTextField = "KodKursus"
            ddlKodKursus.DataValueField = "KursusID"
            ddlKodKursus.DataBind()

            '--ALL
            ddlKodKursus.Items.Add(New ListItem("-Pilih-", "0"))

        Catch ex As Exception
            lblmsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_kelas_list()
        strSQL = " SELECT kpmkv_kelas.NamaKelas, kpmkv_kelas.KelasID FROM kpmkv_kelas"
        strSQL += " INNER JOIN kpmkv_kelas_kursus ON kpmkv_kelas.KelasID=kpmkv_kelas_kursus.KelasID"
        strSQL += " WHERE kpmkv_kelas.KolejRecordID='" & lblKolejID.Text & "'  AND kpmkv_kelas.Tahun= '" & ddlTahun.SelectedValue & "' AND kpmkv_kelas.IsDeleted='N' AND kpmkv_kelas_kursus.KursusID='" & ddlKodKursus.SelectedValue & "'"
        'Response.Write(strSQL)
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlNamaKelas.DataSource = ds
            ddlNamaKelas.DataTextField = "NamaKelas"
            ddlNamaKelas.DataValueField = "KelasID"
            ddlNamaKelas.DataBind()

            '--ALL
            ddlNamaKelas.Items.Add(New ListItem("-Pilih-", "0"))

        Catch ex As Exception
            lblmsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try


    End Sub
    '-------------senarai matapelajaran------------'
    Private Function BindData(ByVal gvTable As GridView) As Boolean
        Dim myDataSet As New DataSet
        Dim myDataAdapter As New SqlDataAdapter(getSQL, strConn)
        myDataAdapter.SelectCommand.CommandTimeout = 120

        Try
            myDataAdapter.Fill(myDataSet, "myaccount")

            If myDataSet.Tables(0).Rows.Count = 0 Then

            End If
            gvTable.DataSource = myDataSet
            gvTable.DataBind()
            objConn.Close()
        Catch ex As Exception
            lblmsg.Text = "System Error:" & ex.Message
            Return False
        End Try

        Return True

    End Function
    Private Function getSQL() As String
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY Tahun"

        tmpSQL = " SELECT * FROM  kpmkv_matapelajaran "
        strWhere = "WHERE SUBSTRING(KodMataPelajaran,4,1) ='" & ddlSemester.SelectedValue & "' AND Tahun='" & ddlTahun.SelectedValue & "' AND IsDeleted='N'"

        'tahun
        If Not ddlTahun.Text = "" Then
            strWhere += " AND Tahun='" & ddlTahun.Text & "'"
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

    Protected Sub btnCari_Click(sender As Object, e As EventArgs) Handles btnCari.Click
        lblmsg.Text = ""
        'txtNama.Text = ""
        strRet = BindData(datRespondent)
        '-pensyarah-'
        strRet = BindData2(datRespondent2)
        '-penetapan pensyarah-'
        'strRet = BindData3(datRespondent3)

    End Sub
    '-------------------senarai pensyarah-----------'
    Private Function BindData2(ByVal gvTable As GridView) As Boolean
        Dim myDataSet As New DataSet
        Dim myDataAdapter As New SqlDataAdapter(getSQL2, strConn)
        myDataAdapter.SelectCommand.CommandTimeout = 120

        Try
            myDataAdapter.Fill(myDataSet, "myaccount")

            If myDataSet.Tables(0).Rows.Count = 0 Then

            End If

            gvTable.DataSource = myDataSet
            gvTable.DataBind()
            objConn.Close()
        Catch ex As Exception
            lblmsg.Text = "System Error:" & ex.Message
            Return False
        End Try

        Return True

    End Function

    Private Function getSQL2() As String
        Dim tmpSQL As String
        Dim strWhere As String = ""
        Dim strOrder As String = " ORDER BY Nama ASC"

        tmpSQL = "SELECT * FROM kpmkv_pensyarah"
        strWhere = "  WHERE KolejRecordId='" & lblKolejID.Text & "' AND IsDeleted='N'"

        getSQL2 = tmpSQL & strWhere & strOrder
        ''--debug
        'Response.Write(getSQL2)

        Return getSQL2

    End Function
    Private Sub datRespondent2_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles datRespondent2.PageIndexChanging
        datRespondent2.PageIndex = e.NewPageIndex
        strRet = BindData2(datRespondent2)

    End Sub
    Private Function GetData2(ByVal cmd As SqlCommand) As DataTable
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

    Protected Sub datRespondent2_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles datRespondent2.RowCommand
        If (e.CommandName = "Pilih") Then

            Dim PensyarahID = Int32.Parse(e.CommandArgument.ToString())

            Dim str As String
            For i As Integer = 0 To datRespondent.Rows.Count - 1
                Dim row As GridViewRow = datRespondent.Rows(i)
                Dim isChecked As Boolean = DirectCast(row.FindControl("chkSelect"), CheckBox).Checked

                str = datRespondent.DataKeys(i).Value.ToString
                If isChecked Then
                    strSQL = "SELECT * FROM kpmkv_pensyarah_matapelajaran WHERE KelasID='" & ddlNamaKelas.SelectedValue & "'  And MataPelajaranID='" & str & "' "
                    strRet = oCommon.isExist(strSQL)
                    If strRet = True Then

                        strSQL = "DELETE FROM kpmkv_pensyarah_matapelajaran WHERE KelasID='" & ddlNamaKelas.SelectedValue & "' And MataPelajaranID='" & str & "' "
                        strRet = oCommon.getFieldValue(strSQL)
                    End If

                    strSQL = "INSERT INTO kpmkv_pensyarah_matapelajaran(Tahun,Semester,Sesi,PensyarahID, KelasID, MataPelajaranID, IsDeleted)"
                    strSQL += " VALUES('" & ddlTahun.SelectedValue & "','" & ddlSemester.SelectedValue & "','" & chkSesi.Text & "','" & PensyarahID & "', '" & ddlNamaKelas.SelectedValue & "', '" & str & "', 'N')"
                    strRet = oCommon.ExecuteSQL(strSQL)

                    If strRet = "0" Then

                        divMsg.Attributes("class") = "info"
                        lblmsg.Text = "Penetapan pensyarah berjaya didaftarkan"

                         



                        Else
                            divMsg.Attributes("class") = "error"
                            lblmsg.Text = "Penetapan pensyarah tidak berjaya didaftarkan"
                        End If
                    End If
            Next

        End If
    End Sub

  
    Protected Sub chkSesi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkSesi.SelectedIndexChanged
        kpmkv_namakluster_list()
        ddlNamaKluster.Text = "0"

        kpmkv_kodkursus_list()
        ddlKodKursus.Text = "0"

        kpmkv_kelas_list()
        ddlNamaKelas.Text = "0"
    End Sub

    Protected Sub ddlNamaKluster_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlNamaKluster.SelectedIndexChanged
        kpmkv_kodkursus_list()
        ddlKodKursus.Text = "0"

        kpmkv_kelas_list()
        ddlNamaKelas.Text = "0"
    End Sub

    Private Sub ddlKodKursus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlKodKursus.SelectedIndexChanged
        kpmkv_kelas_list()
        ddlNamaKelas.Text = "0"

    End Sub
End Class