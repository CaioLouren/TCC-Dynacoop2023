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
    public class ContatoController
    {
        public IOrganizationService ServiceClient { get; set; }
        public Contato Contato { get; set; }

        public ContatoController(IOrganizationService crmServiceCliente)
        {
            ServiceClient = crmServiceCliente;
            this.Contato = new Contato(ServiceClient);
        }

        public ContatoController(CrmServiceClient crmServiceCliente)
        {
            ServiceClient = crmServiceCliente;
            this.Contato = new Contato(ServiceClient);
        }
        public Entity GetContactByCpf(string cpf)
        {
            return Contato.GetContactByCpf(cpf);
        }
    }
}
