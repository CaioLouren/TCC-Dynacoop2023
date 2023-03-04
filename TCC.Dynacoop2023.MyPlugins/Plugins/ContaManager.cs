using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Dynacoop2023.MyPlugins.TccISV;
using TCC.Dynacoop2023.SharedProject.Controller;

namespace TCC.Dynacoop2023.MyPlugins.Plugins
{
    public class ContaManager : PluginCore
    {
        public override void ExecutePlugin(IServiceProvider serviceProvider)
        {
            Entity conta = (Entity)this.Context.InputParameters["Target"];
            string cnpj = (string)conta.Attributes["dyn1_cnpj"];

            ContaController contaController = new ContaController(this.Service);

            if(contaController.GetAccountByCnpj(cnpj) != null)
            {
                throw new InvalidPluginExecutionException("CNPJ Já existente");
            }
        }
    }
}
