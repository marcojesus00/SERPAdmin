Public Class ParCli
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut, Status, SuperUser As String
    Private Saldo As Decimal = 0
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
            LlenarNumeracion()
            LlenarPanelClientes()
            DlDepto_SelectedIndexChanged(sender, e)
            AccesosMenu()
            TxtF1.Text = DateTime.Now.ToString("yyyy-MM-01")
            TxtF2.Text = DateTime.Now.ToString("yyyy-MM-dd")
        End If

        If dlNumeracion.SelectedItem.Text = "Manual" Then
            txtNumeracion.Enabled = True
        Else
            txtNumeracion.Enabled = False
            txtNumeracion.Text = Session("Numeracion").Rows(dlNumeracion.SelectedIndex).Item("NumSig")
        End If
    End Sub

    Sub LlenarNumeracion()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT A.ID IdCorre, A.Descripcion, A.Prefijo + RIGHT('000000000000'+ CONVERT(VARCHAR,A.NumSig), A.Digitos) NumSig
                FROM CorrelaDet A 
				INNER JOIN CorrelaEnc B ON A.IdCorre = B.IdCorre
                WHERE A.Activo = 'Y'
                AND B.ObjectCode = '18'"
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

    Sub LlenarPanelClientes()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String = ""

        ' Departamentos
        Sql = "SELECT RTRIM(CodDepto) CodDepto, DesDepto
              FROM DeptoMuni
              GROUP BY CodDepto, DesDepto"
        Datos = Conf.EjecutaSql(Sql)

        dlDepto.DataSource = Datos.Tables(0)
        dlDepto.DataTextField = "DesDepto"
        dlDepto.DataValueField = "CodDepto"
        dlDepto.DataBind()
        '''''''''''''''''''''''''''''''''''''''

        ' Formas de pago
        Sql = "SELECT CodPago, Descripcion      
                FROM FormasPago"
        Datos = Conf.EjecutaSql(Sql)

        dlPago.DataSource = Datos.Tables(0)
        dlPago.DataTextField = "Descripcion"
        dlPago.DataValueField = "CodPago"
        dlPago.DataBind()
        ''''''''''''''''''''''''''''''''''''''

        ' Grupos de CLientes
        Sql = "SELECT IdGrupCod, Prefijo    
                FROM GrupoMaestro"
        Datos = Conf.EjecutaSql(Sql)

        dlGrupo.DataSource = Datos.Tables(0)
        dlGrupo.DataTextField = "Prefijo"
        dlGrupo.DataValueField = "IdGrupCod"
        dlGrupo.DataBind()
        ''''''''''''''''''''''''''''''''''''''

        ' Lista de Precios
        Sql = "SELECT ListCod, ListNom
                FROM ListEnc"
        Datos = Conf.EjecutaSql(Sql)

        dlPrecios.DataSource = Datos.Tables(0)
        dlPrecios.DataTextField = "ListNom"
        dlPrecios.DataValueField = "ListCod"
        dlPrecios.DataBind()
        ''''''''''''''''''''''''''''''''''''''

        ' Vendedores
        Sql = "SELECT CONVERT(VARCHAR,A.EncarCod) Codigo, EncarNom
             FROM ENCARGADO A	            
             WHERE Activo = 'Y'
             ORDER BY A.EncarCod"
        Datos = Conf.EjecutaSql(Sql)

        dlVendedor.DataSource = Datos.Tables(0)
        dlVendedor.DataTextField = "EncarNom"
        dlVendedor.DataValueField = "Codigo"
        dlVendedor.DataBind()
        ''''''''''''''''''''''''''''''''''''''''

        ' Impuestos
        Sql = "SELECT CodImpuesto, Descripcion
             FROM Impuestos"
        Datos = Conf.EjecutaSql(Sql)

        dlImpuesto.DataSource = Datos.Tables(0)
        dlImpuesto.DataTextField = "Descripcion"
        dlImpuesto.DataValueField = "CodImpuesto"
        dlImpuesto.DataBind()
        ''''''''''''''''''''''''''''''''''''''''
    End Sub

    Sub LlenarGvMov()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String
        Sql = " DECLARE @F1 DATE, @F2 DATE, @SALDO FLOAT
                SET @F1 = '" + TxtF1.Text + "'
                SET @F2 = '" + TxtF2.Text + "'

                SELECT @SALDO = SUM(A.Valor)
                FROM MaestroMov A
                WHERE A.CodigoM = '" + Session("NumSig") + "'
                AND A.FechaRef < @F1

                SELECT 0 ID,  @F1 [Fecha y hora], 'Saldo inicial' [Documento Tipo], '' Numero, ISNULL(@SALDO,0) Valor, ISNULL(@SALDO,0) Saldo, '' Usuario

                UNION ALL

                SELECT A.ID, A.FechaSys, B.Descripcion, A.NumRef, A.Valor, 0 Saldo, A.CodUsuario
                FROM MaestroMov A
                LEFT JOIN Objetos B ON A.ObjectCodeRef = B.ObjectCode
                WHERE A.CodigoM = '" + Session("NumSig") + "'
                AND A.FechaRef BETWEEN @F1 AND @F2
                ORDER BY ID "
        Datos = Conf.EjecutaSql(Sql)

        GvMov.DataSource = Datos.Tables(0)
        GvMov.DataBind()
    End Sub

    Sub AccesosMenu()
        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String = "SELECT * FROM MenuAcceso A WHERE A.CodUsuario = '" + Session("Usuario_Aut") + "' AND CodMenu LIKE 'Par%'"
        Datos = conf.EjecutaSql(Sql)
        Tabla = Datos.Tables(0)

        Tabla.DefaultView.RowFilter = "CodMenu = 'ParCli'"
        If Tabla.DefaultView.Count > 0 Then
            menuClientes.Enabled = True
        Else
            Response.Redirect(Tabla.Rows(0).Item("CodMenu") + ".aspx")
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

    Protected Sub TxtBuscarCliente_TextChanged(sender As Object, e As EventArgs)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT TOP 100 CodigoM Codigo, NombreM Nombre, NombreExt [Descripcion], Identidad, RTN, Saldo Saldo
                FROM Maestro
	            WHERE CodigoM + NombreM + NombreExt LIKE '%" + txtBuscarCliente.Text + "%'"
        Datos = Conf.EjecutaSql(Sql)

        gvClientes.DataSource = Datos.Tables(0)
        gvClientes.DataBind()
    End Sub

    Protected Sub TxtNumeracion_TextChanged(sender As Object, e As EventArgs)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String
        Session.Add("NumSig", txtNumeracion.Text)

        Sql = "SELECT *
                FROM Maestro
	            WHERE CodigoM = '" + Session("NumSig") + "'"
        Datos = Conf.EjecutaSql(Sql)

        If Datos.Tables(0).Rows.Count > 0 Then
            txtNumeracion.Enabled = False
            txtNombreCli.Value = Datos.Tables(0).Rows(0).Item("NombreM")
            txtDescripCli.Value = Datos.Tables(0).Rows(0).Item("NombreExt")
            txtIdentidad.Value = Datos.Tables(0).Rows(0).Item("Identidad")
            txtRTN.Value = Datos.Tables(0).Rows(0).Item("RTN")
            dlTipo.SelectedValue = Datos.Tables(0).Rows(0).Item("Tipo")
            dlDepto.SelectedValue = Datos.Tables(0).Rows(0).Item("Depart")
            DlDepto_SelectedIndexChanged(sender, e)
            dlMunicipio.SelectedValue = Datos.Tables(0).Rows(0).Item("Municipio")
            txtDireccion.Text = Datos.Tables(0).Rows(0).Item("Direccion")
            dlGrupo.SelectedValue = Datos.Tables(0).Rows(0).Item("GrupoCod")
            dlPago.SelectedValue = Datos.Tables(0).Rows(0).Item("CodPago")
            txtCredito.Text = Datos.Tables(0).Rows(0).Item("LimitCred")
            lblSaldo.InnerText = Datos.Tables(0).Rows(0).Item("Saldo")
            dlImpuesto.SelectedValue = Datos.Tables(0).Rows(0).Item("CodImpuesto")
            dlPrecios.SelectedValue = Datos.Tables(0).Rows(0).Item("ListCod")
            dlVendedor.SelectedValue = Datos.Tables(0).Rows(0).Item("EncarCod")
            txtTelefono.Value = Datos.Tables(0).Rows(0).Item("Telefono")
            txtCorreo.Value = Datos.Tables(0).Rows(0).Item("Correo")
            txtContacto.Value = Datos.Tables(0).Rows(0).Item("Contacto")
            dlNumeracion.SelectedValue = Datos.Tables(0).Rows(0).Item("SerieCod")
            dlEsatdo.SelectedValue = Datos.Tables(0).Rows(0).Item("Activo")
            btnGuardarCli.Text = "Actualizar"
            BtnMov.Visible = True
        Else
            btnGuardarCli.Text = "   Crear   "
            BtnMov.Visible = False
        End If
    End Sub

    Protected Sub DlDepto_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT RTRIM(CodMuni) CodMuni, DesMuni
                  FROM DeptoMuni
                  WHERE CodDepto = '" + dlDepto.SelectedValue + "'"
        Datos1 = Conf.EjecutaSql(Sql)

        dlMunicipio.DataSource = Datos1.Tables(0)
        dlMunicipio.DataTextField = "DesMuni"
        dlMunicipio.DataValueField = "CodMuni"
        dlMunicipio.DataBind()
    End Sub

    Protected Sub DlNumeracion_SelectedIndexChanged(sender As Object, e As EventArgs)
        If dlNumeracion.SelectedItem.Text = "Manual" Then
            txtNumeracion.Text = ""
        Else
            Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String

            Sql = "SELECT A.Prefijo + RIGHT('000000000000'+ CONVERT(VARCHAR,A.NumSig), A.Digitos) NumSig
                FROM CorrelaDet A 
                WHERE A.Activo = 'Y'
                AND A.Descripcion = '" + dlNumeracion.SelectedItem.Text + "'"
            Datos = Conf.EjecutaSql(Sql)

            txtNumeracion.Text = Datos.Tables(0).Rows(0).Item("NumSig")
        End If
    End Sub

    Private Sub MenuClientes_Click(sender As Object, e As EventArgs) Handles menuClientes.Click
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
        Response.Redirect("ParUsu.aspx")
    End Sub

    Private Sub MenuSeries_Click(sender As Object, e As EventArgs) Handles menuSeries.Click
        Response.Redirect("ParSer.aspx")
    End Sub

    Private Sub MenuGeneral_Click(sender As Object, e As EventArgs) Handles menuGeneral.Click
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
        btnAgregarCli.Visible = True
        btnGuardarCli.Visible = True
        imgClientes.Attributes.Add("class", "fas fa-search")
        TxtNumeracion_TextChanged(sender, e)
    End Sub

    Private Sub BtnBuscarCli_Click(sender As Object, e As EventArgs) Handles btnBuscarCli.Click
        If imgClientes.Attributes.Item("class") = "fas fa-search" Then
            imgClientes.Attributes.Add("class", "fas fa-search-minus")
            trClientes.Visible = True
            trAgregarClie.Visible = False
            btnAgregarCli.Visible = False
            btnGuardarCli.Visible = False
            TxtBuscarCliente_TextChanged(sender, e)
        Else
            imgClientes.Attributes.Add("class", "fas fa-search")
            trClientes.Visible = False
            trAgregarClie.Visible = True
            btnAgregarCli.Visible = True
            btnGuardarCli.Visible = True
        End If
    End Sub

    Private Sub BtnAgregarCli_Click(sender As Object, e As EventArgs) Handles btnAgregarCli.Click
        Response.Redirect("ParCli.aspx")
    End Sub

    Private Sub BtnGuardarCli_Click(sender As Object, e As EventArgs) Handles btnGuardarCli.Click
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String
        Dim CU As String = "C"

        If btnGuardarCli.Text = "Actualizar" Then
            CU = "U"
        End If

        If String.IsNullOrEmpty(txtCredito.Text.ToString) Or String.IsNullOrWhiteSpace(txtCredito.Text.ToString) Then
            txtCredito.Text = "0"
        End If

        Sql = "DECLARE @CU varchar(1) = '" + CU + "'
        ,@CodigoM varchar(30) = '" + Session("NumSig") + "'
        ,@NombreM varchar(100) = '" + txtNombreCli.Value + "'
        ,@Tipo varchar(1) = '" + dlTipo.SelectedValue + "'
        ,@NombreExt varchar(100) = '" + txtDescripCli.Value + "'
        ,@UsuarioCreacion varchar(50) = '" + Session("Usuario_Aut") + "'
        ,@Identidad varchar(100) = '" + txtIdentidad.Value + "'
        ,@RTN varchar(100) = '" + txtRTN.Value + "'
        ,@Direccion varchar(max) = '" + txtDireccion.Text + "'
        ,@Contacto varchar(50) = '" + txtContacto.Value + "'
        ,@Telefono varchar(50) = '" + txtTelefono.Value + "'
        ,@Correo varchar(50) = '" + txtCorreo.Value + "'
        ,@Depart varchar(2) = '" + dlDepto.SelectedValue + "'
        ,@Municipio varchar(50) = '" + dlMunicipio.SelectedValue + "'
        ,@GrupoCod int = '" + dlGrupo.SelectedValue + "'
        ,@MonedaCod int = '1'
        ,@Activo varchar(1) = '" + dlEsatdo.SelectedValue + "'
        ,@EncarCod int = '" + dlVendedor.SelectedValue + "'
        ,@IdUsuario int = '1'
        ,@LimitCred numeric(19,6) = '" + txtCredito.Text + "'
        ,@CodPago int = '" + dlPago.SelectedValue + "'
        ,@SerieCod int = '" + dlNumeracion.SelectedValue + "'
        ,@ListCod int = '" + dlPrecios.SelectedValue + "'
        ,@CodImpuesto int = '" + dlImpuesto.SelectedValue + "'
        EXECUTE [dbo].[SP_Maestro] 
        @CU, 
        @CodigoM, 
        @NombreM, 
        @Tipo, 
        @NombreExt, 
        @UsuarioCreacion, 
        @Identidad, 
        @RTN, 
        @Direccion, 
        @Contacto,
        @Telefono,
        @Correo,
        @Depart,
        @Municipio,
        @GrupoCod,
        @MonedaCod,
        @Activo,
        @EncarCod,
        @IdUsuario,
        @LimitCred, 
        @CodPago, 
        @SerieCod,
        @ListCod,
        @CodImpuesto"
        Datos = Conf.EjecutaSql(Sql)

        If Integer.Parse(Datos.Tables(0).Rows(0).Item("Error")) = 0 Then
            BtnAgregarCli_Click(sender, e)
        Else
            lblMsg.Text = "Error: " + Datos.Tables(0).Rows(0).Item("MSG").ToString
            DivMSG.Attributes.Add("class", "alert alert-danger")
        End If
    End Sub

    Private Sub BtnMov_Click(sender As Object, e As EventArgs) Handles BtnMov.Click
        trMov.Visible = True
        trAgregarClie.Visible = False
        btnAgregarCli.Visible = False
        btnGuardarCli.Visible = False
        imgClientes.Visible = False

        LlenarGvMov()
    End Sub

    Private Sub BtnBuscarMov_Click(sender As Object, e As EventArgs) Handles btnBuscarMov.Click
        LlenarGvMov()
    End Sub

    Private Sub BtnCancelarMov_Click(sender As Object, e As EventArgs) Handles btnCancelarMov.Click
        trMov.Visible = False
        trAgregarClie.Visible = True
        btnAgregarCli.Visible = True
        btnGuardarCli.Visible = True
        imgClientes.Visible = True
    End Sub

    Private Sub GvMov_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GvMov.RowDataBound
        If ((e.Row.RowType = DataControlRowType.DataRow) And (e.Row.RowType <> DataControlRowType.EmptyDataRow)) Then
            If Decimal.Parse(e.Row.Cells(4).Text) < 0 Then
                e.Row.ControlStyle.ForeColor = System.Drawing.Color.Green
            End If
            If e.Row.RowIndex = 0 Then
                e.Row.ControlStyle.ForeColor = System.Drawing.Color.Blue
            End If
            Saldo += Decimal.Parse(e.Row.Cells(4).Text)
            e.Row.Cells(5).Text = Saldo.ToString
        End If
    End Sub

End Class