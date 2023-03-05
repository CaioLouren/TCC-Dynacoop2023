using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Deployment;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TCC.Dynacoop2023.SharedProject.Model
{
    public class Oportunidade
    {
        public IOrganizationService ServiceClient { get; set; }

        public string Logicalname { get; set; }

        public Oportunidade(IOrganizationService crmServiceClient)
        {
            this.ServiceClient = crmServiceClient;
            this.Logicalname = "opportunity";
        }

        public Oportunidade(CrmServiceClient crmServiceClient)
        {
            this.ServiceClient = crmServiceClient;
            this.Logicalname = "opportunity";
        }

        public Entity GetOpportunityByIdUnico(string idUnico)
        {
            QueryExpression queryOpportunity = new QueryExpression(this.Logicalname);
            queryOpportunity.Criteria.AddCondition("dyn1_idunico", ConditionOperator.Equal, idUnico);
            return RetrieveOneOpportunity(queryOpportunity);
        }

        private Entity RetrieveOneOpportunity(QueryExpression queryAccount)
        {
            EntityCollection accounts = this.ServiceClient.RetrieveMultiple(queryAccount);

            if (accounts.Entities.Count() > 0)
                return accounts.Entities.FirstOrDefault();
            else
                Console.WriteLine("Nenhuma oportunidade encontrada com esse Id único");

            return null;
        }

        public string GeraIdUnico()
        {
            string idUnico = $"OPP-{NumeroAleatorio(5)}-{LetraAleatorio(1)}{NumeroAleatorio(1)}{LetraAleatorio(1)}{NumeroAleatorio(1)} ";
            return idUnico;
        }

        public string NumeroAleatorio(int tamanho)
        {
            string numero = "0123456789";
            var random = new Random();
            var resultado = new string(Enumerable.Repeat(numero, tamanho).Select(s => s[random.Next(s.Length)]).ToArray());
            return resultado;
        }

        public string LetraAleatorio(int tamanho)
        {
            string numero = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            var resultado = new string(Enumerable.Repeat(numero, tamanho).Select(s => s[random.Next(s.Length)]).ToArray());
            return resultado;
        }
    }
}