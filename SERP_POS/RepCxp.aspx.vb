Imports System.Data.SqlClient
Imports System.IO

Public Class RepCxp
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut, Status, SuperUser, Funcion As String
    Private TotalDias As Decimal = 0
    Private TotalDocumento As Decimal = 0
    Private TotalVenta As Decimal = 0
    Private TotalCosto As Decimal = 0
    Private TotalGanacia As Decimal = 0
    Private Datos, Datos1 As DataSet
    Private Tabla As DataTable = New DataTable
    Private TablaPago As DataTable = New DataTable
    Private Conector As SqlConnection
    Private Adaptador As SqlDataAdapter

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario_Aut") = "" Or Session("Destino") <> "RepCxp.aspx" Then
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

            txtBuscar_TextChanged(sender, e)
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

        dlVendedor.DataSource = Datos.Tables(0)
        dlVendedor.DataTextField = "EncarNom"
        dlVendedor.DataValueField = "EncarCod"
        dlVendedor.DataBind()
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
        Dim Conf, Conf1 As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql, Sql1 As String

        Sql = " SELECT A.Codigo, A.Cliente, 
                CONVERT(VARCHAR(50), CAST(SUM(A.Saldo) AS MONEY ),1) Saldo, 
                CASE WHEN SUM(A.Corriente) = 0 THEN '-' ELSE CONVERT(VARCHAR(50), CAST(SUM(A.Corriente) AS MONEY ),1) END Corriente, 
                CASE WHEN SUM(A.[30 a 60]) = 0 THEN '-' ELSE CONVERT(VARCHAR(50), CAST(SUM(A.[30 a 60]) AS MONEY ),1) END [30 a 60], 
                CASE WHEN SUM(A.[60 a 90]) = 0 THEN '-' ELSE CONVERT(VARCHAR(50), CAST(SUM(A.[60 a 90]) AS MONEY ),1) END [60 a 90], 
                CASE WHEN SUM(A.[90 a 120]) = 0 THEN '-' ELSE CONVERT(VARCHAR(50), CAST(SUM(A.[90 a 120]) AS MONEY ),1) END [90 a 120], 
                CASE WHEN SUM(A.[120+]) = 0 THEN '-' ELSE CONVERT(VARCHAR(50), CAST(SUM(A.[120+]) AS MONEY ),1) END [120+]
                FROM (
                SELECT 
                A.CodigoM Codigo, 
                B.NombreM Cliente, 
                A.TotalDoc - A.ValorApli Saldo,
                CASE 
	                WHEN DATEDIFF(DAY,A.FechaVen,GETDATE()) < 30 THEN A.TotalDoc - A.ValorApli
	                ELSE 0
                END Corriente,
                CASE 		
	                WHEN DATEDIFF(DAY,A.FechaVen,GETDATE()) >= 30 AND DATEDIFF(DAY,A.FechaVen,GETDATE()) < 60 THEN A.TotalDoc - A.ValorApli
	                ELSE 0
                END [30 a 60],
                CASE 		
	                WHEN DATEDIFF(DAY,A.FechaVen,GETDATE()) >= 60 AND DATEDIFF(DAY,A.FechaVen,GETDATE()) < 90 THEN A.TotalDoc - A.ValorApli
	                ELSE 0
                END [60 a 90],
                CASE 		
	                WHEN DATEDIFF(DAY,A.FechaVen,GETDATE()) >= 90 AND DATEDIFF(DAY,A.FechaVen,GETDATE()) < 120 THEN A.TotalDoc - A.ValorApli
	                ELSE 0
                END [90 a 120],
                CASE 		
	                WHEN DATEDIFF(DAY,A.FechaVen,GETDATE()) > 120 THEN A.TotalDoc - A.ValorApli
	                ELSE 0
                END [120+]
                FROM ComFacEnc A
                INNER JOIN Maestro B ON A.CodigoM = B.CodigoM
                WHERE A.Estado = 'O'
                AND A.EncarCod LIKE '%" + dlVendedor.SelectedValue + "%'
                AND A.CodigoM + B.NombreM LIKE '%" + txtBuscar.Text + "%'
                ) A GROUP BY A.Codigo, A.Cliente 
                ORDER BY A.Codigo ASC"


        Sql1 = " SELECT 
                A.CodigoM Codigo, 
                B.NombreM Cliente, 
                CONVERT(VARCHAR,A.FechaDoc,103) Fecha, 
                CONVERT(VARCHAR,A.FechaVen,103) Vencimiento, 
                A.NumDoc Documento, 
                C.EncarNom Vendedor, 
                CONVERT(VARCHAR(50), CAST(A.TotalDoc - A.ValorApli AS MONEY ),1) Saldo,
                CASE 
	                WHEN DATEDIFF(DAY,A.FechaVen,GETDATE()) < 30 THEN CONVERT(VARCHAR(50), CAST(A.TotalDoc - A.ValorApli AS MONEY ),1)
	                ELSE '-'
                END Corriente,
                CASE 		
	                WHEN DATEDIFF(DAY,A.FechaVen,GETDATE()) >= 30 AND DATEDIFF(DAY,A.FechaVen,GETDATE()) < 60 THEN CONVERT(VARCHAR(50), CAST(A.TotalDoc - A.ValorApli AS MONEY ),1)
	                ELSE '-'
                END [30 a 60],
                CASE 		
	                WHEN DATEDIFF(DAY,A.FechaVen,GETDATE()) >= 60 AND DATEDIFF(DAY,A.FechaVen,GETDATE()) < 90 THEN CONVERT(VARCHAR(50), CAST(A.TotalDoc - A.ValorApli AS MONEY ),1)
	                ELSE '-'
                END [60 a 90],
                CASE 		
	                WHEN DATEDIFF(DAY,A.FechaVen,GETDATE()) >= 90 AND DATEDIFF(DAY,A.FechaVen,GETDATE()) < 120 THEN CONVERT(VARCHAR(50), CAST(A.TotalDoc - A.ValorApli AS MONEY ),1)
	                ELSE '-'
                END [90 a 120],
                CASE 		
	                WHEN DATEDIFF(DAY,A.FechaVen,GETDATE()) > 120 THEN CONVERT(VARCHAR(50), CAST(A.TotalDoc - A.ValorApli AS MONEY ),1)
	                ELSE '-'
                END [120+]
                FROM ComFacEnc A
                INNER JOIN Maestro B ON A.CodigoM = B.CodigoM
                LEFT JOIN Encargado C ON A.EncarCod = C.EncarCod
                WHERE A.Estado = 'O'
                AND A.EncarCod LIKE '%" + dlVendedor.SelectedValue + "%'
                AND A.CodigoM + B.NombreM LIKE '%" + txtBuscar.Text + "%'
                ORDER BY A.CodigoM, A.NumDoc ASC "
        Datos1 = Conf1.EjecutaSql(Sql1)
        Session.Add("gvDetalle", Datos1.Tables(0))

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
            Dim Codigo As String = Fila.Item("Codigo").ToString
            Dim gvDetalle As GridView = TryCast(e.Row.FindControl("GVDetalle"), GridView)

            Session("gvDetalle").DefaultView.RowFilter = "Codigo='" + Codigo + "'"
            gvDetalle.DataSource = Session("gvDetalle")
            gvDetalle.DataBind()

            TotalDocumento += Convert.ToDecimal(Fila.Item("Saldo"))
            TotalVenta += Convert.ToDecimal(Replace(Fila.Item("Corriente"), "-", "0"))
            TotalCosto += Convert.ToDecimal(Fila.Item("Saldo")) - Convert.ToDecimal(Replace(Fila.Item("Corriente"), "-", "0"))

            lblTotalDocumento.Text = "Saldo: " + Format(TotalDocumento, "#,##0.00")
            lblTotalVenta.Text = "Corriente: " + Format(TotalVenta, "#,##0.00")
            lblTotalCosto.Text = "Vencido: " + Format(TotalCosto, "#,##0.00")
        End If
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        txtBuscar_TextChanged(sender, e)
    End Sub

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.ServerClick
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = " SELECT 
                A.CodigoM Codigo, 
                B.NombreM Cliente, 
                A.FechaDoc Fecha, 
                A.FechaVen Vencimiento, 
                A.NumDoc Documento, 
                C.EncarNom Vendedor, 
                A.TotalDoc - A.ValorApli Saldo,
                CASE 
	                WHEN DATEDIFF(DAY,A.FechaVen,GETDATE()) < 30 THEN A.TotalDoc - A.ValorApli
	                ELSE 0
                END Corriente,
                CASE 		
	                WHEN DATEDIFF(DAY,A.FechaVen,GETDATE()) >= 30 AND DATEDIFF(DAY,A.FechaVen,GETDATE()) < 60 THEN A.TotalDoc - A.ValorApli
	                ELSE 0
                END [30 a 60],
                CASE 		
	                WHEN DATEDIFF(DAY,A.FechaVen,GETDATE()) >= 60 AND DATEDIFF(DAY,A.FechaVen,GETDATE()) < 90 THEN A.TotalDoc - A.ValorApli
	                ELSE 0
                END [60 a 90],
                CASE 		
	                WHEN DATEDIFF(DAY,A.FechaVen,GETDATE()) >= 90 AND DATEDIFF(DAY,A.FechaVen,GETDATE()) < 120 THEN A.TotalDoc - A.ValorApli
	                ELSE 0
                END [90 a 120],
                CASE 		
	                WHEN DATEDIFF(DAY,A.FechaVen,GETDATE()) > 120 THEN A.TotalDoc - A.ValorApli
	                ELSE 0
                END [120+]
                FROM ComFacEnc A
                INNER JOIN Maestro B ON A.CodigoM = B.CodigoM
                LEFT JOIN Encargado C ON A.EncarCod = C.EncarCod
                WHERE A.Estado = 'O'
                AND A.EncarCod  LIKE '%" + dlVendedor.SelectedValue + "%'
                AND A.CodigoM + A.NombreM LIKE '%" + txtBuscar.Text + "%'
                ORDER BY A.CodigoM "
        Datos = Conf.EjecutaSql(Sql)
        Session.Add("GVR", Datos)

        GVtoExcel(Session("GVR").Tables(0), "CXP " + DateTime.Now.ToString("yyyy-MM-dd"))
    End Sub

End Class