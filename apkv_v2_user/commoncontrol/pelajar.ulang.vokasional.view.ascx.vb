Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization
Public Class pelajar_ulang_vokasional_view
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
                kpmkv_semester_list()

                kpmkv_kodkursus_list()

                kpmkv_kelas_list()

                lblMsg.Text = ""
                LoadPage()
                Modul_Nama()
                LoadPageAkademikGred()
                HiddenPBPACheckBox()
                Hidden()
            End If

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try

    End Sub
    Private Sub kpmkv_tahun_list()
        strSQL = "SELECT Tahun FROM kpmkv_tahun ORDER BY Tahun"
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
            ddlTahun.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_semester_list()
        strSQL = "SELECT Semester FROM kpmkv_semester ORDER BY Semester"
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
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_kodkursus_list()

        strSQL = "SELECT kpmkv_kursus.KodKursus, kpmkv_kursus.KursusID"
        strSQL += " FROM kpmkv_kelas_kursus INNER JOIN kpmkv_kursus ON kpmkv_kelas_kursus.KursusID = kpmkv_kursus.KursusID INNER JOIN"
        strSQL += " kpmkv_kelas ON kpmkv_kelas_kursus.KelasID = kpmkv_kelas.KelasID"
        strSQL += " WHERE kpmkv_kelas.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kursus.Tahun='" & ddlTahun.Text & "'"
        strSQL += " AND kpmkv_kursus.Sesi='" & chkSesi.SelectedValue & "' GROUP BY kpmkv_kursus.KodKursus,kpmkv_kursus.KursusID"
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
            ' ddlKodKursus.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_kelas_list()
        strSQL = " SELECT kpmkv_kelas.NamaKelas, kpmkv_kelas.KelasID"
        strSQL += " FROM  kpmkv_kelas_kursus LEFT OUTER JOIN kpmkv_kelas ON kpmkv_kelas_kursus.KelasID = kpmkv_kelas.KelasID LEFT OUTER JOIN"
        strSQL += " kpmkv_kursus ON kpmkv_kelas_kursus.KursusID = kpmkv_kursus.KursusID"
        strSQL += " WHERE kpmkv_kelas.KolejRecordID='" & lblKolejID.Text & "' AND kpmkv_kelas_kursus.KursusID= '" & ddlKodKursus.SelectedValue & "' ORDER BY KelasID"
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
            'ddlNamaKelas.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub Modul_Nama()

        '100
        strSQL = "SELECT NamaModul AS Modul FROM kpmkv_modul WHERE KursusID='" & lblKursusID.Text & "' AND KodModul Like '%01%' and tahun='" & lblTahun.Text & "'and Semester='" & lblSemester.Text & "' and Sesi='" & lblSesi.Text & "' "
        strRet = oCommon.getFieldValue(strSQL)
        lblModul1.Text = strRet

        '200
        strSQL = " SELECT NamaModul AS Modul FROM kpmkv_modul WHERE KursusID='" & lblKursusID.Text & "' AND KodModul Like '%02%' and tahun='" & lblTahun.Text & "'and Semester='" & lblSemester.Text & "' and Sesi='" & lblSesi.Text & "' "
        strRet = oCommon.getFieldValue(strSQL)
        lblModul2.Text = strRet

        '300
        strSQL = " SELECT NamaModul AS Modul FROM kpmkv_modul WHERE KursusID='" & lblKursusID.Text & "' AND KodModul Like '%03%' and tahun='" & lblTahun.Text & "'and Semester='" & lblSemester.Text & "' and Sesi='" & lblSesi.Text & "' "
        strRet = oCommon.getFieldValue(strSQL)
        lblModul3.Text = strRet

        '400
        strSQL = " SELECT NamaModul AS Modul FROM kpmkv_modul WHERE KursusID='" & lblKursusID.Text & "' AND KodModul Like '%04%' and tahun='" & lblTahun.Text & "'and Semester='" & lblSemester.Text & "' and Sesi='" & lblSesi.Text & "' "
        strRet = oCommon.getFieldValue(strSQL)
        lblModul4.Text = strRet

        '500
        strSQL = " SELECT NamaModul AS Modul FROM kpmkv_modul WHERE KursusID='" & lblKursusID.Text & "' AND KodModul Like '%05%' and tahun='" & lblTahun.Text & "'and Semester='" & lblSemester.Text & "' and Sesi='" & lblSesi.Text & "' "
        strRet = oCommon.getFieldValue(strSQL)
        lblModul5.Text = strRet

        strSQL = " SELECT NamaModul AS Modul FROM kpmkv_modul WHERE KursusID='" & lblKursusID.Text & "' AND KodModul Like '%06%' and tahun='" & lblTahun.Text & "'and Semester='" & lblSemester.Text & "' and Sesi='" & lblSesi.Text & "' "
        strRet = oCommon.getFieldValue(strSQL)
        lblModul6.Text = strRet

    End Sub
    Private Sub LoadPageAkademikGred()

        strSQL = "SELECT  kpmkv_pelajar_markah.GredV1, kpmkv_pelajar_markah.GredV2, kpmkv_pelajar_markah.GredV3, kpmkv_pelajar_markah.GredV4, "
        strSQL += " kpmkv_pelajar_markah.GredV5, kpmkv_pelajar_markah.GredV6, kpmkv_pelajar_markah.GredV7, kpmkv_pelajar_markah.GredV8"
        strSQL += " FROM kpmkv_pelajar_markah"
        strSQL += " WHERE kpmkv_pelajar_markah.PelajarID='" & Request.QueryString("PelajarID") & "'"
        strSQL += " AND kpmkv_pelajar_markah.Tahun='" & lblTahun.Text & "'"
        strSQL += " AND kpmkv_pelajar_markah.Semester='" & lblSemester.Text & "'"
        strSQL += " AND kpmkv_pelajar_markah.Sesi='" & lblSesi.Text & "'"

        ''--debug
        ''Response.Write(getSQL)
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

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("GredV1")) Then
                    lblGred1.Text = ds.Tables(0).Rows(0).Item("GredV1")
                Else
                    lblGred1.Text = ""
                End If

                '--Account Details 
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("GredV2")) Then
                    lblGred2.Text = ds.Tables(0).Rows(0).Item("GredV2")
                Else
                    lblGred2.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("GredV3")) Then
                    lblGred3.Text = ds.Tables(0).Rows(0).Item("GredV3")
                Else
                    lblGred3.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("GredV4")) Then
                    lblGred4.Text = ds.Tables(0).Rows(0).Item("GredV4")
                Else
                    lblGred4.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("GredV5")) Then
                    lblGred5.Text = ds.Tables(0).Rows(0).Item("GredV5")
                Else
                    lblGred5.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("GredV6")) Then
                    lblGred6.Text = ds.Tables(0).Rows(0).Item("GredV6")
                Else
                    lblGred6.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("GredV7")) Then
                    lblGred7.Text = ds.Tables(0).Rows(0).Item("GredV7")
                Else
                    lblGred7.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("GredV8")) Then
                    lblGred8.Text = ds.Tables(0).Rows(0).Item("GredV8")
                Else
                    lblGred8.Text = ""
                End If
                '--
            End If
        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try


    End Sub
    Private Sub HiddenPBPACheckBox()
        '('D','E','D-','D+','T')

        If lblGred1.Text = "D" Then
            PB1.Enabled = True
            PA1.Enabled = True
            PB9.Enabled = True
            PA9.Enabled = True
        End If

        If lblGred1.Text = "E" Then
            PB1.Enabled = True
            PA1.Enabled = True
            PB9.Enabled = True
            PA9.Enabled = True
        End If

        If lblGred1.Text = "D-" Then
            PB1.Enabled = True
            PA1.Enabled = True
            PB9.Enabled = True
            PA9.Enabled = True
        End If

        If lblGred1.Text = "D+" Then
            PB1.Enabled = True
            PA1.Enabled = True
            PB9.Enabled = True
            PA9.Enabled = True
        End If

        If lblGred1.Text = "T" Then
            PB1.Enabled = True
            PA1.Enabled = True
            PB9.Enabled = True
            PA9.Enabled = True
        End If
        '-----------------------
        If lblGred2.Text = "D" Then
            PB2.Enabled = True
            PA2.Enabled = True
            PB10.Enabled = True
            PA10.Enabled = True
        End If

        If lblGred2.Text = "E" Then
            PB2.Enabled = True
            PA2.Enabled = True
            PB10.Enabled = True
            PA10.Enabled = True
        End If

        If lblGred2.Text = "D-" Then
            PB2.Enabled = True
            PA2.Enabled = True
            PB10.Enabled = True
            PA10.Enabled = True
        End If

        If lblGred2.Text = "D+" Then
            PB2.Enabled = True
            PA2.Enabled = True
            PB10.Enabled = True
            PA10.Enabled = True
        End If

        If lblGred2.Text = "T" Then
            PB2.Enabled = True
            PA2.Enabled = True
            PB10.Enabled = True
            PA10.Enabled = True
        End If
        '-------------

        If lblGred3.Text = "D" Then
            PB3.Enabled = True
            PA3.Enabled = True
            PB11.Enabled = True
        End If

        If lblGred3.Text = "E" Then
            PB3.Enabled = True
            PA3.Enabled = True
            PB11.Enabled = True
        End If

        If lblGred3.Text = "D-" Then
            PB3.Enabled = True
            PA3.Enabled = True
            PB11.Enabled = True
        End If

        If lblGred3.Text = "D+" Then
            PB3.Enabled = True
            PA3.Enabled = True
            PB11.Enabled = True
        End If
        If lblGred3.Text = "T" Then
            PB3.Enabled = True
            PA3.Enabled = True
            PB11.Enabled = True
        End If
        '----------------------------
        If lblGred4.Text = "D" Then
            PB4.Enabled = True
            PA4.Enabled = True
            PB12.Enabled = True
            PA12.Enabled = True
        End If

        If lblGred4.Text = "E" Then
            PB4.Enabled = True
            PA4.Enabled = True
            PB12.Enabled = True
            PA12.Enabled = True
        End If

        If lblGred4.Text = "D-" Then
            PB4.Enabled = True
            PA4.Enabled = True
            PB12.Enabled = True
            PA12.Enabled = True
        End If

        If lblGred4.Text = "D+" Then
            PB4.Enabled = True
            PA4.Enabled = True
            PB12.Enabled = True
            PA12.Enabled = True
        End If

        If lblGred4.Text = "T" Then
            PB4.Enabled = True
            PA4.Enabled = True
            PB12.Enabled = True
            PA12.Enabled = True
        End If


        If lblGred5.Text = "D" Then
            PB5.Enabled = True
            PA5.Enabled = True
            PB13.Enabled = True
            PA13.Enabled = True
        End If

        If lblGred5.Text = "E" Then
            PB5.Enabled = True
            PA5.Enabled = True
            PB13.Enabled = True
            PA13.Enabled = True
        End If

        If lblGred5.Text = "D-" Then
            PB5.Enabled = True
            PA5.Enabled = True
            PB13.Enabled = True
            PA13.Enabled = True
        End If

        If lblGred5.Text = "D+" Then
            PB5.Enabled = True
            PA5.Enabled = True
            PB13.Enabled = True
            PA13.Enabled = True
        End If

        If lblGred5.Text = "T" Then
            PB5.Enabled = True
            PA5.Enabled = True
            PB13.Enabled = True
            PA13.Enabled = True
        End If
        '-------
        If lblGred6.Text = "D" Then
            PB6.Enabled = True
            PA6.Enabled = True
            PB14.Enabled = True
            PA14.Enabled = True
        End If

        If lblGred6.Text = "E" Then
            PB6.Enabled = True
            PA6.Enabled = True
            PB14.Enabled = True
            PA14.Enabled = True
        End If

        If lblGred6.Text = "D-" Then
            PB6.Enabled = True
            PA6.Enabled = True
            PB14.Enabled = True
            PA14.Enabled = True
        End If

        If lblGred6.Text = "D+" Then
            PB6.Enabled = True
            PA6.Enabled = True
            PB14.Enabled = True
            PA14.Enabled = True
        End If

        If lblGred6.Text = "T" Then
            PB6.Enabled = True
            PA6.Enabled = True
            PB14.Enabled = True
            PA14.Enabled = True
        End If
        '----------------
        If lblGred7.Text = "D" Then
            PB7.Enabled = True
            PA7.Enabled = True
            PB15.Enabled = True
            PA15.Enabled = True
        End If

        If lblGred7.Text = "E" Then
            PB7.Enabled = True
            PA7.Enabled = True
            PB15.Enabled = True
            PA15.Enabled = True
        End If

        If lblGred7.Text = "D-" Then
            PB7.Enabled = True
            PA7.Enabled = True
            PB15.Enabled = True
            PA15.Enabled = True
        End If

        If lblGred7.Text = "D+" Then
            PB7.Enabled = True
            PA7.Enabled = True
            PB15.Enabled = True
            PA15.Enabled = True
        End If

        If lblGred7.Text = "T" Then
            PB7.Enabled = True
            PA7.Enabled = True
            PB15.Enabled = True
            PA15.Enabled = True
        End If
        '------
        If lblGred8.Text = "D" Then
            PB8.Enabled = True
            PA8.Enabled = True
            PB16.Enabled = True
        End If

        If lblGred8.Text = "E" Then
            PB8.Enabled = True
            PA8.Enabled = True
            PB16.Enabled = True
            PA16.Enabled = True
        End If

        If lblGred8.Text = "D-" Then
            PB8.Enabled = True
            PA8.Enabled = True
            PB16.Enabled = True
            PA16.Enabled = True
        End If

        If lblGred8.Text = "D+" Then
            PB8.Enabled = True
            PA8.Enabled = True
            PB16.Enabled = True
            PA16.Enabled = True
        End If

        If lblGred8.Text = "T" Then
            PB8.Enabled = True
            PA8.Enabled = True
            PB16.Enabled = True
            PA16.Enabled = True
        End If
    End Sub
    Private Sub Hidden()
        If String.IsNullOrEmpty(lblModul4.Text) Then
            lblGred4.Visible = False
            lblGred5.Visible = False
            lblGred6.Visible = False
            lblGred7.Visible = False
            lblGred8.Visible = False
            PB4.Visible = False
            PB5.Visible = False
            PB6.Visible = False
            PB7.Visible = False
            PB8.Visible = False
            PB12.Visible = False
            PB13.Visible = False
            PB14.Visible = False
            PB15.Visible = False
            PB16.Visible = False
            PA4.Visible = False
            PA5.Visible = False
            PA6.Visible = False
            PA7.Visible = False
            PA8.Visible = False
            PB12.Visible = False
            PA13.Visible = False
            PA14.Visible = False
            PA15.Visible = False
            PA16.Visible = False
        ElseIf String.IsNullOrEmpty(lblModul5.Text) Then
            lblGred5.Visible = False
            lblGred6.Visible = False
            lblGred7.Visible = False
            lblGred8.Visible = False
            PB5.Visible = False
            PB6.Visible = False
            PB7.Visible = False
            PB8.Visible = False

            PB13.Visible = False
            PB14.Visible = False
            PB15.Visible = False
            PB16.Visible = False
            PA5.Visible = False
            PA6.Visible = False
            PA7.Visible = False
            PA8.Visible = False
            PA13.Visible = False
            PA14.Visible = False
            PA15.Visible = False
            PA16.Visible = False
        ElseIf String.IsNullOrEmpty(lblModul6.Text) Then
            lblGred6.Visible = False
            lblGred7.Visible = False
            lblGred8.Visible = False
            PB6.Visible = False
            PB7.Visible = False
            PB8.Visible = False

            PB14.Visible = False
            PB15.Visible = False
            PB16.Visible = False
            PA6.Visible = False
            PA7.Visible = False
            PA8.Visible = False

            PA14.Visible = False
            PA15.Visible = False
            PA16.Visible = False
        ElseIf String.IsNullOrEmpty(lblmodul7.Text) Then
            lblGred7.Visible = False
            lblGred8.Visible = False
            PB7.Visible = False
            PB8.Visible = False

            PB15.Visible = False
            PB16.Visible = False
            PA7.Visible = False
            PA8.Visible = False

            PA15.Visible = False
            PA16.Visible = False
        End If
    End Sub
    Private Sub LoadPage()
        strSQL = "SELECT kpmkv_pelajar.Pengajian, kpmkv_pelajar.Tahun, kpmkv_pelajar.Semester, kpmkv_pelajar.Sesi, kpmkv_pelajar.Nama, kpmkv_pelajar.MYKAD, kpmkv_pelajar.AngkaGiliran, "
        strSQL += " kpmkv_kursus.KursusID, kpmkv_kursus.KodKursus, kpmkv_kursus.NamaKursus, kpmkv_kluster.NamaKluster, kpmkv_kelas.NamaKelas"
        strSQL += " FROM kpmkv_pelajar LEFT OUTER JOIN kpmkv_kursus ON kpmkv_pelajar.KursusID = kpmkv_kursus.KursusID LEFT OUTER JOIN"
        strSQL += " kpmkv_kluster ON kpmkv_kursus.KlusterID = kpmkv_kluster.KlusterID LEFT OUTER JOIN"
        strSQL += " kpmkv_kelas ON kpmkv_pelajar.KelasID = kpmkv_kelas.KelasID"
        strSQL += " WHERE kpmkv_pelajar.PelajarID='" & Request.QueryString("PelajarID") & "'"
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
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Semester")) Then
                    lblSemester.Text = ds.Tables(0).Rows(0).Item("Semester")
                Else
                    lblSemester.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("NamaKluster")) Then
                    lblKluster.Text = ds.Tables(0).Rows(0).Item("NamaKluster")
                Else
                    lblKluster.Text = ""
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
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("KursusID")) Then
                    lblKursusID.Text = ds.Tables(0).Rows(0).Item("KursusID")
                Else
                    lblKursusID.Text = ""
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
            End If
        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Sub ddlTahun_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTahun.SelectedIndexChanged
        kpmkv_kelas_list()
    End Sub
    '--CHECK form validation.
    Private Function ValidateForm() As Boolean
        lblMsg.Text = ""

        If ddlTahun.Text = "PILIH" Then
            lblMsg.Text = "Sila pilih Tahun."
            ddlTahun.Focus()
            Return False
        End If

        If ddlTahun.Text > lblTahun.Text Then
        Else
            lblMsg.Text = "Sila pilih Tahun kehadapan."
            ddlTahun.Focus()
            Return False
        End If

        If chkSesi.Text = "" Then
            lblMsg.Text = "Sila Tick() pada kotak yang Sesi."
            chkSesi.Focus()
            Return False
        End If

        If ddlNamaKelas.Text = "PILIH" Then
            lblMsg.Text = "Sila pilih Kelas."
            ddlNamaKelas.Focus()
            Return False
        End If

        Return True
    End Function
    Protected Sub btnApprove_Click(sender As Object, e As EventArgs) Handles btnApprove.Click
        lblMsg.Text = ""
        'check form validation. if failed exit
        If ValidateForm() = False Then
            Exit Sub
        End If


        '1PB teori
        strSQL = "SELECT KursusID from kpmkv_kursus WHERE Tahun='" & lblTahun.Text & "'"
        strSQL += " AND Sesi='" & lblSesi.Text & "' AND KodKursus='" & lblKodKursus.Text & "'"
        Dim StrKodKursusID As String = oCommon.getFieldValue(strSQL)

        If PB1.Checked = True Then
            'get markah
            Dim A_Teori1 As String

            strSQL = "Select A_Teori1 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            A_Teori1 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(A_Teori1.ToString())) Then
                A_Teori1 = 0
            End If
            '--------------------------

            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul1.Text & "'"
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PBTeori FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PbTeori1 As String = oCommon.getFieldValue(strSQL)
                If PbTeori1 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PBTeori='1', MarkahPATeori='" & A_Teori1 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                    '4/7/2017
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul1.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PBTeori,MarkahPATeori)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul1.Text & "','1', '" & A_Teori1 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If

        If PB9.Checked = True Then
            'get markah
            Dim A_Amali1 As String

            strSQL = "Select A_Amali1 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            A_Amali1 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(A_Amali1.ToString())) Then
                A_Amali1 = 0
            End If
            '--------------------------

            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul1.Text & "'"
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PBAmali FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PbAmali1 As String = oCommon.getFieldValue(strSQL)
                If PbAmali1 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PBAmali='1', MarkahPAAmali='" & A_Amali1 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul1.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PBAmali, MarkahPAAmali)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul1.Text & "','1','" & A_Amali1 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
        End If
        'm1------------------------------------------------------------------
        '2
        If PB2.Checked = True Then
            'get markah
            Dim A_Teori2 As String

            strSQL = "Select A_Teori2 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            A_Teori2 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(A_Teori2.ToString())) Then
                A_Teori2 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul2.Text & "'"
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PBTeori FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PBTeori2 As String = oCommon.getFieldValue(strSQL)
                If PBTeori2 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PBTeori='1', MarkahPATeori='" & A_Teori2 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul2.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PBTeori,MarkahPATeori)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul2.Text & "','1','" & A_Teori2 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If

        If PB10.Checked = True Then
            'get markah
            Dim A_Amali2 As String

            strSQL = "Select A_Amali2 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            A_Amali2 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(A_Amali2.ToString())) Then
                A_Amali2 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul2.Text & "'"
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PBAmali FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PBAmali2 As String = oCommon.getFieldValue(strSQL)
                If PBAmali2 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PBAmali='1', MarkahPAAmali='" & A_Amali2 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul2.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PBAmali,MarkahPAAmali)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul2.Text & "','1','" & A_Amali2 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If
        'm2-----------------------------------
        '3.
        If PB3.Checked = True Then
            'get markah
            Dim A_Teori3 As String

            strSQL = "Select A_Teori3 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            A_Teori3 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(A_Teori3.ToString())) Then
                A_Teori3 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul3.Text & "'"
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PBTeori FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PBTeori3 As String = oCommon.getFieldValue(strSQL)
                If PBTeori3 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PBTeori='1', MarkahPATeori='" & A_Teori3 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul3.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PBTeori,MarkahPATeori)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul3.Text & "','1','" & A_Teori3 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If

        If PB11.Checked = True Then
            'get markah
            Dim A_Amali3 As String

            strSQL = "Select A_Amali3 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            A_Amali3 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(A_Amali3.ToString())) Then
                A_Amali3 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul3.Text & "'"
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PBAmali FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PBAmali3 As String = oCommon.getFieldValue(strSQL)
                If PBAmali3 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PBAmali='1', MarkahPAAmali='" & A_Amali3 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul3.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PBAmali,MarkahPAAmali)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul3.Text & "','1','" & A_Amali3 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If
        'm3-------------------------------------------------------
        '4
        If PB4.Checked = True Then
            'get markah
            Dim A_Teori4 As String

            strSQL = "Select A_Teori4 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            A_Teori4 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(A_Teori4.ToString())) Then
                A_Teori4 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul4.Text & "' "
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PBTeori FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PBTeori4 As String = oCommon.getFieldValue(strSQL)
                If PBTeori4 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PBTeori='1', MarkahPATeori='" & A_Teori4 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul4.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PBTeori,MarkahPATeori)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul4.Text & "','1','" & A_Teori4 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If


        If PB12.Checked = True Then
            'get markah
            Dim A_Amali4 As String

            strSQL = "Select A_Amali4 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            A_Amali4 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(A_Amali4.ToString())) Then
                A_Amali4 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul4.Text & "'"
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PBAmali FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PBAmali4 As String = oCommon.getFieldValue(strSQL)
                If PBAmali4 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PBAmali='1', MarkahPAAmali='" & A_Amali4 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul4.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PBAmali,MarkahPAAmali)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul4.Text & "','1','" & A_Amali4 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If
        'm4-----------------------------------------------------------
        '5
        If PB5.Checked = True Then
            'get markah
            Dim A_Teori5 As String

            strSQL = "Select A_Teori5 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            A_Teori5 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(A_Teori5.ToString())) Then
                A_Teori5 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul5.Text & "' "
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PBTeori FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PBTeori5 As String = oCommon.getFieldValue(strSQL)
                If PBTeori5 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PBTeori='1', MarkahPATeori='" & A_Teori5 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul5.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PBTeori,MarkahPATeori)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul5.Text & "','1','" & A_Teori5 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If

        If PB13.Checked = True Then
            'get markah
            Dim A_Amali5 As String

            strSQL = "Select A_Amali5 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            A_Amali5 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(A_Amali5.ToString())) Then
                A_Amali5 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul5.Text & "' "
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PBAmali FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PBAmali5 As String = oCommon.getFieldValue(strSQL)
                If PBAmali5 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PBAmali='1', MarkahPAAmali='" & A_Amali5 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul5.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PBAmali,MarkahPAAmali)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul5.Text & "','1','" & A_Amali5 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If
        'm5------------------------------------------------------
        '6
        If PB6.Checked = True Then
            'get markah
            Dim A_Teori6 As String

            strSQL = "Select A_Teori6 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            A_Teori6 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(A_Teori6.ToString())) Then
                A_Teori6 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul6.Text & "' "
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PBTeori FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PBTeori6 As String = oCommon.getFieldValue(strSQL)
                If PBTeori6 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PBTeori='1', MarkahPATeori='" & A_Teori6 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul6.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PBTeori,MarkahPATeori)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul6.Text & "','1','" & A_Teori6 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If

        If PB14.Checked = True Then
            'get markah
            Dim A_Amali6 As String

            strSQL = "Select A_Amali6 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            A_Amali6 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(A_Amali6.ToString())) Then
                A_Amali6 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul6.Text & "' "
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PBAmali FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PBAmali6 As String = oCommon.getFieldValue(strSQL)
                If PBAmali6 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PBAmali='1', MarkahPAAmali='" & A_Amali6 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul6.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PBAmali,MarkahPAAmali)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul6.Text & "','1','" & A_Amali6 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If
        'm6-------------------------------------------------------------------------
        '7
        If PB7.Checked = True Then
            'get markah
            Dim A_Teori7 As String

            strSQL = "Select A_Teori7 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            A_Teori7 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(A_Teori7.ToString())) Then
                A_Teori7 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblmodul7.Text & "' "
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PBTeori FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PBTeori7 As String = oCommon.getFieldValue(strSQL)
                If PBTeori7 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PBTeori='1', MarkahPATeori='" & A_Teori7 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblmodul7.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PBTeori,MarkahPATeori)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblmodul7.Text & "','1','" & A_Teori7 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If

        If PB15.Checked = True Then
            'get markah
            Dim A_Amali7 As String

            strSQL = "Select A_Amali7 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            A_Amali7 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(A_Amali7.ToString())) Then
                A_Amali7 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblmodul7.Text & "' "
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PBAmali FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PBAmali7 As String = oCommon.getFieldValue(strSQL)
                If PBAmali7 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PBAmali='1', MarkahPAAmali='" & A_Amali7 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblmodul7.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PBAmali,MarkahPAAmali)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblmodul7.Text & "','1','" & A_Amali7 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If
        'm7------------------------------------------------------------------------------------

        'PATeori-----------------------------
        If PA1.Checked = True Then
            'get markah
            Dim B_Teori1 As String

            strSQL = "Select B_Teori1 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            B_Teori1 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(B_Teori1.ToString())) Then
                B_Teori1 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul1.Text & "' "
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PATeori FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PATeori1 As String = oCommon.getFieldValue(strSQL)
                If PATeori1 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PATeori='1', MarkahPBTeori='" & B_Teori1 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul1.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PATeori,MarkahPBTeori)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul1.Text & "','1','" & B_Teori1 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If
        If PA2.Checked = True Then
            'get markah
            Dim B_Teori2 As String

            strSQL = "Select B_Teori2 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            B_Teori2 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(B_Teori2.ToString())) Then
                B_Teori2 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul2.Text & "' "
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PATeori FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PATeori2 As String = oCommon.getFieldValue(strSQL)
                If PATeori2 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PATeori='1', MarkahPBTeori='" & B_Teori2 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul2.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PATeori,MarkahPBTeori)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul2.Text & "','1','" & B_Teori2 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If
        If PA3.Checked = True Then
            'get markah
            Dim B_Teori3 As String

            strSQL = "Select B_Teori3 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            B_Teori3 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(B_Teori3.ToString())) Then
                B_Teori3 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul3.Text & "'"
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PATeori FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PATeori3 As String = oCommon.getFieldValue(strSQL)
                If PATeori3 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PATeori='1', MarkahPBTeori='" & B_Teori3 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul3.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PATeori,MarkahPBTeori)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul3.Text & "','1','" & B_Teori3 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If
        If PA4.Checked = True Then
            'get markah
            Dim B_Teori4 As String

            strSQL = "Select B_Teori4 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            B_Teori4 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(B_Teori4.ToString())) Then
                B_Teori4 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul4.Text & "' "
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PATeori FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PATeori4 As String = oCommon.getFieldValue(strSQL)
                If PATeori4 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PATeori='1', MarkahPBTeori='" & B_Teori4 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul4.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PATeori,MarkahPBTeori)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul4.Text & "','1','" & B_Teori4 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If
        If PA5.Checked = True Then
            'get markah
            Dim B_Teori5 As String

            strSQL = "Select B_Teori5 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            B_Teori5 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(B_Teori5.ToString())) Then
                B_Teori5 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul5.Text & "' "
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PATeori FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PATeori5 As String = oCommon.getFieldValue(strSQL)
                If PATeori5 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PATeori='1', MarkahPBTeori='" & B_Teori5 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul5.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PATeori,MarkahPBTeori)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul5.Text & "','1','" & B_Teori5 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If
        If PA6.Checked = True Then
            'get markah
            Dim B_Teori6 As String

            strSQL = "Select B_Teori6 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            B_Teori6 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(B_Teori6.ToString())) Then
                B_Teori6 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul6.Text & "' "
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PATeori FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PATeori6 As String = oCommon.getFieldValue(strSQL)
                If PATeori6 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PATeori='1', MarkahPBTeori='" & B_Teori6 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul6.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PATeori,MarkahPBTeori)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul6.Text & "','1','" & B_Teori6 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If
        If PA7.Checked = True Then
            'get markah
            Dim B_Teori7 As String

            strSQL = "Select B_Teori7 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            B_Teori7 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(B_Teori7.ToString())) Then
                B_Teori7 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblmodul7.Text & "' "
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PATeori FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PATeori7 As String = oCommon.getFieldValue(strSQL)
                If PATeori7 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PATeori='1', MarkahPBTeori='" & B_Teori7 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblmodul7.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PATeori,MarkahPBTeori)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblmodul7.Text & "','1','" & B_Teori7 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If

        'PATeori------------------------------------
        If PA9.Checked = True Then
            'get markah
            Dim B_Amali1 As String

            strSQL = "Select B_Amali1 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            B_Amali1 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(B_Amali1.ToString())) Then
                B_Amali1 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul1.Text & "'"
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PAAmali FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PAAmali1 As String = oCommon.getFieldValue(strSQL)
                If PAAmali1 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PAAmali='1', MarkahPBAmali='" & B_Amali1 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul1.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PAAmali,MarkahPBAmali)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul1.Text & "','1','" & B_Amali1 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If
        If PA10.Checked = True Then
            'get markah
            Dim B_Amali2 As String

            strSQL = "Select B_Amali2 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            B_Amali2 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(B_Amali2.ToString())) Then
                B_Amali2 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul2.Text & "' "
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PAAmali FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PAAmali2 As String = oCommon.getFieldValue(strSQL)
                If PAAmali2 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PAAmali='1', MarkahPBAmali='" & B_Amali2 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul2.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PAAmali,MarkahPBAmali)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul2.Text & "','1','" & B_Amali2 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If
        If PA11.Checked = True Then
            'get markah
            Dim B_Amali3 As String

            strSQL = "Select B_Amali3 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            B_Amali3 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(B_Amali3.ToString())) Then
                B_Amali3 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul3.Text & "' "
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PAAmali FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PAAmali3 As String = oCommon.getFieldValue(strSQL)
                If PAAmali3 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PAAmali='1', MarkahPBAmali='" & B_Amali3 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul3.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PAAmali,MarkahPBAmali)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul3.Text & "','1','" & B_Amali3 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If
        If PA12.Checked = True Then
            'get markah
            Dim B_Amali4 As String

            strSQL = "Select B_Amali4 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            B_Amali4 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(B_Amali4.ToString())) Then
                B_Amali4 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul4.Text & "'"
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PAAmali FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PAAmali4 As String = oCommon.getFieldValue(strSQL)
                If PAAmali4 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PAAmali='1', MarkahPBAmali='" & B_Amali4 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul4.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PAAmali,MarkahPBAmali)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul4.Text & "','1','" & B_Amali4 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If
        If PA13.Checked = True Then
            'get markah
            Dim B_Amali5 As String

            strSQL = "Select B_Amali5 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            B_Amali5 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(B_Amali5.ToString())) Then
                B_Amali5 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul5.Text & "'"
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PAAmali FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PAAmali5 As String = oCommon.getFieldValue(strSQL)
                If PAAmali5 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PAAmali='1', MarkahPBAmali='" & B_Amali5 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul5.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PAAmali,MarkahPBAmali)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul5.Text & "','1','" & B_Amali5 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If
        If PA14.Checked = True Then
            'get markah
            Dim B_Amali6 As String

            strSQL = "Select B_Amali6 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            B_Amali6 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(B_Amali6.ToString())) Then
                B_Amali6 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblModul6.Text & "'"
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PAAmali FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PAAmali6 As String = oCommon.getFieldValue(strSQL)
                If PAAmali6 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PAAmali='1', MarkahPBAmali='" & B_Amali6 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblModul6.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PAAmali,MarkahPBAmali)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblModul6.Text & "','1','" & B_Amali6 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If
        If PA15.Checked = True Then
            'get markah
            Dim B_Amali7 As String

            strSQL = "Select B_Amali7 from kpmkv_pelajar_markah Where PelajarID='" & Request.QueryString("PelajarID") & "'"
            strSQL += " AND Tahun='" & lblTahun.Text & "' AND Semester='" & lblSemester.Text & "' "
            strSQL += " AND Sesi='" & lblSesi.Text & "' AND KursusID='" & StrKodKursusID & "'"
            B_Amali7 = oCommon.getFieldValue(strSQL)

            If (String.IsNullOrEmpty(B_Amali7.ToString())) Then
                B_Amali7 = 0
            End If
            '--------------------------
            strSQL = "SELECT PelajarID FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
            strSQL += " AND NamaModul='" & lblmodul7.Text & "'"
            If oCommon.isExist(strSQL) = True Then
                strSQL = "SELECT PAAmali FROM kpmkv_pelajar_ulang_vokasional WHERE  PelajarID='" & Request.QueryString("PelajarID") & "' AND KolejRecordID='" & lblKolejID.Text & "'"
                strSQL += " AND NamaModul='" & lblModul1.Text & "'"
                Dim PAAmali7 As String = oCommon.getFieldValue(strSQL)
                If PAAmali7 = "" Then
                    strSQL = "UPDATE kpmkv_pelajar_ulang_vokasional SET PAAmali='1', MarkahPBAmali='" & B_Amali7 & "' Where PelajarID ='" & Request.QueryString("PelajarID") & "'"
                    strRet = oCommon.ExecuteSQL(strSQL)
                Else
                    divMsg.Attributes("class") = "error"
                    lblMsg.Text = "Calon ini telah mengulang bagi NamaModul='" & lblmodul7.Text & "'"
                    Exit Sub
                End If
            Else
                strSQL = "INSERT INTO kpmkv_pelajar_ulang_vokasional(PelajarID, KolejRecordID, Pengajian, Tahun, Semester, Sesi, KursusID, KelasID, NamaModul, PAAmali,MarkahPBAmali)"
                strSQL += "VALUES('" & Request.QueryString("PelajarID") & "','" & lblKolejID.Text & "','" & lblPengajian.Text & "','" & ddlTahun.Text & "','" & ddlSemester.Text & "','" & chkSesi.Text & "','" & ddlKodKursus.Text & "','" & ddlNamaKelas.SelectedValue & "','" & lblmodul7.Text & "','1','" & B_Amali7 & "')"
                strRet = oCommon.ExecuteSQL(strSQL)
            End If
            End If

        If lblMsg.Text = "" Then
            divMsg.Attributes("class") = "info"
            lblMsg.Text = "Berjaya!.Daftar Calon Ulang Vokasional."
            'Modul_Nama()
            'Hidden()
            'HiddenPBPACheckBox()
        End If
        Modul_Nama()
        Hidden()
        HiddenPBPACheckBox()

    End Sub


    Private Sub chkSesi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkSesi.SelectedIndexChanged
        kpmkv_kodkursus_list()
        kpmkv_kelas_list()
    End Sub

    Private Sub ddlKodKursus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlKodKursus.SelectedIndexChanged
        kpmkv_kelas_list()
    End Sub
End Class
