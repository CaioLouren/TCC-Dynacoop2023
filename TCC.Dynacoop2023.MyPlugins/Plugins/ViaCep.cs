using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Dynacoop2023.MyPlugins.TccISV;

namespace TCC.Dynacoop2023.MyPlugins.Actions
{
    public class ViaCep : PluginCore
    {
        public override void ExecutePlugin(IServiceProvider serviceProvider)
        {
            var cep = this.Context.InputParameters["Cep"].ToString();
            var dadosCep = BuscaCep.ViaCep(cep).Result;
            this.Context.OutputParameters["DadosCep"] = dadosCep;
        }
    }
}
