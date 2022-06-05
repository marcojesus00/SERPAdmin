<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Inicio.aspx.vb" Inherits="SERP_POS.Inicio" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>SERP</title>    
    <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
    <link rel="shortcut icon" type="image/x-icon" href="svgs/solid/drafting-compass.svg"/>
 
    <script src="js/all.min.js"></script>
        <style type="text/css"> 
        .Boton:hover{
            transform:scale(1.05);
        }
            p {font-size:small;
                width:100%;
                text-align:center;                
            }
        .card-link{
            color:black;            
        }
    </style>
    
</head>
<body style="background-image:url('imagenes/3412131.jpg');background-repeat:no-repeat; background-size:cover" onload="redireccionar()">    
    <form id="form1" runat="server" defaultbutton ="btnAceptarUsu">     

        <nav class="navbar navbar-expand-lg navbar-dark bg-dark border-bottom"> 
            <div class="navbar-brand">
                <asp:LinkButton ID="btnVersion" runat="server" CssClass="d-inline-block align-top"><i class="fas fa-drafting-compass" style="font-size: x-large; color:white"></i></asp:LinkButton>&nbsp SERP
            </div>
            
            <h5 style="width: 100%; text-align: center"><asp:Label runat="server" ID="lblEstacion" style="color:white"></asp:Label></h5>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="navbar-nav ml-auto mt-2 mt-lg-0">                    
                     <li class="nav-item active">
                        <asp:LinkButton ID="btnLogOut" runat="server" CssClass="btn btn-dark" Style="width:100px">salir&nbsp<i class="fas fa-door-open" style="font-size: x-large; color:white"></i></asp:LinkButton>
                    </li>    
                </ul>
            </div>
        </nav>

        <div class="row container-fluid" style="padding-top: 10px;">
            <div class="col-3">
                <div id="accordion" class="shadow-sm">
                    <div class="card">
                        <div class="card-header">
                            <a class="collapsed card-link btn-block" data-toggle="collapse" href="#collapseTwo"><i class="fas fa-tags" style="font-size: 22px;"></i>&nbsp&nbsp Ventas</a>
                        </div>
                        <div id="collapseTwo" class="collapse" data-parent="#accordion">
                            <div class="card-body">
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnVenPed" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Pedidos</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnVenFac" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Facturas</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnVenNC" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Notas de credito</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnVenCaj" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Operaciones de caja</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card">
                        <div class="card-header">
                            <a class="collapsed card-link btn-block" data-toggle="collapse" href="#collapseThree"><i class="fas fa-shopping-basket" style="font-size: 22px;"></i>&nbsp&nbsp Compras</a>
                        </div>
                        <div id="collapseThree" class="collapse" data-parent="#accordion">
                            <div class="card-body">
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnComPed" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Pedidos</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnComFac" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Facturas</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnComNC" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Notas de credito</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card">
                        <div class="card-header">
                            <a class="collapsed card-link btn-block" data-toggle="collapse" href="#collapsefour"><i class="fas fa-pallet" style="font-size: 22px;"></i>&nbsp&nbsp Inventarios</a>
                        </div>
                        <div id="collapsefour" class="collapse" data-parent="#accordion">
                            <div class="card-body">
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnInvEnt" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Entradas</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnInvSal" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Salidas</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnInvTra" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Traslados</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnInvCon" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Conteo</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card">
                        <div class="card-header">
                            <a class="collapsed card-link btn-block" data-toggle="collapse" href="#collapsefive"><i class="fas fa-piggy-bank" style="font-size: 22px;"></i>&nbsp&nbsp Bancos</a>
                        </div>
                        <div id="collapsefive" class="collapse" data-parent="#accordion">
                            <div class="card-body">
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnBanRec" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Pagos recibidos</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnBanEfe" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Pagos efectuados</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnBanDep" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Depositos</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card">
                        <div class="card-header">
                            <a class="collapsed card-link btn-block" data-toggle="collapse" href="#collapseSeven"><i class="fas fa-balance-scale-left" style="font-size: 22px;"></i>&nbsp&nbsp Finanzas</a>
                        </div>
                        <div id="collapseSeven" class="collapse" data-parent="#accordion">
                            <div class="card-body">
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnFinPer" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Periodos</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnFinPla" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Plan de cuentas</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnFinAsi" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Asientos</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnFinBal" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Balance general</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnFinEst" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Estado de resultado</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card">
                        <div class="card-header">
                            <a class="collapsed card-link btn-block" data-toggle="collapse" href="#collapsesix"><i class="fas fa-chart-pie" style="font-size: 22px;"></i>&nbsp&nbsp Reportes</a>
                        </div>
                        <div id="collapsesix" class="collapse" data-parent="#accordion">
                            <div class="card-body">
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnRepExi" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Existencias</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnRepKar" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Kardex</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnRepVen" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Ventas</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnRepCom" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Compras</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnRepCXC" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Cuentas por cobrar</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnRepCXP" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Cuentas por pagar</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card">
                        <div class="card-header">
                            <a class="collapsed card-link btn-block" data-toggle="collapse" href="#collapseOne"><i class="fas fa-cogs" style="font-size: 22px;"></i>&nbsp&nbsp Configuraciones</a>
                        </div>
                        <div id="collapseOne" class="collapse" data-parent="#accordion">
                            <div class="card-body">
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnUsuario" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Cambio de clave</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row table">
                                    <div class="col">
                                        <asp:LinkButton Enabled="false" ID="btnParametros" runat="server"><i class="fas fa-arrow-right"></i>&nbsp Parametros y maestros</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col">
                <div class="row">
                    <div class="col-6">
                        <div class="card text-white bg-info shadow-sm">
                            <div class="card-header">
                                <h5>Notas importantes</h5>
                            </div>
                            <div class="card-body">
                                <pre class="card-text text-white" runat="server" id="LblMensaje"></pre>
                            </div>
                        </div>
                    </div>
                    <div class="col-6">
                        <div class="card shadow-sm">
                            <div class="card-header">
                                <h5>Top Vendedores</h5>
                            </div>
                            <div class="card-body">
                                <asp:Chart runat="server" ID="GraTopVen">
                                    <Series>
                                        <asp:Series Name="Series1" ChartArea="ChartArea1" IsValueShownAsLabel="True" />
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1">
                                            <AxisX>
                                                <MajorGrid Enabled="False" />
                                            </AxisX>
                                        </asp:ChartArea>
                                    </ChartAreas>                          
                                </asp:Chart>                      
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="container-fluid" style="background-color: rgba(0, 0, 0,0.8); position: absolute; left: 0; top: 0; bottom: 0" id="PanelUsuario" runat="server" visible="false">
               <div style="padding-left: 38%; padding-right: 38%;padding-top:5%">
                   <div class="row rounded" style="background-color:white; padding: 5px 20px 15px 20px">
                       <div class="col" style="width:100%;text-align:center">
                           <div class="row" style="padding-bottom:2px">
                               <div class="col"><img src="imagenes/logo.jpg" width="150" alt="..."/></div>
                           </div>                           
                           <div class="row" style="padding-bottom:2px">
                               <div class="col input-group input-group-sm">
                                   <div class="input-group-prepend">
                                       <span class="input-group-text" style="width: 30px"><i class="fas fa-user"></i></span>
                                   </div>
                                   <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control form-control-sm" TextMode="SingleLine" placeholder="Usuario" />
                               </div>
                           </div>
                           <div class="row" style="padding-bottom:2px">
                               <div class="col input-group input-group-sm">
                                   <div class="input-group-prepend">
                                       <span class="input-group-text" style="width: 30px"><i class="fas fa-key"></i></span>
                                   </div>
                                   <asp:TextBox ID="txtClave" runat="server" CssClass="form-control form-control-sm" TextMode="Password" placeholder="Contraseña" />
                               </div>
                            </div>
                           <div class="row" style="padding-bottom:8px">
                               <div class="col input-group input-group-sm">
                                   <div class="input-group-prepend">
                                       <span class="input-group-text" style="width: 30px"><i class="fas fa-database"></i></span>
                                   </div>
                                   <asp:Label ID="lblBD" runat="server" CssClass="form-control form-control-sm text-left"></asp:Label>                                   
                               </div>
                           </div> 
                           <div class="row">
                               <div class="col">
                                   <asp:Button ID="btnAceptarUsu" runat="server" Text="  Aceptar  " CssClass="btn btn-primary btn-sm"/></div>
                           </div>                               
                       </div>                                        
                   </div>                         
               </div>               
           </div>

        <div class="container-fluid" style="background-color: rgba(0,0,0,0.7); position: absolute; left: 0; top: 0; bottom: 0" id="PanelCambioPass" runat="server" visible="false">
               <div style="padding-left: 40%; padding-right: 40%;padding-top:15%">
                   <div class="row rounded" style="background-color:white; padding: 15px 20px 15px 20px">
                       <div class="col" style="width:100%;text-align:center">
                           <div class="row" style="padding-bottom:3px">
                               <div class="col input-group input-group-sm">
                                   <div class="input-group-prepend">
                                       <span class="input-group-text" style="width: 30px"><i class="fas fa-user"></i></span>
                                   </div>
                                   <asp:TextBox ID="txtCamUsuario" runat="server" CssClass="form-control form-control-sm" TextMode="SingleLine" placeholder="Usuario" />
                               </div>
                           </div>
                           <div class="row" style="padding-bottom:4px">
                               <div class="col input-group input-group-sm">
                                   <div class="input-group-prepend">
                                       <span class="input-group-text" style="width: 30px"><i class="fas fa-key"></i></span>
                                   </div>
                                   <asp:TextBox ID="txtCamPass" runat="server" CssClass="form-control form-control-sm" TextMode="Password" placeholder="Clave actual" />
                               </div>
                            </div>
                           <div class="row" style="padding-bottom:4px">
                               <div class="col input-group input-group-sm">
                                   <div class="input-group-prepend">
                                       <span class="input-group-text" style="width: 30px"><i class="fas fa-file"></i></span>
                                   </div>
                                   <asp:TextBox ID="txtCamPass1" runat="server" CssClass="form-control form-control-sm" TextMode="Password" placeholder="Clave nueva" />
                               </div>
                            </div>
                           <div class="row" style="padding-bottom:4px">
                               <div class="col input-group input-group-sm">
                                   <div class="input-group-prepend">
                                       <span class="input-group-text" style="width: 30px"><i class="fas fa-file"></i></span>
                                   </div>
                                   <asp:TextBox ID="txtCamPass2" runat="server" CssClass="form-control form-control-sm" TextMode="Password" placeholder="Confirmar nueva" />
                               </div>
                            </div>
                           <div class="row">
                               <div class="col">
                                   <asp:Button ID="btnAceptarPass" runat="server" Text="Aceptar" CssClass="btn btn-primary btn-sm"/>&nbsp&nbsp<input id="btnCancelarPass" runat="server" type="button" value="Cancelar" class="btn btn-secondary btn-sm"/></div>
                           </div>                           
                       </div>                                        
                   </div>                         
               </div>               
           </div>

        <div class="container-fluid" style="background-color: rgba(0,0,0,0.7); position: absolute; left: 0; top: 0; bottom: 0" id="PanelVersion" runat="server" visible="false">
               <div style="padding-left: 40%; padding-right: 40%;padding-top:15%">
                   <div class="row rounded" style="background-color:white; padding: 15px 20px 15px 20px">
                       <div class="col" style="width:100%;text-align:center">
                           <div class="row" style="padding-bottom:3px">
                             <p><b>Creado por SERP <br /> San Pedro Sula, Honduras <br />Agosto 2020<br /> Version 1.0.1</b></p>
                           </div>                    
                           <div class="row">
                               <div class="col"><asp:Button ID="btnAceptarVer" runat="server" Text="Aceptar" CssClass="btn btn-primary btn-sm"/></div>
                           </div>                           
                       </div>                                        
                   </div>                         
               </div>               
           </div>               

        <div style="height:20px; font-size:x-small; position:fixed; bottom:50px; right: 0px;"><div style="padding-top:10px;padding-right:10px;"><a target="_blank" href="http://www.freepik.com">Designed by Freepik</a></div></div>
        <div style="width:100%; height:40px; font-size:smaller; position:fixed; bottom:0px; background-color:#202020; left: 0px; color: white;"><div style="padding-top:10px;padding-left:10px;">Soporte &nbsp&nbsp<i class="far fa-envelope"></i>&nbsp<a id="Mailito" href="mailto:germandavid86@gmail.com">germandavid86@gmail.com</a>&nbsp&nbsp&nbsp<i class="fab fa-whatsapp"></i><a target="_blank" href="https://api.whatsapp.com/send?phone=50496516994&text=hola,%20tendo%20una%20consulta?">+504 9651-6994</a></div></div>
        <script src="js/JQuery.js"></script>
        <script src="js/popper.min.js"></script>
        <script src="js/bootstrap.min.js"></script>
        <script type="text/javascript">
            function redireccionar() {
                if (screen.width < 640)
                    window.location.href = "iniciomovil.aspx";
            }
        </script>
    </form>
</body>
</html>
