<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FinPer.aspx.vb" Inherits="SERP_POS.FinPer" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Periodo Contable</title>
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
      </div>
    </div>
    <!-- /#sidebar-wrapper -->

    <!-- Page Content -->
       <div id="page-content-wrapper">

           <nav class="navbar navbar-expand-lg navbar-light bg-light border-bottom">
               <button class="btn btn-light" id="menu-toggle"><i class="fas fa-bars" style="font-size: x-large; color: darkslategray"></i></button>
               <h4 style="width:100%; text-align:center">Periodo Contable</h4>
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
                       <asp:GridView ID="gvDetalle" runat="server" CssClass="table table-sm table-bordered table-hover" AutoGenerateColumns="False">
                           <Columns>
                               <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="fas fa-edit text-primary" EditText="" ShowEditButton="True">
                                   <ControlStyle CssClass="fas fa-edit text-primary"></ControlStyle>
                                   <ItemStyle HorizontalAlign="Center"></ItemStyle>
                               </asp:CommandField>
                               <asp:TemplateField HeaderText="ID">
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("ID") %>' ID="glblID"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Descripcion">
                                   <EditItemTemplate>
                                       <asp:TextBox CssClass="form-control form-control-sm" TextMode="SingleLine" MaxLength="50" runat="server" Text='<%# Bind("Descripcion") %>' ID="gtxtDescripcion"></asp:TextBox>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Descripcion") %>' ID="Label2"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Documento desde">
                                   <EditItemTemplate>
                                       <asp:TextBox CssClass="form-control form-control-sm" TextMode="Date" runat="server" Text='<%# Bind("FechaDoc1") %>' ID="gtxtFechaDoc1"></asp:TextBox>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("FechaDoc1") %>' ID="Label3"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Hasta">
                                   <EditItemTemplate>
                                       <asp:TextBox CssClass="form-control form-control-sm" TextMode="Date" runat="server" Text='<%# Bind("FechaDoc2") %>' ID="gtxtFechaDoc2"></asp:TextBox>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("FechaDoc2") %>' ID="Label4"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Vencimiento desde">
                                   <EditItemTemplate>
                                       <asp:TextBox CssClass="form-control form-control-sm" TextMode="Date" runat="server" Text='<%# Bind("FechaVen1") %>' ID="gtxtFechaVen1"></asp:TextBox>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("FechaVen1") %>' ID="Label3"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Hasta">
                                   <EditItemTemplate>
                                       <asp:TextBox CssClass="form-control form-control-sm" TextMode="Date" runat="server" Text='<%# Bind("FechaVen2") %>' ID="gtxtFechaVen2"></asp:TextBox>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("FechaVen2") %>' ID="Label4"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Estado">
                                   <EditItemTemplate>
                                       <asp:DropDownList runat="server" Text='<%# Bind("Estado") %>' ID="gdlEstado">
                                           <asp:ListItem Selected="True" Value="A">A</asp:ListItem>
                                           <asp:ListItem Value="C">C</asp:ListItem>
                                       </asp:DropDownList>
                                   </EditItemTemplate>
                                   <ItemTemplate>
                                       <asp:Label runat="server" Text='<%# Bind("Estado") %>' ID="Label7"></asp:Label>
                                   </ItemTemplate>
                               </asp:TemplateField>
                           </Columns>
                       </asp:GridView>
                   </div>
               </div>
           </div>                

           <div id="PanelNuevo" runat="server" visible="false" class="container-fluid" style="background-color: rgba(0,0,0,0.6); position: absolute; left: 0; top: 0; bottom: 0">
               <div style="padding-left: 25%; padding-right: 25%; padding-top: 40px;">
                   <div class="row rounded" style="background-color: white; padding: 5px 20px 15px 20px">
                       <div class="col" style="width: 100%; text-align: center">
                           <div class="row">
                               <div class="col">
                                   <asp:Label Font-Size="Large" Width="100%" ID="lblTituloT" runat="server"></asp:Label>
                               </div>
                           </div>
                           <div class="row">
                               <div class="col">
                                   <div class="input-group input-group-sm">
                                       <div class="input-group-prepend">
                                           <label for="txtDescripcion" style="width:140px;" class="input-group-text">Descripcion</label>
                                       </div>
                                       <asp:TextBox ID="txtDescripcion" TextMode="SingleLine" MaxLength="50" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                   </div>
                               </div>
                           </div>
                           <div class="row">
                               <div class="col">
                                   <div class="input-group input-group-sm">
                                       <div class="input-group-prepend">
                                           <label for="txtFechaDoc1" style="width:140px;" class="input-group-text">Documento Desde</label>
                                       </div>
                                       <asp:TextBox ID="txtFechaDoc1" runat="server" TextMode="Date" CssClass="form-control form-control-sm text-left"></asp:TextBox>
                                   </div>
                               </div>
                           </div>
                           <div class="row">
                               <div class="col">
                                   <div class="input-group input-group-sm">
                                       <div class="input-group-prepend">
                                           <label for="txtFechaDoc2" style="width:140px;" class="input-group-text">Hasta</label>
                                       </div>
                                       <asp:TextBox ID="txtFechaDoc2" runat="server" TextMode="Date" CssClass="form-control form-control-sm"></asp:TextBox>
                                   </div>
                               </div>
                           </div>
                           <div class="row">
                               <div class="col">
                                   <div class="input-group input-group-sm">
                                       <div class="input-group-prepend">
                                           <label for="txtFechaVen1" style="width:140px;" class="input-group-text">Vencimiento Desde</label>
                                       </div>
                                       <asp:TextBox ID="txtFechaVen1" runat="server" TextMode="Date" CssClass="form-control form-control-sm text-left"></asp:TextBox>
                                   </div>
                               </div>
                           </div>
                           <div class="row">
                               <div class="col">
                                   <div class="input-group input-group-sm">
                                       <div class="input-group-prepend">
                                           <label for="txtFechaVen2" style="width:140px;" class="input-group-text">Hasta</label>
                                       </div>
                                       <asp:TextBox ID="txtFechaVen2" runat="server" TextMode="Date" CssClass="form-control form-control-sm text-left"></asp:TextBox>
                                   </div>
                               </div>
                           </div>
                           <div class="row">
                               <div class="col">
                                   <div class="input-group input-group-sm">
                                       <div class="input-group-prepend">
                                           <label for="dlEstado" style="width:140px;" class="input-group-text">Estado</label>
                                       </div>
                                       <asp:DropDownList ID="dlEstado" runat="server" CssClass="form-control form-control-sm text-left">
                                           <asp:ListItem Selected="True">A</asp:ListItem>
                                           <asp:ListItem>C</asp:ListItem>
                                       </asp:DropDownList>
                                   </div>
                               </div>
                           </div>
                           <div class="row-cols">&nbsp</div>
                           <div class="row">
                               <div class="col" style="text-align: center;">
                                   <button type="button" class="btn btn-secondary btn-sm" id="btnCancelar" runat="server">Cancelar</button>&nbsp&nbsp<asp:Button ID="btnAgregar" runat="server" Text="  Crear  " CssClass="btn btn-primary btn-sm" />
                               </div>
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
