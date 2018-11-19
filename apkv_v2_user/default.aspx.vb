Public Partial Class _default1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Request.Cookies("kpmkv_loginid") IsNot Nothing Then
                Response.Cookies("kpmkv_loginid").Expires = DateTime.Now.AddDays(-1)
            End If
        Catch ex As Exception
            Response.Cookies("kpmkv_loginid").Value = ""
            Response.Cookies("kpmkv_loginid").Expires = DateTime.Now.AddDays(1)
        End Try

    End Sub
    

End Class