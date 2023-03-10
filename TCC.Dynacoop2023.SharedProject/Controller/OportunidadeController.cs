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

        public string[] getIgnoreFields()
        {
            return Oportunidade.getIgnoreFields();
        }
        public EntityReference ValidaLookup(KeyValuePair<string, object> value, IOrganizationService service, IOrganizationService service2)
        {
            return Oportunidade.ValidaLookup(value, service, service2);
        }
        public void AtribuiOpp(Entity oportunidade, Entity oppAmbiente2, IOrganizationService organizationService, OportunidadeController oportunidadeController)
        {
            Oportunidade.AtribuiOpp(oportunidade, oppAmbiente2, organizationService, oportunidadeController);
        }
        public Entity GetOpportunityById(Guid id)
        {
            return Oportunidade.GetOpportunityById(id);
        }

    }
}
