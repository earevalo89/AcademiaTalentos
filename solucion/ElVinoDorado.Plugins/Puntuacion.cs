using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ElVinoDorado.Plugins
{
    class Puntuacion
    {
        public static T Deserialize<T>(string jsonString)
        {
            // Método generico para transformar string en clases
            using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(ms);
            }
        }

        //public static KeyValuePair<string, double?>? Obtener(string token, string codigo)
        public static string Obtener(string token, string codigo)
        {
            // Para obtener la url con la que se trabajara haga clic en: https://pastebin.com/ZZdiLML1

            string url = $"https://private-anon-6bff2d4852-globalwinescore.apiary-proxy.com/globalwinescores/latest/?wine_id={codigo}&limit=1";


            // Dentro de using se asegura la ejecución de un proceso en ese espacio de memoria.
            // WebClient es similar a un navegador web
            using (var client = new WebClient())
            {
                client.Headers.Add("Authorization", $"Token {token}");

                var responseBytes = client.DownloadData(url);

                // Transforma la variable en string
                string response = Encoding.UTF8.GetString(responseBytes);

                var items = Deserialize<Vinos>(response);


                // return ((items != null && items.results.Count > 0) ? items.results[0].score : null);  
                if (items != null && items.results.Count > 0)
                {
                    double? score = items.results[0].score;
                    string name = items.results[0].wine;
                    string retorno = score.ToString() + ";" + name;

                    return retorno;
                    //return new KeyValuePair<string, double?>(items.results[0].wine, score);
                }
                else
                {
                    return null;
                }                              
            }
        }
    }
}
