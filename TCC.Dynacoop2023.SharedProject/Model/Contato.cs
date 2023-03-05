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
    public class Contato
    {
        public IOrganizationService ServiceClient { get; set; }

        public string Logicalname { get; set; }

        public Contato(IOrganizationService crmServiceClient)
        {
            this.ServiceClient = crmServiceClient;
            this.Logicalname = "contact";
        }

        public Contato(CrmServiceClient crmServiceClient)
        {
            this.ServiceClient = crmServiceClient;
            this.Logicalname = "contact";
        }

        public Entity GetContactByCpf(string cpf)
        {
            QueryExpression queryContact = new QueryExpression(this.Logicalname);
            queryContact.Criteria.AddCondition("dyn1_cpf", ConditionOperator.Equal, cpf);
            return RetrieveOneContact(queryContact);
        }

        private Entity RetrieveOneContact(QueryExpression queryAccount)
        {
            EntityCollection accounts = this.ServiceClient.RetrieveMultiple(queryAccount);

            if (accounts.Entities.Count() > 0)
                return accounts.Entities.FirstOrDefault();
            else
                Console.WriteLine("Nenhum contato encontrada com esse cpf");

            return null;
        }
    }
}