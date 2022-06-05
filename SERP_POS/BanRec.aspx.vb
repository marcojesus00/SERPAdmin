Public Class BanRec
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut, Status, SuperUser, Funcion As String
    Private Datos As DataSet
    Private Tabla As DataTable
    Private TablaPago As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario_Aut") = "" Or Session("Destino") <> "BanRec.aspx" Then
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

        If Not IsPostBack Then
            txtFecha.Value = DateTime.Now.ToString("yyyy-MM-dd")
            txtVencimiento.Value = DateTime.Now.ToString("yyyy-MM-dd")
            LlenarNumeracion()

            Tabla = New DataTable
            TablaPago = New DataTable

            Tabla.Columns.Add("Tipo")
            Tabla.Columns.Add("Documento")
            Tabla.Columns.Add("Fecha")
            Tabla.Columns.Add("DiasdeAtraso")
            Tabla.Columns.Add("SaldoVencido")
            Tabla.Columns.Add("ImporteAplicado")

            TablaPago.Columns.Add("Monto")
            TablaPago.Columns.Add("Medio")
            TablaPago.Columns.Add("Referencia")
            TablaPago.Columns.Add("Banco")

            Session.Add("Tabla", Tabla)
            Session.Add("tbPago", TablaPago)

            If dlNumeracion.SelectedValue = "Manual" Then
                txtNumeracion.Enabled = True
            Else
                txtNumeracion.Enabled = False
                txtNumeracion.Text = Session("Numeracion").Rows(dlNumeracion.SelectedIndex).Item("NumSig")
            End If
        End If
    End Sub

    Sub LlenarNumeracion()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT A.ID IdCorre, A.Descripcion, A.NumSig, Info
                FROM CorrelaDet A 
				INNER JOIN CorrelaEnc B ON A.IdCorre = B.IdCorre
                WHERE A.Activo = 'Y'
                AND B.ObjectCode = '16'"
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

        Sql = "SELECT fd.Numdoc Documento,
                Convert(Date, fd.FechaDoc) Fecha,                 
                DateDiff(DD, fd.fechaDoc, Convert(Date, getdate())) [Dias atraso],      
                fd.TotalDoc-fd.ValorApli [Saldo vencido],
                fd.TotalDoc [Total documento], 
                'FV' Tipo
                    FROM VenFacEnc fd               
                    WHERE fd.codigoM = '" + txtCodigo.Text + "' 
                    AND fd.Estado = 'O'
                UNION ALL
                Select nc.NumDoc,
                nc.FechaSis,
                0,
                -(nc.TotalDoc-nc.ValorApli),
                nc.TotalDoc,
                'NC' 
                    from VenNcEnc nc 
                    where CodigoM = '" + txtCodigo.Text + "'  
                    and Estado = 'O'"
        Datos = Conf.EjecutaSql(Sql)

        gvProductos.DataSource = Datos.Tables(0)
        gvProductos.DataBind()
    End Sub

    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub

    Sub Totales()
        Dim Total As Decimal = 0

        For i As Integer = 0 To Session("Tabla").Rows.Count - 1
            Total += Decimal.Parse(Session("Tabla").Rows(i).Item("ImporteAplicado"))
        Next
        lblTotal.InnerText = FormatNumber(Total)
    End Sub

    Sub TotalPagos()
        Dim Total As Decimal = 0
        For i As Integer = 0 To Session("tbPago").Rows.Count - 1
            Total += Decimal.Parse(Session("tbPago").Rows(i).Item("Monto"))
        Next

        lblSaldo.InnerText = FormatNumber(Decimal.Parse(lblTotal.InnerText) - Total)
        lblPagado.InnerText = FormatNumber(Total)
        lblSubTotal.InnerText = FormatNumber(Total)
    End Sub

    Protected Sub TxtBuscarCliente_TextChanged(sender As Object, e As EventArgs)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT TOP 200 CodigoM Codigo, NombreM Nombre, NombreExt [Descripcion], Identidad, RTN, Saldo
                FROM Maestro
                    WHERE Tipo = 'C' and saldo <> 0
                    AND CodigoM + NombreM + NombreExt LIKE '%" + txtBuscarCliente.Text + "%'"
        Datos = Conf.EjecutaSql(Sql)

        gvClientes.DataSource = Datos.Tables(0)
        gvClientes.DataBind()
    End Sub

    Protected Sub TxtCodigoCliente_TextChanged(sender As Object, e As EventArgs)
        Dim Conf, conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT A.NombreM, A.RTN, B.ListNom, C.CodPago, C.Descripcion, CONVERT(Varchar,DATEADD(DD,C.Dias,CONVERT(DATE,GETDATE())),126) Fecha, A.Codimpuesto, A.EncarCod
                FROM Maestro A
                INNER JOIN ListEnc B ON A.ListCod = B.ListCod
                INNER JOIN FormasPago C ON A.CodPago = C.CodPago
                 WHERE A.Tipo = 'C'
                 AND A.CodigoM = '" + txtCodigo.Text + "'"
        Datos = Conf.EjecutaSql(Sql)

        If Datos.Tables(0).Rows.Count <> 0 Then
            txtNombre.Value = Datos.Tables(0).Rows(0).Item("NombreM")
            txtRTN.Value = Datos.Tables(0).Rows(0).Item("RTN")
        Else
            txtNombre.Value = ""
            txtRTN.Value = ""
        End If
        Session("Tabla").Clear
        gvDetalle.DataSource = Session("Tabla")
        gvDetalle.DataBind()
        Totales()
    End Sub

    Protected Sub TxtBuscar_TextChanged(sender As Object, e As EventArgs)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT TOP 100 A.CodArticulo Codigo, A.NomArticulo Descripcion, C.Precio, B.Stock, A.Costo
                 FROM Articulo A
                 INNER JOIN ArticuloExis B ON A.CodArticulo = B.CodArticulo
                 INNER JOIN ListDet C ON A.CodArticulo = C.CodArticulo
                 INNER JOIN ListEnc D ON C.ListCod = D.ListCod
                 WHERE A.Activo = 'Y'"
        Datos = Conf.EjecutaSql(Sql)

        'If Datos.Tables(0).Rows.Count = 1 Then
        '    Dim Fila As DataRow = Session("Tabla").NewRow

        '    Fila("Documento") = Datos.Tables(0).Rows(0).Item("Documento")
        '    Fila("Descripcion") = Datos.Tables(0).Rows(0).Item("Descripcion")
        '    Fila("Fecha") = FormatNumber(1)
        '    Fila("Precio") = FormatNumber(Datos.Tables(0).Rows(0).Item("Precio"))
        '    Fila("PrecioConDes") = FormatNumber(Datos.Tables(0).Rows(0).Item("Precio"))
        '    Fila("Costo") = Datos.Tables(0).Rows(0).Item("Costo")
        '    Fila("Descuento") = "0.00"
        '    'Fila("Impuesto") = dlImpuesto.SelectedItem.Text
        '    Fila("Total") = FormatNumber(Datos.Tables(0).Rows(0).Item("Precio"))
        '    Session("Tabla").Rows.Add(Fila)

        '    gvDetalle.DataSource = Session("Tabla")
        '    gvDetalle.DataBind()
        '    Totales()
        '    txtBuscar.Text = ""
        '    txtBuscar.Focus()
        '    'Else
        '    '    txtProductos.Text = txtBuscar.Text
        '    '    btnBuscarProductos2_Click(sender, e)
        'End If

    End Sub

    Protected Sub TxtProductos_TextChanged(sender As Object, e As EventArgs)
        PanelProductos.Visible = True
        PanelTotales.Visible = False
        txtTipo.Text = ""
        txtCodProducto.InnerText = ""
        txtFechaFac.Text = ""
        txtDias.Text = ""
        txtSaldo.Text = ""

        BuscarProductos()
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

    Protected Sub DlMedio_SelectedIndexChanged(sender As Object, e As EventArgs)
        If dlMedio.SelectedValue = "Efectivo" Then
            colBanco.Visible = False
            colRef.Visible = False
            dlBanco.SelectedIndex = 0
            txtRef.Text = ""
        Else
            colBanco.Visible = True
            colRef.Visible = True
        End If
    End Sub

    Private Sub GvDetalle_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvDetalle.RowDeleting
        If Decimal.Parse(lblPagado.InnerText) = 0 Then
            Session("Tabla").Rows(e.RowIndex).Delete()
            Session("Tabla").AcceptChanges()
            gvDetalle.DataSource = Session("Tabla")
            gvDetalle.DataBind()

            Totales()
        Else
            Msg("Para eliminar un producto antes elimine el pago")
        End If
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

        If Session("Tabla").Rows(e.RowIndex).Item("Tipo") = "FV" Then
            If FormatNumber(CType(Fila.FindControl("gvtxtImporApl"), TextBox).Text.TrimEnd) > 0 And FormatNumber(CType(Fila.FindControl("gvtxtImporApl"), TextBox).Text.TrimEnd) <= Session("Tabla").Rows(e.RowIndex).Item("SaldoVencido") Then
                Session("Tabla").Rows(e.RowIndex).Item("ImporteAplicado") = FormatNumber(CType(Fila.FindControl("gvtxtImporApl"), TextBox).Text.TrimEnd)
            End If
        Else
            If FormatNumber(CType(Fila.FindControl("gvtxtImporApl"), TextBox).Text.TrimEnd) <= Session("Tabla").Rows(e.RowIndex).Item("SaldoVencido") And FormatNumber(CType(Fila.FindControl("gvtxtImporApl"), TextBox).Text.TrimEnd) < 0 Then
                Session("Tabla").Rows(e.RowIndex).Item("ImporteAplicado") = FormatNumber(CType(Fila.FindControl("gvtxtImporApl"), TextBox).Text.TrimEnd)
            End If
        End If

        gvDetalle.EditIndex = -1
        gvDetalle.DataSource = Session("Tabla")
        gvDetalle.DataBind()
        Totales()
    End Sub

    Private Sub GvPagos_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvPagos.RowDeleting
        Session("tbPago").Rows(e.RowIndex).Delete()
        Session("Tabla").AcceptChanges()
        gvPagos.DataSource = Session("tbPago")
        gvPagos.DataBind()

        TotalPagos()
        txtMontoP.Text = lblSaldo.InnerText
    End Sub

    Private Sub GvProductos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvProductos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(gvProductos, "Select$" & e.Row.RowIndex)
            e.Row.Attributes("style") = "cursor:pointer"
        End If
    End Sub

    Private Sub GvProductos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvProductos.RowCommand
        Dim Fila As Integer = Convert.ToInt32(e.CommandArgument)
        txtTipo.Text = gvProductos.Rows(Fila).Cells(1).Text
        txtCodProducto.InnerText = gvProductos.Rows(Fila).Cells(2).Text
        txtFechaFac.Text = gvProductos.Rows(Fila).Cells(3).Text
        txtDias.Text = gvProductos.Rows(Fila).Cells(4).Text
        txtSaldo.Text = gvProductos.Rows(Fila).Cells(5).Text
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

    Private Sub Btnbuscarcliente_click(sender As Object, e As EventArgs) Handles btnBuscarCliente.Click
        TxtBuscarCliente_TextChanged(sender, e)
        PanelClientes.Visible = True
        PanelTotales.Visible = False
    End Sub

    Private Sub BtnCancelarC_Click(sender As Object, e As EventArgs) Handles btnCancelarC.Click
        PanelClientes.Visible = False
        PanelTotales.Visible = True
    End Sub

    Private Sub BtnCerrarPago_Click(sender As Object, e As EventArgs) Handles btnCerrarPago.Click
        PanelPagos.Visible = False
        PanelTotales.Visible = True
    End Sub

    Private Sub BtnAgregarPago_Click(sender As Object, e As EventArgs) Handles btnAgregarPago.Click
        If Decimal.Parse(txtMontoP.Text) <= Decimal.Parse(lblSaldo.InnerText) And Decimal.Parse(txtMontoP.Text) <> 0 Then
            Dim Fila As DataRow = Session("tbPago").NewRow

            Fila("Monto") = FormatNumber(txtMontoP.Text)
            Fila("Referencia") = txtRef.Text
            Fila("Medio") = dlMedio.SelectedValue
            Fila("Banco") = dlBanco.SelectedValue
            Session("tbPago").Rows.Add(Fila)

            TotalPagos()

            gvPagos.DataSource = Session("tbPago")
            gvPagos.DataBind()
            txtMontoP.Text = Decimal.Parse(lblSaldo.InnerText)
        End If
    End Sub

    Private Sub BtnCrear_Click(sender As Object, e As EventArgs) Handles btnCrear.Click
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Detalle As String = ""
        Dim Pago As String = ""
        Dim Sql As String
        Dim Numeracion As Integer

        If Decimal.Parse(lblPagado.InnerHtml) > Decimal.Parse(lblTotal.InnerHtml) Then
            lblMsg.Text = "Error: El pago no puede ser mayor que el total de Documentos "
            lblMsg.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If

        If Decimal.Parse(lblPagado.InnerHtml) < Decimal.Parse(lblTotal.InnerHtml) Then
            lblMsg.Text = "Error: Debe realizar el pago del Documento "
            lblMsg.ControlStyle.CssClass = "alert alert-danger"
            Exit Sub
        End If

        If dlNumeracion.SelectedItem.Text = "Manual" Then
            Numeracion = -1
        Else
            Numeracion = dlNumeracion.SelectedValue
        End If

        For I As Integer = 0 To Session("Tabla").Rows.Count - 1
            Detalle += " INSERT INTO [dbo].[BanRecDet1]
        ([NumDoc]
        ,[ObjectCode]
        ,[NumDocRef]
        ,[ValorDoc]
        ,[Aplicado]
        ,[Tipo])
        VALUES
        (''NumSig''
        ,''16''
        ,''" + Session("Tabla").Rows(I).Item("Documento") + "''
        ,''" + Replace(Session("Tabla").Rows(I).Item("SaldoVencido").ToString, ",", "") + "''
        ,''" + Replace(Session("Tabla").Rows(I).Item("ImporteAplicado").ToString, ",", "") + "''
        ,''" + Session("Tabla").Rows(I).Item("Tipo") + "'') "
        Next

        For I As Integer = 0 To Session("tbPago").Rows.Count - 1
            Pago += " INSERT INTO [dbo].[BanRecDet2]
        ([NumDoc]
        ,[Monto]
        ,[Medio]
        ,[Referencia]
        ,[Banco])
        VALUES
        (''NumSig''
        ,''" + Replace(Session("tbPago").Rows(I).Item("Monto").ToString, ",", "") + "''
        ,''" + Session("tbPago").Rows(I).Item("Medio") + "''
        ,''" + Session("tbPago").Rows(I).Item("Referencia") + "''
        ,''" + Session("tbPago").Rows(I).Item("Banco") + "'') "
        Next

        Sql = "EXEC SP_BanRec
         '" + txtFecha.Value + "'
        ,'" + txtCodigo.Text + "'
        ,'" + txtNombre.Value + "'
        ,'" + Session("Usuario_Aut") + "'
        ,'C'          
        ,''
        ,'LPS'
        ,'1'
        ,'" + lblPagado.InnerHtml + "'
        ,'0'
        ,'" + Numeracion.ToString + "'
        ,'" + txtComentarios.Text + "'   
        ,'" + Detalle + "'
        ,'" + Pago + "'"

        Datos = Conf.EjecutaSql(Sql)

        If Integer.Parse(Datos.Tables(0).Rows(0).Item("Error")) = 0 Then
            Session.Add("NumDoc", Datos.Tables(0).Rows(0).Item("NumDoc").ToString)
            lblMsg.Text = Datos.Tables(0).Rows(0).Item("MSG").ToString
            lblMsg.ControlStyle.CssClass = "alert alert-success"
            txtCodigo.Text = ""
            TxtCodigoCliente_TextChanged(sender, e)
            LlenarNumeracion()
            Session("tbPago").Clear
            gvPagos.DataSource = Session("tbPago")
            gvPagos.DataBind()
            TotalPagos()
            Dim javaScript As String = "window.open('reportes.aspx','_blank','scrollbars=yes,resizable=yes,top=5,left=5,width=700,height=700');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)
        Else
            lblMsg.Text = "Error: " + Datos.Tables(0).Rows(0).Item("Error").ToString + " " + Datos.Tables(0).Rows(0).Item("MSG")
            lblMsg.ControlStyle.CssClass = "alert alert-danger"
        End If

    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        PanelProductos.Visible = False
        PanelTotales.Visible = True
        ''txtProductos.Text = ""
    End Sub

    Private Sub BtnBuscarProductos2_Click(sender As Object, e As EventArgs) Handles btnBuscarProductos2.Click
        PanelProductos.Visible = True
        PanelTotales.Visible = False
        txtTipo.Text = ""
        txtCodProducto.InnerText = ""
        txtFechaFac.Text = ""
        txtDias.Text = ""
        txtSaldo.Text = ""

        BuscarProductos()
    End Sub

    Private Sub BtnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Response.Redirect("BanRec.aspx")
    End Sub

    Private Sub BtnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        Session("Tabla").DefaultView.RowFilter = "Tipo= '" + txtTipo.Text + "' AND Documento= '" + txtCodProducto.InnerText + "'"
        If Session("Tabla").DefaultView.Count > 0 Then
            Session("Tabla").DefaultView.RowFilter = "Tipo like '%%'"
            Exit Sub
        End If
        Session("Tabla").DefaultView.RowFilter = "Tipo like '%%'"

        If txtCodProducto.InnerText.Length > 0 Then
            Dim Fila As DataRow = Session("Tabla").NewRow

            Fila("Tipo") = txtTipo.Text
            Fila("Documento") = txtCodProducto.InnerText
            Fila("Fecha") = FormatDateTime(txtFechaFac.Text)
            Fila("DiasdeAtraso") = FormatNumber(txtDias.Text)
            Fila("SaldoVencido") = FormatNumber(txtSaldo.Text)
            Fila("ImporteAplicado") = FormatNumber(txtSaldo.Text)
            Session("Tabla").Rows.Add(Fila)

            gvDetalle.DataSource = Session("Tabla")
            gvDetalle.DataBind()
            Totales()

            txtCodProducto.InnerText = ""

            If chkFijo.Checked = False Then
                PanelProductos.Visible = False
                PanelTotales.Visible = True
            End If
        End If
    End Sub

    Private Sub BarMedio_ServerClick(sender As Object, e As EventArgs) Handles barMedio.ServerClick
        PanelPagos.Visible = True
        PanelTotales.Visible = False
        TotalPagos()
        txtMontoP.Text = Decimal.Parse(lblSaldo.InnerText)
        gvPagos.DataSource = Session("tbPago")
        gvPagos.DataBind()
    End Sub

End Class