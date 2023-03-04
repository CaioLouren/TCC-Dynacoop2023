using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Dynacoop2023.Environment1.ConctionFactory;
using TCC.Dynacoop2023.MyPlugins.TccISV;

namespace TCC.Dynacoop2023.MyPlugins.Plugins
{
    public class Ambiente1 : PluginCore
    {
        public override void ExecutePlugin(IServiceProvider serviceProvider)
        {
            Entity prod = (Entity)this.Context.InputParameters["Target"];
            prod.Attributes["tcc2_integracao"] = true;
            IOrganizationService organizationService = ConectionFactory.organizationService();
            organizationService.Create(prod);
        }
    }
}
