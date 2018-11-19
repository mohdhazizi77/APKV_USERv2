Imports System.Data.SqlClient
Imports System.Globalization
Public Class notifikasi_kemasukanMarkah
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String
    Dim strRet As String
    Dim IntTakwim As Integer = 0

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                lblMsg.Text = ""


                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Server.HtmlEncode(Request.Cookies("kpmkv_loginid").Value) & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                takwim()

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub takwim()
        '------exist takwim
        strSQL = "SELECT * FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Pentaksiran Berterusan Vokasional' AND Aktif='1'"
        If oCommon.isExist(strSQL) = True Then

            'count data takwim
            'Get the data from database into datatable
            Dim cmd As New SqlCommand("SELECT TakwimID FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Pentaksiran Berterusan Vokasional' AND Aktif='1'")
            Dim dt As DataTable = GetData(cmd)

            For i As Integer = 0 To dt.Rows.Count - 1
                IntTakwim = dt.Rows(i)("TakwimID")

                strSQL = "SELECT TarikhMula,TarikhAkhir FROM kpmkv_takwim WHERE TakwimID='" & IntTakwim & "'"
                strRet = oCommon.getFieldValueEx(strSQL)

                Dim ar_user_login As Array
                ar_user_login = strRet.Split("|")
                Dim strMula As String = ar_user_login(0)
                Dim strAkhir As String = ar_user_login(1)

                Dim strdateNow As Date = Date.Now
                Dim startDate = DateTime.ParseExact(strMula, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                Dim endDate = DateTime.ParseExact(strAkhir, "dd-MM-yyyy", CultureInfo.InvariantCulture)

                Dim ts As New TimeSpan
                ts = endDate.Subtract(strdateNow)
                Dim dayDiff = ts.Days

                If strMula IsNot Nothing Then
                    If strAkhir IsNot Nothing And dayDiff >= 0 Then
                        'list here
                        pbv()


                    End If
                Else

                    lblMsg.Text = "Pentaksiran Berterusan Vokasional telah ditutup!"
                End If
            Next
        Else

            lblMsg.Text = "Pentaksiran Berterusan Vokasional telah ditutup!"
        End If

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


    Private Sub pbv()
        Try

            strSQL = "SELECT Kohort,Semester,Sesi FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "'"
            strSQL +=" And SubMenuText ='Pentaksiran Berterusan Vokasional' AND Aktif='1' ORDER BY Kohort ASC"

            Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
            Dim objConn As SqlConnection = New SqlConnection(strConn)
            Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            Dim nCount As Integer = 1
            Dim MyTable As DataTable = New DataTable
            MyTable = ds.Tables(0)
            Dim numrows As Integer

            numrows = MyTable.Rows.Count

            Dim strKohortText, strSesiText, strSemesterText, strMenu As String
            Dim strKohort, strSesi, strSemester, strKodKursus As String
            Dim strCount As Integer

            strKohortText = ""
            strSesiText = ""
            strSemesterText = ""
            strMenu = ""

            strKohort = ""
            strSesi = ""
            strSemester = ""
            strKodKursus = ""
            strCount = 0



            If numrows > 0 Then
                For i = 0 To numrows - 1
                    'Print header for menu
                    strKohortText = ds.Tables(0).Rows(i).Item("Kohort")
                    strSemesterText = ds.Tables(0).Rows(i).Item("Semester")
                    strSesiText = ds.Tables(0).Rows(i).Item("Sesi")



                    strSQL = "SELECT max(m.tahun) as tahun,max(m.sesi) as sesi,max(m.Semester) as semester,max(k.KodKursus) as kod"
                    strSQL += " FROM kpmkv_pelajar_markah AS m"
                    strSQL += " LEFT JOIN kpmkv_kursus AS k ON m.KursusID =k.KursusID "
                    strSQL += " LEFT JOIN kpmkv_pelajar as p ON p.PelajarID =m.PelajarID "
                    strSQL += " WHERE m.tahun='" & strKohortText & "' AND (m.isSahPBV='0' OR m.isSahPBV IS NULL)  "
                    strSQL += " AND m.KolejRecordID='" & lblKolejID.Text & "' "
                    strSQL += " AND  p.StatusID ='2' AND p.JenisCalonID ='2'"
                    strSQL += " AND m.Semester='" & strSemesterText & "' AND m.Sesi='" & strSesiText & "' "
                    strSQL += " AND p.KelasID IS NOT NULL"
                    strSQL += " GROUP BY m.tahun, m.sesi, k.KodKursus,m.Semester"
                    strSQL += " ORDER BY m.tahun,m.Semester "
                    strRet = oCommon.getFieldValue(strSQL)


                    Dim strConn1 As String = ConfigurationManager.AppSettings("ConnectionString")
                    Dim objConn1 As SqlConnection = New SqlConnection(strConn1)
                    Dim sqlDA1 As New SqlDataAdapter(strSQL, objConn1)

                    Dim ds1 As DataSet = New DataSet
                    sqlDA1.Fill(ds1, "AnyTable")

                    Dim nCount1 As Integer = 1
                    Dim MyTable1 As DataTable = New DataTable
                    MyTable1 = ds1.Tables(0)
                    Dim numrows1 As Integer

                    numrows1 = MyTable1.Rows.Count


                    For k = 0 To numrows1 - 1
                        strKohort = ds1.Tables(0).Rows(k).Item("Tahun")
                        strSemester = ds1.Tables(0).Rows(k).Item("Semester")
                        strSesi = ds1.Tables(0).Rows(k).Item("Sesi")
                        strKodKursus = ds1.Tables(0).Rows(k).Item("Kod")


                        strMenu += "<tr>"
                        strMenu += "<td style ='border-Bottom:solid 1px black;border-Right:solid 1px grey;width:15%;text-align:center;padding:2px'>" & strKohort & "</td>"
                        strMenu += "<td style ='border-Bottom:solid 1px black;border-Right:solid 1px grey;width:10%;text-align:center;padding:2px'>" & strSemester & "</td>"
                        strMenu += "<td style ='border-Bottom:solid 1px black;border-Right:solid 1px grey;width:10%;text-align:center;padding:2px'>" & strSesi & "</td>"
                        strMenu += "<td style ='border-Bottom:solid 1px black;;padding:2px'>"
                        strMenu += "<a href='markah.create.PBV.aspx?VKohort=" + strKohort + "&VSemester=" + strSemester + "&VSesi=" + strSesi + "&VKod=" + strKodKursus + "'>" & strKodKursus & "</td>"
                        strMenu += "</tr>"


                    Next
                    strCount += numrows1

                Next
                tblContent.InnerHtml = strMenu


            End If
        Catch ex As Exception
            objConn.Dispose()
            End Try
    End Sub

End Class