Public Partial Class logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            divMsg.Attributes("class") = "error"
            lblMsg.Text = Request.QueryString("msg")

            Response.Redirect("default.aspx")
        Catch ex As Exception
            lblMsg.Text = "Sessi tamat. Sila login semula."
        End Try

    End Sub

End Class