Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization

Partial Public Class kolej_view_header
    Inherits System.Web.UI.UserControl


    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                LoadPage()
            End If

        Catch ex As Exception
            'lblMsg.Text = "System Error:" & ex.Message
        End Try

    End Sub

    Private Sub LoadPage()
        strSQL = "SELECT * FROM kpmkv_kolej WHERE RecordID='" & Request.QueryString("RecordID") & "'"
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
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Jenis")) Then
                    lblJenis.Text = ds.Tables(0).Rows(0).Item("Jenis")
                Else
                    lblJenis.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Kod")) Then
                    lblKod.Text = ds.Tables(0).Rows(0).Item("Kod")
                Else
                    lblKod.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Nama")) Then
                    lblNama.Text = ds.Tables(0).Rows(0).Item("Nama")
                Else
                    lblNama.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Tel")) Then
                    lblTel.Text = ds.Tables(0).Rows(0).Item("Tel")
                Else
                    lblTel.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Fax")) Then
                    lblFax.Text = ds.Tables(0).Rows(0).Item("Fax")
                Else
                    lblFax.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Email")) Then
                    lblEmail.Text = ds.Tables(0).Rows(0).Item("Email")
                Else
                    lblEmail.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Alamat1")) Then
                    lblAlamat1.Text = ds.Tables(0).Rows(0).Item("Alamat1")
                Else
                    lblAlamat1.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Alamat2")) Then
                    lblAlamat2.Text = ds.Tables(0).Rows(0).Item("Alamat2")
                Else
                    lblAlamat2.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Poskod")) Then
                    lblPoskod.Text = ds.Tables(0).Rows(0).Item("Poskod")
                Else
                    lblPoskod.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Bandar")) Then
                    lblBandar.Text = ds.Tables(0).Rows(0).Item("Bandar")
                Else
                    lblBandar.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Negeri")) Then
                    lblNegeri.Text = ds.Tables(0).Rows(0).Item("Negeri")
                Else
                    lblNegeri.Text = ""
                End If

                '--PEngarah
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("NamaPengarah")) Then
                    lblNamaPengarah.Text = ds.Tables(0).Rows(0).Item("NamaPengarah")
                Else
                    lblNamaPengarah.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("EmailPengarah")) Then
                    lblEmailPengarah.Text = ds.Tables(0).Rows(0).Item("EmailPengarah")
                Else
                    lblEmailPengarah.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("MobilePengarah")) Then
                    lblBimbitPengarah.Text = ds.Tables(0).Rows(0).Item("MobilePengarah")
                Else
                    lblBimbitPengarah.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("TelPengarah")) Then
                    lblTelPengarah.Text = ds.Tables(0).Rows(0).Item("TelPengarah")
                Else
                    lblTelPengarah.Text = ""
                End If

                '--SUP
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("NamaKJPP")) Then
                    lblNamaKJPP.Text = ds.Tables(0).Rows(0).Item("NamaKJPP")
                Else
                    lblNamaKJPP.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("EmailKJPP")) Then
                    lblEmailKJPP.Text = ds.Tables(0).Rows(0).Item("EmailKJPP")
                Else
                    lblEmailKJPP.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("MobileKJPP")) Then
                    lblBimbitPengarah.Text = ds.Tables(0).Rows(0).Item("MobileKJPP")
                Else
                    lblBimbitPengarah.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("TelKJPP")) Then
                    lblTelKJPP.Text = ds.Tables(0).Rows(0).Item("TelKJPP")
                Else
                    lblTelKJPP.Text = ""
                End If

            End If

        Catch ex As Exception
           
        Finally
            objConn.Dispose()
        End Try

    End Sub

End Class