using EventoWeb.Nucleo.Aplicacao.Comunicacao;
using EventoWeb.Nucleo.Negocio.Excecoes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EventoWeb.Nucleo.Persistencia.Comunicacao
{
    public class ServicoEmail : AServicoEmail
    {
        public override async Task Enviar(Email email)
        {
            if (Configuracao == null)
                throw new ExcecaoNegocio("ServicoEmail", "Configuração de email precisa ser informada.");

            using var clientHttp = new HttpClient() { BaseAddress = new Uri("https://api.sendinblue.com") };
            clientHttp.DefaultRequestHeaders.Accept.Clear();
            clientHttp.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            clientHttp.DefaultRequestHeaders.Add("api-key", Configuracao.SenhaEmail);
            await clientHttp.PostAsync("v3/smtp/email",
                new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        sender = new
                        {
                            name = Configuracao.EnderecoEmail,
                            email = Configuracao.EnderecoEmail
                        },
                        to = new[]
                        {
                            new
                            {
                                name=email.Endereco,
                                email=email.Endereco
                            }
                        },
                        subject = email.Assunto,
                        htmlContent = email.Conteudo,
                        attachment = GerarAnexos(email.Anexos)
                    }),
                    Encoding.UTF8, "application/json")).ConfigureAwait(false);
        }

        private object GerarAnexos(List<AnexoEmail> anexos)
        {
            if (anexos == null || anexos.Count == 0)
                return null;

            return anexos.Select(x =>
            {
                if (x.Tipo == EnumTipoAnexoEmail.URL)
                    return (object)new
                    {
                        url = x.Url
                    };
                else
                    return (object)new
                    {
                        content = x.ArquivoBase64,
                        name = x.NomeArquivo
                    };
            }).ToList();            
        }

        /*public override void Enviar(Email email)
        {
            if (Configuracao == null)
                throw new ExcecaoNegocio("ServicoEmail", "Configuração de email precisa ser informada.");

            using (SmtpClient clienteSmtp = new SmtpClient(Configuracao.ServidorEmail, Configuracao.PortaServidor.Value))
            {
                clienteSmtp.EnableSsl = Configuracao.TipoSeguranca.Value == TipoSegurancaEmail.SSL;
                clienteSmtp.Credentials = new NetworkCredential(Configuracao.UsuarioEmail, Configuracao.SenhaEmail);

                MailMessage mensagemEmail = new MailMessage(new MailAddress(Configuracao.EnderecoEmail), new MailAddress(email.Endereco))
                {
                    Body = email.Conteudo,
                    IsBodyHtml = true,
                    Subject = email.Assunto,
                };
                mensagemEmail.ReplyToList.Add(new MailAddress(Configuracao.EnderecoEmail));

                clienteSmtp.Send(mensagemEmail);
            }
        }*/
    }
}
