using System;
using System.Threading.Tasks;
using System.Net.Http;
using TCC.Dynacoop2023.MyPlugins.TccISV;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using RestSharp;
using System.Runtime.ConstrainedExecution;
using TCC.Dynacoop2023.SharedProject.Model;

namespace TCC.Dynacoop2023.MyPlugins.Actions
{
    public class BuscaCep : ActionCore
    {
        [Input("Cep")]
        public InArgument<string> Cep { get; set; }

        [Output("DadosCep")]
        public OutArgument<string> DadosCep { get; set; }
        public string Log { get; set; }

        public override void ExecuteAction(CodeActivityContext context)
        {
            try
            {
                this.Log += "Entrou no processo";

                RestResponse response = GetDadosCEPOnAPI(this.Cep.ToString());

            }
            catch (Exception ex)
            {
                throw new Exception(this.Log + " - " + ex.ToString());
            }

        }

        private RestResponse GetDadosCEPOnAPI(string cep)
        {
            this.Log += "GetProductsOnAPI";

            var options = new RestClientOptions($"https://viacep.com.br/ws/{cep}/json/")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/account", Method.Post);
            RestResponse response = client.Execute(request);
            return response;
        }

    }
}
