Imports System.IO

Public Class RepExi
    Inherits System.Web.UI.Page
    Public Usuario, Clave, Servidor, Bd, Usuario_Aut, Clave_Aut, Status, SuperUser, Funcion As String
    Private Datos As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Usuario_Aut") = "" Or Session("Destino") <> "RepExi.aspx" Then
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
            LlenarAlmacen()
            GvPrincipal.DataSource = ""
            GvPrincipal.DataBind()
        End If
    End Sub

    Sub LlenarAlmacen()
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT '' AlmacenCod, 'Todos' Descripcion UNION ALL 
               SELECT AlmacenCod, Descripcion
                 FROM Almacen
                 WHERE Activo = 'Y'"
        Datos = Conf.EjecutaSql(Sql)

        dlAlmacen.DataSource = Datos.Tables(0)
        dlAlmacen.DataTextField = "Descripcion"
        dlAlmacen.DataValueField = "AlmacenCod"
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

    Protected Sub TxtBuscar_TextChanged(sender As Object, e As EventArgs)
        Dim Conf As New Configuracion(Usuario, Clave, Bd, Servidor)
        Dim Sql As String

        Sql = "SELECT A.CodArticulo Codigo, A.NomArticulo Descripcion, B.Stock, A.Costo, B.AlmacenCod Almacen
	            FROM Articulo A
	            INNER JOIN ArticuloExis B ON A.CodArticulo = B.CodArticulo	
	            WHERE A.Activo = 'Y'
                AND A.Tipo = 'A'
                AND B.AlmacenCod LIKE '%" + dlAlmacen.SelectedValue + "%'
                AND A.CodArticulo + A.NomArticulo+ + A.CodigoBarra LIKE '%" + txtBuscar.Text + "%'"
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
            If Decimal.Parse(e.Row.Cells(2).Text.ToString) <= 0.00 Then
                e.Row.Cells(2).ControlStyle.ForeColor = System.Drawing.Color.Red
            Else
                e.Row.Cells(2).ControlStyle.ForeColor = System.Drawing.Color.Blue
            End If
        End If
    End Sub

    Private Sub BtnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        TxtBuscar_TextChanged(sender, e)
    End Sub

    Private Sub BtnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.ServerClick
        GVtoExcel(Session("GV").Tables(0), "Existencias " + DateTime.Now.ToString("yyyy-MM-dd"))
    End Sub

    Private Sub BtnImprimir_ServerClick(sender As Object, e As EventArgs) Handles btnImprimir.ServerClick
        Dim javaScript As String = "window.open('reportes.aspx','_blank','scrollbars=yes,resizable=yes,top=5,left=5,width=700,height=700');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "script", javaScript, True)
    End Sub

End Class