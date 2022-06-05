<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ParArt.aspx.vb" Inherits="SERP_POS.ParArt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Articulos</title>
 <!-- Bootstrap core CSS -->
  <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
  <link rel="shortcut icon" type="image/x-icon" href="svgs/solid/drafting-compass.svg"/>

  <!-- Custom styles for this template -->
  <link href="css/simple-sidebar.css" rel="stylesheet"/>
  <link rel="stylesheet" href="css/all.min.css" />
    <style>
        .row .col .row {padding:1px;}
        .input-group-text {width: 95px;}
        #btnNuevo, #btnBuscar {padding-left: 15px;}
        i {font-size:large;}
    </style>
</head>
<body>
    <form id="form1" runat="server">

   <div class="d-flex" id="wrapper">

    <!-- Sidebar -->
    <div class="bg-light border-right" id="sidebar-wrapper">
      <div class="sidebar-heading text-primary" style="font-weight: bold">Parametros</div>
      <div class="list-group list-group-flush">
        <asp:LinkButton Enabled="false" runat="server" ID="menuClientes" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-address-book"></i>&nbsp Clientes</asp:LinkButton>
        <asp:LinkButton Enabled="false" runat="server" id="menuPagos" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-credit-card"></i>&nbsp Formas de Pago</asp:LinkButton>
        <asp:LinkButton Enabled="false" runat="server" id="menuVendedores" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-users"></i>&nbsp Vendedores</asp:LinkButton>        
        <asp:LinkButton Enabled="false" runat="server" id="MenuAlmacen" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-warehouse"></i>&nbsp Almacenes</asp:LinkButton>
        <asp:LinkButton Enabled="false" runat="server" ID="menuArticulos" CssClass="list-group-item list-group-item-action bg-light text-primary"><i class="fas fa-dolly-flatbed"></i>&nbsp Articulos</asp:LinkButton>
        <asp:LinkButton Enabled="false" runat="server" ID="menuGrupoArt" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-boxes"></i>&nbsp Grupos de articulos</asp:LinkButton>
        <asp:LinkButton Enabled="false" runat="server" ID="menuPrecios" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-money-bill-alt"></i>&nbsp Precios</asp:LinkButton>       
        <asp:LinkButton Enabled="false" runat="server" ID="menuImpuestos" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-coins"></i>&nbsp Impuestos</asp:LinkButton>
        <asp:LinkButton Enabled="false" runat="server" ID="menuUsuarios" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-user-edit"></i>&nbsp Usuarios</asp:LinkButton>         
        <asp:LinkButton Enabled="false" runat="server" ID="menuSeries" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-list-ol"></i>&nbsp Series</asp:LinkButton>
        <asp:LinkButton Enabled="false" runat="server" ID="menuGeneral" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-toggle-on"></i>&nbsp General</asp:LinkButton>         
      </div>
    </div>
    <!-- /#sidebar-wrapper -->

    <!-- Page Content -->
       <div id="page-content-wrapper">

           <nav class="navbar navbar-expand-lg navbar-light bg-light border-bottom">
               <button class="btn btn-light" id="menu-toggle"><i class="fas fa-bars" style="color: darkslategray"></i></button>
               <h4 style="width:100%; text-align:center">Articulos</h4>
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

           <div class="container-fluid">
               <div class="row" style="padding-top: 15px;">
                   <div class="col-3">
                       <asp:LinkButton ID="btnNuevo" runat="server"><i class="far fa-file-alt" ></i></asp:LinkButton>                  
                       <asp:LinkButton ID="btnBuscar" runat="server"><i runat="server" id="imgClientes" class="fas fa-search" ></i></asp:LinkButton>                                              
                   </div>                   
                   <div class="col" style="text-align: right"><asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-sm btn-primary" Text="   Crear   "/></div>
               </div>
               <div class="row" id="trAgregarClie" runat="server" visible="true" style="padding-top: 10px;">
                   <div class="col">
                       <div class="row">
                           <div class="col-4 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Numeracion</span>
                               </div>
                               <asp:DropDownList CssClass="form-control form-control-sm" ID="dlNumeracion" runat="server" ToolTip="Serie.." OnSelectedIndexChanged="dlNumeracion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                               <asp:TextBox TextMode="SingleLine" CssClass="form-control form-control-sm" ID="txtNumeracion" MaxLength="30" required="required" runat="server" ToolTip="Numero de Articulo..." OnTextChanged="txtNumeracion_TextChanged" AutoPostBack="true" />
                           </div>   
                           <div class="col-1"></div>
                           <div class="col-3 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Tipo</span>
                               </div>
                               <asp:DropDownList ID="dlTipo" runat="server"  CssClass=" form-control form-control-sm">
                                   <asp:ListItem Selected="True" Value="A">Articulo</asp:ListItem>
                                   <asp:ListItem Value="S">Servicio</asp:ListItem>
                               </asp:DropDownList>
                           </div>
                           <div class="col-3 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Lista Precios</span>
                               </div>
                               <asp:DropDownList ID="dlPrecios" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                           </div>
                       </div>
                       <div class="row">
                           <div class="col-5 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Nombre</span>
                               </div>
                               <input type="text" id="txtNombre" maxlength="100" required="required" runat="server" class="form-control form-control-sm" />
                           </div>
                           <div class="col-3 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Grupo</span>
                               </div>
                               <asp:DropDownList ID="dlGrupo" runat="server" Cssclass="form-control form-control-sm"></asp:DropDownList>
                           </div>
                           <div class="col-2 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Activo</span>
                               </div>
                               <asp:DropDownList ID="dlEsatdo" runat="server" Cssclass="form-control form-control-sm">
                                   <asp:ListItem Selected="True" Value="Y">Si</asp:ListItem>
                                   <asp:ListItem Value="N">No</asp:ListItem>
                               </asp:DropDownList>
                           </div>
                       </div>
                       <div class="row">
                           <div class="col-5 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Descripcion</span>
                               </div>
                               <input type="text" id="txtDescrip" maxlength="100" runat="server" class="form-control form-control-sm" />
                           </div>   
                           <div class="col-3 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Impuesto</span>
                               </div>
                               &nbsp&nbsp<asp:CheckBox ID="chkImpuesto" Checked="true" CssClass="form-check form-check-inline" runat="server" />
                           </div>  
                       </div>
                       <div class="row">
                           <div class="col-4 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Cod. barras</span>
                               </div>
                               <input type="text" id="txtBarras" maxlength="50" required="required" runat="server" class="form-control form-control-sm" />
                           </div>
                           <div class="col-1"></div>
                           <div class="col-3">
                               <div class="input-group input-group-sm">
                                   <div class="input-group-prepend">
                                       <span class="input-group-text">Franciones</span>
                                   </div>
                                   &nbsp&nbsp<asp:CheckBox ID="chkFranciones" Checked="true" CssClass="form-check form-check-inline" runat="server" />
                               </div>   
                           </div>                                     
                       </div>                       
                       <div class="row">
                           <div class="col-5 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Comentarios</span>
                               </div>
                               <asp:TextBox ID="txtComentario" maxlength="50" TextMode="MultiLine" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                           </div>
                           <div class="col-3">                                                             
                               <div class="input-group input-group-sm">
                                   <div class="input-group-prepend">
                                       <span class="input-group-text">Compra</span>
                                   </div>
                                   &nbsp&nbsp<asp:CheckBox ID="chkCompra" Checked="true" CssClass="form-check form-check-inline" runat="server" />
                               </div>
                               <div class="input-group input-group-sm" style="padding-top: 2px;">
                                   <div class="input-group-prepend">
                                       <span class="input-group-text">Venta</span>
                                   </div>
                                   &nbsp&nbsp<asp:CheckBox ID="chkVenta" Checked="true" CssClass="form-check form-check-inline" runat="server" />
                               </div>
                           </div>
                       </div>                     
                       </div>
                   </div>
               </div>           

           <div class="container-fluid" style="padding-top: 15px;" id="trClientes" runat="server" visible="false">
               <div class="row">
                   <div class="col">
                       <asp:TextBox ID="txtBuscarCliente" runat="server" CssClass="form-control form-control-sm" placeholder="Codigo o Nombre de cliente..." Width="50%" TextMode="SingleLine" OnTextChanged="txtBuscarCliente_TextChanged" AutoPostBack="true" />
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

           <div style="position:fixed; bottom:20px; padding-left:15px;" >               
               <asp:Label ID="lblMsg" runat="server"></asp:Label>
           </div>

       </div>     
    <!-- /#page-content-wrapper -->

  </div>
   <!-- /#wrapper -->

    <!-- Bootstrap core JavaScript -->
    <script src="vendor/jquery/jquery.min.js"></script>
    <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script src="js/jquery.maskedinput.js"></script>

    <!-- Menu Toggle Script -->
    <script>
    $("#menu-toggle").click(function(e) {
        e.preventDefault();
        $("#wrapper").toggleClass("toggled");
    });

    jQuery(function($){      
        $("#txtIdentidad").mask("9999-9999-99999");                
    });
    </script>
    </form>  
</body>
</html>
