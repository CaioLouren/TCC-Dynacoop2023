using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Dynacoop2023.ConsoleApplication

{
    public class Singleton
    {
        public static CrmServiceClient GetService()
        {
            string url = "org5b78e0b4";
            string clientId = "a04156f7-0acc-4223-a6eb-79f304ca8072";
            string clientSecret = "Mag8Q~EWsa-BeXmYZ8Cng4m7hRLm9dIbKWUhkaAz";

            CrmServiceClient serviceClient = new CrmServiceClient($"AuthType=ClientSecret;Url=https://{url}.crm2.dynamics.com/;AppId={clientId};ClientSecret={clientSecret};");

            if (!serviceClient.CurrentAccessToken.Equals(null))
                Console.WriteLine("Conexão Realizada com Sucesso");
            else
                Console.WriteLine("Erro na conexão");

            return serviceClient;
        }
    }
}
