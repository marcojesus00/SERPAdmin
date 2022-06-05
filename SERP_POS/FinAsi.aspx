<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FinAsi.aspx.vb" Inherits="SERP_POS.FinAsi" EnableEventValidation="false" %>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FinAsi.aspx.vb" Inherits="SERP_POS.FinAsi" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Asiento Contable</title>
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
      <div class="sidebar-heading text-primary" style="font-weight:bolder">Finanzas</div>
      <div class="list-group list-group-flush">        
        <a runat="server" id="barNuevo" class="list-group-item list-group-item-action bg-light"><i class="far fa-file-alt" style="font-size:large; text-align:center"></i>&nbsp Nuevo</a>
        <a runat="server" id="barBuscar" class="list-group-item list-group-item-action bg-light"><i class="fas fa-search" style="font-size:large; text-align:center"></i>&nbsp Buscar</a>
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
               <h4 style="width:100%; text-align:center">Asiento Contable <asp:Label ID="lblEstado" runat="server" Text="(Abierto)" style="font-size:medium"></asp:Label></h4>
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
                       &nbsp
                   </div>                   
                   <div class="col" style="text-align: right">
                       <asp:Button ID="btnCrear" runat="server" Text="    Crear    " CssClass="btn btn-sm btn-primary" />
                   </div>
               </div>
               <div class="row">                  
                   <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="txtFecha" class="input-group-text" style="width: 95px">Documento</label>
                           </div>
                           <input type="date" class="form-control form-control-sm" id="txtFecha" runat="server" />
                       </div>
                   </div>
                   <div class="col">&nbsp</div>
                   <div class="col">&nbsp</div>
               </div>
               <div class="row">
                   <div class="col">
                       <div class="input-group input-group-sm">
                           <div class="input-group-prepend">
                               <label for="txtVencimiento" class="input-group-text" style="width: 95px">Vencimiento</label>
                           </div>
                           <input type="date" class="form-control form-control-sm" id="txtVencimiento" runat="server" />
                       </div>
                   </div>
                   <div class="col">&nbsp</div>
                   <div class="col">&nbsp</div>
               </div>
               <div class="row" style="padding-top: 15px">
                   <div class="col">
                       <div class="input-group input-group-sm">
                           <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control form-control-sm" placeholder="Buscar cuenta..." OnTextChanged="TxtBuscar_TextChanged" AutoPostBack="true" />
                           <div class="input-group-append">
                               <label class="input-group-text">
                                   <asp:LinkButton ID="btnBuscarProductos2" runat="server" CssClass="fas fa-search text-secondary"></asp:LinkButton></label>
                           </div>
                       </div>
                   </div>
                   <div class="col">&nbsp</div>
                   <div class="col">&nbsp</div>
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
                               <asp:TemplateField HeaderText="Cuenta">                                  
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Cuenta") %>' ID="Label1"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Nombre">
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Nombre") %>' ID="Label2"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Debe">
                                   <EditItemTemplate>
                                       <asp:TextBox CssClass="form-control form-control-sm" TextMode="Number" runat="server" Text='<%# Bind("Debe") %>' ID="gvtxtDebe"></asp:TextBox>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Debe") %>' ID="Label3"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Haber">
                                   <EditItemTemplate>
                                       <asp:TextBox CssClass="form-control form-control-sm" TextMode="Number" runat="server" Text='<%# Bind("Haber") %>' ID="gvtxtHaber"></asp:TextBox>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Haber") %>' ID="Label4"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>                     
                               <asp:TemplateField HeaderText="Asociado">
                                   <EditItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Asociado") %>' ID="gvtxtAsociado"></asp:Label>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Asociado") %>' ID="Label7"></asp:Label>
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
                               <asp:TextBox ID="txtProductos" runat="server" CssClass="form-control form-control-sm" placeholder="Buscar cuenta..." OnTextChanged="TxtProductos_TextChanged" AutoPostBack="true" />
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
                               <span class="input-group-text">Cuenta</span>
                           </div>
                           <label id="txtCuenta" runat="server" class="form-control form-control-sm"></label>
                       </div>
                       <div class="col input-group input-group-sm">
                           <div class="input-group-prepend">
                               <span class="input-group-text">Debe</span>
                           </div>
                           <asp:TextBox ID="txtDebe" runat="server" CssClass="form-control form-control-sm" TextMode="Number" />
                       </div>
                       <div class="col input-group input-group-sm">
                           <div class="input-group-prepend">
                               <span class="input-group-text">Haber</span>
                           </div>
                           <asp:TextBox ID="txtHaber" runat="server" CssClass="form-control form-control-sm" TextMode="Number" />
                       </div>    
                        <div class="col input-group input-group-sm" style="display:none">
                           <div class="input-group-prepend">
                               <span class="input-group-text">Nombre</span>
                           </div>
                           <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control form-control-sm" TextMode="SingleLine" />
                       </div>
                       <div class="col input-group input-group-sm" style="display:none">
                           <div class="input-group-prepend">
                               <span class="input-group-text">Grupo</span>
                           </div>
                           <asp:TextBox ID="txtGrupo" runat="server" CssClass="form-control form-control-sm" TextMode="SingleLine" />
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
                                   <asp:BoundField DataField="Cuenta" HeaderText="Cuenta"></asp:BoundField>
                                   <asp:BoundField DataField="Nombre" HeaderText="Nombre"></asp:BoundField>
                                   <asp:BoundField DataField="Saldo" HeaderText="Saldo"></asp:BoundField>
                                   <asp:BoundField DataField="Grupo" HeaderText="Grupo"></asp:BoundField>
                               </Columns>
                           </asp:GridView>
                       </div>
                   </div>
               </div>               
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
                       <span class="input-group-text" style="width: 80px">Debe</span>
                   </div>
                   <label id="lblTDebe" runat="server" class="form-control form-control-sm">0.00</label>
               </div>
               <div class="row input-group input-group-sm">
                   <div class="input-group-prepend">
                       <span class="input-group-text" style="width: 80px">Haber</span>
                   </div>
                   <label id="lblTHaber" runat="server" class="form-control form-control-sm">0.00</label>                  
               </div>                           
               <div class="row input-group input-group-sm">
                   <div class="input-group-prepend">
                       <span class="input-group-text" style="width: 80px">Diferencia</span>
                   </div>
                   <label id="lblTDiferencia" runat="server" class="form-control form-control-sm">0.00</label>                   
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

           <div class="container-fluid" style="padding-top: 5px;">
               <div runat="server" id="DivMSG" role="alert">
                  <asp:Label ID="lblMsg" runat="server"></asp:Label>
               </div>
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
