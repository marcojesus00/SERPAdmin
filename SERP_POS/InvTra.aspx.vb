Public Class InvTra
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut, Status, SuperUser, Funcion As String
    Private Datos As DataSet
    Private Tabla As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario_Aut") = "" Or Session("Destino") <> "InvTra.aspx" Then
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
            txtF1Reim.Text = DateTime.Now.ToString("yyyy-MM-dd")
            txtF2Reim.Text = DateTime.Now.ToString("yyyy-MM-dd")
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
            Tabla.Columns.Add("ImpuestoValor")
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

        dlAlmacen2.DataSource = Datos.Tables(0)
        dlAlmacen2.DataTextField = "Descripcion"
        dlAlmacen2.DataValueField = "AlmacenCod"
        dlAlmacen2.DataBind()
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
                AND B.ObjectCode = '15'"
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

        Sql = "SELECT TOP 100 A.CodArticulo Codigo, A.NomArticulo Descripcion, A.Costo Precio, B.Stock, A.Costo, A.Fracciones
	            FROM Articulo A
	            INNER JOIN ArticuloExis B ON A.CodArticulo = B.CodArticulo
	            WHERE A.Activo = 'Y'	
                AND A.Tipo = 'A'
                AND B.AlmacenCod = '" + dlAlmacen.SelectedValue + "'
                AND A.CodArticulo + A.NomArticulo + A.CodigoBarra LIKE '%" + txtProductos.Text + "%'"
        Datos = Conf.EjecutaSql(Sql)

        gvProductos.DataSource = Datos.Tables(0)
        gvProductos.DataBind()
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
            dlAlmacen2.Enabled = True
        Else
            dlAlmacen.Enabled = False
            dlAlmacen2.Enabled = False
        End If
    End Sub

    Protected Sub TxtBuscar_TextChanged(sender As Object, e As EventArgs)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT TOP 100 A.CodArticulo Codigo, A.NomArticulo Descripcion, A.Costo Precio, B.Stock, A.Costo, A.Fracciones
	            FROM Articulo A
	            INNER JOIN ArticuloExis B ON A.CodArticulo = B.CodArticulo	
	            WHERE A.Activo = 'Y'
                AND A.Tipo = 'A'
                AND B.AlmacenCod = '" + dlAlmacen.SelectedValue + "'
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

    Protected Sub TxtReimprimir_TextChanged(sender As Object, e As EventArgs)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String
        Sql = " SELECT A.NumDoc Numero, CONVERT(VARCHAR,A.FechaSis,103) Fecha, A.CodigoM Codigo, A.NombreM Nombre,CONVERT(VARCHAR(50), CAST(A.TotalDoc AS MONEY ),1) Total, CASE A.Estado WHEN 'C' THEN 'Cerrado' ELSE 'Abierto' END Estado
	            FROM InvTraEnc A
                WHERE A.CodigoM + A.NombreM LIKE '%" + txtReimprimir.Text + "%' 
                AND CONVERT(DATE,A.FechaSis) BETWEEN '" + txtF1Reim.Text + "' AND '" + txtF2Reim.Text + "'"
        Datos = Conf.EjecutaSql(Sql)

        GvReimprimir.DataSource = Datos.Tables(0)
        GvReimprimir.DataBind()
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

    Private Sub GvReimprimir_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GvReimprimir.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(GvReimprimir, "Select$" & e.Row.RowIndex)
            e.Row.Attributes("style") = "cursor:pointer"
        End If
    End Sub

    Private Sub GvReimprimir_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GvReimprimir.RowCommand
        Dim Fila As Integer = Convert.ToInt32(e.CommandArgument)
        Session.Add("NumDoc", GvReimprimir.Rows(Fila).Cells(1).Text)
        Dim javaScript As String = "window.open('reportes.aspx','_blank','scrollbars=yes,resizable=yes,top=5,left=5,width=700,height=700');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)
    End Sub

    Private Sub BtnCrear_Click(sender As Object, e As EventArgs) Handles btnCrear.Click
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Detalle As String = ""
        Dim Sql As String
        Dim Estado As String = "C"
        Dim Numeracion As Integer
        Dim ImpuestoArticulo, TotalsinDesc As Decimal

        If Decimal.Parse(lblTotal.InnerText.ToString) = 0 Then
            lblMsg.Text = "Error: Traslado no puede ser cero"
            lblMsg.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If

        If dlNumeracion.SelectedItem.Text = "Manual" Then
            Numeracion = -1
        Else
            Numeracion = dlNumeracion.SelectedValue
        End If

        For I As Integer = 0 To Session("Tabla").Rows.Count - 1
            Session("dtImpuesto").DefaultView.RowFilter = "Descripcion ='" + Session("Tabla").Rows(I).Item("Impuesto") + "'"

            TotalsinDesc = Decimal.Parse(Session("Tabla").Rows(I).Item("Precio")) * Decimal.Parse(Session("Tabla").Rows(I).Item("Cantidad"))
            ImpuestoArticulo = Decimal.Parse(Session("Tabla").Rows(I).Item("PrecioConDes")) * (Decimal.Parse(Session("dtImpuesto").DefaultView.Item(0).Item("Porcentaje")) / 100)

            Detalle += " INSERT INTO [dbo].[InvTraDet]
           ([NumDoc]
           ,[CodArticulo]
           ,[Descripcion]
           ,[Cantidad]
           ,[AlmacenCod]
           ,[AlmacenCod2]
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
           ,''" + Session("Tabla").Rows(I).Item("Cantidad") + "''
           ,''" + dlAlmacen.SelectedValue + "''
           ,''" + dlAlmacen2.SelectedValue + "''
           ,''" + Session("Tabla").Rows(I).Item("Descuento") + "''
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

        Sql = "EXEC SP_InvTra
            'N'
           ,'" + txtFecha.Value + "'
           ,'" + txtVencimiento.Value + "'
           ,''
           ,''
           ,'' 
           ,''
           ,'" + Session("Usuario_Aut") + "'
           ,'" + Estado + "'
           ,''
           ,'LPS'
           ,'1'
           ,'0'
           ,'0'
           ,'" + Replace(lblDescuento.InnerText.ToString, ",", "") + "'
           ,'" + Replace(lblImpuesto.InnerText.ToString, ",", "") + "'
           ,'" + Replace(lblTotal.InnerText.ToString, ",", "") + "'
           ,'0'
           ,'0'
           ,'" + Numeracion.ToString + "'
           ,'" + txtComentarios.Text + "'
           ,'" + Session("Numeracion").Rows(dlNumeracion.SelectedIndex).Item("Info") + "'
           ,''
           ,'" + Detalle + "'"
        Datos = Conf.EjecutaSql(Sql)

        If Integer.Parse(Datos.Tables(0).Rows(0).Item("Error")) = 0 Then
            Session.Add("NumDoc", Datos.Tables(0).Rows(0).Item("NumDoc").ToString)
            lblMsg.Text = " Traslado creada N. " + Session("NumDoc") + " "
            lblMsg.ControlStyle.CssClass = "alert alert-success"
            LlenarNumeracion()
            Session("Tabla").Clear
            gvDetalle.DataSource = Session("Tabla")
            gvDetalle.DataBind()
            Totales()

            Dim javaScript As String = "window.open('reportes.aspx','_blank','scrollbars=yes,resizable=yes,top=5,left=5,width=700,height=700');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)
        Else
            lblMsg.Text = " Error: " + Datos.Tables(0).Rows(0).Item("Error").ToString + " "
            lblMsg.ControlStyle.CssClass = "alert alert-danger"
        End If
    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        PanelProductos.Visible = False
        PanelTotales.Visible = True
        txtProductos.Text = ""
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

    Private Sub BtnBuscarProductos_Click(sender As Object, e As EventArgs) Handles btnBuscarProductos.Click
        txtProductos.Text = txtBuscar.Text
        BuscarProductos()
    End Sub

    Private Sub BtnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Response.Redirect("InvTra.aspx")
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

    Private Sub BtnCancelarReimp_Click(sender As Object, e As EventArgs) Handles btnCancelarReimp.Click
        PanelReimprimir.Visible = False
        PanelTotales.Visible = True
    End Sub

    Private Sub BtnBuscarReimp_Click(sender As Object, e As EventArgs) Handles btnBuscarReimp.Click
        TxtReimprimir_TextChanged(sender, e)
    End Sub

    Private Sub BarReimpresion_ServerClick(sender As Object, e As EventArgs) Handles barReimpresion.ServerClick
        PanelReimprimir.Visible = True
        PanelTotales.Visible = False
        TxtReimprimir_TextChanged(sender, e)
    End Sub

End Class