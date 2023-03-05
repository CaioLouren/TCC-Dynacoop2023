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
    public class Conta
    {
        public IOrganizationService ServiceClient { get; set; }

        public string Logicalname { get; set; }

        public Conta(IOrganizationService crmServiceClient)
        {
            this.ServiceClient = crmServiceClient;
            this.Logicalname = "account";
        }

        public Conta(CrmServiceClient crmServiceClient)
        {
            this.ServiceClient = crmServiceClient;
            this.Logicalname = "account";
        }

        public Entity GetAccountByCnpj(string cnpj)
        {
            QueryExpression queryAccount = new QueryExpression(this.Logicalname);
            queryAccount.Criteria.AddCondition("dyn1_cnpj", ConditionOperator.Equal, cnpj);
            return RetrieveOneAccount(queryAccount);
        }

        private Entity RetrieveOneAccount(QueryExpression queryAccount)
        {
            EntityCollection accounts = this.ServiceClient.RetrieveMultiple(queryAccount);

            if (accounts.Entities.Count() > 0)
                return accounts.Entities.FirstOrDefault();
            else
                Console.WriteLine("Nenhuma conta encontrada com esse cpf");

            return null;
        }
    }
}