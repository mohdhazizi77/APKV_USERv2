Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient

Public Class takwim_update
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String
    Dim strRet As String
    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                kpmkv_tahun_list()
                ddlTahun.Text = Now.Year

                menu_list()
                ddlMenu.Text = "ALL"

                kpmkv_takwim_load()
            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try

    End Sub

    Private Sub kpmkv_tahun_list()
        strSQL = "SELECT Tahun FROM kpmkv_tahun WITH (NOLOCK) ORDER BY Tahun"
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

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub menu_list()

        strSQL = "SELECT * FROM tbl_menuheader ORDER BY HeaderCode"
        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try

            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            Dim nCount As Integer = 1
            Dim MyTable As DataTable = New DataTable
            MyTable = ds.Tables(0)

            ddlMenu.DataSource = ds
            ddlMenu.DataTextField = "HeaderText"
            ddlMenu.DataValueField = "HeaderCode"
            ddlMenu.DataBind()

            ''--add blank row
            ddlMenu.Items.Insert(0, New ListItem("ALL", "0"))


        Catch ex As Exception
            lblMsg.Text = "Database error!" & ex.Message
        Finally
            objConn.Dispose()
        End Try

    End Sub
    Private Sub kpmkv_takwim_load()
        strSQL = "SELECT * FROM kpmkv_takwim WHERE TakwimID=" & Request.QueryString("takwimid")
        '--debug
        'Response.Write(strSQL)

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
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Tahun")) Then
                    ddlTahun.Text = ds.Tables(0).Rows(0).Item("Tahun")
                Else
                    ddlTahun.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Kategori")) Then
                    ddlMenu.SelectedItem.Text = ds.Tables(0).Rows(0).Item("Kategori")
                Else
                    ddlMenu.SelectedItem.Text = ""
                End If

                Dim strTarikhMula As String = ""
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("TarikhMula")) Then
                    strTarikhMula = ds.Tables(0).Rows(0).Item("TarikhMula")
                Else
                    strTarikhMula = ""
                End If
                txtTarikhMula.Text = oCommon.DateFormat(strTarikhMula, "dddd dd-MM-yyyy")
                calTarikhMula.SelectedDate = txtTarikhMula.Text


                Dim strTarikhAkhir As String = ""
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("TarikhAkhir")) Then
                    strTarikhAkhir = ds.Tables(0).Rows(0).Item("TarikhAkhir")
                Else
                    strTarikhAkhir = ""
                End If
                txtTarikhAkhir.Text = oCommon.DateFormat(strTarikhAkhir, "dddd dd-MM-yyyy")
                calTarikhAkhir.SelectedDate = txtTarikhAkhir.Text

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Tajuk")) Then
                    txtTajuk.Text = ds.Tables(0).Rows(0).Item("Tajuk")
                Else
                    txtTajuk.Text = ""
                End If

                If Not IsDBNull(ds.Tables(0).Rows(0).Item("Catatan")) Then
                    txtCatatan.Text = ds.Tables(0).Rows(0).Item("Catatan").ToString.Replace(Environment.NewLine, "<br />")
                Else
                    txtCatatan.Text = ""
                End If

            End If

        Catch ex As Exception
            'lblMsg.Text = "System error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try
    End Sub


    Protected Sub lnkList_Click(sender As Object, e As EventArgs) Handles lnkList.Click
        Response.Redirect("admin.takwim.list.aspx")

    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        'check form validation. if failed exit
        If ValidateForm() = False Then
            Exit Sub
        End If

        lblTarikhMula.Text = calTarikhMula.SelectedDate.ToString("yyyy-MM-dd")
        lblTarikhAkhir.Text = calTarikhAkhir.SelectedDate.ToString("yyyy-MM-dd")

        'UPDATE
        strSQL = "UPDATE kpmkv_takwim SET Tahun='" & ddlTahun.Text & "',Kategori='" & ddlMenu.Text & "',TarikhMula='" & lblTarikhMula.Text & "',TarikhAkhir='" & lblTarikhAkhir.Text & "',Tajuk='" & oCommon.FixSingleQuotes(txtTajuk.Text.ToUpper) & "',Catatan='" & oCommon.FixSingleQuotes(txtCatatan.Text) & "' WHERE TakwimID=" & Request.QueryString("takwimid")
        strRet = oCommon.ExecuteSQL(strSQL)
        If strRet = "0" Then
            lblMsg.Text = "Kemaskini berjaya!"
        Else
            lblMsg.Text = "system error:" & strRet
        End If

    End Sub


    '--CHECK form validation.
    Private Function ValidateForm() As Boolean
        If txtTarikhMula.Text.Length = 0 Then
            lblMsg.Text = "Medan ini mesti diisi."
            txtTarikhMula.Focus()
            Return False
        End If

        If txtTarikhAkhir.Text.Length = 0 Then
            lblMsg.Text = "Medan ini mesti diisi."
            txtTarikhAkhir.Focus()
            Return False
        End If

        Return True
    End Function
    Private Sub btnDateMula_Click(sender As Object, e As ImageClickEventArgs) Handles btnDateMula.Click
        Dim [date] As New DateTime()
        'Flip the visibility attribute
        calTarikhMula.Visible = Not (calTarikhMula.Visible)
        'If the calendar is visible try assigning the date from the textbox
        If calTarikhMula.Visible Then
            'If the Conversion was successfull assign the textbox's date
            If DateTime.TryParse(txtTarikhMula.Text, [date]) Then
                calTarikhMula.SelectedDate = [date]
            End If
            calTarikhMula.Attributes.Add("style", "POSITION: absolute")
        End If

    End Sub

    Private Sub calTarikhMula_SelectionChanged(sender As Object, e As EventArgs) Handles calTarikhMula.SelectionChanged
        txtTarikhMula.Text = calTarikhMula.SelectedDate.ToString("dddd dd-MM-yyyy")
        lblTarikh.Text = calTarikhMula.SelectedDate.ToString("yyyy-MM-dd")

        calTarikhMula.Visible = False
    End Sub

    Private Sub btnDateAkhir_Click(sender As Object, e As ImageClickEventArgs) Handles btnDateAkhir.Click
        Dim [date] As New DateTime()
        'Flip the visibility attribute
        calTarikhAkhir.Visible = Not (calTarikhAkhir.Visible)
        'If the calendar is visible try assigning the date from the textbox
        If calTarikhAkhir.Visible Then
            'If the Conversion was successfull assign the textbox's date
            If DateTime.TryParse(txtTarikhAkhir.Text, [date]) Then
                calTarikhAkhir.SelectedDate = [date]
            End If
            calTarikhAkhir.Attributes.Add("style", "POSITION: absolute")
        End If

    End Sub

    Private Sub calTarikhAkhir_SelectionChanged(sender As Object, e As EventArgs) Handles calTarikhAkhir.SelectionChanged
        txtTarikhAkhir.Text = calTarikhAkhir.SelectedDate.ToString("dddd dd-MM-yyyy")
        lblTarikh.Text = calTarikhAkhir.SelectedDate.ToString("yyyy-MM-dd")

        calTarikhAkhir.Visible = False
    End Sub

End Class