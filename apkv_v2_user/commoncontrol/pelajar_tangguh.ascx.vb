﻿Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization

Public Class pelajar_tangguh1
    Inherits System.Web.UI.UserControl
    Dim oCommon As New Commonfunction
    Dim strSQL As String = ""
    Dim strRet As String = ""

    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Dim SubMenuText As String = "Penilaian Akhir Akademik"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                'kolejnama
                strSQL = "SELECT Nama FROM kpmkv_users WHERE LoginID='" & Session("LoginID") & "'"
                Dim strKolejnama As String = oCommon.getFieldValue(strSQL)
                'kolejid
                strSQL = "SELECT RecordID FROM kpmkv_kolej WHERE Nama='" & strKolejnama & "'"
                lblKolejID.Text = oCommon.getFieldValue(strSQL)

                lblMsg.Text = ""

                ''------exist takwim
                'strSQL = "SELECT * FROM kpmkv_takwim WHERE Tahun='" & Now.Year & "' AND SubMenuText = '" & SubMenuText & "' AND Aktif='1' AND GETDATE() BETWEEN CONVERT(date, TarikhMula, 103) AND DATEADD(day,1,CONVERT(date, TarikhAkhir, 103))"

                'If oCommon.isExist(strSQL) = True Then

                '    chkSesi.Enabled = True

                '    kpmkv_tahun_list()
                '    kpmkv_semester_list()
                '    kpmkv_kodkursus_list()
                '    kpmkv_kelas_list()

                'Else

                '    chkSesi.Enabled = False

                '    btnGred.Enabled = False
                '    btnUpdate.Enabled = False
                '    lblMsg.Text = "Penilaian Akhir Akademik telah ditutup!"

                'End If

            End If



        Catch ex As Exception
            lblMsg.Text = "System Error:" & ex.Message
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        lblMsg.Text = ""
        BindGrid()
    End Sub

    Private Sub BindGrid()

        Using cmd As New SqlCommand(getSQL)
            Using sda As New SqlDataAdapter()
                cmd.Connection = objConn
                cmd.Parameters.AddWithValue("@MYKAD", txtMykad.Text)
                cmd.Parameters.AddWithValue("@KolejRecordID", lblKolejID.Text)
                sda.SelectCommand = cmd
                Using dt As New DataTable()

                    sda.Fill(dt)

                    datRespondent.DataSource = dt
                    datRespondent.DataBind()

                    If dt.Rows.Count = 0 Then

                        divMsg.Attributes("class") = "error"
                        lblMsg.Text = "Rekod tidak dijumpai!"

                    Else

                        divMsg.Attributes("class") = "info"
                        lblMsg.Text = "Jumlah Rekod#:" & dt.Rows.Count

                    End If

                End Using
            End Using
        End Using

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
        Dim strOrder As String = " ORDER BY Tahun DESC"

        tmpSQL = "SELECT * FROM kpmkv_pelajar LEFT OUTER JOIN kpmkv_status ON kpmkv_pelajar.StatusID = kpmkv_status.StatusID WHERE kpmkv_pelajar.StatusID='3' AND KolejRecordID = @KolejRecordID "


        '--Negeri
        If Not txtMykad.Text = "" Then
            strWhere += " AND MYKAD = @MYKAD "
        End If


        getSQL = tmpSQL & strWhere & strOrder
        ''--debug
        ''Response.Write(getSQL)

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

    Private Sub datRespondent_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles datRespondent.SelectedIndexChanging
        Dim strKeyID As String = datRespondent.DataKeys(e.NewSelectedIndex).Value.ToString
        Dim encryptStrKeyID As String = HttpUtility.UrlEncode(oCommon.Encrypt(strKeyID.Trim()))
        Response.Redirect("pelajar.tangguh.update.aspx?PelajarID=" & encryptStrKeyID)

    End Sub


End Class