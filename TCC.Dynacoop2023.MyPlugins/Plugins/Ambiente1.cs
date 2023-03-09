using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Dynacoop2023.Environment1.ConctionFactory;
using TCC.Dynacoop2023.MyPlugins.TccISV;
using TCC.Dynacoop2023.SharedProject.Controller;

namespace TCC.Dynacoop2023.MyPlugins.Plugins
{
    public class Ambiente1 : PluginCore
    {
        public override void ExecutePlugin(IServiceProvider serviceProvider)
        {
            Entity prod = (Entity)this.Context.InputParameters["Target"];
            prod.Attributes["tcc2_integracao"] = true;
            ProdutoController produtoController = new ProdutoController(this.Service);

            produtoController.CriaProdAmb2(prod);
        }
    }
}
