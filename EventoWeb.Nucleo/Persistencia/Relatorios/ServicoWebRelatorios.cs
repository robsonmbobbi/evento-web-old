using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Relatorios
{
    public class ServicoWebRelatorios
    {

        public byte[] SolicitarRelatorio<TDadosRelatorio>(TDadosRelatorio dadosRelatorio, string chamadaWs)
        {
            HttpClient clienteHttp = new HttpClient();
            clienteHttp.BaseAddress = new Uri("http://localhost:8989/api/relatorios/");

            StringContent dados = new StringContent(JsonConvert.SerializeObject(dadosRelatorio), Encoding.UTF8, "application/json");

            var retorno = clienteHttp.PutAsync(chamadaWs, dados).Result;

            if (retorno.StatusCode == System.Net.HttpStatusCode.OK)
                return retorno.Content.ReadAsByteArrayAsync().Result;
            else
            {
                var mensagem = retorno.Content.ReadAsStringAsync().Result;
                throw new Exception("Ocorreu um problema no serviço de relatórios.", new Exception(mensagem));
            }
        }

    }
}
