using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TCC.Dynacoop2023.MyPlugins.TccISV;
using TCC.Dynacoop2023.SharedProject.Controller;

namespace TCC.Dynacoop2023.MyPlugins.Plugins
{
    public class OportunidadeManager : PluginCore
    {
        public override void ExecutePlugin(IServiceProvider serviceProvider)
        {
            Entity oportunidade = (Entity)this.Context.InputParameters["Target"];
            OportunidadeController oportunidadeController = new OportunidadeController(this.Service);
            string IdUnico = oportunidadeController.GeraIdUnico();
            if (oportunidadeController.GetOpportunityByIdUnico(IdUnico) != null)
            {
                 IdUnico = oportunidadeController.GeraIdUnico();
            }
            oportunidade.Attributes["dyn1_idunico"] = IdUnico;
        }
    }
}
