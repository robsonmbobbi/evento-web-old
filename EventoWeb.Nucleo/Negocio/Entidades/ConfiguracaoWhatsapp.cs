using EventoWeb.Nucleo.Negocio.Excecoes;
using System;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class ConfiguracaoWhatsapp : Entidade
    {
        private string m_Instancia;
        private string m_HostApi;
        private Evento m_Evento;
        private string m_ChaveApi;

        public ConfiguracaoWhatsapp(Evento evento, string instancia, string hostApi, string chaveApi)
        {
            m_Evento = evento ?? throw new ExcecaoNegocioAtributo("ConfiguracaoEmail", "evento", "Evento não informado.");
            Instancia = instancia;
            HostApi = hostApi;
            ChaveApi = chaveApi;
        }

        protected ConfiguracaoWhatsapp() { }

        public virtual Evento Evento { get => m_Evento; }

        public virtual string Instancia 
        {
            get => m_Instancia;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    throw new Exception($"{nameof(Instancia)} não poder nula ou vazia.");

                m_Instancia = value;
            }
        }
        
        public virtual string HostApi 
        {
            get => m_HostApi;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    throw new Exception($"{nameof(HostApi)} não poder nula ou vazia.");

                m_HostApi = value;
            }
        }

        public virtual string ChaveApi
        {
            get => m_ChaveApi;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    throw new Exception($"{nameof(ChaveApi)} não poder nula ou vazia.");

                m_ChaveApi = value;
            }
        }
    }
}
