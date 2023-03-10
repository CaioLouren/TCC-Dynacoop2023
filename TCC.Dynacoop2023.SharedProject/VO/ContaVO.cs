using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TCC.Dynacoop2023.SharedProject.VO
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ContaVO
    {
        [JsonProperty(PropertyName = "cep")]
        public string Cep { get; set; }

        [JsonProperty(PropertyName = "logradouro")]
        public string Logradouro { get; set; }

        [JsonProperty(PropertyName = "localidade")]
        public string Localidade { get; set; }

        [JsonProperty(PropertyName ="complemento")]
        public string Complemento { get; set; }

        [JsonProperty(PropertyName = "uf")]
        public string Uf { get; set; }

        [JsonProperty(PropertyName = "bairro")]
        public string Bairro { get; set; }

        [JsonProperty(PropertyName = "ibge")]
        public string Ibge { get; set; }

        [JsonProperty(PropertyName = "ddd")]
        public string Ddd { get; set; }
    }
}
