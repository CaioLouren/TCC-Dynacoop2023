using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TCC.Dynacoop2023.SharedProject.VO
{
    public class ContaVO
    {
        [JsonProperty("Cep")]
        public string Cep { get; set; }

        [JsonProperty("DadosCep")]
        public string DadosCep { get; set; }
    }
}
