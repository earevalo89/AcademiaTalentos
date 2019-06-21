using Microsoft.AspNetCore.Mvc;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;

namespace ElVinoDorado.Website.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Clase para conexión 
            var uri = new Uri("https://mohmal0.api.crm4.dynamics.com/XRMServices/2011/Organization.svc");
            var clientCredentials = new ClientCredentials();
            clientCredentials.UserName.UserName = "pruebascursom@mohmal.in";
            clientCredentials.UserName.Password = "Ytrewq12";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var organizationService = new OrganizationServiceProxy(uri, null, clientCredentials, null);

            // Código para probar conexión
            // var request = new WhoAmIRequest();
            // var response = organizationService.Execute(request);

            //Busqueda de vinos
            var query = new QueryExpression("product"); // entidad producto
            query.ColumnSet = new ColumnSet(true); // Traer todas las columnas
            query.AddOrder("name", OrderType.Ascending); // Ordenación
            query.Criteria.AddCondition("evd_puntuacion", ConditionOperator.NotEqual, Convert.ToDouble(0));
            query.Criteria.AddCondition("evd_puntuacion", ConditionOperator.NotNull);
            var productos = organizationService.RetrieveMultiple(query).Entities;

            var vinos = productos.Select(vino => new Models.Vino
            {
                Codigo = vino.GetAttributeValue<string>("productnumber"),
                Nombre = vino.GetAttributeValue<string>("name"),
                Puntuacion = vino.GetAttributeValue<double>("evd_puntuacion")
            }).ToList();           

            return View(vinos);
        }

        [Route("/clientepotencial")]
        [HttpPost]
        public IActionResult ClientePotencial(string nombre, string email)
        {
            // Clase para conexión 
            var uri = new Uri("https://mohmal0.api.crm4.dynamics.com/XRMServices/2011/Organization.svc");
            var clientCredentials = new ClientCredentials();
            clientCredentials.UserName.UserName = "pruebascursom@mohmal.in";
            clientCredentials.UserName.Password = "Ytrewq12";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var organizationService = new OrganizationServiceProxy(uri, null, clientCredentials, null);

            var cliente = new Entity("lead");
            cliente["firstname"] = nombre;
            cliente["emailaddress1"] = email;
            organizationService.Create(cliente);                      

            return Redirect("/");
        }
   }
}
