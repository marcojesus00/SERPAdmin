<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ParUsu.aspx.vb" Inherits="SERP_POS.ParUsu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Usuarios</title>
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
        <asp:LinkButton Enabled="false" runat="server" ID="menuPagos" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-credit-card"></i>&nbsp Formas de Pago</asp:LinkButton>
        <asp:LinkButton Enabled="false" runat="server" ID="menuVendedores" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-users"></i>&nbsp Vendedores</asp:LinkButton>        
        <asp:LinkButton Enabled="false" runat="server" ID="MenuAlmacen" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-warehouse"></i>&nbsp Almacenes</asp:LinkButton>
        <asp:LinkButton Enabled="false" runat="server" ID="menuArticulos" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-dolly-flatbed"></i>&nbsp Articulos</asp:LinkButton>
        <asp:LinkButton Enabled="false" runat="server" ID="menuGrupoArt" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-boxes"></i>&nbsp Grupos de articulos</asp:LinkButton>
        <asp:LinkButton Enabled="false" runat="server" ID="menuPrecios" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-money-bill-alt"></i>&nbsp Precios</asp:LinkButton>        
        <asp:LinkButton Enabled="false" runat="server" ID="menuImpuestos" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-coins"></i>&nbsp Impuestos</asp:LinkButton>
        <asp:LinkButton Enabled="false" runat="server" ID="menuUsuarios" CssClass="list-group-item list-group-item-action bg-light text-primary"><i class="fas fa-user-edit"></i>&nbsp Usuarios</asp:LinkButton>                
        <asp:LinkButton Enabled="false" runat="server" ID="menuSeries" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-list-ol"></i>&nbsp Series</asp:LinkButton>
        <asp:LinkButton Enabled="false" runat="server" ID="menuGeneral" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-toggle-on"></i>&nbsp General</asp:LinkButton>         
      </div>
    </div>
    <!-- /#sidebar-wrapper -->

    <!-- Page Content -->
       <div id="page-content-wrapper">

           <nav class="navbar navbar-expand-lg navbar-light bg-light border-bottom">
               <button class="btn btn-light" id="menu-toggle"><i class="fas fa-bars" style="color: darkslategray"></i></button>
               <h4 style="width:100%; text-align:center">Usuarios</h4>
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

           <div class="container-fluid" id="trUsuarios" runat="server" visible="true">               
               <div class="row" id="trLista" runat="server" visible="true" style="padding-top: 15px;">                     
                   <div class="col-10">
                       <asp:GridView ID="GvEncargado" runat="server" CssClass="table table-sm table-bordered table-hover" EmptyDataText="No se econtraron listas de precios" AutoGenerateColumns="False">
                           <Columns>
                               <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-arrow-right text-primary" ShowSelectButton="True" SelectText=""></asp:CommandField>
                               <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-arrow-right text-primary" EditText="" ShowEditButton="True">
                                   <ControlStyle CssClass="fas fa-edit text-primary"></ControlStyle>
                               </asp:CommandField>
                               <asp:TemplateField HeaderText="Codigo">
                                   <EditItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("ID") %>' ID="ID"></asp:Label>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("ID") %>' ID="Label1"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField> 
                               <asp:TemplateField HeaderText="Usuario">
                                   <EditItemTemplate>
                                       <asp:TextBox runat="server" Text='<%# Bind("CodUsuario") %>' ID="Usuario"></asp:TextBox>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("CodUsuario") %>' ID="Label1"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>                              
                               <asp:TemplateField HeaderText="Nombre">
                                   <EditItemTemplate>
                                       <asp:TextBox runat="server" Text='<%# Bind("Nombre") %>' ID="Nombre"></asp:TextBox>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Nombre") %>' ID="Label2"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Funcion">
                                   <EditItemTemplate>
                                       <asp:DropDownList ID="Funcion" runat="server" CssClass="form-control form-control-sm" SelectedValue='<%# Bind("Funcion") %>'>
                                           <asp:ListItem Value="Limitada" Selected="True">Limitada</asp:ListItem>
                                           <asp:ListItem Value="Administrador">Administrador</asp:ListItem>
                                       </asp:DropDownList>                                       
                                   </EditItemTemplate>
                                   <ItemTemplate>                                       
                                       <asp:Label runat="server" Text='<%# Bind("Funcion") %>' ID="Label2"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Cargo">
                                   <EditItemTemplate>
                                       <asp:TextBox runat="server" Text='<%# Bind("Cargo") %>' ID="Cargo" CssClass="form-control form-control-sm"></asp:TextBox>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Cargo") %>' ID="Label2"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                                <asp:TemplateField HeaderText="Clave">
                                   <EditItemTemplate>
                                       <asp:TextBox runat="server" TextMode="Password" Text='<%# Bind("Pass") %>' ID="Pass" CssClass="form-control form-control-sm" style="width:100px"></asp:TextBox>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='' ID="Label2"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Activo">
                                   <EditItemTemplate>
                                       <asp:DropDownList ID="Activo" runat="server" CssClass="form-control form-control-sm" SelectedValue='<%# Bind("Activo") %>'>
                                           <asp:ListItem Value="Y" Selected="True">Y</asp:ListItem>
                                           <asp:ListItem Value="N">N</asp:ListItem>
                                       </asp:DropDownList>                                       
                                   </EditItemTemplate>
                                   <ItemTemplate>                                       
                                       <asp:Label runat="server" Text='<%# Bind("Activo") %>' ID="Label2"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                           </Columns>
                       </asp:GridView>
                   </div>
               </div>
           </div>        
           
           <div class="container-fluid" style="padding-top: 15px;" id="trAccesos" runat="server" visible="false">
               <div class="row">                   
                   <div class="col-10" style="text-align: right">
                       <asp:LinkButton ID="btnCerrar" runat="server" Style="font-size: x-large; vertical-align: central"><i class="far fa-times-circle text-secondary"></i></asp:LinkButton>&nbsp</div>
               </div>
               <div class="row" style="background-color: white">                   
                   <div class="col-10">
                       <asp:GridView ID="gvAccesos" runat="server" CssClass="table table-sm table-bordered table-hover" EmptyDataText="No se econtraron articulos" AutoGenerateColumns="False" AllowSorting="True">
                           <Columns>
                               <asp:CommandField EditText="" ShowEditButton="True">
                                   <ControlStyle CssClass="fas fa-edit text-primary"></ControlStyle>
                                   <ItemStyle HorizontalAlign="Center"></ItemStyle>
                               </asp:CommandField>
                               <asp:TemplateField HeaderText="Menu">                           
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Menu") %>' ID="Label1"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Pantalla">
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Pantalla") %>' ID="Label2"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                              <asp:TemplateField HeaderText="Activo">
                                   <EditItemTemplate>
                                       <asp:CheckBox ID="gvActivo" runat="server" Checked='<%# Bind("Activo") %>' />                                    
                                   </EditItemTemplate>
                                   <ItemTemplate>                                       
                                       <asp:CheckBox ID="gvtemp" runat="server" Enabled="false" Checked='<%# Bind("Activo") %>' />
                                   </ItemTemplate>
                               </asp:TemplateField>               
                           </Columns>
                       </asp:GridView>
                   </div>
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
