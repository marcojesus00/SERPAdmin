Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing

Public Class ParGen
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

        lblMsg.Text = ""
        lblMsg.ControlStyle.CssClass = ""

        If Not IsPostBack Then
            AccesosMenu()
            Llenardatos()
        End If
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

        Tabla.DefaultView.RowFilter = "CodMenu = 'ParSer'"
        If Tabla.DefaultView.Count > 0 Then
            menuSeries.Enabled = True
        Else
            menuSeries.ToolTip = "No Tiene Acceso"
            menuSeries.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'ParUsu'"
        If Tabla.DefaultView.Count > 0 Then
            menuUsuarios.Enabled = True
        Else
            menuUsuarios.ToolTip = "No Tiene Acceso"
            menuUsuarios.Style.Value = "color:darkgrey;"
        End If
    End Sub

    Sub Llenardatos()
        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim FilePath As String = ""

        Dim Sql As String = "SELECT * FROM Empresa"
        Datos = conf.EjecutaSql(Sql)

        TxtID.Text = Datos.Tables(0).Rows(0).Item("ID")
        TxtNombre.Text = Datos.Tables(0).Rows(0).Item("Nombre")
        TxtAlias.Text = Datos.Tables(0).Rows(0).Item("Alias")
        TxtDireccion.Text = Datos.Tables(0).Rows(0).Item("Direccion")
        TxtTelefono.Text = Datos.Tables(0).Rows(0).Item("Telefono")
        TxtEmail.Text = Datos.Tables(0).Rows(0).Item("Email")
        TxtWeb.Text = Datos.Tables(0).Rows(0).Item("Web")
        chkInventario.Checked = Datos.Tables(0).Rows(0).Item("Inventario")
        DlPapel.SelectedItem.Text = Datos.Tables(0).Rows(0).Item("Papel")
        DlImpresion.SelectedItem.Text = Datos.Tables(0).Rows(0).Item("Impresion")
        TxtCopias.Text = Datos.Tables(0).Rows(0).Item("Copias")
        TxtNomImpresora.Text = Datos.Tables(0).Rows(0).Item("NomImpresora")
        txtMensaje.Text = Datos.Tables(0).Rows(0).Item("Mensaje")
        TxtTiempo.Text = Datos.Tables(0).Rows(0).Item("Tiempo")
        txtSerieComp.Text = Datos.Tables(0).Rows(0).Item("SeriePagoCompra")
        txtSerieVen.Text = Datos.Tables(0).Rows(0).Item("SeriePagoVenta")

        If Datos.Tables(0).Rows(0).Item("Logo").Equals(DBNull.Value) = False Then
            Dim imageData As Byte() = DirectCast(Datos.Tables(0).Rows(0).Item("Logo"), Byte())
            If Not imageData Is Nothing Then
                Using ms As New MemoryStream(imageData, 0, imageData.Length)
                    FilePath = Server.MapPath("~") + "\imagenes\logo.jpg"
                    Image.FromStream(ms, True).Save(FilePath, Imaging.ImageFormat.Jpeg)
                End Using
                imgFoto.Src = "~/imagenes/logo.jpg"
                imgFoto.Visible = True
            End If
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

    Private Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String = ""
        Dim Inventario As String = "0"

        If chkInventario.Checked Then
            Inventario = "1"
        End If

        Sql = "UPDATE [dbo].[Empresa]
               SET [ID] = '" + TxtID.Text + "'
                  ,[Nombre] = '" + TxtNombre.Text + "'
                  ,[Alias] = '" + TxtAlias.Text + "'
                  ,[Direccion] = '" + TxtDireccion.Text + "'
                  ,[Telefono] = '" + TxtTelefono.Text + "'
                  ,[Email] = '" + TxtEmail.Text + "'
                  ,[web] = '" + TxtWeb.Text + "'                  
                  ,[Inventario] = '" + Inventario + "'
                  ,[Papel] = '" + DlPapel.SelectedValue + "'
                  ,[Impresion] = '" + DlImpresion.SelectedValue + "'
                  ,[Copias] = '" + TxtCopias.Text + "'
                  ,[NomImpresora] = '" + TxtNomImpresora.Text + "'
                  ,[Tiempo] = '" + TxtTiempo.Text + "'
                  ,[Mensaje] = '" + txtMensaje.Text + "'
                  ,[SeriePagoVenta] = '" + txtSerieVen.Text + "'
                  ,[SeriePagoCompra] = '" + txtSerieComp.Text + "'"
        Conf.EjecutaSql(Sql)

        If FuLogo.HasFile = True Then
            If FuLogo.PostedFile.ContentLength >= 10000000 Then
                'Msg("La imagen debe ser menor a 10 Mb")
                lblMsg.Text = "La imagen debe ser menor a 10 Mb"
                lblMsg.ControlStyle.CssClass = "alert alert-danger"
            Else
                Dim FS As Stream = FuLogo.PostedFile.InputStream
                Dim BR As BinaryReader = New BinaryReader(FS)
                Dim Bytes As Byte() = BR.ReadBytes(FS.Length)
                GuardarImagen(Bytes)
            End If
        End If

        lblMsg.Text = "Se guardo correctame"
        lblMsg.ControlStyle.CssClass = "alert alert-success"

        'Msg("Se guardo correctame")
    End Sub

    Sub GuardarImagen(ByVal Imagen As Byte())
        Conector = New SqlConnection("Server=" + Servidor + "; Database=" + Bd + "; UID=" + Usuario + "; PWD=" + Clave)
        Try
            Conector.Open()
            Dim cmd As New SqlCommand("UPDATE Empresa SET Logo = @Logo", Conector)
            cmd.Parameters.Add(New SqlParameter("@Logo", SqlDbType.Image)).Value = Imagen
            cmd.ExecuteNonQuery()
            Conector.Close()
        Catch ex As Exception
            If (Conector.State = ConnectionState.Open) Then
                Conector.Close()
            End If
            Msg(ex.Message)
        End Try
    End Sub

End Class