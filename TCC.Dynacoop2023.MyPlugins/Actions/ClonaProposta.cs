using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using TCC.Dynacoop2023.MyPlugins.TccISV;
using System.Activities;
using TCC.Dynacoop2023.SharedProject.Model;
using TCC.Dynacoop2023.SharedProject.Controller;

namespace TCC.Dynacoop2023.MyPlugins.Actions
{
    public class ClonaProposta : ActionCore
    {
        public override void ExecuteAction(CodeActivityContext context)
        {
            /*// Obter a oportunidade original a ser clonada
            Guid opportunityId = new Guid("12345678-9abc-def0-1234-567890abcdef");
            Entity opportunity = this.Service.Retrieve("opportunity", opportunityId, new ColumnSet(true));

            // Criar uma nova oportunidade e copiar todos os campos da oportunidade original
            Entity clonedOpportunity = new Entity("opportunity");
            foreach (var attribute in opportunity.Attributes)
            {
                clonedOpportunity[attribute.Key] = attribute.Value;
            }

            // Definir o nome da nova oportunidade
            string clonedOpportunityName = "Nova oportunidade";
            clonedOpportunity["name"] = clonedOpportunityName;

            // Chamar a Action para clonar a oportunidade
            ClonarOportunidadeRequest request = new ClonarOportunidadeRequest();
            request.OriginalOpportunityId = opportunityId;
            request.ClonedOpportunity = clonedOpportunity;
            service.Execute(request);*/

        }
    }
}
