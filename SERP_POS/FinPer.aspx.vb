Public Class FinPer
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut, Status, SuperUser, Funcion As String
    Private Datos As DataSet
    Private Tabla As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario_Aut") = "" Or Session("Destino") <> "FinPer.aspx" Then
            Response.Redirect("inicio.aspx")
        End If

        Usuario = Session("Usuario")
        Clave = Session("Clave")
        Bd = Session("Bd")
        Servidor = Session("Sevidor")
        Funcion = Session("Funcion")
        Session.Timeout = Session("Tiempo")

        lblMsg.Text = ""
        DivMSG.Attributes.Remove("class")

        If Not IsPostBack Then
            Tabla = New DataTable
            Tabla.Columns.Add("ID")
            Tabla.Columns.Add("Descripcion")
            Tabla.Columns.Add("FechaDoc1")
            Tabla.Columns.Add("FechaDoc2")
            Tabla.Columns.Add("FechaVen1")
            Tabla.Columns.Add("FechaVen2")
            Tabla.Columns.Add("Estado")
            Session.Add("Tabla", Tabla)
            LlenarPeriodos()
        End If
    End Sub

    Private Sub LlenarPeriodos()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT ID, Descripcion, CONVERT(VARCHAR,FechaDoc1,120) FechaDoc1, CONVERT(VARCHAR,FechaDoc2,120) FechaDoc2, CONVERT(VARCHAR,FechaVen1,120) FechaVen1, CONVERT(VARCHAR,FechaVen2,120) FechaVen2, CodUsuario, FechaSis, Estado
               FROM FinPeriodos"
        Datos = Conf.EjecutaSql(Sql)
        Session.Add("Tabla", Datos.Tables(0))
        gvDetalle.DataSource = Session("Tabla")
        gvDetalle.DataBind()
    End Sub

    Private Sub GvDetalle_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvDetalle.RowDeleting
        Session("Tabla").Rows(e.RowIndex).Delete()
        Session("Tabla").AcceptChanges()
        gvDetalle.DataSource = Session("Tabla")
        gvDetalle.DataBind()
    End Sub

    Private Sub GvDetalle_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvDetalle.RowEditing
        gvDetalle.EditIndex = e.NewEditIndex
        gvDetalle.DataSource = Session("Tabla")
        gvDetalle.DataBind()
    End Sub

    Private Sub GvDetalle_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvDetalle.RowCancelingEdit
        gvDetalle.EditIndex = -1
        gvDetalle.DataSource = Session("Tabla")
        gvDetalle.DataBind()
    End Sub

    Private Sub GvDetalle_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gvDetalle.RowUpdating
        Dim Fila = gvDetalle.Rows(e.RowIndex)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "DECLARE @CU varchar(1) = 'U'
            ,@ID int = '" + CType(Fila.FindControl("glblID"), Label).Text.TrimEnd + "'
            ,@Descripcion varchar(50) = '" + CType(Fila.FindControl("gtxtDescripcion"), TextBox).Text.TrimEnd + "'
            ,@FechaSis1 date = '" + CType(Fila.FindControl("gtxtFechaDoc1"), TextBox).Text.TrimEnd + "'
            ,@FechaSis2 date = '" + CType(Fila.FindControl("gtxtFechaDoc2"), TextBox).Text.TrimEnd + "'
            ,@FechaDoc1 date = '" + CType(Fila.FindControl("gtxtFechaDoc1"), TextBox).Text.TrimEnd + "'
            ,@FechaDoc2 date = '" + CType(Fila.FindControl("gtxtFechaDoc2"), TextBox).Text.TrimEnd + "'
            ,@FechaVen1 date = '" + CType(Fila.FindControl("gtxtFechaVen1"), TextBox).Text.TrimEnd + "'
            ,@FechaVen2 date = '" + CType(Fila.FindControl("gtxtFechaVen2"), TextBox).Text.TrimEnd + "'
            ,@CodUsuario varchar(50) = '" + Session("Usuario_Aut") + "'
            ,@Estado varchar(1) = '" + CType(Fila.FindControl("gdlEstado"), DropDownList).SelectedValue.TrimEnd + "'

        EXECUTE [dbo].[SP_FinPer] 
           @CU
          ,@ID
          ,@Descripcion
          ,@FechaSis1
          ,@FechaSis2
          ,@FechaDoc1
          ,@FechaDoc2
          ,@FechaVen1
          ,@FechaVen2
          ,@CodUsuario
          ,@Estado"
        Datos = Conf.EjecutaSql(Sql)

        If Integer.Parse(Datos.Tables(0).Rows(0).Item("Error")) = 0 Then
            lblMsg.Text = Datos.Tables(0).Rows(0).Item("MSG").ToString
            DivMSG.Attributes.Add("class", "alert alert-success")
        Else
            lblMsg.Text = Datos.Tables(0).Rows(0).Item("MSG").ToString
            DivMSG.Attributes.Add("class", "alert alert-danger")
            Exit Sub
        End If

        Session("Tabla").Rows(e.RowIndex).Item("Descripcion") = CType(Fila.FindControl("gtxtDescripcion"), TextBox).Text.TrimEnd
        Session("Tabla").Rows(e.RowIndex).Item("FechaDoc1") = CType(Fila.FindControl("gtxtFechaDoc1"), TextBox).Text.TrimEnd
        Session("Tabla").Rows(e.RowIndex).Item("FechaDoc2") = CType(Fila.FindControl("gtxtFechaDoc2"), TextBox).Text.TrimEnd
        Session("Tabla").Rows(e.RowIndex).Item("FechaVen1") = CType(Fila.FindControl("gtxtFechaVen1"), TextBox).Text.TrimEnd
        Session("Tabla").Rows(e.RowIndex).Item("FechaVen2") = CType(Fila.FindControl("gtxtFechaVen2"), TextBox).Text.TrimEnd
        Session("Tabla").Rows(e.RowIndex).Item("Estado") = CType(Fila.FindControl("gdlEstado"), DropDownList).SelectedValue.TrimEnd
        gvDetalle.EditIndex = -1
        gvDetalle.DataSource = Session("Tabla")
        gvDetalle.DataBind()
    End Sub

    Private Sub BarNuevo_ServerClick(sender As Object, e As EventArgs) Handles barNuevo.ServerClick
        PanelNuevo.Visible = True
        lblTituloT.Text = "Nuevo Periodo"
    End Sub

    Private Sub BtnCancelar_ServerClick(sender As Object, e As EventArgs) Handles btnCancelar.ServerClick
        PanelNuevo.Visible = False
    End Sub

    Private Sub BtnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "DECLARE @CU varchar(1) = 'C'
            ,@ID int = '0'
            ,@Descripcion varchar(50) = '" + txtDescripcion.Text + "'
            ,@FechaSis1 date = '" + txtFechaDoc1.Text + "'
            ,@FechaSis2 date = '" + txtFechaDoc2.Text + "'
            ,@FechaDoc1 date = '" + txtFechaDoc1.Text + "'
            ,@FechaDoc2 date = '" + txtFechaDoc2.Text + "'
            ,@FechaVen1 date = '" + txtFechaVen1.Text + "'
            ,@FechaVen2 date = '" + txtFechaVen2.Text + "'
            ,@CodUsuario varchar(50) = '" + Session("Usuario_Aut") + "'
            ,@Estado varchar(1) = '" + dlEstado.SelectedValue + "'

        EXECUTE [dbo].[SP_FinPer] 
           @CU
          ,@ID
          ,@Descripcion
          ,@FechaSis1
          ,@FechaSis2
          ,@FechaDoc1
          ,@FechaDoc2
          ,@FechaVen1
          ,@FechaVen2
          ,@CodUsuario
          ,@Estado"
        Datos = Conf.EjecutaSql(Sql)

        If Integer.Parse(Datos.Tables(0).Rows(0).Item("Error")) = 0 Then
            LlenarPeriodos()
            PanelNuevo.Visible = False
        Else
            lblMsg.Text = Datos.Tables(0).Rows(0).Item("MSG").ToString
            DivMSG.Attributes.Add("class", "alert alert-danger")
        End If
    End Sub

End Class