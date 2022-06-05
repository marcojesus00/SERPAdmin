Imports System.Xml

Public Class Inicio
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Status As String
    Private Datos As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario_Aut") = "" Then
            PanelUsuario.Visible = True
        End If

        LeerXml()

        Session.Add("Usuario", Usuario)
        Session.Add("Clave", Clave)
        Session.Add("Bd", Bd)
        Session.Add("Sevidor", Servidor)
        lblEstacion.Text = Session("Titulo")
        LblMensaje.InnerText = Session("Mensaje")

        LlenarGrafica()

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
            'm_xmld.Load("C:\inetpub\wwwroot\serp\conf.xml")
            m_xmld.Load(Server.MapPath("conf.xml"))

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
                lblBD.Text = Bd
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

        Dim Tabla As DataTable = Datos.Tables(0)

        Tabla.DefaultView.RowFilter = "CodMenu like 'Par%'"
        If Tabla.DefaultView.Count > 0 Then
            btnParametros.Enabled = True
            btnParametros.ToolTip = ""
            btnParametros.Style.Value = "color:blue;"
        Else
            btnParametros.ToolTip = "No tiene acceso"
            btnParametros.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'Clave'"
        If Tabla.DefaultView.Count > 0 Then
            btnUsuario.Enabled = True
            btnUsuario.ToolTip = ""
            btnUsuario.Style.Value = "color:blue;"
        Else
            btnUsuario.ToolTip = "No tiene acceso"
            btnUsuario.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'VenPed'"
        If Tabla.DefaultView.Count > 0 Then
            btnVenPed.Enabled = True
            btnVenPed.ToolTip = ""
            btnVenPed.Style.Value = "color:blue;"
        Else
            btnVenPed.ToolTip = "No tiene acceso"
            btnVenPed.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'VenFac'"
        If Tabla.DefaultView.Count > 0 Then
            btnVenFac.Enabled = True
            btnVenFac.ToolTip = ""
            btnVenFac.Style.Value = "color:blue;"
        Else
            btnVenFac.ToolTip = "No tiene acceso"
            btnVenFac.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'VenNC'"
        If Tabla.DefaultView.Count > 0 Then
            btnVenNC.Enabled = True
            btnVenNC.ToolTip = ""
            btnVenNC.Style.Value = "color:blue;"
        Else
            btnVenNC.ToolTip = "No tiene acceso"
            btnVenNC.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'VenCaj'"
        If Tabla.DefaultView.Count > 0 Then
            btnVenCaj.Enabled = True
            btnVenCaj.ToolTip = ""
            btnVenCaj.Style.Value = "color:blue;"
        Else
            btnVenCaj.ToolTip = "No tiene acceso"
            btnVenCaj.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'ComPed'"
        If Tabla.DefaultView.Count > 0 Then
            btnComPed.Enabled = True
            btnComPed.ToolTip = ""
            btnComPed.Style.Value = "color:blue;"
        Else
            btnComPed.ToolTip = "No tiene acceso"
            btnComPed.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'ComFac'"
        If Tabla.DefaultView.Count > 0 Then
            btnComFac.Enabled = True
            btnComFac.ToolTip = ""
            btnComFac.Style.Value = "color:blue;"
        Else
            btnComFac.ToolTip = "No tiene acceso"
            btnComFac.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'ComNC'"
        If Tabla.DefaultView.Count > 0 Then
            btnComNC.Enabled = True
            btnComNC.ToolTip = ""
            btnComNC.Style.Value = "color:blue;"
        Else
            btnComNC.ToolTip = "No tiene acceso"
            btnComNC.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'InvEnt'"
        If Tabla.DefaultView.Count > 0 Then
            btnInvEnt.Enabled = True
            btnInvEnt.ToolTip = ""
            btnInvEnt.Style.Value = "color:blue;"
        Else
            btnInvEnt.ToolTip = "No tiene acceso"
            btnInvEnt.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'InvSal'"
        If Tabla.DefaultView.Count > 0 Then
            btnInvSal.Enabled = True
            btnInvSal.ToolTip = ""
            btnInvSal.Style.Value = "color:blue;"
        Else
            btnInvSal.ToolTip = "No tiene acceso"
            btnInvSal.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'InvTra'"
        If Tabla.DefaultView.Count > 0 Then
            btnInvTra.Enabled = True
            btnInvTra.ToolTip = ""
            btnInvTra.Style.Value = "color:blue;"
        Else
            btnInvTra.ToolTip = "No tiene acceso"
            btnInvTra.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'InvCon'"
        If Tabla.DefaultView.Count > 0 Then
            btnInvCon.Enabled = True
            btnInvCon.ToolTip = ""
            btnInvCon.Style.Value = "color:blue;"
        Else
            btnInvCon.ToolTip = "No tiene acceso"
            btnInvCon.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'BanRec'"
        If Tabla.DefaultView.Count > 0 Then
            btnBanRec.Enabled = True
            btnBanRec.ToolTip = ""
            btnBanRec.Style.Value = "color:blue;"
        Else
            btnBanRec.ToolTip = "No tiene acceso"
            btnBanRec.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'BanEfe'"
        If Tabla.DefaultView.Count > 0 Then
            btnBanEfe.Enabled = True
            btnBanEfe.ToolTip = ""
            btnBanEfe.Style.Value = "color:blue;"
        Else
            btnBanEfe.ToolTip = "No tiene acceso"
            btnBanEfe.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'BanDep'"
        If Tabla.DefaultView.Count > 0 Then
            btnBanDep.Enabled = True
            btnBanDep.ToolTip = ""
            btnBanDep.Style.Value = "color:blue;"
        Else
            btnBanDep.ToolTip = "No tiene acceso"
            btnBanDep.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'FinPer'"
        If Tabla.DefaultView.Count > 0 Then
            btnFinPer.Enabled = True
            btnFinPer.ToolTip = ""
            btnFinPer.Style.Value = "color:blue;"
        Else
            btnFinPer.ToolTip = "No tiene acceso"
            btnFinPer.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'FinPla'"
        If Tabla.DefaultView.Count > 0 Then
            btnFinPla.Enabled = True
            btnFinPla.ToolTip = ""
            btnFinPla.Style.Value = "color:blue;"
        Else
            btnFinPla.ToolTip = "No tiene acceso"
            btnFinPla.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'FinAsi'"
        If Tabla.DefaultView.Count > 0 Then
            btnFinAsi.Enabled = True
            btnFinAsi.ToolTip = ""
            btnFinAsi.Style.Value = "color:blue;"
        Else
            btnFinAsi.ToolTip = "No tiene acceso"
            btnFinAsi.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'FinBal'"
        If Tabla.DefaultView.Count > 0 Then
            btnFinBal.Enabled = True
            btnFinBal.ToolTip = ""
            btnFinBal.Style.Value = "color:blue;"
        Else
            btnFinBal.ToolTip = "No tiene acceso"
            btnFinBal.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'FinEst'"
        If Tabla.DefaultView.Count > 0 Then
            btnFinEst.Enabled = True
            btnFinEst.ToolTip = ""
            btnFinEst.Style.Value = "color:blue;"
        Else
            btnFinEst.ToolTip = "No tiene acceso"
            btnFinEst.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'RepExi'"
        If Tabla.DefaultView.Count > 0 Then
            btnRepExi.Enabled = True
            btnRepExi.ToolTip = ""
            btnRepExi.Style.Value = "color:blue;"
        Else
            btnRepExi.ToolTip = "No tiene acceso"
            btnRepExi.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'RepKar'"
        If Tabla.DefaultView.Count > 0 Then
            btnRepKar.Enabled = True
            btnRepKar.ToolTip = ""
            btnRepKar.Style.Value = "color:blue;"
        Else
            btnRepKar.ToolTip = "No tiene acceso"
            btnRepKar.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'RepVen'"
        If Tabla.DefaultView.Count > 0 Then
            btnRepVen.Enabled = True
            btnRepVen.ToolTip = ""
            btnRepVen.Style.Value = "color:blue;"
        Else
            btnRepVen.ToolTip = "No tiene acceso"
            btnRepVen.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'RepCom'"
        If Tabla.DefaultView.Count > 0 Then
            btnRepCom.Enabled = True
            btnRepCom.ToolTip = ""
            btnRepCom.Style.Value = "color:blue;"
        Else
            btnRepCom.ToolTip = "No tiene acceso"
            btnRepCom.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'RepCxc'"
        If Tabla.DefaultView.Count > 0 Then
            btnRepCXC.Enabled = True
            btnRepCXC.ToolTip = ""
            btnRepCXC.Style.Value = "color:blue;"
        Else
            btnRepCXC.ToolTip = "No tiene acceso"
            btnRepCXC.Style.Value = "color:darkgrey;"
        End If

        Tabla.DefaultView.RowFilter = "CodMenu = 'RepCxp'"
        If Tabla.DefaultView.Count > 0 Then
            btnRepCXP.Enabled = True
            btnRepCXP.ToolTip = ""
            btnRepCXP.Style.Value = "color:blue;"
        Else
            btnRepCXP.ToolTip = "No tiene acceso"
            btnRepCXP.Style.Value = "color:darkgrey;"
        End If
    End Sub

    Sub LlenarGrafica()
        Dim Sql As String

        Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Sql = " SELECT B.EncarNom Vendedor, SUM(A.TotalDoc - A.TotalImp) Venta, CONVERT(VARCHAR(50), CAST(SUM(A.TotalDoc - A.TotalImp) AS MONEY ),1) Valor
             FROM VenFacEnc A
             INNER JOIN Encargado B ON A.EncarCod = B.EncarCod
             WHERE DATEPART(YEAR,A.FechaDoc) = DATEPART(YEAR,GETDATE())
			 AND DATEPART(MONTH,A.FechaDoc) = DATEPART(MONTH,GETDATE()) 
             GROUP BY B.EncarNom
             ORDER BY SUM(A.TotalDoc - A.TotalImp) DESC "
        Datos = conf.EjecutaSql(Sql)

        Dim Filas As Integer
        Filas = Datos.Tables(0).Rows.Count

        If Filas >= 0 Then
            Dim A As Integer
            If Filas > 10 Then
                A = 10
            Else
                A = Filas - 1
            End If

            For i As Integer = 0 To A
                GraTopVen.Series(0).Points.AddXY(Datos.Tables(0).Rows(i).Item("Vendedor"), Datos.Tables(0).Rows(i).Item("Venta"))
                GraTopVen.Series(0).Points(i).Label = (i + 1).ToString
            Next

            GraTopVen.Series(0).Font = New Drawing.Font(GraTopVen.Series(0).Font.Name, 12)
            'GraTopVen.ChartAreas(0).Area3DStyle.Enable3D = True
            'GraTopVen.Series(0).LabelForeColor = Drawing.Color.DarkBlue
            GraTopVen.ChartAreas(0).AxisX.LabelStyle.Font = New Drawing.Font(GraTopVen.Series(0).Font.Name, 10)
            GraTopVen.Series(0).Palette = DataVisualization.Charting.ChartColorPalette.BrightPastel
            GraTopVen.Series(0).SmartLabelStyle.Enabled = False
            'GraTopVen.Series(0).LabelAngle = -45
            GraTopVen.ChartAreas(0).AxisX.Interval = 1
            GraTopVen.Width = "400"
            GraTopVen.Height = "200"
        End If
    End Sub

    Private Sub BtnAceptarUsu_Click(sender As Object, e As EventArgs) Handles btnAceptarUsu.Click
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT TOP 1 CodUsuario, Pass, Nombre, Cargo, Sucursal, Funcion FROM Usuario WHERE CodUsuario = '" & txtUsuario.Text & "' AND CONVERT(VARCHAR,DECRYPTBYPASSPHRASE('serp',Pass)) collate Chinese_PRC_CS_AS_WS  = '" + txtClave.Text + "'"
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
        lblEstacion.Text = Session("Titulo")
        LblMensaje.InnerText = Session("Mensaje")
        PanelUsuario.Visible = False
    End Sub

    Private Sub BtnVenFac_Click(sender As Object, e As EventArgs) Handles btnVenFac.Click
        Session.Add("Destino", "VenFac.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub BtnVenNC_Click(sender As Object, e As EventArgs) Handles btnVenNC.Click
        Session.Add("Destino", "VenNC.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub BtnVenPed_Click(sender As Object, e As EventArgs) Handles btnVenPed.Click
        Session.Add("Destino", "VenPed.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub BtnComFac_Click(sender As Object, e As EventArgs) Handles btnComFac.Click
        Session.Add("Destino", "ComFac.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub BtnComNC_Click(sender As Object, e As EventArgs) Handles btnComNC.Click
        Session.Add("Destino", "ComNC.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub BtnComPed_Click(sender As Object, e As EventArgs) Handles btnComPed.Click
        Session.Add("Destino", "ComPed.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub BtnInvEnt_Click(sender As Object, e As EventArgs) Handles btnInvEnt.Click
        Session.Add("Destino", "InvEnt.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub BtnInvSal_Click(sender As Object, e As EventArgs) Handles btnInvSal.Click
        Session.Add("Destino", "InvSal.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub BtnInvTra_Click(sender As Object, e As EventArgs) Handles btnInvTra.Click
        Session.Add("Destino", "InvTra.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub BtnBanRec_Click(sender As Object, e As EventArgs) Handles btnBanRec.Click
        Session.Add("Destino", "BanRec.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub BtnBanEfe_Click(sender As Object, e As EventArgs) Handles btnBanEfe.Click
        Session.Add("Destino", "BanEfe.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub BtnFinPla_Click(sender As Object, e As EventArgs) Handles btnFinPla.Click
        Session.Add("Destino", "FinPla.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub BtnFinAsi_Click(sender As Object, e As EventArgs) Handles btnFinAsi.Click
        Session.Add("Destino", "FinAsi.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub BtnFinPer_Click(sender As Object, e As EventArgs) Handles btnFinPer.Click
        Session.Add("Destino", "FinPer.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub BtnRepExi_Click(sender As Object, e As EventArgs) Handles btnRepExi.Click
        Session.Add("Destino", "RepExi.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub BtnRepKar_Click(sender As Object, e As EventArgs) Handles btnRepKar.Click
        Session.Add("Destino", "RepKar.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub BtnRepVen_Click(sender As Object, e As EventArgs) Handles btnRepVen.Click
        Session.Add("Destino", "RepVen.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub BtnRepCom_Click(sender As Object, e As EventArgs) Handles btnRepCom.Click
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

    Private Sub BtnUsuario_Click(sender As Object, e As EventArgs) Handles btnUsuario.Click
        PanelCambioPass.Visible = True
        txtCamUsuario.Text = Session("Usuario_Aut")
    End Sub

    Private Sub BtnParametros_Click(sender As Object, e As EventArgs) Handles btnParametros.Click
        Session.Add("Destino", "ParCli.aspx")
        Response.Redirect(Session("Destino"))
    End Sub

    Private Sub BtnCancelarPass_ServerClick(sender As Object, e As EventArgs) Handles btnCancelarPass.ServerClick
        PanelCambioPass.Visible = False
    End Sub

    Private Sub BtnAceptarPass_Click(sender As Object, e As EventArgs) Handles btnAceptarPass.Click
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT TOP 1 CodUsuario, Pass, Nombre, Cargo, Sucursal, Funcion FROM Usuario WHERE CodUsuario = '" & txtCamUsuario.Text & "' AND CONVERT(VARCHAR,DECRYPTBYPASSPHRASE('serp',Pass)) collate Chinese_PRC_CS_AS_WS  = '" + txtCamPass.Text + "'"
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
        If txtCamPass1.Text.ToString.Equals(txtCamPass2.Text) Then
            Sql = "UPDATE Usuario SET pass = ENCRYPTBYPASSPHRASE('serp','" + txtCamPass1.Text + "') WHERE CodUsuario = '" + txtCamUsuario.Text + "'"
            Conf.EjecutaSql(Sql)
            PanelCambioPass.Visible = False
        Else
            Msg("la clave nueva no coincide")
        End If
    End Sub

    Private Sub BtnVersion_Click(sender As Object, e As EventArgs) Handles btnVersion.Click
        PanelVersion.Visible = True
    End Sub

    Private Sub BtnAceptarVer_Click(sender As Object, e As EventArgs) Handles btnAceptarVer.Click
        PanelVersion.Visible = False
    End Sub

    Private Sub BtnLogOut_Click(sender As Object, e As EventArgs) Handles btnLogOut.Click
        Session.Add("Usuario_Aut", "")
        Response.Redirect("inicio.aspx")
    End Sub

End Class