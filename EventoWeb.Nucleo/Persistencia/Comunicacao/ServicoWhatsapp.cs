using EventoWeb.Nucleo.Aplicacao.Comunicacao;
using EventoWeb.Nucleo.Negocio.Excecoes;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Comunicacao
{
    public class ServicoWhatsapp : AServicoWhatsapp
    {
        public override async void Enviar(string destinatario, string mensagem)
        {
            if (Configuracao == null)
                throw new ExcecaoNegocio(nameof(ServicoWhatsapp), "Configuração de whatsapp precisa ser informada.");

            using var clienteHttp = new HttpClient() { BaseAddress = new Uri(Configuracao.HostApi) };
            clienteHttp.DefaultRequestHeaders.Accept.Clear();
            clienteHttp.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            clienteHttp.DefaultRequestHeaders.Add("apikey", Configuracao.ChaveApi);

            var dadosEnviar = new
            {
                number = destinatario,
                text = mensagem
            };

            var dadosEnviarJson = JsonConvert.SerializeObject(dadosEnviar);
            using var conteudoRequisicao = new StringContent(dadosEnviarJson, Encoding.UTF8, "application/json");

            using var response = await clienteHttp.PostAsync($"message/sendText/{Configuracao.Instancia}", conteudoRequisicao);
            response.EnsureSuccessStatusCode();
        }
    }
}
