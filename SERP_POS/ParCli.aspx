<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ParCli.aspx.vb" Inherits="SERP_POS.ParCli" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Clientes</title>
 <!-- Bootstrap core CSS -->
  <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
  <link rel="shortcut icon" type="image/x-icon" href="svgs/solid/drafting-compass.svg"/>

  <!-- Custom styles for this template -->
  <link href="css/simple-sidebar.css" rel="stylesheet"/>
  <link rel="stylesheet" href="css/all.min.css" />
    <style>
        .row .col .row {padding:1px;}
        .input-group-text {width: 90px;}
        #btnAgregarCli, #btnBuscarCli {padding-left: 15px;}
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
        <asp:LinkButton Enabled="false" runat="server" ID="menuClientes" CssClass="list-group-item list-group-item-action bg-light text-primary"><i class="fas fa-address-book"></i>&nbsp Clientes</asp:LinkButton>
        <asp:LinkButton Enabled="false" runat="server" id="menuPagos" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-credit-card"></i>&nbsp Formas de Pago</asp:LinkButton>
        <asp:LinkButton Enabled="false" runat="server" id="menuVendedores" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-users"></i>&nbsp Vendedores</asp:LinkButton>        
        <asp:LinkButton Enabled="false" runat="server" id="MenuAlmacen" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-warehouse"></i>&nbsp Almacenes</asp:LinkButton>
        <asp:LinkButton Enabled="false" runat="server" ID="menuArticulos" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-dolly-flatbed"></i>&nbsp Articulos</asp:LinkButton>
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
               <h4 style="width:100%; text-align:center">Clientes</h4>
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
                       <asp:LinkButton ID="btnAgregarCli" runat="server"><i class="far fa-file-alt" ></i></asp:LinkButton>
                       <asp:LinkButton ID="btnBuscarCli" runat="server"><i runat="server" id="imgClientes" class="fas fa-search"></i></asp:LinkButton>
                   </div>
                   <div class="col" style="text-align: right">
                       <asp:Button ID="btnGuardarCli" runat="server" CssClass="btn btn-sm btn-primary" Text="   Crear   " />
                   </div>
               </div>
               <div class="row" id="trAgregarClie" runat="server" visible="true" style="padding-top: 10px;">
                   <div class="col">
                       <div class="row">
                           <div class="col-4 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Numeracion</span>
                               </div>
                               <asp:DropDownList CssClass="form-control form-control-sm" ID="dlNumeracion" runat="server" ToolTip="Serie.." OnSelectedIndexChanged="DlNumeracion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                               <asp:TextBox TextMode="SingleLine" CssClass="form-control form-control-sm" ID="txtNumeracion" required="true" runat="server" ToolTip="Numero de cliente..." OnTextChanged="TxtNumeracion_TextChanged" AutoPostBack="true" />
                               <div class="input-group-append">
                                   <asp:LinkButton ID="BtnMov" runat="server" Visible="false" style="padding-top:5px; padding-left:5px;"><i class="fas fa-arrow-right" style="font-size:22px;"></i></asp:LinkButton>                                  
                               </div>                               
                           </div>
                           <div class="col-1"></div>
                           <div class="col-3 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Tipo</span>
                               </div>
                               <asp:DropDownList ID="dlTipo" runat="server" CssClass="form-control form-control-sm">
                                   <asp:ListItem Selected="True" Value="C">Cliente</asp:ListItem>
                                   <asp:ListItem Value="P">Proveedor</asp:ListItem>
                               </asp:DropDownList>
                           </div>
                           <div class="col-2 input-group input-group-sm">
                               <div class="input-group-prepend" style="width: 60px">
                                   <span class="input-group-text">Saldo</span>
                               </div>
                               <label id="lblSaldo" runat="server" class="form-control form-control-sm" />
                           </div>
                       </div>
                       <div class="row">
                           <div class="col-5 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Nombre</span>
                               </div>
                               <input type="text" id="txtNombreCli" required="required" runat="server" class="form-control form-control-sm" />
                           </div>
                           <div class="col-3 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Grupo</span>
                               </div>
                               <asp:DropDownList ID="dlGrupo" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                           </div>
                           <div class="col-2 input-group input-group-sm">
                               <div class="input-group-prepend" style="width: 60px">
                                   <span class="input-group-text">Activo</span>
                               </div>
                               <asp:DropDownList ID="dlEsatdo" runat="server" CssClass="form-control form-control-sm">
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
                               <input type="text" id="txtDescripCli" runat="server" class="form-control form-control-sm" />
                           </div>
                           <div class="col-3 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Forma pago</span>
                               </div>
                               <asp:DropDownList ID="dlPago" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                           </div>
                       </div>
                       <div class="row">
                           <div class="col-4 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Identidad</span>
                               </div>
                               <input type="text" id="txtIdentidad" required="required" runat="server" class="form-control form-control-sm" />
                           </div>
                           <div class="col-1"></div>
                           <div class="col-3 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Credito</span>
                               </div>
                               <asp:TextBox TextMode="Number" ID="txtCredito" runat="server" CssClass="form-control form-control-sm" />
                           </div>
                       </div>
                       <div class="row">
                           <div class="col-4 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">RTN</span>
                               </div>
                               <input type="text" id="txtRTN" runat="server" class="form-control form-control-sm" />
                           </div>
                           <div class="col-1"></div>
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
                                   <span class="input-group-text">Contacto</span>
                               </div>
                               <input type="text" id="txtContacto" runat="server" class="form-control form-control-sm" />
                           </div>
                           <div class="col-3 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Impuesto</span>
                               </div>
                               <asp:DropDownList ID="dlImpuesto" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                           </div>
                       </div>
                       <div class="row">
                           <div class="col-4 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Telefono</span>
                               </div>
                               <input type="text" id="txtTelefono" runat="server" class="form-control form-control-sm" />
                           </div>
                           <div class="col-1"></div>
                           <div class="col-3 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Vendedor</span>
                               </div>
                               <asp:DropDownList ID="dlVendedor" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                           </div>
                       </div>
                       <div class="row">
                           <div class="col-4 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Correo</span>
                               </div>
                               <input type="text" id="txtCorreo" runat="server" class="form-control form-control-sm" />
                           </div>
                       </div>
                       <div class="row">
                           <div class="col-5 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Depto</span>
                               </div>
                               <asp:DropDownList ID="dlDepto" runat="server" CssClass="form-control form-control-sm" OnSelectedIndexChanged="dlDepto_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                           </div>
                       </div>
                       <div class="row">
                           <div class="col-5 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Municipio</span>
                               </div>
                               <asp:DropDownList ID="dlMunicipio" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                           </div>
                       </div>
                       <div class="row">
                           <div class="col-5 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Direccion</span>
                               </div>
                               <asp:TextBox ID="txtDireccion" TextMode="MultiLine" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
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

           <div class="container-fluid" style="padding-top: 15px;" id="trMov" runat="server" visible="false">
               <div class="row">
                   <div class="col">
                       <div class="input-group input-group-sm">
                           <asp:TextBox ID="TxtF1" runat="server" TextMode="Date" class="form-control form-control-sm"></asp:TextBox>
                           <asp:TextBox ID="TxtF2" runat="server" TextMode="Date" class="form-control form-control-sm"></asp:TextBox>
                           <div class="input-group-append">
                               <label class="input-group-text" style="width:30px">
                                   <asp:LinkButton ID="btnBuscarMov" runat="server" class="fas fa-search text-secondary"></asp:LinkButton></label>
                           </div>
                       </div>
                   </div>
                   <div class="col" style="text-align: right">
                       <asp:LinkButton ID="btnCancelarMov" runat="server"><i class="far fa-times-circle text-secondary" style="font-size: 24px;"></i></asp:LinkButton>
                   </div>
               </div>
               <div class="row" style="background-color: white">
                   <div class="col">
                       <asp:GridView ID="GvMov" runat="server" CssClass="table table-sm table-bordered table-hover" EmptyDataText="No se econtraron registros">                    
                       </asp:GridView>
                   </div>
               </div>
           </div>

           <div class="container-fluid" style="padding-top: 5px;">
               <div runat="server" id="DivMSG" role="alert">
                  <p><asp:Label ID="lblMsg" runat="server"></asp:Label></p>
               </div>
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
