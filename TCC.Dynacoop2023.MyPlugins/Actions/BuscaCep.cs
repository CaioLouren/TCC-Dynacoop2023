using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace TCC.Dynacoop2023.MyPlugins.Actions
{
    public class BuscaCep
    {
        public static async Task<string> ViaCep(string cep)
        {
            try
            {
                HttpClient client = new HttpClient();
                var resultado = await client.GetStringAsync($"https://viacep.com.br/ws/{cep}/json/");
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
