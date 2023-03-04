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
    public class ContatoManager : PluginCore
    {
        public override void ExecutePlugin(IServiceProvider serviceProvider)
        {
            Entity conta = (Entity)this.Context.InputParameters["Target"];
            string cpf = (string)conta.Attributes["dyn1_cpf"];

            ContatoController contatoController = new ContatoController(this.Service);

            if (contatoController.GetContactByCpf(cpf) != null)
            {
                throw new InvalidPluginExecutionException("CPF Já existente");
            }
        }
    }
}
