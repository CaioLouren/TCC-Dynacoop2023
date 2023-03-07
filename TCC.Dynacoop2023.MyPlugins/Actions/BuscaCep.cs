using TCC.Dynacoop2023.MyPlugins.TccISV;
using TCC.Dynacoop2023.SharedProject.VO;
using Microsoft.Xrm.Sdk.Workflow;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Dynacoop2023.SharedProject.Extensions;

namespace TCC.Dynacoop2023.MyPlugins.Actions
{
    public class BuscaCep : ActionCore
    {
        [Input("dyn1_cep")]
        public InArgument<string> ProductId { get; set; }


        [Output("dyn1_logradouro")]
        public OutArgument<string> Logradouro { get; set; }
        
        [Output("dyn1_complemento")]
        public OutArgument<string> Complemento { get; set; }
        
        [Output("dyn1_uf")]
        public OutArgument<string> Uf { get; set; }

        [Output("dyn1_bairro")]
        public OutArgument<string> Bairro { get; set; }
        
        [Output("dyn1_codigoibge")]
        public OutArgument<string> CodigoIbge { get; set; }

        [Output("dyn1_ddd")]
        public OutArgument<string> Ddd { get; set; }
        public string Log { get; set; }

        public override void ExecuteAction(CodeActivityContext context)
        {
            try
            {
                this.Log += "Entrou no processo";

                RestResponse response = GetProductsOnAPI();
                ContaVO contaFound = GetProductWithID(context, response);

                this.Log += "Setando valor";

                ProductName.Set(context, contaFound.);
            }
            catch (Exception ex)
            {
                throw new Exception(this.Log + " - " + ex.ToString());
            }
        }

        private ProductVO GetProductWithID(CodeActivityContext context, RestResponse response)
        {
            this.Log += "GetProductWithID";
            this.Log += response.Content;

            List<ProductVO> productsVO = JsonConvert.DeserializeObject<List<ProductVO>>(response.Content);

            //List<ProductVO> productsVO = new List<ProductVO>();
            //productsVO.Add(new ProductVO()
            //{
            //    ProductId = "PROD-0001",
            //    ProductName = "Box"
            //});

            this.Log += "Converteu JSON";

            try
            {
                var productFound = (from p in productsVO
                                    where p.ProductId == ProductId.Get(context)
                                    select p).ToList().FirstOrDefault();

                if (productFound == null)
                {
                    throw new Exception("Produto com esse ID não encontrado");
                }

                return productFound;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel ler o Parametro Product Id");
            }
        }

        private RestResponse GetProductsOnAPI()
        {
            this.Log += "GetCepOnAPI";

            var options = new RestClientOptions($"viacep.com.br/ws/01001000/json/")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/product", Method.Post);
            RestResponse response = client.Execute(request);
            return response;
        }
    }
}
