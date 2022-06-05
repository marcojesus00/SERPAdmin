<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ComFac.aspx.vb" Inherits="SERP_POS.ComFac" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Factura proveedor</title>
 <!-- Bootstrap core CSS -->  
  <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
  <link rel="shortcut icon" type="image/x-icon" href="svgs/solid/drafting-compass.svg"/>

  <!-- Custom styles for this template -->
  <link href="css/simple-sidebar.css" rel="stylesheet"/>
  <link rel="stylesheet" href="css/all.min.css" />
    <style>
        .row {padding:1px;
        }       
    </style>
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnAgregar">

   <div class="d-flex" id="wrapper">

    <!-- Sidebar -->
    <div class="bg-light border-right" id="sidebar-wrapper">
      <div class="sidebar-heading text-primary" style="font-weight:bolder">Compras</div>
      <div class="list-group list-group-flush"> 
        <a runat="server" id="barMedio" class="list-group-item list-group-item-action bg-light"><i class="far fa-credit-card" style="font-size:large; text-align:center"></i>&nbsp Pago&nbsp<label id="lblPagado" class="badge badge-secondary" runat="server">0.00</label></a>        
        <a runat="server" id="barNuevo" class="list-group-item list-group-item-action bg-light"><i class="far fa-file-alt" style="font-size:large; text-align:center"></i>&nbsp Nuevo</a>
        <a runat="server" id="barBuscar" class="list-group-item list-group-item-action bg-light"><i class="fas fa-search" style="font-size:large; text-align:center"></i>&nbsp Buscar</a>
        <a runat="server" id="barCancelar" class="list-group-item list-group-item-action bg-light" visible="false"><i class="far fa-trash-alt" style="font-size:large; text-align:center"></i>&nbsp Cancelar</a>
        <a runat="server" id="barReimpresion" class="list-group-item list-group-item-action bg-light" visible="false"><i class="fas fa-print" style="font-size:large; text-align:center"></i>&nbsp Imprimir</a>        
        <a runat="server" id="barCopiar" class="list-group-item list-group-item-action bg-light"><i class="far fa-copy" style="font-size:large; text-align:center"></i>&nbsp Copiar de</a>
        <p class="list-group-item list-group-item-action bg-light"><i class="far fa-comment-alt" style="font-size:large; text-align:center"></i>&nbsp Comentarios<asp:TextBox ID="txtComentarios" TextMode="MultiLine" onkeypress="if (this.value.length > 200) { return false; }" runat="server" CssClass="form-control form-control-sm"></asp:TextBox></p>
      </div>
    </div>
    <!-- /#sidebar-wrapper -->

    <!-- Page Content -->
       <div id="page-content-wrapper">

           <nav class="navbar navbar-expand-lg navbar-light bg-light border-bottom">
               <button class="btn btn-light" id="menu-toggle"><i class="fas fa-bars" style="font-size: x-large; color: darkslategray"></i></button>
               <h4 style="width:100%; text-align:center">Factura proveedor <asp:Label ID="lblEstado" runat="server" Text="(Abierto)" style="font-size:medium"></asp:Label></h4>
               <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                   <span class="navbar-toggler-icon"></span>
               </button>

               <div class="collapse navbar-collapse" id="navbarSupportedContent">
                   <ul class="navbar-nav ml-auto mt-2 mt-lg-0">
                       <li class="nav-item active">
                           <a class="btn btn-light" onclick="window.open('inicio.aspx','_self')"><i class="fas fa-home" style="font-size: x-large; color: darkslategray"></i></a>
                       </li>
                   </ul>
               </div>
           </nav>

           <div class="container-fluid" style="padding-top: 5px;" id="PanelFactura" runat="server">
               <div class="row">
                   <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <asp:DropDownList ID="dlNumeracion" runat="server" CssClass="input-group-text" ToolTip="Serie de Facturacion..." OnSelectedIndexChanged="dlNumeracion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                           </div>
                           <asp:TextBox TextMode="number" CssClass="form-control form-control-sm" ID="txtNumeracion" runat="server" ToolTip="Numero de Factura..." />
                       </div>
                   </div>
                   <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dlAlmacen" class="input-group-text" style="width: 95px">Almacen</label>
                           </div>
                           <asp:DropDownList ID="dlAlmacen" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                       </div>
                   </div>                    
                   <div class="col" style="text-align: right">
                       <asp:Button ID="btnCrear" runat="server" Text="    Crear    " CssClass="btn btn-sm btn-primary" />
                   </div>
               </div>
               <div class="row">
                   <div class="col">
                       <div class="input-group input-group-sm">
                           <asp:TextBox TextMode="SingleLine" CssClass="form-control form-control-sm" placeholder="Codigo" ID="txtCodigo" MaxLength="30" runat="server" OnTextChanged="TxtCodigoCliente_TextChanged" AutoPostBack="true" />
                           <div class="input-group-append">
                               <label class="input-group-text input-group-"><asp:LinkButton ID="btnBuscarCliente" runat="server" CssClass="fas fa-search text-secondary"></asp:LinkButton></label>
                           </div>                           
                       </div>                       
                   </div>
                   <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="txtFecha" class="input-group-text" style="width:95px">Documento</label>
                           </div>
                           <input type="date" class="form-control form-control-sm" id="txtFecha" runat="server" />
                       </div>
                   </div>
                   <div class="col"></div>
               </div>
               <div class="row">
                   <div class="col">
                       <input type="text" class="form-control form-control-sm" placeholder="Nombre" id="txtNombre" maxlength="50" runat="server" />
                   </div>
                   <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="txtVencimiento" class="input-group-text" style="width:95px">Vencimiento</label>
                           </div>
                           <input type="date" class="form-control form-control-sm" id="txtVencimiento" runat="server" />
                       </div>
                   </div>
                  <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label class="input-group-text" style="width:110px">Lista Precios</label>
                           </div>
                           <label class="form-control form-control-sm" id="lblLista" runat="server"></label>
                       </div>
                   </div>            
               </div>
               <div class="row">
                   <div class="col">
                       <input type="text" class="form-control form-control-sm" placeholder="RTN" id="txtRTN" maxlength="30" runat="server" />
                   </div>
                   <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="dlVendedor" class="input-group-text" style="width:95px">Vendedor</label>
                           </div>
                           <asp:DropDownList ID="dlVendedor" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                       </div>
                   </div>
                    <div class="col" style="text-align:left">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label class="input-group-text" style="width: 110px">Forma de Pago</label>
                           </div>
                           <label class="form-control form-control-sm" id="lblFormaPago" runat="server"></label>
                       </div>
                   </div>                                      
               </div>
               <div class="row" style="padding-top: 15px">
                       <div class="col">
                           <div class="input-group input-group-sm">
                               <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control form-control-sm" placeholder="Buscar producto..." OnTextChanged="TxtBuscar_TextChanged"  AutoPostBack="true" />
                               <div class="input-group-append">
                                   <label class="input-group-text">
                                       <asp:LinkButton ID="btnBuscarProductos2" runat="server" CssClass="fas fa-search text-secondary"></asp:LinkButton></label>
                               </div>
                           </div>
                       </div>
                       <div class="col"></div>
                   </div>
               <div class="row">
                   <div class="col">
                       <asp:GridView ID="gvDetalle" runat="server" CssClass="table table-sm table-bordered table-hover" AutoGenerateColumns="False">
                           <Columns>
                               <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-edit text-primary" EditText="" ShowEditButton="True">
                                   <ControlStyle CssClass="fas fa-edit text-primary"></ControlStyle>

                                   <ItemStyle HorizontalAlign="Center"></ItemStyle>
                               </asp:CommandField>
                               <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-trash-alt text-danger" DeleteText="" ShowDeleteButton="True">
                                   <ControlStyle CssClass="fas fa-trash-alt text-danger"></ControlStyle>

                                   <ItemStyle HorizontalAlign="Center"></ItemStyle>
                               </asp:CommandField>
                               <asp:TemplateField HeaderText="Codigo">
                                   <EditItemTemplate>
                                       <asp:Label CssClass="form-control form-control-sm" runat="server" Text='<%# Bind("Codigo") %>' ID="gvtxtCodigo"></asp:Label>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Codigo") %>' ID="Label1"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Producto">
                                   <EditItemTemplate>
                                       <asp:TextBox CssClass="form-control form-control-sm" runat="server" Text='<%# Bind("Producto") %>' ID="gvtxtProducto"></asp:TextBox>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Producto") %>' ID="Label2"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Cantidad">
                                   <EditItemTemplate>
                                       <asp:TextBox CssClass="form-control form-control-sm" TextMode="Number" runat="server" Text='<%# Bind("Cantidad") %>' ID="gvtxtCantidad"></asp:TextBox>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Cantidad") %>' ID="Label3"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Precio">
                                   <EditItemTemplate>
                                       <asp:TextBox CssClass="form-control form-control-sm" TextMode="Number" runat="server" Text='<%# Bind("PrecioConDes") %>' ID="gvtxtPrecio"></asp:TextBox>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("PrecioConDes") %>' ID="Label4"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="% Descuento ">
                                   <EditItemTemplate>
                                       <asp:TextBox CssClass="form-control form-control-sm" TextMode="Number" runat="server" Text='<%# Bind("Descuento") %>' ID="gvtxtDescuento"></asp:TextBox>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Descuento") %>' ID="Label5"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Impuesto">
                                   <EditItemTemplate>
                                       <asp:DropDownList CssClass="form-control form-control-sm" runat="server" Text='<%# Bind("Impuesto") %>' ID="gvtxtImpuesto" DataSourceID="SqlDataSource1" DataTextField="Descripcion" DataValueField="Descripcion">
                                       </asp:DropDownList>
                                       <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:SERPConnectionString %>' SelectCommand="SELECT Descripcion, Porcentaje
                 FROM Impuestos
"></asp:SqlDataSource>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Impuesto") %>' ID="Label6"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Total">
                                   <EditItemTemplate>
                                       <asp:Label CssClass="form-control form-control-sm" runat="server" Text='<%# Bind("Total") %>' ID="gvtxtTotal"></asp:Label>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Total") %>' ID="Label7"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>

                           </Columns>
                       </asp:GridView>
                   </div>
               </div>

           </div>

           <div class="container-fluid" style="background:white; position: absolute; left: 0; top: 0; bottom: 0" id="PanelProductos" runat="server" visible="false">               
               <div style="padding-left: 15%; padding-right: 15%; padding-top: 40px;">
                   <div class="row" style="padding-top: 15px">
                       <div class="col">
                           <div class="input-group input-group-sm">
                               <asp:TextBox ID="txtProductos" runat="server" CssClass="form-control form-control-sm" placeholder="Buscar producto..." OnTextChanged="TxtProductos_TextChanged" AutoPostBack="true" />
                               <div class="input-group-append">
                                   <label class="input-group-text">
                                       <asp:LinkButton ID="btnBuscarProductos" runat="server" CssClass="fas fa-search text-secondary"></asp:LinkButton></label>
                               </div>
                           </div>
                       </div>
                       <div class="col" style="text-align: right"><asp:LinkButton ID="btnCerrar" runat="server" Style="font-size: x-large"><i class="far fa-times-circle text-secondary"></i></asp:LinkButton></div>
                   </div>
                   <div class="row" style="padding-top: 15px">
                       <div class="col input-group input-group-sm">
                           <div class="input-group-prepend">
                               <span class="input-group-text">Cod</span>
                           </div>
                           <label id="txtCodProducto" runat="server" class="form-control form-control-sm"></label>
                       </div>
                       <div class="col input-group input-group-sm" style="padding-left: 0px">
                           <div class="input-group-prepend">
                               <span class="input-group-text">Cant</span>
                           </div>
                           <asp:TextBox ID="txtCantidad" runat="server" CssClass="form-control form-control-sm" TextMode="Number" />
                       </div>
                       <div class="col input-group input-group-sm" style="display: none">
                           <div class="input-group-prepend">
                               <span class="input-group-text">Lps</span>
                           </div>
                           <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control form-control-sm" TextMode="Number" />
                       </div>
                       <div class="col input-group input-group-sm" style="padding-left: 0px">
                           <div class="input-group-prepend">
                               <span class="input-group-text">Imp</span>
                           </div>
                           <asp:DropDownList ID="dlImpuesto" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                       </div>
                       <div class="col" style="display: none">
                           <asp:TextBox ID="txtNomProducto" runat="server" CssClass="form-control form-control-sm" />
                       </div>
                       <div class="col" style="display: none">
                           <asp:TextBox ID="txtCosto" runat="server" CssClass="form-control form-control-sm" />
                       </div>
                       <div class="col" style="display: none">
                           <asp:TextBox ID="txtFracciones" runat="server" CssClass="form-control form-control-sm" />
                       </div>
                       <div class="col">
                           <asp:LinkButton ID="btnAgregar" runat="server"><i class="fa fa-plus text-primary" style="font-size:x-large;padding-left:2px;padding-right:2px"></i></asp:LinkButton>
                       </div>
                       <div class="col form-check">
                           <input class="form-check-input" type="checkbox" value="" id="chkFijo" runat="server" />
                           <label class="form-check-label" for="chkFijo">
                               Mantener ventana
                           </label>
                       </div>
                   </div>
                   <div class="row" style="background-color: white">
                       <div class="col">
                           <asp:GridView ID="gvProductos" runat="server" CssClass="table table-sm table-bordered table-hover" EmptyDataText="No se econtraron productos" AutoGenerateColumns="false">
                               <Columns>
                                   <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-arrow-right text-primary" SelectText="" ShowSelectButton="True"></asp:CommandField>
                                   <asp:BoundField DataField="Codigo" HeaderText="Codigo"></asp:BoundField>
                                   <asp:BoundField DataField="Descripcion" HeaderText="Descripcion"></asp:BoundField>
                                   <asp:BoundField DataField="Precio" HeaderText="Precio"></asp:BoundField>
                                   <asp:BoundField DataField="Stock" HeaderText="Stock"></asp:BoundField>
                                   <asp:BoundField DataField="Costo" HeaderText="Costo" ItemStyle-ForeColor="Transparent"></asp:BoundField>
                                   <asp:BoundField DataField="Fracciones" HeaderText="Frac"></asp:BoundField>
                               </Columns>
                           </asp:GridView>
                       </div>
                   </div>
               </div>               
           </div>

           <div class="container-fluid" style="background-color: rgba(0,0,0,0.6); position:absolute; left: 0; top: 0; bottom: 0" id="PanelClientes" runat="server" visible="false">
               <div style="padding-left: 15%; padding-right: 15%; padding-top: 40px;">
                   <div class="row" style="background-color: white; padding-top: 15px;">                   
                       <div class="col">
                           <asp:TextBox ID="txtBuscarCliente" runat="server" CssClass="form-control form-control-sm" placeholder="Codigo o Nombre de cliente..." Width="50%" TextMode="SingleLine" OnTextChanged="TxtBuscarCliente_TextChanged" AutoPostBack="true"/>
                       </div>                    
                   </div>
                   <div class="row" style="background-color: white">
                       <div class="col">
                           <asp:GridView ID="gvClientes" runat="server" CssClass="table table-sm table-bordered table-hover" EmptyDataText="No se econtraron clientes">
                               <Columns>
                                   <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-arrow-right text-primary" SelectText="" ShowSelectButton="True"></asp:CommandField>
                               </Columns>
                           </asp:GridView>
                       </div>                       
                   </div>
               </div>
               <asp:LinkButton ID="btnCancelarC" runat="server" Style="position: absolute; right: 15%; top: 38px; font-size: x-large"><i class="far fa-times-circle text-secondary"></i></asp:LinkButton>
           </div>

           <div class="container-fluid" style="background-color: rgba(0,0,0,0.2); position: absolute; left: 0; top: 0; bottom: 0" id="PanelPagos" runat="server" visible="false">
               <div style="padding-left: 15%; padding-right: 15%; padding-top: 40px;">
                   <div class="row" style="background-color: white; padding-top: 15px">
                       <div class="col input-group input-group-sm">
                           <div class="input-group-prepend">
                               <span class="input-group-text">Valor</span>
                           </div>
                           <asp:TextBox ID="txtMontoP" runat="server" CssClass="form-control form-control-sm" TextMode="Number" />
                       </div>                                     
                       <div class="col input-group input-group-sm" style="padding-left: 0px">
                           <div class="input-group-prepend">
                               <span class="input-group-text">Medio</span>
                           </div>
                           <asp:DropDownList ID="dlMedio" runat="server" CssClass="form-control form-control-sm" OnSelectedIndexChanged="dlMedio_SelectedIndexChanged" AutoPostBack="true">
                               <asp:ListItem Selected="True">Efectivo</asp:ListItem>
                               <asp:ListItem>Tarjeta</asp:ListItem>
                               <asp:ListItem>Cheque</asp:ListItem>
                               <asp:ListItem>Transferencia</asp:ListItem>
                           </asp:DropDownList>
                       </div>
                       <div class="col input-group input-group-sm" style="padding-left: 0px" runat="server" id="colRef" visible="false">
                           <div class="input-group-prepend">
                               <span class="input-group-text">Num</span>
                           </div>
                           <asp:TextBox ID="txtRef" runat="server" CssClass="form-control form-control-sm" TextMode="SingleLine" />
                       </div> 
                       <div class="col input-group input-group-sm" style="padding-left: 0px" id="colBanco" runat="server" visible="false">
                           <div class="input-group-prepend">
                               <span class="input-group-text">Banco</span>
                           </div>
                           <asp:DropDownList ID="dlBanco" runat="server" CssClass="form-control form-control-sm">
                               <asp:ListItem Selected="True">Ninguno</asp:ListItem>
                               <asp:ListItem>BAC</asp:ListItem>
                               <asp:ListItem>Atlantida</asp:ListItem>
                               <asp:ListItem>Fihcosa</asp:ListItem>
                               <asp:ListItem>Banpais</asp:ListItem>
                           </asp:DropDownList>
                       </div>
                       <div class="col input-group input-group-sm" style="padding-left: 0px">
                           <div class="input-group-prepend">
                               <span class="input-group-text" style="font-weight: bold">Saldo</span>
                           </div>
                           <label id="lblSaldo" runat="server" class="form-control form-control-sm" style="font-weight: bold">0.00</label>
                       </div> 
                       <div class="col">
                           <asp:LinkButton ID="btnAgregarPago" runat="server"><i class="fa fa-plus text-primary" style="font-size:x-large;padding-left:2px;padding-right:2px"></i></asp:LinkButton>
                       </div>                  
                   </div>
                   <div class="row" style="background-color: white">
                       <div class="col">
                           <asp:GridView ID="gvPagos" runat="server" CssClass="table table-sm table-bordered table-hover" EmptyDataText="Favor ingresar pago">
                               <Columns>
                                   <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-trash-alt text-danger" ShowDeleteButton="True" DeleteText=""></asp:CommandField>
                               </Columns>
                           </asp:GridView>
                       </div>
                   </div>
                    
               </div>
               <asp:LinkButton ID="btnCerrarPago" runat="server" Style="position: absolute; right: 15%; top: 38px; font-size: x-large"><i class="far fa-times-circle text-secondary"></i></asp:LinkButton>
           </div>   
           
           <div class="container-fluid" style="background-color: rgba(0,0,0,0.6); position: absolute; left: 0; top: 0; bottom: 0" id="PanelBuscar" runat="server" visible="false">
               <div style="padding-left: 15%; padding-right: 15%; padding-top: 40px;">
                   <div class="row" style="background-color: white; padding-top: 15px;">
                       <div class="col">
                           <div class="input-group input-group-sm">
                               <asp:TextBox ID="txtBuscarBus" runat="server" CssClass="form-control form-control-sm" placeholder="Cliente..." OnTextChanged="TxtBuscarBus_TextChanged" AutoPostBack="true" />
                               <asp:TextBox ID="txtF1Bus" TextMode="Date" runat="server" CssClass="form-control form-control-sm" />
                               <asp:TextBox ID="txtF2Bus" TextMode="Date" runat="server" CssClass="form-control form-control-sm" />
                               <div class="input-group-append">
                                   <label class="input-group-text">
                                       <asp:LinkButton ID="btnBuscarBus" runat="server" CssClass="fas fa-search text-secondary"></asp:LinkButton></label>
                               </div>
                           </div>
                       </div>
                       <div class="col" style="text-align: right">
                           <asp:LinkButton ID="btnCancelarBuscar" runat="server" Style="font-size: x-large"><i class="far fa-times-circle text-secondary"></i></asp:LinkButton>
                       </div>
                   </div>
                   <div class="row" style="background-color: white">
                       <div class="col">
                           <asp:GridView ID="GvBuscar" runat="server" CssClass="table table-sm table-bordered table-hover" EmptyDataText="No se econtraron datos" AutoGenerateColumns="false">
                               <Columns>
                                   <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-arrow-right text-primary" SelectText="" ShowSelectButton="True"></asp:CommandField>
                                   <asp:BoundField DataField="Numero" HeaderText="Numero"></asp:BoundField>
                                   <asp:BoundField DataField="Fecha Sis" HeaderText="Fecha Sis"></asp:BoundField>
                                   <asp:BoundField DataField="Fecha Doc" HeaderText="Fecha Doc"></asp:BoundField>
                                   <asp:BoundField DataField="Codigo" HeaderText="Codigo"></asp:BoundField>
                                   <asp:BoundField DataField="Nombre" HeaderText="Nombre"></asp:BoundField>
                                   <asp:BoundField DataField="Total" HeaderText="Total"></asp:BoundField>
                                   <asp:BoundField DataField="Estado" HeaderText="Estado"></asp:BoundField>
                               </Columns>
                           </asp:GridView>
                       </div>
                   </div>
               </div>
           </div>

           <div class="container-fluid" style="background-color: rgba(0,0,0,0.6); position: absolute; left: 0; top: 0; bottom: 0" id="PanelCopiar" runat="server" visible="false">
               <div style="padding-left: 15%; padding-right: 15%; padding-top: 40px;">
                   <div class="row" style="background-color: white; padding-top: 15px;">
                       <div class="col">
                           <div class="input-group input-group-sm">
                               <asp:TextBox ID="txtCopiar" runat="server" CssClass="form-control form-control-sm" placeholder="Cliente..." OnTextChanged="TxtCopiar_TextChanged" AutoPostBack="true" />
                               <asp:TextBox ID="txtF1Copiar" TextMode="Date" runat="server" CssClass="form-control form-control-sm" />
                               <asp:TextBox ID="txtF2Copiar" TextMode="Date" runat="server" CssClass="form-control form-control-sm" />
                               <div class="input-group-append">
                                   <label class="input-group-text">
                                       <asp:LinkButton ID="btnBuscarCopiar" runat="server" CssClass="fas fa-search text-secondary"></asp:LinkButton></label>
                               </div>
                           </div>
                       </div>
                       <div class="col" style="text-align: right">
                           <asp:LinkButton ID="btnCancelarCopiar" runat="server" Style="font-size: x-large"><i class="far fa-times-circle text-secondary"></i></asp:LinkButton></div>
                   </div>
                   <div class="row" style="background-color: white">
                       <div class="col">
                           <asp:GridView ID="GvCopiar" runat="server" CssClass="table table-sm table-bordered table-hover" EmptyDataText="No se econtraron datos" AutoGenerateColumns="false">
                               <Columns>
                                   <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-arrow-right text-primary" SelectText="" ShowSelectButton="True"></asp:CommandField>
                                   <asp:BoundField DataField="Numero" HeaderText="Numero"></asp:BoundField>
                                   <asp:BoundField DataField="Fecha Sis" HeaderText="Fecha Sis"></asp:BoundField>
                                   <asp:BoundField DataField="Fecha Doc" HeaderText="Fecha Doc"></asp:BoundField>
                                   <asp:BoundField DataField="Codigo" HeaderText="Codigo"></asp:BoundField>
                                   <asp:BoundField DataField="Nombre" HeaderText="Nombre"></asp:BoundField>
                                   <asp:BoundField DataField="Total" HeaderText="Total"></asp:BoundField>
                                   <asp:BoundField DataField="Estado" HeaderText="Estado"></asp:BoundField>
                               </Columns>
                           </asp:GridView>
                       </div>
                   </div>
               </div>
           </div>

           <div id="PanelTotales" runat="server" style="width: 300px; height: auto; position: fixed; right: 0px; bottom: 10px">
               <div class="row input-group input-group-sm">
                   <div class="input-group-prepend">
                       <span class="input-group-text" style="width: 80px">Sub Total</span>
                   </div>
                   <label id="lblSubTotal" runat="server" class="form-control form-control-sm">0.00</label>
                   <div class="input-group-append">
                       <span class="input-group-text">L</span>
                   </div>
               </div>
               <div class="row input-group input-group-sm">
                   <div class="input-group-prepend">
                       <span class="input-group-text" style="width: 80px">Descuento</span>
                   </div>
                   <label id="lblDescuento" runat="server" class="form-control form-control-sm">0.00</label>
                   <div class="input-group-append">
                       <span class="input-group-text">L</span>
                   </div>
               </div>
               <div class="row input-group input-group-sm">
                   <div class="input-group-prepend">
                       <span class="input-group-text" style="width: 80px">Impuesto</span>
                   </div>
                   <label id="lblImpuesto" runat="server" class="form-control form-control-sm">0.00</label>
                   <div class="input-group-append">
                       <span class="input-group-text">L</span>
                   </div>
               </div>               
               <div class="row input-group input-group-sm">
                   <div class="input-group-prepend">
                       <span class="input-group-text" style="width: 80px; font-weight: bold">Total</span>
                   </div>
                   <label id="lblTotal" runat="server" class="form-control form-control-sm" style="font-weight: bold">0.00</label>
                   <div class="input-group-append">
                       <span class="input-group-text" style="font-weight: bold">L</span>
                   </div>
               </div>
           </div>

           <div class="container-fluid" style="background-color: rgba(0, 0, 0,0.8); position: absolute; left: 0; top: 0; bottom: 0" id="PanelCancelar" runat="server" visible="false">
               <div style="padding-left: 38%; padding-right: 38%;padding-top:5%">
                   <div class="row rounded" style="background-color:white; padding: 5px 20px 15px 20px">
                       <div class="col" style="width:100%;text-align:center">
                           <div class="row" style="padding-bottom:3px">
                               <div class="col"><label>Desea cancelar el documento, esta acción es permanente.</label></div>
                           </div>  
                           <div class="row">
                               <div class="col">
                                   <asp:Button ID="btnAceptarCan" runat="server" Text="   Si   " CssClass="btn btn-primary btn-sm"/>&nbsp &nbsp &nbsp <asp:Button ID="bntCancelarCan" runat="server" Text="  No  " CssClass="btn btn-secondary btn-sm"/></div>
                           </div>                               
                       </div>                                        
                   </div>                         
               </div>               
           </div>

           <div style="position:fixed; bottom:20px; padding-left:15px;" >               
               <asp:Label ID="lblMsg" runat="server"></asp:Label>
           </div>

            <br />
            <br />
            <br />
            <br />
            <br />          

       </div>
    <!-- /#page-content-wrapper -->

  </div>
   <!-- /#wrapper -->

    <!-- Bootstrap core JavaScript -->
    <script src="vendor/jquery/jquery.min.js"></script>
    <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

    <!-- Menu Toggle Script -->
    <script>
    $("#menu-toggle").click(function(e) {
        e.preventDefault();
        $("#wrapper").toggleClass("toggled");
    });
    </script>
    </form>  
</body>
</html>
