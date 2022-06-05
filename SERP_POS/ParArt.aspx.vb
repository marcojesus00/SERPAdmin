Imports System.Data.SqlClient

Public Class ParArt
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut, Status, SuperUser As String
    Private Datos As DataSet
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
            LlenarNumeracion()
            LlenarPanelClientes()
            AccesosMenu()
        End If

        If dlNumeracion.SelectedItem.Text = "Manual" Then
            txtNumeracion.Enabled = True
        Else
            txtNumeracion.Enabled = False
            txtNumeracion.Text = Session("Numeracion").Rows(dlNumeracion.SelectedIndex).Item("NumSig")
        End If
    End Sub

    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub

    Sub LlenarNumeracion()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT A.ID IdCorre, A.Descripcion, A.Prefijo + RIGHT('000000000000'+ CONVERT(VARCHAR,A.NumSig), A.Digitos) NumSig
                FROM CorrelaDet A 
				INNER JOIN CorrelaEnc B ON A.IdCorre = B.IdCorre
                WHERE A.Activo = 'Y'
                AND B.ObjectCode = '19'"
        Datos = Conf.EjecutaSql(Sql)
        Session.Add("Numeracion", Datos.Tables(0))

        dlNumeracion.DataSource = Session("Numeracion")
        dlNumeracion.DataTextField = "Descripcion"
        dlNumeracion.DataValueField = "IdCorre"
        dlNumeracion.DataBind()

        dlNumeracion.Items.Add("Manual")
        dlNumeracion.SelectedIndex = 0
        txtNumeracion.Text = Session("Numeracion").Rows(0).Item("NumSig")

    End Sub

    Sub LlenarPrecios()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String
        Sql = "SELECT a.ListCod, a.ListNom + ' '+CONVERT(VARCHAR,b.Precio)+' ListNom'
                FROM ListEnc a
                INNER JOIN ListDet b on a.ListCod = b.ListCod
                WHERE B.CodArticulo = '" + txtNumeracion.Text + "'"
        Datos = Conf.EjecutaSql(Sql)

        dlPrecios.DataSource = Datos.Tables(0)
        dlPrecios.DataTextField = "ListNom"
        dlPrecios.DataValueField = "ListCod"
        dlPrecios.DataBind()
    End Sub

    Sub LlenarPanelClientes()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        ' Grupos de Articulos
        Sql = "SELECT IdGrupCod, Prefijo    
                FROM GrupoArtEnc"
        Datos = Conf.EjecutaSql(Sql)

        dlGrupo.DataSource = Datos.Tables(0)
        dlGrupo.DataTextField = "Prefijo"
        dlGrupo.DataValueField = "IdGrupCod"
        dlGrupo.DataBind()
        ''''''''''''''''''''''''''''''''''''''

        ' Lista de Precios
        LlenarPrecios()
        ''''''''''''''''''''''''''''''''''''''
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

    Protected Sub TxtNumeracion_TextChanged(sender As Object, e As EventArgs)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String
        Session.Add("NumSig", txtNumeracion.Text)

        Sql = "SELECT *
                FROM Articulo
	            WHERE CodArticulo = '" + Session("NumSig") + "'"
        Datos = Conf.EjecutaSql(Sql)

        If Datos.Tables(0).Rows.Count > 0 Then
            txtNumeracion.Enabled = False
            txtNombre.Value = Datos.Tables(0).Rows(0).Item("NomArticulo")
            txtDescrip.Value = Datos.Tables(0).Rows(0).Item("NomExt")
            txtBarras.Value = Datos.Tables(0).Rows(0).Item("CodigoBarra")
            dlTipo.SelectedValue = Datos.Tables(0).Rows(0).Item("Tipo")
            dlGrupo.SelectedValue = Datos.Tables(0).Rows(0).Item("GrupoCod")
            chkImpuesto.Checked = Datos.Tables(0).Rows(0).Item("Impuesto")
            dlNumeracion.SelectedValue = Datos.Tables(0).Rows(0).Item("SerieCod")
            txtComentario.Text = Datos.Tables(0).Rows(0).Item("Comentario")
            dlEsatdo.SelectedValue = Datos.Tables(0).Rows(0).Item("Activo")
            chkImpuesto.Checked = Integer.Parse(Datos.Tables(0).Rows(0).Item("Impuesto").ToString)
            chkFranciones.Checked = Integer.Parse(Datos.Tables(0).Rows(0).Item("Fracciones").ToString)
            chkCompra.Checked = Integer.Parse(Datos.Tables(0).Rows(0).Item("Compra").ToString)
            chkVenta.Checked = Integer.Parse(Datos.Tables(0).Rows(0).Item("Venta").ToString)
            btnGuardar.Text = "Actualizar"
            dlPrecios.Items.Clear()
            dlPrecios.DataBind()

            dlPrecios.Items.Add("Costo " + FormatNumber(Datos.Tables(0).Rows(0).Item("Costo").ToString))
            dlPrecios.Items.Add("Compra " + FormatNumber(Datos.Tables(0).Rows(0).Item("PrecioCompra").ToString))

            Sql = "SELECT A.ListCod, A.ListNom + ' '+ CONVERT(VARCHAR(50), CAST(B.Precio AS MONEY),1) ListNom
                    FROM ListEnc A
                    INNER JOIN ListDet B ON A.ListCod = B.ListCod
                    WHERE B.CodArticulo = '" + Session("NumSig") + "'"
            Datos = Conf.EjecutaSql(Sql)

            For i As Integer = 0 To Datos.Tables(0).Rows.Count - 1
                dlPrecios.Items.Add(Datos.Tables(0).Rows(i).Item("ListNom"))
            Next

        Else
            btnGuardar.Text = "   Crear   "
            LlenarPrecios()
        End If
    End Sub

    Protected Sub TxtBuscarCliente_TextChanged(sender As Object, e As EventArgs)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT TOP 100 CodArticulo Codigo, NomArticulo Nombre, NomExt [Descripcion], CodigoBarra, Costo Saldo
                FROM Articulo
	            WHERE CodArticulo + NomArticulo + NomExt LIKE '%" + txtBuscarCliente.Text + "%'"
        Datos = Conf.EjecutaSql(Sql)

        gvClientes.DataSource = Datos.Tables(0)
        gvClientes.DataBind()
    End Sub

    Protected Sub DlNumeracion_SelectedIndexChanged(sender As Object, e As EventArgs)
        If dlNumeracion.SelectedItem.Text = "Manual" Then
            txtNumeracion.Text = ""
        Else
            Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String = ""

            Sql = "SELECT A.Prefijo + RIGHT('000000000000'+ CONVERT(VARCHAR,A.NumSig), A.Digitos) NumSig
                FROM CorrelaDet A 
                WHERE A.Activo = 'Y'
                AND A.Descripcion = '" + dlNumeracion.SelectedItem.Text + "'"
            Datos = Conf.EjecutaSql(Sql)

            txtNumeracion.Text = Datos.Tables(0).Rows(0).Item("NumSig")
        End If
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
        Response.Redirect("ParUsu.aspx")
    End Sub

    Private Sub MenuSeries_Click(sender As Object, e As EventArgs) Handles menuSeries.Click
        Response.Redirect("ParSer.aspx")
    End Sub

    Private Sub menuGeneral_Click(sender As Object, e As EventArgs) Handles menuGeneral.Click
        Response.Redirect("ParGen.aspx")
    End Sub

    Private Sub GvClientes_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvClientes.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(gvClientes, "Select$" & e.Row.RowIndex)
            e.Row.Attributes("style") = "cursor:pointer"
        End If
    End Sub

    Private Sub GvClientes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvClientes.RowCommand
        Dim Fila As Integer = Convert.ToInt32(e.CommandArgument)
        txtNumeracion.Text = gvClientes.Rows(Fila).Cells(1).Text
        trClientes.Visible = False
        trAgregarClie.Visible = True
        btnNuevo.Visible = True
        btnGuardar.Visible = True
        imgClientes.Attributes.Add("class", "fas fa-search")
        TxtNumeracion_TextChanged(sender, e)
    End Sub

    Private Sub BtnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        If imgClientes.Attributes.Item("class") = "fas fa-search" Then
            imgClientes.Attributes.Add("class", "fas fa-search-minus")
            trClientes.Visible = True
            trAgregarClie.Visible = False
            btnNuevo.Visible = False
            btnGuardar.Visible = False
            TxtBuscarCliente_TextChanged(sender, e)
        Else
            imgClientes.Attributes.Add("class", "fas fa-search")
            trClientes.Visible = False
            trAgregarClie.Visible = True
            btnNuevo.Visible = True
            btnGuardar.Visible = True
        End If
    End Sub

    Private Sub BtnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Response.Redirect("ParArt.aspx")
    End Sub

    Private Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String
        Dim CU As String = "C"
        Dim Impuesto As String = "0"
        Dim Fraccion As String = "0"
        Dim Venta As String = "0"
        Dim Compra As String = "0"

        If btnGuardar.Text = "Actualizar" Then
            CU = "U"
        End If

        If chkImpuesto.Checked Then
            Impuesto = "1"
        End If
        If chkFranciones.Checked Then
            Fraccion = "1"
        End If
        If chkVenta.Checked Then
            Venta = "1"
        End If
        If chkCompra.Checked Then
            Compra = "1"
        End If

        Sql = "EXECUTE SP_Articulo
               '" + CU + "'
              ,'" + Session("NumSig") + "'
              ,'" + txtNombre.Value + "'
              ,'" + txtDescrip.Value + "'
              ,'" + dlGrupo.SelectedValue + "'
              ,'" + dlNumeracion.SelectedValue + "'
              ,'" + txtBarras.Value.ToString + "'
              ,'" + txtComentario.Text + "'  
              ,'" + Session("Usuario_Aut") + "'
              ,'" + dlEsatdo.SelectedValue + "'  
              ,'" + dlTipo.SelectedValue + "' 
              ,'" + Impuesto + "'
              ,'" + Fraccion + "'
              ,'" + Venta + "'
              ,'" + Compra + "'"

        Datos = Conf.EjecutaSql(Sql)

        If Integer.Parse(Datos.Tables(0).Rows(0).Item("Error")) = 0 Then
            lblMsg.Text = Datos.Tables(0).Rows(0).Item("MSG").ToString
            Response.Redirect("ParArt.aspx")
            lblMsg.ControlStyle.CssClass = "alert alert-success"
        Else
            lblMsg.Text = "Error: " + Datos.Tables(0).Rows(0).Item("Error").ToString + Datos.Tables(0).Rows(0).Item("MSG").ToString
            lblMsg.ControlStyle.CssClass = "alert alert-danger"
        End If
    End Sub

End Class