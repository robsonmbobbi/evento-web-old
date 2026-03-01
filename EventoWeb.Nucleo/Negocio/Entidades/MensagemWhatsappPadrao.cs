using EventoWeb.Nucleo.Negocio.Excecoes;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public class MensagemWhatsappPadrao : Entidade
    {
        private Evento m_Evento;
        private ModeloMensagem m_MensagemInscricaoRegistradaAdulto;
        private ModeloMensagem m_MensagemInscricaoConfirmada;
        private ModeloMensagem m_MensagemInscricaoCodigoAcessoAcompanhamento;
        private ModeloMensagem m_MensagemInscricaoCodigoAcessoCriacao;
        private ModeloMensagem m_MensagemInscricaoRegistradaInfantil;

        public MensagemWhatsappPadrao(Evento evento)
        {
            if (evento == null)
                throw new ExcecaoNegocioAtributo($"{nameof(MensagemWhatsappPadrao)}", "evento", "Evento não informado.");

            m_Evento = evento;
        }

        protected MensagemWhatsappPadrao() { }

        public virtual Evento Evento { get => m_Evento; }

        public virtual ModeloMensagem MensagemInscricaoRegistradaAdulto
        {
            get { return m_MensagemInscricaoRegistradaAdulto; }
            set { m_MensagemInscricaoRegistradaAdulto = value; }
        }

        public virtual ModeloMensagem MensagemInscricaoRegistradaInfantil
        {
            get { return m_MensagemInscricaoRegistradaInfantil; }
            set { m_MensagemInscricaoRegistradaInfantil = value; }
        }

        public virtual ModeloMensagem MensagemInscricaoConfirmada
        {
            get { return m_MensagemInscricaoConfirmada; }
            set { m_MensagemInscricaoConfirmada = value; }
        }
        public virtual ModeloMensagem MensagemInscricaoCodigoAcessoAcompanhamento
        {
            get { return m_MensagemInscricaoCodigoAcessoAcompanhamento; }
            set { m_MensagemInscricaoCodigoAcessoAcompanhamento = value; }
        }
        public virtual ModeloMensagem MensagemInscricaoCodigoAcessoCriacao
        {
            get { return m_MensagemInscricaoCodigoAcessoCriacao; }
            set { m_MensagemInscricaoCodigoAcessoCriacao = value; }
        }
    }
}
