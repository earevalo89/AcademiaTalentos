using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;

namespace ElVinoDorado.Plugins
{
    public class PuntuacionVinoActivity : CodeActivity
    {
        [Input("Token")]
        public InArgument<string> Token { get; set; }
        [Input("Product")]
        [ReferenceTarget("product")]
        public InArgument<EntityReference> Product { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var contextWorkflow = context.GetExtension<IWorkflowContext>();
            var serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            var user = contextWorkflow.InitiatingUserId;

            var organizationService = serviceFactory.CreateOrganizationService(user);

            var product = Product.Get<EntityReference>(context);
            var token = Token.Get<string>(context);
            if (product == null || token == null) return;

            // Es equivalente a un select
            var entity = organizationService.Retrieve("product", product.Id, new ColumnSet(true));
            var codigo = entity.GetAttributeValue<string>("productnumber");
            var puntuacion = Puntuacion.Obtener(token, codigo);
            string[] valores = puntuacion.Split(';');
            var wine = valores[1];
            var score = Convert.ToDouble(valores[0]);
            entity["evd_puntuacion"] = score;
            entity["name"] = wine;
            organizationService.Update(entity);
            
        }
    }
}
