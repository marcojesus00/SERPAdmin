<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ParSer.aspx.vb" Inherits="SERP_POS.ParSer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Series</title>
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
                    <asp:LinkButton Enabled="false" runat="server" ID="menuUsuarios" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-user-edit"></i>&nbsp Usuarios</asp:LinkButton>
                    <asp:LinkButton Enabled="false" runat="server" ID="menuSeries" CssClass="list-group-item list-group-item-action bg-light text-primary"><i class="fas fa-list-ol"></i>&nbsp Series</asp:LinkButton>
                    <asp:LinkButton Enabled="false" runat="server" ID="menuGeneral" CssClass="list-group-item list-group-item-action bg-light"><i class="fas fa-toggle-on"></i>&nbsp General</asp:LinkButton>
                </div>
            </div>
            <!-- /#sidebar-wrapper -->

            <!-- Page Content -->
            <div id="page-content-wrapper">

                <nav class="navbar navbar-expand-lg navbar-light bg-light border-bottom">
                    <button class="btn btn-light" id="menu-toggle"><i class="fas fa-bars" style="color: darkslategray"></i></button>
                    <h4 style="width: 100%; text-align: center">Series de numeracion</h4>
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
                    <div class="row" id="trLista" runat="server" visible="true" style="padding-top: 15px;">
                        <div class="col-8">
                            <asp:GridView ID="GvLista" runat="server" CssClass="table table-sm table-bordered table-hover" EmptyDataText="No se econtraron listas de precios" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-arrow-right text-primary" ShowSelectButton="True" SelectText=""></asp:CommandField>
                                    <asp:TemplateField HeaderText="Codigo">
                                        <EditItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("Codigo") %>' ID="gvListCod"></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("Codigo") %>' ID="Label1"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo">
                                        <EditItemTemplate>
                                            <asp:TextBox runat="server" Text='<%# Bind("Tipo") %>' ID="gvListNom"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("Tipo") %>' ID="Label2"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>

                <div class="container-fluid" style="padding-top: 15px;" id="trPrecios" runat="server" visible="false">
                    <div class="row">
                        <div class="col">
                            <asp:TextBox ID="txtBuscarArticulo" runat="server" CssClass="form-control form-control-sm" placeholder="Codigo o Nombre de cliente..." Width="100%" TextMode="SingleLine" OnTextChanged="txtBuscarArticulo_TextChanged" AutoPostBack="true" />
                        </div>                        
                        <div class="col" style="text-align: right;">
                            <asp:LinkButton ID="btnCerrar" runat="server" Style="vertical-align: central;"><i class="fas fa-times text-secondary"></i></asp:LinkButton>&nbsp
                        </div>
                    </div>

                    <div class="row" style="background-color: white">
                        <div class="col">
                            <asp:GridView ID="gvPrecios" runat="server" CssClass="table table-sm table-bordered table-hover" EmptyDataText="No se econtraron articulos" AutoGenerateColumns="False" AllowSorting="True">
                                <Columns>
                                    <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-edit text-primary" ShowEditButton="True" EditText=""></asp:CommandField>
                                    <asp:TemplateField HeaderText="Codigo" SortExpression="Codigo">
                                        <EditItemTemplate>
                                            <asp:Label ID="gvCodigo" runat="server" Text='<%# Bind("Codigo") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Codigo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descripcion" SortExpression="Descripcion">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="gvDescripcion" runat="server" TextMode="MultiLine" CssClass="form-control form-control-sm" Text='<%# Bind("Descripcion") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("Descripcion") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Inicio" SortExpression="Inicio">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="gvInicio" runat="server" CssClass="form-control form-control-sm" Text='<%# Bind("Inicio") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("Inicio") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Final" SortExpression="Final">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="gvFinal" runat="server" TextMode="Number" CssClass="form-control form-control-sm" Text='<%# Bind("Final") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("Final") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Siguiente" SortExpression="Siguiente">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="gvSiguiente" TextMode="Number" runat="server" CssClass="form-control form-control-sm" Text='<%# Bind("Siguiente") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label6" runat="server" TextMode="Number" Text='<%# Bind("Siguiente") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Prefijo" SortExpression="Prefijo">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="gvPrefijo" runat="server" CssClass="form-control form-control-sm" Text='<%# Bind("Prefijo") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label7" runat="server" Text='<%# Bind("Prefijo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Digitos" SortExpression="Digitos" ControlStyle-Width="60px">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="gvDigitos" runat="server" TextMode="Number" CssClass="form-control form-control-sm" Text='<%# Bind("Digitos") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label8" runat="server" Text='<%# Bind("Digitos") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Desde" SortExpression="Desde">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="gvDesde" runat="server" TextMode="Date" CssClass="form-control form-control-sm" Width="120px" Text='<%# Bind("Desde") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label9" runat="server" Text='<%# Bind("Desde") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hasta" SortExpression="Hasta">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="gvHasta" runat="server" TextMode="Date" CssClass="form-control form-control-sm" Width="120px" Text='<%# Bind("Hasta") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label10" runat="server" Text='<%# Bind("Hasta") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="info" SortExpression="info">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="gvinfo" runat="server" TextMode="MultiLine" CssClass="form-control form-control-sm" Text='<%# Bind("info") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label12" runat="server" Text='<%# Bind("info") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Activo" SortExpression="Activo" ControlStyle-Width="60px">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="gvActivo" runat="server" CssClass="form-control form-control-sm" Text='<%# Bind("Activo") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label13" runat="server" Text='<%# Bind("Activo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <asp:Label runat="server" ID="lblMSG"></asp:Label>
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
