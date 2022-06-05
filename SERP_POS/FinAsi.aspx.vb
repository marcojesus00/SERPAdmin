Public Class FinAsi
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut, Status, SuperUser, Funcion As String
    Private Datos As DataSet
    Private Tabla As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario_Aut") = "" Or Session("Destino") <> "FinAsi.aspx" Then
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
            Session.Add("CopiaNumDoc", "")
            Session.Add("ObjectRef", "")
            txtFecha.Value = DateTime.Now.ToString("yyyy-MM-dd")
            txtVencimiento.Value = DateTime.Now.ToString("yyyy-MM-dd")
            txtF1Bus.Text = DateTime.Now.ToString("yyyy-MM-dd")
            txtF2Bus.Text = DateTime.Now.ToString("yyyy-MM-dd")
            txtF1Copiar.Text = DateTime.Now.ToString("yyyy-MM-dd")
            txtF2Copiar.Text = DateTime.Now.ToString("yyyy-MM-dd")
            LlenarNumeracion()

            Tabla = New DataTable
            Tabla.Columns.Add("Cuenta")
            Tabla.Columns.Add("Nombre")
            Tabla.Columns.Add("Debe")
            Tabla.Columns.Add("Haber")
            Tabla.Columns.Add("Asociado")
            Tabla.Columns.Add("Grupo")
            Session.Add("Tabla", Tabla)

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
                AND B.ObjectCode = '20'"
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

        Sql = "SELECT TOP (100)
               [Cuenta]
              ,[Nombre]     
              ,[Saldo]
              ,[Grupo]
                  FROM [FinCuentas] A
                  WHERE A.Nivel = '5'
                  AND A.Activo = 'Y'
                  AND A.Cuenta + A.Nombre like '%" + txtProductos.Text + "%'
                  ORDER BY A.Cuenta"
        Datos = Conf.EjecutaSql(Sql)

        gvProductos.DataSource = Datos.Tables(0)
        gvProductos.DataBind()
    End Sub

    Sub LlenarCopia()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String
        Sql = " SELECT A.Cuenta, A.Nombre, A.Debe, A.Haber, A.Asociado, A.Grupo
	            FROM FinAsiDet A
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
        Sql = "SELECT D.FechaDoc, D.FechaVen, CASE D.Estado WHEN 'O' THEN '(Abierto)' WHEN 'C' THEN '(Cerrado)' ELSE '(Cancelado)' END Estado, D.SerieCod, D.Comentario
	            FROM FinAsiEnc D
                WHERE D.NumDoc = '" + Session("BuscarNumDoc") + "'"
        Datos = Conf.EjecutaSql(Sql)

        dlNumeracion.SelectedValue = Datos.Tables(0).Rows(0).Item("SerieCod")
        txtNumeracion.Text = Session("BuscarNumDoc")
        txtFecha.Value = Date.Parse(Datos.Tables(0).Rows(0).Item("FechaDoc").ToString).ToString("yyyy-MM-dd")
        txtVencimiento.Value = Date.Parse(Datos.Tables(0).Rows(0).Item("FechaVen").ToString).ToString("yyyy-MM-dd")
        lblEstado.Text = Datos.Tables(0).Rows(0).Item("Estado")
        txtComentarios.Text = Datos.Tables(0).Rows(0).Item("Comentario")
        '--------------------------------------------

        '------------- Inactivar --------------------
        txtFecha.Disabled = True
        txtVencimiento.Disabled = True
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
        barNuevo.Visible = True
        barCopiar.Visible = False
        gvDetalle.Columns.Item(0).Visible = False
        gvDetalle.Columns.Item(1).Visible = False
        '--------------------------------------------

        '--------------- Llenado de detalle ---------
        Sql = "SELECT A.Cuenta, A.Nombre, CONVERT(VARCHAR(50), CAST(A.Debe AS MONEY ),1) Debe, CONVERT(VARCHAR(50), CAST(A.Haber AS MONEY ),1) Haber, A.Asociado, A.Grupo
	            FROM FinAsiDet A
                WHERE A.NumDoc = '" + Session("BuscarNumDoc") + "'"
        Datos = Conf.EjecutaSql(Sql)

        Session.Add("Tabla", Datos.Tables(0))
        gvDetalle.DataSource = Session("Tabla")
        gvDetalle.DataBind()
        Totales()
        '--------------------------------------------
    End Sub

    Sub Totales()
        Dim TDebe As Decimal = 0
        Dim THaber As Decimal = 0

        For i As Integer = 0 To Session("Tabla").Rows.Count - 1
            TDebe += Decimal.Parse(Session("Tabla").Rows(i).Item("Debe"))
            THaber += Decimal.Parse(Session("Tabla").Rows(i).Item("Haber"))
        Next
        lblTDebe.InnerText = FormatNumber(Decimal.Round(TDebe, 2))
        lblTHaber.InnerText = FormatNumber(Decimal.Round(THaber, 2))
        lblTDiferencia.InnerText = FormatNumber(Decimal.Round(TDebe - THaber, 2))
    End Sub

    Protected Sub TxtBuscarBus_TextChanged()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String
        Sql = " SELECT A.NumDoc Numero, CONVERT(VARCHAR,A.FechaSis,103) [Fecha Sis], CONVERT(VARCHAR,A.FechaDoc,103) [Fecha Doc], CONVERT(VARCHAR(50), CAST(A.TotalDoc AS MONEY ),1) Total, CASE A.Estado WHEN 'C' THEN 'Cerrado' ELSE 'Abierto' END Estado
	            FROM FinAsiEnc A
                WHERE CONVERT(DATE,A.FechaSis) BETWEEN '" + txtF1Bus.Text + "' AND '" + txtF2Bus.Text + "'"
        Datos = Conf.EjecutaSql(Sql)

        GvBuscar.DataSource = Datos.Tables(0)
        GvBuscar.DataBind()
    End Sub

    Protected Sub TxtBuscar_TextChanged(sender As Object, e As EventArgs)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT TOP (100)
               [Cuenta]
              ,[Nombre]     
              ,[Saldo]
              ,[Grupo]
                  FROM [FinCuentas] A
                  WHERE A.Nivel = '5'
                  AND A.Activo = 'Y'
                  AND A.Cuenta + A.Nombre like  '%" + txtProductos.Text + "%'
                  ORDER BY A.Cuenta"
        Datos = Conf.EjecutaSql(Sql)

        If Datos.Tables(0).Rows.Count = 1 Then
            Dim Fila As DataRow = Session("Tabla").NewRow

            Fila("Cuenta") = Datos.Tables(0).Rows(0).Item("Cuenta")
            Fila("Nombre") = Datos.Tables(0).Rows(0).Item("Nombre")
            Fila("Debe") = "0.00"
            Fila("Haber") = "0.00"
            Fila("Asociado") = ""
            Fila("Grupo") = Datos.Tables(0).Rows(0).Item("Grupo")
            Session("Tabla").Rows.Add(Fila)

            Session("Tabla").DefaultView.RowFilter = "Cuenta like '%%'"
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
        txtCuenta.InnerText = ""
        txtDebe.Text = ""
        txtHaber.Text = ""
        BuscarProductos()
    End Sub

    Protected Sub TxtCopiar_TextChanged(sender As Object, e As EventArgs)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String
        Sql = " SELECT A.NumDoc Numero, CONVERT(VARCHAR,A.FechaSis,103) [Fecha Sis], CONVERT(VARCHAR,A.FechaDoc,103) [Fecha Doc], CONVERT(VARCHAR(50), CAST(A.TotalDoc AS MONEY ),1) Total, CASE A.Estado WHEN 'C' THEN 'Cerrado' ELSE 'Abierto' END Estado
	            FROM FinAsiEnc A
                WHERE CONVERT(DATE,A.FechaSis) BETWEEN '" + txtF1Copiar.Text + "' AND '" + txtF2Copiar.Text + "'"
        Datos = Conf.EjecutaSql(Sql)

        GvCopiar.DataSource = Datos.Tables(0)
        GvCopiar.DataBind()
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
        Session("Tabla").Rows(e.NewEditIndex).Item("Debe") = Session("Tabla").Rows(e.NewEditIndex).Item("Debe").ToString.Replace(",", "")
        Session("Tabla").Rows(e.NewEditIndex).Item("Haber") = Session("Tabla").Rows(e.NewEditIndex).Item("Haber").ToString.Replace(",", "")
        Session("Tabla").AcceptChanges()
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

        Session("Tabla").Rows(e.RowIndex).Item("Debe") = FormatNumber(Decimal.Parse(CType(Fila.FindControl("gvtxtDebe"), TextBox).Text.TrimEnd))
        Session("Tabla").Rows(e.RowIndex).Item("Haber") = FormatNumber(Decimal.Parse(CType(Fila.FindControl("gvtxtHaber"), TextBox).Text.TrimEnd))

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
        Session.Add("ObjectRef", "20")
        PanelCopiar.Visible = False
        PanelTotales.Visible = True
        txtComentarios.Text = "Copiado de asiento N. " + Session("CopiaNumDoc")
        LlenarCopia()
    End Sub

    Private Sub GvProductos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvProductos.RowCommand
        Dim Fila As Integer = Convert.ToInt32(e.CommandArgument)
        txtCuenta.InnerText = gvProductos.Rows(Fila).Cells(1).Text
        txtNombre.Text = gvProductos.Rows(Fila).Cells(2).Text
        txtGrupo.Text = gvProductos.Rows(Fila).Cells(4).Text
        txtDebe.Text = "0.00"
        txtHaber.Text = "0.00"
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

    Private Sub BarNuevo_ServerClick(sender As Object, e As EventArgs) Handles barNuevo.ServerClick
        Response.Redirect("FinAsi.aspx")
    End Sub

    Private Sub BarCopiar_ServerClick(sender As Object, e As EventArgs) Handles barCopiar.ServerClick
        PanelCopiar.Visible = True
        PanelTotales.Visible = False
        TxtCopiar_TextChanged(sender, e)
    End Sub

    Private Sub BtnCrear_Click(sender As Object, e As EventArgs) Handles btnCrear.Click
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Detalle As String = ""
        Dim Sql As String
        Dim Numeracion As Integer

        If Decimal.Parse(lblTDiferencia.InnerText.ToString) <> 0 Then
            lblMsg.Text = "Error: Favor cuadrar asiento, no debe tener diferencia"
            DivMSG.Attributes.Add("class", "alert alert-danger")
            Exit Sub
        End If

        If Decimal.Parse(lblTDebe.InnerText.ToString) = 0 Or Decimal.Parse(lblTHaber.InnerText.ToString) = 0 Then
            lblMsg.Text = "Error: Detalle de asiento no puede ser 0"
            DivMSG.Attributes.Add("class", "alert alert-danger")
            Exit Sub
        End If

        If dlNumeracion.SelectedItem.Text = "Manual" Then
            Numeracion = -1
        Else
            Numeracion = dlNumeracion.SelectedValue
        End If

        For I As Integer = 0 To Session("Tabla").Rows.Count - 1
            Detalle += " INSERT INTO [dbo].[FinAsiDet]
           ([NumDoc]
           ,[Cuenta]
           ,[Nombre]
           ,[Grupo]
           ,[Debe]
           ,[Haber]
           ,[SaldoPend]
           ,[Estado]
           ,[MonedaCod]
           ,[MonedaVal]
           ,[Asociado]
           ,[NumRef]
           ,[DocBase]
           ,[ObjectBase]
           ,[Fecha]
           ,[CodUsuario])
            VALUES
           (''NumSig''
           ,''" + Session("Tabla").Rows(I).Item("Cuenta") + "''
           ,''" + Session("Tabla").Rows(I).Item("Nombre") + "''
           ,''" + Session("Tabla").Rows(I).Item("Grupo") + "''
           ,''" + Replace(Session("Tabla").Rows(I).Item("Debe").ToString, ",", "") + "''   
           ,''" + Replace(Session("Tabla").Rows(I).Item("Haber").ToString, ",", "") + "'' 
           ,''0''
           ,''C''
           ,''LPS''
           ,''1''
           ,''" + Session("Tabla").Rows(I).Item("Asociado") + "''           
           ,''''
           ,''''
           ,''''
           ,''" + txtFecha.Value + "''
           ,''" + Session("Usuario_Aut") + "'') "
        Next

        Sql = " DECLARE @Impreso varchar(1) = 'N'
               ,@FechaDoc date = '" + txtFecha.Value + "'
               ,@FechaVen date = '" + txtVencimiento.Value + "'
               ,@CodUsuario varchar(50) = '" + Session("Usuario_Aut") + "'
               ,@Estado varchar(1) = 'C'
               ,@NumRef varchar(30) = '" + Session("CopiaNumDoc") + "'
               ,@MonedaCod varchar(30) = 'LPS'
               ,@MonedaVal float = '1'
               ,@TotalDoc float = '" + Replace(lblTDebe.InnerText.ToString, ",", "") + "'
               ,@SerieCod int = '" + Numeracion.ToString + "'
               ,@Comentario varchar(100) = '" + txtComentarios.Text + "'
               ,@DocBase int = ''
               ,@ObjectBase varchar(30) = ''
               ,@Detalle varchar(max) = '" + Detalle + "'
                EXECUTE [dbo].[SP_FinAsi] 
                   @Impreso
                  ,@FechaDoc
                  ,@FechaVen
                  ,@CodUsuario
                  ,@Estado
                  ,@NumRef
                  ,@MonedaCod
                  ,@MonedaVal
                  ,@TotalDoc
                  ,@SerieCod
                  ,@Comentario
                  ,@DocBase
                  ,@ObjectBase
                  ,@Detalle"
        Datos = Conf.EjecutaSql(Sql)

        If Integer.Parse(Datos.Tables(0).Rows(0).Item("Error")) = 0 Then
            Session.Add("NumDoc", Datos.Tables(0).Rows(0).Item("NumDoc").ToString)
            lblMsg.Text = Datos.Tables(0).Rows(0).Item("MSG").ToString
            DivMSG.Attributes.Add("class", "alert alert-success")
            Session("Tabla").Rows.Clear()
            Session("Tabla").AcceptChanges()
            gvDetalle.DataSource = Session("Tabla")
            gvDetalle.DataBind()
            LlenarNumeracion()
        Else
            lblMsg.Text = Datos.Tables(0).Rows(0).Item("MSG").ToString
            DivMSG.Attributes.Add("class", "alert alert-danger")
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
        txtCuenta.InnerText = ""
        txtNombre.Text = ""
        txtDebe.Text = "0.00"
        txtHaber.Text = "0.00"
        BuscarProductos()
    End Sub

    Private Sub BtnBuscarProductos_Click(sender As Object, e As EventArgs) Handles btnBuscarProductos.Click
        txtProductos.Text = txtBuscar.Text
        BuscarProductos()
    End Sub

    Private Sub BtnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        If txtCuenta.InnerText.Length > 0 Then
            Dim Fila As DataRow = Session("Tabla").NewRow

            Fila("Cuenta") = txtCuenta.InnerText
            Fila("Nombre") = txtNombre.Text
            Fila("Debe") = FormatNumber(txtDebe.Text)
            Fila("Haber") = FormatNumber(txtHaber.Text)
            Fila("Asociado") = ""
            Fila("Grupo") = txtGrupo.Text
            Session("Tabla").Rows.Add(Fila)

            gvDetalle.DataSource = Session("Tabla")
            gvDetalle.DataBind()
            Totales()

            txtCuenta.InnerText = ""
            txtNombre.Text = ""

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

End Class