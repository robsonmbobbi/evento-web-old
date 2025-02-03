using EventoWeb.Nucleo.Negocio.Entidades;
using EventoWeb.Nucleo.Negocio.Excecoes;

namespace EventoWeb.Nucleo.Aplicacao.Comunicacao
{
    public abstract class AServicoWhatsapp
    {
        private ConfiguracaoWhatsapp m_Configuracao;

        public ConfiguracaoWhatsapp Configuracao
        {
            get => m_Configuracao;
            set
            {
                m_Configuracao = value ?? throw new ExcecaoNegocio(nameof(AServicoWhatsapp), "Configuração de whatsapp precisa ser informada.");
            }
        }

        public abstract void Enviar(string destinatario, string mensagem);
    }
}
