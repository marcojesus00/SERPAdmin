Imports System.Data.SqlClient

Public Class ParUsu
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut, Status, SuperUser As String
    Private TotalDias As Decimal = 0
    Private Datos, Datos1 As DataSet
    Private Tabla As DataTable = New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario_Aut") = "" Or Session("Destino") <> "ParCli.aspx" Then
            Response.Redirect("inicio.aspx")
        End If

        Usuario = Session("Usuario")
        Clave = Session("Clave")
        Bd = Session("Bd")
        Servidor = Session("Sevidor")
        Session.Timeout = Session("Tiempo")

        If Not IsPostBack Then
            Session.Add("Orden", "0")
            LlenarLista()
            AccesosMenu()
        End If
    End Sub

    Sub LlenarLista()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String = ""

        Sql = "SELECT 0 ID, '...' CodUsuario, '' Nombre, 'Limitada' Funcion, '' Cargo, 'Y' Activo, '' Pass UNION ALL
               SELECT ID, CodUsuario, Nombre, Funcion, Cargo, Activo, Pass FROM Usuario ORDER BY ID"
        Datos = Conf.EjecutaSql(Sql)
        Session.Add("GvEncargado", Datos.Tables(0))

        GvEncargado.DataSource = Session("GvEncargado")
        GvEncargado.DataBind()
    End Sub

    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub

    Sub AccesosMenu()
        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String = "SELECT * FROM MenuAcceso A WHERE A.CodUsuario = '" + Session("Usuario_Aut") + "'"
        Datos = conf.EjecutaSql(Sql)
        Tabla = Datos.Tables(0)

        Tabla.DefaultView.RowFilter = "CodMenu = 'ParCli'"
        If Tabla.DefaultView.Count > 0 Then
            menuClientes.Enabled = True
        Else
            menuClientes.ToolTip = "No Tiene Acceso"
            menuClientes.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'ParPag'"
        If Tabla.DefaultView.Count > 0 Then
            menuPagos.Enabled = True
        Else
            menuPagos.ToolTip = "No Tiene Acceso"
            menuPagos.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'ParVen'"
        If Tabla.DefaultView.Count > 0 Then
            menuVendedores.Enabled = True
        Else
            menuVendedores.ToolTip = "No Tiene Acceso"
            menuVendedores.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'ParAlm'"
        If Tabla.DefaultView.Count > 0 Then
            MenuAlmacen.Enabled = True
        Else
            MenuAlmacen.ToolTip = "No Tiene Acceso"
            MenuAlmacen.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'ParArt'"
        If Tabla.DefaultView.Count > 0 Then
            menuArticulos.Enabled = True
        Else
            menuArticulos.ToolTip = "No Tiene Acceso"
            menuArticulos.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'ParGArt'"
        If Tabla.DefaultView.Count > 0 Then
            menuGrupoArt.Enabled = True
        Else
            menuGrupoArt.ToolTip = "No Tiene Acceso"
            menuGrupoArt.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'ParPre'"
        If Tabla.DefaultView.Count > 0 Then
            menuPrecios.Enabled = True
        Else
            menuPrecios.ToolTip = "No Tiene Acceso"
            menuPrecios.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'ParImp'"
        If Tabla.DefaultView.Count > 0 Then
            menuImpuestos.Enabled = True
        Else
            menuImpuestos.ToolTip = "No Tiene Acceso"
            menuImpuestos.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'ParUsu'"
        If Tabla.DefaultView.Count > 0 Then
            menuUsuarios.Enabled = True
        Else
            menuUsuarios.ToolTip = "No Tiene Acceso"
            menuUsuarios.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'ParSer'"
        If Tabla.DefaultView.Count > 0 Then
            menuSeries.Enabled = True
        Else
            menuSeries.ToolTip = "No Tiene Acceso"
            menuSeries.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'ParGen'"
        If Tabla.DefaultView.Count > 0 Then
            menuGeneral.Enabled = True
        Else
            menuGeneral.ToolTip = "No Tiene Acceso"
            menuGeneral.Style.Value = "color:darkgrey;"
        End If
    End Sub

    Sub LLenarGVAccesos()
        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim sql As String = ""

        sql = " SELECT A.Menu, A.Pantalla, CASE WHEN B.CodMenu IS NULL THEN 0 ELSE 1 END Activo, A.CodMenu
	                FROM MenuPantalla A
	                LEFT JOIN MenuAcceso B ON A.CodMenu = B.CodMenu AND B.CodUsuario = '" + Session("CodUsuario") + "'	                
	                ORDER BY A.Menu, A.Pantalla "
        Datos = conf.EjecutaSql(sql)
        Session.Add("GVMenu", Datos.Tables(0))
        gvAccesos.DataSource = Session("GVMenu")
        gvAccesos.DataBind()
    End Sub

    Private Sub MenuClientes_Click(sender As Object, e As EventArgs) Handles menuClientes.Click
        Response.Redirect("ParCli.aspx")
    End Sub

    Private Sub MenuPagos_Click(sender As Object, e As EventArgs) Handles menuPagos.Click
        Response.Redirect("ParPag.aspx")
    End Sub

    Private Sub MenuVendedores_Click(sender As Object, e As EventArgs) Handles menuVendedores.Click
        Response.Redirect("ParVen.aspx")
    End Sub

    Private Sub MenuAlmacen_Click(sender As Object, e As EventArgs) Handles MenuAlmacen.Click
        Response.Redirect("ParAlm.aspx")
    End Sub

    Private Sub MenuArticulos_Click(sender As Object, e As EventArgs) Handles menuArticulos.Click
        Response.Redirect("ParArt.aspx")
    End Sub

    Private Sub MenuGrupoArt_Click(sender As Object, e As EventArgs) Handles menuGrupoArt.Click
        Response.Redirect("ParGArt.aspx")
    End Sub

    Private Sub MenuPrecios_Click(sender As Object, e As EventArgs) Handles menuPrecios.Click
        Response.Redirect("ParPre.aspx")
    End Sub

    Private Sub MenuImpuestos_Click(sender As Object, e As EventArgs) Handles menuImpuestos.Click
        Response.Redirect("ParImp.aspx")
    End Sub

    Private Sub MenuUsuarios_Click(sender As Object, e As EventArgs) Handles menuUsuarios.Click
    End Sub

    Private Sub MenuSeries_Click(sender As Object, e As EventArgs) Handles menuSeries.Click
        Response.Redirect("ParSer.aspx")
    End Sub

    Private Sub menuGeneral_Click(sender As Object, e As EventArgs) Handles menuGeneral.Click
        Response.Redirect("ParGen.aspx")
    End Sub

    Private Sub GvEncargados_Sorting(sender As Object, e As GridViewSortEventArgs) Handles GvEncargado.Sorting
        Dim Orden As String

        If Session("Orden") = "0" Then
            Orden = "Asc"
            Session.Add("Orden", "1")
        Else
            Orden = "Desc"
            Session.Add("Orden", "0")
        End If

        Session("GvEncargado").DefaultView.Sort = e.SortExpression.ToString + " " + Orden
        GvEncargado.DataSource = Session("GvEncargado")
        GvEncargado.DataBind()
    End Sub

    Private Sub GvEncargado_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles GvEncargado.RowEditing
        GvEncargado.EditIndex = e.NewEditIndex
        GvEncargado.DataSource = Session("GvEncargado")
        GvEncargado.DataBind()
    End Sub

    Private Sub GvEncargado_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GvEncargado.RowUpdating
        Dim Fila = GvEncargado.Rows(e.RowIndex)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String = ""

        If CType(Fila.FindControl("ID"), Label).Text.TrimEnd = "0" Then
            Sql = "INSERT INTO Usuario (CodUsuario, Nombre, Funcion, Cargo, Activo, Pass, FechaSys) VALUES 
                   ('" + CType(Fila.FindControl("Usuario"), TextBox).Text.TrimEnd + "', 
                    '" + CType(Fila.FindControl("Nombre"), TextBox).Text.TrimEnd + "', 
                    '" + CType(Fila.FindControl("Funcion"), DropDownList).SelectedValue.TrimEnd + "', 
                    '" + CType(Fila.FindControl("Cargo"), TextBox).Text.TrimEnd + "', 
                    '" + CType(Fila.FindControl("Activo"), DropDownList).SelectedValue.TrimEnd + "',                     
                    ENCRYPTBYPASSPHRASE('serp''" + CType(Fila.FindControl("Pass"), TextBox).Text.TrimEnd + "'), 
                    GETDATE()) "
            Conf.EjecutaSql(Sql)

            LlenarLista()
        Else
            Sql = "UPDATE Usuario SET  Nombre = '" + CType(Fila.FindControl("Nombre"), TextBox).Text.TrimEnd + "',
                                        Pass = ENCRYPTBYPASSPHRASE('serp''" + CType(Fila.FindControl("Pass"), TextBox).Text.TrimEnd + "'),
                                        Funcion = '" + CType(Fila.FindControl("Funcion"), DropDownList).SelectedValue.TrimEnd + "',
                                        Cargo = '" + CType(Fila.FindControl("Cargo"), TextBox).Text.TrimEnd + "',
                                        Activo = '" + CType(Fila.FindControl("Activo"), DropDownList).SelectedValue.TrimEnd + "'
                   WHERE ID = '" + CType(Fila.FindControl("ID"), Label).Text.TrimEnd + "'"
            Datos = Conf.EjecutaSql(Sql)
        End If
        GvEncargado.EditIndex = -1
        LlenarLista()
    End Sub

    Private Sub GvEncargado_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles GvEncargado.RowCancelingEdit
        GvEncargado.EditIndex = -1
        GvEncargado.DataSource = Session("GvEncargado")
        GvEncargado.DataBind()
    End Sub

    Private Sub GvEncargado_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GvEncargado.RowDataBound
        If ((e.Row.RowType = DataControlRowType.DataRow) And (e.Row.RowType <> DataControlRowType.EmptyDataRow)) Then
            If e.Row.RowIndex = 0 Then
                e.Row.Cells(2).ControlStyle.ForeColor = System.Drawing.Color.Transparent
                e.Row.Cells(3).ControlStyle.ForeColor = System.Drawing.Color.Blue
                e.Row.Cells(4).ControlStyle.ForeColor = System.Drawing.Color.Transparent
            End If
        End If
    End Sub

    Private Sub GvEncargado_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles GvEncargado.SelectedIndexChanging
        Dim Fila As Integer = Convert.ToInt32(e.NewSelectedIndex)
        Session.Add("CodUsuario", Session("GvEncargado").Rows(Fila).Item("CodUsuario").ToString)

        If e.NewSelectedIndex <> 0 Then
            LLenarGVAccesos()
            trUsuarios.Visible = False
            trAccesos.Visible = True
        End If
    End Sub

    Private Sub gvAccesos_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvAccesos.RowEditing
        gvAccesos.EditIndex = e.NewEditIndex
        gvAccesos.DataSource = Session("GVMenu")
        gvAccesos.DataBind()
    End Sub

    Private Sub gvAccesos_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvAccesos.RowCancelingEdit
        gvAccesos.EditIndex = -1
        gvAccesos.DataSource = Session("GVMenu")
        gvAccesos.DataBind()
    End Sub

    Private Sub gvAccesos_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gvAccesos.RowUpdating
        Dim Fila = gvAccesos.Rows(e.RowIndex)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String = ""

        If CType(Fila.FindControl("gvActivo"), CheckBox).Checked Then
            Sql = "INSERT INTO MenuAcceso (CodUsuario, CodMenu) VALUES ('" + Session("CodUsuario") + "', '" + Session("GVMenu").Rows(e.RowIndex).Item("CodMenu") + "') "
            Conf.EjecutaSql(Sql)
            Session("GVMenu").Rows(e.RowIndex).Item("Activo") = "1"
        Else
            Sql = "DELETE MenuAcceso WHERE CodUsuario = '" + Session("CodUsuario") + "' AND CodMenu = '" + Session("GVMenu").Rows(e.RowIndex).Item("CodMenu") + "' "
            Conf.EjecutaSql(Sql)
            Session("GVMenu").Rows(e.RowIndex).Item("Activo") = "0"
        End If
        gvAccesos.EditIndex = -1
        gvAccesos.DataSource = Session("GVMenu")
        gvAccesos.DataBind()
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        trUsuarios.Visible = True
        trAccesos.Visible = False
    End Sub

End Class