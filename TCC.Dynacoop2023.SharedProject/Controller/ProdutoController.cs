using TCC.Dynacoop2023.SharedProject.Model;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Rest;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Dynacoop2023.SharedProject.Controller
{
    public class ProdutoController
    {
        public IOrganizationService ServiceClient { get; set; }
        public Produto Produto  { get; set; }

        public ProdutoController(IOrganizationService crmServiceCliente)
        {
            ServiceClient = crmServiceCliente;
            this.Produto = new Produto(ServiceClient);
        }

        public ProdutoController(CrmServiceClient crmServiceCliente)
        {
            ServiceClient = crmServiceCliente;
            this.Produto = new Produto(ServiceClient);
        }
    }
}
