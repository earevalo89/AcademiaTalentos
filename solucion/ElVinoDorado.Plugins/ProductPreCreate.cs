using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace ElVinoDorado.Plugins
{
    public class ProductPreCreate : IPlugin
    {
        string token = "";
        public ProductPreCreate(string config)
        {
            token = config;
        }

        /// <summary>
        /// Cuando se está creando el producto y se hace clic en guardar, se ejecuta una consulta
        /// al servicio web donde consulta la puntuación del vino y actualiza el campo en el CRM.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public void Execute(IServiceProvider serviceProvider)
        {
            // Se declara el contexto que hace referencia al plugin utilizado
            var context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (!context.InputParameters.Contains("Target") ||
                !(context.InputParameters["Target"] is Entity))
                return;

            var entity = (Entity)context.InputParameters["Target"];
            if (!entity.Attributes.Contains("productnumber"))
                return;

            // "productnumber" es el nombre físico del campo a trabajar en el crm
            var codigo = entity.GetAttributeValue<string>("productnumber");

            // consultar la puntuación del vino
            var puntuacion = Puntuacion.Obtener(token, codigo);
            string[] valores = puntuacion.Split(';');
            var wine = valores[1];
            var score = Convert.ToDouble(valores[0]);

            entity["evd_puntuacion"] = score;
            entity["name"] = wine;
        }
    }
}
