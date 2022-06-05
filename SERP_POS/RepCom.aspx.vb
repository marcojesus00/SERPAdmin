Imports System.IO

Public Class RepCom
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut, Status, SuperUser, Funcion As String
    Private TotalDias As Decimal = 0
    Private TotalDocumento As Decimal = 0
    Private TotalVenta As Decimal = 0
    Private TotalCosto As Decimal = 0
    Private TotalGanacia As Decimal = 0
    Private Datos As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario_Aut") = "" Or Session("Destino") <> "RepCom.aspx" Then
            Response.Redirect("inicio.aspx")
        End If

        Usuario = Session("Usuario")
        Clave = Session("Clave")
        Bd = Session("Bd")
        Servidor = Session("Sevidor")
        Funcion = Session("Funcion")
        Session.Timeout = Session("Tiempo")

        If Not IsPostBack Then
            Session.Add("Orden", "0")
            LlenarVendedores()

            GvPrincipal.DataSource = ""
            GvPrincipal.DataBind()
            txtF1Reim.Text = DateTime.Now.ToString("yyyy-MM-dd")
            txtF2Reim.Text = DateTime.Now.ToString("yyyy-MM-dd")
        End If
    End Sub

    Sub LlenarVendedores()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String = ""

        Sql = "	SELECT CASE CONVERT(VARCHAR,A.EncarCod) WHEN '-1' THEN '' ELSE CONVERT(VARCHAR,A.EncarCod) END  EncarCod, A.EncarNom 
                FROM (SELECT '-1' EncarCod, 'Todos' EncarNom UNION ALL
                SELECT A.EncarCod, A.EncarNom
		        FROM Encargado A
		        WHERE A.Activo = 'Y') A"
        Datos = Conf.EjecutaSql(Sql)

        dlAlmacen.DataSource = Datos.Tables(0)
        dlAlmacen.DataTextField = "EncarNom"
        dlAlmacen.DataValueField = "EncarCod"
        dlAlmacen.DataBind()
    End Sub

    Sub Msg(Mensaje As String)
        Dim msg As String
        msg = "<script language='javascript'>"
        msg += "alert('" + Mensaje + "');"
        msg += "<" & "/script>"
        Response.Write(msg)
    End Sub

    Private Sub GVtoExcel(dt As Data.DataTable, Nombre As String)
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=" + Nombre + ".xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"

        gvExcel.DataSource = dt
        gvExcel.DataBind()

        Using sw As StringWriter = New StringWriter()
            Dim hw As HtmlTextWriter = New HtmlTextWriter(sw)

            gvExcel.RenderControl(hw)

            Response.Output.Write(sw.ToString())
            Response.Flush()
            Response.End()
        End Using
    End Sub

    Protected Sub txtBuscar_TextChanged(sender As Object, e As EventArgs)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        If dlFecha.SelectedValue = "Fecha Sis" Then
            Sql = "SELECT 
                A.NumDoc Documento, 
                A.FechaSis [Fecha Sistema],
                A.FechaDoc [Fecha Documento],
                A.CodigoM [Codigo Cliente], 
                A.NombreM [Nombre Cliente], 
                A.TotalDoc Total, 
                B.CodArticulo Codigo, 
                B.Descripcion Articulo, 
                B.Cantidad, 
                CONVERT(VARCHAR(50), CAST(B.PrecioConDes AS MONEY ),1) Precio,                 
                CONVERT(VARCHAR(50), CAST(B.Cantidad*B.PrecioConDes AS MONEY ),1) [Total Linea],                 
                C.EncarNom Comprador
	                FROM ComFacEnc A
	                INNER JOIN ComFacDet B ON A.NumDoc = B.NumDoc
	                INNER JOIN Encargado C ON A.EncarCod = C.EncarCod
	                WHERE A.EncarCod  LIKE '%" + dlAlmacen.SelectedValue + "%'
                    AND A.CodigoM + A.NombreM LIKE '%" + txtBuscar.Text + "%'
                    AND CONVERT(DATE,A.FechaSis) BETWEEN '" + txtF1Reim.Text + "' AND '" + txtF2Reim.Text + "'
        UNION ALL
            SELECT 
                A.NumDoc Documento, 
                A.FechaSis [Fecha Sistema],
                A.FechaDoc [Fecha Documento],
                A.CodigoM [Codigo Cliente], 
                A.NombreM [Nombre Cliente], 
                -A.TotalDoc Total, 
                B.CodArticulo Codigo, 
                B.Descripcion Articulo, 
                -B.Cantidad, 
                CONVERT(VARCHAR(50), CAST(-B.PrecioConDes AS MONEY ),1) Precio,                 
                CONVERT(VARCHAR(50), CAST(-B.Cantidad*B.PrecioConDes AS MONEY ),1) [Total Linea],                 
                C.EncarNom Comprador
	                FROM ComNcEnc A
	                INNER JOIN ComNcDet B ON A.NumDoc = B.NumDoc
	                INNER JOIN Encargado C ON A.EncarCod = C.EncarCod
	                WHERE A.EncarCod  LIKE '%" + dlAlmacen.SelectedValue + "%'
                    AND A.CodigoM + A.NombreM LIKE '%" + txtBuscar.Text + "%'
                    AND CONVERT(DATE,A.FechaSis) BETWEEN '" + txtF1Reim.Text + "' AND '" + txtF2Reim.Text + "' 
                    ORDER BY A.FechaSis ASC"
        Else
            Sql = "SELECT 
                A.NumDoc Documento, 
                A.FechaSis [Fecha Sistema],
                A.FechaDoc [Fecha Documento],
                A.CodigoM [Codigo Cliente], 
                A.NombreM [Nombre Cliente], 
                A.TotalDoc Total, 
                B.CodArticulo Codigo, 
                B.Descripcion Articulo, 
                B.Cantidad, 
                CONVERT(VARCHAR(50), CAST(B.PrecioConDes AS MONEY ),1) Precio,                 
                CONVERT(VARCHAR(50), CAST(B.Cantidad*B.PrecioConDes AS MONEY ),1) [Total Linea],                 
                C.EncarNom Comprador
	                FROM ComFacEnc A
	                INNER JOIN ComFacDet B ON A.NumDoc = B.NumDoc
	                INNER JOIN Encargado C ON A.EncarCod = C.EncarCod
	                WHERE A.EncarCod  LIKE '%" + dlAlmacen.SelectedValue + "%'
                    AND A.CodigoM + A.NombreM LIKE '%" + txtBuscar.Text + "%'
                    AND CONVERT(DATE,A.FechaDoc) BETWEEN '" + txtF1Reim.Text + "' AND '" + txtF2Reim.Text + "'
        UNION ALL
            SELECT 
                A.NumDoc Documento, 
                A.FechaSis [Fecha Sistema],
                A.FechaDoc [Fecha Documento],
                A.CodigoM [Codigo Cliente], 
                A.NombreM [Nombre Cliente], 
                -A.TotalDoc Total, 
                B.CodArticulo Codigo, 
                B.Descripcion Articulo, 
                -B.Cantidad, 
                CONVERT(VARCHAR(50), CAST(-B.PrecioConDes AS MONEY ),1) Precio,                 
                CONVERT(VARCHAR(50), CAST(-B.Cantidad*B.PrecioConDes AS MONEY ),1) [Total Linea],                 
                C.EncarNom Comprador
	                FROM ComNcEnc A
	                INNER JOIN ComNcDet B ON A.NumDoc = B.NumDoc
	                INNER JOIN Encargado C ON A.EncarCod = C.EncarCod
	                WHERE A.EncarCod  LIKE '%" + dlAlmacen.SelectedValue + "%'
                    AND A.CodigoM + A.NombreM LIKE '%" + txtBuscar.Text + "%'
                    AND CONVERT(DATE,A.FechaDoc) BETWEEN '" + txtF1Reim.Text + "' AND '" + txtF2Reim.Text + "'
                    ORDER BY A.FechaSis ASC"
        End If

        Datos = Conf.EjecutaSql(Sql)
        Session.Add("GV", Datos)

        GvPrincipal.DataSource = Session("GV").Tables(0)
        GvPrincipal.DataBind()
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        'MyBase.VerifyRenderingInServerForm(control)
    End Sub

    Private Sub GvPrincipal_Sorting(sender As Object, e As GridViewSortEventArgs) Handles GvPrincipal.Sorting
        Dim Orden As String

        If Session("Orden") = "0" Then
            Orden = "Asc"
            Session.Add("Orden", "1")
        Else
            Orden = "Desc"
            Session.Add("Orden", "0")
        End If

        Session("GV").Tables(0).DefaultView.Sort = e.SortExpression.ToString + " " + Orden
        GvPrincipal.DataSource = Session("GV").Tables(0)
        GvPrincipal.DataBind()
    End Sub

    Private Sub GvPrincipal_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GvPrincipal.RowDataBound
        If ((e.Row.RowType = DataControlRowType.DataRow) And (e.Row.RowType <> DataControlRowType.EmptyDataRow)) Then
            Dim Fila As System.Data.DataRowView = e.Row.DataItem
            TotalVenta += Convert.ToDecimal(Fila.Item("Total Linea"))
            TotalDocumento += 1

            lblTotalDocumento.Text = "Facturas: " + Format(TotalDocumento, "#,##0")
            lblTotalVenta.Text = "Gasto: " + Format(TotalVenta, "#,##0.00")
            'lblTotalCosto.Text = "Costo: " + Format(TotalCosto, "#,##0.00")
            'lblTotalGanancia.Text = "Ganacia: " + Format(TotalGanacia, "#,##0.00")
        End If
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        txtBuscar_TextChanged(sender, e)
    End Sub

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.ServerClick
        GVtoExcel(Session("GV").Tables(0), "Compras " + DateTime.Now.ToString("yyyy-MM-dd"))
    End Sub

    Private Sub btnImprimir_ServerClick(sender As Object, e As EventArgs) Handles btnImprimir.ServerClick
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        If dlFecha.SelectedValue = "Fecha Sis" Then
            Sql = "SELECT 
                A.NumDoc Documento, 
                A.FechaSis [Fecha Sistema],
                A.FechaDoc [Fecha Documento],
                A.CodigoM [Codigo Cliente], 
                A.NombreM [Nombre Cliente], 
                A.TotalDoc Total, 
                B.CodArticulo Codigo, 
                B.Descripcion Articulo, 
                B.Cantidad, 
                CONVERT(VARCHAR(50), CAST(B.PrecioConDes AS MONEY ),1) Precio,                 
                CONVERT(VARCHAR(50), CAST(B.Cantidad*B.PrecioConDes AS MONEY ),1) [Total Linea],                 
                C.EncarNom Comprador
	                FROM ComFacEnc A
	                INNER JOIN ComFacDet B ON A.NumDoc = B.NumDoc
	                INNER JOIN Encargado C ON A.EncarCod = C.EncarCod
	                WHERE A.EncarCod  LIKE '%" + dlAlmacen.SelectedValue + "%'
                    AND A.CodigoM + A.NombreM LIKE '%" + txtBuscar.Text + "%'
                    AND CONVERT(DATE,A.FechaSis) BETWEEN '" + txtF1Reim.Text + "' AND '" + txtF2Reim.Text + "'
        UNION ALL
            SELECT 
                A.NumDoc Documento, 
                A.FechaSis [Fecha Sistema],
                A.FechaDoc [Fecha Documento],
                A.CodigoM [Codigo Cliente], 
                A.NombreM [Nombre Cliente], 
                -A.TotalDoc Total, 
                B.CodArticulo Codigo, 
                B.Descripcion Articulo, 
                -B.Cantidad, 
                CONVERT(VARCHAR(50), CAST(-B.PrecioConDes AS MONEY ),1) Precio,                 
                CONVERT(VARCHAR(50), CAST(-B.Cantidad*B.PrecioConDes AS MONEY ),1) [Total Linea],                 
                C.EncarNom Comprador
	                FROM ComNcEnc A
	                INNER JOIN ComNcDet B ON A.NumDoc = B.NumDoc
	                INNER JOIN Encargado C ON A.EncarCod = C.EncarCod
	                WHERE A.EncarCod  LIKE '%" + dlAlmacen.SelectedValue + "%'
                    AND A.CodigoM + A.NombreM LIKE '%" + txtBuscar.Text + "%'
                    AND CONVERT(DATE,A.FechaSis) BETWEEN '" + txtF1Reim.Text + "' AND '" + txtF2Reim.Text + "' 
                    ORDER BY A.FechaSis ASC"
        Else
            Sql = "SELECT 
                A.NumDoc Documento, 
                A.FechaSis [Fecha Sistema],
                A.FechaDoc [Fecha Documento],
                A.CodigoM [Codigo Cliente], 
                A.NombreM [Nombre Cliente], 
                A.TotalDoc Total, 
                B.CodArticulo Codigo, 
                B.Descripcion Articulo, 
                B.Cantidad, 
                CONVERT(VARCHAR(50), CAST(B.PrecioConDes AS MONEY ),1) Precio,                 
                CONVERT(VARCHAR(50), CAST(B.Cantidad*B.PrecioConDes AS MONEY ),1) [Total Linea],                 
                C.EncarNom Comprador
	                FROM ComFacEnc A
	                INNER JOIN ComFacDet B ON A.NumDoc = B.NumDoc
	                INNER JOIN Encargado C ON A.EncarCod = C.EncarCod
	                WHERE A.EncarCod  LIKE '%" + dlAlmacen.SelectedValue + "%'
                    AND A.CodigoM + A.NombreM LIKE '%" + txtBuscar.Text + "%'
                    AND CONVERT(DATE,A.FechaDoc) BETWEEN '" + txtF1Reim.Text + "' AND '" + txtF2Reim.Text + "'
        UNION ALL
            SELECT 
                A.NumDoc Documento, 
                A.FechaSis [Fecha Sistema],
                A.FechaDoc [Fecha Documento],
                A.CodigoM [Codigo Cliente], 
                A.NombreM [Nombre Cliente], 
                -A.TotalDoc Total, 
                B.CodArticulo Codigo, 
                B.Descripcion Articulo, 
                -B.Cantidad, 
                CONVERT(VARCHAR(50), CAST(-B.PrecioConDes AS MONEY ),1) Precio,                 
                CONVERT(VARCHAR(50), CAST(-B.Cantidad*B.PrecioConDes AS MONEY ),1) [Total Linea],                 
                C.EncarNom Comprador
	                FROM ComNcEnc A
	                INNER JOIN ComNcDet B ON A.NumDoc = B.NumDoc
	                INNER JOIN Encargado C ON A.EncarCod = C.EncarCod
	                WHERE A.EncarCod  LIKE '%" + dlAlmacen.SelectedValue + "%'
                    AND A.CodigoM + A.NombreM LIKE '%" + txtBuscar.Text + "%'
                    AND CONVERT(DATE,A.FechaDoc) BETWEEN '" + txtF1Reim.Text + "' AND '" + txtF2Reim.Text + "'
                    ORDER BY A.FechaSis ASC"
        End If

        Datos = Conf.EjecutaSql(Sql)
        Session.Add("GVR", Datos)

        Dim javaScript As String = "window.open('reportes.aspx','_blank','scrollbars=yes,resizable=yes,top=5,left=5,width=700,height=700');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)
    End Sub

End Class