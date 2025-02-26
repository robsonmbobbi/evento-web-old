using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Excecoes;

namespace EventoWeb.Nucleo.Aplicacao.Comunicacao
{
    public class Email
    {
        public string Endereco { get; set; }
        public string Assunto { get; set; }
        public string Conteudo { get; set; }
        public List<AnexoEmail> Anexos { get; set; }
    }

    public enum EnumTipoAnexoEmail { URL, Arquivo }
    public class AnexoEmail
    {
        public AnexoEmail(string url)
        {
            Url = url;
            Tipo = EnumTipoAnexoEmail.URL;
        }

        public AnexoEmail(string nomeArquivo, string arquivoBase64)
        {
            NomeArquivo = nomeArquivo;
            ArquivoBase64 = arquivoBase64;
            Tipo = EnumTipoAnexoEmail.Arquivo;
        }

        public string Url { get; }
        public string NomeArquivo { get; }
        public string ArquivoBase64 { get; }
        public EnumTipoAnexoEmail Tipo { get; }
    }

    public abstract class AServicoEmail 
    {
        private ConfiguracaoEmail m_Configuracao;

        public ConfiguracaoEmail Configuracao 
        {
            get => m_Configuracao;
            set
            {
                m_Configuracao = value ?? throw new ExcecaoNegocio("AServicoEmail", "Configuração de email precisa ser informada.");
            }
        }

        public abstract Task Enviar(Email email);
    }
}
