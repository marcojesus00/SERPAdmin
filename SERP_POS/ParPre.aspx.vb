Imports System.Data.SqlClient

Public Class ParPre
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut, Status, SuperUser As String
    Private TotalDias As Decimal = 0
    Private Datos, Datos1 As DataSet
    Private Tabla As DataTable = New DataTable
    Private Conector As SqlConnection
    Private Adaptador As SqlDataAdapter

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

        Sql = "SELECT 0 Codigo, '...' Nombre
                   UNION ALL
               SELECT ListCod Codigo, ListNom Nombre
                FROM ListEnc"
        Datos = Conf.EjecutaSql(Sql)
        Session.Add("GvLista", Datos.Tables(0))

        GvLista.DataSource = Session("GvLista")
        GvLista.DataBind()
    End Sub

    Sub LlenarPrecios()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String = ""

        Sql = "SELECT A.ListCod Lista, A.CodArticulo Codigo, B.NomArticulo Descripcion, C.Descripcion Grupo, B.Costo, Precio
                FROM ListDet A
                INNER JOIN Articulo B ON A.CodArticulo = B.CodArticulo
                INNER JOIN GrupoArtEnc C ON B.GrupoCod = C.IdGrupCod
                WHERE ListCod = '" + Session("ListCod") + "'
                AND A.CodArticulo + B.NomArticulo + C.Descripcion LIKE '%" + txtBuscarArticulo.Text + "%'"
        Datos = Conf.EjecutaSql(Sql)

        Session.Add("gvPrecios", Datos.Tables(0))
        gvPrecios.DataSource = Session("gvPrecios")
        gvPrecios.DataBind()
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

    Protected Sub txtBuscarArticulo_TextChanged(sender As Object, e As EventArgs)
        LlenarPrecios()
    End Sub

    Private Sub MenuClientes_Click(sender As Object, e As EventArgs) Handles menuClientes.Click
        Response.Redirect("ParCli.aspx")
    End Sub

    Private Sub MenuPagos_Click(sender As Object, e As EventArgs) Handles menuPagos.Click
        Response.Redirect("FarPag.aspx")
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
    End Sub

    Private Sub MenuImpuestos_Click(sender As Object, e As EventArgs) Handles menuImpuestos.Click
        Response.Redirect("ParImp.aspx")
    End Sub

    Private Sub MenuUsuarios_Click(sender As Object, e As EventArgs) Handles menuUsuarios.Click
        Response.Redirect("ParUsu.aspx")
    End Sub

    Private Sub MenuSeries_Click(sender As Object, e As EventArgs) Handles menuSeries.Click
        Response.Redirect("ParSer.aspx")
    End Sub

    Private Sub menuGeneral_Click(sender As Object, e As EventArgs) Handles menuGeneral.Click
        Response.Redirect("ParGen.aspx")
    End Sub

    Private Sub gvPrecios_Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvPrecios.Sorting
        Dim Orden As String

        If Session("Orden") = "0" Then
            Orden = "Asc"
            Session.Add("Orden", "1")
        Else
            Orden = "Desc"
            Session.Add("Orden", "0")
        End If

        Session("gvPrecios").DefaultView.Sort = e.SortExpression.ToString + " " + Orden
        gvPrecios.DataSource = Session("gvPrecios")
        gvPrecios.DataBind()
    End Sub

    Private Sub gvPrecios_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles gvPrecios.RowEditing
        gvPrecios.EditIndex = e.NewEditIndex
        gvPrecios.DataSource = Session("gvPrecios")
        gvPrecios.DataBind()
    End Sub

    Private Sub gvPrecios_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gvPrecios.RowUpdating
        Dim Fila = gvPrecios.Rows(e.RowIndex)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String = ""

        Sql = "UPDATE ListDet SET Precio = '" + FormatNumber(CType(Fila.FindControl("gvtxtPrecio"), TextBox).Text.TrimEnd) + "'
                WHERE ListCod = '" + Session("ListCod") + "'
                AND CodArticulo = '" + CType(Fila.FindControl("gvlblProducto"), Label).Text.TrimEnd + "'"
        Datos = Conf.EjecutaSql(Sql)

        Session("gvPrecios").Rows(e.RowIndex).Item("Precio") = FormatNumber(Decimal.Parse(CType(Fila.FindControl("gvtxtPrecio"), TextBox).Text.TrimEnd))

        gvPrecios.EditIndex = -1
        gvPrecios.DataSource = Session("gvPrecios")
        gvPrecios.DataBind()
    End Sub

    Private Sub gvPrecios_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles gvPrecios.RowCancelingEdit
        gvPrecios.EditIndex = -1
        gvPrecios.DataSource = Session("gvPrecios")
        gvPrecios.DataBind()
    End Sub

    Private Sub GvLista_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles GvLista.RowEditing
        GvLista.EditIndex = e.NewEditIndex
        GvLista.DataSource = Session("GvLista")
        GvLista.DataBind()
    End Sub

    Private Sub GvLista_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GvLista.RowUpdating
        Dim Fila = GvLista.Rows(e.RowIndex)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String = ""

        If CType(Fila.FindControl("gvListCod"), Label).Text.TrimEnd = "0" Then
            Sql = "INSERT INTO ListEnc (ListNom) VALUES ('" + CType(Fila.FindControl("gvListNom"), TextBox).Text.TrimEnd + "') 
                   
                    DECLARE @LISTCOD INT
                    SELECT @LISTCOD = MAX(ListCod) FROM ListEnc

                    INSERT INTO ListDet
                    SELECT @LISTCOD, A.CodArticulo, 0
                    FROM Articulo A"
            Conf.EjecutaSql(Sql)

            LlenarLista()
        Else

            Sql = "UPDATE ListEnc SET ListNom = '" + CType(Fila.FindControl("gvListNom"), TextBox).Text.TrimEnd + "'
                WHERE ListCod = '" + CType(Fila.FindControl("gvListCod"), Label).Text.TrimEnd + "'"

            Datos = Conf.EjecutaSql(Sql)

            Session("GvLista").Rows(e.RowIndex).Item("Nombre") = CType(Fila.FindControl("gvListNom"), TextBox).Text.TrimEnd
        End If
        GvLista.EditIndex = -1
        GvLista.DataSource = Session("GvLista")
        GvLista.DataBind()
    End Sub

    Private Sub GvLista_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles GvLista.RowCancelingEdit
        GvLista.EditIndex = -1
        GvLista.DataSource = Session("GvLista")
        GvLista.DataBind()
    End Sub

    Private Sub GvLista_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles GvLista.SelectedIndexChanging
        Dim Fila As Integer = Convert.ToInt32(e.NewSelectedIndex)
        Session.Add("ListCod", Session("GvLista").Rows(Fila).Item("Codigo").ToString)

        If e.NewSelectedIndex <> 0 Then
            LlenarPrecios()
            trLista.Visible = False
            trPrecios.Visible = True
        End If
    End Sub

    Private Sub GvLista_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GvLista.RowDataBound
        If ((e.Row.RowType = DataControlRowType.DataRow) And (e.Row.RowType <> DataControlRowType.EmptyDataRow)) Then
            If e.Row.RowIndex = 0 Then
                e.Row.Cells(2).ControlStyle.ForeColor = System.Drawing.Color.Transparent
                e.Row.Cells(3).ControlStyle.ForeColor = System.Drawing.Color.Blue
            End If
        End If
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        trLista.Visible = True
        trPrecios.Visible = False
    End Sub

End Class