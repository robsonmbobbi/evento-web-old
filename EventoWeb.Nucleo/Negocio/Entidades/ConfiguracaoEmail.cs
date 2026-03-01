using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Net.Mail;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum TipoSegurancaEmail { SSL, Nenhuma }

    public class ConfiguracaoEmail: Entidade
    {
        private String m_EnderecoEmail;
        private String m_UsuarioEmail;
        private String m_SenhaEmail;
        private String m_ServidorEmail;
        private int? m_PortaServidor;
        private TipoSegurancaEmail? m_TipoSeguranca;
        private Evento m_Evento;

        public ConfiguracaoEmail(Evento evento)
        {
            if (evento == null)
                throw new ExcecaoNegocioAtributo("ConfiguracaoEmail", "evento", "Evento não informado.");

            m_Evento = evento;
        }

        protected ConfiguracaoEmail() { }

        public virtual Evento Evento { get => m_Evento; }

        public virtual String EnderecoEmail
        {
            get { return m_EnderecoEmail; }
            set 
            {
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        new MailAddress(value);
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException("O endereço de email informado não é válido.", "EnderecoEmail");
                    }
                }
                m_EnderecoEmail = value; 
            }
        }

        public virtual String UsuarioEmail
        {
            get { return m_UsuarioEmail; }
            set { m_UsuarioEmail = value; }
        }

        public virtual String SenhaEmail
        {
            get { return m_SenhaEmail; }
            set { m_SenhaEmail = value; }
        }

        public virtual String ServidorEmail
        {
            get { return m_ServidorEmail; }
            set { m_ServidorEmail = value; }
        }

        public virtual int? PortaServidor
        {
            get { return m_PortaServidor; }
            set { m_PortaServidor = value; }
        }

        public virtual TipoSegurancaEmail? TipoSeguranca
        {
            get { return m_TipoSeguranca; }
            set { m_TipoSeguranca = value; }
        }        

        public virtual Boolean ConfiguracaoInformada
        {
            get
            {
                return !String.IsNullOrEmpty(m_EnderecoEmail) &&
                       !String.IsNullOrEmpty(m_SenhaEmail) &&
                       !String.IsNullOrEmpty(m_ServidorEmail) &&
                       !String.IsNullOrEmpty(m_UsuarioEmail) &&
                       m_PortaServidor != null &&
                       m_TipoSeguranca != null;
            }
        }
    }

    public class MensagemEmailPadrao : Entidade
    {
        private Evento m_Evento;
        private ModeloMensagem m_MensagemInscricaoRegistradaAdulto;
        private ModeloMensagem m_MensagemInscricaoConfirmada;
        private ModeloMensagem m_MensagemInscricaoCodigoAcessoAcompanhamento;
        private ModeloMensagem m_MensagemInscricaoCodigoAcessoCriacao;
        private ModeloMensagem m_MensagemInscricaoRegistradaInfantil;

        public MensagemEmailPadrao(Evento evento)
        {
            if (evento == null)
                throw new ExcecaoNegocioAtributo("MensagemEmailPadrao", "evento", "Evento não informado.");

            m_Evento = evento;
        }

        protected MensagemEmailPadrao() { }

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

    public class ModeloMensagem
    {
        private string m_Mensagem;
        private string m_Assunto;

        public ModeloMensagem(string assunto, string mensagem)
        {
            if (string.IsNullOrWhiteSpace(assunto))
                throw new Excecoes.ExcecaoNegocioAtributo("ModeloMensagem", "assunto", "O assunto deve ser informado.");

            if (string.IsNullOrWhiteSpace(mensagem))
                throw new Excecoes.ExcecaoNegocioAtributo("ModeloMensagem", "mensagem", "A mensagem deve ser informada.");

            m_Assunto = assunto;
            m_Mensagem = mensagem;
        }

        protected ModeloMensagem() { }

        public virtual string Assunto { get => m_Assunto; }
        public virtual string Mensagem { get => m_Mensagem; }
    }
}
