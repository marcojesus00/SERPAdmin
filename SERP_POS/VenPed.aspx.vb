Public Class VenPed
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut, Status, SuperUser, Funcion As String
    Private Datos As DataSet
    Private Tabla As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario_Aut") = "" Or Session("Destino") = "VenPed" Then
            Response.Redirect("inicio.aspx")
        End If

        Usuario = Session("Usuario")
        Clave = Session("Clave")
        Bd = Session("Bd")
        Servidor = Session("Sevidor")
        Funcion = Session("Funcion")
        Session.Timeout = Session("Tiempo")

        lblMsg.Text = ""
        lblMsg.ControlStyle.CssClass = ""

        If Funcion = "Administrador" Then
            gvProductos.Columns(5).ItemStyle.ForeColor = System.Drawing.Color.Black
        End If

        If Not IsPostBack Then
            txtFecha.Value = DateTime.Now.ToString("yyyy-MM-dd")
            txtVencimiento.Value = DateTime.Now.ToString("yyyy-MM-dd")
            txtF1Bus.Text = DateTime.Now.ToString("yyyy-MM-dd")
            txtF2Bus.Text = DateTime.Now.ToString("yyyy-MM-dd")
            txtF1Copiar.Text = DateTime.Now.ToString("yyyy-MM-dd")
            txtF2Copiar.Text = DateTime.Now.ToString("yyyy-MM-dd")
            LlenarVendedor()
            LlenarAlmacen()
            LlenarNumeracion()
            LlenarImpuestos()

            Tabla = New DataTable
            Tabla.Columns.Add("Codigo")
            Tabla.Columns.Add("Producto")
            Tabla.Columns.Add("Cantidad")
            Tabla.Columns.Add("Precio")
            Tabla.Columns.Add("PrecioConDes")
            Tabla.Columns.Add("Costo")
            Tabla.Columns.Add("Descuento")
            Tabla.Columns.Add("Impuesto")
            Tabla.Columns.Add("CodImpuesto")
            Tabla.Columns.Add("Total")
            Tabla.Columns.Add("Fracciones")

            Session.Add("Tabla", Tabla)

            If dlNumeracion.SelectedValue = "Manual" Then
                txtNumeracion.Enabled = True
            Else
                txtNumeracion.Enabled = False
                txtNumeracion.Text = Session("Numeracion").Rows(dlNumeracion.SelectedIndex).Item("NumSig")
            End If
        End If
    End Sub

    Sub LlenarVendedor()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT CONVERT(VARCHAR,A.EncarCod) Codigo, EncarNom Vendedor
	            FROM ENCARGADO A	            
	            WHERE Activo = 'Y'
                ORDER BY A.EncarCod"
        Datos = Conf.EjecutaSql(Sql)

        dlVendedor.DataSource = Datos.Tables(0)
        dlVendedor.DataTextField = "Vendedor"
        dlVendedor.DataValueField = "Codigo"
        dlVendedor.DataBind()
    End Sub

    Sub LlenarAlmacen()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT AlmacenCod, Descripcion
                 FROM Almacen
                 WHERE Activo = 'Y'
                 ORDER BY ID"
        Datos = Conf.EjecutaSql(Sql)

        dlAlmacen.DataSource = Datos.Tables(0)
        dlAlmacen.DataTextField = "Descripcion"
        dlAlmacen.DataValueField = "AlmacenCod"
        dlAlmacen.DataBind()
    End Sub

    Sub LlenarImpuestos()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT Descripcion, Porcentaje, CodImpuesto
	            FROM Impuestos"
        Datos = Conf.EjecutaSql(Sql)
        Session.Add("dtImpuesto", Datos.Tables(0))

        dlImpuesto.DataSource = Session("dtImpuesto")
        dlImpuesto.DataTextField = "Descripcion"
        dlImpuesto.DataValueField = "Porcentaje"
        dlImpuesto.DataBind()
    End Sub

    Sub LlenarNumeracion()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT A.ID IdCorre, A.Descripcion, A.NumSig, Info
                FROM CorrelaDet A 
				INNER JOIN CorrelaEnc B ON A.IdCorre = B.IdCorre
                WHERE A.Activo = 'Y'
                AND B.ObjectCode = '4'"
        Datos = Conf.EjecutaSql(Sql)
        Session.Add("Numeracion", Datos.Tables(0))

        dlNumeracion.DataSource = Session("Numeracion")
        dlNumeracion.DataTextField = "Descripcion"
        dlNumeracion.DataValueField = "IdCorre"
        dlNumeracion.DataBind()

        dlNumeracion.Items.Add("Manual")
        dlNumeracion.SelectedIndex = 0
        txtNumeracion.Text = Session("Numeracion").Rows(dlNumeracion.SelectedIndex).Item("NumSig")
    End Sub

    Sub BuscarProductos()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT TOP 200 A.CodArticulo Codigo, A.NomArticulo Descripcion, C.Precio, B.Stock, A.Costo, A.Fracciones
	            FROM Articulo A
	            INNER JOIN ArticuloExis B ON A.CodArticulo = B.CodArticulo
				INNER JOIN ListDet C ON A.CodArticulo = C.CodArticulo
				INNER JOIN ListEnc D ON C.ListCod = D.ListCod
	            WHERE A.Activo = 'Y'
                AND A.Venta = '1'
                AND B.AlmacenCod = '" + dlAlmacen.SelectedValue + "'
				AND D.ListNom = '" + lblLista.InnerText + "'
                AND A.CodArticulo + A.NomArticulo + A.CodigoBarra LIKE '%" + txtProductos.Text + "%'"
        Datos = Conf.EjecutaSql(Sql)

        gvProductos.DataSource = Datos.Tables(0)
        gvProductos.DataBind()
    End Sub

    Sub LlenarCopia()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String
        Sql = " SELECT A.CodArticulo Codigo, A.Descripcion Producto, A.Cantidad, CONVERT(VARCHAR(50), CAST(A.Precio AS MONEY ),1) Precio, CONVERT(VARCHAR(50), CAST(A.PrecioConDes AS MONEY ),1) PrecioConDes, A.Costo, A.Descuento, B.Descripcion Impuesto, A.Impuesto CodImpuesto, A.TotalLinea Total, C.Fracciones
	            FROM VenPedDet A
                INNER JOIN Impuestos B ON A.TipoImpuesto = B.CodImpuesto
                INNER JOIN Articulo C ON A.CodArticulo = C.CodArticulo
                WHERE A.NumDoc = '" + Session("CopiaNumDoc") + "'"
        Datos = Conf.EjecutaSql(Sql)

        Session("Tabla") = Datos.Tables(0)
        gvDetalle.DataSource = Session("Tabla")
        gvDetalle.DataBind()
        Totales()
    End Sub

    Sub LlenarBuscar()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        '--------------- Llenado de encabezados ---
        Sql = "SELECT D.CodigoM, D.NombreM, d.RNT, D.FechaDoc, D.FechaVen, D.EncarCod, CASE D.Estado WHEN 'O' THEN '(Abierto)' WHEN 'C' THEN '(Cerrado)' ELSE '(Cancelado)' END Estado, D.SerieCod, D.Comentario, E.Descripcion Pago
	            FROM VenPedEnc D
                INNER JOIN FormasPago E ON D.CodPago = E.CodPago 
                WHERE D.NumDoc = '" + Session("BuscarNumDoc") + "'"
        Datos = Conf.EjecutaSql(Sql)

        dlNumeracion.SelectedValue = Datos.Tables(0).Rows(0).Item("SerieCod")
        txtNumeracion.Text = Session("BuscarNumDoc")
        txtCodigo.Text = Datos.Tables(0).Rows(0).Item("CodigoM")
        txtNombre.Value = Datos.Tables(0).Rows(0).Item("NombreM")
        txtRTN.Value = Datos.Tables(0).Rows(0).Item("RNT")
        txtFecha.Value = Date.Parse(Datos.Tables(0).Rows(0).Item("FechaDoc").ToString).ToString("yyyy-MM-dd")
        txtVencimiento.Value = Date.Parse(Datos.Tables(0).Rows(0).Item("FechaVen").ToString).ToString("yyyy-MM-dd")
        dlVendedor.SelectedValue = Datos.Tables(0).Rows(0).Item("EncarCod")
        lblEstado.Text = Datos.Tables(0).Rows(0).Item("Estado")
        txtComentarios.Text = Datos.Tables(0).Rows(0).Item("Comentario")
        lblFormaPago.InnerText = Datos.Tables(0).Rows(0).Item("Pago")
        lblLista.InnerText = ""
        '--------------------------------------------

        '------------- Inactivar ---------------------
        txtCodigo.Enabled = False
        txtNombre.Disabled = True
        btnBuscarCliente.Enabled = False
        txtRTN.Disabled = True
        txtFecha.Disabled = True
        txtVencimiento.Disabled = True
        dlVendedor.Enabled = False
        dlNumeracion.Enabled = False
        txtNumeracion.Enabled = False
        txtBuscar.Enabled = False
        btnBuscarProductos2.Enabled = False
        btnCrear.Visible = False
        txtComentarios.Enabled = False

        If Datos.Tables(0).Rows(0).Item("Estado") = "(Cancelado)" Then
            barReimpresion.Visible = False
        Else
            barReimpresion.Visible = True
        End If
        BarNuevo.Visible = True
        barCopiar.Visible = False
        If Datos.Tables(0).Rows(0).Item("Estado") = "(Abierto)" Then
            BarCancelar.Visible = True
        Else
            BarCancelar.Visible = False
        End If

        gvDetalle.Columns.Item(0).Visible = False
        gvDetalle.Columns.Item(1).Visible = False
        '--------------------------------------------

        '--------------- Llenado de detalle ---------
        Sql = "SELECT A.CodArticulo Codigo, A.Descripcion Producto, A.Cantidad, CONVERT(VARCHAR(50), CAST(A.Precio AS MONEY ),1) Precio, CONVERT(VARCHAR(50), CAST(A.PrecioConDes AS MONEY ),1) PrecioConDes, A.Costo, A.Descuento, B.Descripcion Impuesto, A.Impuesto CodImpuesto, A.TotalLinea Total, C.Fracciones
	            FROM VenPedDet A
                INNER JOIN Impuestos B ON A.TipoImpuesto = B.CodImpuesto
                INNER JOIN Articulo C ON A.CodArticulo = C.CodArticulo                
                WHERE A.NumDoc = '" + Session("BuscarNumDoc") + "'"
        Datos = Conf.EjecutaSql(Sql)

        Session.Add("Tabla", Datos.Tables(0))
        gvDetalle.DataSource = Session("Tabla")
        gvDetalle.DataBind()
        Totales()
        '--------------------------------------------
    End Sub

    Sub Totales()
        Dim Total As Decimal = 0
        Dim TotalConDes As Decimal = 0
        Dim TotalImp As Decimal = 0

        For i As Integer = 0 To Session("Tabla").Rows.Count - 1
            Total += Decimal.Parse(Session("Tabla").Rows(i).Item("Precio")) * Decimal.Parse(Session("Tabla").Rows(i).Item("Cantidad"))
            TotalConDes += Decimal.Parse(Session("Tabla").Rows(i).Item("Total"))
            Session("dtImpuesto").DefaultView.RowFilter = "Descripcion ='" + Session("Tabla").Rows(i).Item("Impuesto") + "'"
            TotalImp += Decimal.Parse(Session("Tabla").Rows(i).Item("Total")) * (Decimal.Parse(Session("dtImpuesto").DefaultView.Item(0).Item("Porcentaje")) / 100)
        Next
        lblSubTotal.InnerText = FormatNumber(Decimal.Round(Total, 2))
        lblDescuento.InnerText = FormatNumber(Decimal.Round(Total - TotalConDes, 2))
        lblImpuesto.InnerText = FormatNumber(TotalImp)
        lblTotal.InnerText = FormatNumber(TotalConDes + TotalImp)

        If Decimal.Parse(lblTotal.InnerText.ToString) = 0 Then
            dlAlmacen.Enabled = True
        Else
            dlAlmacen.Enabled = False
        End If
    End Sub

    Protected Sub TxtBuscarBus_TextChanged()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String
        Sql = " SELECT A.NumDoc Numero, CONVERT(VARCHAR,A.FechaSis,103) [Fecha Sis], CONVERT(VARCHAR,A.FechaDoc,103) [Fecha Doc], A.CodigoM Codigo, A.NombreM Nombre,CONVERT(VARCHAR(50), CAST(A.TotalDoc AS MONEY ),1) Total, CASE A.Estado WHEN 'C' THEN 'Cerrado' WHEN 'O' THEN 'Abierto' ELSE 'Cancelado' END Estado
	            FROM VenPedEnc A
                WHERE A.CodigoM + A.NombreM LIKE '%" + txtBuscarBus.Text + "%' 
                AND CONVERT(DATE,A.FechaSis) BETWEEN '" + txtF1Bus.Text + "' AND '" + txtF2Bus.Text + "'"
        Datos = Conf.EjecutaSql(Sql)

        GvBuscar.DataSource = Datos.Tables(0)
        GvBuscar.DataBind()
    End Sub

    Protected Sub TxtCodigoCliente_TextChanged(sender As Object, e As EventArgs)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT A.NombreM, A.RTN, B.ListNom, C.CodPago, C.Descripcion, CONVERT(Varchar,DATEADD(DD,C.Dias,CONVERT(DATE,GETDATE())),126) Fecha, A.Codimpuesto, A.EncarCod, D.Porcentaje
                FROM Maestro A
                INNER JOIN ListEnc B ON A.ListCod = B.ListCod
                INNER JOIN FormasPago C ON A.CodPago = C.CodPago
				LEFT JOIN Impuestos D ON A.CodImpuesto = D.CodImpuesto
             WHERE A.Tipo = 'C' 
             AND A.CodigoM = '" + txtCodigo.Text + "'"
        Datos = Conf.EjecutaSql(Sql)

        If Datos.Tables(0).Rows.Count <> 0 Then
            Session.Add("Impuesto", Datos.Tables(0).Rows(0).Item("Codimpuesto"))
            Session.Add("CodPago", Datos.Tables(0).Rows(0).Item("CodPago"))
            txtNombre.Value = Datos.Tables(0).Rows(0).Item("NombreM")
            txtRTN.Value = Datos.Tables(0).Rows(0).Item("RTN")
            lblLista.InnerText = Datos.Tables(0).Rows(0).Item("ListNom")
            lblFormaPago.InnerText = Datos.Tables(0).Rows(0).Item("Descripcion")
            txtVencimiento.Value = Datos.Tables(0).Rows(0).Item("Fecha")
            dlVendedor.SelectedValue = Datos.Tables(0).Rows(0).Item("EncarCod")
            dlImpuesto.SelectedValue = Datos.Tables(0).Rows(0).Item("Porcentaje")
        Else
            txtNombre.Value = ""
            txtRTN.Value = ""
            lblLista.InnerText = ""
            lblFormaPago.InnerText = ""
            txtVencimiento.Value = DateTime.Now.ToString("yyyy-MM-dd")
            dlImpuesto.SelectedIndex = 0
            dlVendedor.SelectedIndex = 0
        End If
        Session("Tabla").Clear
        gvDetalle.DataSource = Session("Tabla")
        gvDetalle.DataBind()
        Totales()
    End Sub

    Protected Sub TxtBuscar_TextChanged(sender As Object, e As EventArgs)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT TOP 100 A.CodArticulo Codigo, A.NomArticulo Descripcion, C.Precio, B.Stock, A.Costo, A.Fracciones
	            FROM Articulo A
	            INNER JOIN ArticuloExis B ON A.CodArticulo = B.CodArticulo
				INNER JOIN ListDet C ON A.CodArticulo = C.CodArticulo
				INNER JOIN ListEnc D ON C.ListCod = D.ListCod
	            WHERE A.Activo = 'Y'
                AND A.Venta = '1'
                AND B.AlmacenCod = '" + dlAlmacen.SelectedValue + "'
				AND D.ListNom = '" + lblLista.InnerText + "'
                AND A.CodArticulo + A.NomArticulo + A.CodigoBarra LIKE '%" + txtBuscar.Text + "%'"
        Datos = Conf.EjecutaSql(Sql)

        If Datos.Tables(0).Rows.Count = 1 Then
            Dim Fila As DataRow = Session("Tabla").NewRow

            Session("Tabla").DefaultView.RowFilter = "Codigo='" + Datos.Tables(0).Rows(0).Item("Codigo") + "'"
            If Session("Tabla").DefaultView.Count > 0 Then
                Session("Tabla").DefaultView.Item(0).Item("Cantidad") = FormatNumber(Decimal.Parse(Session("Tabla").DefaultView.Item(0).Item("Cantidad").ToString) + 1)
                Session("Tabla").DefaultView.Item(0).Item("Total") = FormatNumber(Decimal.Parse(Session("Tabla").DefaultView.Item(0).Item("Cantidad").ToString) * Decimal.Parse(Session("Tabla").DefaultView.Item(0).Item("Precio").ToString))
            Else
                Fila("Codigo") = Datos.Tables(0).Rows(0).Item("Codigo")
                Fila("Producto") = Datos.Tables(0).Rows(0).Item("Descripcion")
                Fila("Cantidad") = FormatNumber(1)
                Fila("Precio") = FormatNumber(Datos.Tables(0).Rows(0).Item("Precio"))
                Fila("PrecioConDes") = FormatNumber(Datos.Tables(0).Rows(0).Item("Precio"))
                Fila("Costo") = Datos.Tables(0).Rows(0).Item("Costo")
                Fila("Descuento") = "0.00"
                Fila("Impuesto") = dlImpuesto.SelectedItem.Text
                Fila("Total") = FormatNumber(Datos.Tables(0).Rows(0).Item("Precio"))
                Fila("Fracciones") = Datos.Tables(0).Rows(0).Item("Fracciones")
                Session("Tabla").Rows.Add(Fila)
            End If

            Session("Tabla").DefaultView.RowFilter = "Codigo like '%%'"
            gvDetalle.DataSource = Session("Tabla")
            gvDetalle.DataBind()
            Totales()
            txtBuscar.Text = ""
            txtBuscar.Focus()
        Else
            txtProductos.Text = txtBuscar.Text
            BtnBuscarProductos2_Click(sender, e)
        End If
    End Sub

    Protected Sub TxtProductos_TextChanged(sender As Object, e As EventArgs)
        PanelProductos.Visible = True
        PanelTotales.Visible = False
        txtCodProducto.InnerText = ""
        txtNomProducto.Text = ""
        txtCantidad.Text = ""
        txtPrecio.Text = ""
        BuscarProductos()
    End Sub

    Protected Sub TxtCopiar_TextChanged(sender As Object, e As EventArgs)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String
        Sql = " SELECT A.NumDoc Numero, CONVERT(VARCHAR,A.FechaSis,103) [Fecha Sis], CONVERT(VARCHAR,A.FechaDoc,103) [Fecha Doc], A.CodigoM Codigo, A.NombreM Nombre,CONVERT(VARCHAR(50), CAST(A.TotalDoc AS MONEY ),1) Total, CASE A.Estado WHEN 'C' THEN 'Cerrado' ELSE 'Abierto' END Estado
	            FROM VenPedEnc A
                WHERE A.Estado <> 'C'
                AND A.CodigoM + A.NombreM LIKE '%" + txtCopiar.Text + "%' 
                AND CONVERT(DATE,A.FechaSis) BETWEEN '" + txtF1Copiar.Text + "' AND '" + txtF2Copiar.Text + "'"
        Datos = Conf.EjecutaSql(Sql)

        GvCopiar.DataSource = Datos.Tables(0)
        GvCopiar.DataBind()
    End Sub

    Protected Sub TxtBuscarCliente_TextChanged(sender As Object, e As EventArgs)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT TOP 200 CodigoM Codigo, NombreM Nombre, NombreExt [Descripcion], Identidad, RTN, Saldo
                FROM Maestro
	            WHERE CodigoM + NombreM + NombreExt LIKE '%" + txtBuscarCliente.Text + "%'
                AND Tipo = 'C'"
        Datos = Conf.EjecutaSql(Sql)

        gvClientes.DataSource = Datos.Tables(0)
        gvClientes.DataBind()
    End Sub

    Protected Sub DlNumeracion_SelectedIndexChanged(sender As Object, e As EventArgs)
        If dlNumeracion.SelectedItem.Text = "Manual" Then
            txtNumeracion.Text = ""
        Else
            Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String

            Sql = "SELECT A.NumSig
                FROM CorrelaDet A 
                WHERE A.Activo = 'Y'
                AND A.Descripcion = '" + dlNumeracion.SelectedItem.Text + "'"
            Datos = Conf.EjecutaSql(Sql)

            txtNumeracion.Text = Datos.Tables(0).Rows(0).Item("NumSig")
        End If
    End Sub

    Private Sub GvDetalle_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvDetalle.RowDeleting
        Session("Tabla").Rows(e.RowIndex).Delete()
        Session("Tabla").AcceptChanges()
        gvDetalle.DataSource = Session("Tabla")
        gvDetalle.DataBind()

        Totales()
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
        Dim Des As Decimal
        Dim Fila = gvDetalle.Rows(e.RowIndex)

        Session("Tabla").Rows(e.RowIndex).Item("Producto") = CType(Fila.FindControl("gvtxtProducto"), TextBox).Text.TrimEnd
        If Session("Tabla").Rows(e.RowIndex).Item("Fracciones") = "0" Then
            Session("Tabla").Rows(e.RowIndex).Item("Cantidad") = FormatNumber(Decimal.Round(Decimal.Parse(CType(Fila.FindControl("gvtxtCantidad"), TextBox).Text.TrimEnd), 0))
        Else
            Session("Tabla").Rows(e.RowIndex).Item("Cantidad") = FormatNumber(CType(Fila.FindControl("gvtxtCantidad"), TextBox).Text.TrimEnd)
        End If

        If Decimal.Parse(Session("Tabla").Rows(e.RowIndex).Item("Precio")) = 0 Then
            Des = 0.00
        Else
            Des = Decimal.Parse(CType(Fila.FindControl("gvtxtPrecio"), TextBox).Text.TrimEnd) / Decimal.Parse(Session("Tabla").Rows(e.RowIndex).Item("Precio"))
        End If

        If Des = 1 Then
            Session("Tabla").Rows(e.RowIndex).Item("Descuento") = FormatNumber(CType(Fila.FindControl("gvtxtDescuento"), TextBox).Text.TrimEnd)
            Session("Tabla").Rows(e.RowIndex).Item("PrecioConDes") = Decimal.Round(Decimal.Parse(CType(Fila.FindControl("gvtxtPrecio"), TextBox).Text.TrimEnd) - (Decimal.Parse(CType(Fila.FindControl("gvtxtPrecio"), TextBox).Text.TrimEnd) * (Decimal.Parse(CType(Fila.FindControl("gvtxtDescuento"), TextBox).Text.TrimEnd) / 100)), 2)
        Else
            Session("Tabla").Rows(e.RowIndex).Item("Descuento") = Decimal.Round((((Des * 100) - 100) * -1), 2)
            Session("Tabla").Rows(e.RowIndex).Item("PrecioConDes") = FormatNumber(Decimal.Parse(CType(Fila.FindControl("gvtxtPrecio"), TextBox).Text.TrimEnd))
        End If

        Session("Tabla").Rows(e.RowIndex).Item("Impuesto") = CType(Fila.FindControl("gvtxtImpuesto"), DropDownList).SelectedItem.Text
        Session("Tabla").Rows(e.RowIndex).Item("Total") = FormatNumber(Decimal.Parse(Session("Tabla").Rows(e.RowIndex).Item("Cantidad")) * Decimal.Parse(Session("Tabla").Rows(e.RowIndex).Item("PrecioConDes")))

        gvDetalle.EditIndex = -1
        gvDetalle.DataSource = Session("Tabla")
        gvDetalle.DataBind()
        Totales()
    End Sub

    Private Sub GvProductos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvProductos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(gvProductos, "Select$" & e.Row.RowIndex)
            e.Row.Attributes("style") = "cursor:pointer"
        End If
    End Sub

    Private Sub GvProductos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvProductos.RowCommand
        Dim Fila As Integer = Convert.ToInt32(e.CommandArgument)
        If txtCodProducto.InnerText = gvProductos.Rows(Fila).Cells(1).Text Then
            txtCantidad.Text = Decimal.Parse(txtCantidad.Text) + 1
        Else
            txtCodProducto.InnerText = gvProductos.Rows(Fila).Cells(1).Text
            txtNomProducto.Text = gvProductos.Rows(Fila).Cells(2).Text
            txtCantidad.Text = "1"
            txtPrecio.Text = gvProductos.Rows(Fila).Cells(3).Text
            txtCosto.Text = gvProductos.Rows(Fila).Cells(5).Text
            txtFracciones.Text = gvProductos.Rows(Fila).Cells(6).Text
        End If
    End Sub

    Private Sub GvBuscar_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GvBuscar.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(GvBuscar, "Select$" & e.Row.RowIndex)
            e.Row.Attributes("style") = "cursor:pointer"
        End If
    End Sub

    Private Sub GvBuscar_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GvBuscar.RowCommand
        Dim Fila As Integer = Convert.ToInt32(e.CommandArgument)
        Session.Add("BuscarNumDoc", GvBuscar.Rows(Fila).Cells(1).Text)
        PanelBuscar.Visible = False
        PanelTotales.Visible = True
        LlenarBuscar()
    End Sub

    Private Sub GvCopiar_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GvCopiar.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(GvCopiar, "Select$" & e.Row.RowIndex)
            e.Row.Attributes("style") = "cursor:pointer"
        End If
    End Sub

    Private Sub GvCopiar_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GvCopiar.RowCommand
        Dim Fila As Integer = Convert.ToInt32(e.CommandArgument)
        Session.Add("CopiaNumDoc", GvCopiar.Rows(Fila).Cells(1).Text)
        txtCodigo.Text = GvCopiar.Rows(Fila).Cells(4).Text
        TxtCodigoCliente_TextChanged(sender, e)
        PanelCopiar.Visible = False
        PanelTotales.Visible = True
        txtComentarios.Text = "Copiado de Pedido N. " + Session("CopiaNumDoc")
        LlenarCopia()
    End Sub

    Private Sub GvClientes_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvClientes.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(gvClientes, "Select$" & e.Row.RowIndex)
            e.Row.Attributes("style") = "cursor:pointer"
        End If
    End Sub

    Private Sub GvClientes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvClientes.RowCommand
        Dim Fila As Integer = Convert.ToInt32(e.CommandArgument)
        txtCodigo.Text = gvClientes.Rows(Fila).Cells(1).Text
        PanelClientes.Visible = False
        PanelTotales.Visible = True
        TxtCodigoCliente_TextChanged(sender, e)
    End Sub

    Private Sub BtnBuscarCliente_Click(sender As Object, e As EventArgs) Handles btnBuscarCliente.Click
        TxtBuscarCliente_TextChanged(sender, e)
        PanelClientes.Visible = True
        PanelTotales.Visible = False
    End Sub

    Private Sub BtnBuscarProductos2_Click(sender As Object, e As EventArgs) Handles btnBuscarProductos2.Click
        PanelProductos.Visible = True
        PanelTotales.Visible = False
        txtCodProducto.InnerText = ""
        txtNomProducto.Text = ""
        txtCantidad.Text = ""
        txtPrecio.Text = ""
        BuscarProductos()
    End Sub

    Private Sub BtnCancelarC_Click(sender As Object, e As EventArgs) Handles btnCancelarC.Click
        PanelClientes.Visible = False
        PanelTotales.Visible = True
    End Sub

    Private Sub BtnCrear_Click(sender As Object, e As EventArgs) Handles btnCrear.Click
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Detalle As String = ""
        Dim Sql As String
        Dim Estado As String = "O"
        Dim Numeracion As Integer
        Dim ImpuestoArticulo, TotalsinDesc As Decimal

        '-Inicio- Valiaciones
        If Decimal.Parse(lblTotal.InnerText.ToString) = 0 Then
            lblMsg.Text = "Error: Pedido no puede ser cero"
            lblMsg.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If

        If dlNumeracion.SelectedItem.Text = "Manual" Then
            Numeracion = -1
        Else
            Numeracion = dlNumeracion.SelectedValue
        End If
        '-Fin- Valiaciones

        '-Inicio- Detalle de docuemnto
        For I As Integer = 0 To Session("Tabla").Rows.Count - 1
            Session("dtImpuesto").DefaultView.RowFilter = "Descripcion ='" + Session("Tabla").Rows(I).Item("Impuesto") + "'"

            TotalsinDesc = Decimal.Parse(Session("Tabla").Rows(I).Item("Precio")) * Decimal.Parse(Session("Tabla").Rows(I).Item("Cantidad"))
            ImpuestoArticulo = Decimal.Parse(Session("Tabla").Rows(I).Item("PrecioConDes")) * (Decimal.Parse(Session("dtImpuesto").DefaultView.Item(0).Item("Porcentaje")) / 100)

            Detalle += " INSERT INTO [dbo].[VenPedDet]
           ([NumDoc]
           ,[CodArticulo]
           ,[Descripcion]
           ,[Cantidad]
           ,[AlmacenCod]
           ,[Descuento]
           ,[TipoImpuesto]
           ,[Impuesto]
           ,[Precio]
           ,[PrecioConDes]
           ,[Costo]
           ,[SubAntDesc]
           ,[TotalLinea]
           ,[SaldoPend]
           ,[Estado]
           ,[MonedaCod]
           ,[Cuenta]
           ,[ConCuenta]
           ,[Reconcilia]
           ,[CodUM]
           ,[Unidad]
           ,[TransID]
           ,[NumRef]
           ,[Fecha]
           ,[CodUsuario])
     VALUES
           (''NumSig''
           ,''" + Session("Tabla").Rows(I).Item("Codigo") + "''
           ,''" + Session("Tabla").Rows(I).Item("Producto") + "''
           ,''" + Replace(Session("Tabla").Rows(I).Item("Cantidad").ToString, ",", "") + "''
           ,''" + dlAlmacen.SelectedValue + "''
           ,''" + Session("Tabla").Rows(I).Item("Descuento").ToString + "''
           ,''" + Session("dtImpuesto").DefaultView.Item(0).Item("CodImpuesto").ToString + "''
           ,''" + ImpuestoArticulo.ToString + "''
           ,''" + Replace(Session("Tabla").Rows(I).Item("Precio").ToString, ",", "") + "''
           ,''" + Replace(Session("Tabla").Rows(I).Item("PrecioConDes").ToString, ",", "") + "''
           ,''" + Replace(Session("Tabla").Rows(I).Item("Costo").ToString, ",", "") + "''
           ,''" + Replace(TotalsinDesc.ToString, ",", "") + "''
           ,''" + Replace(Session("Tabla").Rows(I).Item("Total").ToString, ",", "") + "''
           ,''0''
           ,''C''
           ,''LPS''
           ,''''
           ,''''
           ,''''
           ,''1''
           ,''Unidad''
           ,0
           ,''''
           ,''" + txtFecha.Value + "''
           ,''" + Session("Usuario_Aut") + "'') "
        Next
        '-Fin- Detalle de docuemnto

        '-Inicio- Crea documento
        Sql = "DECLARE 
        @Impreso varchar(1)
        ,@FechaDoc date
        ,@FechaVen date
        ,@CodigoM varchar(30)
        ,@NombreM varchar(50)
        ,@CodPago int
        ,@EncarCod int
        ,@CodUsuario varchar(50)
        ,@Estado varchar(1)
        ,@NumRef varchar(30)
        ,@ObjectRef VARCHAR(30)
        ,@MonedaCod varchar(30)
        ,@MonedaVal varchar(30)
        ,@Anticipo float
        ,@GastAdicio float
        ,@TotalDesc float
        ,@TotalImp float
        ,@TotalDoc float
        ,@ValorApli float
        ,@TransID int
        ,@SerieCod int
        ,@Comentario varchar(100)
        ,@CAI varchar(50)
        ,@RNT varchar(30)
        ,@Detalle varchar(max)

        EXEC [SP_VenPed]
        @Impreso = 'N'
        ,@FechaDoc = '" + txtFecha.Value + "'
        ,@FechaVen = '" + txtVencimiento.Value + "'
        ,@CodigoM = '" + txtCodigo.Text + "'
        ,@NombreM = '" + txtNombre.Value + "'
        ,@CodPago = '" + Session("CodPago").ToString + "' 
        ,@EncarCod = '" + dlVendedor.SelectedValue + "'
        ,@CodUsuario = '" + Session("Usuario_Aut") + "'
        ,@Estado = '" + Estado + "'
        ,@NumRef  = '" + Session("CopiaNumDoc") + "'
        ,@ObjectRef = '" + Session("ObjectRef") + "'
        ,@MonedaCod = 'LPS'
        ,@MonedaVal = '1'
        ,@Anticipo = '0'
        ,@GastAdicio = '0'
        ,@TotalDesc = '" + Replace(lblDescuento.InnerText.ToString, ",", "") + "'
        ,@TotalImp = '" + Replace(lblImpuesto.InnerText.ToString, ",", "") + "'
        ,@TotalDoc = '" + Replace(lblTotal.InnerText.ToString, ",", "") + "'
        ,@ValorApli = '0'
        ,@TransID = '0'
        ,@SerieCod = '" + Numeracion.ToString + "'
        ,@Comentario = '" + txtComentarios.Text + "'
        ,@CAI = '" + Session("Numeracion").Rows(dlNumeracion.SelectedIndex).Item("Info") + "'
        ,@RNT = '" + txtRTN.Value + "'
        ,@Detalle = '" + Detalle + "'"
        Datos = Conf.EjecutaSql(Sql)
        '-Fin- Crea documento

        '-Inicio- Optiene resultado de creacion de documento
        If Integer.Parse(Datos.Tables(0).Rows(0).Item("Error")) = 0 Then
            Session.Add("NumDoc", Datos.Tables(0).Rows(0).Item("NumDoc").ToString)
            lblMsg.Text = Datos.Tables(0).Rows(0).Item("MSG").ToString
            lblMsg.ControlStyle.CssClass = "alert alert-success"
            txtCodigo.Text = ""
            TxtCodigoCliente_TextChanged(sender, e)
            LlenarNumeracion()

            Dim javaScript As String = "window.open('reportes.aspx','_blank','scrollbars=yes,resizable=yes,top=5,left=5,width=700,height=700');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)
        Else
            lblMsg.Text = "Error: " + Datos.Tables(0).Rows(0).Item("Error").ToString + Datos.Tables(0).Rows(0).Item("MSG").ToString
            lblMsg.ControlStyle.CssClass = "alert alert-danger"
        End If
        '-Fin- Optiene resultado de creacion de documento
    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        PanelProductos.Visible = False
        PanelTotales.Visible = True
        txtProductos.Text = ""
    End Sub

    Private Sub BtnBuscarProductos_Click(sender As Object, e As EventArgs) Handles btnBuscarProductos.Click
        PanelProductos.Visible = True
        PanelTotales.Visible = False
        txtCodProducto.InnerText = ""
        txtNomProducto.Text = ""
        txtCantidad.Text = ""
        txtPrecio.Text = ""
        BuscarProductos()
    End Sub

    Private Sub BtnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        If txtCodProducto.InnerText.Length > 0 Then
            Dim Fila As DataRow = Session("Tabla").NewRow

            Fila("Codigo") = txtCodProducto.InnerText
            Fila("Producto") = txtNomProducto.Text
            If txtFracciones.Text = "0" Then
                Fila("Cantidad") = FormatNumber(Decimal.Round(Decimal.Parse(txtCantidad.Text), 0))
                Fila("Total") = FormatNumber(Decimal.Round(Decimal.Parse(txtCantidad.Text), 0) * Decimal.Parse(txtPrecio.Text))
            Else
                Fila("Cantidad") = FormatNumber(txtCantidad.Text)
                Fila("Total") = FormatNumber(Decimal.Parse(txtCantidad.Text) * Decimal.Parse(txtPrecio.Text))
            End If
            Fila("Precio") = FormatNumber(txtPrecio.Text)
            Fila("PrecioConDes") = FormatNumber(txtPrecio.Text)
            Fila("Costo") = FormatNumber(txtCosto.Text)
            Fila("Descuento") = "0.00"
            Fila("Impuesto") = dlImpuesto.SelectedItem.Text
            Fila("CodImpuesto") = Session("dtImpuesto").Rows(dlImpuesto.SelectedIndex).Item("CodImpuesto")
            Fila("Fracciones") = txtFracciones.Text
            Session("Tabla").Rows.Add(Fila)

            gvDetalle.DataSource = Session("Tabla")
            gvDetalle.DataBind()
            Totales()

            txtCodProducto.InnerText = ""
            txtCantidad.Text = ""

            If chkFijo.Checked = False Then
                PanelProductos.Visible = False
                PanelTotales.Visible = True
            End If
        End If
    End Sub

    Private Sub BtnCancelarBuscar_Click(sender As Object, e As EventArgs) Handles btnCancelarBuscar.Click
        PanelBuscar.Visible = False
        PanelTotales.Visible = True
    End Sub

    Private Sub BtnBuscarBus_Click(sender As Object, e As EventArgs) Handles btnBuscarBus.Click
        TxtBuscarBus_TextChanged()
    End Sub

    Private Sub BtnCancelarCopiar_Click(sender As Object, e As EventArgs) Handles btnCancelarCopiar.Click
        PanelCopiar.Visible = False
        PanelTotales.Visible = True
    End Sub

    Private Sub BtnBuscarCopiar_Click(sender As Object, e As EventArgs) Handles btnBuscarCopiar.Click
        TxtCopiar_TextChanged(sender, e)
    End Sub

    Private Sub BarReimpresion_ServerClick(sender As Object, e As EventArgs) Handles barReimpresion.ServerClick
        Session.Add("NumDoc", txtNumeracion.Text)
        Dim javaScript As String = "window.open('reportes.aspx','_blank','scrollbars=yes,resizable=yes,top=5,left=5,width=700,height=700');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)
    End Sub

    Private Sub BarBuscar_ServerClick(sender As Object, e As EventArgs) Handles barBuscar.ServerClick
        PanelBuscar.Visible = True
        PanelTotales.Visible = False
        TxtBuscarBus_TextChanged()
    End Sub

    Private Sub BarNuevo_ServerClick(sender As Object, e As EventArgs) Handles BarNuevo.ServerClick
        Response.Redirect("VenPed.aspx")
    End Sub

    Private Sub BarCancelar_ServerClick(sender As Object, e As EventArgs) Handles BarCancelar.ServerClick
        PanelCancelar.Visible = True
        PanelTotales.Visible = False
    End Sub

    Private Sub BarCopiar_ServerClick(sender As Object, e As EventArgs) Handles barCopiar.ServerClick
        PanelCopiar.Visible = True
        PanelTotales.Visible = False
        TxtCopiar_TextChanged(sender, e)
    End Sub

    Private Sub BntCancelarCan_Click(sender As Object, e As EventArgs) Handles bntCancelarCan.Click
        PanelCancelar.Visible = False
        PanelTotales.Visible = True
    End Sub

    Private Sub BtnAceptarCan_Click(sender As Object, e As EventArgs) Handles btnAceptarCan.Click
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "UPDATE VenPedEnc SET Estado = 'D' WHERE NumDoc = '" + txtNumeracion.Text + "'"
        Conf.EjecutaSql(Sql)
        Response.Redirect("VenPed.aspx")
    End Sub

End Class