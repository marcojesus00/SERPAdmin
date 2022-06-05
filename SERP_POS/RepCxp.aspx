<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RepCxp.aspx.vb" Inherits="SERP_POS.RepCxp" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cuentas por pagar</title>
 <!-- Bootstrap core CSS -->  
  <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
    <link rel="shortcut icon" type="image/x-icon" href="svgs/solid/drafting-compass.svg"/>

  <!-- Custom styles for this template -->
  <link href="css/simple-sidebar.css" rel="stylesheet"/>
  <link rel="stylesheet" href="css/all.min.css" />
    <style>
        .row {padding:1px;
        }  
        .Boton:hover{transform:scale(1.05);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

   <div class="d-flex" id="wrapper">

    <!-- Page Content -->
       <div id="page-content-wrapper">

           <nav class="navbar navbar-expand-lg navbar-light bg-light border-bottom">              
               <h4 style="width:100%; text-align:center">Cuentas por pagar</h4>
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

           <div class="container-fluid" id="PanelReimprimir" runat="server">
               <div style="padding-left: 5%; padding-right: 5%;">
                   <div class="row" style="background-color: white; padding-top: 15px;">
                       <div class="col-8">
                           <div class="input-group input-group-sm">
                               <asp:TextBox ID="txtBuscar" runat="server" class="form-control form-control-sm" placeholder="Cliente..." OnTextChanged="txtBuscar_TextChanged" AutoPostBack="true" />
                               <asp:DropDownList ID="dlVendedor" runat="server" CssClass="form-control form-control-sm" OnSelectedIndexChanged="txtBuscar_TextChanged" AutoPostBack="true"></asp:DropDownList>                               
                               <div class="input-group-append">
                                   <label class="input-group-text">
                                       <asp:LinkButton ID="btnBuscar" runat="server" class="fas fa-search text-secondary"></asp:LinkButton></label>
                               </div>
                           </div>
                       </div>                       
                       <div class="col">
                           <div style="padding-top:4px; text-align:right;">
                               <a id="btnExcel" runat="server" style="font-size:x-large" class="fas fa-file-download Boton text-dark"></a>&nbsp                            
                           </div>
                       </div> 
                   
                   </div>
                   <div class="row" style="background-color: white">
                       <div class="col">
                           <asp:GridView ID="GvPrincipal" runat="server" RowStyle-HorizontalAlign="Right" CssClass="table table-sm table-hover table-bordered" EmptyDataText="No se econtraron resultados" AllowSorting="true" AutoGenerateColumns="False">
                               <Columns>
                                   <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                       <ItemTemplate>
                                           <img alt="" src="svgs/solid/angle-down.svg" width="15" style="padding: 0px;" />
                                           <asp:Panel ID="pnlOrders" runat="server" Style="display: none; padding: 0px;">
                                               <asp:GridView ID="GVDetalle" runat="server" RowStyle-HorizontalAlign="Right" AutoGenerateColumns="false" CssClass="table-bordered table-info">
                                                   <Columns>
                                                       <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                                       <asp:BoundField DataField="Vencimiento" HeaderText="Vencimiento" />
                                                       <asp:BoundField DataField="Documento" HeaderText="Documento" />
                                                       <asp:BoundField DataField="Vendedor" HeaderText="Vendedor" />
                                                       <asp:BoundField DataField="Saldo" HeaderText="Saldo" />
                                                       <asp:BoundField DataField="Corriente" HeaderText="Corriente" />
                                                       <asp:BoundField DataField="30 a 60" HeaderText="30 a 60" />
                                                       <asp:BoundField DataField="60 a 90" HeaderText="60 a 90" />
                                                       <asp:BoundField DataField="90 a 120" HeaderText="90 a 120" />
                                                       <asp:BoundField DataField="120+" HeaderText="120+" />
                                                   </Columns>
                                               </asp:GridView>
                                           </asp:Panel>
                                       </ItemTemplate>
                                   </asp:TemplateField>
                                   <asp:BoundField DataField="Codigo" HeaderText="Codigo" SortExpression="Codigo" ItemStyle-HorizontalAlign="Left" />
                                   <asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" ItemStyle-HorizontalAlign="Left" />
                                   <asp:BoundField DataField="Saldo" HeaderText="Saldo" SortExpression="Saldo" />
                                   <asp:BoundField DataField="Corriente" HeaderText="Corriente" SortExpression="Corriente" />
                                   <asp:BoundField DataField="30 a 60" HeaderText="30 a 60" SortExpression="30 a 60" />
                                   <asp:BoundField DataField="60 a 90" HeaderText="60 a 90" SortExpression="60 a 90" />
                                   <asp:BoundField DataField="90 a 120" HeaderText="90 a 120" SortExpression="90 a 120" />
                                   <asp:BoundField DataField="120+" HeaderText="120+" SortExpression="120+" />
                               </Columns>
                           </asp:GridView>
                       </div>
                   </div>
               </div>
           </div>

           <div style="visibility: hidden">
            <asp:GridView ID="gvExcel" runat="server"></asp:GridView>
           </div>

           <div style="position:fixed; bottom:20px; padding-left:15px;" >               
               <asp:Label ID="lblMsg" runat="server"></asp:Label>
           </div>

           <div style="width:100%; height:40px; font-size:smaller; position:fixed; bottom:0px; background-color:#202020; left: 0px; color: white;"><div style="padding-top:10px;padding-left:10px;">
               <asp:Label ID="lblTotalDocumento" style="font-size:medium" runat="server"/>&nbsp&nbsp&nbsp<asp:Label ID="lblTotalVenta" style="font-size:medium" runat="server"/>&nbsp&nbsp&nbsp<asp:Label ID="lblTotalCosto" style="font-size:medium" runat="server"/>&nbsp&nbsp&nbsp<asp:Label ID="lblTotalGanancia" runat="server"/></div></div>

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
    <script src="js/Jquery1.8.3.js"></script>
    <script type="text/javascript">
        $("[src*=angle-down]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "svgs/solid/angle-up.svg");
        });
        $("[src*=angle-up]").live("click", function () {
            $(this).attr("src", "svgs/solid/angle-down.svg");
            $(this).closest("tr").next().remove();
        });
    </script>
    <script>
    $("#menu-toggle").click(function(e) {
        e.preventDefault();
        $("#wrapper").toggleClass("toggled");
    });     
    </script>
    </form>  
</body>
</html>
