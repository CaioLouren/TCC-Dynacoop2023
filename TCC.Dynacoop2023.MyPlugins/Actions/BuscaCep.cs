using System;
using System.Threading.Tasks;
using System.Net.Http;
using TCC.Dynacoop2023.MyPlugins.TccISV;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using RestSharp;
using System.Runtime.ConstrainedExecution;
using TCC.Dynacoop2023.SharedProject.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using TCC.Dynacoop2023.SharedProject.VO;
using System.Linq;

namespace TCC.Dynacoop2023.MyPlugins.Actions
{
    public class BuscaCep : ActionCore
    {
        [Input("Cep")]
        public InArgument<string> Cep { get; set; }

        [Output("Logradouro")]
        public OutArgument<string> Logradouro { get; set; }

        [Output("Localidade")]
        public OutArgument<string> Localidade { get; set; }

        [Output("Complemento")]
        public OutArgument<string> Complemento { get; set; }

        [Output("Uf")]
        public OutArgument<string> Uf { get; set; }

        [Output("Bairro")]
        public OutArgument<string> Bairro { get; set; }

        [Output("Ibge")]
        public OutArgument<string> Ibge { get; set; }

        [Output("Ddd")]
        public OutArgument<string> Ddd { get; set; }
        public string Log { get; set; }

        public override void ExecuteAction(CodeActivityContext context)
        {
            try
            {
                this.Log += "Entrou no processo";

                RestResponse response = GetDadosCEPOnAPI(context);
                ContaVO contaFound = GetAccountWithID(context, response);


                this.Log += "Setando valor";

                Logradouro.Set(context, contaFound.Logradouro);
                Localidade.Set(context, contaFound.Localidade);
                Complemento.Set(context, contaFound.Complemento);
                Uf.Set(context, contaFound.Uf);
                Bairro.Set(context, contaFound.Bairro);
                Ibge.Set(context, contaFound.Ibge);
                Ddd.Set(context, contaFound.Ddd);

            }
            catch (Exception ex)
            {
                throw new Exception(this.Log + " - " + ex.ToString());
            }

        }

        private ContaVO GetAccountWithID(CodeActivityContext context, RestResponse response)
        {
            this.Log += "GetAccountWithID";

            this.Log += response.Content;
            List<ContaVO> contaVO = JsonConvert.DeserializeObject<List<ContaVO>>(response.Content);

            try
            {
                var contaFound = (from c in contaVO 
                                  where c.Cep == Cep.Get(context)
                                  select c).ToList().FirstOrDefault();

                if (contaFound == null)
                {
                    throw new Exception("Conta com esse CEP não encontrado");
                }

                return contaFound;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel ler o Parametro do Cep da Conta");
            }
        }


        private RestResponse GetDadosCEPOnAPI(CodeActivityContext context)
        {

            this.Log += " GetDadosCEPOnAPI ";

            var options = new RestClientOptions("https://viacep.com.br")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/ws/{Cep.Get(context)}/json/", Method.Get);
            RestResponse response = client.Execute(request);
            return response;
        }

    }
}
