Imports System.Data.SqlClient
Imports System.Xml

Public Class iniciomovil
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Status As String
    Private Datos As DataSet
    Public Tabla As DataTable = New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario_Aut") = "" Then
            PanelUsuario.Visible = True
        End If
        'dlDB.Items.Add("SERP")
        'dlDB.SelectedIndex = 0

        LeerXml()

        Session.Add("Usuario", Usuario)
        Session.Add("Clave", Clave)
        Session.Add("Bd", Bd)
        Session.Add("Sevidor", Servidor)

        If Not IsPostBack Then
            AccesosMenu()
        End If
    End Sub

    Sub LeerXml()
        Try
            Dim m_xmld As XmlDocument
            Dim m_nodelist As XmlNodeList
            Dim m_node As XmlNode

            'Creamos el "Document"
            m_xmld = New XmlDocument()

            'Cargamos el archivo
            m_xmld.Load("C:\inetpub\wwwroot\serp\conf.xml")

            'Obtenemos la lista de los nodos "name"
            m_nodelist = m_xmld.SelectNodes("/parametros/name")

            'Iniciamos el ciclo de lectura
            For Each m_node In m_nodelist
                ''Obtenemos el atributo del codigo
                'Dim mCodigo = m_node.Attributes.GetNamedItem("codigo").Value

                Servidor = m_node.ChildNodes.Item(0).InnerText
                Usuario = m_node.ChildNodes.Item(1).InnerText
                Clave = m_node.ChildNodes.Item(2).InnerText
                Bd = m_node.ChildNodes.Item(3).InnerText
                'Msg(Server.MapPath("~") + "conf.xml")
            Next
        Catch ex As Exception
            'Error trapping
            Msg(ex.ToString())
        End Try

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

        Tabla.DefaultView.RowFilter = "CodMenu like 'Par%'"
        If Tabla.DefaultView.Count > 0 Then
            btnParametros.Enabled = True
            btnParametros.ToolTip = ""
            btnParametros.Style.Value = "color:blue;"
        Else
            btnParametros.ToolTip = "No Tiene Acceso"
            btnParametros.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'Clave'"
        If Tabla.DefaultView.Count > 0 Then
            btnUsuario.Enabled = True
            btnUsuario.ToolTip = ""
            btnUsuario.Style.Value = "color:blue;"
        Else
            btnUsuario.ToolTip = "No Tiene Acceso"
            btnUsuario.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'VenPed'"
        If Tabla.DefaultView.Count > 0 Then
            btnVenPed.Enabled = True
            btnVenPed.ToolTip = ""
            btnVenPed.Style.Value = "color:blue;"
        Else
            btnVenPed.ToolTip = "No Tiene Acceso"
            btnVenPed.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'VenFac'"
        If Tabla.DefaultView.Count > 0 Then
            btnVenFac.Enabled = True
            btnVenFac.ToolTip = ""
            btnVenFac.Style.Value = "color:blue;"
        Else
            btnVenFac.ToolTip = "No Tiene Acceso"
            btnVenFac.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'VenNC'"
        If Tabla.DefaultView.Count > 0 Then
            btnVenNC.Enabled = True
            btnVenNC.ToolTip = ""
            btnVenNC.Style.Value = "color:blue;"
        Else
            btnVenNC.ToolTip = "No Tiene Acceso"
            btnVenNC.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'VenCaj'"
        If Tabla.DefaultView.Count > 0 Then
            btnVenCaj.Enabled = True
            btnVenCaj.ToolTip = ""
            btnVenCaj.Style.Value = "color:blue;"
        Else
            btnVenCaj.ToolTip = "No Tiene Acceso"
            btnVenCaj.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'ComPed'"
        If Tabla.DefaultView.Count > 0 Then
            btnComPed.Enabled = True
            btnComPed.ToolTip = ""
            btnComPed.Style.Value = "color:blue;"
        Else
            btnComPed.ToolTip = "No Tiene Acceso"
            btnComPed.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'ComFac'"
        If Tabla.DefaultView.Count > 0 Then
            btnComFac.Enabled = True
            btnComFac.ToolTip = ""
            btnComFac.Style.Value = "color:blue;"
        Else
            btnComFac.ToolTip = "No Tiene Acceso"
            btnComFac.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'ComNC'"
        If Tabla.DefaultView.Count > 0 Then
            btnComNC.Enabled = True
            btnComNC.ToolTip = ""
            btnComNC.Style.Value = "color:blue;"
        Else
            btnComNC.ToolTip = "No Tiene Acceso"
            btnComNC.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'InvEnt'"
        If Tabla.DefaultView.Count > 0 Then
            btnInvEnt.Enabled = True
            btnInvEnt.ToolTip = ""
            btnInvEnt.Style.Value = "color:blue;"
        Else
            btnInvEnt.ToolTip = "No Tiene Acceso"
            btnInvEnt.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'InvSal'"
        If Tabla.DefaultView.Count > 0 Then
            btnInvSal.Enabled = True
            btnInvSal.ToolTip = ""
            btnInvSal.Style.Value = "color:blue;"
        Else
            btnInvSal.ToolTip = "No Tiene Acceso"
            btnInvSal.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'InvTra'"
        If Tabla.DefaultView.Count > 0 Then
            btnInvTra.Enabled = True
            btnInvTra.ToolTip = ""
            btnInvTra.Style.Value = "color:blue;"
        Else
            btnInvTra.ToolTip = "No Tiene Acceso"
            btnInvTra.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'InvCon'"
        If Tabla.DefaultView.Count > 0 Then
            btnInvCon.Enabled = True
            btnInvCon.ToolTip = ""
            btnInvCon.Style.Value = "color:blue;"
        Else
            btnInvCon.ToolTip = "No Tiene Acceso"
            btnInvCon.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'BanRec'"
        If Tabla.DefaultView.Count > 0 Then
            btnBanRec.Enabled = True
            btnBanRec.ToolTip = ""
            btnBanRec.Style.Value = "color:blue;"
        Else
            btnBanRec.ToolTip = "No Tiene Acceso"
            btnBanRec.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'BanEfe'"
        If Tabla.DefaultView.Count > 0 Then
            btnBanEfe.Enabled = True
            btnBanEfe.ToolTip = ""
            btnBanEfe.Style.Value = "color:blue;"
        Else
            btnBanEfe.ToolTip = "No Tiene Acceso"
            btnBanEfe.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'BanDep'"
        If Tabla.DefaultView.Count > 0 Then
            btnBanDep.Enabled = True
            btnBanDep.ToolTip = ""
            btnBanDep.Style.Value = "color:blue;"
        Else
            btnBanDep.ToolTip = "No Tiene Acceso"
            btnBanDep.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'RepExi'"
        If Tabla.DefaultView.Count > 0 Then
            btnRepExi.Enabled = True
            btnRepExi.ToolTip = ""
            btnRepExi.Style.Value = "color:blue;"
        Else
            btnRepExi.ToolTip = "No Tiene Acceso"
            btnRepExi.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'RepKar'"
        If Tabla.DefaultView.Count > 0 Then
            btnRepKar.Enabled = True
            btnRepKar.ToolTip = ""
            btnRepKar.Style.Value = "color:blue;"
        Else
            btnRepKar.ToolTip = "No Tiene Acceso"
            btnRepKar.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'RepVen'"
        If Tabla.DefaultView.Count > 0 Then
            btnRepVen.Enabled = True
            btnRepVen.ToolTip = ""
            btnRepVen.Style.Value = "color:blue;"
        Else
            btnRepVen.ToolTip = "No Tiene Acceso"
            btnRepVen.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'RepCom'"
        If Tabla.DefaultView.Count > 0 Then
            btnRepCom.Enabled = True
            btnRepCom.ToolTip = ""
            btnRepCom.Style.Value = "color:blue;"
        Else
            btnRepCom.ToolTip = "No Tiene Acceso"
            btnRepCom.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'RepCxc'"
        If Tabla.DefaultView.Count > 0 Then
            btnRepCXC.Enabled = True
            btnRepCXC.ToolTip = ""
            btnRepCXC.Style.Value = "color:blue;"
        Else
            btnRepCXC.ToolTip = "No Tiene Acceso"
            btnRepCXC.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'RepCxp'"
        If Tabla.DefaultView.Count > 0 Then
            btnRepCXP.Enabled = True
            btnRepCXP.ToolTip = ""
            btnRepCXP.Style.Value = "color:blue;"
        Else
            btnRepCXP.ToolTip = "No Tiene Acceso"
            btnRepCXP.Style.Value = "color:darkgrey;"
        End If
    End Sub

    Private Sub btnAceptarUsu_Click(sender As Object, e As EventArgs) Handles btnAceptarUsu.Click
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT TOP 1 CodUsuario, Pass, Nombre, Cargo, Sucursal, Funcion FROM Usuario WHERE CodUsuario = '" & txtUsuario.Text & "' AND Pass= '" + txtClave.Text + "'"
        Datos = Conf.EjecutaSql(Sql)

        If Datos.Tables(0).Rows.Count = 0 Then
            Msg("Usuario o Clave incorrecto")
            Session.Add("Usuario_Aut", "")
            Exit Sub
        End If

        Session.Add("Usuario_Aut", txtUsuario.Text)
        Session.Add("Funcion", Datos.Tables(0).Rows(0).Item("Funcion"))
        Session.Add("Bd", Bd)

        AccesosMenu()

        Sql = "SELECT *
                FROM Empresa"
        Datos = Conf.EjecutaSql(Sql)

        Session.Add("Titulo", Datos.Tables(0).Rows(0).Item("Nombre"))
        Session.Add("Mensaje", Datos.Tables(0).Rows(0).Item("Mensaje"))
        Session.Add("Tiempo", Datos.Tables(0).Rows(0).Item("Tiempo"))
        Session.Timeout = Session("Tiempo")
        PanelUsuario.Visible = False
    End Sub

    Private Sub btnVenFac_Click(sender As Object, e As EventArgs) Handles btnVenFac.Click
        Session.Add("Destino", "VenFac.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub btnVenNC_Click(sender As Object, e As EventArgs) Handles btnVenNC.Click
        Session.Add("Destino", "VenNC.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub btnVenPed_Click(sender As Object, e As EventArgs) Handles btnVenPed.Click
        Session.Add("Destino", "VenPed.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub btnComFac_Click(sender As Object, e As EventArgs) Handles btnComFac.Click
        Session.Add("Destino", "ComFac.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub btnComNC_Click(sender As Object, e As EventArgs) Handles btnComNC.Click
        Session.Add("Destino", "ComNC.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub btnComPed_Click(sender As Object, e As EventArgs) Handles btnComPed.Click
        Session.Add("Destino", "ComPed.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub btnInvEnt_Click(sender As Object, e As EventArgs) Handles btnInvEnt.Click
        Session.Add("Destino", "InvEnt.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub btnInvSal_Click(sender As Object, e As EventArgs) Handles btnInvSal.Click
        Session.Add("Destino", "InvSal.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub btnInvTra_Click(sender As Object, e As EventArgs) Handles btnInvTra.Click
        Session.Add("Destino", "InvTra.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub btnBanRec_Click(sender As Object, e As EventArgs) Handles btnBanRec.Click
        Session.Add("Destino", "BanRec.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub btnRepExi_Click(sender As Object, e As EventArgs) Handles btnRepExi.Click
        Session.Add("Destino", "RepExi.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub btnRepKar_Click(sender As Object, e As EventArgs) Handles btnRepKar.Click
        Session.Add("Destino", "RepKar.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub btnRepVen_Click(sender As Object, e As EventArgs) Handles btnRepVen.Click
        Session.Add("Destino", "RepVen.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub btnRepCom_Click(sender As Object, e As EventArgs) Handles btnRepCom.Click
        Session.Add("Destino", "RepCom.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub BtnRepCXC_Click(sender As Object, e As EventArgs) Handles btnRepCXC.Click
        Session.Add("Destino", "RepCxc.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub BtnRepCXP_Click(sender As Object, e As EventArgs) Handles btnRepCXP.Click
        Session.Add("Destino", "RepCxp.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub btnUsuario_Click(sender As Object, e As EventArgs) Handles btnUsuario.Click
        PanelCambioPass.Visible = True
        txtCamUsuario.Text = Session("Usuario_Aut")
    End Sub

    Private Sub btnParametros_Click(sender As Object, e As EventArgs) Handles btnParametros.Click
        Session.Add("Destino", "ParCli.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub btnCancelarPass_ServerClick(sender As Object, e As EventArgs) Handles btnCancelarPass.ServerClick
        PanelCambioPass.Visible = False
    End Sub

    Private Sub btnAceptarPass_Click(sender As Object, e As EventArgs) Handles btnAceptarPass.Click
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT TOP 1 CodUsuario, Pass, Nombre, Cargo, Sucursal, Funcion FROM Usuario WHERE CodUsuario = '" & txtCamUsuario.Text & "' AND Pass= '" + txtCamPass.Text + "'"
        Try
            Datos = Conf.EjecutaSql(Sql)

            If Datos.Tables(0).Rows.Count = 0 Then
                Msg("Usuario o Clave incorrecto")
                Session.Add("Usuario_Aut", "")
                Exit Sub
            End If
        Catch ex As Exception
            Msg(ex.Message.ToString() + " - " + ex.Source.ToString())
        End Try
        If txtCamPass1.Text = txtCamPass2.Text Then
            Sql = "UPDATE Usuario SET PASS = '" + txtCamPass1.Text + "' WHERE CodUsuario = '" + txtCamUsuario.Text + "'"
            Conf.EjecutaSql(Sql)
            PanelCambioPass.Visible = False
        Else
            Msg("la clave nueva no coincide")
        End If
    End Sub

    Private Sub btnVersion_Click(sender As Object, e As EventArgs) Handles btnVersion.Click
        PanelVersion.Visible = True
    End Sub

    Private Sub btnAceptarVer_Click(sender As Object, e As EventArgs) Handles btnAceptarVer.Click
        PanelVersion.Visible = False
    End Sub

    Private Sub btnLogOut_Click(sender As Object, e As EventArgs) Handles btnLogOut.Click
        Session.Add("Usuario_Aut", "")
        Response.Redirect("inicio.aspx")
    End Sub

End Class