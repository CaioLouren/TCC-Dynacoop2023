using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Dynacoop2023.MyPlugins.TccISV;

namespace TCC.Dynacoop2023.Environment2.Plugins2
{
    public class Environment2 : PluginCore
    {
        public override void ExecutePlugin(IServiceProvider serviceProvider)
        {
            Entity prod = (Entity)this.Context.InputParameters["Target"];

            if (!(bool)prod.Attributes["tcc2_integracao"])
            {
                throw new InvalidPluginExecutionException("Não pode criar no ambiente 2");
            }
        }
    }
}
