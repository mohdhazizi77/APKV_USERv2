Imports System.Data.SqlClient

Public Class pentaksir_bmsetara_reset
    Inherits System.Web.UI.UserControl

    Dim oCommon As New Commonfunction
    Dim strSQL As String
    Dim strRet As String
    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Dim strKolejRecordID As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then

                lblMsg.Text = ""

            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message
        End Try
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click

        Response.Redirect("admin.login.success.aspx")

    End Sub

    Private Sub btnCari_Click(sender As Object, e As EventArgs) Handles btnCari.Click

        lblAG.Text = ""
        lblMYKAD.Text = ""
        lblNama.Text = ""
        ddlMP_list()
        ddlPentaksir_list()

        If Not txtCarian.Text = "" Then

            strSQL = "SELECT RecordID FROM kpmkv_users WHERE LoginID = '" & Session("LoginID") & "' AND Pwd = '" & Session("Password") & "'"
            Dim RecordID As String = oCommon.getFieldValue(strSQL)

            strSQL = "  SELECT AngkaGiliran, MYKAD, Nama, KolejRecordID FROM kpmkv_pentaksir_bmsetara_calon
                        WHERE 
                        KolejRecordID = '" & RecordID & "'
                        AND (AngkaGiliran = '" & txtCarian.Text & "'
                        OR MYKAD = '" & txtCarian.Text & "')"
            strRet = oCommon.getFieldValueEx(strSQL)

            If Not strRet = "" Then

                Dim ar_Pelajar As Array
                ar_Pelajar = strRet.Split("|")

                Dim strAG As String = ar_Pelajar(0)
                Dim strMYKAD As String = ar_Pelajar(1)
                Dim strNama As String = ar_Pelajar(2)
                strKolejRecordID = ar_Pelajar(3)

                lblAG.Text = strAG
                lblMYKAD.Text = strMYKAD
                lblNama.Text = strNama

                ddlMP_list()
                ddlMP.SelectedIndex = 0
                ddlPentaksir_list()
                ddlPentaksir.SelectedIndex = 0

            End If

        End If

    End Sub

    Private Sub ddlMP_list()

        strSQL = "  SELECT MataPelajaran FROM kpmkv_pentaksir_bmsetara
                    WHERE KolejRecordID = '" & strKolejRecordID & "'"

        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlMP.DataSource = ds
            ddlMP.DataTextField = "MataPelajaran"
            ddlMP.DataValueField = "MataPelajaran"
            ddlMP.DataBind()

            '--ALL
            'ddlMP.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Sub ddlPentaksir_list()

        strSQL = "  SELECT DISTINCT Nama FROM kpmkv_pentaksir_bmsetara
                    WHERE KolejRecordID = '" & strKolejRecordID & "'"

        Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
        Dim objConn As SqlConnection = New SqlConnection(strConn)
        Dim sqlDA As New SqlDataAdapter(strSQL, objConn)

        Try
            Dim ds As DataSet = New DataSet
            sqlDA.Fill(ds, "AnyTable")

            ddlPentaksir.DataSource = ds
            ddlPentaksir.DataTextField = "Nama"
            ddlPentaksir.DataValueField = "Nama"
            ddlPentaksir.DataBind()

            '--ALL
            'ddlPentaksir.Items.Add(New ListItem("PILIH", "PILIH"))

        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message

        Finally
            objConn.Dispose()
        End Try

    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click

        If Not lblAG.Text = "" Then

            If ddlMP.SelectedValue = "BAHASA MELAYU 3" Then

                strSQL = "  UPDATE kpmkv_pentaksir_bmsetara_calon
                        SET
                        BM3_1 = NULL,
                        StatusBM3_1 = 0,
                        BM3_2 = NULL,
                        StatusBM3_2 = 0,
                        BM3_Total = NULL,
                        StatusHantarBM3_Pentaksir = 'BELUM HANTAR'
                        WHERE
                        MYKAD = '" & lblMYKAD.Text & "'
                        AND AngkaGiliran = '" & lblAG.Text & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

            Else

                strSQL = "  UPDATE kpmkv_pentaksir_bmsetara_calon
                        SET
                        BM3 = NULL,
                        BM4_Total = NULL,
                        StatusHantarBM4_Pentaksir = 'BELUM HANTAR'
                        WHERE
                        MYKAD = '" & lblMYKAD.Text & "'
                        AND AngkaGiliran = '" & lblAG.Text & "'"
                strRet = oCommon.ExecuteSQL(strSQL)

            End If

        End If

    End Sub
End Class

