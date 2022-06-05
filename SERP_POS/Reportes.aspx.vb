Public Class Reportes

    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Reporte, Imprimir As String
    Private Datos As DataSet
    Public Informe As New CrystalDecisions.CrystalReports.Engine.ReportDocument

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario") = "" Then
            Response.Redirect("inicio.aspx")
        End If
        Session.Timeout = Session("Tiempo")

        Informe.Close()
        Informe.Dispose()

        Imprimir = "No"
        lblMsg.Text = ""
        Usuario = Session("Usuario")
        Clave = Session("Clave")
        Servidor = Session("Sevidor")
        Bd = Session("Bd")

        If Session("Destino") = "VenPed.aspx" Then
            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String
            Sql = " EXEC CR_VenPed '" + Session("NumDoc") + "' "
            Datos = conf.EjecutaSql(Sql)

            Informe = New PedVen
            ReporteCR(Informe, "PedVen_" + Session("NumDoc"), Datos)
        End If

        If Session("Destino") = "VenFac.aspx" Then
            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String
            Sql = " EXEC CR_VenFac '" + Session("NumDoc") + "' "
            Datos = conf.EjecutaSql(Sql)

            Imprimir = "Si"
            Informe = New FacVen
            ReporteCR(Informe, "FacVen_" + Session("NumDoc"), Datos)
        End If

        If Session("Destino") = "VenNC.aspx" Then
            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String
            Sql = " EXEC CR_VenNC '" + Session("NumDoc") + "' "
            Datos = conf.EjecutaSql(Sql)

            Informe = New NCVen
            ReporteCR(Informe, "NCVen_" + Session("NumDoc"), Datos)
        End If

        If Session("Destino") = "ComPed.aspx" Then
            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String
            Sql = " EXEC CR_ComPed '" + Session("NumDoc") + "' "
            Datos = conf.EjecutaSql(Sql)

            Informe = New PedCom
            ReporteCR(Informe, "PedCom_" + Session("NumDoc"), Datos)
        End If

        If Session("Destino") = "ComFac.aspx" Then
            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String
            Sql = " EXEC CR_ComFac '" + Session("NumDoc") + "' "
            Datos = conf.EjecutaSql(Sql)

            Informe = New FacCom
            ReporteCR(Informe, "FacCom_" + Session("NumDoc"), Datos)
        End If

        If Session("Destino") = "ComNC.aspx" Then
            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String
            Sql = " EXEC CR_ComNC '" + Session("NumDoc") + "' "
            Datos = conf.EjecutaSql(Sql)

            Informe = New NcCom
            ReporteCR(Informe, "NCCom_" + Session("NumDoc"), Datos)
        End If

        If Session("Destino") = "InvEnt.aspx" Then
            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String
            Sql = " EXEC CR_InvEnt '" + Session("NumDoc") + "' "
            Datos = conf.EjecutaSql(Sql)

            Informe = New EntInv
            ReporteCR(Informe, "InvEnt_" + Session("NumDoc"), Datos)
        End If

        If Session("Destino") = "InvSal.aspx" Then
            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String
            Sql = " EXEC CR_InvSal '" + Session("NumDoc") + "' "
            Datos = conf.EjecutaSql(Sql)

            Informe = New SalInv
            ReporteCR(Informe, "InvSal_" + Session("NumDoc"), Datos)
        End If

        If Session("Destino") = "RepExi.aspx" Then
            Datos = Session("GV")

            Informe = New ExiRep
            ReporteCR(Informe, "Existencias", Datos)
        End If

        If Session("Destino") = "RepKar.aspx" Then
            Datos = Session("GV")

            Informe = New KarRep
            ReporteCR(Informe, "Kardex", Datos)
        End If

        If Session("Destino") = "RepVen.aspx" Then
            Datos = Session("GVR")

            Informe = New VenRep
            ReporteCR(Informe, "Ventas", Datos)
        End If

        If Session("Destino") = "RepCom.aspx" Then
            Datos = Session("GVR")

            Informe = New ComRep
            ReporteCR(Informe, "Compras", Datos)
        End If

    End Sub

    Sub ReporteCR(Informe As CrystalDecisions.CrystalReports.Engine.ReportDocument, Nombre_Archivo As String, DatosCR As DataSet)
        Informe.SetDataSource(DatosCR.Tables(0))

        If Imprimir = "Si" Then
            Dim conf As New Configuracion(Usuario, Clave, Bd, Servidor)
            Dim Sql As String
            Sql = "SELECT Impresion, NomImpresora, Copias FROM Empresa"
            Datos = conf.EjecutaSql(Sql)

            Imprimir = Datos.Tables(0).Rows(0).Item("Impresion")
        End If

        If Imprimir = "Directo" Then
            For I As Integer = 0 To Integer.Parse(Datos.Tables(0).Rows(0).Item("Copias").ToString) - 1
                Informe.PrintOptions.PrinterName = Datos.Tables(0).Rows(0).Item("NomImpresora")
                Informe.PrintToPrinter(1, False, 0, 0)
            Next
            Cerrar()
        Else
            Dim exportOpts As CrystalDecisions.Shared.ExportOptions = New CrystalDecisions.Shared.ExportOptions()
            Dim pdfOpts As New CrystalDecisions.Shared.PdfRtfWordFormatOptions
            exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat
            exportOpts.ExportFormatOptions = pdfOpts
            Informe.ExportToHttpResponse(exportOpts, Response, False, Nombre_Archivo)
            Informe.Close()
            Informe.Dispose()
        End If
    End Sub

    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = " < Script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub

    Sub Cerrar()
        Dim msg As String
        msg = "<Script language='javascript'>window.top.close();</script>"
        Response.Write(msg)
    End Sub

End Class