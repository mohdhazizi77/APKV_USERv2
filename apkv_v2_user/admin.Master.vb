Imports System.Data.SqlClient
Imports System.Globalization
Partial Public Class admin
    Inherits System.Web.UI.MasterPage
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""
    Dim IntTakwim As Integer = 0

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                takwim()

            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub takwim()
        '------exist takwim
        strSQL = "SELECT * FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Notifikasi Kemasukan Markah' AND Aktif='1'"
        If oCommon.isExist(strSQL) = True Then

            'count data takwim
            'Get the data from database into datatable
            Dim cmd As New SqlCommand("SELECT TakwimID FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText='Notifikasi Kemasukan Markah' AND Aktif='1'")
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
                        lblNotifikasi.Text = "<a href='notifikasi.kemasukanMarkah.aspx?Action=p' style='color:Yellow'>Notifikasi Kemasukan Markah</a>"


                    End If
                Else
                    lblNotifikasi.Text = "Notifikasi Kemasukan Markah"
                End If
            Next
        Else
            lblNotifikasi.Text = "Notifikasi Kemasukan Markah"


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
End Class