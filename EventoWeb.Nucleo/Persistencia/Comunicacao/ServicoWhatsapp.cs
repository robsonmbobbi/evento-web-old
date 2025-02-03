using EventoWeb.Nucleo.Aplicacao.Comunicacao;
using EventoWeb.Nucleo.Negocio.Excecoes;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace EventoWeb.Nucleo.Persistencia.Comunicacao
{
    public class ServicoWhatsapp : AServicoWhatsapp
    {
        public override async void Enviar(string destinatario, string mensagem)
        {
            if (Configuracao == null)
                throw new ExcecaoNegocio(nameof(ServicoWhatsapp), "Configuração de whatsapp precisa ser informada.");

            var factory = new ConnectionFactory { HostName = Configuracao.HostRabbitMQ };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "evolution-send", durable: false, exclusive: false, autoDelete: false,
                arguments: null);

            var jsonMessage = new 
            {
                instance = Configuracao.Instancia,
                number = destinatario,
                text = mensagem
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jsonMessage));

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: string.Empty, body: body);
        }        
    }
}
