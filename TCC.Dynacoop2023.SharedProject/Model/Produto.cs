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
using TCC.Dynacoop2023.Environment1.ConctionFactory;

namespace TCC.Dynacoop2023.SharedProject.Model
{
    public class Produto
    {
        public IOrganizationService ServiceClient { get; set; }

        public string Logicalname { get; set; }

        public Produto(IOrganizationService crmServiceClient)
        {
            this.ServiceClient = crmServiceClient;
            this.Logicalname = "product";
        }

        public Produto(CrmServiceClient crmServiceClient)
        {
            this.ServiceClient = crmServiceClient;
            this.Logicalname = "product";
        }

        public void CriaProdAmb2(Entity target)
        {
            IOrganizationService serviceAmbienteDois = ConectionFactory.organizationService();
            Entity produto = GrupoUnid(serviceAmbienteDois, target);
            serviceAmbienteDois.Create(produto);
        }
        private Entity GrupoUnid(IOrganizationService serviceAmbDois, Entity product)
        {
            Entity productReturn = product;
            if (productReturn.Contains("defaultuomscheduleid") && productReturn.Contains("defaultuomid"))
            {
                Guid idGrupoUnidade = productReturn.GetAttributeValue<EntityReference>("defaultuomscheduleid").Id;
                Guid idUnidade = productReturn.GetAttributeValue<EntityReference>("defaultuomid").Id;
                EntityCollection grupoUnid = QueryProductUnidadePadrao(this.ServiceClient, idGrupoUnidade);
                EntityCollection unidades = QueryProductUnidadePrincipal(this.ServiceClient, idUnidade);
                Verifica(serviceAmbDois, productReturn, grupoUnid, unidades);
            }
            return CopiaAssunto(serviceAmbDois, productReturn);
        }

        private void Verifica(IOrganizationService serviceAmbDois, Entity productReturn, EntityCollection grupoUnid, EntityCollection unidades)
        {
            if (grupoUnid.Entities.Count > 0 && unidades.Entities.Count > 0)
            {
                Entity entidadeGrupo = grupoUnid.Entities.FirstOrDefault();
                Entity entidadeUnidade = unidades.Entities.FirstOrDefault();

                string nomeGrupoUnid = entidadeGrupo.GetAttributeValue<String>("name");

                EntityCollection gruposPorNome = ChamaQuery(serviceAmbDois, "uomschedule", nomeGrupoUnid);
                if (gruposPorNome.Entities.Count > 0)
                {
                    Guid idGrupoPorNome = gruposPorNome.Entities.FirstOrDefault().Id;
                    productReturn["defaultuomscheduleid"] = new EntityReference("uomschedule", idGrupoPorNome);
                }
                else
                {
                    Guid idNovoGrupo = serviceAmbDois.Create(entidadeGrupo);
                    productReturn["defaultuomscheduleid"] = new EntityReference("uomschedule", idNovoGrupo);
                }

                string nomeUnid = entidadeUnidade.GetAttributeValue<String>("name");
                EntityCollection unidPorNome = ChamaQuery(serviceAmbDois, "uom", nomeUnid);

                if (unidPorNome.Entities.Count > 0)
                {
                    Guid idUnidPorNome = unidPorNome.Entities.FirstOrDefault().Id;
                    productReturn["defaultuomid"] = new EntityReference("uom", idUnidPorNome);
                }
                else
                {
                    Guid idNovaUnidade = serviceAmbDois.Create(entidadeUnidade);
                    productReturn["defaultuomid"] = new EntityReference("uom", idNovaUnidade);
                }
            }
            else
            {
                EntityCollection grupoPadrao = ChamaQuery(serviceAmbDois, "uomschedule", "Unidade Padrão");
                productReturn["defaultuomscheduleid"] = new EntityReference("uomschedule", grupoPadrao.Entities.FirstOrDefault().Id);

                EntityCollection unidadePadrao = ChamaQuery(serviceAmbDois, "uom", "Unidade Principal");
                productReturn["defaultuomid"] = new EntityReference("uom", unidadePadrao.Entities.FirstOrDefault().Id);
            }
        }

        private static EntityCollection QueryProductUnidadePrincipal(IOrganizationService serviceAmbUm, Guid idUnidade)
        {
            QueryExpression queryUnidade = new QueryExpression("uom");
            queryUnidade.ColumnSet.AddColumns("uomscheduleid", "quantity", "name");
            queryUnidade.Criteria.AddCondition("name", ConditionOperator.NotEqual, "Unidade Principal");
            queryUnidade.Criteria.AddCondition("uomid", ConditionOperator.Equal, idUnidade);
            EntityCollection unidades = serviceAmbUm.RetrieveMultiple(queryUnidade);
            return unidades;
        }

        private static EntityCollection QueryProductUnidadePadrao(IOrganizationService serviceAmbUm, Guid idGrupoUnidade)
        {
            QueryExpression queryGrupoUnidade = new QueryExpression("uomschedule");
            queryGrupoUnidade.ColumnSet.AddColumns("name", "baseuomname");
            queryGrupoUnidade.Criteria.AddCondition("name", ConditionOperator.NotEqual, "Unidade Padrão");
            queryGrupoUnidade.Criteria.AddCondition("uomscheduleid", ConditionOperator.Equal, idGrupoUnidade);
            EntityCollection grupoUnid = serviceAmbUm.RetrieveMultiple(queryGrupoUnidade);
            return grupoUnid;
        }

        private Entity CopiaAssunto(IOrganizationService service, Entity product)
        {
            Entity productReturn = product;
            if (productReturn.Contains("subjectid"))
            {
                Guid idAssunto = productReturn.GetAttributeValue<EntityReference>("subjectid").Id;

                QueryExpression queryAssunto = new QueryExpression("subject");
                queryAssunto.ColumnSet.AddColumns("title", "description");
                queryAssunto.Criteria.AddCondition("title", ConditionOperator.Equal, "Assunto Padrão");
                queryAssunto.Criteria.AddCondition("subjectid", ConditionOperator.Equal, idAssunto);
                EntityCollection assuntos = service.RetrieveMultiple(queryAssunto);

                if (assuntos.Entities.Count > 0)
                {
                    Guid idNovoAssunto = service.Create(assuntos.Entities.FirstOrDefault());
                    productReturn["subjectid"] = new EntityReference("subject", idNovoAssunto);
                }
                else
                {
                    EntityCollection assuntoPadrao = ChamaQuery(service, "subject", "Assunto Padrão", "title");
                    productReturn["subjectid"] = new EntityReference("subject", assuntoPadrao.Entities.FirstOrDefault().Id);
                }
            }
            return productReturn;
        }
        private EntityCollection ChamaQuery(IOrganizationService service, string entidade, string comparacao, string coluna = "name")
        {
            QueryExpression query = new QueryExpression(entidade);
            query.Criteria.AddCondition(coluna, ConditionOperator.Equal, comparacao);
            return service.RetrieveMultiple(query);
        }
    }
}

