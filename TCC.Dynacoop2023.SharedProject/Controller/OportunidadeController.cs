using Microsoft.Crm.Sdk.Messages;
using Microsoft.Rest;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Dynacoop2023.SharedProject.Model;

namespace TCC.Dynacoop2023.SharedProject.Controller
{
    public class OportunidadeController
    {
        public IOrganizationService ServiceClient { get; set; }
        public Oportunidade Oportunidade { get; set; }

        public OportunidadeController(IOrganizationService crmServiceCliente)
        {
            ServiceClient = crmServiceCliente;
            this.Oportunidade = new Oportunidade(ServiceClient);
        }

        public OportunidadeController(CrmServiceClient crmServiceCliente)
        {
            ServiceClient = crmServiceCliente;
            this.Oportunidade = new Oportunidade(ServiceClient);
        }

        public Entity GetOpportunityByIdUnico(string idUnico)
        {
            return Oportunidade.GetOpportunityByIdUnico(idUnico);
        }

        public string GeraIdUnico()
        {
            return Oportunidade.GeraIdUnico();
        }
    }
}
