using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Dynacoop2023.Environment1.ConctionFactory
{
    public class ConectionFactory 
    {
        public static IOrganizationService organizationService()
        {
            string url = "org1cd15510"; 
            string clientId = "535f830d-bdb4-4733-a1a9-a621942725e6";
            string clientSecret = "ibF8Q~Is.TByPGf4g2jpUjdhj.BF-ZmpqG89fdy-";

            CrmServiceClient serviceClient = new CrmServiceClient($"AuthType=ClientSecret;Url=https://{url}.crm2.dynamics.com/;AppId={clientId};ClientSecret={clientSecret};");

            if (!serviceClient.CurrentAccessToken.Equals(null))
                Console.WriteLine("Conexão Realizada com Sucesso");
            else
                Console.WriteLine("Erro na conexão");

            return serviceClient;

        }
    }
}
