using Microsoft.Xrm.Sdk;
using System;

namespace ElVinoDorado.Plugins
{
    public class ProductPostUpdate : IPlugin
    {
        string token = "";
        public ProductPostUpdate(string config)
        {
            token = config;
        }

        /// <summary>
        /// En caso de que se actualice la entidad, para este caso el producto, se vuelve a consultar el servicio para 
        /// asegurar que la puntuación corresponda al vino, ejemplo si se cambia el código del vino.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Evitar que se ejecute por segunda o más veces, debido a que este método se ejecuta 
            // después de una actualización en crm y en este código se le está indicando que vuelva 
            // a actualizar se controla la profundidad de actualizaciones para que no se ejecute más de una vez
            if (context.Depth > 2)
                return;

            if (!context.PreEntityImages.Contains("PreImage") || !(context.PreEntityImages["PreImage"] is Entity))
                return;
            var entityPre = (Entity)context.PreEntityImages["PreImage"];

            if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is Entity))
                return;
            var entity = (Entity)context.InputParameters["Target"];

            if (entity["productnumber"] != entityPre["productnumber"])
            {
                // "productnumber" es el nombre físico del campo a trabajar en el crm
                var codigo = entity.GetAttributeValue<string>("productnumber");
                var name = entity.GetAttributeValue<string>("name");
                // consultar la puntuación del vino mediante servicio web
                var puntuacion = Puntuacion.Obtener(token, codigo);
                string[] valores = puntuacion.Split(';');
                var wine = valores[1];
                var score = Convert.ToDouble(valores[0]);
                // return new KeyValuePair<string, double?>(items.results[0].wine, score);

                // var wine = Puntuacion.Obtener(token, codigo).;
                // Asignar el valor obtenido al campo correpondiente en el CRM
                /*
                entity["evd_puntuacion"] = puntuacion.Value;
                entity["name"] = puntuacion.Value;
                */
                entity["evd_puntuacion"] = score;
                entity["name"] = wine;

                var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                var service = serviceFactory.CreateOrganizationService(context.UserId);
                service.Update(entity);
            }                           
        }
    }
}
