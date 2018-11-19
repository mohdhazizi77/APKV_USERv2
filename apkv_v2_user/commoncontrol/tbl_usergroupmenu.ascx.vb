Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Globalization


Partial Public Class tbl_usergroupmenu
    Inherits System.Web.UI.UserControl
    Dim strSQL As String
    Dim oCommon As New Commonfunction
    Dim strRet As String
    Dim strConn As String = ConfigurationManager.AppSettings("ConnectionString")
    Dim objConn As SqlConnection = New SqlConnection(strConn)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            tbl_usergroupmenu_load()
        End If
    End Sub
    Private Sub tbl_usergroupmenu_load()

        Try

            strSQL = "select tbl_menuheader_kolej.Idx, tbl_menu_kolej .HeaderCode,LOWER(tbl_menuheader_kolej.HeaderText) AS HeaderText  from tbl_usergroupmenu_kolej "
            strSQL += " inner join tbl_menu_kolej on tbl_usergroupmenu_kolej.MenuCode = tbl_menu_kolej.MenuCode"
            strSQL += " inner join tbl_menuheader_kolej on tbl_menuheader_kolej .HeaderCode =tbl_menu_kolej.HeaderCode "
            strSQL += " WHERE tbl_usergroupmenu_kolej.UserGroupCode='" & Session("UserGroupCode") & "'"

            strSQL += " group by tbl_menu_kolej .HeaderCode ,tbl_menuheader_kolej .HeaderCode ,tbl_menuheader_kolej.HeaderText,tbl_menuheader_kolej.Idx "
            strSQL += " order by tbl_menuheader_kolej.Idx ASC"
            'Response.Write(strSQL)

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

            Dim strHeader, strMenu As String
            Dim strHeaderText, strMenuLink, strSubMenuText As String
            Dim strMenuImg As String
            Dim strHeaderCode As String
            strHeader = ""
            strMenu = ""
            strMenuImg = ""
            strHeaderCode = ""
            If numrows > 0 Then
                For i = 0 To numrows - 1
                    'Print header for menu
                    strHeaderText = ds.Tables(0).Rows(i).Item("HeaderText")
                    strHeaderCode = ds.Tables(0).Rows(i).Item("HeaderCode")

                    strSQL = "SELECT tbl_submenu_kolej.Idx,tbl_submenu_kolej.SubMenuImg ,tbl_submenu_kolej.SubMenuLink, tbl_submenu_kolej.SubMenuText"
                    strSQL += " FROM tbl_submenu_kolej INNER JOIN tbl_usergroupmenu_kolej on tbl_usergroupmenu_kolej.MenuCode=tbl_submenu_kolej .ParentMenuCode"
                    strSQL += " WHERE tbl_usergroupmenu_kolej.UserGroupCode='" & Session("UserGroupCode") & "' "
                    strSQL += " AND tbl_submenu_kolej.HeaderCode ='" & strHeaderCode & "' ORDER BY tbl_submenu_kolej.Idx ASC"
                    strRet = oCommon.getFieldValue(strSQL)
                    'Response.Write(strSQL)

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
                    '--get header
                    strMenu += "<tr><td class='fbnav_header' colspan=2'>" & strHeaderText & "</td></tr>"
                    For k = 0 To numrows1 - 1
                        strMenuImg = ds1.Tables(0).Rows(k).Item("SubMenuImg")
                        strMenuLink = ds1.Tables(0).Rows(k).Item("SubMenuLink")
                        strSubMenuText = ds1.Tables(0).Rows(k).Item("SubMenuText")


                        strMenu += "<tr class='fbnav_items'>"
                        strMenu += "<td class='fbnav_items'><a href='" & strMenuLink & "' rel='nofollow' target='_self'><img style='vertical-align: middle; border: none;' src='" & strMenuImg & "' width='16px' height='16px' alt='::' />" & strSubMenuText & "</a></td>"
                        strMenu += "</tr>"

                    Next
                    strMenu += "<tr><td colspan=2>&nbsp</td></tr>"
                    tblUserGroupMenu.InnerHtml = strMenu

                Next


            End If
        Catch ex As Exception
            objConn.Dispose()
        End Try

    End Sub

    
End Class