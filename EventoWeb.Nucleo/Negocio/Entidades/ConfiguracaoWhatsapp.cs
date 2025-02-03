using EventoWeb.Nucleo.Negocio.Excecoes;
using System;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class ConfiguracaoWhatsapp : Entidade
    {
        private string m_Instancia;
        private string m_HostRabbitMQ;
        private Evento m_Evento;

        public ConfiguracaoWhatsapp(Evento evento, string instancia, string hostRabbitMQ)
        {
            if (evento == null)
                throw new ExcecaoNegocioAtributo("ConfiguracaoEmail", "evento", "Evento não informado.");

            m_Evento = evento;

            Instancia = instancia;
            HostRabbitMQ = hostRabbitMQ;
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
        
        public virtual string HostRabbitMQ 
        {
            get => m_HostRabbitMQ;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    throw new Exception($"{nameof(HostRabbitMQ)} não poder nula ou vazia.");

                m_HostRabbitMQ = value;
            }
        }
    }
}
