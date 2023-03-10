using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TCC.Dynacoop2023.SharedProject.VO
{
    public class ContaVO
    {
        [JsonProperty("logradouro")]
        public string Logradouro { get; set; }

        [JsonProperty("localidade")]
        public string Localidade { get; set; }

        [JsonProperty("complemento")]
        public string Complemento { get; set; }

        [JsonProperty("uf")]
        public string Uf { get; set; }

        [JsonProperty("bairro")]
        public string Bairro { get; set; }

        [JsonProperty("ibge")]
        public string Ibge { get; set; }

        [JsonProperty("ddd")]
        public string Ddd { get; set; }
    }
}
