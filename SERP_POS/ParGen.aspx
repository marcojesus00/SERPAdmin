<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ParGen.aspx.vb" Inherits="SERP_POS.ParGen" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>General</title>
 <!-- Bootstrap core CSS -->
  <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
  <link rel="shortcut icon" type="image/x-icon" href="svgs/solid/drafting-compass.svg"/>

  <!-- Custom styles for this template -->
  <link href="css/simple-sidebar.css" rel="stylesheet"/>
  <link rel="stylesheet" href="css/all.min.css" />
    <style>
        .row .col .row {padding:1px;}
        .input-group-text {width: 90px;}        
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
        <asp:LinkButton Enabled="false" runat="server" ID="menuArticulos" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-dolly-flatbed"></i>&nbsp Articulos</asp:LinkButton>
        <asp:LinkButton Enabled="false" runat="server" ID="menuGrupoArt" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-boxes"></i>&nbsp Grupos de articulos</asp:LinkButton>
        <asp:LinkButton Enabled="false" runat="server" ID="menuPrecios" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-money-bill-alt"></i>&nbsp Precios</asp:LinkButton>       
        <asp:LinkButton Enabled="false" runat="server" ID="menuImpuestos" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-coins"></i>&nbsp Impuestos</asp:LinkButton>
        <asp:LinkButton Enabled="false" runat="server" ID="menuUsuarios" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-user-edit"></i>&nbsp Usuarios</asp:LinkButton>         
        <asp:LinkButton Enabled="false" runat="server" ID="menuSeries" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-list-ol"></i>&nbsp Series</asp:LinkButton>
        <asp:LinkButton Enabled="false" runat="server" ID="menuGeneral" CssClass="list-group-item list-group-item-action bg-light text-primary"><i class="fas fa-toggle-on"></i>&nbsp General</asp:LinkButton>         
      </div>
    </div>
    <!-- /#sidebar-wrapper -->

    <!-- Page Content -->
       <div id="page-content-wrapper">

           <nav class="navbar navbar-expand-lg navbar-light bg-light border-bottom">
               <button class="btn btn-light" id="menu-toggle"><i class="fas fa-bars" style="color: darkslategray"></i></button>
               <h4 style="width: 100%; text-align: center">General</h4>
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
               <div class="row" id="trAgregarClie" runat="server" visible="true" style="padding-top: 10px;">
                   <div class="col">
                       <div class="row">
                           <div class="col-4 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">ID</span>
                               </div>
                               <asp:TextBox TextMode="SingleLine" CssClass="form-control form-control-sm" ID="TxtID" required="required" runat="server"/>
                           </div>
                           <div class="col-1"></div>
                           <div class="col-3 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text" style="width: 135px">Tipo papel</span>
                               </div>
                               <asp:DropDownList ID="DlPapel" runat="server" CssClass=" form-control form-control-sm">
                                   <asp:ListItem Selected="True" Value="Listin">Listin</asp:ListItem>
                                   <asp:ListItem Value="Carta">Carta</asp:ListItem>
                               </asp:DropDownList>
                           </div>
                           <div class="col" style="text-align: right">
                               <asp:Button ID="BtnGuardar" runat="server" CssClass="btn btn-sm btn-primary" Text="  Guardar  " />
                           </div>
                       </div>
                       <div class="row">
                           <div class="col-5 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Nombre</span>
                               </div>
                               <asp:TextBox TextMode="SingleLine" ID="TxtNombre" required="required" runat="server" CssClass="form-control form-control-sm" />
                           </div>
                           <div class="col-3 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text" style="width: 135px">Forma impresion</span>
                               </div>
                               <asp:DropDownList ID="DlImpresion" runat="server" CssClass="form-control form-control-sm">
                                   <asp:ListItem>Directo</asp:ListItem>
                                   <asp:ListItem>Indirecto</asp:ListItem>
                               </asp:DropDownList>
                           </div>
                       </div>
                       <div class="row">
                           <div class="col-5 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Alias</span>
                               </div>
                               <asp:TextBox ID="TxtAlias" runat="server" class="form-control form-control-sm" />
                           </div>
                           <div class="col-3 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text" style="width: 135px">Numero copias</span>
                               </div>
                               <asp:TextBox TextMode="Number" CssClass="form-control form-control-sm" ID="TxtCopias" required="required" runat="server" />
                           </div>
                       </div>
                       <div class="row">
                           <div class="col-4 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Telefono</span>
                               </div>
                               <asp:TextBox ID="TxtTelefono" required="required" runat="server" CssClass="form-control form-control-sm" />
                           </div>
                           <div class="col-1"></div>
                           <div class="col-4 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text" style="width: 135px">Nombre impresora</span>
                               </div>
                               <asp:TextBox TextMode="SingleLine" CssClass="form-control form-control-sm" ID="TxtNomImpresora" required="required" runat="server" />
                           </div>
                       </div>
                       <div class="row">
                           <div class="col-4 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Email</span>
                               </div>
                               <asp:TextBox ID="TxtEmail" TextMode="Email" required="required" runat="server" CssClass="form-control form-control-sm" />
                           </div>
                           <div class="col-1"></div>
                           <div class="col-3 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text" style="width: 135px">Inventario negativo</span>
                               </div>
                               &nbsp&nbsp<asp:CheckBox ID="chkInventario" Checked="true" CssClass="form-check form-check-inline" runat="server" />
                           </div>
                       </div>
                       <div class="row">
                           <div class="col-4 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Web</span>
                               </div>
                               <asp:TextBox ID="TxtWeb" required="required" runat="server" CssClass="form-control form-control-sm" />
                           </div>
                           <div class="col-1"></div>
                           <div class="col-3 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text" style="width: 135px">Tiempo session</span>
                               </div>
                               <asp:TextBox TextMode="Number" CssClass="form-control form-control-sm" ID="TxtTiempo" required="required" runat="server" />
                               <div class="input-group-append">
                                   <span class="input-group-text" style="width:40px">Min</span>
                               </div>
                           </div>
                       </div>
                       <div class="row">
                           <div class="col-5 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Direccion</span>
                               </div>
                               <asp:TextBox ID="TxtDireccion" TextMode="MultiLine" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                           </div>
                           <div class="col-3">
                               <div class="input-group input-group-sm">
                                   <div class="input-group-prepend">
                                       <span class="input-group-text" style="width: 135px">Serie pago efectuado</span>
                                   </div>
                                   <asp:TextBox TextMode="Number" CssClass="form-control form-control-sm" ID="txtSerieComp" required="required" runat="server" />
                               </div>
                               <div class="input-group input-group-sm" style="padding-top:2px;">
                                   <div class="input-group-prepend">
                                       <span class="input-group-text" style="width: 135px">Serie pago recibido</span>
                                   </div>
                                   <asp:TextBox TextMode="Number" CssClass="form-control form-control-sm" ID="txtSerieVen" required="required" runat="server" />
                               </div>
                           </div>
                       </div>
                       <div class="row">
                             <div class="col-5 input-group input-group-sm">
                               <div class="input-group-prepend">
                                   <span class="input-group-text">Mensaje</span>
                               </div>
                               <asp:TextBox ID="txtMensaje" TextMode="MultiLine" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                           </div>                           
                       </div>
                       <div class="row">
                           <div class="col-5">
                               <div class="input-group input-group-sm">
                                   <asp:FileUpload ID="FuLogo" CssClass="form-control form-control-file" runat="server" Style="height: 50px; padding: 10px 10px 10px 10px;" name="filename" onchange="return validarExt()" />
                               </div>
                           </div>
                       </div>
                       <div class="row">
                           <div class="col-5" id="visorArchivo">
                               <embed id="imgFoto" runat="server" class="foto" height="160" src="~/imagenes/No_Tiene.png" onclick="window.open('imagenes/logo.png','_blank','scrollbars=yes,resizable=yes,top=5,left=5,width=700,height=700')" />
                           </div>
                       </div>
                   </div>
               </div>
           </div>

           <div style="position:fixed; bottom:20px; padding-left:30%;" >               
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

    function validarExt() {
            var archivoInput = document.getElementById('FuLogo');
            var archivoRuta = archivoInput.value;
            var extPermitidas = /(.jpg)$/i;

            if (!extPermitidas.exec(archivoRuta)) {
                alert('Asegurese de haber seleccionado una Imagen JPG');
                archivoInput.value = '';
                return false;
            }
        
            else {
                //Mostrar Imagen
                if (archivoInput.files && archivoInput.files[0]) {
                    var visor = new FileReader();
                    visor.onload = function (e) {
                        document.getElementById("imgFoto").visible = "False";
                        document.getElementById('visorArchivo').innerHTML = '<embed src="' + e.target.result + '" height="160"/>';
                    };
                    visor.readAsDataURL(archivoInput.files[0]);
                }
                }
            }
    </script>
    </form>  
</body>
</html>
